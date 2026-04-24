namespace POS_qu
{
    partial class ItemDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtBarcode = new TextBox();
            txtName = new TextBox();
            cmbUnit = new ComboBox();
            cmbCategory = new ComboBox();
            cmbSupplier = new ComboBox();
            lblBrand = new Label();
            cmbBrand = new ComboBox();
            btnAddBrand = new Button();
            lblRack = new Label();
            cmbRack = new ComboBox();
            btnAddRack = new Button();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblValuation = new Label();
            cmbValuation = new ComboBox();
            panel1 = new Panel();
            chk_IsProduced = new CheckBox();
            chk_IsPackage = new CheckBox();
            chk_HasMaterials = new CheckBox();
            chk_is_changeprice_p = new CheckBox();
            chk_RequireNotePayment = new CheckBox();
            chk_IsSellable = new CheckBox();
            chk_IsPurchasable = new CheckBox();
            chk_is_inventory_p = new CheckBox();
            label6 = new Label();
            tabHarga = new TabControl();
            tabPageUnitVariant = new TabPage();
            lblVariantTitle = new Label();
            btnUnitVariant = new Button();
            dgvVariants = new DataGridView();
            Harga = new TabPage();
            btnDeletePrice = new Button();
            btnEditPrice = new Button();
            btnAddPrice = new Button();
            dgvMultiPrice = new DataGridView();
            Bahan = new TabPage();
            lblAssemblyTitle = new Label();
            btnAddMaterial = new Button();
            btnRemoveMaterial = new Button();
            dgvMaterials = new DataGridView();
            colMaterialItem = new DataGridViewTextBoxColumn();
            colMaterialQty = new DataGridViewTextBoxColumn();
            colMaterialBaseUnit = new DataGridViewButtonColumn();
            colMaterialViewUnits = new DataGridViewButtonColumn();
            colMaterialUnit = new DataGridViewComboBoxColumn();
            colMaterialPrice = new DataGridViewTextBoxColumn();
            colMaterialSubtotal = new DataGridViewTextBoxColumn();
            lblTotalHppTitle = new Label();
            lblTotalHppValue = new Label();
            lblAssemblySellPrice = new Label();
            txtAssemblySellPrice = new TextBox();
            lblAssemblyMarginTitle = new Label();
            lblAssemblyMarginValue = new Label();
            pnlPricing = new Panel();
            lblStockOut = new Label();
            lblStockValueHpp = new Label();
            lblStockValueSell = new Label();
            panel2 = new Panel();
            outHargaAkhir = new Label();
            outMargin = new Label();
            outDiskon = new Label();
            label12 = new Label();
            Margin = new Label();
            Diskon = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            txtDiscountFormula = new TextBox();
            txtSellPrice = new TextBox();
            txtBuyPrice = new TextBox();
            txtStock = new TextBox();
            dtpExpired = new DateTimePicker();
            lblExpired = new Label();
            txtNote = new TextBox();
            label7 = new Label();
            btnCancel = new Button();
            btnSimpan = new Button();
            cmbSort = new ComboBox();
            label13 = new Label();
            panel1.SuspendLayout();
            tabHarga.SuspendLayout();
            tabPageUnitVariant.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVariants).BeginInit();
            Harga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMultiPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMaterials).BeginInit();
            pnlPricing.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 26);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(83, 28);
            label1.TabIndex = 0;
            label1.Text = "Barcode";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(38, 58);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(130, 28);
            label2.TabIndex = 0;
            label2.Text = "Nama Barang";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(38, 109);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(57, 28);
            label3.TabIndex = 0;
            label3.Text = "Units";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(38, 147);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(92, 28);
            label4.TabIndex = 0;
            label4.Text = "Category";
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(38, 186);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(85, 28);
            label5.TabIndex = 0;
            label5.Text = "Supplier";
            label5.TextAlign = ContentAlignment.TopCenter;
            // 
            // txtBarcode
            // 
            txtBarcode.Location = new Point(192, 26);
            txtBarcode.Margin = new Padding(2);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(327, 34);
            txtBarcode.TabIndex = 1;
            // 
            // txtName
            // 
            txtName.Location = new Point(192, 58);
            txtName.Margin = new Padding(2);
            txtName.Name = "txtName";
            txtName.Size = new Size(327, 34);
            txtName.TabIndex = 2;
            // 
            // cmbUnit
            // 
            cmbUnit.FormattingEnabled = true;
            cmbUnit.Location = new Point(192, 102);
            cmbUnit.Margin = new Padding(2);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(244, 36);
            cmbUnit.TabIndex = 3;
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(192, 141);
            cmbCategory.Margin = new Padding(2);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(244, 36);
            cmbCategory.TabIndex = 4;
            // 
            // cmbSupplier
            // 
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Location = new Point(192, 179);
            cmbSupplier.Margin = new Padding(2);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(327, 36);
            cmbSupplier.TabIndex = 5;
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Segoe UI", 10F);
            lblBrand.Location = new Point(38, 296);
            lblBrand.Margin = new Padding(2, 0, 2, 0);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(57, 28);
            lblBrand.TabIndex = 57;
            lblBrand.Text = "Merk";
            // 
            // cmbBrand
            // 
            cmbBrand.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBrand.Font = new Font("Segoe UI", 10F);
            cmbBrand.FormattingEnabled = true;
            cmbBrand.Location = new Point(192, 292);
            cmbBrand.Margin = new Padding(2);
            cmbBrand.Name = "cmbBrand";
            cmbBrand.Size = new Size(280, 36);
            cmbBrand.TabIndex = 58;
            // 
            // btnAddBrand
            // 
            btnAddBrand.FlatAppearance.BorderColor = Color.LightGray;
            btnAddBrand.FlatStyle = FlatStyle.Flat;
            btnAddBrand.Location = new Point(480, 292);
            btnAddBrand.Margin = new Padding(2);
            btnAddBrand.Name = "btnAddBrand";
            btnAddBrand.Size = new Size(39, 31);
            btnAddBrand.TabIndex = 59;
            btnAddBrand.Text = "+";
            btnAddBrand.UseVisualStyleBackColor = true;
            btnAddBrand.Click += btnAddBrand_Click;
            // 
            // lblRack
            // 
            lblRack.AutoSize = true;
            lblRack.Font = new Font("Segoe UI", 10F);
            lblRack.Location = new Point(38, 334);
            lblRack.Margin = new Padding(2, 0, 2, 0);
            lblRack.Name = "lblRack";
            lblRack.Size = new Size(44, 28);
            lblRack.TabIndex = 60;
            lblRack.Text = "Rak";
            // 
            // cmbRack
            // 
            cmbRack.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRack.Font = new Font("Segoe UI", 10F);
            cmbRack.FormattingEnabled = true;
            cmbRack.Location = new Point(192, 330);
            cmbRack.Margin = new Padding(2);
            cmbRack.Name = "cmbRack";
            cmbRack.Size = new Size(280, 36);
            cmbRack.TabIndex = 61;
            // 
            // btnAddRack
            // 
            btnAddRack.FlatAppearance.BorderColor = Color.LightGray;
            btnAddRack.FlatStyle = FlatStyle.Flat;
            btnAddRack.Location = new Point(480, 330);
            btnAddRack.Margin = new Padding(2);
            btnAddRack.Name = "btnAddRack";
            btnAddRack.Size = new Size(39, 31);
            btnAddRack.TabIndex = 62;
            btnAddRack.Text = "+";
            btnAddRack.UseVisualStyleBackColor = true;
            btnAddRack.Click += btnAddRack_Click;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Segoe UI", 10F);
            lblWarehouse.Location = new Point(38, 372);
            lblWarehouse.Margin = new Padding(2, 0, 2, 0);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(109, 28);
            lblWarehouse.TabIndex = 63;
            lblWarehouse.Text = "Ke Gudang";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new Font("Segoe UI", 10F);
            cmbWarehouse.FormattingEnabled = true;
            cmbWarehouse.Location = new Point(192, 368);
            cmbWarehouse.Margin = new Padding(2);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(327, 36);
            cmbWarehouse.TabIndex = 64;
            // 
            // lblValuation
            // 
            lblValuation.AutoSize = true;
            lblValuation.Font = new Font("Segoe UI", 10F);
            lblValuation.Location = new Point(38, 410);
            lblValuation.Margin = new Padding(2, 0, 2, 0);
            lblValuation.Name = "lblValuation";
            lblValuation.Size = new Size(152, 28);
            lblValuation.TabIndex = 65;
            lblValuation.Text = "Sistem Penilaian";
            // 
            // cmbValuation
            // 
            cmbValuation.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbValuation.Font = new Font("Segoe UI", 10F);
            cmbValuation.FormattingEnabled = true;
            cmbValuation.Location = new Point(192, 406);
            cmbValuation.Margin = new Padding(2);
            cmbValuation.Name = "cmbValuation";
            cmbValuation.Size = new Size(160, 36);
            cmbValuation.TabIndex = 66;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(chk_IsProduced);
            panel1.Controls.Add(chk_IsPackage);
            panel1.Controls.Add(chk_HasMaterials);
            panel1.Controls.Add(chk_is_changeprice_p);
            panel1.Controls.Add(chk_RequireNotePayment);
            panel1.Controls.Add(chk_IsSellable);
            panel1.Controls.Add(chk_IsPurchasable);
            panel1.Controls.Add(chk_is_inventory_p);
            panel1.Controls.Add(label6);
            panel1.Location = new Point(38, 450);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(525, 301);
            panel1.TabIndex = 7;
            // 
            // chk_IsProduced
            // 
            chk_IsProduced.AutoSize = true;
            chk_IsProduced.Location = new Point(26, 269);
            chk_IsProduced.Margin = new Padding(2);
            chk_IsProduced.Name = "chk_IsProduced";
            chk_IsProduced.Size = new Size(139, 32);
            chk_IsProduced.TabIndex = 14;
            chk_IsProduced.Text = "Rakitan (ASM)";
            chk_IsProduced.UseVisualStyleBackColor = true;
            // 
            // chk_IsPackage
            // 
            chk_IsPackage.AutoSize = true;
            chk_IsPackage.Location = new Point(26, 237);
            chk_IsPackage.Margin = new Padding(2);
            chk_IsPackage.Name = "chk_IsPackage";
            chk_IsPackage.Size = new Size(125, 32);
            chk_IsPackage.TabIndex = 13;
            chk_IsPackage.Text = "Box/Paket";
            chk_IsPackage.UseVisualStyleBackColor = true;
            // 
            // chk_HasMaterials
            // 
            chk_HasMaterials.AutoSize = true;
            chk_HasMaterials.Location = new Point(26, 205);
            chk_HasMaterials.Margin = new Padding(2);
            chk_HasMaterials.Name = "chk_HasMaterials";
            chk_HasMaterials.Size = new Size(171, 32);
            chk_HasMaterials.TabIndex = 12;
            chk_HasMaterials.Text = "Memiliki Bahan";
            chk_HasMaterials.UseVisualStyleBackColor = true;
            // 
            // chk_is_changeprice_p
            // 
            chk_is_changeprice_p.AutoSize = true;
            chk_is_changeprice_p.Location = new Point(26, 173);
            chk_is_changeprice_p.Margin = new Padding(2);
            chk_is_changeprice_p.Name = "chk_is_changeprice_p";
            chk_is_changeprice_p.Size = new Size(301, 32);
            chk_is_changeprice_p.TabIndex = 11;
            chk_is_changeprice_p.Text = "Bisa edit harga saat Pembelian";
            chk_is_changeprice_p.UseVisualStyleBackColor = true;
            // 
            // chk_RequireNotePayment
            // 
            chk_RequireNotePayment.AutoSize = true;
            chk_RequireNotePayment.Location = new Point(26, 141);
            chk_RequireNotePayment.Margin = new Padding(2);
            chk_RequireNotePayment.Name = "chk_RequireNotePayment";
            chk_RequireNotePayment.Size = new Size(243, 32);
            chk_RequireNotePayment.TabIndex = 10;
            chk_RequireNotePayment.Text = "Catatan Saat Pembelian";
            chk_RequireNotePayment.UseVisualStyleBackColor = true;
            // 
            // chk_IsSellable
            // 
            chk_IsSellable.AutoSize = true;
            chk_IsSellable.Location = new Point(26, 109);
            chk_IsSellable.Margin = new Padding(2);
            chk_IsSellable.Name = "chk_IsSellable";
            chk_IsSellable.Size = new Size(88, 32);
            chk_IsSellable.TabIndex = 9;
            chk_IsSellable.Text = "Dijual";
            chk_IsSellable.UseVisualStyleBackColor = true;
            // 
            // chk_IsPurchasable
            // 
            chk_IsPurchasable.AutoSize = true;
            chk_IsPurchasable.Location = new Point(26, 77);
            chk_IsPurchasable.Margin = new Padding(2);
            chk_IsPurchasable.Name = "chk_IsPurchasable";
            chk_IsPurchasable.Size = new Size(89, 32);
            chk_IsPurchasable.TabIndex = 8;
            chk_IsPurchasable.Text = "Dibeli";
            chk_IsPurchasable.UseVisualStyleBackColor = true;
            // 
            // chk_is_inventory_p
            // 
            chk_is_inventory_p.AutoSize = true;
            chk_is_inventory_p.Location = new Point(26, 45);
            chk_is_inventory_p.Margin = new Padding(2);
            chk_is_inventory_p.Name = "chk_is_inventory_p";
            chk_is_inventory_p.Size = new Size(151, 32);
            chk_is_inventory_p.TabIndex = 7;
            chk_is_inventory_p.Text = "Hitung Stock";
            chk_is_inventory_p.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 13);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(127, 28);
            label6.TabIndex = 0;
            label6.Text = "Item Settings";
            // 
            // tabHarga
            // 
            tabHarga.Controls.Add(tabPageUnitVariant);
            tabHarga.Controls.Add(Harga);
            tabHarga.Controls.Add(Bahan);
            tabHarga.ItemSize = new Size(160, 34);
            tabHarga.Location = new Point(567, 318);
            tabHarga.Margin = new Padding(2);
            tabHarga.Name = "tabHarga";
            tabHarga.SelectedIndex = 0;
            tabHarga.Size = new Size(1167, 429);
            tabHarga.SizeMode = TabSizeMode.Fixed;
            tabHarga.TabIndex = 15;
            // 
            // tabPageUnitVariant
            // 
            tabPageUnitVariant.Controls.Add(lblVariantTitle);
            tabPageUnitVariant.Controls.Add(btnUnitVariant);
            tabPageUnitVariant.Controls.Add(dgvVariants);
            tabPageUnitVariant.Location = new Point(4, 38);
            tabPageUnitVariant.Margin = new Padding(2);
            tabPageUnitVariant.Name = "tabPageUnitVariant";
            tabPageUnitVariant.Size = new Size(1159, 387);
            tabPageUnitVariant.TabIndex = 3;
            tabPageUnitVariant.Text = "Multi Variant";
            tabPageUnitVariant.UseVisualStyleBackColor = true;
            // 
            // lblVariantTitle
            // 
            lblVariantTitle.AutoSize = true;
            lblVariantTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblVariantTitle.Location = new Point(16, 16);
            lblVariantTitle.Margin = new Padding(2, 0, 2, 0);
            lblVariantTitle.Name = "lblVariantTitle";
            lblVariantTitle.Size = new Size(277, 32);
            lblVariantTitle.TabIndex = 1;
            lblVariantTitle.Text = "Daftar Satuan && Varian";
            // 
            // btnUnitVariant
            // 
            btnUnitVariant.BackColor = Color.DodgerBlue;
            btnUnitVariant.FlatAppearance.BorderSize = 0;
            btnUnitVariant.FlatStyle = FlatStyle.Flat;
            btnUnitVariant.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnUnitVariant.ForeColor = Color.White;
            btnUnitVariant.Location = new Point(16, 48);
            btnUnitVariant.Margin = new Padding(2);
            btnUnitVariant.Name = "btnUnitVariant";
            btnUnitVariant.Size = new Size(160, 32);
            btnUnitVariant.TabIndex = 0;
            btnUnitVariant.Text = "Kelola Varian Satuan";
            btnUnitVariant.UseVisualStyleBackColor = false;
            btnUnitVariant.Click += btnUnitVariant_Click;
            // 
            // dgvVariants
            // 
            dgvVariants.AllowUserToAddRows = false;
            dgvVariants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVariants.BackgroundColor = Color.White;
            dgvVariants.BorderStyle = BorderStyle.None;
            dgvVariants.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvVariants.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvVariants.ColumnHeadersHeight = 50;
            dgvVariants.EnableHeadersVisualStyles = false;
            dgvVariants.GridColor = Color.FromArgb(235, 235, 235);
            dgvVariants.Location = new Point(2, 113);
            dgvVariants.Margin = new Padding(2);
            dgvVariants.Name = "dgvVariants";
            dgvVariants.ReadOnly = true;
            dgvVariants.RowHeadersVisible = false;
            dgvVariants.RowHeadersWidth = 62;
            dgvVariants.RowTemplate.Height = 45;
            dgvVariants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVariants.Size = new Size(1042, 270);
            dgvVariants.TabIndex = 2;
            // 
            // Harga
            // 
            Harga.Controls.Add(btnDeletePrice);
            Harga.Controls.Add(btnEditPrice);
            Harga.Controls.Add(btnAddPrice);
            Harga.Controls.Add(dgvMultiPrice);
            Harga.Location = new Point(4, 38);
            Harga.Margin = new Padding(2);
            Harga.Name = "Harga";
            Harga.Padding = new Padding(2);
            Harga.Size = new Size(1159, 387);
            Harga.TabIndex = 0;
            Harga.Text = "Multi Harga";
            Harga.UseVisualStyleBackColor = true;
            // 
            // btnDeletePrice
            // 
            btnDeletePrice.BackColor = Color.FromArgb(220, 53, 69);
            btnDeletePrice.FlatAppearance.BorderSize = 0;
            btnDeletePrice.FlatStyle = FlatStyle.Flat;
            btnDeletePrice.ForeColor = Color.White;
            btnDeletePrice.Location = new Point(320, 16);
            btnDeletePrice.Margin = new Padding(2);
            btnDeletePrice.Name = "btnDeletePrice";
            btnDeletePrice.Size = new Size(140, 35);
            btnDeletePrice.TabIndex = 53;
            btnDeletePrice.Text = "Hapus Harga";
            btnDeletePrice.UseVisualStyleBackColor = true;
            btnDeletePrice.Click += btnDeletePrice_Click;
            // 
            // btnEditPrice
            // 
            btnEditPrice.BackColor = Color.FromArgb(255, 193, 7);
            btnEditPrice.FlatAppearance.BorderSize = 0;
            btnEditPrice.FlatStyle = FlatStyle.Flat;
            btnEditPrice.ForeColor = Color.Black;
            btnEditPrice.Location = new Point(168, 16);
            btnEditPrice.Margin = new Padding(2);
            btnEditPrice.Name = "btnEditPrice";
            btnEditPrice.Size = new Size(140, 35);
            btnEditPrice.TabIndex = 53;
            btnEditPrice.Text = "Edit Harga";
            btnEditPrice.UseVisualStyleBackColor = true;
            btnEditPrice.Click += btnEditPrice_Click;
            // 
            // btnAddPrice
            // 
            btnAddPrice.BackColor = Color.FromArgb(40, 167, 69);
            btnAddPrice.FlatAppearance.BorderSize = 0;
            btnAddPrice.FlatStyle = FlatStyle.Flat;
            btnAddPrice.ForeColor = Color.White;
            btnAddPrice.Location = new Point(16, 16);
            btnAddPrice.Margin = new Padding(2);
            btnAddPrice.Name = "btnAddPrice";
            btnAddPrice.Size = new Size(140, 35);
            btnAddPrice.TabIndex = 53;
            btnAddPrice.Text = "Tambah Harga";
            btnAddPrice.UseVisualStyleBackColor = true;
            btnAddPrice.Click += btnAddPrice_Click;
            // 
            // dgvMultiPrice
            // 
            dgvMultiPrice.BackgroundColor = Color.White;
            dgvMultiPrice.BorderStyle = BorderStyle.None;
            dgvMultiPrice.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMultiPrice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMultiPrice.Location = new Point(16, 64);
            dgvMultiPrice.Margin = new Padding(2);
            dgvMultiPrice.Name = "dgvMultiPrice";
            dgvMultiPrice.RowHeadersWidth = 62;
            dgvMultiPrice.Size = new Size(1028, 316);
            dgvMultiPrice.TabIndex = 52;
            // 
            // Bahan
            // 
            Bahan.Controls.Add(lblAssemblyMarginValue);
            Bahan.Controls.Add(lblAssemblyMarginTitle);
            Bahan.Controls.Add(txtAssemblySellPrice);
            Bahan.Controls.Add(lblAssemblySellPrice);
            Bahan.Controls.Add(lblTotalHppValue);
            Bahan.Controls.Add(lblTotalHppTitle);
            Bahan.Controls.Add(dgvMaterials);
            Bahan.Controls.Add(btnRemoveMaterial);
            Bahan.Controls.Add(btnAddMaterial);
            Bahan.Controls.Add(lblAssemblyTitle);
            Bahan.Location = new Point(4, 38);
            Bahan.Margin = new Padding(2);
            Bahan.Name = "Bahan";
            Bahan.Size = new Size(1159, 387);
            Bahan.TabIndex = 2;
            Bahan.Text = "Item Berbahan / Rakitan";
            Bahan.UseVisualStyleBackColor = true;
            // 
            // lblAssemblyTitle
            // 
            lblAssemblyTitle.AutoSize = true;
            lblAssemblyTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAssemblyTitle.Location = new Point(16, 16);
            lblAssemblyTitle.Margin = new Padding(2, 0, 2, 0);
            lblAssemblyTitle.Name = "lblAssemblyTitle";
            lblAssemblyTitle.Size = new Size(246, 28);
            lblAssemblyTitle.TabIndex = 0;
            lblAssemblyTitle.Text = "Komponen Item Rakitan";
            // 
            // btnAddMaterial
            // 
            btnAddMaterial.BackColor = Color.FromArgb(40, 167, 69);
            btnAddMaterial.FlatAppearance.BorderSize = 0;
            btnAddMaterial.FlatStyle = FlatStyle.Flat;
            btnAddMaterial.ForeColor = Color.White;
            btnAddMaterial.Location = new Point(16, 56);
            btnAddMaterial.Margin = new Padding(2);
            btnAddMaterial.Name = "btnAddMaterial";
            btnAddMaterial.Size = new Size(160, 35);
            btnAddMaterial.TabIndex = 1;
            btnAddMaterial.Text = "Tambah Item";
            btnAddMaterial.UseVisualStyleBackColor = false;
            btnAddMaterial.Click += btnAddMaterial_Click;
            // 
            // btnRemoveMaterial
            // 
            btnRemoveMaterial.BackColor = Color.FromArgb(220, 53, 69);
            btnRemoveMaterial.FlatAppearance.BorderSize = 0;
            btnRemoveMaterial.FlatStyle = FlatStyle.Flat;
            btnRemoveMaterial.ForeColor = Color.White;
            btnRemoveMaterial.Location = new Point(188, 56);
            btnRemoveMaterial.Margin = new Padding(2);
            btnRemoveMaterial.Name = "btnRemoveMaterial";
            btnRemoveMaterial.Size = new Size(160, 35);
            btnRemoveMaterial.TabIndex = 2;
            btnRemoveMaterial.Text = "Hapus Item";
            btnRemoveMaterial.UseVisualStyleBackColor = false;
            btnRemoveMaterial.Click += btnRemoveMaterial_Click;
            // 
            // dgvMaterials
            // 
            dgvMaterials.AllowUserToAddRows = false;
            dgvMaterials.BackgroundColor = Color.White;
            dgvMaterials.BorderStyle = BorderStyle.None;
            dgvMaterials.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMaterials.Columns.AddRange(new DataGridViewColumn[] { colMaterialItem, colMaterialQty, colMaterialBaseUnit, colMaterialViewUnits, colMaterialUnit, colMaterialPrice, colMaterialSubtotal });
            dgvMaterials.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMaterials.Location = new Point(16, 104);
            dgvMaterials.Margin = new Padding(2);
            dgvMaterials.Name = "dgvMaterials";
            dgvMaterials.RowHeadersVisible = false;
            dgvMaterials.RowHeadersWidth = 62;
            dgvMaterials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaterials.Size = new Size(1124, 190);
            dgvMaterials.TabIndex = 3;
            // 
            // colMaterialItem
            // 
            colMaterialItem.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colMaterialItem.DataPropertyName = "ComponentName";
            colMaterialItem.HeaderText = "Item";
            colMaterialItem.MinimumWidth = 8;
            colMaterialItem.Name = "colMaterialItem";
            colMaterialItem.ReadOnly = true;
            // 
            // colMaterialQty
            // 
            colMaterialQty.DataPropertyName = "Qty";
            colMaterialQty.HeaderText = "Qty";
            colMaterialQty.MinimumWidth = 8;
            colMaterialQty.Name = "colMaterialQty";
            colMaterialQty.Width = 90;
            // 
            // colMaterialBaseUnit
            // 
            colMaterialBaseUnit.HeaderText = "";
            colMaterialBaseUnit.MinimumWidth = 8;
            colMaterialBaseUnit.Name = "colMaterialBaseUnit";
            colMaterialBaseUnit.Text = "BASE";
            colMaterialBaseUnit.UseColumnTextForButtonValue = true;
            colMaterialBaseUnit.Width = 60;
            // 
            // colMaterialViewUnits
            // 
            colMaterialViewUnits.HeaderText = "";
            colMaterialViewUnits.MinimumWidth = 8;
            colMaterialViewUnits.Name = "colMaterialViewUnits";
            colMaterialViewUnits.Text = "ALL";
            colMaterialViewUnits.UseColumnTextForButtonValue = true;
            colMaterialViewUnits.Width = 50;
            // 
            // colMaterialUnit
            // 
            colMaterialUnit.DataPropertyName = "UnitId";
            colMaterialUnit.DisplayMember = "display";
            colMaterialUnit.HeaderText = "Satuan";
            colMaterialUnit.MinimumWidth = 8;
            colMaterialUnit.Name = "colMaterialUnit";
            colMaterialUnit.ValueMember = "id";
            colMaterialUnit.Width = 140;
            // 
            // colMaterialPrice
            // 
            colMaterialPrice.DataPropertyName = "UnitCost";
            colMaterialPrice.HeaderText = "Harga";
            colMaterialPrice.MinimumWidth = 8;
            colMaterialPrice.Name = "colMaterialPrice";
            colMaterialPrice.ReadOnly = true;
            colMaterialPrice.Width = 140;
            // 
            // colMaterialSubtotal
            // 
            colMaterialSubtotal.DataPropertyName = "Subtotal";
            colMaterialSubtotal.HeaderText = "Total";
            colMaterialSubtotal.MinimumWidth = 8;
            colMaterialSubtotal.Name = "colMaterialSubtotal";
            colMaterialSubtotal.ReadOnly = true;
            colMaterialSubtotal.Width = 160;
            // 
            // lblTotalHppTitle
            // 
            lblTotalHppTitle.AutoSize = true;
            lblTotalHppTitle.Location = new Point(16, 312);
            lblTotalHppTitle.Margin = new Padding(2, 0, 2, 0);
            lblTotalHppTitle.Name = "lblTotalHppTitle";
            lblTotalHppTitle.Size = new Size(111, 20);
            lblTotalHppTitle.TabIndex = 4;
            lblTotalHppTitle.Text = "Harga Pokok (HPP)";
            // 
            // lblTotalHppValue
            // 
            lblTotalHppValue.AutoSize = true;
            lblTotalHppValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalHppValue.Location = new Point(188, 308);
            lblTotalHppValue.Margin = new Padding(2, 0, 2, 0);
            lblTotalHppValue.Name = "lblTotalHppValue";
            lblTotalHppValue.Size = new Size(18, 23);
            lblTotalHppValue.TabIndex = 5;
            lblTotalHppValue.Text = "0";
            // 
            // lblAssemblySellPrice
            // 
            lblAssemblySellPrice.AutoSize = true;
            lblAssemblySellPrice.Location = new Point(16, 344);
            lblAssemblySellPrice.Margin = new Padding(2, 0, 2, 0);
            lblAssemblySellPrice.Name = "lblAssemblySellPrice";
            lblAssemblySellPrice.Size = new Size(79, 20);
            lblAssemblySellPrice.TabIndex = 6;
            lblAssemblySellPrice.Text = "Harga Jual";
            // 
            // txtAssemblySellPrice
            // 
            txtAssemblySellPrice.Location = new Point(188, 340);
            txtAssemblySellPrice.Margin = new Padding(2);
            txtAssemblySellPrice.Name = "txtAssemblySellPrice";
            txtAssemblySellPrice.Size = new Size(220, 27);
            txtAssemblySellPrice.TabIndex = 7;
            txtAssemblySellPrice.TextChanged += txtAssemblySellPrice_TextChanged;
            // 
            // lblAssemblyMarginTitle
            // 
            lblAssemblyMarginTitle.AutoSize = true;
            lblAssemblyMarginTitle.Location = new Point(430, 344);
            lblAssemblyMarginTitle.Margin = new Padding(2, 0, 2, 0);
            lblAssemblyMarginTitle.Name = "lblAssemblyMarginTitle";
            lblAssemblyMarginTitle.Size = new Size(56, 20);
            lblAssemblyMarginTitle.TabIndex = 8;
            lblAssemblyMarginTitle.Text = "Margin";
            // 
            // lblAssemblyMarginValue
            // 
            lblAssemblyMarginValue.AutoSize = true;
            lblAssemblyMarginValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAssemblyMarginValue.Location = new Point(510, 340);
            lblAssemblyMarginValue.Margin = new Padding(2, 0, 2, 0);
            lblAssemblyMarginValue.Name = "lblAssemblyMarginValue";
            lblAssemblyMarginValue.Size = new Size(18, 23);
            lblAssemblyMarginValue.TabIndex = 9;
            lblAssemblyMarginValue.Text = "0";
            // 
            // pnlPricing
            // 
            pnlPricing.BackColor = Color.White;
            pnlPricing.BorderStyle = BorderStyle.FixedSingle;
            pnlPricing.Controls.Add(lblStockOut);
            pnlPricing.Controls.Add(lblStockValueHpp);
            pnlPricing.Controls.Add(lblStockValueSell);
            pnlPricing.Controls.Add(panel2);
            pnlPricing.Controls.Add(label11);
            pnlPricing.Controls.Add(label10);
            pnlPricing.Controls.Add(label9);
            pnlPricing.Controls.Add(label8);
            pnlPricing.Controls.Add(txtDiscountFormula);
            pnlPricing.Controls.Add(txtSellPrice);
            pnlPricing.Controls.Add(txtBuyPrice);
            pnlPricing.Controls.Add(txtStock);
            pnlPricing.Location = new Point(614, 26);
            pnlPricing.Margin = new Padding(2);
            pnlPricing.Name = "pnlPricing";
            pnlPricing.Size = new Size(1120, 250);
            pnlPricing.TabIndex = 67;
            // 
            // lblStockOut
            // 
            lblStockOut.AutoSize = true;
            lblStockOut.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStockOut.ForeColor = Color.DodgerBlue;
            lblStockOut.Location = new Point(404, 20);
            lblStockOut.Margin = new Padding(2, 0, 2, 0);
            lblStockOut.Name = "lblStockOut";
            lblStockOut.Size = new Size(36, 28);
            lblStockOut.TabIndex = 56;
            lblStockOut.Text = "bb";
            // 
            // lblStockValueHpp
            // 
            lblStockValueHpp.AutoSize = true;
            lblStockValueHpp.Font = new Font("Segoe UI", 9F);
            lblStockValueHpp.ForeColor = Color.DimGray;
            lblStockValueHpp.Location = new Point(404, 56);
            lblStockValueHpp.Margin = new Padding(2, 0, 2, 0);
            lblStockValueHpp.Name = "lblStockValueHpp";
            lblStockValueHpp.Size = new Size(28, 25);
            lblStockValueHpp.TabIndex = 57;
            lblStockValueHpp.Text = "cc";
            // 
            // lblStockValueSell
            // 
            lblStockValueSell.AutoSize = true;
            lblStockValueSell.Font = new Font("Segoe UI", 9F);
            lblStockValueSell.ForeColor = Color.DimGray;
            lblStockValueSell.Location = new Point(404, 78);
            lblStockValueSell.Margin = new Padding(2, 0, 2, 0);
            lblStockValueSell.Name = "lblStockValueSell";
            lblStockValueSell.Size = new Size(28, 25);
            lblStockValueSell.TabIndex = 58;
            lblStockValueSell.Text = "cc";
            // 
            // panel2
            // 
            panel2.Controls.Add(outHargaAkhir);
            panel2.Controls.Add(outMargin);
            panel2.Controls.Add(outDiskon);
            panel2.Controls.Add(label12);
            panel2.Controls.Add(Margin);
            panel2.Controls.Add(Diskon);
            panel2.Location = new Point(753, 20);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(320, 141);
            panel2.TabIndex = 2;
            // 
            // outHargaAkhir
            // 
            outHargaAkhir.AutoSize = true;
            outHargaAkhir.Location = new Point(154, 64);
            outHargaAkhir.Margin = new Padding(2, 0, 2, 0);
            outHargaAkhir.Name = "outHargaAkhir";
            outHargaAkhir.Size = new Size(16, 28);
            outHargaAkhir.TabIndex = 4;
            outHargaAkhir.Text = ".";
            // 
            // outMargin
            // 
            outMargin.AutoSize = true;
            outMargin.Location = new Point(154, 38);
            outMargin.Margin = new Padding(2, 0, 2, 0);
            outMargin.Name = "outMargin";
            outMargin.Size = new Size(16, 28);
            outMargin.TabIndex = 4;
            outMargin.Text = ".";
            // 
            // outDiskon
            // 
            outDiskon.AutoSize = true;
            outDiskon.Location = new Point(154, 13);
            outDiskon.Margin = new Padding(2, 0, 2, 0);
            outDiskon.Name = "outDiskon";
            outDiskon.Size = new Size(16, 28);
            outDiskon.TabIndex = 4;
            outDiskon.Text = ".";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(19, 64);
            label12.Margin = new Padding(2, 0, 2, 0);
            label12.Name = "label12";
            label12.Size = new Size(116, 28);
            label12.TabIndex = 0;
            label12.Text = "Harga Akhir";
            // 
            // Margin
            // 
            Margin.AutoSize = true;
            Margin.Location = new Point(19, 38);
            Margin.Margin = new Padding(2, 0, 2, 0);
            Margin.Name = "Margin";
            Margin.Size = new Size(75, 28);
            Margin.TabIndex = 0;
            Margin.Text = "Margin";
            // 
            // Diskon
            // 
            Diskon.AutoSize = true;
            Diskon.Location = new Point(19, 13);
            Diskon.Margin = new Padding(2, 0, 2, 0);
            Diskon.Name = "Diskon";
            Diskon.Size = new Size(72, 28);
            Diskon.TabIndex = 0;
            Diskon.Text = "Diskon";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(20, 133);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(149, 28);
            label11.TabIndex = 1;
            label11.Text = "Formula Diskon";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(20, 95);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(103, 28);
            label10.TabIndex = 1;
            label10.Text = "Harga Jual";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(20, 57);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(101, 28);
            label9.TabIndex = 1;
            label9.Text = "Harga Beli";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(20, 19);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(58, 28);
            label8.TabIndex = 1;
            label8.Text = "stock";
            // 
            // txtDiscountFormula
            // 
            txtDiscountFormula.Location = new Point(170, 129);
            txtDiscountFormula.Margin = new Padding(2);
            txtDiscountFormula.Name = "txtDiscountFormula";
            txtDiscountFormula.Size = new Size(218, 34);
            txtDiscountFormula.TabIndex = 19;
            // 
            // txtSellPrice
            // 
            txtSellPrice.Location = new Point(170, 91);
            txtSellPrice.Margin = new Padding(2);
            txtSellPrice.Name = "txtSellPrice";
            txtSellPrice.Size = new Size(218, 34);
            txtSellPrice.TabIndex = 18;
            // 
            // txtBuyPrice
            // 
            txtBuyPrice.Location = new Point(170, 53);
            txtBuyPrice.Margin = new Padding(2);
            txtBuyPrice.Name = "txtBuyPrice";
            txtBuyPrice.Size = new Size(218, 34);
            txtBuyPrice.TabIndex = 17;
            // 
            // txtStock
            // 
            txtStock.Location = new Point(170, 15);
            txtStock.Margin = new Padding(2);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(218, 34);
            txtStock.TabIndex = 16;
            // 
            // dtpExpired
            // 
            dtpExpired.Checked = false;
            dtpExpired.Font = new Font("Segoe UI", 10F);
            dtpExpired.Format = DateTimePickerFormat.Short;
            dtpExpired.Location = new Point(192, 258);
            dtpExpired.Margin = new Padding(2);
            dtpExpired.Name = "dtpExpired";
            dtpExpired.ShowCheckBox = true;
            dtpExpired.Size = new Size(113, 34);
            dtpExpired.TabIndex = 54;
            // 
            // lblExpired
            // 
            lblExpired.AutoSize = true;
            lblExpired.Font = new Font("Segoe UI", 10F);
            lblExpired.Location = new Point(38, 258);
            lblExpired.Margin = new Padding(2, 0, 2, 0);
            lblExpired.Name = "lblExpired";
            lblExpired.Size = new Size(81, 28);
            lblExpired.TabIndex = 55;
            lblExpired.Text = "Expired:";
            // 
            // txtNote
            // 
            txtNote.Location = new Point(192, 224);
            txtNote.Margin = new Padding(2);
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(327, 34);
            txtNote.TabIndex = 6;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(38, 224);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(112, 28);
            label7.TabIndex = 0;
            label7.Text = "Keterangan";
            label7.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(1535, 795);
            btnCancel.Margin = new Padding(2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 45);
            btnCancel.TabIndex = 51;
            btnCancel.Text = "Batal";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSimpan
            // 
            btnSimpan.BackColor = Color.FromArgb(0, 122, 255);
            btnSimpan.FlatAppearance.BorderSize = 0;
            btnSimpan.FlatStyle = FlatStyle.Flat;
            btnSimpan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSimpan.ForeColor = Color.White;
            btnSimpan.Location = new Point(1644, 795);
            btnSimpan.Margin = new Padding(2);
            btnSimpan.Name = "btnSimpan";
            btnSimpan.Size = new Size(90, 45);
            btnSimpan.TabIndex = 50;
            btnSimpan.Text = "Simpan";
            btnSimpan.UseVisualStyleBackColor = true;
            btnSimpan.Click += btnSimpan_Click;
            // 
            // cmbSort
            // 
            cmbSort.FormattingEnabled = true;
            cmbSort.Location = new Point(495, 101);
            cmbSort.Name = "cmbSort";
            cmbSort.Size = new Size(114, 36);
            cmbSort.TabIndex = 56;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(438, 105);
            label13.Margin = new Padding(2, 0, 2, 0);
            label13.Name = "label13";
            label13.Size = new Size(72, 28);
            label13.TabIndex = 0;
            label13.Text = "Urutan";
            // 
            // ItemDetailForm
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 249);
            ClientSize = new Size(1791, 873);
            Controls.Add(btnSimpan);
            Controls.Add(btnCancel);
            Controls.Add(pnlPricing);
            Controls.Add(cmbSort);
            Controls.Add(lblExpired);
            Controls.Add(dtpExpired);
            Controls.Add(tabHarga);
            Controls.Add(panel1);
            Controls.Add(cmbValuation);
            Controls.Add(lblValuation);
            Controls.Add(cmbWarehouse);
            Controls.Add(lblWarehouse);
            Controls.Add(btnAddRack);
            Controls.Add(cmbRack);
            Controls.Add(lblRack);
            Controls.Add(btnAddBrand);
            Controls.Add(cmbBrand);
            Controls.Add(lblBrand);
            Controls.Add(cmbSupplier);
            Controls.Add(cmbCategory);
            Controls.Add(cmbUnit);
            Controls.Add(txtNote);
            Controls.Add(txtName);
            Controls.Add(txtBarcode);
            Controls.Add(label7);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label13);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ItemDetailForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Detail Item";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabHarga.ResumeLayout(false);
            tabPageUnitVariant.ResumeLayout(false);
            tabPageUnitVariant.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVariants).EndInit();
            Harga.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMultiPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMaterials).EndInit();
            pnlPricing.ResumeLayout(false);
            pnlPricing.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtBarcode;
        private TextBox txtName;
        private ComboBox cmbUnit;
        private ComboBox cmbCategory;
        private ComboBox cmbSupplier;
        private Label lblBrand;
        private ComboBox cmbBrand;
        private Button btnAddBrand;
        private Label lblRack;
        private ComboBox cmbRack;
        private Button btnAddRack;
        private Label lblWarehouse;
        private ComboBox cmbWarehouse;
        private Label lblValuation;
        private ComboBox cmbValuation;
        private Panel panel1;
        private CheckBox chk_is_changeprice_p;
        private CheckBox chk_RequireNotePayment;
        private CheckBox chk_IsSellable;
        private CheckBox chk_IsPurchasable;
        private CheckBox chk_is_inventory_p;
        private Label label6;
        private CheckBox chk_IsProduced;
        private CheckBox chk_IsPackage;
        private CheckBox chk_HasMaterials;
        private TabControl tabHarga;
        private TabPage Harga;
        private TextBox txtNote;
        private Label label7;
        private Label label8;
        private TextBox txtStock;
        private Label label9;
        private TextBox txtSellPrice;
        private TextBox txtBuyPrice;
        private Label label10;
        private TabPage Bahan;
        private Panel pnlPricing;
        private Panel panel2;
        private Label label12;
        private Label Margin;
        private Label Diskon;
        private Label label11;
        private TextBox txtDiscountFormula;
        private Label outMargin;
        private Label outDiskon;
        private Label outHargaAkhir;
        private Button btnCancel;
        private Button btnSimpan;
        private DataGridView dgvMultiPrice;
        private Button btnEditPrice;
        private Button btnAddPrice;
        private Button btnDeletePrice;
        private TabPage tabPageUnitVariant;
        private Button btnUnitVariant;
        private DateTimePicker dtpExpired;
        private Label lblExpired;
        private Label lblStockOut;
        private Label lblStockValueHpp;
        private Label lblStockValueSell;
        private Label lblVariantTitle;
        private DataGridView dgvVariants;
        private ComboBox cmbSort;
        private Label label13;
        private Label lblAssemblyTitle;
        private Button btnAddMaterial;
        private Button btnRemoveMaterial;
        private DataGridView dgvMaterials;
        private DataGridViewTextBoxColumn colMaterialItem;
        private DataGridViewTextBoxColumn colMaterialQty;
        private DataGridViewButtonColumn colMaterialBaseUnit;
        private DataGridViewButtonColumn colMaterialViewUnits;
        private DataGridViewComboBoxColumn colMaterialUnit;
        private DataGridViewTextBoxColumn colMaterialPrice;
        private DataGridViewTextBoxColumn colMaterialSubtotal;
        private Label lblTotalHppTitle;
        private Label lblTotalHppValue;
        private Label lblAssemblySellPrice;
        private TextBox txtAssemblySellPrice;
        private Label lblAssemblyMarginTitle;
        private Label lblAssemblyMarginValue;
    }
}
