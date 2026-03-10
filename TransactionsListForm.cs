using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public class TransactionsListForm : Form
    {
        public TransactionsListForm()
        {
            Text = "Daftar Transaksi";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterParent;
            Controls.Add(new Label
            {
                Text = "Daftar Transaksi belum diimplementasikan.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
        }
    }
}
