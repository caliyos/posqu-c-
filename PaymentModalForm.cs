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
    public partial class PaymentModalForm : Form
    {
        private decimal totalAmount;
        public PaymentModalForm(decimal totalAmount)
        {
            InitializeComponent();
            // Display the total in the label
            lblTotal.Text = "Total: " + totalAmount.ToString("C");
            this.totalAmount = totalAmount;

            // Default cashback to 0
            txtCashback.Text = "0";
        }
        // Property to get the payment details
        public decimal PaymentAmount => Convert.ToDecimal(txtPaymentAmount.Text);
        public decimal Cashback
        {
            get
            {
                // Remove currency symbols and commas before parsing
                string cleanText = txtCashback.Text.Replace("$", "").Replace(",", "");

                // Try to parse the clean text to decimal
                if (decimal.TryParse(cleanText, out decimal cashback))
                {
                    return cashback;
                }

                // If parsing fails, return 0 as a default value
                return 0;
            }
        }
        public string PaymentMethod => cmbPaymentMethod.SelectedItem?.ToString() ?? "Cash";

        // Event handler for when the 'Pay' button is clicked
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (txtPaymentAmount.Text == string.Empty || cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            // Calculate cashback as PaymentAmount - Total
            decimal paymentAmount = PaymentAmount;
            decimal cashback = paymentAmount - totalAmount;

            // Update the cashback field with the calculated value
            txtCashback.Text = cashback.ToString("C");

            // Close the modal and return the result
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        // Event handler for when the payment amount changes (recalculate cashback)
        private void txtPaymentAmount_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Run.");
            // Only recalculate if the payment amount is valid
            if (decimal.TryParse(txtPaymentAmount.Text, out decimal paymentAmount))
            {
                decimal cashback = paymentAmount - this.totalAmount;
                txtCashback.Text = cashback >= 0 ? cashback.ToString("C") : "0";
            }
            else
            {
                txtCashback.Text = "0";
            }
        }

        private void PaymentModalForm_Load(object sender, EventArgs e)
        {

        }

        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
