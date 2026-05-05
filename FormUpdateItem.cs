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
        private decimal _unitPrice;
        private bool _discountIsAmount;
        private bool _isUpdatingPreview;

        public FormUpdateItem(InvoiceData invoice, InvoiceItem item, ICartService cartService, Action<InvoiceData> onUpdate)
        {
            InitializeComponent();

            _item = item;
            _cartService = cartService;
            _onUpdate = onUpdate;
            _invoiceData = invoice;

            _unitPrice = _item.Price;
            lblName.Text = _item.Name ?? "-";
            lblPrice.Text = "Rp " + _unitPrice.ToString("N0");
            lblSatuan.Text = string.IsNullOrWhiteSpace(_item.UnitVariant) ? (_item.Unit ?? "-") : _item.UnitVariant;
            numQty.Value = _item.Qty > 0 ? _item.Qty : 1;

            numDiscPercent.Minimum = 0;
            numDiscPercent.Maximum = 100;
            numDiscPercent.DecimalPlaces = 2;
            numDiscPercent.Increment = 0.25M;
            numDiscPercent.Value = _item.DiscountPercent < 0 ? 0 : (_item.DiscountPercent > 100 ? 100 : _item.DiscountPercent);

            numDiscAmount.DecimalPlaces = 0;
            numDiscAmount.Increment = 1000M;
            numDiscAmount.Minimum = 0;
            numDiscAmount.Maximum = 1000000000M;
            numDiscAmount.Value = _item.DiscountAmount < 0 ? 0 : _item.DiscountAmount;

            _discountIsAmount = (_item.DiscountAmount > 0m && _item.DiscountPercent <= 0m);
            rdoDiscAmount.Checked = _discountIsAmount;
            rdoDiscPercent.Checked = !_discountIsAmount;

            txtNote.Text = _item.Note ?? "";

            numQty.ValueChanged += (_, __) => RecalcPreview();
            numDiscPercent.ValueChanged += (_, __) => RecalcPreview();
            numDiscAmount.ValueChanged += (_, __) => RecalcPreview();
            rdoDiscPercent.CheckedChanged += (_, __) => { _discountIsAmount = !rdoDiscPercent.Checked; RecalcPreview(); };
            rdoDiscAmount.CheckedChanged += (_, __) => { _discountIsAmount = rdoDiscAmount.Checked; RecalcPreview(); };

            RecalcPreview();
        }

        private void RecalcPreview()
        {
            try
            {
                if (_isUpdatingPreview) return;
                _isUpdatingPreview = true;
                var qty = (int)numQty.Value;
                if (qty < 0) qty = 0;
                decimal subTotal = _unitPrice * qty;
                if (subTotal < 0m) subTotal = 0m;

                decimal discAmount;
                decimal discPercent;
                if (_discountIsAmount)
                {
                    discAmount = numDiscAmount.Value;
                    if (discAmount < 0m) discAmount = 0m;
                    if (discAmount > subTotal) discAmount = subTotal;
                    discPercent = subTotal <= 0m ? 0m : Math.Round((discAmount * 100m) / subTotal, 2, MidpointRounding.AwayFromZero);
                    if (discPercent < 0m) discPercent = 0m;
                    if (discPercent > 100m) discPercent = 100m;
                    if (numDiscPercent.Enabled == true) { }
                    numDiscPercent.Enabled = false;
                    numDiscAmount.Enabled = true;
                    numDiscPercent.Value = discPercent;
                }
                else
                {
                    discPercent = numDiscPercent.Value;
                    if (discPercent < 0m) discPercent = 0m;
                    if (discPercent > 100m) discPercent = 100m;
                    discAmount = Math.Round((subTotal * discPercent) / 100m, 2, MidpointRounding.AwayFromZero);
                    if (discAmount < 0m) discAmount = 0m;
                    if (discAmount > subTotal) discAmount = subTotal;
                    numDiscPercent.Enabled = true;
                    numDiscAmount.Enabled = false;
                    numDiscAmount.Value = discAmount;
                }

                decimal total = subTotal - discAmount;

                lblLineTotal.Text = "Rp " + total.ToString("N0");
            }
            catch
            {
            }
            finally
            {
                _isUpdatingPreview = false;
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

                var discPercent = (decimal)numDiscPercent.Value;
                if (discPercent < 0m) discPercent = 0m;
                if (discPercent > 100m) discPercent = 100m;
                var discAmount = (decimal)numDiscAmount.Value;
                if (discAmount < 0m) discAmount = 0m;
                var note = txtNote.Text?.Trim() ?? "";

                if (_discountIsAmount)
                {
                    discPercent = 0m;
                }
                else
                {
                    discAmount = 0m;
                }

                var updatedInvoice = _cartService.UpdateItemQtyWithMeta(_item.pt_id, newQty, discPercent, discAmount, note, _invoiceData);

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
