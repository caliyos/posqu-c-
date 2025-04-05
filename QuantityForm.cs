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
    public partial class QuantityForm : Form
    {
        public int Quantity { get; private set; }
        public QuantityForm(int availableStock)
        {
            InitializeComponent();
            lblStockAvailable.Text = $"Stock Available: {availableStock}";
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtQuantity.Text, out int enteredQuantity))
            {
                if (enteredQuantity > 0)
                {
                    Quantity = enteredQuantity;
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QuantityForm_Load(object sender, EventArgs e)
        {

        }
    }
}
