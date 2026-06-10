using Npgsql;
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

namespace POS_qu
{
    public partial class DashboardNew : Form
    {
        public DashboardNew()
        {
            MessageBox.Show("Constructor");
            InitializeComponent();
            this.Load += DashboardNew_Load;
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


        private void DashboardNew_Load(object sender, EventArgs e)
        {
            MessageBox.Show("DashboardNew_Load");
            try
            {
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
                MessageBox.Show(lblSessionInfo == null ? "NULL" : "ADA");
                if (lblSessionInfo != null)
                {
                    var now = DateTime.Now;
                    lblSessionInfo.Text =
                        $"User: {user.Username} (ID: {user.UserId})\n" +
                        $"Role: {user.RoleName} (ID: {user.RoleId})\n" +
                        $"Terminal: {user.TerminalName} (ID: {user.TerminalId})\n" +
                        $"Shift: {user.ShiftId}\n" +
                        $"PC ID: {pcId}\n" +
                        $"Database: {dbSummary}\n" +
                        $"TimeZone App: {(!string.IsNullOrWhiteSpace(appTz) ? appTz : "-")} | DB: {(!string.IsNullOrWhiteSpace(dbTz) ? dbTz : "-")}\n" +
                        $"Time: {now:HH:mm:ss dd/MM/yyyy}";
                }

              
            }
            catch (Exception ex)
            {
                label2.Text = "❌ Error memuat sesi: " + ex.Message;
                label2.Visible = true;
            }
        }
    }
}
