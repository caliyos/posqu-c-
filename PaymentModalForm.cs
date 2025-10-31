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
        public decimal PaymentAmount
        {
            get
            {
                string input = txtPaymentAmount.Text
                    .Replace("Rp", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Trim();

                if (decimal.TryParse(input, out decimal result))
                    return result;

                return 0; // default kalau parsing gagal
            }
        }
        public decimal Cashback
        {
            get
            {
                // Remove currency symbols and commas before parsing
                string cleanText = txtCashback.Text.Replace("Rp. ", "").Replace(",", "");

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
            if (decimal.TryParse(txtPaymentAmount.Text, out decimal paymentAmount))
            {
                decimal totalAmount = GrandTotal > 0 ? GrandTotal : this.totalAmount;

                decimal cashback = paymentAmount - totalAmount;
                txtCashback.Text = cashback > 0 ? cashback.ToString("C") : "$0.00";
            }
            else
            {
                txtCashback.Text = "$0.00";
            }

            // 🔥 Panggil event ke form utama agar bisa update kalkulasi global
            PaymentAmountChanged?.Invoke(PaymentAmount);
        }


        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbPaymentMethod.SelectedItem.ToString();

            // Hide all panels first
            panelCardDetails.Visible = false;
            panelEwalletDetails.Visible = false;
            panelBankTransfer.Visible = false;
            panelSplitPayment.Visible = false;

            switch (selected)
            {
                case "Card":
                    panelCardDetails.Visible = true;
                    break;
                case "QRIS":
                    panelEwalletDetails.Visible = true;
                    break;
                case "Bank Transfer":
                    panelBankTransfer.Visible = true;
                    break;
                case "Split Payment":
                    panelSplitPayment.Visible = true;
                    break;
                    // Cash doesn't show a panel
            }
        }



        private void PaymentModalForm_Load(object sender, EventArgs e)
        {

        }

        public event Action<decimal> GlobalDiscountChanged;
        public event Action<decimal> PaymentAmountChanged;

        public decimal GlobalDiscountPercent { get; private set; }
        public decimal GrandTotal { get; private set; }

        private void txtGlobalDiscountPercent_TextChanged(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtGlobalDiscountPercent.Text, out decimal discountPercent))
                discountPercent = 0;

            if (discountPercent > 100)
            {
                discountPercent = 100;
                txtGlobalDiscountPercent.Text = "100";
            }

            decimal discountedTotal = totalAmount - (totalAmount * discountPercent / 100);

            // Simpan ke properti publik
            GlobalDiscountPercent = discountPercent;
            GrandTotal = discountedTotal;

            // 🔥 Kirim event ke form utama (agar panggil CalculateAllTotals)
            GlobalDiscountChanged?.Invoke(discountPercent);

            // Update tampilan
            lblGrandTotal.Text = "Grand Total: " + discountedTotal.ToString("C");

            if (decimal.TryParse(txtPaymentAmount.Text, out decimal paymentAmount))
            {
                decimal cashback = paymentAmount - discountedTotal;
                txtCashback.Text = cashback > 0 ? cashback.ToString("C") : "$0.00";
            }
        }




    }
}
