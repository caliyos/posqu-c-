using DocumentFormat.OpenXml.Bibliography;
using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class DashboardNew : Form
    {
        public DashboardNew()
        {
            //MessageBox.Show("Constructor");
            InitializeComponent();
            this.Load += DashboardNew_Load;
            timerClock.Tick += timerClock_Tick;
        }
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
        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void DashboardNew_Load(object sender, EventArgs e)
        {

            //MessageBox.Show("DashboardNew_Load");
            try
            {

                timerClock.Interval = 1000; // 1 detik
                timerClock.Start();

                label42.Text =
                    $"Licensed To : {GlobalContext.LicenseInfo}";

                var user = SessionUser.GetCurrentUser();
                string pcId = Utility.GetPcId();

                if (user == null)
                {
                    label2.Text = "⚠️ Tidak ada sesi pengguna aktif.";
                    label2.Visible = true;
                    if (lblSessionInfo != null) lblSessionInfo.Text = "-";
                    return;
                }

                GlobalContext.RefreshConnectionInfo();
                string dbSummary = !string.IsNullOrWhiteSpace(GlobalContext.ConnectionSummary) ? GlobalContext.ConnectionSummary : "-";

                string appTz = DbConfig.AppTimeZone ?? "";
                string dbTz = GetDatabaseTimeZone() ?? "";
                //label2.Visible = false;
                lblSessionInfo.Text = "xx";
                //MessageBox.Show(lblSessionInfo == null ? "NULL" : "ADA");
                
                if (lblSessionInfo != null)
                {
                    var now = DateTime.Now;

                    string internetStatus =
                        Utility.IsInternetAvailable()
                            ? "Online"
                            : "Offline";

                    lblSessionInfo.Text =
                        $"User: {user.Username} (ID: {user.UserId})\n" +
                        $"Role: {user.RoleName} (ID: {user.RoleId})\n" +
                        $"Terminal: {user.TerminalName} (ID: {user.TerminalId})\n" +
                        $"Shift: {user.ShiftId}\n" +
                        $"PC ID: {pcId}\n" +
                        $"Database: {dbSummary}\n" +
                        $"Internet: {internetStatus}\n" +
                        $"TimeZone App: {(!string.IsNullOrWhiteSpace(appTz) ? appTz : "-")} | DB: {(!string.IsNullOrWhiteSpace(dbTz) ? dbTz : "-")}\n" +
                        $"Waktu Login: {now:HH:mm:ss dd/MM/yyyy}";
                }


            }
            catch (Exception ex)
            {
                label2.Text = "❌ Error memuat sesi: " + ex.Message;
                label2.Visible = true;
            }
        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void roundedPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void supportPengembangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form
            {
                Text = $"Support {GlobalContext.DeveloperName}",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(500, 250),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new System.Windows.Forms.Label
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                Text =
                    $"{Application.ProductName}\r\n" +
                    $"Version : {Application.ProductVersion}\r\n\r\n" +
                    $"Developer : {GlobalContext.DeveloperName}\r\n\r\n" +
                    $"Website : {GlobalContext.DeveloperWebsite}\r\n" +
                    $"Email   : {GlobalContext.DeveloperEmail}\r\n" +
                    $"WA      : {GlobalContext.DeveloperWhatsapp}\r\n\r\n" +
                    "Untuk dokumentasi, pelaporan bug, dan dukungan teknis silakan kunjungi website resmi.",
                AutoSize = false
            };

            frm.Controls.Add(lbl);
            frm.ShowDialog(this);
        }

        private void cekUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form
            {
                Text = "Cek Update",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(400, 180),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new System.Windows.Forms.Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text =
                    $"{Application.ProductName}\r\n" +
                    $"Version : {Application.ProductVersion}\r\n\r\n" +
                    "Fitur cek update online belum tersedia.",
                AutoSize = false
            };

            frm.Controls.Add(lbl);
            frm.ShowDialog(this);
        }

        private void aboutBeoposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form
            {
                Text = $"About {Application.ProductName}",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(500, 300),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new System.Windows.Forms.Label
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                Font = new Font("Segoe UI", 10),
                Text =
                    $"{Application.ProductName}\r\n" +
                    $"Version : {Application.ProductVersion}\r\n\r\n" +
                    $"Developed by {GlobalContext.DeveloperName}\r\n" +
                    "© 2026 All Rights Reserved\r\n\r\n" +
                    $"Licensed To : {GlobalContext.LicenseInfo}\r\n\r\n" +
                    $"Website : {GlobalContext.DeveloperWebsite}\r\n" +
                    $"Email : {GlobalContext.DeveloperEmail}",
                AutoSize = false
            };

            frm.Controls.Add(lbl);
            frm.ShowDialog(this);
        }

     
    }
}
