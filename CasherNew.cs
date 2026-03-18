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
            btnOpenCashier.Click += btnOpenCashier_Click;
            btnCloseCashier.Click += btnCloseCashier_Click;
            var btnPending = this.Controls.Find("btnPendingList", true);
            if (btnPending != null && btnPending.Length > 0 && btnPending[0] is Button)
            {
                ((Button)btnPending[0]).Click += btnPendingList_Click;
            }
            var btnCustom = this.Controls.Find("btnCustomTransaction", true).FirstOrDefault() as Button;
            if (btnCustom == null)
            {
                btnCustom = new Button
                {
                    Name = "btnCustomTransaction",
                    Text = "Custom Transaction",
                    Width = 160,
                    Height = 36
                };
                if (btnOpenCashier != null)
                {
                    btnCustom.Left = btnOpenCashier.Left;
                    btnCustom.Top = btnOpenCashier.Bottom + 8;
                }
                else
                {
                    btnCustom.Left = 20;
                    btnCustom.Top = 12;
                }
                btnCustom.Click += BtnCustomTransaction_Click;
                this.Controls.Add(btnCustom);
                btnCustom.BringToFront();
            }

            // ===============================
            // 🔹 DATAGRIDVIEW STYLE
            // ===============================
            SetupCartGrid();

            PromptOpenShiftIfNeeded();
            PromptResumeCartIfAny();
            UpdateShiftInfoUI();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.K:
                    OpenSearchShortcut();
                    return true;

                case Keys.F1:
                    ShowShortcutsHelp();
                    return true;

                case Keys.Control | Keys.S:
                    txtCariBarang.Focus();
                    return true;

                case Keys.Control | Keys.F:
                    // SearchAndAddItem();
                    return true;

                case Keys.Control | Keys.I:
                    // BtnToggleInfo.PerformClick();
                    return true;

                case Keys.Control | Keys.Shift | Keys.O:
                    OpenShift();
                    return true;

                case Keys.Control | Keys.Shift | Keys.C:
                    CloseShift();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
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
                _currentInvoice = _cartService.AddItemByName(_currentInvoice, selectedItem.name, 1);
            }


            //_currentInvoice = _cartService.AddItemByName(_currentInvoice, search.SelectedItem.name, 1);
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
            MessageBox.Show("Fitur Close Shift sementara tidak tersedia.", "Info");
        }

        private void btnOpenCashier_Click(object sender, EventArgs e) => OpenShift();
        private void btnCloseCashier_Click(object sender, EventArgs e) => CloseShift();
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
                Text = $"{item.Name} | {item.Qty} {item.Unit} {item.Price:N0}  Diskon: {item.DiscountAmount:N0}  Pajak: {item.Tax:N0}",
                Font = new Font("Consolas", 9, FontStyle.Regular),
                AutoSize = true,
                ForeColor = Color.Black
            };

            var lblTotal = new Label
            {
                Text = $"  Total: {item.Total:N0}",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Black
            };

            var lblQty = new Label
            {
                Text = $"{item.Qty} {item.Unit}",
                Font = new Font("Consolas", 9, FontStyle.Regular),
                AutoSize = false,
                Width = 100,
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(panel.Width - 100, 0)
            };


            panel.Controls.Add(lblDetail);

            var separator = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Height = 1,
                Width = panel.Width,
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
            if (invoice.GlobalDiscountPercent > 0)
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
            lblTotal.Text = _currentInvoice.GrandTotal.ToString("N0");
            labelNumOfItems.Text = _currentInvoice.Items.Sum(x => x.Qty).ToString();
            lblKembalian.Text = _currentInvoice.ChangeAmount.ToString("N0");
        }


        private void RenderInvoice(InvoiceData invoice)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = invoice.Items.Select(i => new
            {
                i.Name,
                i.Qty,
                Satuan =  i.Unit ,
                Harga = i.Price,
                Total = i.Total
            }).ToList();


            dataGridViewCart4.DataSource = bs;
            dataGridViewCart4.Columns["Harga"].DefaultCellStyle.Format = "N0";
            dataGridViewCart4.Columns["Total"].DefaultCellStyle.Format = "N0";
            labelNumOfItems.Text = invoice.Items.Sum(x => x.Qty).ToString();
        }

        private void dataGridViewCart4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridViewCart4.Rows[e.RowIndex];
            string itemName = row.Cells["Name"].Value.ToString();

            var item = _currentInvoice.Items.FirstOrDefault(x => x.Name == itemName);
            if (item == null) return;

            var modal = new FormUpdateItem(_currentInvoice, item, _cartService, updatedInvoice =>
            {

                _currentInvoice = updatedInvoice;

                RenderInvoice(_currentInvoice);
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

                TransactionResult result;
                if (paymentModal.IsSplitPayment && paymentModal.SplitPayments != null)
                {
                    var parts = paymentModal.SplitPayments.ToList();
                    result = _transactionService.ProcessSplitPaymentAndSave(
                        _currentInvoice,
                        parts
                    );
                }
                else
                {
                    // 🔹 PROCESS + SAVE + LOG
                    result = _transactionService.ProcessPaymentAndSave(
                        _currentInvoice,
                        paymentModal.PaymentAmount,
                        paymentModal.PaymentMethod
                    );
                }

                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

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
                MessageBox.Show("Payment successful!");
            }
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
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    Font = new Font("Segoe UI", 10),
                    TabIndex = 1
                };
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
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    Font = new Font("Segoe UI", 10),
                    TabIndex = 2
                };

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
                        RowHeadersVisible = false,
                        Font = new Font("Segoe UI", 10),
                        TabIndex = 0
                    };

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
                    RowHeadersVisible = false,
                    Font = new Font("Segoe UI", 10),
                    TabIndex = 2
                };

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
