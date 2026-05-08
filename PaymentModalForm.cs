using Npgsql;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class PaymentModalForm : Form
    {
        private decimal totalAmount;
        private TextBox? _activeMoneyBox;
        private FlowLayoutPanel? _quickAmountPanel;
        private FlowLayoutPanel? _quickAmountBottomPanel;
        private TableLayoutPanel? _keypadTable;
        private Panel? _afterPayPanel;
        private string _receiptText = "";
        private bool _isProcessing;
        private bool _isPaid;

        public sealed class PaymentProcessResult
        {
            public bool IsSuccess { get; init; }
            public string Message { get; init; } = "";
            public string ReceiptText { get; init; } = "";
        }

        public Func<PaymentProcessResult>? ProcessPaymentHandler { get; set; }
        public bool IsPaid => _isPaid;
        public PaymentModalForm(decimal totalAmount, string? customerName = null)
        {
            InitializeComponent();
            this.totalAmount = totalAmount;
            lblTotal.Text = "Rp " + totalAmount.ToString("N0");
            if (lblCustomerValue != null)
                lblCustomerValue.Text = string.IsNullOrWhiteSpace(customerName) ? "Umum" : customerName;
            UpdatePayButtonState();

            //txtCashback.Text = "0";
            this.Load += (_, __) =>
            {
                SafeWireNumericFormatters();
                InitializeKeypadUi();
                InitializeQuickAmountBottomUi();
                if (_quickAmountBottomPanel != null)
                    LoadQuickAmountButtonsFromDatabase(_quickAmountBottomPanel);
                ConfigureKeyboardFirstUx();
            };
        }

        public void SetTotalAmount(decimal newTotalAmount)
        {
            totalAmount = newTotalAmount < 0 ? 0 : newTotalAmount;
            if (lblTotal != null)
                lblTotal.Text = "Rp " + totalAmount.ToString("N0");
            UpdatePayButtonState();
        }

        // ================================
        // ✅ EVENTS untuk komunikasi ke form utama
        // ================================
        public event Action<decimal> GlobalDiscountChanged;
        public event Action<decimal> GlobalDiscountAmountChanged;
        public event Action<decimal> PaymentAmountChanged;
        public event Action<string> GlobalNoteChanged;         // ✅ NEW
        public event Action<decimal> DeliveryAmountChanged;    // ✅ NEW

        // ================================
        // PROPERTIES
        // ================================
        public bool IsSplitPayment
        {
            get
            {
                return PaymentMethod == "Split Bill" || PaymentMethod == "Split Payment";
            }
        }
        public IEnumerable<(string Method, decimal Amount)>? SplitPayments { get; set; }

        public decimal PaymentAmount
        {
            get
            {
                string input = txtPaymentAmount.Text
                    .Replace("Rp", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Trim();

                if (decimal.TryParse(input, out decimal result))
                    return result;

                return 0;
            }
        }

        //public decimal Cashback
        //{
        //    get
        //    {
        //        string cleanText = txtCashback.Text.Replace("Rp. ", "").Replace(",", "");
        //        if (decimal.TryParse(cleanText, out decimal cashback))
        //            return cashback;
        //        return 0;
        //    }
        //}

        public string PaymentMethod => cmbPaymentMethod.SelectedItem?.ToString() ?? "Cash";

        public decimal GlobalDiscountPercent { get; private set; }
        public decimal GlobalDiscountAmount { get; private set; }
        public decimal GrandTotal { get; private set; }
        public bool GlobalDiscountIsAmount => rdoGlobalAmount != null && rdoGlobalAmount.Checked;

        // ✅ NEW: Properti untuk Global Note dan Delivery Amount
        public string GlobalNote => txtGlobalNote?.Text?.Trim() ?? "";
        public decimal DeliveryAmount
        {
            get
            {
                if (decimal.TryParse(txtDeliveryAmount.Text, out decimal value))
                    return value;
                return 0;
            }
        }

        // ================================
        // EVENTS HANDLER
        // ================================
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;
            if (_isPaid)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            if (cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            if (IsSplitPayment)
            {
                decimal cash = 0, card = 0;
                if (txtCashPart != null && !string.IsNullOrWhiteSpace(txtCashPart.Text))
                    cash = ParseMoney(txtCashPart.Text);
                if (txtCardPart != null && !string.IsNullOrWhiteSpace(txtCardPart.Text))
                    card = ParseMoney(txtCardPart.Text);

                if (cash <= 0 && card <= 0)
                {
                    MessageBox.Show("Isi nominal split payment (Cash/Card).");
                    return;
                }
                var parts = new List<(string Method, decimal Amount)>();
                if (cash > 0) parts.Add(("Cash", cash));
                if (card > 0) parts.Add(("Card", card));
                SplitPayments = parts;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtPaymentAmount.Text))
                {
                    MessageBox.Show("Masukkan nominal pembayaran.");
                    return;
                }
                var _ = PaymentAmount; // trigger parse
            }

            if (ProcessPaymentHandler == null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            try
            {
                if (!ConfirmBeforePay())
                    return;

                _isProcessing = true;
                btnPay.Enabled = false;
                var result = ProcessPaymentHandler.Invoke();
                if (!result.IsSuccess)
                {
                    btnPay.Enabled = true;
                    MessageBox.Show(result.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _isPaid = true;
                _receiptText = result.ReceiptText ?? "";
                ShowAfterPayPanel();
            }
            catch (Exception ex)
            {
                btnPay.Enabled = true;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void ConfigureKeyboardFirstUx()
        {
            this.KeyPreview = true;
            this.AcceptButton = btnPay;
            this.KeyDown -= PaymentModalForm_KeyDown;
            this.KeyDown += PaymentModalForm_KeyDown;

            if (txtPaymentAmount != null)
            {
                txtPaymentAmount.TabIndex = 0;
                txtPaymentAmount.Focus();
                _activeMoneyBox = txtPaymentAmount;
            }
            int nextTab = 1;
            if (_quickAmountBottomPanel != null)
            {
                foreach (Control c in _quickAmountBottomPanel.Controls)
                {
                    if (c is Button b)
                    {
                        b.TabStop = true;
                        b.TabIndex = nextTab++;
                    }
                }
            }

            if (cmbPaymentMethod != null)
                cmbPaymentMethod.TabIndex = Math.Max(nextTab, 20);
            if (btnPay != null)
                btnPay.TabIndex = 30;

            WireFocusHighlight(panelCard);
        }

        private void PaymentModalForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_isPaid)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.F2)
            {
                txtPaymentAmount?.Focus();
                _activeMoneyBox = txtPaymentAmount;
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.F3)
            {
                cmbPaymentMethod?.Focus();
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.F4)
            {
                if (GlobalDiscountIsAmount)
                    txtGlobalDiscountAmount?.Focus();
                else
                    txtGlobalDiscountPercent?.Focus();
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.F5)
            {
                txtDeliveryAmount?.Focus();
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.F6)
            {
                txtGlobalNote?.Focus();
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.F7)
            {
                ApplyExactPayment();
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.F9)
            {
                if (btnPay != null && btnPay.Enabled)
                    btnPay.PerformClick();
                e.Handled = true;
                return;
            }
        }

        private void txtPaymentAmount_TextChanged(object sender, EventArgs e)
        {

            PaymentAmountChanged?.Invoke(PaymentAmount);
            UpdatePayButtonState();
            // optional format on leave to avoid caret jump
        }

        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbPaymentMethod.SelectedItem.ToString();

            panelCardDetails.Visible = false;
            panelEwalletDetails.Visible = false;
            panelBankTransfer.Visible = false;
            panelSplitPayment.Visible = false;

            switch (selected)
            {
                case "Card":
                    panelCardDetails.Visible = true;
                    break;
                case "QRIS":
                    panelEwalletDetails.Visible = true;
                    break;
                case "Bank Transfer":
                    panelBankTransfer.Visible = true;
                    break;
                case "Split Payment":
                case "Split Bill":
                    panelSplitPayment.Visible = true;
                    break;
            }
            UpdatePayButtonState();
        }

        private void txtGlobalDiscountPercent_TextChanged(object sender, EventArgs e)
        {
            if (rdoGlobalAmount != null && rdoGlobalAmount.Checked) return;
            if (!decimal.TryParse(txtGlobalDiscountPercent.Text, out decimal discountPercent))
                discountPercent = 0;

            if (discountPercent > 100)
            {
                discountPercent = 100;
                txtGlobalDiscountPercent.Text = "100";
            }

            GlobalDiscountPercent = discountPercent;

            GlobalDiscountChanged?.Invoke(discountPercent);
        }

        private void txtGlobalDiscountAmount_TextChanged(object sender, EventArgs e)
        {
            if (rdoGlobalPercent != null && rdoGlobalPercent.Checked) return;
            GlobalDiscountAmount = ParseMoney(txtGlobalDiscountAmount.Text);
            GlobalDiscountAmountChanged?.Invoke(GlobalDiscountAmount);
        }


        // ✅ NEW: Global Note change event
        private void txtGlobalNote_TextChanged(object sender, EventArgs e)
        {
            GlobalNoteChanged?.Invoke(GlobalNote);
        }

        // ✅ NEW: Delivery Amount change event
        private void txtDeliveryAmount_TextChanged(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtDeliveryAmount.Text, out decimal delivery))
                delivery = 0;

            DeliveryAmountChanged?.Invoke(delivery);
        }

        private void PaymentModalForm_Load(object sender, EventArgs e)
        {
            // Optional initialization
        }

        private void UpdatePayButtonState()
        {
            decimal paid = PaymentAmount;
            if (IsSplitPayment)
            {
                decimal cash = txtCashPart != null ? ParseMoney(txtCashPart.Text) : 0;
                decimal card = txtCardPart != null ? ParseMoney(txtCardPart.Text) : 0;
                paid = cash + card;
            }
            decimal change = paid - totalAmount;

            bool valid = paid >= totalAmount && cmbPaymentMethod.SelectedItem != null;

            btnPay.Enabled = valid;
            if (lblChangeValue != null)
            {
                var shown = change > 0 ? change : 0m;
                lblChangeValue.Text = "Rp " + shown.ToString("N0");
            }
        }

        private static decimal ParseMoney(string text)
        {
            string clean = text.Replace("Rp", "")
                               .Replace(".", "")
                               .Replace(",", "")
                               .Trim();
            if (decimal.TryParse(clean, out var value)) return value;
            return 0;
        }

        private void SafeWireNumericFormatters()
        {
            if (txtPaymentAmount != null)
            {
                txtPaymentAmount.Leave += (s, e) => txtPaymentAmount.Text = FormatN0(txtPaymentAmount.Text);
                txtPaymentAmount.Enter += (s, e) => _activeMoneyBox = txtPaymentAmount;
            }
            if (txtCashPart != null)
            {
                txtCashPart.Leave += (s, e) => txtCashPart.Text = FormatN0(txtCashPart.Text);
                txtCashPart.TextChanged += (s, e) => UpdatePayButtonState();
                txtCashPart.Enter += (s, e) => _activeMoneyBox = txtCashPart;
            }
            if (txtCardPart != null)
            {
                txtCardPart.Leave += (s, e) => txtCardPart.Text = FormatN0(txtCardPart.Text);
                txtCardPart.TextChanged += (s, e) => UpdatePayButtonState();
                txtCardPart.Enter += (s, e) => _activeMoneyBox = txtCardPart;
            }

            if (txtGlobalDiscountAmount != null)
            {
                txtGlobalDiscountAmount.Leave += (s, e) => txtGlobalDiscountAmount.Text = FormatN0(txtGlobalDiscountAmount.Text);
            }
            if (txtDeliveryAmount != null)
            {
                txtDeliveryAmount.Leave += (s, e) => txtDeliveryAmount.Text = FormatN0(txtDeliveryAmount.Text);
            }

            if (rdoGlobalPercent != null && rdoGlobalAmount != null)
            {
                rdoGlobalPercent.CheckedChanged += (s, e) => ApplyGlobalDiscountMode();
                rdoGlobalAmount.CheckedChanged += (s, e) => ApplyGlobalDiscountMode();
                rdoGlobalPercent.Checked = true;
                ApplyGlobalDiscountMode();
            }
        }

        private readonly Dictionary<Control, Color> _focusOriginalBackColor = new();
        private readonly Dictionary<Button, (Color Back, Color Fore)> _focusOriginalButtonColors = new();

        private void WireFocusHighlight(Control? root)
        {
            if (root == null) return;
            foreach (Control c in root.Controls)
            {
                if (c is TextBox || c is ComboBox)
                {
                    if (!_focusOriginalBackColor.ContainsKey(c))
                        _focusOriginalBackColor[c] = c.BackColor;
                    c.Enter += (_, __) =>
                    {
                        c.BackColor = Color.FromArgb(255, 249, 196);
                        c.ForeColor = Color.Black;
                    };
                    c.Leave += (_, __) =>
                    {
                        if (_focusOriginalBackColor.TryGetValue(c, out var orig))
                            c.BackColor = orig;
                    };
                }

                if (c is Button b)
                {
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.BorderSize = Math.Max(1, b.FlatAppearance.BorderSize);
                    b.Enter += (_, __) =>
                    {
                        if (!_focusOriginalButtonColors.ContainsKey(b))
                            _focusOriginalButtonColors[b] = (b.BackColor, b.ForeColor);

                        b.FlatAppearance.BorderSize = 3;
                        b.FlatAppearance.BorderColor = Color.FromArgb(255, 193, 7);
                        var origBack = _focusOriginalButtonColors[b].Back;
                        b.BackColor = ControlPaint.Light(origBack, 0.35f);
                        b.ForeColor = Color.Black;
                    };
                    b.Leave += (_, __) =>
                    {
                        b.FlatAppearance.BorderSize = 1;
                        b.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
                        if (_focusOriginalButtonColors.TryGetValue(b, out var col))
                        {
                            b.BackColor = col.Back;
                            b.ForeColor = col.Fore;
                        }
                    };
                }

                if (c.Controls.Count > 0)
                    WireFocusHighlight(c);
            }
        }

        private void ApplyGlobalDiscountMode()
        {
            bool isAmount = rdoGlobalAmount != null && rdoGlobalAmount.Checked;
            if (txtGlobalDiscountPercent != null) txtGlobalDiscountPercent.Visible = !isAmount;
            if (txtGlobalDiscountAmount != null) txtGlobalDiscountAmount.Visible = isAmount;
            if (isAmount)
            {
                txtGlobalDiscountAmount_TextChanged(this, EventArgs.Empty);
            }
            else
            {
                txtGlobalDiscountPercent_TextChanged(this, EventArgs.Empty);
            }
        }

        private static string FormatN0(string input)
        {
            var v = ParseMoney(input);
            return v.ToString("N0");
        }

        private void InitializeKeypadUi()
        {
            if (panelKeypad == null) return;

            panelKeypad.Controls.Clear();

            var lbl = new Label
            {
                Text = "Input Cepat",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = false,
                Height = 28,
                Dock = DockStyle.Top
            };
            _quickAmountPanel = null;

            _keypadTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 5
            };
            _keypadTable.ColumnStyles.Clear();
            _keypadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            _keypadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            _keypadTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            _keypadTable.RowStyles.Clear();
            for (int i = 0; i < 5; i++)
                _keypadTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));

            panelKeypad.Controls.Add(_keypadTable);
            panelKeypad.Controls.Add(lbl);

            _activeMoneyBox ??= txtPaymentAmount;

            AddKeypadButton("1", () => AppendDigits("1"), 0, 0);
            AddKeypadButton("2", () => AppendDigits("2"), 1, 0);
            AddKeypadButton("3", () => AppendDigits("3"), 2, 0);
            AddKeypadButton("4", () => AppendDigits("4"), 0, 1);
            AddKeypadButton("5", () => AppendDigits("5"), 1, 1);
            AddKeypadButton("6", () => AppendDigits("6"), 2, 1);
            AddKeypadButton("7", () => AppendDigits("7"), 0, 2);
            AddKeypadButton("8", () => AppendDigits("8"), 1, 2);
            AddKeypadButton("9", () => AppendDigits("9"), 2, 2);
            AddKeypadButton("0", () => AppendDigits("0"), 0, 3);
            AddKeypadButton("00", () => AppendDigits("00"), 1, 3);
            AddKeypadButton("000", () => AppendDigits("000"), 2, 3);
            AddKeypadButton("C", ClearActiveMoney, 0, 4);
            AddKeypadButton("⌫", BackspaceActiveMoney, 1, 4);
            AddKeypadButton("SET", () => { if (txtPaymentAmount != null) _activeMoneyBox = txtPaymentAmount; }, 2, 4);
        }

        private void AddKeypadButton(string text, Action onClick, int col, int row)
        {
            if (_keypadTable == null) return;
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Margin = new Padding(6),
            };
            btn.TabStop = false;
            btn.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += (_, __) => onClick();
            _keypadTable.Controls.Add(btn, col, row);
        }

        private void AppendDigits(string digits)
        {
            var tb = _activeMoneyBox ?? txtPaymentAmount;
            if (tb == null) return;

            var current = new string((tb.Text ?? "").Where(char.IsDigit).ToArray());
            if (current.Length > 18) return;
            var next = (current + digits).TrimStart('0');
            if (string.IsNullOrEmpty(next)) next = "0";
            if (!decimal.TryParse(next, out var value)) return;
            tb.Text = value.ToString("N0");
        }

        private void BackspaceActiveMoney()
        {
            var tb = _activeMoneyBox ?? txtPaymentAmount;
            if (tb == null) return;
            var current = new string((tb.Text ?? "").Where(char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(current)) current = "0";
            if (current.Length <= 1)
            {
                tb.Text = "0";
                return;
            }
            var next = current.Substring(0, current.Length - 1).TrimStart('0');
            if (string.IsNullOrEmpty(next)) next = "0";
            if (!decimal.TryParse(next, out var value)) return;
            tb.Text = value.ToString("N0");
        }

        private void InitializeQuickAmountBottomUi()
        {
            if (panelCard == null) return;
            if (btnPay == null) return;
            if (cmbPaymentMethod == null) return;

            if (_quickAmountBottomPanel != null && panelCard.Controls.Contains(_quickAmountBottomPanel))
                return;

            var gap = 10;
            var h = 52;

            var panel = new FlowLayoutPanel
            {
                Height = h,
                Width = btnPay.Width,
                Location = new Point(btnPay.Left, btnPay.Top),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0),
                Margin = new Padding(0),
                TabStop = false
            };

            panelCard.Controls.Add(panel);
            panel.BringToFront();
            _quickAmountBottomPanel = panel;

            btnPay.Top = panel.Bottom + gap;
        }

        private void ClearActiveMoney()
        {
            var tb = _activeMoneyBox ?? txtPaymentAmount;
            if (tb == null) return;
            tb.Text = "0";
        }

        private void LoadQuickAmountButtonsFromDatabase()
        {
            if (_quickAmountPanel == null) return;
            LoadQuickAmountButtonsFromDatabase(_quickAmountPanel);
        }

        private void LoadQuickAmountButtonsFromDatabase(FlowLayoutPanel targetPanel)
        {
            if (targetPanel == null) return;
            targetPanel.Controls.Clear();
            bool isBottom = ReferenceEquals(targetPanel, _quickAmountBottomPanel);

            var btnExact = new Button
            {
                Text = "Uang Pas",
                Height = 38,
                Width = 110,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Margin = new Padding(6)
            };
            btnExact.TabStop = isBottom;
            btnExact.FlatAppearance.BorderSize = 0;
            btnExact.Click += (_, __) => ApplyExactPayment();
            targetPanel.Controls.Add(btnExact);

            var amounts = new List<decimal>();
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using (var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS payment_shortcut_amounts(
    id SERIAL PRIMARY KEY,
    amount NUMERIC(18,2) NOT NULL,
    sort_order INT NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM payment_shortcut_amounts;", conn))
                {
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        using var seed = new NpgsqlCommand(@"
INSERT INTO payment_shortcut_amounts(amount, sort_order, is_active) VALUES
(20000, 1, TRUE),
(50000, 2, TRUE),
(100000, 3, TRUE),
(200000, 4, TRUE),
(500000, 5, TRUE);
", conn);
                        seed.ExecuteNonQuery();
                    }
                }

                using (var cmd = new NpgsqlCommand(@"
SELECT amount
FROM payment_shortcut_amounts
WHERE is_active = TRUE
ORDER BY sort_order ASC, amount ASC
LIMIT 12;", conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        if (!r.IsDBNull(0))
                            amounts.Add(r.GetDecimal(0));
                    }
                }
            }
            catch
            {
                amounts.AddRange(new[] { 20000m, 50000m, 100000m, 200000m });
            }

            foreach (var a in amounts.Distinct().Take(12))
            {
                var btn = new Button
                {
                    Text = a.ToString("N0"),
                    Height = 38,
                    Width = 110,
                    BackColor = Color.FromArgb(0, 122, 255),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                    Margin = new Padding(6)
                };
                btn.TabStop = isBottom;
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (_, __) =>
                {
                    var tb = _activeMoneyBox ?? txtPaymentAmount;
                    if (tb == null) return;
                    tb.Text = a.ToString("N0");
                };
                targetPanel.Controls.Add(btn);
            }
        }

        private bool ConfirmBeforePay()
        {
            try
            {
                string method = PaymentMethod ?? "";
                if (string.IsNullOrWhiteSpace(method))
                    method = cmbPaymentMethod?.SelectedItem?.ToString() ?? "";

                decimal paid = PaymentAmount;
                string detail = "";
                if (IsSplitPayment)
                {
                    decimal cash = txtCashPart != null ? ParseMoney(txtCashPart.Text) : 0m;
                    decimal card = txtCardPart != null ? ParseMoney(txtCardPart.Text) : 0m;
                    paid = cash + card;
                    detail = $"\nCash: Rp {cash:N0}\nCard: Rp {card:N0}";
                }
                var change = paid - totalAmount;
                if (change < 0m) change = 0m;

                string msg =
                    "Konfirmasi pembayaran:\n\n" +
                    $"Total: Rp {totalAmount:N0}\n" +
                    $"Metode: {method}\n" +
                    $"Dibayar: Rp {paid:N0}\n" +
                    $"Kembalian: Rp {change:N0}" +
                    detail +
                    "\n\nLanjutkan?";

                return MessageBox.Show(
                    msg,
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1
                ) == DialogResult.Yes;
            }
            catch
            {
                return true;
            }
        }

        private void ApplyExactPayment()
        {
            try
            {
                if (IsSplitPayment)
                {
                    if (txtCashPart != null)
                    {
                        txtCashPart.Focus();
                        _activeMoneyBox = txtCashPart;
                        txtCashPart.Text = totalAmount.ToString("N0");
                    }
                    if (txtCardPart != null)
                        txtCardPart.Text = "0";
                    return;
                }

                if (txtPaymentAmount != null)
                {
                    txtPaymentAmount.Focus();
                    _activeMoneyBox = txtPaymentAmount;
                    txtPaymentAmount.Text = totalAmount.ToString("N0");
                }
            }
            catch
            {
            }
        }

        private void ShowAfterPayPanel()
        {
            if (_afterPayPanel == null)
            {
                _afterPayPanel = BuildAfterPayPanel();
                panelCard.Controls.Add(_afterPayPanel);
            }

            _afterPayPanel.Visible = true;
            _afterPayPanel.BringToFront();

            DisablePaymentInputs();
        }

        private void DisablePaymentInputs()
        {
            txtPaymentAmount.Enabled = false;
            cmbPaymentMethod.Enabled = false;
            txtGlobalDiscountPercent.Enabled = false;
            txtGlobalDiscountAmount.Enabled = false;
            txtDeliveryAmount.Enabled = false;
            txtGlobalNote.Enabled = false;
            if (txtCashPart != null) txtCashPart.Enabled = false;
            if (txtCardPart != null) txtCardPart.Enabled = false;
            btnPay.Enabled = false;
        }

        private Panel BuildAfterPayPanel()
        {
            var p = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 246, 250),
                Padding = new Padding(18),
                Visible = false
            };

            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(18)
            };

            var title = new Label
            {
                Text = "Pembayaran Berhasil",
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                AutoSize = false,
                Height = 44,
                Dock = DockStyle.Top
            };

            var desc = new Label
            {
                Text = "Pilih aksi: cetak / simpan / kirim nota.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = false,
                Height = 28,
                Dock = DockStyle.Top
            };

            var btnGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 210,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            btnGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            btnGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            btnGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            btnGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            btnGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));

            var btnPrint = MakeActionButton("Print Nota", Color.FromArgb(0, 122, 255), PrintReceipt);
            var btnSavePng = MakeActionButton("Save PNG", Color.FromArgb(108, 117, 125), SavePng);
            var btnSavePdf = MakeActionButton("Save PDF", Color.FromArgb(108, 117, 125), SavePdf);
            var btnWaText = MakeActionButton("WA (Text)", Color.FromArgb(37, 211, 102), SendToWhatsApp);
            var btnWaPng = MakeActionButton("WA (PNG)", Color.FromArgb(37, 211, 102), SendToWhatsAppPng);
            var btnDone = MakeActionButton("Selesai", Color.FromArgb(40, 167, 69), () =>
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            });

            btnGrid.Controls.Add(btnPrint, 0, 0);
            btnGrid.Controls.Add(btnSavePng, 1, 0);
            btnGrid.Controls.Add(btnSavePdf, 0, 1);
            btnGrid.Controls.Add(btnWaText, 1, 1);
            btnGrid.Controls.Add(btnWaPng, 0, 2);
            btnGrid.Controls.Add(btnDone, 1, 2);

            var emailRow = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52
            };
            var txtEmail = new TextBox
            {
                PlaceholderText = "Email tujuan (contoh: kasir@toko.com)",
                Font = new Font("Segoe UI", 11F),
                Dock = DockStyle.Fill
            };
            var btnSendEmail = new Button
            {
                Text = "Email Text",
                Dock = DockStyle.Right,
                Width = 140,
                BackColor = Color.FromArgb(0, 122, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold)
            };
            btnSendEmail.FlatAppearance.BorderSize = 0;
            btnSendEmail.Click += (_, __) => SendToEmail(txtEmail.Text);

            var btnSendEmailPng = new Button
            {
                Text = "Email PNG",
                Dock = DockStyle.Right,
                Width = 140,
                BackColor = Color.FromArgb(0, 122, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold)
            };
            btnSendEmailPng.FlatAppearance.BorderSize = 0;
            btnSendEmailPng.Click += (_, __) => SendToEmailPng(txtEmail.Text);

            emailRow.Controls.Add(btnSendEmailPng);
            emailRow.Controls.Add(btnSendEmail);
            emailRow.Controls.Add(txtEmail);

            card.Controls.Add(emailRow);
            card.Controls.Add(btnGrid);
            card.Controls.Add(desc);
            card.Controls.Add(title);
            p.Controls.Add(card);
            return p;
        }

        private static Button MakeActionButton(string text, Color color, Action onClick)
        {
            var b = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Margin = new Padding(8)
            };
            b.FlatAppearance.BorderSize = 0;
            b.Click += (_, __) => onClick();
            return b;
        }

        private void PrintReceipt()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var doc = new PrintDocument();
            doc.PrintPage += (_, e) =>
            {
                using var font = new Font("Consolas", 9);
                float y = 10;
                foreach (var line in text.Replace("\r\n", "\n").Split('\n'))
                {
                    e.Graphics.DrawString(line, font, Brushes.Black, new PointF(10, y));
                    y += font.GetHeight(e.Graphics) + 2;
                }
            };
            using var dlg = new PrintDialog { Document = doc, UseEXDialog = true };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                doc.PrinterSettings = dlg.PrinterSettings;
                doc.Print();
            }
        }

        private void SavePng()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG Image (*.png)|*.png",
                FileName = $"nota_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            using var bmp = RenderReceiptTextToBitmap(text);
            bmp.Save(sfd.FileName, ImageFormat.Png);
        }

        private void SavePdf()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var doc = new PrintDocument();
            doc.PrintPage += (_, e) =>
            {
                using var font = new Font("Consolas", 9);
                float y = 10;
                foreach (var line in text.Replace("\r\n", "\n").Split('\n'))
                {
                    e.Graphics.DrawString(line, font, Brushes.Black, new PointF(10, y));
                    y += font.GetHeight(e.Graphics) + 2;
                }
            };
            using var dlg = new PrintDialog { Document = doc, UseEXDialog = true };
            try
            {
                var ps = new PrinterSettings();
                ps.PrinterName = "Microsoft Print to PDF";
                if (ps.IsValid)
                    doc.PrinterSettings = ps;
            }
            catch { }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                doc.PrinterSettings = dlg.PrinterSettings;
                doc.Print();
            }
        }

        private void SendToWhatsApp()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var url = "https://wa.me/?text=" + Uri.EscapeDataString(text);
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void SendToWhatsAppPng()
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string path;
            try
            {
                path = SaveReceiptPngToTemp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var bmp = RenderReceiptTextToBitmap(text);
                using var clone = (Bitmap)bmp.Clone();
                Clipboard.SetImage(clone);
            }
            catch
            {
            }

            try
            {
                var url = "https://wa.me/?text=" + Uri.EscapeDataString("Nota siap. Jika pakai WhatsApp Desktop/Web: paste (Ctrl+V) untuk kirim PNG.");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
            }

            try
            {
                Process.Start(new ProcessStartInfo("explorer.exe", "/select,\"" + path + "\"") { UseShellExecute = true });
            }
            catch
            {
            }
        }

        private void SendToEmail(string email)
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            email = (email ?? "").Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Isi email tujuan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var url = $"mailto:{Uri.EscapeDataString(email)}?subject={Uri.EscapeDataString("Nota Pembayaran")}&body={Uri.EscapeDataString(text)}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void SendToEmailPng(string email)
        {
            var text = _receiptText ?? "";
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Nota kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            email = (email ?? "").Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Isi email tujuan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string path;
            try
            {
                path = SaveReceiptPngToTemp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var outlookType = Type.GetTypeFromProgID("Outlook.Application");
                if (outlookType != null)
                {
                    dynamic outlook = Activator.CreateInstance(outlookType);
                    dynamic mail = outlook.CreateItem(0);
                    mail.To = email;
                    mail.Subject = "Nota Pembayaran";
                    mail.Body = text;
                    mail.Attachments.Add(path);
                    mail.Display();
                    return;
                }
            }
            catch
            {
            }

            try
            {
                var url = $"mailto:{Uri.EscapeDataString(email)}?subject={Uri.EscapeDataString("Nota Pembayaran")}&body={Uri.EscapeDataString("Terlampir nota (PNG). Silakan attach file dari folder yang terbuka.\\n\\n" + text)}";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
            }

            try
            {
                Process.Start(new ProcessStartInfo("explorer.exe", "/select,\"" + path + "\"") { UseShellExecute = true });
            }
            catch
            {
            }
        }

        private string SaveReceiptPngToTemp()
        {
            var text = _receiptText ?? "";
            var dir = Path.Combine(Path.GetTempPath(), "POS-qu");
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, $"nota_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            using var bmp = RenderReceiptTextToBitmap(text);
            bmp.Save(path, ImageFormat.Png);
            return path;
        }

        private static Bitmap RenderReceiptTextToBitmap(string text)
        {
            var lines = text.Replace("\r\n", "\n").Split('\n');
            using var tmp = new Bitmap(1, 1);
            using var g = Graphics.FromImage(tmp);
            using var font = new Font("Consolas", 10);

            float width = 520;
            float height = 20;
            foreach (var line in lines)
            {
                var size = g.MeasureString(line, font);
                if (size.Width + 40 > width) width = size.Width + 40;
                height += size.Height + 4;
            }

            var bmp = new Bitmap((int)Math.Ceiling(width), (int)Math.Ceiling(height));
            using var g2 = Graphics.FromImage(bmp);
            g2.Clear(Color.White);
            g2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            float y = 12;
            foreach (var line in lines)
            {
                g2.DrawString(line, font, Brushes.Black, new PointF(12, y));
                y += font.GetHeight(g2) + 4;
            }
            return bmp;
        }

    }
}
