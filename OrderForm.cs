using POS_qu.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_qu.Models;

namespace POS_qu
{
    public partial class OrderForm : Form
    {
        public bool IsSaved { get; private set; } = false;

        public OrderForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            // Isi contoh data untuk combobox (bisa diganti ambil dari database)
            comboBoxOrderType.Items.AddRange(new string[] { "Pending", "Processing", "Completed", "Cancelled" });
            //comboBoxPaymentMethod.Items.AddRange(new string[] { "Cash", "Card", "QRIS", "Bank Transfer", "COD" });
            comboBoxDeliveryMethod.Items.AddRange(new string[] { "Di Antar", "Di Ambil Sendiri" });

            comboBoxOrderType.SelectedIndex = 0;
            //comboBoxPaymentMethod.SelectedIndex = 0;
            comboBoxDeliveryMethod.SelectedIndex = 0;


            // Generate nomor order otomatis saat form load
            textBoxOrderNumber.Text = GenerateOrderNumber();
        }

        private void ComboBoxDeliveryMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show dateTimePickerDeliveryTime hanya jika metode pengantaran "Di Antar"
            if (comboBoxDeliveryMethod.SelectedItem != null &&
                comboBoxDeliveryMethod.SelectedItem.ToString() == "Di Antar")
            {
                dateTimePickerDeliveryTime.Visible = true;
                //labelDeliveryTime.Visible = true;
            }
            else
            {
                dateTimePickerDeliveryTime.Visible = false;
                //labelDeliveryTime.Visible = false;
            }
        }


        private string GenerateOrderNumber()
        {
            return "ORD-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

  

        // Method untuk ambil data order dari form, bisa dipanggil dari luar
        public Orders GetOrder()
        {
            return new Orders
            {
                OrderNumber = textBoxOrderNumber.Text.Trim(),
                OrderCode = Guid.NewGuid().ToString(),
                OrderTotal = 0m, // Bisa update sesuai kebutuhan
                OrderStatus = comboBoxOrderType.SelectedIndex,
                PaymentMethod = "", // Sudah dihilangkan dari form, jadi kosong
                DeliveryMethod = comboBoxDeliveryMethod.SelectedItem?.ToString(),
                DeliveryTime = comboBoxDeliveryMethod.SelectedItem?.ToString() == "Di Antar" ? (DateTime?)dateTimePickerDeliveryTime.Value : null,
                OrderNote = richTextBoxKeterangan.Text.Trim(),
                CustomerName = textBoxCustomer.Text.Trim(),
                CustomerPhone = textBoxNoHp.Text.Trim(),
                TerminalId = 1, // Sesuaikan context session kamu
                ShiftId = 1,
                UserId = 1,
                CreatedBy = 1
            };
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCustomer.Text))
            {
                MessageBox.Show("Customer harus diisi!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Jangan simpan di sini, cukup set DialogResult dan tutup form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

  

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (!IsSaved)
            {
                var result = MessageBox.Show("Order belum disimpan, tetap keluar?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }
            this.Close();
        }

        private void labelKeterangan_Click(object sender, EventArgs e)
        {

        }
    }
}
