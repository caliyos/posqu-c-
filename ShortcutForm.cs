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
    public partial class ShortcutForm : Form
    {
        public ShortcutForm()
        {
            InitializeComponent();


            listShortcut.Columns.Add("Shortcut", 120);
            listShortcut.Columns.Add("Function", 250);
     
            LoadShortcut();
        }



        private void LoadShortcut()
        {
            listShortcut.Items.Clear();

            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + N", "Transaksi Baru (kosongkan cart)" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + P", "Bayar / Payment" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + K", "Cari Barang (Search) + pilih item" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + F", "Cari Barang (Search) + pilih item" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + S", "Fokus ke kolom input cari barang" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "F4", "Pilih Customer" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "F3", "Simpan Draft" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + D", "Buka Draft" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + B", "Bon / Cicilan" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + M", "Monitoring Piutang / Bon" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + I", "Daftar Pending (sementara belum tersedia)" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + Shift + O", "Buka Shift Kasir" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Ctrl + Shift + C", "Tutup Shift Kasir" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "F1", "Buka bantuan Shortcut" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "F12", "Buka bantuan Shortcut" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Enter (di Search)", "Pilih item dari list" }));
            listShortcut.Items.Add(new ListViewItem(new[] { "Double Click (di Search)", "Pilih item dari list" }));
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= 0x20000; // Shadow
                return cp;
            }
        }
    }
}
