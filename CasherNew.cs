using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
//using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using POS_qu.Controllers;
using POS_qu.Core;
using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.Repositories;
using POS_qu.Services;
using POS_qu.DTO;
using POS_qu.Core.Interfaces;

namespace POS_qu
{
    public partial class CasherNew : Form
    {
        private readonly Dictionary<Control, Color> _focusOriginalBackColor = new();
        private readonly Dictionary<Button, (Color BackColor, Color ForeColor)> _focusOriginalButtonColors = new();
        private readonly Dictionary<Control, Color> _focusOriginalBorderColor = new();

        private CartActivity cartrepo;
        private IActivityService activityService;
        private ILogger flogger = new FileLogger();
        private ILogger dlogger = new DbLogger();
        private InvoiceData _currentInvoice;
        private CartService _cartService;
        private ITransactionService _transactionService;
        private bool _isLoadingCashierWarehouses;
        public CasherNew()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi; // bukan Font!
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1024, 768);
            this.KeyPreview = true; // Ensure the form can intercept key events
            //this.KeyDown += CasherNew_KeyDown; // Add the key down event handler
        }

        private void CasherNew_Load(object sender, EventArgs e)
        {
            // ===============================
            // 🔹 FORM SETUP
            // ===============================
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;
            //this.KeyDown += CasherNew_KeyDown;
            // this.FormBorderStyle = FormBorderStyle.None; // opsional full kiosk

            // ===============================
            // 🔹 DEPENDENCY & SERVICE INIT (DI)
            // ===============================
            cartrepo = new CartActivity();
            activityService = new ActivityService(flogger, dlogger);
            _cartService = new CartService((CartActivity)cartrepo, activityService);
            repo = new TransactionRepo();
            _transactionService = new TransactionService(repo, activityService);

            // ===============================
            // 🔹 SESSION
            // ===============================
            StartNewTransaction();
            LoadWarehousesForCashier();

            // ===============================
            // 🔹 TIMER (Live Date Time)
            // ===============================
            var timer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };

            timer.Tick += (s, args) =>
            {
                lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            };

            timer.Start();

            // ===============================
            // 🔹 EVENT BINDING
            // ===============================
            txtCariBarang.KeyDown += TxtCariBarang_KeyDown;
            dataGridViewCart4.CellDoubleClick += dataGridViewCart4_CellDoubleClick;
            cmbWarehouse.SelectedIndexChanged += cmbWarehouse_SelectedIndexChanged;
            dataGridViewCart4.KeyDown += DataGridViewCart4_KeyDown;
            btnOpenCashier.Click += btnOpenCashier_Click;
            btnCloseCashier.Click += btnCloseCashier_Click;
            
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
            button4.Click += button4_Click;
            button5.Click += button5_Click;
            button6.Click += button6_Click;
            
            if (btnPendingList != null)
            {
                btnPendingList.Click += btnPendingList_Click;
            }
            if (btnCustomer != null)
            {
                btnCustomer.Click += btnCustomer_Click;
            }
            if (btnCustomTransaction != null)
            {
                btnCustomTransaction.Click += BtnCustomTransaction_Click;
            }
            if (btnPaymentShortcuts != null)
            {
                btnPaymentShortcuts.Click += BtnPaymentShortcuts_Click;
            }

            PromptOpenShiftIfNeeded();
            PromptResumeCartIfAny();
            UpdateShiftInfoUI();

            ConfigureKeyboardFirstUx();
        }

        private void LoadWarehousesForCashier()
        {
            _isLoadingCashierWarehouses = true;
            try
            {
                var dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("name", typeof(string));

                using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    con.Open();
                    using var cmd = new NpgsqlCommand("SELECT id, name FROM warehouses WHERE is_active = TRUE ORDER BY id ASC", con);
                    using var r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        int id = r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0;
                        string name = r["name"]?.ToString() ?? "";
                        if (id > 0)
                            dt.Rows.Add(id, name);
                    }
                }

                cmbWarehouse.DisplayMember = "name";
                cmbWarehouse.ValueMember = "id";
                cmbWarehouse.DataSource = dt;

                int desired = SessionUser.GetCurrentUser()?.WarehouseId ?? 1;
                if (dt.Rows.Count == 1)
                {
                    desired = Convert.ToInt32(dt.Rows[0]["id"]);
                    cmbWarehouse.Enabled = false;
                }

                bool found = false;
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["id"]) == desired)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found && dt.Rows.Count > 0)
                    desired = Convert.ToInt32(dt.Rows[0]["id"]);

                cmbWarehouse.SelectedValue = desired;
                SessionUser.UpdateWarehouseId(desired);
            }
            catch
            {
                cmbWarehouse.Enabled = false;
                SessionUser.UpdateWarehouseId(1);
            }
            finally
            {
                _isLoadingCashierWarehouses = false;
            }
        }

        private void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingCashierWarehouses) return;
            int wid = 0;
            try
            {
                if (cmbWarehouse.SelectedValue != null)
                    wid = Convert.ToInt32(cmbWarehouse.SelectedValue);
            }
            catch
            {
                wid = 0;
            }

            if (wid <= 0) return;

            if (_currentInvoice != null && _currentInvoice.Items != null && _currentInvoice.Items.Count > 0)
            {
                var res = MessageBox.Show(
                    "Ganti gudang akan reset cart yang sedang berjalan. Lanjutkan?",
                    "Ganti Gudang",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (res != DialogResult.Yes)
                {
                    _isLoadingCashierWarehouses = true;
                    try
                    {
                        cmbWarehouse.SelectedValue = SessionUser.GetCurrentUser()?.WarehouseId ?? 1;
                    }
                    finally
                    {
                        _isLoadingCashierWarehouses = false;
                    }
                    return;
                }

                StartNewTransaction();
            }

            SessionUser.UpdateWarehouseId(wid);
        }

        private void ConfigureKeyboardFirstUx()
        {
            if (txtCariBarang != null)
            {
                txtCariBarang.TabStop = true;
                txtCariBarang.TabIndex = 0;
                txtCariBarang.Focus();
            }
            SetupFocusHighlight(this);
            ConfigureCartGridKeyboardUi();
        }

        private void ConfigureCartGridKeyboardUi()
        {
            if (dataGridViewCart4 == null) return;
            dataGridViewCart4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCart4.MultiSelect = false;
            dataGridViewCart4.StandardTab = true;
            dataGridViewCart4.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 255);
            dataGridViewCart4.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridViewCart4.RowHeadersVisible = false;
        }

        private void SetupFocusHighlight(Control root)
        {
            if (root == null) return;
            WireFocusHandlers(root);
        }

        private void WireFocusHandlers(Control c)
        {
            if (c == null) return;

            if (c is TextBox || c is ComboBox || c is NumericUpDown)
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
                if (!_focusOriginalButtonColors.ContainsKey(b))
                    _focusOriginalButtonColors[b] = (b.BackColor, b.ForeColor);

                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = Math.Max(1, b.FlatAppearance.BorderSize);
                b.Enter += (_, __) =>
                {
                    b.FlatAppearance.BorderSize = 3;
                    b.FlatAppearance.BorderColor = Color.FromArgb(255, 193, 7);
                };
                b.Leave += (_, __) =>
                {
                    b.FlatAppearance.BorderSize = 1;
                    b.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
                };
            }

            if (c is DataGridView dgv)
            {
                dgv.Enter += (_, __) =>
                {
                    dgv.BorderStyle = BorderStyle.FixedSingle;
                    dgv.BackgroundColor = Color.White;
                };
                dgv.Leave += (_, __) =>
                {
                    dgv.BorderStyle = BorderStyle.FixedSingle;
                };
            }

            foreach (Control child in c.Controls)
            {
                WireFocusHandlers(child);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.N:
                    if (_currentInvoice != null && _currentInvoice.Items != null && _currentInvoice.Items.Count > 0)
                    {
                        var confirm = MessageBox.Show(
                            "Mulai transaksi baru? Cart saat ini akan dikosongkan.",
                            "Konfirmasi",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );
                        if (confirm != DialogResult.Yes) return true;
                    }
                    StartNewTransaction();
                    RenderInvoice(_currentInvoice);
                    UpdateInvoiceSummary();
                    return true;

                case Keys.Control | Keys.P:
                    button1.PerformClick();
                    return true;

                case Keys.Control | Keys.K:
                    OpenSearchShortcut();
                    return true;

                case Keys.F1:
                    button6.PerformClick();
                    return true;
                    
                case Keys.F4:
                    if (btnCustomer != null) btnCustomer.PerformClick();
                    return true;

                case Keys.Control | Keys.S:
                    txtCariBarang.Focus();
                    return true;

                case Keys.F2:
                    txtCariBarang.Focus();
                    return true;

                case Keys.F8:
                    if (dataGridViewCart4 != null)
                    {
                        dataGridViewCart4.Focus();
                        if (dataGridViewCart4.CurrentCell == null && dataGridViewCart4.Rows.Count > 0)
                            dataGridViewCart4.CurrentCell = dataGridViewCart4.Rows[0].Cells[0];
                    }
                    return true;

                case Keys.Control | Keys.F:
                    OpenSearchShortcut();
                    return true;

                case Keys.Control | Keys.I:
                    if (btnPendingList != null) btnPendingList.PerformClick();
                    return true;

                case Keys.F3:
                    button2.PerformClick();
                    return true;

                case Keys.Control | Keys.D:
                    button4.PerformClick();
                    return true;

                case Keys.Control | Keys.B:
                    button3.PerformClick();
                    return true;

                case Keys.Control | Keys.M:
                    button5.PerformClick();
                    return true;

                case Keys.F12:
                    button6.PerformClick();
                    return true;

                case Keys.Control | Keys.Shift | Keys.O:
                    OpenShift();
                    return true;

                case Keys.Control | Keys.Shift | Keys.C:
                    CloseShift();
                    return true;

                case Keys.Control | Keys.Alt | Keys.P:
                    ShowPaymentShortcutSettingsModal();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnPaymentShortcuts_Click(object? sender, EventArgs e)
        {
            ShowPaymentShortcutSettingsModal();
        }

        private void ShowPaymentShortcutSettingsModal()
        {
            try
            {
                using var modal = new Form
                {
                    Text = "Setting Shortcut Nominal Bayar",
                    StartPosition = FormStartPosition.CenterParent,
                    Size = new Size(760, 560),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    KeyPreview = true
                };

                var header = new Panel { Dock = DockStyle.Top, Height = 64, Padding = new Padding(16), BackColor = Color.White };
                var title = new Label
                {
                    Text = "Shortcut Nominal Pembayaran",
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(51, 51, 51),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                header.Controls.Add(title);

                var grid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    BackgroundColor = Color.White,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    Font = new Font("Segoe UI", 10F)
                };
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 255);
                grid.DefaultCellStyle.SelectionForeColor = Color.White;

                var dt = LoadPaymentShortcutAmounts();
                var bs = new BindingSource { DataSource = dt };
                grid.DataSource = bs;
                if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false;
                if (grid.Columns.Contains("amount")) grid.Columns["amount"].HeaderText = "Nominal";
                if (grid.Columns.Contains("sort_order")) grid.Columns["sort_order"].HeaderText = "Urutan";
                if (grid.Columns.Contains("is_active")) grid.Columns["is_active"].HeaderText = "Aktif";
                if (grid.Columns.Contains("amount")) grid.Columns["amount"].DefaultCellStyle.Format = "N0";

                var footer = new Panel { Dock = DockStyle.Bottom, Height = 70, Padding = new Padding(16), BackColor = Color.White };

                var btnAdd = new Button
                {
                    Text = "Tambah (F2)",
                    Width = 140,
                    Height = 42,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(0, 122, 255),
                    ForeColor = Color.White
                };
                btnAdd.FlatAppearance.BorderSize = 0;

                var btnDeactivate = new Button
                {
                    Text = "Nonaktifkan (Del)",
                    Width = 170,
                    Height = 42,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(220, 53, 69),
                    ForeColor = Color.White
                };
                btnDeactivate.FlatAppearance.BorderSize = 0;

                var btnSave = new Button
                {
                    Text = "Simpan (Ctrl+S)",
                    Width = 170,
                    Height = 42,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(40, 167, 69),
                    ForeColor = Color.White
                };
                btnSave.FlatAppearance.BorderSize = 0;

                var btnClose = new Button
                {
                    Text = "Tutup (Esc)",
                    Width = 140,
                    Height = 42,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White
                };
                btnClose.FlatAppearance.BorderSize = 0;

                btnAdd.Click += (_, __) =>
                {
                    var maxSort = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted) continue;
                        var so = Convert.ToInt32(r["sort_order"]);
                        if (so > maxSort) maxSort = so;
                    }

                    var nr = dt.NewRow();
                    nr["id"] = 0;
                    nr["amount"] = 0m;
                    nr["sort_order"] = maxSort + 1;
                    nr["is_active"] = true;
                    dt.Rows.Add(nr);

                    if (grid.Rows.Count > 0)
                    {
                        grid.CurrentCell = grid.Rows[grid.Rows.Count - 1].Cells[grid.Columns["amount"].Index];
                        grid.BeginEdit(true);
                    }
                };

                btnDeactivate.Click += (_, __) =>
                {
                    if (grid.CurrentRow == null) return;
                    var rowView = grid.CurrentRow.DataBoundItem as DataRowView;
                    if (rowView == null) return;
                    rowView["is_active"] = false;
                };

                btnSave.Click += (_, __) =>
                {
                    grid.EndEdit();
                    bs.EndEdit();
                    SavePaymentShortcutAmounts(dt);
                    MessageBox.Show("Setting shortcut pembayaran tersimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var refreshed = LoadPaymentShortcutAmounts();
                    bs.DataSource = refreshed;
                    grid.DataSource = bs;
                };

                btnClose.Click += (_, __) => modal.Close();

                footer.Controls.Add(btnClose);
                footer.Controls.Add(btnSave);
                footer.Controls.Add(btnDeactivate);
                footer.Controls.Add(btnAdd);

                btnClose.Dock = DockStyle.Right;
                btnSave.Dock = DockStyle.Right;
                btnDeactivate.Dock = DockStyle.Left;
                btnAdd.Dock = DockStyle.Left;

                modal.KeyDown += (_, e) =>
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        modal.Close();
                        e.Handled = true;
                        return;
                    }
                    if (e.KeyCode == Keys.F2)
                    {
                        btnAdd.PerformClick();
                        e.Handled = true;
                        return;
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        btnDeactivate.PerformClick();
                        e.Handled = true;
                        return;
                    }
                    if (e.Control && e.KeyCode == Keys.S)
                    {
                        btnSave.PerformClick();
                        e.Handled = true;
                        return;
                    }
                };

                modal.Controls.Add(grid);
                modal.Controls.Add(footer);
                modal.Controls.Add(header);

                modal.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static DataTable LoadPaymentShortcutAmounts()
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

            using var da = new NpgsqlDataAdapter(@"
SELECT id, amount, sort_order, is_active
FROM payment_shortcut_amounts
ORDER BY sort_order ASC, amount ASC;", conn);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private static void SavePaymentShortcutAmounts(DataTable dt)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var tran = conn.BeginTransaction();

            foreach (DataRow r in dt.Rows)
            {
                if (r.RowState == DataRowState.Deleted) continue;

                var id = Convert.ToInt32(r["id"]);
                var amount = Convert.ToDecimal(r["amount"]);
                var sort = Convert.ToInt32(r["sort_order"]);
                var active = Convert.ToBoolean(r["is_active"]);

                if (amount <= 0m) continue;

                if (id <= 0)
                {
                    using var cmd = new NpgsqlCommand(@"
INSERT INTO payment_shortcut_amounts(amount, sort_order, is_active)
VALUES (@a, @s, @act);", conn, tran);
                    cmd.Parameters.AddWithValue("@a", amount);
                    cmd.Parameters.AddWithValue("@s", sort);
                    cmd.Parameters.AddWithValue("@act", active);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    using var cmd = new NpgsqlCommand(@"
UPDATE payment_shortcut_amounts
SET amount=@a, sort_order=@s, is_active=@act
WHERE id=@id;", conn, tran);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@a", amount);
                    cmd.Parameters.AddWithValue("@s", sort);
                    cmd.Parameters.AddWithValue("@act", active);
                    cmd.ExecuteNonQuery();
                }
            }

            tran.Commit();
        }

        private void OpenSearchShortcut()
        {
            Item selectedItem = null;

            using (var search = new SearchFormItem(""))
            {
                if (search.ShowDialog(this) != DialogResult.OK || search.SelectedItem == null)
                    return;

                selectedItem = search.SelectedItem;
            }

            var item = _cartService.GetItemByName(selectedItem.name);

            if (item == null)
            {
                MessageBox.Show("Barang tidak ditemukan");
                return;
            }

            var itemvariant = _cartService.cekUnitVariant(selectedItem.name);
            if (itemvariant == null)
            {
                MessageBox.Show("Barang tidak ditemukan");
                return;
            }

            var itemm = itemvariant.Item;
            var variants = itemvariant.Variants;

            UnitVariant selectedVariant = null;

            // 🔥 Jika ada lebih dari 1 variant → tampilkan modal
            if (variants != null && variants.Count >= 1)
            {
                selectedVariant = ShowVariantPicker(this, variants);
            }

            try
            {
                if (selectedVariant != null)
                {
                    _currentInvoice = _cartService.AddItemByVariant(
                        _currentInvoice,
                        selectedVariant.Id,
                        1
                    );
                }
                else
                {
                    _currentInvoice = _cartService.AddItemByName(_currentInvoice, selectedItem.name, 1);
                }

                RenderInvoice(_currentInvoice);
                var lastItem = _currentInvoice.Items.Last();
                AddInvoiceItemToPanel(lastItem);
                RenderReceiptUI(
                    _currentInvoice,
                    SessionUser.GetCurrentUser().TerminalName,
                    SessionUser.GetCurrentUser().Username,
                    SessionUser.GetCurrentUser().ShiftId.ToString(),
                    "Unpaid",
                    _currentInvoice.GrandTotal.ToString(),
                    "-",
                    "0"
                );
                UpdateInvoiceSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void ShowShortcutsHelp()
        {
            using (Form f = new Form())
            {
                f.Text = "Daftar Shortcut";
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(420, 360);
                var lb = new ListBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F) };
                lb.Items.Add("Ctrl+K  • Cari barang (Search)");
                lb.Items.Add("Ctrl+N  • Transaksi baru");
                lb.Items.Add("Ctrl+P  • Bayar");
                lb.Items.Add("F3      • Draft");
                lb.Items.Add("Ctrl+D  • Buka Draft");
                lb.Items.Add("F12     • Help");
                lb.Items.Add("Ctrl+Shift+O • Buka Kasir");
                lb.Items.Add("Ctrl+Shift+C • Tutup Kasir");
                f.Controls.Add(lb);
                f.ShowDialog(this);
            }
        }

        private void PromptOpenShiftIfNeeded()
        {
            var session = SessionUser.GetCurrentUser();
            var sc = new POS_qu.Controllers.ShiftController();
            var open = sc.GetOpenShift(session.UserId, session.TerminalId);
            if (open == null)
            {
                using var dlg = new CashierOpenForm();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var shiftId = sc.OpenShift(session.UserId, session.TerminalId, dlg.OpeningCash);
                    flogger.Log(SessionUser.GetCurrentUser().UserId.ToString(), ActivityType.OpenShift.ToString(), null, "shift_open", $"Shift opened #{shiftId} with opening {dlg.OpeningCash}");
                    SessionUser.UpdateShiftId(shiftId);
                }
            }
            else
            {
                SessionUser.UpdateShiftId(Convert.ToInt32(open["id"]));
            }
        }

        private void OpenShift()
        {
            var session = SessionUser.GetCurrentUser();
            var sc = new POS_qu.Controllers.ShiftController();
            var open = sc.GetOpenShift(session.UserId, session.TerminalId);
            if (open != null)
            {
                MessageBox.Show("Shift kasir sudah terbuka.", "Info");
                return;
            }
            using var dlg = new CashierOpenForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var shiftId = sc.OpenShift(session.UserId, session.TerminalId, dlg.OpeningCash);
                flogger.Log(SessionUser.GetCurrentUser().UserId.ToString(), ActivityType.OpenShift.ToString(), null, "Shift_opened", $"Shift opened #{shiftId} with opening {dlg.OpeningCash}");
                SessionUser.UpdateShiftId(shiftId);
                UpdateShiftInfoUI();
            }
        }

        private void CloseShift()
        {
            try
            {
                var session = SessionUser.GetCurrentUser();
                var sc = new POS_qu.Controllers.ShiftController();
                var open = sc.GetOpenShift(session.UserId, session.TerminalId);
                if (open == null)
                {
                    MessageBox.Show("Shift belum dibuka.", "Info");
                    return;
                }

                int shiftId = Convert.ToInt32(open["id"]);
                decimal openingCash = (decimal)open["opening_cash"];

                decimal cashSales = 0m;
                using (var con = new Npgsql.NpgsqlConnection(POS_qu.Helpers.DbConfig.ConnectionString))
                {
                    con.Open();
                    using var cmd = new Npgsql.NpgsqlCommand(@"
SELECT COALESCE(SUM(COALESCE(ts_grand_total,0)),0)
FROM transactions
WHERE deleted_at IS NULL
  AND shift_id = @sid
  AND ts_status = 1
  AND (COALESCE(ts_method,'') = 'Cash' OR COALESCE(ts_method,'') = 'RETURN')
", con);
                    cmd.Parameters.AddWithValue("@sid", shiftId);
                    var obj = cmd.ExecuteScalar();
                    cashSales = obj == null || obj == DBNull.Value ? 0m : Convert.ToDecimal(obj);
                }

                decimal expectedCash = openingCash + cashSales;

                decimal? closingCash = null;
                using (var modal = new Form())
                {
                    modal.Text = "Tutup Shift";
                    modal.Size = new Size(460, 340);
                    modal.StartPosition = FormStartPosition.CenterParent;
                    modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                    modal.MaximizeBox = false;
                    modal.MinimizeBox = false;
                    modal.Padding = new Padding(18);

                    var lblTitle = new Label
                    {
                        Text = $"Shift #{shiftId}",
                        Dock = DockStyle.Top,
                        Height = 34,
                        Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold)
                    };

                    var pnl = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 12, 0, 0) };

                    var lblExpected = new Label
                    {
                        Text = $"Expected Cash: Rp {expectedCash:N0}",
                        Dock = DockStyle.Top,
                        Height = 28,
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(0, 122, 255)
                    };

                    var lblInput = new Label
                    {
                        Text = "Closing Cash (uang fisik di laci):",
                        Dock = DockStyle.Top,
                        Height = 24,
                        Font = new Font("Segoe UI", 10F)
                    };

                    var txt = new TextBox
                    {
                        Dock = DockStyle.Top,
                        Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                        Height = 38,
                        TextAlign = HorizontalAlignment.Right
                    };

                    var spacer = new Panel { Dock = DockStyle.Top, Height = 12 };

                    var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 56 };
                    var btnOk = new Button
                    {
                        Text = "Tutup Shift",
                        Dock = DockStyle.Right,
                        Width = 140,
                        BackColor = Color.FromArgb(220, 53, 69),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnOk.FlatAppearance.BorderSize = 0;

                    var btnCancel = new Button
                    {
                        Text = "Batal",
                        Dock = DockStyle.Right,
                        Width = 90
                    };

                    decimal ParseMoney(string t)
                    {
                        var clean = (t ?? "").Replace("Rp", "").Replace(".", "").Replace(",", "").Trim();
                        if (decimal.TryParse(clean, out var v)) return v;
                        return 0m;
                    }

                    btnOk.Click += (s, e) =>
                    {
                        var v = ParseMoney(txt.Text);
                        if (v <= 0m)
                        {
                            MessageBox.Show("Isi Closing Cash yang valid.");
                            return;
                        }
                        closingCash = v;
                        modal.DialogResult = DialogResult.OK;
                        modal.Close();
                    };
                    btnCancel.Click += (s, e) => modal.Close();

                    pnlButtons.Controls.Add(btnCancel);
                    pnlButtons.Controls.Add(btnOk);

                    pnl.Controls.Add(pnlButtons);
                    pnl.Controls.Add(spacer);
                    pnl.Controls.Add(txt);
                    pnl.Controls.Add(lblInput);
                    pnl.Controls.Add(lblExpected);

                    modal.Controls.Add(pnl);
                    modal.Controls.Add(lblTitle);

                    modal.AcceptButton = btnOk;
                    modal.CancelButton = btnCancel;

                    if (modal.ShowDialog(this) != DialogResult.OK || closingCash == null) return;
                }

                sc.CloseShift(shiftId, expectedCash, closingCash.Value);
                SessionUser.UpdateShiftId(0);
                UpdateShiftInfoUI();
                MessageBox.Show("Shift berhasil ditutup.", "Info");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Gagal Tutup Shift", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenCashier_Click(object sender, EventArgs e) => OpenShift();
        private void btnCloseCashier_Click(object sender, EventArgs e) => CloseShift();
        
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            var customerDto = ShowCustomerPicker();
            if (customerDto != null)
            {
                _currentInvoice.CustomerId = customerDto.Id;
                _currentInvoice.CustomerName = customerDto.Name;
                _currentInvoice.PriceLevelId = customerDto.PriceLevelId ?? 1;
                
                btnCustomer.Text = customerDto.Name;
                
                // Recalculate cart prices based on new customer level
                if (_currentInvoice.Items.Count > 0)
                {
                    _currentInvoice = _cartService.RecalculateCartPrices(_currentInvoice);
                    RenderInvoice(_currentInvoice);
                    RefreshInvoicePanel();
                }
            }
        }

        private void btnPendingList_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Daftar Pending Cart sementara tidak tersedia.", "Info");
        }
        private void BtnCustomTransaction_Click(object? sender, EventArgs e)
        {
            using var dlg = new CustomTransactionForm();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            if (string.IsNullOrEmpty(Session.CartSessionCode))
            {
                StartNewTransaction();
            }
            try
            {
                using var con = new Npgsql.NpgsqlConnection(DbConfig.ConnectionString);
                con.Open();
                using var tran = con.BeginTransaction();
                var repoDraft = new CartActivity();
                var ok = repoDraft.InsertCustomPendingItem(
                    con, tran,
                    Session.CartSessionCode,
                    dlg.ItemName,
                    dlg.Unit,
                    dlg.UnitId,
                    dlg.Qty,
                    dlg.Price,
                    1
                );
                tran.Commit();
                if (!ok)
                {
                    MessageBox.Show("Gagal menambahkan custom item ke pending.");
                    return;
                }
                _currentInvoice = _cartService.LoadInvoiceFromCartSession(Session.CartSessionCode);
                RenderInvoice(_currentInvoice);
                if (_currentInvoice.Items.Any())
                {
                    var lastItem = _currentInvoice.Items.Last();
                    AddInvoiceItemToPanel(lastItem);
                }
                RenderReceiptUI(
                    _currentInvoice,
                    SessionUser.GetCurrentUser().TerminalName,
                    SessionUser.GetCurrentUser().Username,
                    SessionUser.GetCurrentUser().ShiftId.ToString(),
                    "Unpaid",
                    _currentInvoice.GrandTotal.ToString(),
                    "-",
                    "0"
                );
                UpdateInvoiceSummary();
                flogger.Log(SessionUser.GetCurrentUser().UserId.ToString(), "CustomTransaction_Add", null, "custom_to_pending", new { name = dlg.ItemName, qty = dlg.Qty, price = dlg.Price }.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error custom transaction: " + ex.Message);
            }
        }
        private void PromptResumeCartIfAny()
        {
            var session = SessionUser.GetCurrentUser();
            var repo = new CartActivity();
            var dt = repo.GetPendingCartsByCashier(session.UserId);
            if (dt == null || dt.Rows.Count == 0) return;

            if (dt.Rows.Count == 1)
            {
                var code = dt.Rows[0]["cart_session_code"].ToString();
                if (!string.IsNullOrEmpty(Session.CartSessionCode) && Session.CartSessionCode == code) return;
                int items = Convert.ToInt32(dt.Rows[0]["total_items"]);
                decimal total = Convert.ToDecimal(dt.Rows[0]["grand_total"]);
                var result = MessageBox.Show(
                    $"Ada sesi pending: {code}\nItem: {items}\nTotal: {total:N0}\nLanjutkan?",
                    "Resume Cart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (result == DialogResult.Yes)
                {
                    ResumeCartByCode(code);
                }
            }
            else
            {
            }
        }

        private void ResumeCartByCode(string cartCode)
        {
            _currentInvoice = _cartService.LoadInvoiceFromCartSession(cartCode);
            RenderInvoice(_currentInvoice);
            if (_currentInvoice.Items.Any())
            {
                var lastItem = _currentInvoice.Items.Last();
                AddInvoiceItemToPanel(lastItem);
            }
            RenderReceiptUI(
                _currentInvoice,
                SessionUser.GetCurrentUser().TerminalName,
                SessionUser.GetCurrentUser().Username,
                SessionUser.GetCurrentUser().ShiftId.ToString(),
                "Unpaid",
                _currentInvoice.GrandTotal.ToString(),
                "-",
                "0"
            );
            UpdateInvoiceSummary();
        }

        private void UpdateShiftInfoUI()
        {
            try
            {
                var session = SessionUser.GetCurrentUser();
                var sc = new POS_qu.Controllers.ShiftController();
                var open = sc.GetOpenShift(session.UserId, session.TerminalId);
                if (open != null)
                {
                    var openedAt = (DateTime)open["opened_at"];
                    var openingCash = (decimal)open["opening_cash"];
                    lblShiftInfo.Text = $"Shift aktif: #{open["id"]} • {session.Username} • {session.TerminalName} • {openedAt:dd/MM/yyyy HH:mm} • Saldo Awal: Rp {openingCash:N0}";
                    lblShiftInfo.ForeColor = Color.DarkGreen;
                }
                else
                {
                    lblShiftInfo.Text = "Shift: tidak aktif";
                    lblShiftInfo.ForeColor = Color.Firebrick;
                }
            }
            catch
            {
                lblShiftInfo.Text = "Shift: status tidak diketahui";
                lblShiftInfo.ForeColor = Color.DarkOrange;
            }
        }
        private void StartNewTransaction()
        {
            Session.StartNewCart();

            _currentInvoice = new InvoiceData
            {
                Items = new List<InvoiceItem>(),
                CartSessionCode = Session.CartSessionCode,
                GlobalDiscountPercent = 0,
                DeliveryAmount = 0,
                GlobalNote = "",
                Status = "Draft",
                PriceLevelId = 1
            };

            if (btnCustomer != null) btnCustomer.Text = "Pelanggan (F4)";

            lblSessionCode.Text = _currentInvoice.CartSessionCode;
        }






        private void Timer_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }


        private UnitVariant ShowVariantPicker(IWin32Window owner,List<UnitVariant> variants)
        {
            UnitVariant result = null; // hasil default null

            using (Form form = new Form())
            {
                form.Text = "Pilih Satuan";
                form.Width = 400;   // lebih lebar
                form.Height = 350;  // lebih tinggi
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                // =============================
                // LISTBOX VARIANTS
                // =============================
                var listBox = new ListBox()
                {
                    Dock = DockStyle.Top,
                    Height = 220
                };
                foreach (var v in variants)
                    listBox.Items.Add(v);

                listBox.DisplayMember = "UnitName";


                int btnHeight = 40; // tinggi tombol yang nyaman
                int btnSpacing = 10;
                var panelButtons = new Panel()
                {
                    Dock = DockStyle.Bottom,
                    Height = 60,
                    Padding = new Padding(10)
                };

                var btnOk = new Button()
                {
                    Text = "Pilih",
                    Width = 100,
                    Height = btnHeight,
                    Left = 0,
                    Top = 5
                };
                btnOk.Click += (s, e) =>
                {
                    if (listBox.SelectedItem != null)
                        result = listBox.SelectedItem as UnitVariant;
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                var btnDefault = new Button()
                {
                    Text = "Base Unit",
                    Width = 120,
                    Left = 120,
                    Height = btnHeight,
                    Top = 5
                };
                btnDefault.Click += (s, e) =>
                {
                    result = null; // pakai default
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                var btnCancel = new Button()
                {
                    Text = "Batal",
                    Width = 100,
                    Left = 250,
                    Height = btnHeight,
                    Top = 5
                };
                btnCancel.Click += (s, e) =>
                {
                    result = null;
                    form.DialogResult = DialogResult.Cancel;
                    form.Close();
                };

                panelButtons.Controls.Add(btnOk);
                panelButtons.Controls.Add(btnDefault);
                panelButtons.Controls.Add(btnCancel);

                // =============================
                // TAMBAHKAN KE FORM
                // =============================
                form.Controls.Add(listBox);
                form.Controls.Add(panelButtons);

                form.ShowDialog(owner);
            }

            return result; // null kalau klik Satuan Normal atau Batal
        }



        private void TxtCariBarang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            string keyword = txtCariBarang.Text.Trim();
            if (string.IsNullOrEmpty(keyword)) return;

            try
            {

                var item = _cartService.GetItemByName(keyword);

                if (item == null)
                {
                    MessageBox.Show("Barang tidak ditemukan");
                    return;
                }

                var itemvariant = _cartService.cekUnitVariant(keyword);
                if (itemvariant == null)
                {
                    MessageBox.Show("Barang tidak ditemukan");
                    return;
                }

                var itemm = itemvariant.Item;
                var variants = itemvariant.Variants;

                UnitVariant selectedVariant = null;

                // 🔥 Jika ada lebih dari 1 variant → tampilkan modal
                if (variants != null && variants.Count >= 1)
                {
                    selectedVariant = ShowVariantPicker(this,   variants);
                }


                if (selectedVariant != null)
                {
                    // User pilih variant
                    _currentInvoice = _cartService.AddItemByVariant(
                        _currentInvoice,
                        selectedVariant.Id,
                        1
                    );
                }
                else
                {
                    // Tidak ada variant / user batal → pakai base item
                    _currentInvoice = _cartService.AddItemByName(_currentInvoice, keyword, 1);
                }

                //var variants = _cartService.GetVariants(item.Id);



                RenderInvoice(_currentInvoice);

                // Tambahkan item terakhir ke FlowLayoutPanel
                var lastItem = _currentInvoice.Items.Last();
                AddInvoiceItemToPanel(lastItem);

                RenderReceiptUI(
                    _currentInvoice,      // invoice
                    SessionUser.GetCurrentUser().TerminalName,                // terminal (dummy / bisa diambil dari session)
                    SessionUser.GetCurrentUser().Username,            // user
                    SessionUser.GetCurrentUser().ShiftId.ToString(),               // shift
                    "Unpaid",              // statusBayar
                    _currentInvoice.GrandTotal.ToString(), // jumlahBayar
                    "-",               // metodeBayar
                    "0"                   // kembalian, bisa dihitung sesuai bayar
                );
                UpdateInvoiceSummary();

                txtCariBarang.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void AddInvoiceItemToPanel(InvoiceItem item)
        {

            // Tampilkan Qty + Satuan di struk, misal "2 dus" atau "3 pcs"
            string qtySatuan = $"{item.Qty} {(string.IsNullOrEmpty(item.UnitVariant) ? item.Unit : item.UnitVariant)}";


            // Panel utama per item, tipis seperti struk
            int w = Math.Max(240, flpInvoice.ClientSize.Width - 35);
            var panel = new Panel
            {
                Width = w,
                AutoSize = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0),
                Margin = new Padding(0, 0, 0, 2) // spasi tipis antar item
            };

            var stack = new FlowLayoutPanel
            {
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Width = w
            };

            var lblDetail = new Label
            {
                Text = $"{item.Name}",
                Font = new Font("Consolas", 9, FontStyle.Regular),
                AutoSize = false,
                Width = w,
                Height = 18,
                ForeColor = Color.Black
            };

            decimal lineSubTotal = item.Price * item.Qty;
            if (lineSubTotal < 0m) lineSubTotal = 0m;

            string detail2 = $"{qtySatuan} x {item.Price:N0} = {lineSubTotal:N0}";
            if (item.DiscountAmount > 0)
            {
                if (item.DiscountPercent > 0)
                    detail2 += $"  Disc {item.DiscountPercent:N0}% (-{item.DiscountAmount:N0})";
                else
                    detail2 += $"  Disc -{item.DiscountAmount:N0}";
            }
            if (item.Tax > 0)
                detail2 += $"  Tax {item.Tax:N0}";

            var lblDetail2 = new Label
            {
                Text = detail2,
                Font = new Font("Consolas", 9, FontStyle.Regular),
                AutoSize = false,
                Width = w,
                Height = 18,
                ForeColor = Color.Black
            };

            var lblTotal = new Label
            {
                Text = $"Total: {item.Total:N0}",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                AutoSize = false,
                Width = w,
                Height = 20,
                TextAlign = ContentAlignment.MiddleRight,
                Margin = new Padding(0, 0, 0, 0)
            };

            stack.Controls.Add(lblDetail);
            stack.Controls.Add(lblDetail2);
            stack.Controls.Add(lblTotal);

            if (!string.IsNullOrWhiteSpace(item.Note))
            {
                var lblNote = new Label
                {
                    Text = $"  Note: {item.Note}",
                    Font = new Font("Consolas", 9, FontStyle.Italic),
                    AutoSize = false,
                    Width = w,
                    Height = 18,
                    ForeColor = Color.FromArgb(80, 80, 80)
                };
                stack.Controls.Add(lblNote);
            }
            panel.Controls.Add(stack);

            var separator = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Height = 1,
                Width = w,
                Margin = new Padding(0, 2, 0, 2)
            };

            flpInvoice.Controls.Add(panel);
            flpInvoice.Controls.Add(separator);
        }

        private void RenderReceiptUI(
    InvoiceData invoice,
    string terminal,
    string user,
    string shift,
    string statusBayar,
    string jumlahBayar,
    string metodeBayar,
    string kembalian)
        {
            RecalculateInvoiceTotals(invoice);
            flpInvoice.Controls.Clear();

            string judul = "", alamat = "", telepon = "", footer = "";
            bool showNamaToko = true, showAlamat = true, showTelepon = true, showFooter = true;

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM struk_setting ORDER BY updated_at DESC LIMIT 1;";
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        judul = reader["judul"]?.ToString()?.Trim() ?? "";
                        alamat = reader["alamat"]?.ToString()?.Trim() ?? "";
                        telepon = reader["telepon"]?.ToString()?.Trim() ?? "";
                        footer = reader["footer"]?.ToString()?.Trim() ?? "";
                        showNamaToko = reader["is_visible_nama_toko"] as bool? ?? true;
                        showAlamat = reader["is_visible_alamat"] as bool? ?? true;
                        showTelepon = reader["is_visible_telepon"] as bool? ?? true;
                        showFooter = reader["is_visible_footer"] as bool? ?? true;
                    }
                }
            }

            void AddLabel(string text,
                 int fontSize = 9,
                 bool bold = false,
                 Color? color = null,
                 ContentAlignment align = ContentAlignment.MiddleLeft)
            {
                var lbl = new Label
                {
                    Text = text,
                    Font = new Font("Consolas", fontSize, bold ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = color ?? Color.Black,
                    Width = flpInvoice.Width - 5,   // FULL WIDTH
                    AutoSize = false,           
                    TextAlign = align
                };

                flpInvoice.Controls.Add(lbl);
            }



            if (showNamaToko && !string.IsNullOrEmpty(judul))
                AddLabel(judul, 11, true, null, ContentAlignment.MiddleCenter);

            if (showAlamat && !string.IsNullOrEmpty(alamat))
                AddLabel(alamat, 9, false, null, ContentAlignment.MiddleCenter);

            if (showTelepon && !string.IsNullOrEmpty(telepon))
                AddLabel(telepon, 9, false, null, ContentAlignment.MiddleCenter);

            AddLabel(new string('-', 32), 9, false, null, ContentAlignment.MiddleCenter);


            AddLabel($"Terminal : {terminal}");
            AddLabel($"Kasir    : {user}");
            AddLabel($"Shift    : {shift}");
            AddLabel($"Tanggal  : {DateTime.Now:yyyy-MM-dd}");
            AddLabel($"Waktu    : {DateTime.Now:HH:mm:ss}");
            AddLabel(new string('-', 32));

            foreach (var item in invoice.Items)
            {
                AddInvoiceItemToPanel(item);
            }

            AddLabel(new string('-', 32));
            AddLabel($"Subtotal".PadRight(20) + $"{invoice.Subtotal:N0}".PadLeft(10));
            AddLabel($"Item Discount".PadRight(20) + $"-{invoice.TotalDiscount:N0}".PadLeft(10));
            if (invoice.GlobalDiscountIsAmount && invoice.GlobalDiscountValue > 0)
                AddLabel($"Global Disc (Rp)".PadRight(20) + $"-{invoice.GlobalDiscountValue:N0}".PadLeft(10));
            else if (invoice.GlobalDiscountPercent > 0)
                AddLabel($"Global Disc ({invoice.GlobalDiscountPercent:N0}%)".PadRight(20) + $"-{invoice.GlobalDiscountValue:N0}".PadLeft(10));
            if (!string.IsNullOrEmpty(invoice.GlobalNote))
                AddLabel($"Note".PadRight(20) + $"{invoice.GlobalNote}");
            if (invoice.DeliveryAmount > 0)
                AddLabel($"Delivery".PadRight(20) + $"{invoice.DeliveryAmount:N0}".PadLeft(10));
            AddLabel(new string('-', 32));
            AddLabel($"Grand Total".PadRight(20) + $"{invoice.GrandTotal:N0}".PadLeft(10));

            if (decimal.TryParse(jumlahBayar, out decimal jmlBayar)) jumlahBayar = jmlBayar.ToString("N0");
            if (decimal.TryParse(kembalian, out decimal kemb)) kembalian = kemb.ToString("N0");

            AddLabel(new string('-', 32));
            AddLabel($"Status Bayar : {statusBayar}");
            AddLabel($"Metode Bayar : {metodeBayar}");
            AddLabel($"Jumlah Bayar : {jumlahBayar}");
            AddLabel($"Kembalian    : {kembalian}");
            AddLabel(new string('-', 32));

            if (showFooter && !string.IsNullOrEmpty(footer))
                AddLabel(footer, 9, false, Color.Gray, ContentAlignment.MiddleCenter);

        }


        private void UpdateInvoiceSummary()
        {
            RecalculateInvoiceTotals(_currentInvoice);
            lblTotal.Text = _currentInvoice.GrandTotal.ToString("N0");
            labelNumOfItems.Text = _currentInvoice.Items.Sum(x => x.Qty).ToString();
            lblKembalian.Text = _currentInvoice.ChangeAmount.ToString("N0");
        }

        private void RecalculateInvoiceTotals(InvoiceData invoice)
        {
            if (invoice == null) return;
            if (invoice.Items == null) invoice.Items = new List<InvoiceItem>();

            decimal subTotal = 0m;
            decimal itemDiscount = 0m;
            foreach (var i in invoice.Items)
            {
                var lineSub = i.Price * i.Qty;
                if (lineSub < 0m) lineSub = 0m;
                subTotal += lineSub;

                var disc = i.DiscountAmount;
                if (disc < 0m) disc = 0m;
                if (disc > lineSub) disc = lineSub;
                itemDiscount += disc;
            }

            var netAfterItemDiscount = subTotal - itemDiscount;
            if (netAfterItemDiscount < 0m) netAfterItemDiscount = 0m;

            decimal globalDiscValue;
            if (invoice.GlobalDiscountIsAmount)
            {
                globalDiscValue = invoice.GlobalDiscountValue;
            }
            else
            {
                var globalDiscPercent = invoice.GlobalDiscountPercent;
                if (globalDiscPercent < 0m) globalDiscPercent = 0m;
                if (globalDiscPercent > 100m) globalDiscPercent = 100m;
                globalDiscValue = Math.Round((netAfterItemDiscount * globalDiscPercent) / 100m, 2, MidpointRounding.AwayFromZero);
            }
            if (globalDiscValue < 0m) globalDiscValue = 0m;
            if (globalDiscValue > netAfterItemDiscount) globalDiscValue = netAfterItemDiscount;

            var delivery = invoice.DeliveryAmount;
            if (delivery < 0m) delivery = 0m;

            invoice.Subtotal = subTotal;
            invoice.TotalDiscount = itemDiscount;
            invoice.GlobalDiscountValue = globalDiscValue;
            invoice.GrandTotal = (netAfterItemDiscount - globalDiscValue) + delivery;
            if (invoice.GrandTotal < 0m) invoice.GrandTotal = 0m;
        }


        private void RenderInvoice(InvoiceData invoice)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = invoice.Items.Select(i => new
            {
                PtId = i.pt_id,
                i.Name,
                i.Qty,
                Satuan =  i.Unit ,
                Harga = i.Price,
                Diskon = i.DiscountPercent > 0 ? $"{i.DiscountPercent:N0}%" : (i.DiscountAmount > 0 ? $"-{i.DiscountAmount:N0}" : "0"),
                Catatan = i.Note ?? "",
                Total = i.Total
            }).ToList();


            dataGridViewCart4.DataSource = bs;
            if (dataGridViewCart4.Columns.Contains("PtId"))
                dataGridViewCart4.Columns["PtId"].Visible = false;
            dataGridViewCart4.Columns["Harga"].DefaultCellStyle.Format = "N0";
            dataGridViewCart4.Columns["Total"].DefaultCellStyle.Format = "N0";
            labelNumOfItems.Text = invoice.Items.Sum(x => x.Qty).ToString();
        }

        private void dataGridViewCart4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridViewCart4.Rows[e.RowIndex];
            int ptId = Convert.ToInt32(row.Cells["PtId"].Value);
            var item = _currentInvoice.Items.FirstOrDefault(x => x.pt_id == ptId);
            if (item == null) return;

            var modal = new FormUpdateItem(_currentInvoice, item, _cartService, updatedInvoice =>
            {
                _currentInvoice = updatedInvoice;
                RecalculateInvoiceTotals(_currentInvoice);
                RenderInvoice(_currentInvoice);
                RenderReceiptUI(
                    _currentInvoice,
                    SessionUser.GetCurrentUser().TerminalName,
                    SessionUser.GetCurrentUser().Username,
                    SessionUser.GetCurrentUser().ShiftId.ToString(),
                    "Unpaid",
                    _currentInvoice.GrandTotal.ToString(),
                    "-",
                    "0"
                );
                UpdateInvoiceSummary();
            });

            modal.ShowDialog();
        }


        private void DataGridViewCart4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // Supaya tidak pindah ke row bawah default

                // Ambil row yang aktif / selected
                if (dataGridViewCart4.CurrentRow == null) return;

                var row = dataGridViewCart4.CurrentRow;
                int ptId = Convert.ToInt32(row.Cells["PtId"].Value);
                var item = _currentInvoice.Items.FirstOrDefault(x => x.pt_id == ptId);
                if (item == null) return;

                // Buka modal
                var modal = new FormUpdateItem(_currentInvoice, item, _cartService, updatedInvoice =>
                {
                    _currentInvoice = updatedInvoice;
                    RecalculateInvoiceTotals(_currentInvoice);
                    RenderReceiptUI(
                        _currentInvoice,     
                        SessionUser.GetCurrentUser().TerminalName,               
                        SessionUser.GetCurrentUser().Username,           
                        SessionUser.GetCurrentUser().ShiftId.ToString(),              
                        "Unpaid",            
                        _currentInvoice.GrandTotal.ToString(), 
                        "-",             
                        "0"               
                    );

                    RenderInvoice(_currentInvoice);
                    UpdateInvoiceSummary();
                });

                modal.ShowDialog();
            }
        }


        private void RefreshInvoicePanel()
        {
            flpInvoice.Controls.Clear();

            foreach (var item in _currentInvoice.Items)
            {
                AddInvoiceItemToPanel(item);
            }
            UpdateInvoiceSummary();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private TransactionRepo repo;
        private void button1_Click(object sender, EventArgs e)
        {

            RecalculateInvoiceTotals(_currentInvoice);
            using (PaymentModalForm paymentModal =
                new PaymentModalForm(_currentInvoice.GrandTotal, _currentInvoice.CustomerName))
            {
                // 🔹 Realtime preview (UI only)
                paymentModal.PaymentAmountChanged += (amount) =>
                {
                    decimal change = _transactionService
                        .CalculateChangePreview(_currentInvoice, amount);

                    lblKembalian.Text = change.ToString("N0");
                };

                paymentModal.GlobalDiscountChanged += (percent) =>
                {
                    _currentInvoice.GlobalDiscountIsAmount = false;
                    _currentInvoice.GlobalDiscountPercent = percent;
                    _currentInvoice.GlobalDiscountValue = 0m;
                    RecalculateInvoiceTotals(_currentInvoice);
                    paymentModal.SetTotalAmount(_currentInvoice.GrandTotal);
                    RenderReceiptUI(
                        _currentInvoice,
                        SessionUser.GetCurrentUser().TerminalName,
                        SessionUser.GetCurrentUser().Username,
                        SessionUser.GetCurrentUser().ShiftId.ToString(),
                        "Unpaid",
                        _currentInvoice.GrandTotal.ToString(),
                        "-",
                        "0"
                    );
                    UpdateInvoiceSummary();
                };

                paymentModal.GlobalDiscountAmountChanged += (amount) =>
                {
                    _currentInvoice.GlobalDiscountIsAmount = true;
                    _currentInvoice.GlobalDiscountPercent = 0m;
                    _currentInvoice.GlobalDiscountValue = amount;
                    RecalculateInvoiceTotals(_currentInvoice);
                    paymentModal.SetTotalAmount(_currentInvoice.GrandTotal);
                    RenderReceiptUI(
                        _currentInvoice,
                        SessionUser.GetCurrentUser().TerminalName,
                        SessionUser.GetCurrentUser().Username,
                        SessionUser.GetCurrentUser().ShiftId.ToString(),
                        "Unpaid",
                        _currentInvoice.GrandTotal.ToString(),
                        "-",
                        "0"
                    );
                    UpdateInvoiceSummary();
                };

                paymentModal.DeliveryAmountChanged += (delivery) =>
                {
                    _currentInvoice.DeliveryAmount = delivery;
                    RecalculateInvoiceTotals(_currentInvoice);
                    paymentModal.SetTotalAmount(_currentInvoice.GrandTotal);
                    RenderReceiptUI(
                        _currentInvoice,
                        SessionUser.GetCurrentUser().TerminalName,
                        SessionUser.GetCurrentUser().Username,
                        SessionUser.GetCurrentUser().ShiftId.ToString(),
                        "Unpaid",
                        _currentInvoice.GrandTotal.ToString(),
                        "-",
                        "0"
                    );
                    UpdateInvoiceSummary();
                };

                paymentModal.GlobalNoteChanged += (note) =>
                {
                    _currentInvoice.GlobalNote = note ?? "";
                    RenderReceiptUI(
                        _currentInvoice,
                        SessionUser.GetCurrentUser().TerminalName,
                        SessionUser.GetCurrentUser().Username,
                        SessionUser.GetCurrentUser().ShiftId.ToString(),
                        "Unpaid",
                        _currentInvoice.GrandTotal.ToString(),
                        "-",
                        "0"
                    );
                };

                paymentModal.ProcessPaymentHandler = () =>
                {
                    _currentInvoice.GlobalDiscountIsAmount = paymentModal.GlobalDiscountIsAmount;
                    _currentInvoice.GlobalDiscountPercent = paymentModal.GlobalDiscountPercent;
                    _currentInvoice.GlobalDiscountValue = paymentModal.GlobalDiscountIsAmount ? paymentModal.GlobalDiscountAmount : 0m;
                    _currentInvoice.DeliveryAmount = paymentModal.DeliveryAmount;
                    _currentInvoice.GlobalNote = paymentModal.GlobalNote;
                    RecalculateInvoiceTotals(_currentInvoice);

                    TransactionResult result;
                    decimal paid;
                    string methodLabel;

                    if (paymentModal.IsSplitPayment && paymentModal.SplitPayments != null)
                    {
                        var parts = paymentModal.SplitPayments.ToList();
                        paid = parts.Sum(x => x.Amount);
                        methodLabel = "Split Bill";
                        result = _transactionService.ProcessSplitPaymentAndSave(_currentInvoice, parts);
                    }
                    else
                    {
                        paid = paymentModal.PaymentAmount;
                        methodLabel = paymentModal.PaymentMethod;
                        result = _transactionService.ProcessPaymentAndSave(_currentInvoice, paid, methodLabel);
                    }

                    if (!result.IsSuccess)
                    {
                        return new PaymentModalForm.PaymentProcessResult
                        {
                            IsSuccess = false,
                            Message = result.Message ?? "Gagal memproses pembayaran."
                        };
                    }

                    var change = paid - _currentInvoice.GrandTotal;
                    if (change < 0m) change = 0m;

                    var receiptText = BuildReceiptText(
                        _currentInvoice,
                        SessionUser.GetCurrentUser().TerminalName,
                        SessionUser.GetCurrentUser().Username,
                        SessionUser.GetCurrentUser().ShiftId.ToString(),
                        "Paid",
                        methodLabel,
                        paid,
                        change
                    );

                    return new PaymentModalForm.PaymentProcessResult
                    {
                        IsSuccess = true,
                        ReceiptText = receiptText
                    };
                };

                paymentModal.ShowDialog();
                if (!paymentModal.IsPaid) return;

                _currentInvoice = new InvoiceData();
                RenderInvoice(_currentInvoice);
                RenderReceiptUI(
                    _currentInvoice,
                    SessionUser.GetCurrentUser().TerminalName,
                    SessionUser.GetCurrentUser().Username,
                    SessionUser.GetCurrentUser().ShiftId.ToString(),
                    "Unpaid",
                    _currentInvoice.GrandTotal.ToString(),
                    "-",
                    "0"
                );
                UpdateInvoiceSummary();
                StartNewTransaction();
            }
        }

        private string BuildReceiptText(
            InvoiceData invoice,
            string terminal,
            string user,
            string shift,
            string statusBayar,
            string metodeBayar,
            decimal jumlahBayar,
            decimal kembalian
        )
        {
            RecalculateInvoiceTotals(invoice);

            string judul = "", alamat = "", telepon = "", footer = "";
            bool showNamaToko = true, showAlamat = true, showTelepon = true, showFooter = true;

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM struk_setting ORDER BY updated_at DESC LIMIT 1;";
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        judul = reader["judul"]?.ToString()?.Trim() ?? "";
                        alamat = reader["alamat"]?.ToString()?.Trim() ?? "";
                        telepon = reader["telepon"]?.ToString()?.Trim() ?? "";
                        footer = reader["footer"]?.ToString()?.Trim() ?? "";
                        showNamaToko = reader["is_visible_nama_toko"] as bool? ?? true;
                        showAlamat = reader["is_visible_alamat"] as bool? ?? true;
                        showTelepon = reader["is_visible_telepon"] as bool? ?? true;
                        showFooter = reader["is_visible_footer"] as bool? ?? true;
                    }
                }
            }

            var lines = new List<string>();
            void Add(string s) => lines.Add(s ?? "");

            if (showNamaToko && !string.IsNullOrEmpty(judul)) Add(judul);
            if (showAlamat && !string.IsNullOrEmpty(alamat)) Add(alamat);
            if (showTelepon && !string.IsNullOrEmpty(telepon)) Add(telepon);
            Add(new string('-', 32));
            Add($"Terminal : {terminal}");
            Add($"Kasir    : {user}");
            Add($"Shift    : {shift}");
            Add($"Tanggal  : {DateTime.Now:yyyy-MM-dd}");
            Add($"Waktu    : {DateTime.Now:HH:mm:ss}");
            Add(new string('-', 32));

            foreach (var item in invoice.Items)
            {
                var qtySatuan = $"{item.Qty} {(string.IsNullOrEmpty(item.UnitVariant) ? item.Unit : item.UnitVariant)}";
                var lineSub = item.Price * item.Qty;
                if (lineSub < 0m) lineSub = 0m;
                var d = $"{qtySatuan} x {item.Price:N0} = {lineSub:N0}";
                if (item.DiscountAmount > 0m)
                {
                    if (item.DiscountPercent > 0m) d += $"  Disc {item.DiscountPercent:N0}% (-{item.DiscountAmount:N0})";
                    else d += $"  Disc -{item.DiscountAmount:N0}";
                }
                Add(item.Name ?? "");
                Add(d);
                Add($"Total: {item.Total:N0}");
                if (!string.IsNullOrWhiteSpace(item.Note)) Add($"Note: {item.Note}");
                Add("");
            }

            Add(new string('-', 32));
            Add($"Subtotal".PadRight(20) + $"{invoice.Subtotal:N0}".PadLeft(10));
            Add($"Item Discount".PadRight(20) + $"-{invoice.TotalDiscount:N0}".PadLeft(10));
            if (invoice.GlobalDiscountIsAmount && invoice.GlobalDiscountValue > 0)
                Add($"Global Disc (Rp)".PadRight(20) + $"-{invoice.GlobalDiscountValue:N0}".PadLeft(10));
            else if (invoice.GlobalDiscountPercent > 0)
                Add($"Global Disc ({invoice.GlobalDiscountPercent:N0}%)".PadRight(20) + $"-{invoice.GlobalDiscountValue:N0}".PadLeft(10));
            if (!string.IsNullOrEmpty(invoice.GlobalNote))
                Add($"Note: {invoice.GlobalNote}");
            if (invoice.DeliveryAmount > 0)
                Add($"Delivery".PadRight(20) + $"{invoice.DeliveryAmount:N0}".PadLeft(10));
            Add(new string('-', 32));
            Add($"Grand Total".PadRight(20) + $"{invoice.GrandTotal:N0}".PadLeft(10));

            Add(new string('-', 32));
            Add($"Status Bayar : {statusBayar}");
            Add($"Metode Bayar : {metodeBayar}");
            Add($"Jumlah Bayar : {jumlahBayar:N0}");
            Add($"Kembalian    : {kembalian:N0}");
            Add(new string('-', 32));

            if (showFooter && !string.IsNullOrEmpty(footer)) Add(footer);

            return string.Join(Environment.NewLine, lines);
        }


        private (string customer, string note)? ShowDraftInfoModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "Informasi Draft Transaksi";
                modal.Size = new Size(400, 380);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;
                modal.MinimizeBox = false;
                modal.Padding = new Padding(20);

                Label lblTitle = new Label
                {
                    Text = "Simpan Draft",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    Height = 35,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Panel pnlMain = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };

                Label lblCustomer = new Label
                {
                    Text = "Nama Customer:",
                    Dock = DockStyle.Top,
                    Height = 25,
                    TabIndex = 0,
                    TabStop = false
                };

                TextBox txtCustomer = new TextBox
                {
                    Dock = DockStyle.Top,
                    TabIndex = 1,
                    Font = new Font("Segoe UI", 10)
                };

                Panel spacer1 = new Panel { Dock = DockStyle.Top, Height = 15 };

                Label lblNote = new Label
                {
                    Text = "Catatan / Keterangan:",
                    Dock = DockStyle.Top,
                    Height = 25,
                    TabIndex = 2,
                    TabStop = false
                };

                TextBox txtNote = new TextBox
                {
                    Dock = DockStyle.Top,
                    TabIndex = 3,
                    Multiline = true,
                    Height = 80,
                    Font = new Font("Segoe UI", 10)
                };

                Panel pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(0, 10, 0, 0) };

                Button btnOk = new Button
                {
                    Text = "Simpan Draft",
                    Size = new Size(150, 45),
                    Location = new Point(190, 10),
                    BackColor = Color.Orange,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TabIndex = 4
                };

                Button btnCancel = new Button
                {
                    Text = "Batal",
                    Size = new Size(100, 45),
                    Location = new Point(80, 10),
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = 5
                };


                (string, string)? result = null;

                btnOk.Click += (s, e) =>
                {
                    result = (txtCustomer.Text.Trim(), txtNote.Text.Trim());
                    modal.Close();
                };

                btnCancel.Click += (s, e) => modal.Close();

                pnlMain.Controls.Add(txtNote);
                pnlMain.Controls.Add(lblNote);
                pnlMain.Controls.Add(spacer1);
                pnlMain.Controls.Add(txtCustomer);
                pnlMain.Controls.Add(lblCustomer);

                pnlButtons.Controls.Add(btnOk);
                pnlButtons.Controls.Add(btnCancel);

                modal.Controls.Add(pnlMain);
                modal.Controls.Add(lblTitle);
                modal.Controls.Add(pnlButtons);

                modal.AcceptButton = btnOk;
                modal.CancelButton = btnCancel;
                txtCustomer.Focus();

                modal.ShowDialog();
                return result;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Apakah Anda ingin menyimpan cart ini sebagai Draft?",
                "Konfirmasi Draft",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            var info = ShowDraftInfoModal();
            if (info == null) return;

            try
            {
                bool result = _cartService.SaveCartAsDraft(
                    info.Value.customer,
                    info.Value.note
                );

                if (result)
                {
                    StartNewTransaction();
                    RenderInvoice(_currentInvoice);
                    var session = SessionUser.GetCurrentUser();
                    RenderReceiptUI(
                        _currentInvoice,
                        session.TerminalName,
                        session.Username,
                        session.ShiftId.ToString(),
                        "Unpaid",
                        _currentInvoice.GrandTotal.ToString("N0"),
                        "-",
                        "0"
                    );
                    UpdateInvoiceSummary();

                    MessageBox.Show("Cart berhasil disimpan sebagai Draft!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan draft: " + ex.Message);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int? poId = ShowDraftModal();

                if (poId == null) return;

                var session = SessionUser.GetCurrentUser();

                _currentInvoice = _cartService.LoadDraftToInvoice(poId.Value);

                RenderInvoice(_currentInvoice);

                RenderReceiptUI(
                    _currentInvoice,
                    session.TerminalName,
                    session.Username,
                    session.ShiftId.ToString(),
                    "Unpaid",
                    _currentInvoice.GrandTotal.ToString("N0"),
                    "-",
                    "0"
                );

                UpdateInvoiceSummary();

                MessageBox.Show("Draft berhasil dimuat.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Gagal load draft",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }








        private int? ShowDraftModal()
        {
            var session = SessionUser.GetCurrentUser();
            var repoDraft = new CartActivity();
            var drafts = repoDraft.GetDraftOrders(session.TerminalId, session.UserId);
            if (drafts == null || drafts.Count == 0)
            {
                MessageBox.Show("Tidak ada draft.");
                return null;
            }

            int? selectedPoId = null;

            using (Form modal = new Form())
            {
                modal.Text = "Pilih Draft Transaksi";
                modal.Size = new Size(900, 600);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;
                modal.MinimizeBox = false;

                // Header Panel
                Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, Padding = new Padding(15) };

                Label lblTitle = new Label
                {
                    Text = "Daftar Draft Transaksi",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    Dock = DockStyle.Left,
                    Width = 300,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                TextBox txtSearch = new TextBox
                {
                    PlaceholderText = "🔍 Cari nama customer, catatan, atau tanggal...",
                    Dock = DockStyle.Right,
                    Width = 400,
                    Font = new Font("Segoe UI", 11),
                    TabIndex = 0
                };
                pnlHeader.Controls.Add(lblTitle);
                pnlHeader.Controls.Add(txtSearch);

                // Footer Panel
                Panel pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 70, Padding = new Padding(15) };

                Label lblTotal = new Label
                {
                    Text = $"Total Draft: {drafts.Count}",
                    Dock = DockStyle.Left,
                    Width = 200,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic)
                };

                Button btnLoad = new Button
                {
                    Text = "Muat Draft (Enter)",
                    Size = new Size(180, 40),
                    Location = new Point(680, 15),
                    BackColor = Color.DodgerBlue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TabIndex = 2
                };

                Button btnCancel = new Button
                {
                    Text = "Batal (Esc)",
                    Size = new Size(120, 40),
                    Location = new Point(550, 15),
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = 3
                };
                pnlFooter.Controls.Add(lblTotal);
                pnlFooter.Controls.Add(btnLoad);
                pnlFooter.Controls.Add(btnCancel);

                DataGridView grid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                    GridColor = Color.FromArgb(235, 235, 235),
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    Font = new Font("Segoe UI", 11),
                    RowTemplate = { Height = 45 },
                    TabIndex = 1
                };
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 248, 255);
                grid.DefaultCellStyle.SelectionForeColor = Color.Black;
                grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                grid.ColumnHeadersHeight = 45;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                DataGridView gridDetails = new DataGridView
                {
                    Dock = DockStyle.Bottom,
                    Height = 200,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.WhiteSmoke,
                    BorderStyle = BorderStyle.None,
                    CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                    GridColor = Color.FromArgb(220, 220, 220),
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    Font = new Font("Segoe UI", 10),
                    RowTemplate = { Height = 35 },
                    TabIndex = 2
                };
                gridDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                gridDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
                gridDetails.ColumnHeadersHeight = 40;
                gridDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                grid.Columns.Add("PoId", "ID");
                grid.Columns.Add("Customer", "Customer");
                grid.Columns.Add("Note", "Catatan");
                grid.Columns.Add("Total", "Total");
                grid.Columns.Add("CreatedAt", "Tanggal");
                grid.Columns.Add("Status", "Status");
                grid.Columns.Add("CartCode", "Cart");

                grid.Columns["PoId"].Width = 50;
                grid.Columns["Total"].DefaultCellStyle.Format = "N0";
                grid.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                void LoadGrid(IEnumerable<POS_qu.DTO.PendingOrderDto> source)
                {
                    grid.Rows.Clear();
                    foreach (var d in source)
                    {
                        grid.Rows.Add(
                            d.PoId,
                            string.IsNullOrWhiteSpace(d.CustomerName) ? "-" : d.CustomerName,
                            string.IsNullOrWhiteSpace(d.Note) ? "-" : d.Note,
                            d.Total,
                            d.CreatedAt.ToString("dd MMM yyyy HH:mm"),
                            "draft",
                            d.CartSessionCode ?? ""
                        );
                    }
                    lblTotal.Text = $"Total Draft: {source.Count()}";
                    // pewarnaan
                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    }
                }

                LoadGrid(drafts);
                void LoadDetails()
                {
                    if (grid.SelectedRows.Count == 0)
                    {
                        gridDetails.DataSource = null;
                        return;
                    }
                    var cartCode = grid.SelectedRows[0].Cells["CartCode"]?.Value?.ToString() ?? "";
                    if (string.IsNullOrEmpty(cartCode))
                    {
                        gridDetails.DataSource = null;
                        return;
                    }
                    DataTable dt = repoDraft.GetPendingItems(cartCode);
                    gridDetails.DataSource = dt;
                    DataGridViewColumn col;
                    if ((col = gridDetails.Columns["name"]) != null)
                        col.HeaderText = "Item";
                    if ((col = gridDetails.Columns["unit"]) != null)
                        col.HeaderText = "Unit";
                    if ((col = gridDetails.Columns["qty"]) != null)
                    {
                        col.HeaderText = "Qty";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if ((col = gridDetails.Columns["price"]) != null)
                    {
                        col.HeaderText = "Harga";
                        col.DefaultCellStyle.Format = "N0";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if ((col = gridDetails.Columns["sell_price"]) != null)
                    {
                        col.HeaderText = "Harga";
                        col.DefaultCellStyle.Format = "N0";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if ((col = gridDetails.Columns["total"]) != null)
                    {
                        col.HeaderText = "Total";
                        col.DefaultCellStyle.Format = "N0";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                grid.SelectionChanged += (s, e) => LoadDetails();
                LoadDetails();

                txtSearch.TextChanged += (s, e) =>
                {
                    string key = txtSearch.Text.Trim().ToLower();
                    var filtered = drafts.Where(d =>
                        (!string.IsNullOrWhiteSpace(d.CustomerName) && d.CustomerName.ToLower().Contains(key)) ||
                        (!string.IsNullOrWhiteSpace(d.Note) && d.Note.ToLower().Contains(key)) ||
                        d.Total.ToString().Contains(key) ||
                        d.CreatedAt.ToString("dd-MM-yyyy HH:mm").ToLower().Contains(key)
                    );
                    LoadGrid(filtered);
                };

                grid.ColumnHeaderMouseClick += (s, e) =>
                {
                    IEnumerable<POS_qu.DTO.PendingOrderDto> ordered = drafts;
                    switch (e.ColumnIndex)
                    {
                        case 1: ordered = drafts.OrderBy(d => d.CustomerName); break;
                        case 3: ordered = drafts.OrderByDescending(d => d.Total); break;
                        case 4: ordered = drafts.OrderByDescending(d => d.CreatedAt); break;
                        default: ordered = drafts; break;
                    }
                    LoadGrid(ordered);
                };

                btnLoad.Click += (s, ev) =>
                {
                    if (grid.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Pilih draft dulu.");
                        return;
                    }
                    selectedPoId = Convert.ToInt32(grid.SelectedRows[0].Cells["PoId"].Value);
                    modal.DialogResult = DialogResult.OK;
                    modal.Close();
                };

                btnCancel.Click += (s, ev) => modal.Close();

                modal.Controls.Add(gridDetails);
                modal.Controls.Add(grid);
                modal.Controls.Add(pnlHeader);
                modal.Controls.Add(pnlFooter);

                modal.AcceptButton = btnLoad;
                modal.CancelButton = btnCancel;

                txtSearch.Focus();
                modal.ShowDialog();
            }

            return selectedPoId;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_currentInvoice.GrandTotal <= 0)
            {
                MessageBox.Show("Total belanja kosong.");
                return;
            }

            var result = ShowInstallmentModal(_currentInvoice.GrandTotal);
            if (result == null) return;

            repo = new TransactionRepo();
            _transactionService = new TransactionService(repo, activityService);

            var save = _transactionService.ProcessInstallmentAndSave(
                _currentInvoice,
                result.Value.Amount,
                result.Value.Customer,
                result.Value.Note
            );

            if (!save.IsSuccess)
            {
                MessageBox.Show(save.Message);
                return;
            }

            MessageBox.Show("Transaksi Bon berhasil disimpan");

            _currentInvoice = new InvoiceData();
            RenderInvoice(_currentInvoice);
            StartNewTransaction();
        }



        private (decimal Amount, string Customer, string Note)? ShowInstallmentModal(decimal totalDue)
        {
            decimal amount = 0;
            string customer = null;
            string note = null;

            using (Form modal = new Form())
            {
                modal.Text = "Pembayaran Bon / Cicilan";
                modal.Size = new Size(450, 550);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;
                modal.MinimizeBox = false;
                modal.Padding = new Padding(20);

                Label lblTitle = new Label
                {
                    Text = "Detail Pembayaran Bon",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label lblTotalInfo = new Label
                {
                    Text = $"Total yang harus dibayar: {totalDue:N0}",
                    Font = new Font("Segoe UI", 11, FontStyle.Italic),
                    Dock = DockStyle.Top,
                    Height = 30,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.DarkRed
                };

                Panel pnlMain = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10)
                };

                // Nama Customer
                Label lblCustomer = new Label { Text = "Nama Customer:", Dock = DockStyle.Top, Height = 25 };
                TextBox txtCustomer = new TextBox { Dock = DockStyle.Top, TabIndex = 0, Font = new Font("Segoe UI", 11) };

                Button btnPickCustomer = new Button
                {
                    Text = "🔍 Pilih Customer Exist",
                    Dock = DockStyle.Top,
                    Height = 35,
                    TabIndex = 1,
                    Margin = new Padding(0, 5, 0, 15)
                };

                // Spacer
                Panel spacer1 = new Panel { Dock = DockStyle.Top, Height = 15 };

                // Nominal Bayar
                Label lblAmount = new Label { Text = "Nominal Bayar Sekarang:", Dock = DockStyle.Top, Height = 25 };
                TextBox txtAmount = new TextBox
                {
                    Dock = DockStyle.Top,
                    TabIndex = 2,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Text = totalDue.ToString("N0")
                };

                // Spacer
                Panel spacer2 = new Panel { Dock = DockStyle.Top, Height = 15 };

                // Catatan
                Label lblNote = new Label { Text = "Catatan:", Dock = DockStyle.Top, Height = 25 };
                TextBox txtNote = new TextBox
                {
                    Dock = DockStyle.Top,
                    TabIndex = 3,
                    Multiline = true,
                    Height = 80,
                    Font = new Font("Segoe UI", 10)
                };

                // Bottom Buttons
                Panel pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(0, 10, 0, 0) };

                Button btnSave = new Button
                {
                    Text = "Simpan Transaksi",
                    Size = new Size(180, 45),
                    Location = new Point(220, 10),
                    TabIndex = 4,
                    BackColor = Color.DodgerBlue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                Button btnCancel = new Button
                {
                    Text = "Batal",
                    Size = new Size(120, 45),
                    Location = new Point(90, 10),
                    TabIndex = 5,
                    FlatStyle = FlatStyle.Flat
                };

                btnPickCustomer.Click += (s, e) =>
                {
                    var customerDto = ShowCustomerPicker();
                    if (customerDto != null)
                        txtCustomer.Text = customerDto.Name;
                };

                btnCancel.Click += (s, e) => modal.Close();

                btnSave.Click += (s, e) =>
                {
                    string rawAmount = txtAmount.Text.Replace(".", "").Replace(",", "");
                    if (!decimal.TryParse(rawAmount, out amount))
                    {
                        MessageBox.Show("Nominal tidak valid");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtCustomer.Text))
                    {
                        MessageBox.Show("Isi nama customer");
                        return;
                    }

                    customer = txtCustomer.Text;
                    note = txtNote.Text;

                    modal.DialogResult = DialogResult.OK;
                    modal.Close();
                };

                modal.AcceptButton = btnSave;
                modal.CancelButton = btnCancel;

                // Add controls to panel
                pnlMain.Controls.Add(txtNote);
                pnlMain.Controls.Add(lblNote);
                pnlMain.Controls.Add(spacer2);
                pnlMain.Controls.Add(txtAmount);
                pnlMain.Controls.Add(lblAmount);
                pnlMain.Controls.Add(spacer1);
                pnlMain.Controls.Add(btnPickCustomer);
                pnlMain.Controls.Add(txtCustomer);
                pnlMain.Controls.Add(lblCustomer);

                pnlButtons.Controls.Add(btnSave);
                pnlButtons.Controls.Add(btnCancel);

                modal.Controls.Add(pnlMain);
                modal.Controls.Add(lblTotalInfo);
                modal.Controls.Add(lblTitle);
                modal.Controls.Add(pnlButtons);

                txtCustomer.Focus();

                if (modal.ShowDialog() != DialogResult.OK)
                    return null;
            }

            return (amount, customer, note);
        }


        private readonly CustomerService _customerService = new CustomerService();
        private CustomerDto ShowCustomerPicker()
        {
            CustomerDto selectedCustomer = null;

            using (Form modal = new Form())
            {
                modal.Text = "Pilih Data Pelanggan";
                modal.Size = new Size(450, 600);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;
                modal.MinimizeBox = false;

                // Header
                Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 70, Padding = new Padding(15) };
                Label lblTitle = new Label
                {
                    Text = "Database Pelanggan",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                pnlHeader.Controls.Add(lblTitle);

                // List Container
                Panel pnlList = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15, 0, 15, 0) };
                ListBox list = new ListBox
                {
                    Dock = DockStyle.Fill,
                    DisplayMember = "Name",
                    ValueMember = "Id",
                    Font = new Font("Segoe UI", 11),
                    BorderStyle = BorderStyle.FixedSingle,
                    TabIndex = 0
                };
                pnlList.Controls.Add(list);

                void LoadCustomers()
                {
                    var data = _customerService.GetAll();
                    list.DataSource = null;
                    list.DataSource = data;
                }
                LoadCustomers();

                // Footer Buttons
                Panel pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 120, Padding = new Padding(15) };

                Button btnOk = new Button
                {
                    Text = "Pilih Pelanggan (Enter)",
                    Dock = DockStyle.Top,
                    Height = 45,
                    BackColor = Color.DodgerBlue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TabIndex = 1
                };

                Panel spacer = new Panel { Dock = DockStyle.Top, Height = 10 };

                Button btnAdd = new Button
                {
                    Text = "+ Tambah Pelanggan Baru",
                    Dock = DockStyle.Top,
                    Height = 35,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9),
                    TabIndex = 2
                };

                pnlFooter.Controls.Add(btnAdd);
                pnlFooter.Controls.Add(spacer);
                pnlFooter.Controls.Add(btnOk);

                btnAdd.Click += (s, e) =>
                {
                    string name = Microsoft.VisualBasic.Interaction.InputBox(
                        "Masukkan Nama Pelanggan Baru:",
                        "Registrasi Pelanggan"
                    );

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        _customerService.Insert(name);
                        LoadCustomers();
                        // Auto select yang baru ditambah
                        for (int i = 0; i < list.Items.Count; i++)
                        {
                            if (((CustomerDto)list.Items[i]).Name == name)
                            {
                                list.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                };

                btnOk.Click += (s, e) =>
                {
                    if (list.SelectedItem == null)
                    {
                        MessageBox.Show("Silakan pilih pelanggan terlebih dahulu.");
                        return;
                    }
                    selectedCustomer = (CustomerDto)list.SelectedItem;
                    modal.DialogResult = DialogResult.OK;
                    modal.Close();
                };

                // Double click list langsung pilih
                list.DoubleClick += (s, e) => btnOk.PerformClick();

                modal.Controls.Add(pnlList);
                modal.Controls.Add(pnlHeader);
                modal.Controls.Add(pnlFooter);

                modal.AcceptButton = btnOk;

                list.Focus();
                modal.ShowDialog();
            }

            return selectedCustomer;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                repo = new TransactionRepo();
                _transactionService = new TransactionService(repo, activityService);

                var allInstallments = _transactionService.GetAllInstallments();
                if (allInstallments.Count == 0)
                {
                    MessageBox.Show("Belum ada data bon/cicilan.");
                    return;
                }

                using (Form modal = new Form())
                {
                    modal.Text = "Daftar Piutang / Bon Pelanggan";
                    modal.Size = new Size(1000, 600);
                    modal.StartPosition = FormStartPosition.CenterParent;
                    modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                    modal.MaximizeBox = false;
                    modal.MinimizeBox = false;

                    // Header
                    Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(15, 15, 15, 5) };
                    Label lblTitle = new Label
                    {
                        Text = "Monitoring Piutang (Bon)",
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        Dock = DockStyle.Fill
                    };
                    pnlHeader.Controls.Add(lblTitle);

                    // Grid
                    DataGridView grid = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        ReadOnly = true,
                        AutoGenerateColumns = false,
                        AllowUserToAddRows = false,
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        BackgroundColor = Color.White,
                        BorderStyle = BorderStyle.None,
                        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                        GridColor = Color.FromArgb(235, 235, 235),
                        RowHeadersVisible = false,
                        Font = new Font("Segoe UI", 11),
                        RowTemplate = { Height = 45 },
                        TabIndex = 0
                    };
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 248, 255);
                    grid.DefaultCellStyle.SelectionForeColor = Color.Black;
                    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    grid.ColumnHeadersHeight = 45;
                    grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                    grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "TransactionId", Width = 60 });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "No. Transaksi", DataPropertyName = "TransactionNumber", Width = 150 });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Pelanggan", DataPropertyName = "CustomerName", Width = 180 });
                    grid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Total Transaksi",
                        DataPropertyName = "TotalAmount",
                        Width = 130,
                        DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
                    });
                    grid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Sisa Piutang",
                        DataPropertyName = "DueAmount",
                        Width = 130,
                        DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight, ForeColor = Color.Red, Font = new Font("Segoe UI", 10, FontStyle.Bold) }
                    });
                    grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Terakhir Update", DataPropertyName = "CreatedAt", Width = 150 });

                    var transactions = allInstallments
                        .GroupBy(i => i.TransactionId)
                        .Select(g => g.First())
                        .OrderByDescending(x => x.DueAmount)
                        .ToList();

                    grid.DataSource = transactions;

                    // Footer
                    Panel pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(10) };
                    Label lblInfo = new Label
                    {
                        Text = "💡 Double klik pada baris untuk melihat riwayat cicilan atau melakukan pembayaran sisa bon.",
                        Dock = DockStyle.Fill,
                        Font = new Font("Segoe UI", 9, FontStyle.Italic),
                        ForeColor = Color.DimGray,
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    Button btnClose = new Button
                    {
                        Text = "Tutup (Esc)",
                        Dock = DockStyle.Right,
                        Width = 120,
                        FlatStyle = FlatStyle.Flat,
                        TabIndex = 1
                    };
                    btnClose.Click += (s, ev) => modal.Close();
                    pnlFooter.Controls.Add(lblInfo);
                    pnlFooter.Controls.Add(btnClose);

                    grid.CellDoubleClick += (s, ev) =>
                    {
                        if (ev.RowIndex < 0) return;
                        var selected = (InstallmentDto)grid.Rows[ev.RowIndex].DataBoundItem;
                        ShowTransactionInstallments(selected.TransactionId, selected.TransactionNumber);

                        // Refresh data setelah kembali dari detail (siapa tahu ada bayar)
                        var refreshed = _transactionService.GetAllInstallments();
                        grid.DataSource = refreshed.GroupBy(i => i.TransactionId).Select(g => g.First()).OrderByDescending(x => x.DueAmount).ToList();
                    };

                    modal.Controls.Add(grid);
                    modal.Controls.Add(pnlHeader);
                    modal.Controls.Add(pnlFooter);
                    modal.CancelButton = btnClose;

                    modal.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Method untuk menampilkan detail cicilan
        private void ShowTransactionInstallments(int transactionId, string transactionNumber)
        {
            var installments = _transactionService.GetInstallments(transactionId);

            if (installments.Count == 0)
            {
                MessageBox.Show("Belum ada riwayat cicilan.");
                return;
            }

            decimal totalPaid = installments.Sum(i => i.Amount);
            decimal totalDue = installments.First().DueAmount;

            using (Form modal = new Form())
            {
                modal.Text = $"Riwayat Cicilan - {transactionNumber}";
                modal.Size = new Size(850, 550);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;
                modal.MinimizeBox = false;

                // Header Info
                Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, Padding = new Padding(15), BackColor = Color.GhostWhite };
                Label lblSummary = new Label
                {
                    Dock = DockStyle.Fill,
                    Text = $"Ringkasan Pembayaran\nTotal Terbayar: Rp {totalPaid:N0}  |  Sisa Piutang: Rp {totalDue:N0}",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                pnlHeader.Controls.Add(lblSummary);

                // Grid
                DataGridView grid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    AutoGenerateColumns = false,
                    AllowUserToAddRows = false,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                    GridColor = Color.FromArgb(235, 235, 235),
                    RowHeadersVisible = false,
                    Font = new Font("Segoe UI", 11),
                    RowTemplate = { Height = 45 },
                    TabIndex = 2
                };
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 248, 255);
                grid.DefaultCellStyle.SelectionForeColor = Color.Black;
                grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                grid.ColumnHeadersHeight = 45;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Nominal Bayar",
                    DataPropertyName = "Amount",
                    Width = 150,
                    DefaultCellStyle = { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight, ForeColor = Color.Green, Font = new Font("Segoe UI", 10, FontStyle.Bold) }
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Catatan", DataPropertyName = "Note", Width = 250 });
                grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kasir", DataPropertyName = "CreatedByName", Width = 120 });
                grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tanggal", DataPropertyName = "CreatedAt", Width = 180 });

                grid.DataSource = installments;

                // Footer Buttons
                Panel pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 70, Padding = new Padding(15) };

                Button btnPay = new Button
                {
                    Text = "Bayar Sisa Bon",
                    Size = new Size(180, 40),
                    Location = new Point(640, 15),
                    BackColor = Color.ForestGreen,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Visible = totalDue > 0,
                    TabIndex = 0
                };

                Button btnClose = new Button
                {
                    Text = "Tutup",
                    Size = new Size(100, 40),
                    Location = new Point(530, 15),
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = 1
                };

                btnPay.Click += (s, e) =>
                {
                    ShowPaymentForm(transactionId, totalDue);
                    modal.Close(); // Tutup agar bisa refresh di parent
                };

                btnClose.Click += (s, e) => modal.Close();

                pnlButtons.Controls.Add(btnPay);
                pnlButtons.Controls.Add(btnClose);

                modal.Controls.Add(grid);
                modal.Controls.Add(pnlHeader);
                modal.Controls.Add(pnlButtons);

                modal.AcceptButton = btnPay;
                modal.CancelButton = btnClose;

                modal.ShowDialog();
            }
        }

        private void ShowPaymentForm(int transactionId, decimal dueAmount)
        {
            using (Form modal = new Form())
            {
                modal.Text = $"Pembayaran Sisa Bon #{transactionId}";
                modal.Size = new Size(400, 350);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;
                modal.MinimizeBox = false;
                modal.Padding = new Padding(15);

                // =============================
                // LABEL DAN TEXTBOX NOMINAL
                // =============================
                Label lblAmount = new Label
                {
                    Text = $"Jumlah Bayar (Sisa: {dueAmount:N0}):",
                    Dock = DockStyle.Top,
                    Height = 25
                };
                TextBox txtAmount = new TextBox
                {
                    Dock = DockStyle.Top,
                    Text = dueAmount.ToString("N0"),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    TabIndex = 0
                };

                Panel spacer1 = new Panel { Dock = DockStyle.Top, Height = 15 };

                // =============================
                // LABEL DAN TEXTBOX CATATAN
                // =============================
                Label lblNote = new Label
                {
                    Text = "Catatan:",
                    Dock = DockStyle.Top,
                    Height = 25
                };
                TextBox txtNote = new TextBox
                {
                    Dock = DockStyle.Top,
                    Multiline = true,
                    Height = 80,
                    TabIndex = 1
                };

                // =============================
                // BUTTON PANEL
                // =============================
                Panel pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(0, 10, 0, 0) };

                Button btnPay = new Button
                {
                    Text = "Bayar Sekarang",
                    Size = new Size(150, 45),
                    Location = new Point(200, 10),
                    BackColor = Color.ForestGreen,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TabIndex = 2
                };

                Button btnCancel = new Button
                {
                    Text = "Batal",
                    Size = new Size(100, 45),
                    Location = new Point(90, 10),
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = 3
                };

                btnCancel.Click += (s, e) => modal.Close();
                btnPay.Click += (s, e) =>
                {
                    string rawAmount = txtAmount.Text.Replace(".", "").Replace(",", "");
                    if (!decimal.TryParse(rawAmount, out decimal nominal) || nominal <= 0)
                    {
                        MessageBox.Show("Nominal bayar tidak valid.");
                        return;
                    }

                    try
                    {
                        // Misal userId ambil dari session
                        int userId = SessionUser.GetCurrentUser().UserId;
                        _transactionService.PayInstallment(transactionId, nominal, txtNote.Text, userId);

                        MessageBox.Show($"Berhasil bayar bon: {nominal:N0}");
                        modal.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                };

                pnlButtons.Controls.Add(btnPay);
                pnlButtons.Controls.Add(btnCancel);

                modal.Controls.Add(txtNote);
                modal.Controls.Add(lblNote);
                modal.Controls.Add(spacer1);
                modal.Controls.Add(txtAmount);
                modal.Controls.Add(lblAmount);
                modal.Controls.Add(pnlButtons);

                modal.AcceptButton = btnPay;
                modal.CancelButton = btnCancel;

                txtAmount.Focus();
                modal.ShowDialog();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (ShortcutForm frm = new ShortcutForm())
            {
                frm.ShowDialog(this); // Modal
            }
        }
    }

}
