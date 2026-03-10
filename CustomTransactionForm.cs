using POS_qu.Helpers;
using POS_qu.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public class CustomTransactionForm : Form
    {
        public string ItemName => txtName.Text.Trim();
        public string Unit => string.IsNullOrWhiteSpace(txtUnit.Text) ? "pcs" : txtUnit.Text.Trim();
        public int UnitId => 0;
        public decimal Qty { get; private set; }
        public decimal Price { get; private set; }

        private TextBox txtName;
        private TextBox txtUnit;
        private TextBox txtQty;
        private TextBox txtPrice;
        private Button btnOk;
        private Button btnCancel;

        public CustomTransactionForm()
        {
            Text = "Custom Transaction";
            Size = new Size(420, 260);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Padding = new Padding(12);

            var lbl1 = new Label { Text = "Nama:", AutoSize = true, Left = 10, Top = 20 };
            txtName = new TextBox { Left = 120, Top = 16, Width = 250 };
            var lbl2 = new Label { Text = "Unit:", AutoSize = true, Left = 10, Top = 60 };
            txtUnit = new TextBox { Left = 120, Top = 56, Width = 120, Text = "pcs" };
            var lbl3 = new Label { Text = "Qty:", AutoSize = true, Left = 10, Top = 100 };
            txtQty = new TextBox { Left = 120, Top = 96, Width = 120, Text = "1" };
            var lbl4 = new Label { Text = "Harga:", AutoSize = true, Left = 10, Top = 140 };
            txtPrice = new TextBox { Left = 120, Top = 136, Width = 120, Text = "0" };

            btnOk = new Button { Text = "Tambah", Left = 210, Top = 176, Width = 80, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Batal", Left = 300, Top = 176, Width = 70, DialogResult = DialogResult.Cancel };
            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(ItemName))
                {
                    MessageBox.Show("Nama wajib diisi.");
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (!decimal.TryParse(txtQty.Text, out var q) || q <= 0)
                {
                    MessageBox.Show("Qty tidak valid.");
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (!decimal.TryParse(txtPrice.Text.Replace("Rp", "").Replace(".", "").Replace(",", ""), out var p) || p < 0)
                {
                    MessageBox.Show("Harga tidak valid.");
                    this.DialogResult = DialogResult.None;
                    return;
                }
                Qty = q;
                Price = p;
            };

            Controls.Add(lbl1);
            Controls.Add(txtName);
            Controls.Add(lbl2);
            Controls.Add(txtUnit);
            Controls.Add(lbl3);
            Controls.Add(txtQty);
            Controls.Add(lbl4);
            Controls.Add(txtPrice);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
        }
    }
}
