using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using POS_qu.Controllers;
using POS_qu.Models;

namespace POS_qu
{
    public partial class ItemDetailForm : Form
    {
        private ItemController itemController;
        private int? editingItemId = null;
        private List<UnitVariant> unitVariantsFromForm = new List<UnitVariant>(); // Store globally if needed
        private Item _item;

        public ItemDetailForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            itemController = new ItemController();

            InitializeForm();
            ResetForm(); // Mode Tambah
        }

        public ItemDetailForm(Item item)
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            itemController = new ItemController();
            editingItemId = item.id;

            InitializeForm();
            LoadItem(item); // Mode Edit
        }

        // ------------------------
        // Inisialisasi Form Umum
        // ------------------------
        private void InitializeForm()
        {
            SetDefaultSettings();
            LoadCombos();

            // Event handler margin
            txtBuyPrice.TextChanged += (s, e) => UpdateMargin();
            txtSellPrice.TextChanged += (s, e) => UpdateMargin();
            txtDiscountFormula.TextChanged += (s, e) => UpdateMargin();

            // DataGridView harga bertingkat
            dgvMultiPrice.DataSource = new BindingList<ItemPrice>();
            dgvMultiPrice.Hide();
            dgvMultiPrice.ReadOnly = true;
        }

        private void SetDefaultSettings()
        {
            chk_is_inventory_p.Checked = true;
            chk_IsPurchasable.Checked = true;
            chk_IsSellable.Checked = true;
        }

        private void LoadCombos()
        {
            cmbUnit.DataSource = itemController.GetUnits();
            cmbUnit.DisplayMember = "display";
            cmbUnit.ValueMember = "id";

            cmbCategory.DataSource = itemController.GetCategories();
            cmbCategory.DisplayMember = "display";
            cmbCategory.ValueMember = "id";

            cmbSupplier.DataSource = itemController.GetSuppliers();
            cmbSupplier.DisplayMember = "display";
            cmbSupplier.ValueMember = "id";
        }

        // ------------------------
        // Reset / Clear Form (Tambah)
        // ------------------------
        private void ResetForm()
        {
            txtBarcode.Clear();
            txtName.Clear();
            txtBuyPrice.Text = "0";
            txtSellPrice.Text = "0";
            txtStock.Text = "0";
            txtNote.Clear();

            cmbUnit.SelectedIndex = -1;
            cmbCategory.SelectedIndex = -1;
            cmbSupplier.SelectedIndex = -1;

            SetDefaultSettings();

            txtBarcode.Focus();
        }

        // ------------------------
        // Load item untuk mode Edit
        // ------------------------
        private void LoadItem(Item item)
        {
            _item = item;
            txtName.Text = _item.name;
            txtBuyPrice.Text = _item.buy_price.ToString();
            txtSellPrice.Text = _item.sell_price.ToString();
            txtStock.Text = _item.stock.ToString();
            txtBarcode.Text = _item.barcode;
            txtNote.Text = _item.note;

            cmbUnit.SelectedValue = _item.unitid != 0 ? _item.unitid : -1;
            cmbCategory.SelectedValue = _item.category_id != 0 ? _item.category_id : -1;
            cmbSupplier.SelectedValue = _item.supplier_id != 0 ? _item.supplier_id : -1;

            chk_is_inventory_p.Checked = _item.is_inventory_p;
            chk_IsPurchasable.Checked = _item.IsPurchasable;
            chk_IsSellable.Checked = _item.IsSellable;
            chk_RequireNotePayment.Checked = _item.RequireNotePayment;
            chk_is_changeprice_p.Checked = _item.is_changeprice_p;
            chk_HasMaterials.Checked = _item.HasMaterials;
            chk_IsPackage.Checked = _item.IsPackage;
            chk_IsProduced.Checked = _item.IsProduced;

            // Ambil harga bertingkat
            DataTable dt = itemController.GetItemPrices(_item.id);

            // Convert DataTable ke BindingList<ItemPrice>
            var pricesList = new BindingList<ItemPrice>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    pricesList.Add(new ItemPrice
                    {
                        Id = Convert.ToInt32(row["id"]),
                        ItemId = Convert.ToInt32(row["item_id"]),
                        MinQty = Convert.ToInt32(row["min_qty"]),
                        Price = Convert.ToDecimal(row["price"])
                    });
                }
            }

            dgvMultiPrice.DataSource = pricesList;

            txtDiscountFormula.Text = _item.discount_formula;
        }


        // ------------------------
        // Update Margin
        // ------------------------
        private void UpdateMargin()
        {
            if (!decimal.TryParse(txtBuyPrice.Text, out decimal buyPrice)) return;
            if (!decimal.TryParse(txtSellPrice.Text, out decimal sellPrice)) return;

            string formula = txtDiscountFormula.Text.Trim();
            decimal discountAmount = 0;
            string discountText = "";

            if (!string.IsNullOrEmpty(formula))
            {
                if (formula.EndsWith("%") && decimal.TryParse(formula.TrimEnd('%'), out decimal percent))
                {
                    discountAmount = sellPrice * percent / 100m;
                    discountText = formula;
                }
                else if (decimal.TryParse(formula, out decimal nominal))
                {
                    discountAmount = nominal;
                    discountText = "Rp " + discountAmount.ToString("N0");
                }
            }
            else discountText = "0";

            decimal finalPrice = sellPrice - discountAmount;
            decimal margin = finalPrice - buyPrice;
            decimal marginPercent = buyPrice != 0 ? (margin / buyPrice) * 100 : 0;

            outDiskon.Text = discountText;
            outMargin.Text = $"{margin:N0} ({marginPercent:+0.##;-0.##}%)";
            outHargaAkhir.Text = finalPrice.ToString("N0");
        }

        // ------------------------
        // Tombol Simpan
        // ------------------------
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (_item == null) _item = new Item(); // jika mode tambah

            // Update properti dari form
            _item.name = txtName.Text;
            _item.buy_price = decimal.Parse(txtBuyPrice.Text);
            _item.sell_price = decimal.Parse(txtSellPrice.Text);
            _item.stock = int.Parse(txtStock.Text);
            _item.barcode = txtBarcode.Text;
            _item.note = txtNote.Text;
            _item.unitid = cmbUnit.SelectedValue != null ? Convert.ToInt32(cmbUnit.SelectedValue) : 0;
            _item.category_id = cmbCategory.SelectedValue != null ? Convert.ToInt32(cmbCategory.SelectedValue) : 0;
            _item.supplier_id = cmbSupplier.SelectedValue != null ? Convert.ToInt32(cmbSupplier.SelectedValue) : 0;
            _item.is_inventory_p = chk_is_inventory_p.Checked;
            _item.IsPurchasable = chk_IsPurchasable.Checked;
            _item.IsSellable = chk_IsSellable.Checked;
            _item.RequireNotePayment = chk_RequireNotePayment.Checked;
            _item.is_changeprice_p = chk_is_changeprice_p.Checked;
            _item.HasMaterials = chk_HasMaterials.Checked;
            _item.IsPackage = chk_IsPackage.Checked;
            _item.IsProduced = chk_IsProduced.Checked;
            _item.discount_formula = txtDiscountFormula.Text;

            // Update multi-price
            _item.Prices = ((BindingList<ItemPrice>)dgvMultiPrice.DataSource).ToList();

            // UnitVariants sudah ada di _item.UnitVariants (dari btnUnitVariant)
            _item.UnitVariants = unitVariantsFromForm;

            if (editingItemId == null)
            {
                var insertedId = itemController.InsertItem(_item);
                if (insertedId != null)
                {
                    _item.id = insertedId.Value; // simpan ID baru
                    MessageBox.Show("Item berhasil ditambahkan");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                _item.id = editingItemId.Value;
                if (itemController.UpdateItem(_item))
                {
                    MessageBox.Show("Item berhasil diperbarui");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }


        // ------------------------
        // Tombol Batal / Reset
        // ------------------------
        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Apakah Anda ingin membatalkan penginputan?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel; // optional, jika ingin menangani dari form pemanggil
                this.Close(); // tutup form
            }
            // jika No, form tetap terbuka
        }


        // ------------------------
        // Tombol Harga Bertingkat
        // ------------------------
        private void btnAddPrice_Click(object sender, EventArgs e)
        {
            var list = dgvMultiPrice.DataSource as BindingList<ItemPrice>;
            if (list == null) return;

            var newPrice = new ItemPrice { Id = 0, ItemId = editingItemId ?? 0, MinQty = 1, Price = 0 };
            string itemName = txtName.Text;
            decimal buyPrice = decimal.TryParse(txtBuyPrice.Text, out decimal bp) ? bp : 0;

            if (ShowPriceEditor(newPrice, itemName, buyPrice))
            {
                list.Add(newPrice);
                dgvMultiPrice.Refresh();
            }
        }

        private void btnEditPrice_Click(object sender, EventArgs e)
        {
            if (dgvMultiPrice.CurrentRow?.DataBoundItem is not ItemPrice price) return;

            string itemName = txtName.Text;
            decimal buyPrice = decimal.TryParse(txtBuyPrice.Text, out decimal bp) ? bp : 0;

            if (ShowPriceEditor(price, itemName, buyPrice))
            {
                dgvMultiPrice.Refresh();
            }
        }

        private void btnDeletePrice_Click(object sender, EventArgs e)
        {
            var list = dgvMultiPrice.DataSource as BindingList<ItemPrice>;
            if (list == null || dgvMultiPrice.CurrentRow?.DataBoundItem is not ItemPrice price) return;

            if (MessageBox.Show("Hapus harga ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                list.Remove(price);
            }
        }

        // ------------------------
        // ShowPriceEditor Modular
        // ------------------------
        private bool ShowPriceEditor(ItemPrice price, string itemName, decimal buyPrice)
        {
            using Form popup = new Form()
            {
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Width = 420,
                Height = 350,
                Text = "Edit Harga Bertingkat"
            };

            Label lblItemName = new Label() { Left = 20, Top = 15, Width = 360, Text = "Item: " + itemName, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            Label lblMinQty = new Label() { Left = 20, Top = 60, Text = "Min Qty", AutoSize = true };
            TextBox txtMinQty = new TextBox() { Left = 150, Top = 58, Width = 200, Text = price.MinQty.ToString() };
            Label lblPrice = new Label() { Left = 20, Top = 100, Text = "Harga", AutoSize = true };
            TextBox txtPrice = new TextBox() { Left = 150, Top = 98, Width = 200, Text = price.Price.ToString() };

            Label lblFinalPrice = new Label() { Left = 20, Top = 150, Width = 350, Text = "Harga Akhir: -" };
            Label lblMargin = new Label() { Left = 20, Top = 180, Width = 350, Text = "Margin: -" };

            void Recalculate()
            {
                if (!decimal.TryParse(txtPrice.Text, out decimal sellPrice))
                {
                    lblFinalPrice.Text = "Harga Akhir: -";
                    lblMargin.Text = "Margin: -";
                    return;
                }

                decimal margin = sellPrice - buyPrice;
                decimal marginPercent = buyPrice > 0 ? (margin / buyPrice) * 100 : 0;

                lblFinalPrice.Text = $"Harga Akhir: {sellPrice:N0}";
                lblMargin.Text = $"Margin: {margin:N0} ({marginPercent:+0.##;-0.##}%)";
            }

            txtPrice.TextChanged += (s, e) => Recalculate();
            Recalculate();

            Button btnOK = new Button() { Left = 150, Top = 240, Width = 90, Height = 45, Text = "OK", DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Left = 260, Top = 240, Width = 90, Height = 45, Text = "Batal", DialogResult = DialogResult.Cancel };

            popup.Controls.AddRange(new Control[] { lblItemName, lblMinQty, txtMinQty, lblPrice, txtPrice, lblFinalPrice, lblMargin, btnOK, btnCancel });
            popup.AcceptButton = btnOK;
            popup.CancelButton = btnCancel;

            if (popup.ShowDialog() == DialogResult.OK)
            {
                if (int.TryParse(txtMinQty.Text, out int minQty)) price.MinQty = minQty;
                if (decimal.TryParse(txtPrice.Text, out decimal harga)) price.Price = harga;
                return true;
            }

            return false;
        }

        private void btnMultiPrice_Click(object sender, EventArgs e)
        {
            // Toggle visibility dgvMultiPrice
            dgvMultiPrice.Visible = !dgvMultiPrice.Visible;

            // Optional: ubah teks tombol sesuai status
            btnMultiPrice.Text = dgvMultiPrice.Visible ? "Sembunyikan Multi Harga" : "Tampilkan Multi Harga";
        }

        private void btnUnitVariant_Click(object sender, EventArgs e)
        {
            bool isItemNameValid = !string.IsNullOrWhiteSpace(txtName.Text);
            bool isUnitSelected = cmbUnit.SelectedIndex >= 0;

            if (!isItemNameValid || !isUnitSelected)
            {
                btnUnitVariant.Cursor = Cursors.No; // Stop sign cursor
                MessageBox.Show("Please enter item name and select a unit first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnUnitVariant.Cursor = Cursors.Default;

            using (var variantForm = new UnitVariantForm(_item, unitVariantsFromForm))
            {
                var result = variantForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    unitVariantsFromForm = variantForm._item.UnitVariants;
                    _item.UnitVariants = variantForm._item.UnitVariants; // kalau mau langsung update item
                }
            }
        }

  
    }
}
