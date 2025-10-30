using Npgsql;
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
                    BackColor = Color.FromArgb(30, 30, 30),
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
            }
            catch (Exception ex)
            {
                label2.Text = "❌ Error memuat sesi: " + ex.Message;
            }
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
            panel2.Location = new Point(panel1Width + 12, panel2.Location.Y);  // +12 px gap
            panel2.Size = new Size(this.ClientSize.Width - panel1Width - marginRight - 12, this.ClientSize.Height);
        }

        private void masterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void produkToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ProductPage p = new ProductPage();
            this.Hide();  // Sembunyikan MenuNative sementara

            p.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            p.Show();
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

            Casher_POS casherForm = new Casher_POS();
            this.Hide();  // Sembunyikan MenuNative sementara

            casherForm.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            casherForm.Show();

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
    }
}
