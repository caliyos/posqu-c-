using POS_qu.Core.Interfaces;
using POS_qu.Models;
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
    public partial class FormUpdateItem : Form
    {
        private InvoiceItem _item;
        private InvoiceData _invoiceData;
        private ICartService _cartService;
        private Action<InvoiceData> _onUpdate;

        public FormUpdateItem(InvoiceData invoice, InvoiceItem item, ICartService cartService, Action<InvoiceData> onUpdate)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            _item = item;
            _cartService = cartService;
            _onUpdate = onUpdate;
            _invoiceData = invoice;
            // Set field
            lblName.Text = _item.Name;
            lblPrice.Text = _item.Price.ToString("N0");
            lblSatuan.Text = _item.Unit;
            lblQty.Text = _item.Qty.ToString();
            numQty.Value = _item.Qty;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int newQty = (int)numQty.Value;

            // Update DB via CartService
            _invoiceData = _cartService.UpdateItemQty(_item.pt_id, newQty,_invoiceData);

            _onUpdate?.Invoke(_invoiceData);

            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Yakin ingin hapus item ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var updatedInvoice = _cartService.RemoveItem(_item.ItemId,_item.UnitId, _invoiceData,false);

                _onUpdate?.Invoke(updatedInvoice);

                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            // Konfirmasi hapus
            var result = MessageBox.Show(
                "Apakah Anda yakin ingin menghapus item ini?",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                // Ambil item dari Tag atau variabel form
                var itemToRemove = _item; // pastikan _item adalah InvoiceItem yang sedang di modal

                // Hapus dari database via CartService
                var updatedInvoice = _cartService.RemoveItem(itemToRemove.ItemId, itemToRemove.UnitId, _invoiceData, false);

                // Update _currentInvoice di main form lewat callback
                _onUpdate?.Invoke(updatedInvoice);

                // Tutup modal
                this.Close();
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Ambil qty baru dari TextBox / NumericUpDown
                int newQty = (int)numQty.Value;

                if (newQty <= 0)
                {
                    MessageBox.Show("Qty harus lebih dari 0", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var invoicedebug = _invoiceData;
                //MessageBox.Show("here", _invoiceData);
                // Update item via CartService
                var updatedInvoice = _cartService.UpdateItemQty(_item.pt_id, newQty, _invoiceData);

                // Callback ke form utama untuk refresh
                _onUpdate?.Invoke(updatedInvoice);

                // Tutup modal
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal update item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}
