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
using POS_qu.services;
using POS_qu.DTO;

namespace POS_qu
{
    public partial class CasherNew : Form
    {
        private CartActivity cartrepo;
        private IActivityService activityService;
        private ILogger flogger = new FileLogger();
        private ILogger dlogger = new DbLogger();
        private InvoiceData _currentInvoice;
        private CartService _cartService;
        public CasherNew()
        {
            InitializeComponent();
        }

        private void CasherNew_Load(object sender, EventArgs e)
        {
            // ===============================
            // 🔹 FORM SETUP
            // ===============================
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            // this.FormBorderStyle = FormBorderStyle.None; // opsional full kiosk

            // ===============================
            // 🔹 DEPENDENCY & SERVICE INIT
            // ===============================
            cartrepo = new CartActivity();
            activityService = new ActivityService(flogger, dlogger);
            _cartService = new CartService(cartrepo, activityService);

            // ===============================
            // 🔹 SESSION
            // ===============================
            StartNewTransaction();

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
            dataGridViewCart4.KeyDown += DataGridViewCart4_KeyDown;

            // ===============================
            // 🔹 DATAGRIDVIEW STYLE
            // ===============================
            SetupCartGrid();
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
                Status = "Draft"
            };

            lblSessionCode.Text = _currentInvoice.CartSessionCode;
        }


        private void SetupCartGrid()
        {
            dataGridViewCart4.Dock = DockStyle.Fill;
            dataGridViewCart4.ScrollBars = ScrollBars.Vertical;

            dataGridViewCart4.AutoSize = false;
            dataGridViewCart4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCart4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridViewCart4.EnableHeadersVisualStyles = false;
            dataGridViewCart4.RowHeadersVisible = false;
            dataGridViewCart4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCart4.MultiSelect = false;
            dataGridViewCart4.ReadOnly = true;
            dataGridViewCart4.AllowUserToAddRows = false;
            dataGridViewCart4.AllowUserToResizeRows = false;

            // Background
            dataGridViewCart4.BackgroundColor = Color.WhiteSmoke;

            // Cell Style
            dataGridViewCart4.DefaultCellStyle.BackColor = Color.White;
            dataGridViewCart4.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewCart4.DefaultCellStyle.Font = new Font("Yu Gothic", 10, FontStyle.Regular);
            dataGridViewCart4.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dataGridViewCart4.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Header Style
            dataGridViewCart4.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            dataGridViewCart4.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewCart4.ColumnHeadersDefaultCellStyle.Font = new Font("Yu Gothic", 10, FontStyle.Bold);
            dataGridViewCart4.ColumnHeadersHeight = 40;
            dataGridViewCart4.ReadOnly = true;
            dataGridViewCart4.AllowUserToAddRows = false;
            dataGridViewCart4.AllowUserToDeleteRows = false;
            dataGridViewCart4.AllowUserToResizeRows = false;
            dataGridViewCart4.AllowUserToResizeColumns = false;
            dataGridViewCart4.MultiSelect = false;
            dataGridViewCart4.EditMode = DataGridViewEditMode.EditProgrammatically;

        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }


        private UnitVariant ShowVariantPicker(List<UnitVariant> variants)
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


                // =============================
                // PANEL BUTTONS
                // =============================
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

                form.ShowDialog();
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
                    selectedVariant = ShowVariantPicker(variants);
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

