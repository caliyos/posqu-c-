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
            listShortcut.Items.Add(new ListViewItem(new[]
            {
        "Ctrl + N", "Buat Data Baru"
    }));

            listShortcut.Items.Add(new ListViewItem(new[]
            {
        "Ctrl + S", "Simpan Data"
    }));

            listShortcut.Items.Add(new ListViewItem(new[]
            {
        "Ctrl + E", "Edit Data"
    }));

            listShortcut.Items.Add(new ListViewItem(new[]
            {
        "Ctrl + D", "Hapus Data"
    }));

            listShortcut.Items.Add(new ListViewItem(new[]
            {
        "F5", "Refresh"
    }));

            listShortcut.Items.Add(new ListViewItem(new[]
            {
        "Esc", "Tutup Window"
    }));
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
