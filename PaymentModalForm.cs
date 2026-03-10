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
        public bool IsSplitPayment
        {
            get
            {
                return PaymentMethod == "Split Payment";
            }
        }
        public IEnumerable<(string Method, decimal Amount)>? SplitPayments { get; set; }

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
            if (cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            if (IsSplitPayment)
            {
                decimal cash = 0, card = 0;
                if (txtCashPart != null && !string.IsNullOrWhiteSpace(txtCashPart.Text))
                    cash = ParseMoney(txtCashPart.Text);
                if (txtCardPart != null && !string.IsNullOrWhiteSpace(txtCardPart.Text))
                    card = ParseMoney(txtCardPart.Text);

                if (cash <= 0 && card <= 0)
                {
                    MessageBox.Show("Isi nominal split payment (Cash/Card).");
                    return;
                }
                var parts = new List<(string Method, decimal Amount)>();
                if (cash > 0) parts.Add(("Cash", cash));
                if (card > 0) parts.Add(("Card", card));
                SplitPayments = parts;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtPaymentAmount.Text))
                {
                    MessageBox.Show("Masukkan nominal pembayaran.");
                    return;
                }
                var _ = PaymentAmount; // trigger parse
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtPaymentAmount_TextChanged(object sender, EventArgs e)
        {

            PaymentAmountChanged?.Invoke(PaymentAmount);
            UpdatePayButtonState();
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
            UpdatePayButtonState();
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

            GlobalDiscountPercent = discountPercent;

            GlobalDiscountChanged?.Invoke(discountPercent);
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

        private void UpdatePayButtonState()
        {
            decimal payment = PaymentAmount;
            decimal change = payment - totalAmount;

            bool valid = change > 0 && cmbPaymentMethod.SelectedItem != null;

            btnPay.Enabled = valid;
        }

        private static decimal ParseMoney(string text)
        {
            string clean = text.Replace("Rp", "")
                               .Replace(".", "")
                               .Replace(",", "")
                               .Trim();
            if (decimal.TryParse(clean, out var value)) return value;
            return 0;
        }

    }
}