                // ✅ Render full struk di FlowLayoutPanel
                // ✅ Render full struk di FlowLayoutPanel
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
            var panel = new Panel
            {
                Width = flpInvoice.Width - 10,
                AutoSize = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0, 0, 0, 2) // spasi tipis antar item
            };

            // Nama item
            var lblName = new Label
            {
                Text = item.Name,
                Font = new Font("Consolas", 10, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Black
            };

            // Detail qty, unit, harga, diskon, pajak
            var lblDetail = new Label
            {
                Text = $"{item.Name} | {item.Qty} {(string.IsNullOrEmpty(item.UnitVariant) ? item.Unit : item.UnitVariant)} {item.Price:N0}  Diskon: {item.DiscountAmount:N0}  Pajak: {item.Tax:N0}",
                Font = new Font("Consolas", 9, FontStyle.Regular),
                AutoSize = true,
                ForeColor = Color.Black
            };

            // Total harga item
            var lblTotal = new Label
            {
                Text = $"  Total: {item.Total:N0}",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Black
            };

            // 🔹 Label Qty + Satuan (Kanan)
            var lblQty = new Label
            {
                Text = $"{item.Qty} {item.Unit}",
                Font = new Font("Consolas", 9, FontStyle.Regular),
                AutoSize = false,
                Width = 100,
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(panel.Width - 100, 0)
            };





            // Tambahkan label ke panel
            //panel.Controls.Add(lblName);
            //panel.Controls.Add(lblQty);
            panel.Controls.Add(lblDetail);
            //panel.Controls.Add(lblTotal);

            // Separator tipis antar item
            var separator = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Height = 1,
                Width = panel.Width,
                Margin = new Padding(0, 2, 0, 2)
            };

            // Tambahkan ke FlowLayoutPanel utama
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
            flpInvoice.Controls.Clear();

            // 1️⃣ Ambil setting struk
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

            // Helper: tambah label
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
                    AutoSize = false,               // wajib false supaya bisa align
                    TextAlign = align
                };

                flpInvoice.Controls.Add(lbl);
            }



            // === 2️⃣ Header toko ===
            if (showNamaToko && !string.IsNullOrEmpty(judul))
                AddLabel(judul, 11, true, null, ContentAlignment.MiddleCenter);

            if (showAlamat && !string.IsNullOrEmpty(alamat))
                AddLabel(alamat, 9, false, null, ContentAlignment.MiddleCenter);

            if (showTelepon && !string.IsNullOrEmpty(telepon))
                AddLabel(telepon, 9, false, null, ContentAlignment.MiddleCenter);

            AddLabel(new string('-', 32), 9, false, null, ContentAlignment.MiddleCenter);


            // === 3️⃣ Info transaksi ===
            AddLabel($"Terminal : {terminal}");
            AddLabel($"Kasir    : {user}");
            AddLabel($"Shift    : {shift}");
            AddLabel($"Tanggal  : {DateTime.Now:yyyy-MM-dd}");
            AddLabel($"Waktu    : {DateTime.Now:HH:mm:ss}");
            AddLabel(new string('-', 32));

            // === 4️⃣ Detail item ===
            foreach (var item in invoice.Items)
            {
                AddInvoiceItemToPanel(item); // pakai style tipis persis yang kamu punya
            }

            // === 5️⃣ Ringkasan totals ===
            AddLabel(new string('-', 32));
            AddLabel($"Subtotal".PadRight(20) + $"{invoice.Subtotal:N0}".PadLeft(10));
            AddLabel($"Item Discount".PadRight(20) + $"-{invoice.TotalDiscount:N0}".PadLeft(10));
            if (invoice.GlobalDiscountPercent > 0)
                AddLabel($"Global Disc ({invoice.GlobalDiscountPercent:N0}%)".PadRight(20) + $"-{invoice.GlobalDiscountValue:N0}".PadLeft(10));
            if (!string.IsNullOrEmpty(invoice.GlobalNote))
                AddLabel($"Note".PadRight(20) + $"{invoice.GlobalNote}");
            if (invoice.DeliveryAmount > 0)
                AddLabel($"Delivery".PadRight(20) + $"{invoice.DeliveryAmount:N0}".PadLeft(10));
            AddLabel(new string('-', 32));
            AddLabel($"Grand Total".PadRight(20) + $"{invoice.GrandTotal:N0}".PadLeft(10));

            // === 6️⃣ Info pembayaran ===
            if (decimal.TryParse(jumlahBayar, out decimal jmlBayar)) jumlahBayar = jmlBayar.ToString("N0");
            if (decimal.TryParse(kembalian, out decimal kemb)) kembalian = kemb.ToString("N0");

            AddLabel(new string('-', 32));
            AddLabel($"Status Bayar : {statusBayar}");
            AddLabel($"Metode Bayar : {metodeBayar}");
            AddLabel($"Jumlah Bayar : {jumlahBayar}");
            AddLabel($"Kembalian    : {kembalian}");
            AddLabel(new string('-', 32));

            // === 7️⃣ Footer ===
            if (showFooter && !string.IsNullOrEmpty(footer))
                AddLabel(footer, 9, false, Color.Gray, ContentAlignment.MiddleCenter);

        }


        private void UpdateInvoiceSummary()
        {
            lblTotal.Text = _currentInvoice.GrandTotal.ToString();
            labelNumOfItems.Text = _currentInvoice.Items.Sum(x => x.Qty).ToString();
            lblKembalian.Text = _currentInvoice.ChangeAmount.ToString();
        }


        private void RenderInvoice(InvoiceData invoice)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = invoice.Items.Select(i => new
            {
                i.Name,
                i.Qty,
                Satuan = string.IsNullOrEmpty(i.UnitVariant) ? i.Unit : i.UnitVariant,
                Harga = i.Price,
                Total = i.Total
            }).ToList();


            dataGridViewCart4.DataSource = bs;
            // Optional: atur format kolom
            dataGridViewCart4.Columns["Harga"].DefaultCellStyle.Format = "N0";
            dataGridViewCart4.Columns["Total"].DefaultCellStyle.Format = "N0";
            labelNumOfItems.Text = invoice.Items.Sum(x => x.Qty).ToString();
        }

        private void dataGridViewCart4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridViewCart4.Rows[e.RowIndex];
            string itemName = row.Cells["Name"].Value.ToString();

            // Cari item dari _currentInvoice
            var item = _currentInvoice.Items.FirstOrDefault(x => x.Name == itemName);
            if (item == null) return;

            // Buka modal
            var modal = new FormUpdateItem(_currentInvoice, item, _cartService, updatedInvoice =>
            {

                // Update _currentInvoice
                _currentInvoice = updatedInvoice;

                // Refresh DataGridView & FlowLayoutPanel (opsional)
                RenderInvoice(_currentInvoice);
                // Refresh FlowLayoutPanel
                RefreshInvoicePanel();
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
                string itemName = row.Cells["Name"].Value.ToString();

                // Cari item di _currentInvoice
                var item = _currentInvoice.Items.FirstOrDefault(x => x.Name == itemName);
                if (item == null) return;

                // Buka modal
                var modal = new FormUpdateItem(_currentInvoice, item, _cartService, updatedInvoice =>
                {
                    RefreshInvoicePanel();
                    _currentInvoice = updatedInvoice;

                    if (_currentInvoice.Items.Any())
                    {
                        var lastItem = _currentInvoice.Items.Last();
                        AddInvoiceItemToPanel(lastItem);
                    }


                    // ✅ Render full struk di FlowLayoutPanel
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
                    // Refresh DataGridView & FlowLayoutPanel

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
        private TransactionService _transactionService;
        private void button1_Click(object sender, EventArgs e)
        {

            repo = new TransactionRepo();

            using (PaymentModalForm paymentModal =
                new PaymentModalForm(_currentInvoice.GrandTotal))
            {
                _transactionService = new TransactionService(repo, activityService);

                // 🔹 Realtime preview (UI only)
                paymentModal.PaymentAmountChanged += (amount) =>
                {
                    decimal change = _transactionService
                        .CalculateChangePreview(_currentInvoice, amount);

                    lblKembalian.Text = change.ToString("N0");
                };

                if (paymentModal.ShowDialog() != DialogResult.OK)
                    return;

                // Update invoice dari modal
                _currentInvoice.GlobalDiscountPercent = paymentModal.GlobalDiscountPercent;
                _currentInvoice.DeliveryAmount = paymentModal.DeliveryAmount;
                _currentInvoice.GlobalNote = paymentModal.GlobalNote;

                // 🔹 PROCESS + SAVE + LOG
                var result = _transactionService.ProcessPaymentAndSave(
                    _currentInvoice,
                    paymentModal.PaymentAmount,
                    paymentModal.PaymentMethod
                );

                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

                //if (result.IsSuccess)
                //{
                //    PrintInvoice(invoice);
                //}
                //else
                //{
                //    ShowError(result.Message);
                //}

                // 2️⃣ Reset invoice untuk transaksi baru
                _currentInvoice = new InvoiceData();

                // 3️⃣ Clear UI
                RenderInvoice(_currentInvoice);
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
                StartNewTransaction();
                MessageBox.Show("Payment successful!");
            }
        }


        private (string customer, string note)? ShowDraftInfoModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "Informasi Draft";
                modal.Width = 450;
                modal.Height = 270;
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;

                Label lblCustomer = new Label
                {
                    Text = "Nama Customer",
                    Dock = DockStyle.Top,
                    TabIndex = 0,
                    TabStop = false // label tidak perlu fokus
                };

                TextBox txtCustomer = new TextBox
                {
                    Dock = DockStyle.Top,
                    TabIndex = 1
                };

                Label lblNote = new Label
                {
                    Text = "Catatan",
                    Dock = DockStyle.Top,
                    TabIndex = 2,
                    TabStop = false
                };

                TextBox txtNote = new TextBox
                {
                    Dock = DockStyle.Top,
                    TabIndex = 3,
                    Multiline = true,
                    Height = 60
                };

                Button btnOk = new Button
                {
                    Text = "Simpan Draft",
                    Dock = DockStyle.Bottom,
                    TabIndex = 4
                };

                Button btnCancel = new Button
                {
                    Text = "Batal",
                    Dock = DockStyle.Bottom,
                    TabIndex = 5
                };


                (string, string)? result = null;

                btnOk.Click += (s, e) =>
                {
                    result = (txtCustomer.Text.Trim(), txtNote.Text.Trim());
                    modal.Close();
                };

                btnCancel.Click += (s, e) => modal.Close();

                modal.Controls.Add(txtNote);
                modal.Controls.Add(lblNote);
                modal.Controls.Add(txtCustomer);
                modal.Controls.Add(lblCustomer);
                modal.Controls.Add(btnOk);
                modal.Controls.Add(btnCancel);

                // 🔥 letakkan di sini
                modal.AcceptButton = btnOk;     // Enter = klik Simpan
                modal.CancelButton = btnCancel; // Esc = klik Batal
                txtCustomer.Focus();            // fokus awal di customer

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
            var drafts = _cartService.GetDraftOrders();

            if (drafts == null || drafts.Count == 0)
            {
                MessageBox.Show("Tidak ada draft.");
                return null;
            }

            int? selectedPoId = null;

            using (Form modal = new Form())
            {
                modal.Text = "Daftar Draft";
                modal.Width = 800;
                modal.Height = 500;
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;

                TextBox txtSearch = new TextBox
                {
                    Dock = DockStyle.Top,
                    PlaceholderText = "Cari total / tanggal..."
                };

                Label lblTotal = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 25,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                DataGridView grid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                };

                grid.Columns.Add("PoId", "ID");
                grid.Columns.Add("Customer", "Customer");
                grid.Columns.Add("Note", "Catatan");
                grid.Columns.Add("Total", "Total");
                grid.Columns.Add("CreatedAt", "Tanggal");


                void LoadGrid(List<PendingOrderDto> source)
                {
                    grid.Rows.Clear();

                    foreach (var d in source)
                    {
                        grid.Rows.Add(
                            d.PoId,
                            d.CustomerName ?? "-",
                            d.Note ?? "-",
                            d.Total.ToString("N0"),
                            d.CreatedAt.ToString("dd-MM-yyyy HH:mm")
                        );
                    }

                    lblTotal.Text = $"Total Draft: {source.Count}";
                }

                LoadGrid(drafts);

                // ===== SEARCH =====
                txtSearch.TextChanged += (s, e) =>
                {
                    string key = txtSearch.Text.ToLower();

                    var filtered = drafts
                        .Where(x =>
                            (x.CustomerName ?? "").ToLower().Contains(key) ||
                            (x.Note ?? "").ToLower().Contains(key) ||
                            x.Total.ToString().Contains(key) ||
                            x.CreatedAt.ToString("dd-MM-yyyy HH:mm").ToLower().Contains(key)
                        )
                        .ToList();

                    LoadGrid(filtered);
                };


                // ===== SORT HEADER =====
                // ===== SORT HEADER =====
                grid.ColumnHeaderMouseClick += (s, e) =>
                {
                    switch (e.ColumnIndex)
                    {
                        case 1: // Customer
                            drafts = drafts.OrderBy(x => x.CustomerName).ToList();
                            break;

                        case 3: // Total
                            drafts = drafts.OrderByDescending(x => x.Total).ToList();
                            break;

                        case 4: // Tanggal
                            drafts = drafts.OrderByDescending(x => x.CreatedAt).ToList();
                            break;
                    }

                    LoadGrid(drafts);
                };
                Button btnLoad = new Button
                {
                    Text = "Load Draft",
                    Dock = DockStyle.Bottom,
                    Height = 40
                };

                btnLoad.Click += (s, ev) =>
                {
                    if (grid.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Pilih draft dulu.");
                        return;
                    }

                    selectedPoId = Convert.ToInt32(grid.SelectedRows[0].Cells[0].Value);
                    modal.Close();
                };

                Button btnCancel = new Button
                {
                    Text = "Batal",
                    Dock = DockStyle.Bottom,
                    Height = 35
                };

                btnCancel.Click += (s, ev) => modal.Close();

                modal.Controls.Add(grid);
                modal.Controls.Add(lblTotal);
                modal.Controls.Add(txtSearch);
                modal.Controls.Add(btnLoad);
                modal.Controls.Add(btnCancel);

                modal.AcceptButton = btnLoad;
                modal.CancelButton = btnCancel;
                txtSearch.Focus();
                modal.ShowDialog();
            }

            return selectedPoId;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = ShowInstallmentModal();
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

            MessageBox.Show("Cicilan pertama berhasil disimpan");

            _currentInvoice = new InvoiceData();
            RenderInvoice(_currentInvoice);
            StartNewTransaction();
        }



        private (decimal Amount, string Customer, string Note)? ShowInstallmentModal()
        {
            decimal amount = 0;
            string customer = null;
            string note = null;

            using (Form modal = new Form())
            {
                modal.Text = "Pembayaran Cicilan";
                modal.Width = 400;
                modal.Height = 320;
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;

                Label lblCustomer = new Label { Text = "Nama Customer", Dock = DockStyle.Top };
                TextBox txtCustomer = new TextBox { Dock = DockStyle.Top };

                Button btnPickCustomer = new Button
                {
                    Text = "Pilih Customer",
                    Dock = DockStyle.Top,
                    Height = 30
                };
                btnPickCustomer.Click += (s, e) =>
                {
                    var customerDto = ShowCustomerPicker();
                    if (customerDto != null)
                        txtCustomer.Text = customerDto.Name;
                };

                Label lblAmount = new Label { Text = "Nominal Bayar", Dock = DockStyle.Top };
                TextBox txtAmount = new TextBox { Dock = DockStyle.Top };

                Label lblNote = new Label { Text = "Catatan", Dock = DockStyle.Top };
                TextBox txtNote = new TextBox { Dock = DockStyle.Top };

                Button btnSave = new Button
                {
                    Text = "Simpan Pembayaran",
                    Dock = DockStyle.Bottom,
                    Height = 40
                };

                Button btnCancel = new Button
                {
                    Text = "Batal",
                    Dock = DockStyle.Bottom
                };

                modal.AcceptButton = btnSave;
                modal.CancelButton = btnCancel;

                btnCancel.Click += (s, e) => modal.Close();

                btnSave.Click += (s, e) =>
                {
                    if (!decimal.TryParse(txtAmount.Text, out amount))
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

                modal.Controls.Add(txtNote);
                modal.Controls.Add(lblNote);
                modal.Controls.Add(txtAmount);
                modal.Controls.Add(lblAmount);
                modal.Controls.Add(btnPickCustomer);
                modal.Controls.Add(txtCustomer);
                modal.Controls.Add(lblCustomer);
                modal.Controls.Add(btnSave);
                modal.Controls.Add(btnCancel);

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
                modal.Text = "Pilih Customer";
                modal.Width = 350;
                modal.Height = 400;
                modal.StartPosition = FormStartPosition.CenterParent;

                ListBox list = new ListBox
                {
                    Dock = DockStyle.Fill,
                    DisplayMember = "Name",   // tampilkan nama
                    ValueMember = "Id"        // simpan id
                };

                void LoadCustomers()
                {
                    list.DataSource = null;
                    list.DataSource = _customerService.GetAll();
                }

                LoadCustomers();

                Button btnAdd = new Button
                {
                    Text = "Tambah Baru",
                    Dock = DockStyle.Bottom
                };

                Button btnOk = new Button
                {
                    Text = "Pilih",
                    Dock = DockStyle.Bottom
                };

                btnAdd.Click += (s, e) =>
                {
                    string name = Microsoft.VisualBasic.Interaction.InputBox(
                        "Nama customer:",
                        "Tambah Customer"
                    );

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        _customerService.Insert(name);
                        LoadCustomers();
                    }
                };

                btnOk.Click += (s, e) =>
                {
                    if (list.SelectedItem == null) return;

                    selectedCustomer = (CustomerDto)list.SelectedItem;
                    modal.Close();
                };

                modal.Controls.Add(list);
                modal.Controls.Add(btnOk);
                modal.Controls.Add(btnAdd);

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

                // 1️⃣ Ambil semua transaksi yang punya cicilan (ringkasan)
                var allInstallments = _transactionService.GetAllInstallments();

                if (allInstallments.Count == 0)
                {
                    MessageBox.Show("Belum ada cicilan.");
                    return;
                }

                // 2️⃣ Tampilkan daftar transaksi di grid
                using (Form modal = new Form())
                {
                    modal.Text = "Daftar Transaksi Cicilan";
                    modal.Width = 800;
                    modal.Height = 450;
                    modal.StartPosition = FormStartPosition.CenterParent;

                    DataGridView grid = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        ReadOnly = true,
                        AutoGenerateColumns = false,
                        AllowUserToAddRows = false
                    };

                    // Tambahkan kolom ringkas
                    grid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "ID Transaksi",
                        DataPropertyName = "TransactionId",
                        Width = 80
                    });
                    grid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "No. Transaksi",
                        DataPropertyName = "TransactionNumber",
                        Width = 120
                    });
                    grid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Kode",
                        DataPropertyName = "TransactionCode",
                        Width = 120
                    });
                    grid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Total Transaksi",
                        DataPropertyName = "TotalAmount",
                        Width = 120,
                        DefaultCellStyle = { Format = "N0" }
                    });
                    grid.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        HeaderText = "Sisa Bayar",
                        DataPropertyName = "DueAmount",
                        Width = 120,
                        DefaultCellStyle = { Format = "N0" }
                    });

                    // Grup data agar setiap transaksi hanya muncul sekali
                    var transactions = allInstallments
                        .GroupBy(i => i.TransactionId)
                        .Select(g => g.First())
                        .ToList();

                    grid.DataSource = transactions;

                    // 3️⃣ Event klik row → tampilkan detail cicilan transaksi itu
                    grid.CellDoubleClick += (s, ev) =>
                    {
                        if (ev.RowIndex < 0) return;

                        var selected = (InstallmentDto)grid.Rows[ev.RowIndex].DataBoundItem;
                        ShowTransactionInstallments(selected.TransactionId, selected.TransactionNumber);
                    };

                    modal.Controls.Add(grid);
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
            var installments = _transactionService.GetInstallments(transactionId); // cicilan transaksi ini

            if (installments.Count == 0)
            {
                MessageBox.Show("Belum ada cicilan untuk transaksi ini.");
                return;
            }

            decimal totalPaid = installments.Sum(i => i.Amount);
            decimal totalDue = installments.First().DueAmount; // sisa cicilan dari ts_due_amount

       
            using (Form modal = new Form())
            {
                modal.Text = $"Detail Cicilan Transaksi #{transactionNumber}";
                modal.Width = 800;
                modal.Height = 500;
                modal.StartPosition = FormStartPosition.CenterParent;

                // =============================
                // GRID CICILAN
                // =============================
                DataGridView grid = new DataGridView
                {
                    Dock = DockStyle.Top,
                    Height = 350,
                    ReadOnly = true,
                    AutoGenerateColumns = false,
                    AllowUserToAddRows = false,
                    DataSource = installments
                };

                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Nominal Cicilan",
                    DataPropertyName = "Amount",
                    Width = 120,
                    DefaultCellStyle = { Format = "N0" }
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Catatan Cicilan",
                    DataPropertyName = "Note",
                    Width = 200
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Dibuat Oleh",
                    DataPropertyName = "CreatedByName",
                    Width = 120
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Tanggal Cicilan",
                    DataPropertyName = "CreatedAt",
                    Width = 150
                });

                // =============================
                // LABEL TOTAL
                // =============================
                Label lblSummary = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 30,
                    Text = $"Total Dibayar: {totalPaid:N0}, Sisa Cicilan: {totalDue:N0}",
                    TextAlign = ContentAlignment.MiddleLeft
                };


                // =============================
                // BUTTON PANEL
                // =============================
                Panel pnlButtons = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 50
                };

                Button btnPay = new Button
                {
                    Text = "Bayar Cicilan",
                    Width = 120,
                    Left = 10,
                    Top = 10
                };
                btnPay.Click += (s, e) =>
                {
                    modal.Close(); // tutup modal detail
                                   // panggil form bayar cicilan, misal:
                    ShowPaymentForm(transactionId, totalDue);
                };

                Button btnClose = new Button
                {
                    Text = "Tutup",
                    Width = 120,
                    Left = 140,
                    Top = 10
                };
                btnClose.Click += (s, e) => modal.Close();

                pnlButtons.Controls.Add(btnPay);
                pnlButtons.Controls.Add(btnClose);

                // =============================
                // TAMBAHKAN SEMUA KE MODAL
                // =============================
                modal.Controls.Add(grid);
                modal.Controls.Add(lblSummary);
                modal.Controls.Add(pnlButtons);

                modal.ShowDialog();
            }
        }

        private void ShowPaymentForm(int transactionId, decimal dueAmount)
        {
            using (Form modal = new Form())
            {
                modal.Text = $"Bayar Cicilan Transaksi #{transactionId}";
                modal.Width = 400;
                modal.Height = 250;
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;
                modal.MinimizeBox = false;

                // =============================
                // LABEL DAN TEXTBOX NOMINAL
                // =============================
                Label lblAmount = new Label
                {
                    Text = $"Jumlah Bayar (Sisa: {dueAmount:N0}):",
                    Left = 20,
                    Top = 20,
                    Width = 200
                };
                TextBox txtAmount = new TextBox
                {
                    Left = 20,
                    Top = 50,
                    Width = 200,
                    Text = dueAmount.ToString("N0")
                };

                // =============================
                // LABEL DAN TEXTBOX CATATAN
                // =============================
                Label lblNote = new Label
                {
                    Text = "Catatan:",
                    Left = 20,
                    Top = 90,
                    Width = 100
                };
                TextBox txtNote = new TextBox
                {
                    Left = 20,
                    Top = 120,
                    Width = 340
                };

                // =============================
                // BUTTON PANEL
                // =============================
                Button btnPay = new Button
                {
                    Text = "Bayar",
                    Width = 100,
                    Height = 50,
                    Left = 20,
                    Top = 160
                };

                Button btnCancel = new Button
                {
                    Text = "Batal",
                    Width = 100,
                    Height = 50,
                    Left = 140,
                    Top = 160
                };

                btnCancel.Click += (s, e) => modal.Close();
                btnPay.Click += (s, e) =>
                {
                    decimal nominal;
                    if (!decimal.TryParse(txtAmount.Text, out nominal) || nominal <= 0)
                    {
                        MessageBox.Show("Nominal bayar tidak valid.");
                        return;
                    }

                    try
                    {
                        // Misal userId ambil dari session
                        int userId = SessionUser.GetCurrentUser().UserId;
                        _transactionService.PayInstallment(transactionId, nominal, txtNote.Text, userId);

                        MessageBox.Show($"Bayar cicilan: {nominal:N0}\nCatatan: {txtNote.Text}");
                        modal.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                };


                modal.Controls.Add(lblAmount);
                modal.Controls.Add(txtAmount);
                modal.Controls.Add(lblNote);
                modal.Controls.Add(txtNote);
                modal.Controls.Add(btnPay);
                modal.Controls.Add(btnCancel);

                modal.ShowDialog();
            }
        }














    }

}
