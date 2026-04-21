using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using POS_qu.Controllers;
using POS_qu.Models;
using POS_qu.Core.Interfaces;
using POS_qu.Services;
using POS_qu.Repositories;
using POS_qu.Helpers;
using Npgsql;

namespace POS_qu
{
    public partial class ItemDetailForm : Form
    {
        private IProductService _productService;
        private int? editingItemId = null;
        private List<UnitVariant> unitVariantsFromForm = new List<UnitVariant>(); // Store globally if needed
        private Item _item;

        public ItemDetailForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            _productService = new ProductService(new ProductRepository());
            // MODE TAMBAH
            label8.Text = "Stock Awal";     // ✅
            txtStock.Enabled = true;       // ✅ BISA INPUT
            txtStock.ReadOnly = false;

            InitializeForm();
            ResetForm(); // Mode Tambah
        }

        public ItemDetailForm(Item item)
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            _productService = new ProductService(new ProductRepository());
            _item = item;
            editingItemId = item.id;
            // DISABLE STOK DI MODE EDIT
            txtStock.Enabled = false;     // ✅ yang benar
            txtStock.ReadOnly = true;     // ✅ tambahan safety
            label8.Text = "Stock Sekarang";
            if (cmbWarehouse != null) cmbWarehouse.Enabled = false;

