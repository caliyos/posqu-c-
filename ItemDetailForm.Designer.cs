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
            lblProductType = new Label();
            cmbProductType = new ComboBox();
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
            lblAssemblyMarginValue = new Label();
            lblAssemblyMarginTitle = new Label();
            txtAssemblySellPrice = new TextBox();
            lblAssemblySellPrice = new Label();
            lblTotalHppValue = new Label();
            lblTotalHppTitle = new Label();
            dgvMaterials = new DataGridView();
            colMaterialItem = new DataGridViewTextBoxColumn();
            colMaterialQty = new DataGridViewTextBoxColumn();
            colMaterialBaseUnit = new DataGridViewButtonColumn();
            colMaterialViewUnits = new DataGridViewButtonColumn();
            colMaterialUnit = new DataGridViewComboBoxColumn();
            colMaterialPrice = new DataGridViewTextBoxColumn();
            colMaterialSubtotal = new DataGridViewTextBoxColumn();
            btnRemoveMaterial = new Button();
            btnAddMaterial = new Button();
            lblAssemblyTitle = new Label();
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
            label14 = new Label();
            label8 = new Label();
            txtDiscountFormula = new TextBox();
            txtSellPrice = new TextBox();
            txtBuyPrice = new TextBox();
            txtMinQty = new TextBox();
            txtStock = new TextBox();
            dtpExpired = new DateTimePicker();
            lblExpired = new Label();
            txtNote = new TextBox();
            label7 = new Label();
            btnCancel = new Button();
            btnSimpan = new Button();
            cmbSort = new ComboBox();
            label13 = new Label();
            pnlRoot = new Panel();
            pnlBottomBar = new Panel();
            flpBottomActions = new FlowLayoutPanel();
            splitMain = new SplitContainer();
            pnlLeft = new Panel();
            grpInventory = new GroupBox();
            tlpInventory = new TableLayoutPanel();
            grpInfo = new GroupBox();
            tlpInfo = new TableLayoutPanel();
            pnlBrandRow = new Panel();
            pnlRackRow = new Panel();
            tlpRight = new TableLayoutPanel();
            panel1.SuspendLayout();
            tabHarga.SuspendLayout();
            tabPageUnitVariant.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVariants).BeginInit();
            Harga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMultiPrice).BeginInit();
            Bahan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMaterials).BeginInit();
            pnlPricing.SuspendLayout();
            panel2.SuspendLayout();
            pnlRoot.SuspendLayout();
            pnlBottomBar.SuspendLayout();
            flpBottomActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            pnlLeft.SuspendLayout();
            grpInventory.SuspendLayout();
            tlpInventory.SuspendLayout();
            grpInfo.SuspendLayout();
            tlpInfo.SuspendLayout();
            pnlBrandRow.SuspendLayout();
            pnlRackRow.SuspendLayout();
            tlpRight.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(2, 6);
            label1.Margin = new Padding(2, 6, 2, 6);
            label1.Name = "label1";
            label1.Size = new Size(165, 25);
            label1.TabIndex = 0;
            label1.Text = "Barcode";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(2, 43);
            label2.Margin = new Padding(2, 6, 2, 6);
            label2.Name = "label2";
            label2.Size = new Size(165, 25);
            label2.TabIndex = 0;
            label2.Text = "Nama Barang";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(2, 117);
            label3.Margin = new Padding(2, 6, 2, 6);
            label3.Name = "label3";
            label3.Size = new Size(165, 25);
            label3.TabIndex = 0;
            label3.Text = "Units";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(2, 193);
            label4.Margin = new Padding(2, 6, 2, 6);
            label4.Name = "label4";
            label4.Size = new Size(165, 25);
            label4.TabIndex = 0;
            label4.Text = "Category";
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(2, 230);
            label5.Margin = new Padding(2, 6, 2, 6);
            label5.Name = "label5";
            label5.Size = new Size(165, 25);
            label5.TabIndex = 0;
            label5.Text = "Supplier";
            label5.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblProductType
            // 
            lblProductType.AutoSize = true;
            lblProductType.Dock = DockStyle.Fill;
            lblProductType.Location = new Point(2, 80);
            lblProductType.Margin = new Padding(2, 6, 2, 6);
            lblProductType.Name = "lblProductType";
            lblProductType.Size = new Size(165, 25);
            lblProductType.TabIndex = 0;
            lblProductType.Text = "Tipe Produk";
            // 
            // cmbProductType
            // 
            cmbProductType.Dock = DockStyle.Fill;
            cmbProductType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductType.FormattingEnabled = true;
            cmbProductType.Items.AddRange(new object[] { "Stockable (Inventory Item)", "Consumable (Non-Stock Item)", "Service", "Manufactured (BOM/Assembly)" });
            cmbProductType.Location = new Point(171, 76);
            cmbProductType.Margin = new Padding(2, 2, 2, 2);
            cmbProductType.Name = "cmbProductType";
            cmbProductType.Size = new Size(323, 33);
            cmbProductType.TabIndex = 3;
            // 
            // txtBarcode
            // 
            txtBarcode.Dock = DockStyle.Fill;
            txtBarcode.Location = new Point(171, 2);
            txtBarcode.Margin = new Padding(2, 2, 2, 2);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(323, 33);
            txtBarcode.TabIndex = 1;
            // 
            // txtName
            // 
            txtName.Dock = DockStyle.Fill;
            txtName.Location = new Point(171, 39);
            txtName.Margin = new Padding(2, 2, 2, 2);
            txtName.Name = "txtName";
            txtName.Size = new Size(323, 33);
            txtName.TabIndex = 2;
            // 
            // cmbUnit
            // 
            cmbUnit.Dock = DockStyle.Fill;
            cmbUnit.FormattingEnabled = true;
            cmbUnit.Location = new Point(171, 113);
            cmbUnit.Margin = new Padding(2, 2, 2, 2);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(323, 33);
            cmbUnit.TabIndex = 5;
            // 
            // cmbCategory
            // 
            cmbCategory.Dock = DockStyle.Fill;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(171, 189);
            cmbCategory.Margin = new Padding(2, 2, 2, 2);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(323, 33);
            cmbCategory.TabIndex = 7;
            // 
            // cmbSupplier
            // 
            cmbSupplier.Dock = DockStyle.Fill;
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Location = new Point(171, 226);
            cmbSupplier.Margin = new Padding(2, 2, 2, 2);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(323, 33);
            cmbSupplier.TabIndex = 8;
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Dock = DockStyle.Fill;
            lblBrand.Font = new Font("Segoe UI", 10F);
            lblBrand.Location = new Point(2, 267);
            lblBrand.Margin = new Padding(2, 6, 2, 6);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(165, 44);
            lblBrand.TabIndex = 57;
            lblBrand.Text = "Merk";
            // 
            // cmbBrand
            // 
            cmbBrand.Dock = DockStyle.Fill;
            cmbBrand.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBrand.Font = new Font("Segoe UI", 10F);
            cmbBrand.FormattingEnabled = true;
            cmbBrand.Location = new Point(0, 0);
            cmbBrand.Margin = new Padding(2, 2, 2, 2);
            cmbBrand.Name = "cmbBrand";
            cmbBrand.Size = new Size(185, 25);
            cmbBrand.TabIndex = 58;
            // 
            // btnAddBrand
            // 
            btnAddBrand.Dock = DockStyle.Right;
            btnAddBrand.FlatAppearance.BorderColor = Color.LightGray;
            btnAddBrand.FlatStyle = FlatStyle.Flat;
            btnAddBrand.Location = new Point(185, 0);
            btnAddBrand.Margin = new Padding(2, 2, 2, 2);
            btnAddBrand.Name = "btnAddBrand";
            btnAddBrand.Size = new Size(44, 52);
            btnAddBrand.TabIndex = 59;
            btnAddBrand.Text = "+";
            btnAddBrand.UseVisualStyleBackColor = true;
            btnAddBrand.Click += btnAddBrand_Click;
            // 
            // lblRack
            // 
            lblRack.AutoSize = true;
            lblRack.Dock = DockStyle.Fill;
            lblRack.Font = new Font("Segoe UI", 10F);
            lblRack.Location = new Point(2, 323);
            lblRack.Margin = new Padding(2, 6, 2, 6);
            lblRack.Name = "lblRack";
            lblRack.Size = new Size(165, 46);
            lblRack.TabIndex = 60;
            lblRack.Text = "Rak";
            // 
            // cmbRack
            // 
            cmbRack.Dock = DockStyle.Fill;
            cmbRack.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRack.Font = new Font("Segoe UI", 10F);
            cmbRack.FormattingEnabled = true;
            cmbRack.Location = new Point(0, 0);
            cmbRack.Margin = new Padding(2, 2, 2, 2);
            cmbRack.Name = "cmbRack";
            cmbRack.Size = new Size(185, 25);
            cmbRack.TabIndex = 61;
            // 
            // btnAddRack
            // 
            btnAddRack.Dock = DockStyle.Right;
            btnAddRack.FlatAppearance.BorderColor = Color.LightGray;
            btnAddRack.FlatStyle = FlatStyle.Flat;
            btnAddRack.Location = new Point(185, 0);
            btnAddRack.Margin = new Padding(2, 2, 2, 2);
            btnAddRack.Name = "btnAddRack";
            btnAddRack.Size = new Size(44, 54);
            btnAddRack.TabIndex = 62;
            btnAddRack.Text = "+";
            btnAddRack.UseVisualStyleBackColor = true;
            btnAddRack.Click += btnAddRack_Click;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Dock = DockStyle.Fill;
            lblWarehouse.Font = new Font("Segoe UI", 10F);
            lblWarehouse.Location = new Point(2, 6);
            lblWarehouse.Margin = new Padding(2, 6, 2, 6);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(165, 19);
            lblWarehouse.TabIndex = 63;
            lblWarehouse.Text = "Ke Gudang";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.Dock = DockStyle.Fill;
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new Font("Segoe UI", 10F);
            cmbWarehouse.FormattingEnabled = true;
            cmbWarehouse.Location = new Point(171, 2);
            cmbWarehouse.Margin = new Padding(2, 2, 2, 2);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(323, 25);
            cmbWarehouse.TabIndex = 64;
            // 
            // lblValuation
            // 
            lblValuation.AutoSize = true;
            lblValuation.Dock = DockStyle.Fill;
            lblValuation.Font = new Font("Segoe UI", 10F);
            lblValuation.Location = new Point(2, 37);
            lblValuation.Margin = new Padding(2, 6, 2, 6);
            lblValuation.Name = "lblValuation";
            lblValuation.Size = new Size(165, 19);
            lblValuation.TabIndex = 65;
            lblValuation.Text = "Sistem Penilaian";
            // 
            // cmbValuation
            // 
            cmbValuation.Dock = DockStyle.Fill;
            cmbValuation.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbValuation.Font = new Font("Segoe UI", 10F);
            cmbValuation.FormattingEnabled = true;
            cmbValuation.Location = new Point(171, 33);
            cmbValuation.Margin = new Padding(2, 2, 2, 2);
            cmbValuation.Name = "cmbValuation";
            cmbValuation.Size = new Size(323, 25);
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
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 645);
            panel1.Margin = new Padding(0, 10, 0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(518, 396);
            panel1.TabIndex = 7;
            // 
            // chk_IsProduced
            // 
            chk_IsProduced.AutoSize = true;
            chk_IsProduced.Location = new Point(27, 279);
            chk_IsProduced.Margin = new Padding(2, 2, 2, 2);
            chk_IsProduced.Name = "chk_IsProduced";
            chk_IsProduced.Size = new Size(149, 29);
            chk_IsProduced.TabIndex = 14;
            chk_IsProduced.Text = "Rakitan (ASM)";
            chk_IsProduced.UseVisualStyleBackColor = true;
            // 
            // chk_IsPackage
            // 
            chk_IsPackage.AutoSize = true;
            chk_IsPackage.Location = new Point(27, 246);
            chk_IsPackage.Margin = new Padding(2, 2, 2, 2);
            chk_IsPackage.Name = "chk_IsPackage";
            chk_IsPackage.Size = new Size(114, 29);
            chk_IsPackage.TabIndex = 13;
            chk_IsPackage.Text = "Box/Paket";
            chk_IsPackage.UseVisualStyleBackColor = true;
            // 
            // chk_HasMaterials
            // 
            chk_HasMaterials.AutoSize = true;
            chk_HasMaterials.Location = new Point(27, 213);
            chk_HasMaterials.Margin = new Padding(2, 2, 2, 2);
            chk_HasMaterials.Name = "chk_HasMaterials";
            chk_HasMaterials.Size = new Size(161, 29);
            chk_HasMaterials.TabIndex = 12;
            chk_HasMaterials.Text = "Memiliki Bahan";
            chk_HasMaterials.UseVisualStyleBackColor = true;
            // 
            // chk_is_changeprice_p
            // 
            chk_is_changeprice_p.AutoSize = true;
            chk_is_changeprice_p.Location = new Point(27, 180);
            chk_is_changeprice_p.Margin = new Padding(2, 2, 2, 2);
            chk_is_changeprice_p.Name = "chk_is_changeprice_p";
            chk_is_changeprice_p.Size = new Size(288, 29);
            chk_is_changeprice_p.TabIndex = 11;
            chk_is_changeprice_p.Text = "Bisa edit harga saat Pembelian";
            chk_is_changeprice_p.UseVisualStyleBackColor = true;
            // 
            // chk_RequireNotePayment
            // 
            chk_RequireNotePayment.AutoSize = true;
            chk_RequireNotePayment.Location = new Point(27, 146);
            chk_RequireNotePayment.Margin = new Padding(2, 2, 2, 2);
            chk_RequireNotePayment.Name = "chk_RequireNotePayment";
            chk_RequireNotePayment.Size = new Size(230, 29);
            chk_RequireNotePayment.TabIndex = 10;
            chk_RequireNotePayment.Text = "Catatan Saat Pembelian";
            chk_RequireNotePayment.UseVisualStyleBackColor = true;
            // 
            // chk_IsSellable
            // 
            chk_IsSellable.AutoSize = true;
            chk_IsSellable.Location = new Point(27, 113);
            chk_IsSellable.Margin = new Padding(2, 2, 2, 2);
            chk_IsSellable.Name = "chk_IsSellable";
            chk_IsSellable.Size = new Size(80, 29);
            chk_IsSellable.TabIndex = 9;
            chk_IsSellable.Text = "Dijual";
            chk_IsSellable.UseVisualStyleBackColor = true;
            // 
            // chk_IsPurchasable
            // 
            chk_IsPurchasable.AutoSize = true;
            chk_IsPurchasable.Location = new Point(27, 80);
            chk_IsPurchasable.Margin = new Padding(2, 2, 2, 2);
            chk_IsPurchasable.Name = "chk_IsPurchasable";
            chk_IsPurchasable.Size = new Size(80, 29);
            chk_IsPurchasable.TabIndex = 8;
            chk_IsPurchasable.Text = "Dibeli";
            chk_IsPurchasable.UseVisualStyleBackColor = true;
            // 
            // chk_is_inventory_p
            // 
            chk_is_inventory_p.AutoSize = true;
            chk_is_inventory_p.Location = new Point(27, 47);
            chk_is_inventory_p.Margin = new Padding(2, 2, 2, 2);
            chk_is_inventory_p.Name = "chk_is_inventory_p";
            chk_is_inventory_p.Size = new Size(137, 29);
            chk_is_inventory_p.TabIndex = 7;
            chk_is_inventory_p.Text = "Hitung Stock";
            chk_is_inventory_p.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(7, 13);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(121, 25);
            label6.TabIndex = 0;
            label6.Text = "Item Settings";
            // 
            // tabHarga
            // 
            tabHarga.Controls.Add(tabPageUnitVariant);
            tabHarga.Controls.Add(Harga);
            tabHarga.Controls.Add(Bahan);
            tabHarga.Dock = DockStyle.Fill;
            tabHarga.ItemSize = new Size(160, 34);
            tabHarga.Location = new Point(0, 281);
            tabHarga.Margin = new Padding(0);
            tabHarga.Name = "tabHarga";
            tabHarga.SelectedIndex = 0;
            tabHarga.Size = new Size(989, 570);
            tabHarga.SizeMode = TabSizeMode.Fixed;
            tabHarga.TabIndex = 15;
            // 
            // tabPageUnitVariant
            // 
            tabPageUnitVariant.Controls.Add(lblVariantTitle);
            tabPageUnitVariant.Controls.Add(btnUnitVariant);
            tabPageUnitVariant.Controls.Add(dgvVariants);
            tabPageUnitVariant.Location = new Point(4, 38);
            tabPageUnitVariant.Margin = new Padding(2, 2, 2, 2);
            tabPageUnitVariant.Name = "tabPageUnitVariant";
            tabPageUnitVariant.Size = new Size(981, 528);
            tabPageUnitVariant.TabIndex = 3;
            tabPageUnitVariant.Text = "Multi Variant";
            tabPageUnitVariant.UseVisualStyleBackColor = true;
            // 
            // lblVariantTitle
            // 
            lblVariantTitle.AutoSize = true;
            lblVariantTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblVariantTitle.Location = new Point(17, 17);
            lblVariantTitle.Margin = new Padding(2, 0, 2, 0);
            lblVariantTitle.Name = "lblVariantTitle";
            lblVariantTitle.Size = new Size(186, 21);
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
            btnUnitVariant.Location = new Point(17, 50);
            btnUnitVariant.Margin = new Padding(2, 2, 2, 2);
            btnUnitVariant.Name = "btnUnitVariant";
            btnUnitVariant.Size = new Size(169, 34);
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
            dgvVariants.Location = new Point(2, 118);
            dgvVariants.Margin = new Padding(2, 2, 2, 2);
            dgvVariants.Name = "dgvVariants";
            dgvVariants.ReadOnly = true;
            dgvVariants.RowHeadersVisible = false;
            dgvVariants.RowHeadersWidth = 62;
            dgvVariants.RowTemplate.Height = 45;
            dgvVariants.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVariants.Size = new Size(1096, 281);
            dgvVariants.TabIndex = 2;
            // 
            // Harga
            // 
            Harga.Controls.Add(btnDeletePrice);
            Harga.Controls.Add(btnEditPrice);
            Harga.Controls.Add(btnAddPrice);
            Harga.Controls.Add(dgvMultiPrice);
            Harga.Location = new Point(4, 38);
            Harga.Margin = new Padding(2, 2, 2, 2);
            Harga.Name = "Harga";
            Harga.Padding = new Padding(2, 2, 2, 2);
            Harga.Size = new Size(800, 392);
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
            btnDeletePrice.Location = new Point(336, 17);
            btnDeletePrice.Margin = new Padding(2, 2, 2, 2);
            btnDeletePrice.Name = "btnDeletePrice";
            btnDeletePrice.Size = new Size(147, 36);
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
            btnEditPrice.Location = new Point(177, 17);
            btnEditPrice.Margin = new Padding(2, 2, 2, 2);
            btnEditPrice.Name = "btnEditPrice";
            btnEditPrice.Size = new Size(147, 36);
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
            btnAddPrice.Location = new Point(17, 17);
            btnAddPrice.Margin = new Padding(2, 2, 2, 2);
            btnAddPrice.Name = "btnAddPrice";
            btnAddPrice.Size = new Size(147, 36);
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
            dgvMultiPrice.Location = new Point(17, 66);
            dgvMultiPrice.Margin = new Padding(2, 2, 2, 2);
            dgvMultiPrice.Name = "dgvMultiPrice";
            dgvMultiPrice.RowHeadersWidth = 62;
            dgvMultiPrice.Size = new Size(945, 328);
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
            Bahan.Margin = new Padding(2, 2, 2, 2);
            Bahan.Name = "Bahan";
            Bahan.Size = new Size(800, 392);
            Bahan.TabIndex = 2;
            Bahan.Text = "Item Berbahan / Rakitan";
            Bahan.UseVisualStyleBackColor = true;
            // 
            // lblAssemblyMarginValue
            // 
            lblAssemblyMarginValue.AutoSize = true;
            lblAssemblyMarginValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAssemblyMarginValue.Location = new Point(537, 353);
            lblAssemblyMarginValue.Margin = new Padding(2, 0, 2, 0);
            lblAssemblyMarginValue.Name = "lblAssemblyMarginValue";
            lblAssemblyMarginValue.Size = new Size(17, 19);
            lblAssemblyMarginValue.TabIndex = 9;
            lblAssemblyMarginValue.Text = "0";
            // 
            // lblAssemblyMarginTitle
            // 
            lblAssemblyMarginTitle.AutoSize = true;
            lblAssemblyMarginTitle.Location = new Point(452, 357);
            lblAssemblyMarginTitle.Margin = new Padding(2, 0, 2, 0);
            lblAssemblyMarginTitle.Name = "lblAssemblyMarginTitle";
            lblAssemblyMarginTitle.Size = new Size(73, 25);
            lblAssemblyMarginTitle.TabIndex = 8;
            lblAssemblyMarginTitle.Text = "Margin";
            // 
            // txtAssemblySellPrice
            // 
            txtAssemblySellPrice.Location = new Point(198, 353);
            txtAssemblySellPrice.Margin = new Padding(2, 2, 2, 2);
            txtAssemblySellPrice.Name = "txtAssemblySellPrice";
            txtAssemblySellPrice.Size = new Size(231, 33);
            txtAssemblySellPrice.TabIndex = 7;
            txtAssemblySellPrice.TextChanged += txtAssemblySellPrice_TextChanged;
            // 
            // lblAssemblySellPrice
            // 
            lblAssemblySellPrice.AutoSize = true;
            lblAssemblySellPrice.Location = new Point(17, 357);
            lblAssemblySellPrice.Margin = new Padding(2, 0, 2, 0);
            lblAssemblySellPrice.Name = "lblAssemblySellPrice";
            lblAssemblySellPrice.Size = new Size(101, 25);
            lblAssemblySellPrice.TabIndex = 6;
            lblAssemblySellPrice.Text = "Harga Jual";
            // 
            // lblTotalHppValue
            // 
            lblTotalHppValue.AutoSize = true;
            lblTotalHppValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalHppValue.Location = new Point(198, 319);
            lblTotalHppValue.Margin = new Padding(2, 0, 2, 0);
            lblTotalHppValue.Name = "lblTotalHppValue";
            lblTotalHppValue.Size = new Size(17, 19);
            lblTotalHppValue.TabIndex = 5;
            lblTotalHppValue.Text = "0";
            // 
            // lblTotalHppTitle
            // 
            lblTotalHppTitle.AutoSize = true;
            lblTotalHppTitle.Location = new Point(17, 323);
            lblTotalHppTitle.Margin = new Padding(2, 0, 2, 0);
            lblTotalHppTitle.Name = "lblTotalHppTitle";
            lblTotalHppTitle.Size = new Size(170, 25);
            lblTotalHppTitle.TabIndex = 4;
            lblTotalHppTitle.Text = "Harga Pokok (HPP)";
            // 
            // dgvMaterials
            // 
            dgvMaterials.AllowUserToAddRows = false;
            dgvMaterials.BackgroundColor = Color.White;
            dgvMaterials.BorderStyle = BorderStyle.None;
            dgvMaterials.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMaterials.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMaterials.Columns.AddRange(new DataGridViewColumn[] { colMaterialItem, colMaterialQty, colMaterialBaseUnit, colMaterialViewUnits, colMaterialUnit, colMaterialPrice, colMaterialSubtotal });
            dgvMaterials.Location = new Point(17, 108);
            dgvMaterials.Margin = new Padding(2, 2, 2, 2);
            dgvMaterials.Name = "dgvMaterials";
            dgvMaterials.RowHeadersVisible = false;
            dgvMaterials.RowHeadersWidth = 62;
            dgvMaterials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaterials.Size = new Size(1182, 197);
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
            colMaterialUnit.HeaderText = "Satuan";
            colMaterialUnit.MinimumWidth = 8;
            colMaterialUnit.Name = "colMaterialUnit";
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
            // btnRemoveMaterial
            // 
            btnRemoveMaterial.BackColor = Color.FromArgb(220, 53, 69);
            btnRemoveMaterial.FlatAppearance.BorderSize = 0;
            btnRemoveMaterial.FlatStyle = FlatStyle.Flat;
            btnRemoveMaterial.ForeColor = Color.White;
            btnRemoveMaterial.Location = new Point(198, 58);
            btnRemoveMaterial.Margin = new Padding(2, 2, 2, 2);
            btnRemoveMaterial.Name = "btnRemoveMaterial";
            btnRemoveMaterial.Size = new Size(169, 36);
            btnRemoveMaterial.TabIndex = 2;
            btnRemoveMaterial.Text = "Hapus Item";
            btnRemoveMaterial.UseVisualStyleBackColor = false;
            btnRemoveMaterial.Click += btnRemoveMaterial_Click;
            // 
            // btnAddMaterial
            // 
            btnAddMaterial.BackColor = Color.FromArgb(40, 167, 69);
            btnAddMaterial.FlatAppearance.BorderSize = 0;
            btnAddMaterial.FlatStyle = FlatStyle.Flat;
            btnAddMaterial.ForeColor = Color.White;
            btnAddMaterial.Location = new Point(17, 58);
            btnAddMaterial.Margin = new Padding(2, 2, 2, 2);
            btnAddMaterial.Name = "btnAddMaterial";
            btnAddMaterial.Size = new Size(169, 36);
            btnAddMaterial.TabIndex = 1;
            btnAddMaterial.Text = "Tambah Item";
            btnAddMaterial.UseVisualStyleBackColor = false;
            btnAddMaterial.Click += btnAddMaterial_Click;
            // 
            // lblAssemblyTitle
            // 
            lblAssemblyTitle.AutoSize = true;
            lblAssemblyTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAssemblyTitle.Location = new Point(17, 17);
            lblAssemblyTitle.Margin = new Padding(2, 0, 2, 0);
            lblAssemblyTitle.Name = "lblAssemblyTitle";
            lblAssemblyTitle.Size = new Size(195, 21);
            lblAssemblyTitle.TabIndex = 0;
            lblAssemblyTitle.Text = "Komponen Item Rakitan";
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
            pnlPricing.Controls.Add(label14);
            pnlPricing.Controls.Add(label8);
            pnlPricing.Controls.Add(txtDiscountFormula);
            pnlPricing.Controls.Add(txtSellPrice);
            pnlPricing.Controls.Add(txtBuyPrice);
            pnlPricing.Controls.Add(txtMinQty);
            pnlPricing.Controls.Add(txtStock);
            pnlPricing.Dock = DockStyle.Fill;
            pnlPricing.Location = new Point(0, 0);
            pnlPricing.Margin = new Padding(0, 0, 0, 10);
            pnlPricing.Name = "pnlPricing";
            pnlPricing.Size = new Size(989, 271);
            pnlPricing.TabIndex = 67;
            // 
            // lblStockOut
            // 
            lblStockOut.AutoSize = true;
            lblStockOut.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStockOut.ForeColor = Color.DodgerBlue;
            lblStockOut.Location = new Point(425, 21);
            lblStockOut.Margin = new Padding(2, 0, 2, 0);
            lblStockOut.Name = "lblStockOut";
            lblStockOut.Size = new Size(27, 19);
            lblStockOut.TabIndex = 56;
            lblStockOut.Text = "bb";
            // 
            // lblStockValueHpp
            // 
            lblStockValueHpp.AutoSize = true;
            lblStockValueHpp.Font = new Font("Segoe UI", 9F);
            lblStockValueHpp.ForeColor = Color.DimGray;
            lblStockValueHpp.Location = new Point(425, 100);
            lblStockValueHpp.Margin = new Padding(2, 0, 2, 0);
            lblStockValueHpp.Name = "lblStockValueHpp";
            lblStockValueHpp.Size = new Size(19, 15);
            lblStockValueHpp.TabIndex = 57;
            lblStockValueHpp.Text = "cc";
            // 
            // lblStockValueSell
            // 
            lblStockValueSell.AutoSize = true;
            lblStockValueSell.Font = new Font("Segoe UI", 9F);
            lblStockValueSell.ForeColor = Color.DimGray;
            lblStockValueSell.Location = new Point(425, 123);
            lblStockValueSell.Margin = new Padding(2, 0, 2, 0);
            lblStockValueSell.Name = "lblStockValueSell";
            lblStockValueSell.Size = new Size(19, 15);
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
            panel2.Location = new Point(628, 19);
            panel2.Margin = new Padding(2, 2, 2, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(336, 146);
            panel2.TabIndex = 2;
            // 
            // outHargaAkhir
            // 
            outHargaAkhir.AutoSize = true;
            outHargaAkhir.Location = new Point(162, 66);
            outHargaAkhir.Margin = new Padding(2, 0, 2, 0);
            outHargaAkhir.Name = "outHargaAkhir";
            outHargaAkhir.Size = new Size(16, 25);
            outHargaAkhir.TabIndex = 4;
            outHargaAkhir.Text = ".";
            // 
            // outMargin
            // 
            outMargin.AutoSize = true;
            outMargin.Location = new Point(162, 39);
            outMargin.Margin = new Padding(2, 0, 2, 0);
            outMargin.Name = "outMargin";
            outMargin.Size = new Size(16, 25);
            outMargin.TabIndex = 4;
            outMargin.Text = ".";
            // 
            // outDiskon
            // 
            outDiskon.AutoSize = true;
            outDiskon.Location = new Point(162, 13);
            outDiskon.Margin = new Padding(2, 0, 2, 0);
            outDiskon.Name = "outDiskon";
            outDiskon.Size = new Size(16, 25);
            outDiskon.TabIndex = 4;
            outDiskon.Text = ".";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(20, 66);
            label12.Margin = new Padding(2, 0, 2, 0);
            label12.Name = "label12";
            label12.Size = new Size(112, 25);
            label12.TabIndex = 0;
            label12.Text = "Harga Akhir";
            // 
            // Margin
            // 
            Margin.AutoSize = true;
            Margin.Location = new Point(20, 39);
            Margin.Margin = new Padding(2, 0, 2, 0);
            Margin.Name = "Margin";
            Margin.Size = new Size(73, 25);
            Margin.TabIndex = 0;
            Margin.Text = "Margin";
            // 
            // Diskon
            // 
            Diskon.AutoSize = true;
            Diskon.Location = new Point(20, 13);
            Diskon.Margin = new Padding(2, 0, 2, 0);
            Diskon.Name = "Diskon";
            Diskon.Size = new Size(69, 25);
            Diskon.TabIndex = 0;
            Diskon.Text = "Diskon";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(21, 180);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(143, 25);
            label11.TabIndex = 1;
            label11.Text = "Formula Diskon";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(21, 140);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(101, 25);
            label10.TabIndex = 1;
            label10.Text = "Harga Jual";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(21, 101);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(99, 25);
            label9.TabIndex = 1;
            label9.Text = "Harga Beli";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(21, 60);
            label14.Margin = new Padding(2, 0, 2, 0);
            label14.Name = "label14";
            label14.Size = new Size(78, 25);
            label14.TabIndex = 1;
            label14.Text = "min_qty";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(21, 19);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(55, 25);
            label8.TabIndex = 1;
            label8.Text = "stock";
            // 
            // txtDiscountFormula
            // 
            txtDiscountFormula.Location = new Point(179, 176);
            txtDiscountFormula.Margin = new Padding(2, 2, 2, 2);
            txtDiscountFormula.Name = "txtDiscountFormula";
            txtDiscountFormula.Size = new Size(229, 33);
            txtDiscountFormula.TabIndex = 19;
            // 
            // txtSellPrice
            // 
            txtSellPrice.Location = new Point(179, 136);
            txtSellPrice.Margin = new Padding(2, 2, 2, 2);
            txtSellPrice.Name = "txtSellPrice";
            txtSellPrice.Size = new Size(229, 33);
            txtSellPrice.TabIndex = 18;
            // 
            // txtBuyPrice
            // 
            txtBuyPrice.Location = new Point(179, 97);
            txtBuyPrice.Margin = new Padding(2, 2, 2, 2);
            txtBuyPrice.Name = "txtBuyPrice";
            txtBuyPrice.Size = new Size(229, 33);
            txtBuyPrice.TabIndex = 17;
            // 
            // txtMinQty
            // 
            txtMinQty.Location = new Point(179, 55);
            txtMinQty.Margin = new Padding(2, 2, 2, 2);
            txtMinQty.Name = "txtMinQty";
            txtMinQty.Size = new Size(100, 33);
            txtMinQty.TabIndex = 16;
            // 
            // txtStock
            // 
            txtStock.Location = new Point(179, 16);
            txtStock.Margin = new Padding(2, 2, 2, 2);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(100, 33);
            txtStock.TabIndex = 16;
            // 
            // dtpExpired
            // 
            dtpExpired.Checked = false;
            dtpExpired.Dock = DockStyle.Left;
            dtpExpired.Font = new Font("Segoe UI", 10F);
            dtpExpired.Format = DateTimePickerFormat.Short;
            dtpExpired.Location = new Point(171, 414);
            dtpExpired.Margin = new Padding(2, 2, 2, 2);
            dtpExpired.Name = "dtpExpired";
            dtpExpired.ShowCheckBox = true;
            dtpExpired.Size = new Size(168, 25);
            dtpExpired.TabIndex = 54;
            // 
            // lblExpired
            // 
            lblExpired.AutoSize = true;
            lblExpired.Dock = DockStyle.Fill;
            lblExpired.Font = new Font("Segoe UI", 10F);
            lblExpired.Location = new Point(2, 418);
            lblExpired.Margin = new Padding(2, 6, 2, 6);
            lblExpired.Name = "lblExpired";
            lblExpired.Size = new Size(165, 19);
            lblExpired.TabIndex = 55;
            lblExpired.Text = "Expired:";
            // 
            // txtNote
            // 
            txtNote.Dock = DockStyle.Fill;
            txtNote.Location = new Point(171, 377);
            txtNote.Margin = new Padding(2, 2, 2, 2);
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(323, 33);
            txtNote.TabIndex = 6;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(2, 381);
            label7.Margin = new Padding(2, 6, 2, 6);
            label7.Name = "label7";
            label7.Size = new Size(165, 25);
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
            btnCancel.Location = new Point(3, 2);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(154, 56);
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
            btnSimpan.Location = new Point(163, 2);
            btnSimpan.Margin = new Padding(3, 2, 3, 2);
            btnSimpan.Name = "btnSimpan";
            btnSimpan.Size = new Size(180, 56);
            btnSimpan.TabIndex = 50;
            btnSimpan.Text = "Simpan";
            btnSimpan.UseVisualStyleBackColor = true;
            btnSimpan.Click += btnSimpan_Click;
            // 
            // cmbSort
            // 
            cmbSort.Dock = DockStyle.Fill;
            cmbSort.FormattingEnabled = true;
            cmbSort.Location = new Point(172, 151);
            cmbSort.Name = "cmbSort";
            cmbSort.Size = new Size(321, 33);
            cmbSort.TabIndex = 56;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Dock = DockStyle.Fill;
            label13.Location = new Point(2, 154);
            label13.Margin = new Padding(2, 6, 2, 6);
            label13.Name = "label13";
            label13.Size = new Size(165, 27);
            label13.TabIndex = 0;
            label13.Text = "Urut Unit";
            // 
            // pnlRoot
            // 
            pnlRoot.BackColor = Color.Transparent;
            pnlRoot.Controls.Add(pnlBottomBar);
            pnlRoot.Controls.Add(splitMain);
            pnlRoot.Dock = DockStyle.Fill;
            pnlRoot.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pnlRoot.Location = new Point(0, 0);
            pnlRoot.Margin = new Padding(4, 4, 4, 4);
            pnlRoot.Name = "pnlRoot";
            pnlRoot.Padding = new Padding(21, 20, 21, 20);
            pnlRoot.Size = new Size(1574, 891);
            pnlRoot.TabIndex = 0;
            // 
            // pnlBottomBar
            // 
            pnlBottomBar.BackColor = Color.Transparent;
            pnlBottomBar.Controls.Add(flpBottomActions);
            pnlBottomBar.Dock = DockStyle.Bottom;
            pnlBottomBar.Location = new Point(21, 785);
            pnlBottomBar.Margin = new Padding(4, 4, 4, 4);
            pnlBottomBar.Name = "pnlBottomBar";
            pnlBottomBar.Padding = new Padding(0, 12, 0, 0);
            pnlBottomBar.Size = new Size(1532, 86);
            pnlBottomBar.TabIndex = 0;
            // 
            // flpBottomActions
            // 
            flpBottomActions.AutoSize = true;
            flpBottomActions.Controls.Add(btnSimpan);
            flpBottomActions.Controls.Add(btnCancel);
            flpBottomActions.Dock = DockStyle.Right;
            flpBottomActions.FlowDirection = FlowDirection.RightToLeft;
            flpBottomActions.Location = new Point(1186, 12);
            flpBottomActions.Margin = new Padding(0);
            flpBottomActions.Name = "flpBottomActions";
            flpBottomActions.Size = new Size(346, 74);
            flpBottomActions.TabIndex = 0;
            flpBottomActions.WrapContents = false;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(21, 20);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(pnlLeft);
            splitMain.Panel1MinSize = 520;
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(tlpRight);
            splitMain.Panel2MinSize = 700;
            splitMain.Size = new Size(1532, 851);
            splitMain.SplitterDistance = 535;
            splitMain.SplitterWidth = 8;
            splitMain.TabIndex = 1;
            // 
            // pnlLeft
            // 
            pnlLeft.AutoScroll = true;
            pnlLeft.BackColor = Color.Transparent;
            pnlLeft.Controls.Add(panel1);
            pnlLeft.Controls.Add(grpInventory);
            pnlLeft.Controls.Add(grpInfo);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(535, 851);
            pnlLeft.TabIndex = 0;
            // 
            // grpInventory
            // 
            grpInventory.Controls.Add(tlpInventory);
            grpInventory.Dock = DockStyle.Top;
            grpInventory.Location = new Point(0, 539);
            grpInventory.Name = "grpInventory";
            grpInventory.Padding = new Padding(11, 10, 11, 10);
            grpInventory.Size = new Size(518, 106);
            grpInventory.TabIndex = 8;
            grpInventory.TabStop = false;
            grpInventory.Text = "Persediaan";
            // 
            // tlpInventory
            // 
            tlpInventory.AutoSize = true;
            tlpInventory.ColumnCount = 2;
            tlpInventory.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 169F));
            tlpInventory.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpInventory.Controls.Add(lblWarehouse, 0, 0);
            tlpInventory.Controls.Add(cmbWarehouse, 1, 0);
            tlpInventory.Controls.Add(lblValuation, 0, 1);
            tlpInventory.Controls.Add(cmbValuation, 1, 1);
            tlpInventory.Dock = DockStyle.Top;
            tlpInventory.Location = new Point(11, 36);
            tlpInventory.Name = "tlpInventory";
            tlpInventory.RowCount = 2;
            tlpInventory.RowStyles.Add(new RowStyle());
            tlpInventory.RowStyles.Add(new RowStyle());
            tlpInventory.Size = new Size(496, 62);
            tlpInventory.TabIndex = 0;
            // 
            // grpInfo
            // 
            grpInfo.Controls.Add(tlpInfo);
            grpInfo.Dock = DockStyle.Top;
            grpInfo.Location = new Point(0, 0);
            grpInfo.Name = "grpInfo";
            grpInfo.Padding = new Padding(11, 10, 11, 10);
            grpInfo.Size = new Size(518, 539);
            grpInfo.TabIndex = 9;
            grpInfo.TabStop = false;
            grpInfo.Text = "Informasi Produk";
            // 
            // tlpInfo
            // 
            tlpInfo.AutoSize = true;
            tlpInfo.ColumnCount = 2;
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 169F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpInfo.Controls.Add(label1, 0, 0);
            tlpInfo.Controls.Add(txtBarcode, 1, 0);
            tlpInfo.Controls.Add(label2, 0, 1);
            tlpInfo.Controls.Add(txtName, 1, 1);
            tlpInfo.Controls.Add(lblProductType, 0, 2);
            tlpInfo.Controls.Add(cmbProductType, 1, 2);
            tlpInfo.Controls.Add(label3, 0, 3);
            tlpInfo.Controls.Add(cmbUnit, 1, 3);
            tlpInfo.Controls.Add(label13, 0, 4);
            tlpInfo.Controls.Add(cmbSort, 1, 4);
            tlpInfo.Controls.Add(label4, 0, 5);
            tlpInfo.Controls.Add(cmbCategory, 1, 5);
            tlpInfo.Controls.Add(label5, 0, 6);
            tlpInfo.Controls.Add(cmbSupplier, 1, 6);
            tlpInfo.Controls.Add(lblBrand, 0, 7);
            tlpInfo.Controls.Add(pnlBrandRow, 1, 7);
            tlpInfo.Controls.Add(lblRack, 0, 8);
            tlpInfo.Controls.Add(pnlRackRow, 1, 8);
            tlpInfo.Controls.Add(label7, 0, 9);
            tlpInfo.Controls.Add(txtNote, 1, 9);
            tlpInfo.Controls.Add(lblExpired, 0, 10);
            tlpInfo.Controls.Add(dtpExpired, 1, 10);
            tlpInfo.Dock = DockStyle.Top;
            tlpInfo.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tlpInfo.Location = new Point(11, 36);
            tlpInfo.Name = "tlpInfo";
            tlpInfo.RowCount = 11;
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.RowStyles.Add(new RowStyle());
            tlpInfo.Size = new Size(496, 443);
            tlpInfo.TabIndex = 0;
            // 
            // pnlBrandRow
            // 
            pnlBrandRow.Controls.Add(cmbBrand);
            pnlBrandRow.Controls.Add(btnAddBrand);
            pnlBrandRow.Location = new Point(171, 263);
            pnlBrandRow.Margin = new Padding(2, 2, 2, 2);
            pnlBrandRow.Name = "pnlBrandRow";
            pnlBrandRow.Size = new Size(229, 52);
            pnlBrandRow.TabIndex = 58;
            // 
            // pnlRackRow
            // 
            pnlRackRow.Controls.Add(cmbRack);
            pnlRackRow.Controls.Add(btnAddRack);
            pnlRackRow.Location = new Point(171, 319);
            pnlRackRow.Margin = new Padding(2, 2, 2, 2);
            pnlRackRow.Name = "pnlRackRow";
            pnlRackRow.Size = new Size(229, 54);
            pnlRackRow.TabIndex = 61;
            // 
            // tlpRight
            // 
            tlpRight.ColumnCount = 1;
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRight.Controls.Add(pnlPricing, 0, 0);
            tlpRight.Controls.Add(tabHarga, 0, 1);
            tlpRight.Dock = DockStyle.Fill;
            tlpRight.Location = new Point(0, 0);
            tlpRight.Name = "tlpRight";
            tlpRight.RowCount = 2;
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 281F));
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRight.Size = new Size(989, 851);
            tlpRight.TabIndex = 0;
            // 
            // ItemDetailForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 249);
            ClientSize = new Size(1574, 891);
            Controls.Add(pnlRoot);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimizeBox = false;
            MinimumSize = new Size(1572, 912);
            Name = "ItemDetailForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Detail Item";
            WindowState = FormWindowState.Maximized;
            Load += ItemDetailForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabHarga.ResumeLayout(false);
            tabPageUnitVariant.ResumeLayout(false);
            tabPageUnitVariant.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVariants).EndInit();
            Harga.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMultiPrice).EndInit();
            Bahan.ResumeLayout(false);
            Bahan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMaterials).EndInit();
            pnlPricing.ResumeLayout(false);
            pnlPricing.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            pnlRoot.ResumeLayout(false);
            pnlBottomBar.ResumeLayout(false);
            pnlBottomBar.PerformLayout();
            flpBottomActions.ResumeLayout(false);
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            pnlLeft.ResumeLayout(false);
            grpInventory.ResumeLayout(false);
            grpInventory.PerformLayout();
            tlpInventory.ResumeLayout(false);
            tlpInventory.PerformLayout();
            grpInfo.ResumeLayout(false);
            grpInfo.PerformLayout();
            tlpInfo.ResumeLayout(false);
            tlpInfo.PerformLayout();
            pnlBrandRow.ResumeLayout(false);
            pnlRackRow.ResumeLayout(false);
            tlpRight.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label lblProductType;
        private ComboBox cmbProductType;
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
        private Label label14;
        private TextBox txtMinQty;
        private Panel pnlRoot;
        private SplitContainer splitMain;
        private Panel pnlLeft;
        private GroupBox grpInfo;
        private TableLayoutPanel tlpInfo;
        private Panel pnlBrandRow;
        private Panel pnlRackRow;
        private GroupBox grpInventory;
        private TableLayoutPanel tlpInventory;
        private TableLayoutPanel tlpRight;
        private Panel pnlBottomBar;
        private FlowLayoutPanel flpBottomActions;
    }
}
