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
        public MenuNative()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.Load += MenuNative_Load;
            //this.FormBorderStyle = FormBorderStyle.None;

            var user = SessionUser.GetCurrentUser();
            string pcId = Utility.GetPcId();
            //string terminal = SessionUser.TerminalId(pcId);
            //string shift = Utility.GetCurrentShift();

            //string pcId = Utility.GetPcId();
            //string terminal = Utility.GetTerminalName(pcId);
            //string shift = Utility.GetCurrentShift();

            label2.Text = $"User: {user.Username} | Terminal: {user.TerminalId} | Shift: {user.ShiftId} |pcid: {pcId}";


        }

        private void MenuNative_Load(object sender, EventArgs e)
        {
            UpdatePanel2Size();
        }
        private void UpdatePanel2Size()
        {
            int marginRight = 10;
            int panel1Width = panel1.Width;
            panel2.Location = new Point(panel1Width + 12, panel2.Location.Y);  // +12 px gap
            panel2.Size = new Size(this.ClientSize.Width - panel1Width - marginRight - 12, this.ClientSize.Height);
        }

        private void produkToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Casher_POS casherForm = new Casher_POS();
            this.Hide();  // Sembunyikan MenuNative sementara

            casherForm.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            casherForm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void casherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductPage p = new ProductPage();
            this.Hide();  // Sembunyikan MenuNative sementara

            p.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            p.Show();
        }

        private void penjualanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesReports s = new SalesReports();
            this.Hide();  // Sembunyikan MenuNative sementara

            s.FormClosed += (s, args) => this.Show();  // Kalau Casher_POS ditutup, munculkan lagi MenuNative
            s.Show();
        }
    }
}
