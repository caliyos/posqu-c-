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
            using (Form form = new Form())
            {
                form.Text = "Pilih Satuan";
                form.Width = 350;
                form.Height = 300;
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var listBox = new ListBox()
                {
                    Dock = DockStyle.Top,
                    Height = 200
                };

                foreach (var v in variants)
                {
                    listBox.Items.Add(v);
                }

                listBox.DisplayMember = "UnitName";

                var btnOk = new Button()
                {
                    Text = "Pilih",
                    Dock = DockStyle.Bottom,
                    Height = 40
                };

                btnOk.Click += (s, e) =>
                {
                    if (listBox.SelectedItem != null)
                    {
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                };

                form.Controls.Add(listBox);
                form.Controls.Add(btnOk);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    return listBox.SelectedItem as UnitVariant;
                }

                return null; // user batal
            }
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
                Text = $"{item.Name} | {item.Qty} x {item.Price:N0}  Diskon: {item.DiscountAmount:N0}  Pajak: {item.Tax:N0}",
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
                Harga = i.Price,
                Total = i.Total
            }).ToList();


            dataGridViewCart4.DataSource = bs;
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
                var modal = new FormUpdateItem(_currentInvoice,item, _cartService, updatedInvoice =>
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

        private  TransactionRepo repo;
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




















    }




}
