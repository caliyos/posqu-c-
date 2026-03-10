using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public class CashierOpenForm : Form
    {
        private TextBox txtOpening;
        private Button btnOk;
        private Button btnCancel;

        public decimal OpeningCash { get; private set; }

        public CashierOpenForm()
        {
            Text = "Open Shift";
            Size = new Size(360, 180);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Padding = new Padding(12);

            var lbl = new Label
            {
                Text = "Saldo awal kas:",
                Dock = DockStyle.Top,
                Height = 24
            };
            txtOpening = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 28,
                Text = "0"
            };
            var panelButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Width = 80,
                Height = 30,
                Left = 160,
                Top = 10
            };
            btnOk.Click += (s, e) =>
            {
                if (!decimal.TryParse(txtOpening.Text.Replace(".", "").Replace(",", ""), out var val))
                {
                    MessageBox.Show("Nominal tidak valid");
                    this.DialogResult = DialogResult.None;
                    return;
                }
                OpeningCash = val;
            };

            btnCancel = new Button
            {
                Text = "Batal",
                DialogResult = DialogResult.Cancel,
                Width = 80,
                Height = 30,
                Left = 250,
                Top = 10
            };

            panelButtons.Controls.Add(btnOk);
            panelButtons.Controls.Add(btnCancel);

            Controls.Add(panelButtons);
            Controls.Add(txtOpening);
            Controls.Add(lbl);
        }
    }
}
