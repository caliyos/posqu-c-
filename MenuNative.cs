﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using Npgsql;
using POS_qu;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSqu_menu
{
    public partial class MenuNative : Form
    {

        // Tambahkan field baru di dalam kelas MenuNative
        private Label labelMarquee;
        private System.Windows.Forms.Timer marqueeTimer;
        private string marqueeText;
        private int marqueeX;


        // Tambahkan method ini, misal di bawah MenuNative_Load atau di kelas
        private void InitializeMarquee()
        {
            try
            {
                var user = SessionUser.GetCurrentUser();
                if (user == null) return;

                // Setup label marquee
                labelMarquee = new Label()
                {
                    AutoSize = false, // supaya bisa full width
                    ForeColor = Color.White,
                    BackColor = Color.IndianRed,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Height = 30, // tinggi label
                    Dock = DockStyle.Bottom,
                    TextAlign = ContentAlignment.MiddleLeft // teks start dari kiri
                };
                this.Controls.Add(labelMarquee);
                labelMarquee.BringToFront(); // pastikan selalu di atas semua kontrol lain


                // Set initial marquee text
                UpdateMarqueeText();

                // Start X posisi di kanan layar
                marqueeX = this.ClientSize.Width;
                marqueeX = labelMarquee.Width; // start dari kanan
                // Setup timer
                marqueeTimer = new System.Windows.Forms.Timer
                {
                    Interval = 50 // kecepatan scroll dalam ms
                };
                marqueeTimer.Tick += MarqueeTimer_Tick;
                marqueeTimer.Start();
            }
            catch { /* ignore error */ }
        }




        // Update marquee text
        private void UpdateMarqueeText()
        {
            try
            {
                var user = SessionUser.GetCurrentUser();
                if (user == null) return;

                string dbTimeZone = GetDatabaseTimeZone() ?? "Asia/Makassar"; // fallback kalau gagal

                DateTime utcNow = DateTime.UtcNow;
                DateTime localTime;
                try
                {
                    localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, TimeZoneInfo.FindSystemTimeZoneById(dbTimeZone));
                }
                catch
                {
                    localTime = DateTime.Now; // fallback ke local machine time jika error IANA
                }

                marqueeText = $"👤 {user.Username} | 🎭 {user.RoleName} | 🖥️ {user.TerminalName} | ⏰ {localTime:HH:mm:ss dd/MM/yyyy}";
                labelMarquee.Text = marqueeText;
            }
            catch { /* abaikan error timer */ }
        }

        /// <summary>
        /// Ambil timezone dari database PostgreSQL
        /// </summary>
        private string GetDatabaseTimeZone()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("SHOW TIME ZONE;", conn);
                return cmd.ExecuteScalar()?.ToString();
            }
            catch
            {
                return null;
            }
        }

        // Timer tick event
        private void MarqueeTimer_Tick(object sender, EventArgs e)
        {
            marqueeX -= 2; // geser ke kiri
            labelMarquee.Text = marqueeText;

            // Untuk scroll efek, kita bisa pakai padding
            labelMarquee.Padding = new Padding(marqueeX, 0, 0, 0);

            if (marqueeX + TextRenderer.MeasureText(marqueeText, labelMarquee.Font).Width < 0)
            {
                marqueeX = labelMarquee.Width;
            }

            UpdateMarqueeText();
        }


        public MenuNative()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.Load += MenuNative_Load;
            //InitializeMarquee();
        }

        
        //    public MenuNative()
        //{
        //    MessageBox.Show("CTOR 1");
        //    InitializeComponent();
        //    MessageBox.Show("CTOR 2");

        //    this.Load += MenuNative_Load;

        //}

        private void MenuNative_Load(object sender, EventArgs e)
        {
            try
            {
                var user = SessionUser.GetCurrentUser();
                string pcId = Utility.GetPcId();

                if (user == null)
                {
                    label2.Text = "⚠️ Tidak ada sesi pengguna aktif.";
                    return;
                }

                GlobalContext.RefreshConnectionInfo();
                string dbSummary = !string.IsNullOrWhiteSpace(GlobalContext.ConnectionSummary) ? GlobalContext.ConnectionSummary : "-";

                string appTz = DbConfig.AppTimeZone ?? "";
                string dbTz = GetDatabaseTimeZone() ?? "";
                string tzLine = "";
                if (!string.IsNullOrWhiteSpace(appTz) || !string.IsNullOrWhiteSpace(dbTz))
                    tzLine = $"\n🕒 TimeZone App: {(!string.IsNullOrWhiteSpace(appTz) ? appTz : "-")} | DB: {(!string.IsNullOrWhiteSpace(dbTz) ? dbTz : "-")}";

                // tampilkan semua data session user
                label2.Text =
                    $"👤 User: {user.Username} (ID: {user.UserId})\n" +
                    $"🎭 Role: {user.RoleName} (ID: {user.RoleId})\n" +
                    $"🖥️ Terminal: {user.TerminalName} (ID: {user.TerminalId})\n" +
                    $"⏰ Shift: {user.ShiftId}\n" +
                    $"💻 PC ID: {pcId}\n" +
                    $"🗄️ Database: {dbSummary}" +
                    tzLine;

                LoadDashboardSummary();
                RenderSalesBarsLast30Days();
                btnRefreshDashboard.Click += btnRefreshDashboard_Click;
                var btnAdminPendingField = panelWelcome.Controls["btnAdminPending"] as Button;
                if (btnAdminPendingField != null)
                    btnAdminPendingField.Click += (s, ev) =>
                    {
                        ShowPendingAdmin();
                    };
                // Menu item: Pending (Admin)
                var miPendingAdmin = FindMenuItemByName(menuStrip1.Items, "pendingTransaksiAdminToolStripMenuItem");
                if (miPendingAdmin != null)
                {
                    miPendingAdmin.Click += (s, ev) =>
                    {
                        ShowPendingAdmin();
                    };
                }
                var miRetur = FindMenuItemByName(menuStrip1.Items, "returBarangToolStripMenuItem");
                if (miRetur == null)
                {
                    // inject menu Retur Barang jika tidak ada di designer
                    var miCasher = FindMenuItemByName(menuStrip1.Items, "casherToolStripMenuItem");
                    if (miCasher != null)
                    {
                        miRetur = new ToolStripMenuItem
                        {
                            Name = "returBarangToolStripMenuItem",
                            Text = "Retur Barang"
                        };
                        miCasher.DropDownItems.Add(miRetur);
                    }
                }
                if (miRetur != null)
                    miRetur.Click += (s, ev) =>
                    {
                        using var frm = new ReturnListForm();
                        frm.ShowDialog(this);
                    };

                if (saldoAwalToolStripMenuItem != null)
                {
                    saldoAwalToolStripMenuItem.Visible = ShouldShowSaldoAwalMenu();
                }

                WireLocalFirstMenus();
                SetupReportsMenu();
            }
            catch (Exception ex)
            {
                label2.Text = "❌ Error memuat sesi: " + ex.Message;
            }
        }

        private void ShowPendingAdmin()
        {
            try
            {
                using var f = new PendingCartsAdminForm();
                f.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuka Pending Admin:\n" + ex.ToString());
            }
        }

        private void WireLocalFirstMenus()
        {
            var miLicense = FindMenuItemByName(menuStrip1.Items, "licensesToolStripMenuItem");
            if (miLicense != null)
            {
                miLicense.Click -= licensesToolStripMenuItem_Click_LocalFirst;
                miLicense.Click += licensesToolStripMenuItem_Click_LocalFirst;
            }

            var miReports = FindMenuItemByName(menuStrip1.Items, "reportsToolStripMenuItem");
            if (miReports != null)
            {
                var miPro = FindMenuItemByName(miReports.DropDownItems, "proFeatureLockedToolStripMenuItem");
                if (miPro == null)
                {
                    miPro = new ToolStripMenuItem
                    {
                        Name = "proFeatureLockedToolStripMenuItem",
                        Text = "Pro Feature (Locked)"
                    };
                    miReports.DropDownItems.Add(new ToolStripSeparator());
                    miReports.DropDownItems.Add(miPro);
                }
                miPro.Click -= proFeatureLockedToolStripMenuItem_Click_LocalFirst;
                miPro.Click += proFeatureLockedToolStripMenuItem_Click_LocalFirst;
            }

            var miSettings = FindMenuItemByName(menuStrip1.Items, "settingsToolStripMenuItem");
            if (miSettings != null)
            {
                var miSync = FindMenuItemByName(miSettings.DropDownItems, "syncNowToolStripMenuItem");
                if (miSync == null)
                {
                    miSync = new ToolStripMenuItem
                    {
                        Name = "syncNowToolStripMenuItem",
                        Text = "Sync Now"
                    };
                    miSettings.DropDownItems.Add(new ToolStripSeparator());
                    miSettings.DropDownItems.Add(miSync);
                }
                miSync.Click -= syncNowToolStripMenuItem_Click_LocalFirst;
                miSync.Click += syncNowToolStripMenuItem_Click_LocalFirst;
            }

            RefreshProFeatureMenuState();
        }

        private void licensesToolStripMenuItem_Click_LocalFirst(object sender, EventArgs e)
        {
            using var f = new POS_qu.LicenseActivationForm();
            f.ShowDialog(this);
            RefreshProFeatureMenuState();
        }

        private async void proFeatureLockedToolStripMenuItem_Click_LocalFirst(object sender, EventArgs e)
        {
            if (!await EnsureLicenseValidAsync())
                return;

            MessageBox.Show("Pro Feature terbuka (dummy).", "Pro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void syncNowToolStripMenuItem_Click_LocalFirst(object sender, EventArgs e)
        {
            try
            {
                await POS_qu.Helpers.SyncApiClient.SyncDummyItemsAsync("http://localhost:9555");
                MessageBox.Show("Sync OK", "Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> EnsureLicenseValidAsync()
        {
            var lic = await POS_qu.Helpers.LicenseManager.LoadAsync();
            if (POS_qu.Helpers.LicenseManager.IsValidNow(lic))
                return true;

            MessageBox.Show("Menu ini terkunci. Aktivasi license dulu.", "License", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            using var f = new POS_qu.LicenseActivationForm();
            f.ShowDialog(this);

            lic = await POS_qu.Helpers.LicenseManager.LoadAsync();
            var ok = POS_qu.Helpers.LicenseManager.IsValidNow(lic);
            RefreshProFeatureMenuState();
            return ok;
        }

        private async void RefreshProFeatureMenuState()
        {
            try
            {
                var miReports = FindMenuItemByName(menuStrip1.Items, "reportsToolStripMenuItem");
                if (miReports == null) return;
                var miPro = FindMenuItemByName(miReports.DropDownItems, "proFeatureLockedToolStripMenuItem");
                if (miPro == null) return;

                var lic = await POS_qu.Helpers.LicenseManager.LoadAsync();
                bool ok = POS_qu.Helpers.LicenseManager.IsValidNow(lic);
                miPro.Text = ok ? "Pro Feature" : "Pro Feature (Locked)";
            }
            catch
            {
            }
        }

        private bool ShouldShowSaldoAwalMenu()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT 1 FROM transactions WHERE deleted_at IS NULL LIMIT 1", conn);
                var v = cmd.ExecuteScalar();
                return v == null || v == DBNull.Value;
            }
            catch
            {
                return false;
            }
        }

        private void saldoAwalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new SaldoAwalForm();
            f.ShowDialog(this);
            if (saldoAwalToolStripMenuItem != null)
                saldoAwalToolStripMenuItem.Visible = ShouldShowSaldoAwalMenu();
        }

        private void printBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.BarcodePrintForm();
            f.ShowDialog(this);
        }

        private void stockOpnameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.StockOpnameForm();
            f.ShowDialog(this);
        }

        private void daftarStockOpnameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.StockOpnameListForm();
            f.ShowDialog(this);
        }

        private void daftarItemMasukToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.InventoryAdjustmentListForm(POS_qu.InventoryAdjustmentDirection.In);
            f.ShowDialog(this);
        }

        private void daftarItemKeluarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.InventoryAdjustmentListForm(POS_qu.InventoryAdjustmentDirection.Out);
            f.ShowDialog(this);
        }

        private void ubahHargaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.PriceUpdateForm();
            f.ShowDialog(this);
        }

        private void daftarProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.PromotionListForm();
            f.ShowDialog(this);
        }

        private void buatProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.PromotionProgramForm();
            f.ShowDialog(this);
        }

        private void historyHargaBeliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new POS_qu.BuyPriceHistoryForm();
            f.ShowDialog(this);
        }

        private void ruleSyaratToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Rule / Syarat: gunakan kolom Priority + Periode + Tipe pada Program untuk menentukan konflik.\nVersi berikutnya bisa ditambah rule per item/kategori.", "Info");
        }

        private void historyLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("History / Log promo belum diaktifkan.", "Info");
        }

        private ToolStripMenuItem FindMenuItemByName(ToolStripItemCollection items, string name)
        {
            foreach (ToolStripItem it in items)
            {
                if (it.Name == name) return it as ToolStripMenuItem;
                if (it is ToolStripMenuItem mi && mi.DropDownItems.Count > 0)
                {
                    var found = FindMenuItemByName(mi.DropDownItems, name);
                    if (found != null) return found;
                }
            }
            return null;
        }

        private void SetupReportsMenu()
        {
            var miReports = FindMenuItemByName(menuStrip1.Items, "reportsToolStripMenuItem");
            if (miReports == null) return;

            ToolStripMenuItem EnsureMenu(string name, string text)
            {
                var existing = FindMenuItemByName(miReports.DropDownItems, name);
                if (existing != null) return existing;
                var mi = new ToolStripMenuItem { Name = name, Text = text };
                miReports.DropDownItems.Add(mi);
                return mi;
            }

            void EnsureSeparator(string name)
            {
                foreach (ToolStripItem it in miReports.DropDownItems)
                {
                    if (it.Name == name) return;
                }
                miReports.DropDownItems.Add(new ToolStripSeparator { Name = name });
            }

            EnsureSeparator("sepReports1");

            EnsureMenu("miSalesToday", "Penjualan • Hari Ini").Click += (s, e) =>
            {
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var dt = QuerySalesByDay(start, end);
                ShowReportGrid("Penjualan • Hari Ini", dt);
            };

            EnsureMenu("miSalesPeriod", "Penjualan • Per Periode").Click += (s, e) =>
            {
                if (!TryAskDateRange("Penjualan • Per Periode", out var start, out var end)) return;
                var dt = QuerySalesByDay(start, end);
                ShowReportGrid("Penjualan • Per Periode", dt);
            };

            EnsureMenu("miSalesCashier", "Penjualan • Per Kasir").Click += (s, e) =>
            {
                if (!TryAskDateRange("Penjualan • Per Kasir", out var start, out var end)) return;
                var dt = QuerySalesByCashier(start, end);
                ShowReportGrid("Penjualan • Per Kasir", dt);
            };

            EnsureMenu("miSalesProduct", "Penjualan • Per Produk").Click += (s, e) =>
            {
                if (!TryAskDateRange("Penjualan • Per Produk", out var start, out var end)) return;
                var dt = QuerySalesByProduct(start, end, null);
                ShowReportGrid("Penjualan • Per Produk", dt);
            };

            EnsureMenu("miSalesTop", "Produk Terlaris • Per Periode").Click += (s, e) =>
            {
                if (!TryAskDateRange("Produk Terlaris • Per Periode", out var start, out var end)) return;
                var dt = QuerySalesByProduct(start, end, 50);
                ShowReportGrid("Produk Terlaris • Per Periode", dt);
            };

            EnsureMenu("miSalesCategory", "Penjualan • Per Kategori").Click += (s, e) =>
            {
                if (!TryAskDateRange("Penjualan • Per Kategori", out var start, out var end)) return;
                var dt = QuerySalesByCategory(start, end);
                ShowReportGrid("Penjualan • Per Kategori", dt);
            };

            EnsureSeparator("sepReportsCash");

            EnsureMenu("miCashRecap", "Kas • Rekap Cash/Transfer/QRIS").Click += (s, e) =>
            {
                if (!TryAskDateRange("Kas • Rekap Metode Pembayaran", out var start, out var end)) return;
                var dt = QueryPaymentRecap(start, end);
                ShowReportGrid("Kas • Rekap Metode Pembayaran", dt);
            };

            EnsureMenu("miShiftClosing", "Kas • Tutup Kasir (Closing Shift)").Click += (s, e) =>
            {
                if (!TryAskDateRange("Kas • Tutup Kasir (Closing Shift)", out var start, out var end)) return;
                var dt = QueryShiftClosings(start, end);
                ShowReportGrid("Kas • Tutup Kasir (Closing Shift)", dt);
            };

            EnsureMenu("miCashIn", "Kas • Kas Masuk").Click += (s, e) =>
            {
                MessageBox.Show("Kas Masuk belum tersedia (belum ada modul kas masuk/keluar).");
            };

            EnsureMenu("miCashOut", "Kas • Kas Keluar").Click += (s, e) =>
            {
                MessageBox.Show("Kas Keluar belum tersedia (belum ada modul kas masuk/keluar).");
            };

            EnsureSeparator("sepReports2");

            EnsureMenu("miStockNow", "Stok • Saat Ini").Click += (s, e) =>
            {
                var dt = QueryStockNow();
                ShowReportGrid("Stok • Saat Ini", dt);
            };

            EnsureMenu("miStockLow", "Stok • Barang Hampir Habis").Click += (s, e) =>
            {
                var dt = QueryStockLow();
                ShowReportGrid("Stok • Barang Hampir Habis", dt);
            };

            EnsureMenu("miStockMutation", "Stok • Mutasi / Riwayat").Click += (s, e) =>
            {
                if (!TryAskDateRange("Stok • Mutasi / Riwayat", out var start, out var end)) return;
                var dt = QueryStockLog(start, end);
                ShowReportGrid("Stok • Mutasi / Riwayat", dt);
            };

            EnsureSeparator("sepReports3");

            EnsureMenu("miPurchaseSupplier", "Pembelian • Riwayat per Supplier").Click += (s, e) =>
            {
                if (!TryAskDateRange("Pembelian • Riwayat per Supplier", out var start, out var end)) return;
                var dt = QueryPurchaseBySupplier(start, end);
                ShowReportGrid("Pembelian • Riwayat per Supplier", dt);
            };

            EnsureMenu("miGoodsIn", "Pembelian • Barang Masuk (Adjustment IN)").Click += (s, e) =>
            {
                if (!TryAskDateRange("Pembelian • Barang Masuk", out var start, out var end)) return;
                var dt = QueryGoodsIn(start, end);
                ShowReportGrid("Pembelian • Barang Masuk", dt);
            };

            EnsureSeparator("sepReports4");

            EnsureMenu("miCustomerHistory", "Customer • Riwayat Transaksi").Click += (s, e) =>
            {
                if (!TryAskDateRange("Customer • Riwayat Transaksi", out var start, out var end)) return;
                var dt = QueryCustomerTransactions(start, end);
                ShowReportGrid("Customer • Riwayat Transaksi", dt);
            };

            EnsureMenu("miCustomerTop", "Customer • Paling Aktif").Click += (s, e) =>
            {
                if (!TryAskDateRange("Customer • Paling Aktif", out var start, out var end)) return;
                var dt = QueryTopCustomers(start, end);
                ShowReportGrid("Customer • Paling Aktif", dt);
            };

            EnsureSeparator("sepReportsAudit");
            EnsureMenu("miAuditLogs", "Audit Logs").Click += (s, e) =>
            {
                ShowAuditLogsBrowser();
            };
        }

        private void ShowAuditLogsBrowser()
        {
            using var f = new Form();
            f.Text = "Audit Logs";
            f.StartPosition = FormStartPosition.CenterParent;
            f.Size = new Size(1200, 780);
            f.MinimizeBox = false;
            f.MaximizeBox = true;
            f.FormBorderStyle = FormBorderStyle.Sizable;

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 122, Padding = new Padding(12), BackColor = Color.White };
            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Text = "Audit Logs • Search"
            };

            var pnlFilters = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 2
            };
            pnlFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            pnlFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            pnlFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            pnlFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            pnlFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            pnlFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            pnlFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            pnlFilters.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            var lblFrom = new Label { Text = "Dari", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            var dtFrom = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };
            var lblTo = new Label { Text = "Sampai", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            var dtTo = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };
            var lblLimit = new Label { Text = "Limit", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            var cmbLimit = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLimit.Items.AddRange(new object[] { "200", "500", "1000", "2000" });
            cmbLimit.SelectedIndex = 1;

            var lblAction = new Label { Text = "Action", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            var txtAction = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "mis: Transaction_Complete / Cart_" };
            var lblUser = new Label { Text = "User ID", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            var txtUserId = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "opsional" };
            var lblRef = new Label { Text = "Ref ID", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            var txtRefId = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "ts_id / item_id / dll" };

            pnlFilters.Controls.Add(lblFrom, 0, 0);
            pnlFilters.Controls.Add(dtFrom, 1, 0);
            pnlFilters.Controls.Add(lblTo, 2, 0);
            pnlFilters.Controls.Add(dtTo, 3, 0);
            pnlFilters.Controls.Add(lblLimit, 4, 0);
            pnlFilters.Controls.Add(cmbLimit, 5, 0);

            pnlFilters.Controls.Add(lblAction, 0, 1);
            pnlFilters.Controls.Add(txtAction, 1, 1);
            pnlFilters.Controls.Add(lblUser, 2, 1);
            pnlFilters.Controls.Add(txtUserId, 3, 1);
            pnlFilters.Controls.Add(lblRef, 4, 1);
            pnlFilters.Controls.Add(txtRefId, 5, 1);

            pnlTop.Controls.Add(pnlFilters);
            pnlTop.Controls.Add(lblTitle);

            var pnlMid = new Panel { Dock = DockStyle.Top, Height = 54, Padding = new Padding(12), BackColor = Color.White };
            var txtKeyword = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "Kata kunci (description/details)...", Font = new Font("Segoe UI", 10F) };
            var btnSearch = new Button { Text = "Search", Dock = DockStyle.Right, Width = 120 };
            var btnDetail = new Button { Text = "Lihat Detail", Dock = DockStyle.Right, Width = 140 };
            pnlMid.Controls.Add(txtKeyword);
            pnlMid.Controls.Add(btnDetail);
            pnlMid.Controls.Add(btnSearch);

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White
            };

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 56, Padding = new Padding(12), BackColor = Color.WhiteSmoke };
            var lblCount = new Label { Dock = DockStyle.Left, Width = 520, TextAlign = ContentAlignment.MiddleLeft };
            var btnClose = new Button { Text = "Tutup", Dock = DockStyle.Right, Width = 120 };
            pnlBottom.Controls.Add(btnClose);
            pnlBottom.Controls.Add(lblCount);

            f.Controls.Add(grid);
            f.Controls.Add(pnlBottom);
            f.Controls.Add(pnlMid);
            f.Controls.Add(pnlTop);

            dtFrom.Value = DateTime.Today;
            dtTo.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

            string PrettyJson(string raw)
            {
                if (string.IsNullOrWhiteSpace(raw)) return "";
                var t = raw.Trim();
                if (t == "{}" || t == "[]") return t;
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(t);
                    return System.Text.Json.JsonSerializer.Serialize(doc.RootElement, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                }
                catch
                {
                    return raw;
                }
            }

            DataTable Query()
            {
                var from = dtFrom.Value;
                var to = dtTo.Value;
                if (to < from) to = from;
                int limit = 500;
                int.TryParse(cmbLimit.SelectedItem?.ToString(), out limit);
                if (limit <= 0) limit = 500;

                var action = (txtAction.Text ?? "").Trim();
                var keyword = (txtKeyword.Text ?? "").Trim();
                var userIdText = (txtUserId.Text ?? "").Trim();
                var refIdText = (txtRefId.Text ?? "").Trim();

                using var con = new NpgsqlConnection(DbConfig.ConnectionString);
                con.Open();

                var sql = new StringBuilder();
                sql.Append(@"
SELECT
    id,
    COALESCE(user_id,0) AS user_id,
    COALESCE(action,'') AS action,
    COALESCE(reference_id,0) AS reference_id,
    COALESCE(description,'') AS description,
    COALESCE(details,'') AS details,
    created_at
FROM audit_logs
WHERE created_at >= @from AND created_at <= @to
");

                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);

                if (!string.IsNullOrWhiteSpace(action))
                {
                    sql.Append(" AND action ILIKE @action ");
                    cmd.Parameters.AddWithValue("@action", "%" + action + "%");
                }

                if (long.TryParse(userIdText, out var uid) && uid > 0)
                {
                    sql.Append(" AND user_id = @uid ");
                    cmd.Parameters.AddWithValue("@uid", uid);
                }

                if (long.TryParse(refIdText, out var rid) && rid > 0)
                {
                    sql.Append(" AND reference_id = @rid ");
                    cmd.Parameters.AddWithValue("@rid", rid);
                }

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    sql.Append(" AND (COALESCE(description,'') ILIKE @kw OR COALESCE(details,'') ILIKE @kw) ");
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                }

                sql.Append(" ORDER BY created_at DESC, id DESC LIMIT " + limit);
                cmd.CommandText = sql.ToString();

                var dt = new DataTable();
                using var da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }

            void RefreshGrid()
            {
                try
                {
                    var dt = Query();
                    grid.DataSource = dt;
                    if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false;
                    if (grid.Columns.Contains("details")) grid.Columns["details"].Visible = false;
                    if (grid.Columns.Contains("created_at")) { grid.Columns["created_at"].HeaderText = "Waktu"; grid.Columns["created_at"].Width = 160; }
                    if (grid.Columns.Contains("user_id")) { grid.Columns["user_id"].HeaderText = "User"; grid.Columns["user_id"].Width = 80; }
                    if (grid.Columns.Contains("reference_id")) { grid.Columns["reference_id"].HeaderText = "Ref"; grid.Columns["reference_id"].Width = 90; }
                    if (grid.Columns.Contains("action")) { grid.Columns["action"].HeaderText = "Action"; grid.Columns["action"].Width = 240; }
                    if (grid.Columns.Contains("description")) { grid.Columns["description"].HeaderText = "Description"; grid.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }
                    lblCount.Text = $"Rows: {dt.Rows.Count:N0}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Gagal load audit logs", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            void ShowSelected()
            {
                try
                {
                    if (grid.SelectedRows.Count == 0) return;
                    var r = grid.SelectedRows[0];
                    var action = r.Cells["action"]?.Value?.ToString() ?? "";
                    var user = r.Cells["user_id"]?.Value?.ToString() ?? "";
                    var rid = r.Cells["reference_id"]?.Value?.ToString() ?? "";
                    var created = r.Cells["created_at"]?.Value?.ToString() ?? "";
                    var desc = r.Cells["description"]?.Value?.ToString() ?? "";
                    var details = "";
                    try { details = r.Cells["details"]?.Value?.ToString() ?? ""; } catch { details = ""; }

                    using var modal = new Form();
                    modal.Text = "Detail Audit Log";
                    modal.StartPosition = FormStartPosition.CenterParent;
                    modal.Size = new Size(980, 720);
                    modal.MinimizeBox = false;
                    modal.MaximizeBox = true;

                    var txt = new TextBox
                    {
                        Multiline = true,
                        ReadOnly = true,
                        ScrollBars = ScrollBars.Both,
                        Dock = DockStyle.Fill,
                        Font = new Font("Consolas", 10F),
                        WordWrap = false,
                        Text = $"WAKTU: {created}{Environment.NewLine}USER: {user}{Environment.NewLine}REF: {rid}{Environment.NewLine}ACTION: {action}{Environment.NewLine}{Environment.NewLine}DESC:{Environment.NewLine}{desc}{Environment.NewLine}{Environment.NewLine}DETAILS:{Environment.NewLine}{PrettyJson(details)}"
                    };
                    var btnCopy = new Button { Text = "Copy", Dock = DockStyle.Bottom, Height = 44 };
                    btnCopy.Click += (_, __) => { try { Clipboard.SetText(txt.Text ?? ""); } catch { } };
                    modal.Controls.Add(txt);
                    modal.Controls.Add(btnCopy);
                    modal.ShowDialog(f);
                }
                catch
                {
                }
            }

            btnSearch.Click += (_, __) => RefreshGrid();
            btnDetail.Click += (_, __) => ShowSelected();
            grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) ShowSelected(); };
            btnClose.Click += (_, __) => f.Close();

            RefreshGrid();
            f.ShowDialog(this);
        }

        private bool TryAskDateRange(string title, out DateTime start, out DateTime endExclusive)
        {
            start = DateTime.Today;
            endExclusive = DateTime.Today.AddDays(1);

            DateTime selectedStart = start;
            DateTime selectedEndExclusive = endExclusive;

            using var modal = new Form
            {
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(520, 320),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = new Padding(16)
            };

            var lblHint = new Label
            {
                Text = "Pilih rentang tanggal laporan.",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.DimGray
            };

            var panelFields = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 140,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(0, 10, 0, 0)
            };
            panelFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            panelFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            panelFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            panelFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));

            var lbl1 = new Label
            {
                Text = "Tanggal Mulai",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
            };
            var dtp1 = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            var lbl2 = new Label
            {
                Text = "Tanggal Akhir",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
            };
            var dtp2 = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            panelFields.Controls.Add(lbl1, 0, 0);
            panelFields.Controls.Add(dtp1, 1, 0);
            panelFields.Controls.Add(lbl2, 0, 1);
            panelFields.Controls.Add(dtp2, 1, 1);

            var lblNote = new Label
            {
                Text = "Catatan: tanggal akhir dihitung termasuk hari itu (sampai 23:59).",
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.DimGray
            };

            var pnlBtn = new Panel { Dock = DockStyle.Bottom, Height = 46 };
            var btnOk = new Button { Text = "Tampilkan", Dock = DockStyle.Right, Width = 120 };
            var btnCancel = new Button { Text = "Batal", Dock = DockStyle.Right, Width = 100 };
            btnOk.Click += (s, e) =>
            {
                if (dtp1.Value.Date > dtp2.Value.Date)
                {
                    MessageBox.Show("Tanggal mulai tidak boleh lebih besar dari tanggal akhir.");
                    return;
                }
                selectedStart = dtp1.Value.Date;
                selectedEndExclusive = dtp2.Value.Date.AddDays(1);
                modal.DialogResult = DialogResult.OK;
                modal.Close();
            };
            btnCancel.Click += (s, e) => { modal.DialogResult = DialogResult.Cancel; modal.Close(); };
            pnlBtn.Controls.Add(btnCancel);
            pnlBtn.Controls.Add(btnOk);

            modal.Controls.Add(pnlBtn);
            modal.Controls.Add(lblNote);
            modal.Controls.Add(panelFields);
            modal.Controls.Add(lblHint);

            modal.AcceptButton = btnOk;
            modal.CancelButton = btnCancel;
            if (modal.ShowDialog(this) != DialogResult.OK) return false;
            start = selectedStart;
            endExclusive = selectedEndExclusive;
            return true;
        }

        private void ShowReportGrid(string title, DataTable dt)
        {
            using var f = new Form
            {
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(980, 640)
            };

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                BackgroundColor = Color.White
            };

            grid.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
            grid.DataSource = dt;

            static bool IsMoneyColumn(string name)
            {
                if (string.IsNullOrWhiteSpace(name)) return false;
                return name.Contains("omzet", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("total", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("hpp", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("laba", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("profit", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("amount", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("harga", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("price", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("cash", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("nilai", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("grand", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("opening", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("expected", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("closing", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("difference", StringComparison.OrdinalIgnoreCase)
                    || name.Contains("unit_cost", StringComparison.OrdinalIgnoreCase);
            }

            grid.CellFormatting += (_, e) =>
            {
                try
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                    var col = grid.Columns[e.ColumnIndex];
                    var key = col?.DataPropertyName;
                    if (string.IsNullOrWhiteSpace(key)) key = col?.Name;
                    if (string.IsNullOrWhiteSpace(key)) key = col?.HeaderText;
                    if (!IsMoneyColumn(key ?? "")) return;

                    if (e.Value == null || e.Value == DBNull.Value) return;

                    decimal v;
                    if (e.Value is decimal dec) v = dec;
                    else if (e.Value is double dbl) v = Convert.ToDecimal(dbl);
                    else if (e.Value is float fl) v = Convert.ToDecimal(fl);
                    else if (e.Value is int i) v = i;
                    else if (e.Value is long l) v = l;
                    else
                    {
                        string s = Convert.ToString(e.Value) ?? "";
                        if (string.IsNullOrWhiteSpace(s)) return;

                        string digitsOnly = s.Replace(".", "").Replace(",", "").Replace("Rp", "", StringComparison.OrdinalIgnoreCase).Trim();
                        if (!decimal.TryParse(digitsOnly, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                            return;
                    }

                    e.Value = v.ToString("N0", UiNumberFormat.DotCulture);
                    e.FormattingApplied = true;
                }
                catch
                {
                }
            };

            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.ValueType == typeof(decimal) || col.ValueType == typeof(double) || col.ValueType == typeof(float))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N0";
                }
                if (col.ValueType == typeof(int) || col.ValueType == typeof(long))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                var colKey = !string.IsNullOrWhiteSpace(col.DataPropertyName) ? col.DataPropertyName : col.Name;
                if (IsMoneyColumn(colKey))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                    col.DefaultCellStyle.Format = "N0";
                }
            }

            f.Controls.Add(grid);
            f.ShowDialog(this);
        }

        private DataTable QuerySalesByDay(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    DATE(t.created_at) AS tanggal,
    COUNT(*) AS transaksi,
    COALESCE(SUM(t.ts_grand_total),0) AS omzet,
    COALESCE(SUM(h.hpp),0) AS hpp,
    COALESCE(SUM(t.ts_grand_total),0) - COALESCE(SUM(h.hpp),0) AS laba
FROM transactions t
LEFT JOIN (
    SELECT td.ts_id, COALESCE(SUM(td.tsd_buy_price * td.tsd_quantity),0) AS hpp
    FROM transaction_details td
    GROUP BY td.ts_id
) h ON h.ts_id = t.ts_id
WHERE t.deleted_at IS NULL
  AND t.ts_status = 1
  AND t.created_at >= @start
  AND t.created_at < @end
GROUP BY DATE(t.created_at)
ORDER BY DATE(t.created_at) ASC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QuerySalesByCashier(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    t.user_id AS cashier_id,
    COALESCE(u.username,'') AS cashier,
    COUNT(*) AS transaksi,
    COALESCE(SUM(t.ts_grand_total),0) AS omzet,
    COALESCE(SUM(h.hpp),0) AS hpp,
    COALESCE(SUM(t.ts_grand_total),0) - COALESCE(SUM(h.hpp),0) AS laba
FROM transactions t
LEFT JOIN users u ON u.id = t.user_id
LEFT JOIN (
    SELECT td.ts_id, COALESCE(SUM(td.tsd_buy_price * td.tsd_quantity),0) AS hpp
    FROM transaction_details td
    GROUP BY td.ts_id
) h ON h.ts_id = t.ts_id
WHERE t.deleted_at IS NULL
  AND t.ts_status = 1
  AND t.created_at >= @start
  AND t.created_at < @end
GROUP BY t.user_id, COALESCE(u.username,'')
ORDER BY omzet DESC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QuerySalesByProduct(DateTime startInclusive, DateTime endExclusive, int? topN)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string limit = topN.HasValue && topN.Value > 0 ? "LIMIT @top" : "";
            using var cmd = new NpgsqlCommand($@"
SELECT
    td.item_id,
    COALESCE(i.name,'') AS produk,
    COALESCE(SUM(td.tsd_quantity),0) AS qty,
    COALESCE(SUM(td.tsd_total),0) AS omzet,
    COALESCE(SUM(td.tsd_buy_price * td.tsd_quantity),0) AS hpp,
    COALESCE(SUM(td.tsd_total),0) - COALESCE(SUM(td.tsd_buy_price * td.tsd_quantity),0) AS laba
FROM transaction_details td
JOIN transactions t ON t.ts_id = td.ts_id
LEFT JOIN items i ON i.id = td.item_id
WHERE t.deleted_at IS NULL
  AND t.ts_status = 1
  AND t.created_at >= @start
  AND t.created_at < @end
GROUP BY td.item_id, COALESCE(i.name,'')
ORDER BY omzet DESC
{limit}
", conn);

            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            if (topN.HasValue && topN.Value > 0)
                cmd.Parameters.AddWithValue("@top", topN.Value);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QuerySalesByCategory(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    c.id AS category_id,
    COALESCE(c.name,'') AS kategori,
    COALESCE(SUM(td.tsd_quantity),0) AS qty,
    COALESCE(SUM(td.tsd_total),0) AS omzet,
    COALESCE(SUM(td.tsd_buy_price * td.tsd_quantity),0) AS hpp,
    COALESCE(SUM(td.tsd_total),0) - COALESCE(SUM(td.tsd_buy_price * td.tsd_quantity),0) AS laba
FROM transaction_details td
JOIN transactions t ON t.ts_id = td.ts_id
LEFT JOIN items i ON i.id = td.item_id
LEFT JOIN categories c ON c.id = i.category_id
WHERE t.deleted_at IS NULL
  AND t.ts_status = 1
  AND t.created_at >= @start
  AND t.created_at < @end
GROUP BY c.id, COALESCE(c.name,'')
ORDER BY omzet DESC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QueryPaymentRecap(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    COALESCE(t.ts_method,'') AS metode,
    COUNT(*) AS transaksi,
    COALESCE(SUM(t.ts_grand_total),0) AS omzet
FROM transactions t
WHERE t.deleted_at IS NULL
  AND t.ts_status = 1
  AND t.created_at >= @start
  AND t.created_at < @end
GROUP BY COALESCE(t.ts_method,'')
ORDER BY omzet DESC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QueryShiftClosings(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    cs.id AS shift_id,
    cs.user_id,
    COALESCE(u.username,'') AS cashier,
    cs.terminal_id,
    cs.opened_at,
    cs.closed_at,
    cs.status,
    COALESCE(cs.opening_cash,0) AS opening_cash,
    COALESCE(cs.expected_cash,0) AS expected_cash,
    COALESCE(cs.closing_cash,0) AS closing_cash,
    COALESCE(cs.difference_cash,0) AS difference_cash
FROM cashier_shifts cs
LEFT JOIN users u ON u.id = cs.user_id
WHERE cs.opened_at >= @start
  AND cs.opened_at < @end
ORDER BY cs.opened_at DESC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QueryStockNow()
        {
            var ic = new POS_qu.Controllers.ItemController();
            return ic.GetItems(null, null);
        }

        private DataTable QueryStockLow()
        {
            var dt = QueryStockNow();
            if (dt == null) return dt;
            if (!dt.Columns.Contains("stock") || !dt.Columns.Contains("min_qty")) return dt;

            var view = new DataView(dt);
            view.RowFilter = "min_qty > 0 AND stock <= min_qty";
            return view.ToTable();
        }

        private bool ColumnExists(NpgsqlConnection conn, string tableName, string columnName)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.columns
WHERE table_schema='public'
  AND table_name = @t
  AND column_name = @c
LIMIT 1
", conn);
            cmd.Parameters.AddWithValue("@t", tableName);
            cmd.Parameters.AddWithValue("@c", columnName);
            return cmd.ExecuteScalar() != null;
        }

        private DataTable QueryStockLog(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            bool hasWarehouseId = ColumnExists(conn, "stock_log", "warehouse_id");
            bool hasRefType = ColumnExists(conn, "stock_log", "ref_type");
            bool hasRefId = ColumnExists(conn, "stock_log", "ref_id");
            bool hasUnitCost = ColumnExists(conn, "stock_log", "unit_cost");

            string selectWarehouse = hasWarehouseId ? "sl.warehouse_id," : "NULL::int AS warehouse_id,";
            string selectRefType = hasRefType ? "sl.ref_type," : "NULL::text AS ref_type,";
            string selectRefId = hasRefId ? "sl.ref_id," : "NULL::bigint AS ref_id,";
            string selectUnitCost = hasUnitCost ? "sl.unit_cost," : "NULL::numeric AS unit_cost,";

            using var cmd = new NpgsqlCommand($@"
SELECT
    sl.created_at,
    sl.product_id AS item_id,
    COALESCE(i.name,'') AS item_name,
    sl.tipe_transaksi,
    sl.qty_masuk,
    sl.qty_keluar,
    sl.sisa_stock,
    {selectWarehouse}
    {selectRefType}
    {selectRefId}
    {selectUnitCost}
    COALESCE(sl.keterangan,'') AS keterangan
FROM stock_log sl
LEFT JOIN items i ON i.id = sl.product_id
WHERE sl.created_at >= @start
  AND sl.created_at < @end
ORDER BY sl.created_at DESC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QueryPurchaseBySupplier(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            bool hasCreatedAt = ColumnExists(conn, "purchase_orders", "created_at");
            string dateFilter = hasCreatedAt ? "po.created_at >= @start AND po.created_at < @end" : "po.order_date >= @start::date AND po.order_date < @end::date";

            using var cmd = new NpgsqlCommand($@"
SELECT
    po.supplier_id,
    COALESCE(s.name,'') AS supplier,
    COUNT(*) AS total_po,
    COALESCE(SUM(po.total_amount),0) AS total_belanja
FROM purchase_orders po
LEFT JOIN suppliers s ON s.id = po.supplier_id
WHERE {dateFilter}
GROUP BY po.supplier_id, COALESCE(s.name,'')
ORDER BY total_belanja DESC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QueryGoodsIn(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    ia.adjustment_no,
    ia.adjustment_date,
    COALESCE(w.name,'') AS warehouse,
    COALESCE(SUM(ii.qty),0) AS qty_total,
    COALESCE(SUM(COALESCE(ii.buy_price,0) * ii.qty),0) AS total_hpp,
    COALESCE(ia.reason,'') AS reason,
    COALESCE(ia.note,'') AS note
FROM inventory_adjustments ia
JOIN inventory_adjustment_items ii ON ii.adjustment_id = ia.id
LEFT JOIN warehouses w ON w.id = ia.warehouse_id
WHERE ia.direction = 'IN'
  AND ia.adjustment_date >= @start::date
  AND ia.adjustment_date < @end::date
GROUP BY ia.adjustment_no, ia.adjustment_date, COALESCE(w.name,''), COALESCE(ia.reason,''), COALESCE(ia.note,'')
ORDER BY ia.adjustment_date DESC, ia.adjustment_no DESC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QueryCustomerTransactions(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    t.created_at,
    t.ts_id,
    t.ts_numbering,
    t.ts_method,
    COALESCE(c.name, t.ts_freename, 'Guest') AS customer,
    t.ts_grand_total
FROM transactions t
LEFT JOIN customers c ON c.id = t.ts_customer
WHERE t.deleted_at IS NULL
  AND t.ts_status = 1
  AND t.created_at >= @start
  AND t.created_at < @end
ORDER BY t.created_at DESC
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable QueryTopCustomers(DateTime startInclusive, DateTime endExclusive)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    t.ts_customer AS customer_id,
    COALESCE(c.name, t.ts_freename, 'Guest') AS customer,
    COUNT(*) AS transaksi,
    COALESCE(SUM(t.ts_grand_total),0) AS omzet
FROM transactions t
LEFT JOIN customers c ON c.id = t.ts_customer
WHERE t.deleted_at IS NULL
  AND t.ts_status = 1
  AND t.created_at >= @start
  AND t.created_at < @end
GROUP BY t.ts_customer, COALESCE(c.name, t.ts_freename, 'Guest')
ORDER BY transaksi DESC, omzet DESC
LIMIT 50
", conn);
            cmd.Parameters.AddWithValue("@start", startInclusive);
            cmd.Parameters.AddWithValue("@end", endExclusive);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private void SetMenuVisibility(int roleId)
        {
            // roleId: 1=admin, 2=cashier/casher, 3=supervisor

            bool isAdmin = roleId == 1;
            bool isCashier = roleId == 2;
            bool isSupervisor = roleId == 3;

            // Admin: semua menu
            // Cashier: hanya menu kasir
            // Supervisor: produk + stock + laporan (tanpa settings)

            if (masterToolStripMenuItem != null) masterToolStripMenuItem.Visible = isAdmin;
            if (productToolStripMenuItem != null) productToolStripMenuItem.Visible = isAdmin || isSupervisor;
            if (stockMenuToolStripMenuItem != null) stockMenuToolStripMenuItem.Visible = isAdmin || isSupervisor;
            if (promosiDiskonToolStripMenuItem != null) promosiDiskonToolStripMenuItem.Visible = isAdmin;
            if (casherToolStripMenuItem != null) casherToolStripMenuItem.Visible = isAdmin || isCashier;
            if (pembelianToolStripMenuItem != null) pembelianToolStripMenuItem.Visible = isAdmin;
            if (reportsToolStripMenuItem != null) reportsToolStripMenuItem.Visible = isAdmin || isSupervisor;
            if (printingToolStripMenuItem != null) printingToolStripMenuItem.Visible = isAdmin;
            if (settingsToolStripMenuItem != null) settingsToolStripMenuItem.Visible = isAdmin;

            // Submenu laporan: ikut parent (reports). Ini biar tidak muncul “nyempil”.
            if (penjualanToolStripMenuItem != null) penjualanToolStripMenuItem.Visible = (reportsToolStripMenuItem?.Visible ?? false);
        }

        private void UpdatePanel2Size()
        {
            int marginRight = 10;
            int panel1Width = panel1.Width;
            //panel2.Location = new Point(panel1Width + 12, panel2.Location.Y);  // +12 px gap
            //panel2.Size = new Size(this.ClientSize.Width - panel1Width - marginRight - 12, this.ClientSize.Height);
        }

        private void masterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MasterGudang_Click(object sender, EventArgs e)
        {
            WarehouseForm f = new WarehouseForm();
            this.Hide();
            f.FormClosed += (s, args) => this.Show();
            f.Show();
        }

        private void merkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrandForm f = new BrandForm();
            this.Hide();
            f.FormClosed += (s, args) => this.Show();
            f.Show();
        }

        private void rakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RackForm f = new RackForm();
            this.Hide();
            f.FormClosed += (s, args) => this.Show();
            f.Show();
        }

        private void produkToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesReports s = new SalesReports();
            this.Hide();  // Sembunyikan MenuNative sementara

            s.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            s.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void casherToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void penjualanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesReports f = new SalesReports();
            this.Hide();  // Sembunyikan MenuNative sementara

            f.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            f.Show();
        }

        private void pembelianToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurchaseOrderListForm f = new PurchaseOrderListForm();
            this.Hide();
            f.FormClosed += (s, args) => this.Show();
            f.Show();
        }

        private void penerimaanBarangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReceivePOForm f = new ReceivePOForm();
            this.Hide();
            f.FormClosed += (s, args) => this.Show();
            f.Show();
        }

        private void returPembelianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new PurchaseReturnListForm();
            f.ShowDialog(this);
        }

        private void strukSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StrukSetting f = new StrukSetting();
            this.Hide();  // Sembunyikan MenuNative sementara

            f.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            f.Show();
        }

        private void tokoSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TokoSetting f = new TokoSetting();
            this.Hide();  // Sembunyikan MenuNative sementara

            f.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            f.Show();
        }

        private void numberingSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new NumberingSettingForm();
            f.ShowDialog(this);
        }

        private void hppMethodSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new HppMethodSettingForm();
            f.ShowDialog(this);
        }

        private void networkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new PrinterTestForm();
            f.ShowDialog(this);
        }

        private void terminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Terminal f = new Terminal();
            this.Hide();  // Sembunyikan MenuNative sementara

            f.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            f.Show();
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Roles f = new Roles();
            this.Hide();  // Sembunyikan MenuNative sementara

            f.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            f.Show();
        }

        private void manajemenRolesPermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Roles f = new Roles();
            this.Hide();
            f.FormClosed += (s, args) => this.Show();
            f.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Roles f = new Roles();
            this.Hide();
            f.FormClosed += (s, args) => this.Show();
            f.Show();
        }

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockReports f = new StockReports();
            this.Hide();  // Sembunyikan MenuNative sementara

            f.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            f.Show();
        }

        private void jurnalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var f = new JournalReportForm();
            f.ShowDialog(this);
        }

        private void pengeluaranToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private ProductPage productPage;
        private void manajemenProdukToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (productPage == null || productPage.IsDisposed)
            {
                productPage = new ProductPage();
                productPage.Show();
            }
            else
            {
                productPage.BringToFront();
            }
        }

        private void pelangganCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CustomerForm f = new CustomerForm())
            {
                f.ShowDialog(this); // owner = form utama
            }
        }

        private void kategoriBarangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CategoryForm f = new CategoryForm())
            {
                f.ShowDialog(this); // owner = form utama
            }

        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SupplierForm f = new SupplierForm())
            {
                f.ShowDialog(this); // owner = form utama
            }
        }

        private void unitSatuanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (UnitForm f = new UnitForm())
            {
                f.ShowDialog(this); // owner = form utama
            }
        }

        private void akunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AccountsForm f = new AccountsForm())
            {
                f.ShowDialog(this);
            }
        }

        private void jenisPpnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (TaxTypeForm f = new TaxTypeForm())
            {
                f.ShowDialog(this);
            }
        }

        private void daftarTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (TransactionsListForm f = new TransactionsListForm())
            {
                f.ShowDialog(this); // owner = form utama
            }
        }

        private void casherToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            CasherNew casherForm = new CasherNew();
            this.Hide();  // Sembunyikan MenuNative sementara

            casherForm.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            casherForm.Show();

        }

        private void pendingTransaksiAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Manajemen Pending/Draft sementara tidak tersedia.", "Info");
        }

        // returBarangToolStripMenuItem_Click no longer used

        private void LoadDashboardSummary()
        {
            using var conn = new Npgsql.NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            DateTime todayStart = DateTime.Today;
            DateTime todayEnd = DateTime.Today.AddDays(1).AddTicks(-1);
            DateTime monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddTicks(-1);

            decimal omzetToday = GetOmzet(conn, todayStart, todayEnd);
            decimal omzetMonth = GetOmzet(conn, monthStart, monthEnd);
            decimal hppMonth = GetHPP(conn, monthStart, monthEnd);
            decimal profitMonth = omzetMonth - hppMonth;

            var ci = System.Globalization.CultureInfo.GetCultureInfo("id-ID");
            label1.Text = "Welcome • Dashboard";
            label1.Font = new Font(label1.Font, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.DimGray;
            label2.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            // labels in designer
            // lblOmzetToday, lblOmzetMonth, lblHPPMonth, lblProfitMonth
            var form = (POSqu_menu.MenuNative)this;
            // direct access to labels declared in designer
            // set text
            // They are fields; we can set directly
            // Below assumes names exist
            // If not, designer patch added them
            lblOmzetToday.Text = $"Omzet Hari Ini: Rp {omzetToday.ToString("N0", ci)}";
            lblOmzetMonth.Text = $"Omzet Bulan Ini: Rp {omzetMonth.ToString("N0", ci)}";
            lblHPPMonth.Text = $"HPP Bulanan: Rp {hppMonth.ToString("N0", ci)}";
            lblProfitMonth.Text = $"Laba Bersih: Rp {profitMonth.ToString("N0", ci)}";
        }

        private decimal GetOmzet(Npgsql.NpgsqlConnection conn, DateTime start, DateTime end)
        {
            using var cmd = new Npgsql.NpgsqlCommand(@"
                SELECT COALESCE(SUM(ts_grand_total),0)
                FROM transactions
                WHERE ts_status = 1
                  AND created_at BETWEEN @start AND @end
            ", conn);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        private decimal GetHPP(Npgsql.NpgsqlConnection conn, DateTime start, DateTime end)
        {
            using var cmd = new Npgsql.NpgsqlCommand(@"
                SELECT COALESCE(SUM(td.tsd_buy_price * td.tsd_quantity),0)
                FROM transaction_details td
                JOIN transactions t ON t.ts_id = td.ts_id
                WHERE t.ts_status = 1
                  AND t.created_at BETWEEN @start AND @end
            ", conn);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        private void RenderSalesBarsLast30Days()
        {
            var flp = new FlowLayoutPanel();
            flp.Name = "salesBarsPanel";
            flp.Dock = DockStyle.Right;
            flp.Width = 360;
            flp.Height = 160;
            flp.Padding = new Padding(8);
            flp.BackColor = Color.White;
            flp.AutoScroll = true;

            var rows = new List<(string day, decimal sum)>();
            using (var conn = new Npgsql.NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using var cmd = new Npgsql.NpgsqlCommand(@"
                    SELECT TO_CHAR(DATE(created_at), 'DD/MM') AS d, SUM(ts_grand_total) AS s
                    FROM transactions
                    WHERE ts_status = 1
                      AND created_at >= CURRENT_DATE - INTERVAL '30 days'
                    GROUP BY DATE(created_at)
                    ORDER BY DATE(created_at)
                ", conn);
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    rows.Add((rdr.GetString(0), rdr.GetDecimal(1)));
                }
            }

            if (rows.Count == 0)
            {
                var lbl = new Label { Text = "Tidak ada data omzet 30 hari.", AutoSize = true };
                flp.Controls.Add(lbl);
            }
            else
            {
                decimal max = rows.Max(r => r.sum);
                foreach (var r in rows)
                {
                    int barWidth = max > 0 ? (int)(300 * (r.sum / max)) : 0;
                    var rowPanel = new Panel { Width = flp.Width - 24, Height = 24, BackColor = Color.Transparent };
                    var lblDay = new Label { Text = r.day, Width = 60, Height = 24, TextAlign = ContentAlignment.MiddleLeft };
                    var bar = new Panel { Width = barWidth, Height = 16, BackColor = Color.SteelBlue, Margin = new Padding(6, 4, 6, 4) };
                    var lblVal = new Label { Text = r.sum.ToString("N0"), AutoSize = true, Height = 24, TextAlign = ContentAlignment.MiddleLeft };
                    rowPanel.Controls.Add(lblDay);
                    lblDay.Location = new Point(0, 0);
                    rowPanel.Controls.Add(bar);
                    bar.Location = new Point(66, 4);
                    rowPanel.Controls.Add(lblVal);
                    lblVal.Location = new Point(66 + barWidth + 8, 0);
                    flp.Controls.Add(rowPanel);
                }
            }

            panelWelcome.Controls.Add(flp);
            flp.BringToFront();
        }

        private void btnRefreshDashboard_Click(object sender, EventArgs e)
        {
            LoadDashboardSummary();
            foreach (Control c in panelWelcome.Controls)
            {
                if (c.Name == "salesBarsPanel")
                {
                    panelWelcome.Controls.Remove(c);
                    c.Dispose();
                    break;
                }
            }
            RenderSalesBarsLast30Days();
        }

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DatabaseSetting p = new DatabaseSetting();
            this.Hide();  // Sembunyikan MenuNative sementara

            p.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            p.Show();
        }
    }
}