            InitializeForm();
            LoadItem(item); // Mode Edit
           
        }

        private void InitializeForm()
        {
            SetDefaultSettings();
            LoadCombos();
            
            ApplyProfessionalStyle();

            // Event handler margin
            txtBuyPrice.TextChanged += (s, e) => UpdateMargin();
            txtSellPrice.TextChanged += (s, e) => UpdateMargin();
            txtDiscountFormula.TextChanged += (s, e) => UpdateMargin();

            // DataGridView harga bertingkat
            dgvMultiPrice.DataSource = new BindingList<ItemPrice>();
            dgvMultiPrice.AutoGenerateColumns = false;
            dgvMultiPrice.Columns.Clear();
            dgvMultiPrice.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UnitName", HeaderText = "Satuan", Width = 150 });
            dgvMultiPrice.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PriceLevelName", HeaderText = "Level Harga", Width = 150 });
            dgvMultiPrice.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MinQty", HeaderText = "Min Qty", Width = 100 });
            dgvMultiPrice.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaxQty", HeaderText = "Max Qty", Width = 100 });
            dgvMultiPrice.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Price", HeaderText = "Harga Jual", Width = 200, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvMultiPrice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvMultiPrice.ReadOnly = true;
            dgvMultiPrice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMultiPrice.AllowUserToAddRows = false;
            dgvMultiPrice.BackgroundColor = Color.White;
            dgvMultiPrice.BorderStyle = BorderStyle.None;
            dgvMultiPrice.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMultiPrice.GridColor = Color.FromArgb(235, 235, 235);
            dgvMultiPrice.RowHeadersVisible = false;
            dgvMultiPrice.RowTemplate.Height = 45;
            dgvMultiPrice.EnableHeadersVisualStyles = false;
            
            var headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(245, 245, 245);
            headerStyle.ForeColor = Color.FromArgb(80, 80, 80);
            headerStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            headerStyle.Padding = new Padding(5, 0, 5, 0);
            dgvMultiPrice.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvMultiPrice.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvMultiPrice.ColumnHeadersHeight = 50;
            dgvMultiPrice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            var cellStyle1 = new DataGridViewCellStyle();
            cellStyle1.BackColor = Color.White;
            cellStyle1.ForeColor = Color.FromArgb(50, 50, 50);
            cellStyle1.Font = new Font("Segoe UI", 11F);
            cellStyle1.SelectionBackColor = Color.FromArgb(240, 248, 255);
            cellStyle1.SelectionForeColor = Color.Black;
            cellStyle1.Padding = new Padding(5, 0, 5, 0);
            dgvMultiPrice.DefaultCellStyle = cellStyle1;

            var cellStyleAlt = new DataGridViewCellStyle();
            cellStyleAlt.BackColor = Color.FromArgb(252, 252, 252);
            dgvMultiPrice.AlternatingRowsDefaultCellStyle = cellStyleAlt;

            cmbValuation.Items.Clear();
            cmbValuation.Items.Add("FIFO");
            cmbValuation.Items.Add("AVG");
            if (cmbValuation.SelectedIndex < 0) cmbValuation.SelectedIndex = 0;

            txtStock.TextChanged += (s, e) => UpdateStockOutput();
            cmbUnit.SelectedIndexChanged += (s, e) => UpdateStockOutput();
            txtBuyPrice.TextChanged += (s, e) => UpdateStockOutput();
            txtSellPrice.TextChanged += (s, e) => UpdateStockOutput();
            UpdateStockOutput();
            cmbSort.SelectedIndexChanged += cmbSort_SelectedIndexChanged;
        }

        private void ApplyProfessionalStyle()
        {
            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            
            // Buttons
            btnSimpan.BackColor = Color.FromArgb(0, 122, 255);
            btnSimpan.ForeColor = Color.White;
            btnSimpan.FlatStyle = FlatStyle.Flat;
            btnSimpan.FlatAppearance.BorderSize = 0;
            btnSimpan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            
            btnAddPrice.BackColor = Color.FromArgb(40, 167, 69);
            btnAddPrice.ForeColor = Color.White;
            btnAddPrice.FlatStyle = FlatStyle.Flat;
            btnAddPrice.FlatAppearance.BorderSize = 0;
            
            btnEditPrice.BackColor = Color.FromArgb(255, 193, 7);
            btnEditPrice.ForeColor = Color.Black;
            btnEditPrice.FlatStyle = FlatStyle.Flat;
            btnEditPrice.FlatAppearance.BorderSize = 0;
            
            btnDeletePrice.BackColor = Color.FromArgb(220, 53, 69);
            btnDeletePrice.ForeColor = Color.White;
            btnDeletePrice.FlatStyle = FlatStyle.Flat;
            btnDeletePrice.FlatAppearance.BorderSize = 0;
            
            btnUnitVariant.BackColor = Color.DodgerBlue;
            btnUnitVariant.ForeColor = Color.White;
            btnUnitVariant.FlatStyle = FlatStyle.Flat;
            btnUnitVariant.FlatAppearance.BorderSize = 0;
            btnUnitVariant.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            
            btnAddPrice.Text = "Tambah Harga";
            btnEditPrice.Text = "Edit Harga";
            btnDeletePrice.Text = "Hapus Harga";
        }

        private void SetDefaultSettings()
        {
            chk_is_inventory_p.Checked = true;
            chk_IsPurchasable.Checked = true;
            chk_IsSellable.Checked = true;
        }

        private void LoadUnits(string sortBy)
        {
            var dt = _productService.GetUnits();

            string sort = sortBy switch
            {
                "name" => "display ASC",
                "ord" => "ord ASC", // ⚠️ pastikan kolom ord ada di query
                _ => "id ASC"
            };

            DataView dv = dt.DefaultView;
            dv.Sort = sort;

            cmbUnit.DataSource = dv;
            cmbUnit.DisplayMember = "display";
            cmbUnit.ValueMember = "id";
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUnits(cmbSort.SelectedItem.ToString().ToLower());
        }


        private void LoadCombos()
        {
            cmbSort.Items.Clear();
            cmbSort.Items.AddRange(new string[] { "Id", "Name", "Ord" });
            LoadUnits("id");

            cmbUnit.DisplayMember = "display";
            cmbUnit.ValueMember = "id";
            if (cmbUnit.Items.Count > 0 && cmbUnit.SelectedIndex < 0)
            {
                cmbUnit.SelectedIndex = 0;
            }

            cmbCategory.DataSource = _productService.GetCategories();
            cmbCategory.DisplayMember = "display";
            cmbCategory.ValueMember = "id";

            cmbSupplier.DataSource = _productService.GetSuppliers();
            cmbSupplier.DisplayMember = "display";
            cmbSupplier.ValueMember = "id";

            // Tambahan Brand, Rack, Warehouse
            cmbBrand.DataSource = _productService.GetBrands();
            cmbBrand.DisplayMember = "display";
            cmbBrand.ValueMember = "id";

            cmbRack.DataSource = _productService.GetRacks();
            cmbRack.DisplayMember = "display";
            cmbRack.ValueMember = "id";

            // Gunakan metode yang sama atau langsung NpgsqlCommand untuk warehouse
            // Asumsikan WarehouseController atau langsung query
            LoadWarehouses();
        }

        private void LoadWarehouses()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using var adapter = new NpgsqlDataAdapter("SELECT id, name FROM warehouses ORDER BY name ASC", conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                cmbWarehouse.DataSource = dt;
                cmbWarehouse.DisplayMember = "name";
                cmbWarehouse.ValueMember = "id";
            }
            catch {}
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
            if (cmbUnit.SelectedValue == null || (cmbUnit.SelectedValue is int v && v <= 0))
            {
                if (cmbUnit.Items.Count > 0) cmbUnit.SelectedIndex = 0;
            }
            cmbCategory.SelectedValue = _item.category_id != 0 ? _item.category_id : -1;
            cmbSupplier.SelectedValue = _item.supplier_id != 0 ? _item.supplier_id : -1;

            if (_item.brand_id.HasValue && _item.brand_id > 0)
                cmbBrand.SelectedValue = _item.brand_id.Value;

            if (_item.rack_id.HasValue && _item.rack_id > 0)
                cmbRack.SelectedValue = _item.rack_id.Value;

            chk_is_inventory_p.Checked = _item.is_inventory_p;
            chk_IsPurchasable.Checked = _item.IsPurchasable;
            chk_IsSellable.Checked = _item.IsSellable;
            chk_RequireNotePayment.Checked = _item.RequireNotePayment;
            chk_is_changeprice_p.Checked = _item.is_changeprice_p;
            chk_HasMaterials.Checked = _item.HasMaterials;
            chk_IsPackage.Checked = _item.IsPackage;
            chk_IsProduced.Checked = _item.IsProduced;

            var prices = _productService.GetItemPrices(_item.id);
            dgvMultiPrice.DataSource = new BindingList<ItemPrice>(prices);
            _item.Prices = prices; // Ensure it's in the object too

            if (_item.ExpiredAt.HasValue)
                dtpExpired.Value = _item.ExpiredAt.Value;

            if (!string.IsNullOrEmpty(_item.valuation_method))
            {
                int idx = cmbValuation.FindStringExact(_item.valuation_method);
                cmbValuation.SelectedIndex = idx >= 0 ? idx : 0;
            }

            UpdateStockOutput();
            LoadUnitVariantsUI();
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
            if (_item.unitid <= 0)
            {
                MessageBox.Show("Pilih unit terlebih dahulu.");
                cmbUnit.Focus();
                return;
            }
            _item.category_id = cmbCategory.SelectedValue != null ? Convert.ToInt32(cmbCategory.SelectedValue) : 0;
            _item.supplier_id = cmbSupplier.SelectedValue != null ? Convert.ToInt32(cmbSupplier.SelectedValue) : 0;
            _item.brand_id = cmbBrand.SelectedValue != null ? Convert.ToInt32(cmbBrand.SelectedValue) : null;
            _item.rack_id = cmbRack.SelectedValue != null ? Convert.ToInt32(cmbRack.SelectedValue) : null;
            _item.initial_warehouse_id = cmbWarehouse.SelectedValue != null ? Convert.ToInt32(cmbWarehouse.SelectedValue) : 1;
            
            _item.is_inventory_p = chk_is_inventory_p.Checked;
            _item.IsPurchasable = chk_IsPurchasable.Checked;
            _item.IsSellable = chk_IsSellable.Checked;
            _item.RequireNotePayment = chk_RequireNotePayment.Checked;
            _item.is_changeprice_p = chk_is_changeprice_p.Checked;
            _item.HasMaterials = chk_HasMaterials.Checked;
            _item.IsPackage = chk_IsPackage.Checked;
            _item.IsProduced = chk_IsProduced.Checked;
            _item.discount_formula = txtDiscountFormula.Text;
            _item.ExpiredAt = dtpExpired.Value.Date;
            
            _item.valuation_method = cmbValuation?.Text ?? "FIFO";

            // Update multi-price
            _item.Prices = ((BindingList<ItemPrice>)dgvMultiPrice.DataSource).ToList();

            // UnitVariants sudah ada di _item.UnitVariants (dari btnUnitVariant)
            _item.UnitVariants = unitVariantsFromForm;

            try
            {
                string msg;
                if (!editingItemId.HasValue)
                {
                    bool success = _productService.SaveProduct(_item, out msg);
                    if (success)
                    {
                        MessageBox.Show(msg, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    bool success = _productService.SaveProduct(_item, out msg);
                    if (success)
                    {
                        MessageBox.Show(msg, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStockOutput()
        {
            if (lblStockOut == null) return;
            int val = 0;
            int.TryParse(txtStock.Text, out val);
            string unitName = cmbUnit.SelectedIndex >= 0 ? cmbUnit.Text : "pcs";
            lblStockOut.Text = $"{val:N0} {unitName}";
            // nilai stok (HPP) dan nilai jual
            decimal bp = 0m, sp = 0m;
            decimal.TryParse(txtBuyPrice.Text, out bp);
            decimal.TryParse(txtSellPrice.Text, out sp);
            var hpp = bp * val;
            var jual = sp * val;
            if (lblStockValueHpp != null) lblStockValueHpp.Text = $"Nilai Stok (HPP): {hpp:N0}";
            if (lblStockValueSell != null) lblStockValueSell.Text = $"Nilai Jual: {jual:N0}";
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
                Width = 450,
                Height = 450,
                Text = "Edit Harga Bertingkat"
            };

            Label lblItemName = new Label() { Left = 20, Top = 15, Width = 400, Text = "Item: " + itemName, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            
            Label lblUnit = new Label() { Left = 20, Top = 60, Text = "Satuan", AutoSize = true };
            ComboBox cmbVariantUnit = new ComboBox() { Left = 150, Top = 58, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            
            // Populate cmbVariantUnit with base unit and variants
            List<dynamic> unitList = new List<dynamic>();

            // Tambahkan Base Unit (Satuan Dasar)
            if (cmbUnit.SelectedValue != null && int.TryParse(cmbUnit.SelectedValue.ToString(), out int baseUnitId))
            {
                unitList.Add(new { id = baseUnitId, display = cmbUnit.Text });
            }

            // Tambahkan Unit Variants
            foreach (var v in _item.UnitVariants)
            {
                if (v.UnitId > 0 && v.UnitId != (cmbUnit.SelectedValue != null ? Convert.ToInt32(cmbUnit.SelectedValue) : 0))
                {
                    unitList.Add(new { id = v.UnitId, display = v.UnitName });
                }
            }
            
            cmbVariantUnit.ValueMember = "id";
            cmbVariantUnit.DisplayMember = "display";
            cmbVariantUnit.DataSource = unitList;
            if (price.UnitId > 0) cmbVariantUnit.SelectedValue = price.UnitId;
            else if (cmbUnit.SelectedValue != null) cmbVariantUnit.SelectedValue = Convert.ToInt32(cmbUnit.SelectedValue);

            Label lblLevel = new Label() { Left = 20, Top = 100, Text = "Level Harga", AutoSize = true };
            ComboBox cmbLevel = new ComboBox() { Left = 150, Top = 98, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            var dtLevels = _productService.GetPriceLevels();
            
            // Konversi dari DataTable ke custom class/object list agar tidak muncul System.Data.DataRowView
            List<dynamic> levelList = new List<dynamic>();
            foreach (DataRow row in dtLevels.Rows)
            {
                levelList.Add(new { id = Convert.ToInt32(row["id"]), name = row["name"].ToString() });
            }
            
            cmbLevel.ValueMember = "id";
            cmbLevel.DisplayMember = "name";
            cmbLevel.DataSource = levelList;
            if (price.PriceLevelId > 0) cmbLevel.SelectedValue = price.PriceLevelId;

            Label lblMinQty = new Label() { Left = 20, Top = 140, Text = "Min Qty", AutoSize = true };
            TextBox txtMinQty = new TextBox() { Left = 150, Top = 138, Width = 100, Text = price.MinQty.ToString() };
            
            Label lblMaxQty = new Label() { Left = 260, Top = 140, Text = "Max Qty", AutoSize = true };
            TextBox txtMaxQty = new TextBox() { Left = 330, Top = 138, Width = 70, Text = price.MaxQty?.ToString() };

            Label lblPrice = new Label() { Left = 20, Top = 180, Text = "Harga Jual", AutoSize = true };
            TextBox txtPrice = new TextBox() { Left = 150, Top = 178, Width = 250, Text = price.Price.ToString() };

            Label lblFinalPrice = new Label() { Left = 20, Top = 230, Width = 350, Text = "Harga Akhir: -" };
            Label lblMargin = new Label() { Left = 20, Top = 260, Width = 350, Text = "Margin: -" };

            // Auto-update base buyPrice based on selected variant's conversion
            void UpdateVariantBuyPrice()
            {
                if (cmbVariantUnit.SelectedValue == null) return;
                int selectedUnitId = Convert.ToInt32(cmbVariantUnit.SelectedValue);
                int baseUnitId = cmbUnit.SelectedValue != null ? Convert.ToInt32(cmbUnit.SelectedValue) : 0;
                
                decimal variantBuyPrice = buyPrice; // default to base
                
                if (selectedUnitId != baseUnitId)
                {
                    var variant = _item.UnitVariants.FirstOrDefault(v => v.UnitId == selectedUnitId);
                    if (variant != null && variant.Conversion > 0)
                    {
                        variantBuyPrice = buyPrice * (decimal)variant.Conversion;
                    }
                }
                
                // Set the local variable that Recalculate uses
                price.buy_price_temp = variantBuyPrice;
                Recalculate();
            }

            cmbVariantUnit.SelectedIndexChanged += (s, e) => UpdateVariantBuyPrice();

            void Recalculate()
            {
                if (!decimal.TryParse(txtPrice.Text, out decimal sellPrice))
                {
                    lblFinalPrice.Text = "Harga Akhir: -";
                    lblMargin.Text = "Margin: -";
                    return;
                }

                decimal currentBuyPrice = price.buy_price_temp > 0 ? price.buy_price_temp : buyPrice;
                decimal margin = sellPrice - currentBuyPrice;
                decimal marginPercent = currentBuyPrice > 0 ? (margin / currentBuyPrice) * 100 : 0;

                lblFinalPrice.Text = $"Harga Akhir: {sellPrice:N0}";
                lblMargin.Text = $"Margin: {margin:N0} ({marginPercent:+0.##;-0.##}%)";
            }

            txtPrice.TextChanged += (s, e) => Recalculate();
            Recalculate();

            Button btnOK = new Button() { Left = 200, Top = 320, Width = 90, Height = 45, Text = "OK", DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Left = 310, Top = 320, Width = 90, Height = 45, Text = "Batal", DialogResult = DialogResult.Cancel };

            popup.Controls.AddRange(new Control[] { lblItemName, lblUnit, cmbVariantUnit, lblLevel, cmbLevel, lblMinQty, txtMinQty, lblMaxQty, txtMaxQty, lblPrice, txtPrice, lblFinalPrice, lblMargin, btnOK, btnCancel });
            popup.AcceptButton = btnOK;
            popup.CancelButton = btnCancel;

            if (popup.ShowDialog() == DialogResult.OK)
            {
                if (cmbVariantUnit.SelectedValue != null)
                {
                    price.UnitId = Convert.ToInt32(cmbVariantUnit.SelectedValue);
                    price.UnitName = cmbVariantUnit.Text;
                }
                if (cmbLevel.SelectedValue != null)
                {
                    price.PriceLevelId = Convert.ToInt32(cmbLevel.SelectedValue);
                    price.PriceLevelName = cmbLevel.Text;
                }
                if (int.TryParse(txtMinQty.Text, out int minQty)) price.MinQty = minQty;
                if (int.TryParse(txtMaxQty.Text, out int maxQty)) price.MaxQty = maxQty; else price.MaxQty = null;
                if (decimal.TryParse(txtPrice.Text, out decimal harga)) price.Price = harga;
                return true;
            }

            return false;
        }

        private void btnAddBrand_Click(object sender, EventArgs e)
        {
            using (var f = new BrandForm())
            {
                f.ShowDialog();
                LoadCombos();
            }
        }

        private void btnAddRack_Click(object sender, EventArgs e)
        {
            using (var f = new RackForm())
            {
                f.ShowDialog();
                LoadCombos();
            }
        }

        private void btnUnitVariant_Click(object sender, EventArgs e)
        {
            if (_item == null) _item = new Item();
            if (!decimal.TryParse(txtSellPrice.Text, out decimal sellPrice)) sellPrice = 0;
            
            // _item.UnitVariants = ... diisi saat save

            using (var variantForm = new UnitVariantForm(_item, cmbUnit.Text, sellPrice))
            {
                if (variantForm.ShowDialog() == DialogResult.OK)
                {
                    // Update the local list
                    unitVariantsFromForm = variantForm.UnitVariants;
                    _item.UnitVariants = variantForm.UnitVariants; // kalau mau langsung update item
                    //LoadUnitVariantsUI(); // Refresh the tab
                }
            }
        }

        private void ItemDetailForm_Load(object sender, EventArgs e)
        {
            LoadUnitVariantsUI();
            //LoadPriceLevelsUI();
        }

   

        private void btnRefreshPriceLevels_Click(object sender, EventArgs e)
        {
            //LoadPriceLevelsUI();
        }

        private void LoadUnitVariantsUI()
        {
            if (dgvVariants.Columns.Count == 0)
            {
                dgvVariants.AutoGenerateColumns = false;
                dgvVariants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UnitName", HeaderText = "Satuan", Width = 150 });
                dgvVariants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Conversion", HeaderText = "Konversi", Width = 100 });
                dgvVariants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SellPrice", HeaderText = "Harga Jual", Width = 200, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
                dgvVariants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Barcode", HeaderText = "Barcode", Width = 200 });
            }

            dgvVariants.DataSource = null; // Reset binding
            dgvVariants.DataSource = _item.UnitVariants;
        }
    }
}
