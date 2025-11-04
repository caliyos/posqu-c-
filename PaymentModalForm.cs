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
            lblTotal.Text = "Total: " + totalAmount.ToString("C");
            this.totalAmount = totalAmount;

            //txtCashback.Text = "0";
        }

        // ================================
        // ✅ EVENTS untuk komunikasi ke form utama
        // ================================
        public event Action<decimal> GlobalDiscountChanged;
        public event Action<decimal> PaymentAmountChanged;
        public event Action<string> GlobalNoteChanged;         // ✅ NEW
        public event Action<decimal> DeliveryAmountChanged;    // ✅ NEW

        // ================================
        // PROPERTIES
        // ================================
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

                return 0;
            }
        }

        //public decimal Cashback
        //{
        //    get
        //    {
        //        string cleanText = txtCashback.Text.Replace("Rp. ", "").Replace(",", "");
        //        if (decimal.TryParse(cleanText, out decimal cashback))
        //            return cashback;
        //        return 0;
        //    }
        //}

        public string PaymentMethod => cmbPaymentMethod.SelectedItem?.ToString() ?? "Cash";

        public decimal GlobalDiscountPercent { get; private set; }
        public decimal GrandTotal { get; private set; }

        // ✅ NEW: Properti untuk Global Note dan Delivery Amount
        public string GlobalNote => txtGlobalNote?.Text?.Trim() ?? "";
        public decimal DeliveryAmount
        {
            get
            {
                if (decimal.TryParse(txtDeliveryAmount.Text, out decimal value))
                    return value;
                return 0;
            }
        }

        // ================================
        // EVENTS HANDLER
        // ================================
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (txtPaymentAmount.Text == string.Empty || cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            decimal paymentAmount = PaymentAmount;
            //decimal cashback = paymentAmount - totalAmount;

            //txtCashback.Text = cashback.ToString("C");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtPaymentAmount_TextChanged(object sender, EventArgs e)
        {
            //if (decimal.TryParse(txtPaymentAmount.Text, out decimal paymentAmount))
            //{
            //    decimal total = GrandTotal > 0 ? GrandTotal : totalAmount;
            //    decimal cashback = paymentAmount - total;
            //    txtCashback.Text = cashback > 0 ? cashback.ToString("C") : "$0.00";
            //}
            //else
            //{
            //    txtCashback.Text = "$0.00";
            //}

            PaymentAmountChanged?.Invoke(PaymentAmount);
        }

        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbPaymentMethod.SelectedItem.ToString();

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
            }
        }

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

            GlobalDiscountPercent = discountPercent;
            GrandTotal = discountedTotal;

            GlobalDiscountChanged?.Invoke(discountPercent);

            //lblGrandTotal.Text = "Grand Total: " + discountedTotal.ToString("C");

        }

        // ✅ NEW: Global Note change event
        private void txtGlobalNote_TextChanged(object sender, EventArgs e)
        {
            GlobalNoteChanged?.Invoke(GlobalNote);
        }

        // ✅ NEW: Delivery Amount change event
        private void txtDeliveryAmount_TextChanged(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtDeliveryAmount.Text, out decimal delivery))
                delivery = 0;

            DeliveryAmountChanged?.Invoke(delivery);
        }

        private void PaymentModalForm_Load(object sender, EventArgs e)
        {
            // Optional initialization
        }
    }
}

