﻿﻿﻿﻿﻿﻿using Npgsql;
using POS_qu;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            var user = SessionUser.GetCurrentUser();
            if (user == null) return;

            string dbTimeZone = GetDatabaseTimeZone() ?? "Asia/Makassar"; // fallback kalau gagal

            DateTime utcNow = DateTime.UtcNow;
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, TimeZoneInfo.FindSystemTimeZoneById(dbTimeZone));

            marqueeText = $"👤 {user.Username} | 🎭 {user.RoleName} | 🖥️ {user.TerminalName} | ⏰ {localTime:HH:mm:ss dd/MM/yyyy}";
            labelMarquee.Text = marqueeText;
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
            InitializeMarquee();
        }

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

                // tampilkan semua data session user
                label2.Text =
                    $"👤 User: {user.Username} (ID: {user.UserId})\n" +
                    $"🎭 Role: {user.RoleName} (ID: {user.RoleId})\n" +
                    $"🖥️ Terminal: {user.TerminalName} (ID: {user.TerminalId})\n" +
                    $"⏰ Shift: {user.ShiftId}\n" +
                    $"💻 PC ID: {pcId}";

                LoadDashboardSummary();
                RenderSalesBarsLast30Days();
                btnRefreshDashboard.Click += btnRefreshDashboard_Click;
                var btnAdminPendingField = panelWelcome.Controls["btnAdminPending"] as Button;
                if (btnAdminPendingField != null)
                    btnAdminPendingField.Click += (s, ev) =>
                    {
                        MessageBox.Show("Manajemen Pending/Draft sementara tidak tersedia.", "Info");
                    };
                // Menu item: Pending (Admin)
                var miPendingAdmin = FindMenuItemByName(menuStrip1.Items, "pendingTransaksiAdminToolStripMenuItem");
                if (miPendingAdmin != null)
                {
                    miPendingAdmin.Click += (s, ev) =>
                    {
                        MessageBox.Show("Manajemen Pending/Draft sementara tidak tersedia.", "Info");
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
            }
            catch (Exception ex)
            {
                label2.Text = "❌ Error memuat sesi: " + ex.Message;
            }
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

        private void SetMenuVisibility(int roleId)
        {
            // contoh roleId: 1=admin, 2=kasir, 3=supervisor (atau backoffice)

            masterToolStripMenuItem.Visible = (roleId == 1 || roleId == 3); // Admin & Backoffice
            casherToolStripMenuItem.Visible = (roleId == 1 || roleId == 2); // Admin & Supervisor
            productToolStripMenuItem.Visible = (roleId == 1 || roleId == 3);  // Admin & Kasir
            penjualanToolStripMenuItem.Visible = (roleId == 1 || roleId == 3); // Admin & Supervisor
            reportsToolStripMenuItem.Visible = (roleId == 1 || roleId == 3); // Admin & Supervisor
            settingsToolStripMenuItem.Visible = (roleId == 1); // Admin & Supervisor
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

        private void daftarPembelianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurchaseOrderListForm POForm = new PurchaseOrderListForm();
            this.Hide();
            POForm.FormClosed += (s, args) => this.Show();
            POForm.Show();
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

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StockReports f = new StockReports();
            this.Hide();  // Sembunyikan MenuNative sementara

            f.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            f.Show();
        }

        private void pengeluaranToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manajemenProdukToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ProductPage p = new ProductPage();
            this.Hide();  // Sembunyikan MenuNative sementara

            p.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            p.Show();

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
    }
}
