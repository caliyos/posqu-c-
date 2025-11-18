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
            Harga = new TabPage();
            btnDeletePrice = new Button();
            btnEditPrice = new Button();
            btnAddPrice = new Button();
            dgvMultiPrice = new DataGridView();
            btnMultiPrice = new Button();
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
            BoxOrPaket = new TabPage();
            Bahan = new TabPage();
            tabPageUnitVariant = new TabPage();
            txtNote = new TextBox();
            label7 = new Label();
            btnCancel = new Button();
            btnSimpan = new Button();
            btnUnitVariant = new Button();
            panel1.SuspendLayout();
            tabHarga.SuspendLayout();
            Harga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMultiPrice).BeginInit();
            panel2.SuspendLayout();
            tabPageUnitVariant.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(48, 32);
            label1.Name = "label1";
            label1.Size = new Size(76, 25);
            label1.TabIndex = 0;
            label1.Text = "Barcode";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(48, 72);
            label2.Name = "label2";
            label2.Size = new Size(119, 25);
            label2.TabIndex = 0;
            label2.Text = "Nama Barang";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(48, 136);
            label3.Name = "label3";
            label3.Size = new Size(52, 25);
            label3.TabIndex = 0;
            label3.Text = "Units";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(48, 184);
            label4.Name = "label4";
            label4.Size = new Size(84, 25);
            label4.TabIndex = 0;
            label4.Text = "Category";
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(48, 232);
            label5.Name = "label5";
            label5.Size = new Size(77, 25);
            label5.TabIndex = 0;
            label5.Text = "Supplier";
            label5.TextAlign = ContentAlignment.TopCenter;
            // 
            // txtBarcode
            // 
            txtBarcode.Location = new Point(240, 32);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(408, 31);
            txtBarcode.TabIndex = 1;
            // 
            // txtName
            // 
            txtName.Location = new Point(240, 72);
            txtName.Name = "txtName";
            txtName.Size = new Size(408, 31);
            txtName.TabIndex = 2;
            // 
            // cmbUnit
            // 
            cmbUnit.FormattingEnabled = true;
            cmbUnit.Location = new Point(240, 128);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(304, 33);
            cmbUnit.TabIndex = 3;
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(240, 176);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(304, 33);
            cmbCategory.TabIndex = 4;
            // 
            // cmbSupplier
            // 
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Location = new Point(240, 224);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(408, 33);
            cmbSupplier.TabIndex = 5;
            // 
            // panel1
            // 
            panel1.Controls.Add(chk_IsProduced);
            panel1.Controls.Add(chk_IsPackage);
            panel1.Controls.Add(chk_HasMaterials);
            panel1.Controls.Add(chk_is_changeprice_p);
            panel1.Controls.Add(chk_RequireNotePayment);
            panel1.Controls.Add(chk_IsSellable);
            panel1.Controls.Add(chk_IsPurchasable);
            panel1.Controls.Add(chk_is_inventory_p);
            panel1.Controls.Add(label6);
            panel1.Location = new Point(48, 328);
            panel1.Name = "panel1";
            panel1.Size = new Size(656, 376);
            panel1.TabIndex = 7;
            // 
            // chk_IsProduced
            // 
            chk_IsProduced.AutoSize = true;
            chk_IsProduced.Location = new Point(32, 336);
            chk_IsProduced.Name = "chk_IsProduced";
            chk_IsProduced.Size = new Size(130, 29);
            chk_IsProduced.TabIndex = 14;
            chk_IsProduced.Text = "Di produksi";
            chk_IsProduced.UseVisualStyleBackColor = true;
            // 
            // chk_IsPackage
            // 
            chk_IsPackage.AutoSize = true;
            chk_IsPackage.Location = new Point(32, 296);
            chk_IsPackage.Name = "chk_IsPackage";
            chk_IsPackage.Size = new Size(116, 29);
            chk_IsPackage.TabIndex = 13;
            chk_IsPackage.Text = "Box/Paket";
            chk_IsPackage.UseVisualStyleBackColor = true;
            // 
            // chk_HasMaterials
            // 
            chk_HasMaterials.AutoSize = true;
            chk_HasMaterials.Location = new Point(32, 256);
            chk_HasMaterials.Name = "chk_HasMaterials";
            chk_HasMaterials.Size = new Size(157, 29);
            chk_HasMaterials.TabIndex = 12;
            chk_HasMaterials.Text = "Memiliki Bahan";
            chk_HasMaterials.UseVisualStyleBackColor = true;
            // 
            // chk_is_changeprice_p
            // 
            chk_is_changeprice_p.AutoSize = true;
            chk_is_changeprice_p.Location = new Point(32, 216);
            chk_is_changeprice_p.Name = "chk_is_changeprice_p";
            chk_is_changeprice_p.Size = new Size(277, 29);
            chk_is_changeprice_p.TabIndex = 11;
            chk_is_changeprice_p.Text = "Bisa edit harga saat Pembelian";
            chk_is_changeprice_p.UseVisualStyleBackColor = true;
            // 
            // chk_RequireNotePayment
            // 
            chk_RequireNotePayment.AutoSize = true;
            chk_RequireNotePayment.Location = new Point(32, 176);
            chk_RequireNotePayment.Name = "chk_RequireNotePayment";
            chk_RequireNotePayment.Size = new Size(223, 29);
            chk_RequireNotePayment.TabIndex = 10;
            chk_RequireNotePayment.Text = "Catatan Saat Pembelian";
            chk_RequireNotePayment.UseVisualStyleBackColor = true;
            // 
            // chk_IsSellable
            // 
            chk_IsSellable.AutoSize = true;
            chk_IsSellable.Location = new Point(32, 136);
            chk_IsSellable.Name = "chk_IsSellable";
            chk_IsSellable.Size = new Size(82, 29);
            chk_IsSellable.TabIndex = 9;
            chk_IsSellable.Text = "Dijual";
            chk_IsSellable.UseVisualStyleBackColor = true;
            // 
            // chk_IsPurchasable
            // 
            chk_IsPurchasable.AutoSize = true;
            chk_IsPurchasable.Location = new Point(32, 96);
            chk_IsPurchasable.Name = "chk_IsPurchasable";
            chk_IsPurchasable.Size = new Size(83, 29);
            chk_IsPurchasable.TabIndex = 8;
            chk_IsPurchasable.Text = "Dibeli";
            chk_IsPurchasable.UseVisualStyleBackColor = true;
            // 
            // chk_is_inventory_p
            // 
            chk_is_inventory_p.AutoSize = true;
            chk_is_inventory_p.Location = new Point(32, 56);
            chk_is_inventory_p.Name = "chk_is_inventory_p";
            chk_is_inventory_p.Size = new Size(140, 29);
            chk_is_inventory_p.TabIndex = 7;
            chk_is_inventory_p.Text = "Hitung Stock";
            chk_is_inventory_p.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 16);
            label6.Name = "label6";
            label6.Size = new Size(117, 25);
            label6.TabIndex = 0;
            label6.Text = "Item Settings";
            // 
            // tabHarga
            // 
            tabHarga.Controls.Add(Harga);
            tabHarga.Controls.Add(tabPageUnitVariant);
            tabHarga.Controls.Add(BoxOrPaket);
            tabHarga.Controls.Add(Bahan);
            tabHarga.Location = new Point(768, 32);
            tabHarga.Name = "tabHarga";
            tabHarga.SelectedIndex = 0;
            tabHarga.Size = new Size(968, 672);
            tabHarga.TabIndex = 15;
            // 
            // Harga
            // 
            Harga.Controls.Add(btnDeletePrice);
            Harga.Controls.Add(btnEditPrice);
            Harga.Controls.Add(btnAddPrice);
            Harga.Controls.Add(dgvMultiPrice);
            Harga.Controls.Add(btnMultiPrice);
            Harga.Controls.Add(panel2);
            Harga.Controls.Add(label11);
            Harga.Controls.Add(label10);
            Harga.Controls.Add(label9);
            Harga.Controls.Add(label8);
            Harga.Controls.Add(txtDiscountFormula);
            Harga.Controls.Add(txtSellPrice);
            Harga.Controls.Add(txtBuyPrice);
            Harga.Controls.Add(txtStock);
            Harga.Location = new Point(4, 34);
            Harga.Name = "Harga";
            Harga.Padding = new Padding(3);
            Harga.Size = new Size(960, 634);
            Harga.TabIndex = 0;
            Harga.Text = "Harga";
            Harga.UseVisualStyleBackColor = true;
            // 
            // btnDeletePrice
            // 
            btnDeletePrice.Location = new Point(560, 328);
            btnDeletePrice.Name = "btnDeletePrice";
            btnDeletePrice.Size = new Size(112, 34);
            btnDeletePrice.TabIndex = 53;
            btnDeletePrice.Text = "delete";
            btnDeletePrice.UseVisualStyleBackColor = true;
            btnDeletePrice.Click += btnDeletePrice_Click;
            // 
            // btnEditPrice
            // 
            btnEditPrice.Location = new Point(440, 328);
            btnEditPrice.Name = "btnEditPrice";
            btnEditPrice.Size = new Size(112, 34);
            btnEditPrice.TabIndex = 53;
            btnEditPrice.Text = "edit";
            btnEditPrice.UseVisualStyleBackColor = true;
            btnEditPrice.Click += btnEditPrice_Click;
            // 
            // btnAddPrice
            // 
            btnAddPrice.Location = new Point(312, 328);
            btnAddPrice.Name = "btnAddPrice";
            btnAddPrice.Size = new Size(112, 34);
            btnAddPrice.TabIndex = 53;
            btnAddPrice.Text = "add";
            btnAddPrice.UseVisualStyleBackColor = true;
            btnAddPrice.Click += btnAddPrice_Click;
            // 
            // dgvMultiPrice
            // 
            dgvMultiPrice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMultiPrice.Location = new Point(64, 384);
            dgvMultiPrice.Name = "dgvMultiPrice";
            dgvMultiPrice.RowHeadersWidth = 62;
            dgvMultiPrice.Size = new Size(788, 225);
            dgvMultiPrice.TabIndex = 52;
            // 
            // btnMultiPrice
            // 
            btnMultiPrice.Location = new Point(64, 328);
            btnMultiPrice.Name = "btnMultiPrice";
            btnMultiPrice.Size = new Size(224, 34);
            btnMultiPrice.TabIndex = 21;
            btnMultiPrice.Text = "Set Multi Harga";
            btnMultiPrice.UseVisualStyleBackColor = true;
            btnMultiPrice.Click += btnMultiPrice_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(outHargaAkhir);
            panel2.Controls.Add(outMargin);
            panel2.Controls.Add(outDiskon);
            panel2.Controls.Add(label12);
            panel2.Controls.Add(Margin);
            panel2.Controls.Add(Diskon);
            panel2.Location = new Point(512, 40);
            panel2.Name = "panel2";
            panel2.Size = new Size(376, 176);
            panel2.TabIndex = 2;
            // 
            // outHargaAkhir
            // 
            outHargaAkhir.AutoSize = true;
            outHargaAkhir.Location = new Point(192, 80);
            outHargaAkhir.Name = "outHargaAkhir";
            outHargaAkhir.Size = new Size(16, 25);
            outHargaAkhir.TabIndex = 4;
            outHargaAkhir.Text = ".";
            // 
            // outMargin
            // 
            outMargin.AutoSize = true;
            outMargin.Location = new Point(192, 48);
            outMargin.Name = "outMargin";
            outMargin.Size = new Size(16, 25);
            outMargin.TabIndex = 4;
            outMargin.Text = ".";
            // 
            // outDiskon
            // 
            outDiskon.AutoSize = true;
            outDiskon.Location = new Point(192, 16);
            outDiskon.Name = "outDiskon";
            outDiskon.Size = new Size(16, 25);
            outDiskon.TabIndex = 4;
            outDiskon.Text = ".";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(24, 80);
            label12.Name = "label12";
            label12.Size = new Size(106, 25);
            label12.TabIndex = 0;
            label12.Text = "Harga Akhir";
            // 
            // Margin
            // 
            Margin.AutoSize = true;
            Margin.Location = new Point(24, 48);
            Margin.Name = "Margin";
            Margin.Size = new Size(68, 25);
            Margin.TabIndex = 0;
            Margin.Text = "Margin";
            // 
            // Diskon
            // 
            Diskon.AutoSize = true;
            Diskon.Location = new Point(24, 16);
            Diskon.Name = "Diskon";
            Diskon.Size = new Size(67, 25);
            Diskon.TabIndex = 0;
            Diskon.Text = "Diskon";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(24, 168);
            label11.Name = "label11";
            label11.Size = new Size(137, 25);
            label11.TabIndex = 1;
            label11.Text = "Formula Diskon";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(24, 120);
            label10.Name = "label10";
            label10.Size = new Size(94, 25);
            label10.TabIndex = 1;
            label10.Text = "Harga Jual";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(24, 80);
            label9.Name = "label9";
            label9.Size = new Size(92, 25);
            label9.TabIndex = 1;
            label9.Text = "Harga Beli";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(24, 40);
            label8.Name = "label8";
            label8.Size = new Size(54, 25);
            label8.TabIndex = 1;
            label8.Text = "stock";
            // 
            // txtDiscountFormula
            // 
            txtDiscountFormula.Location = new Point(208, 168);
            txtDiscountFormula.Name = "txtDiscountFormula";
            txtDiscountFormula.Size = new Size(272, 31);
            txtDiscountFormula.TabIndex = 19;
            // 
            // txtSellPrice
            // 
            txtSellPrice.Location = new Point(208, 120);
            txtSellPrice.Name = "txtSellPrice";
            txtSellPrice.Size = new Size(272, 31);
            txtSellPrice.TabIndex = 18;
            // 
            // txtBuyPrice
            // 
            txtBuyPrice.Location = new Point(208, 80);
            txtBuyPrice.Name = "txtBuyPrice";
            txtBuyPrice.Size = new Size(272, 31);
            txtBuyPrice.TabIndex = 17;
            // 
            // txtStock
            // 
            txtStock.Location = new Point(208, 40);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(272, 31);
            txtStock.TabIndex = 16;
            // 
            // BoxOrPaket
            // 
            BoxOrPaket.Location = new Point(4, 34);
            BoxOrPaket.Name = "BoxOrPaket";
            BoxOrPaket.Padding = new Padding(3);
            BoxOrPaket.Size = new Size(960, 634);
            BoxOrPaket.TabIndex = 1;
            BoxOrPaket.Text = "Box / Paket";
            BoxOrPaket.UseVisualStyleBackColor = true;
            // 
            // Bahan
            // 
            Bahan.Location = new Point(4, 34);
            Bahan.Name = "Bahan";
            Bahan.Size = new Size(960, 634);
            Bahan.TabIndex = 2;
            Bahan.Text = "Item Berbahan";
            Bahan.UseVisualStyleBackColor = true;
            // 
            // tabPageUnitVariant
            // 
            tabPageUnitVariant.Controls.Add(btnUnitVariant);
            tabPageUnitVariant.Location = new Point(4, 34);
            tabPageUnitVariant.Name = "tabPageUnitVariant";
            tabPageUnitVariant.Size = new Size(960, 634);
            tabPageUnitVariant.TabIndex = 3;
            tabPageUnitVariant.Text = "UnitVariant";
            tabPageUnitVariant.UseVisualStyleBackColor = true;
            // 
            // txtNote
            // 
            txtNote.Location = new Point(240, 280);
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(408, 31);
            txtNote.TabIndex = 6;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(48, 280);
            label7.Name = "label7";
            label7.Size = new Size(101, 25);
            label7.TabIndex = 0;
            label7.Text = "Keterangan";
            label7.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(1488, 704);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 34);
            btnCancel.TabIndex = 51;
            btnCancel.Text = "batal";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSimpan
            // 
            btnSimpan.Location = new Point(1624, 704);
            btnSimpan.Name = "btnSimpan";
            btnSimpan.Size = new Size(112, 34);
            btnSimpan.TabIndex = 50;
            btnSimpan.Text = "simpan";
            btnSimpan.UseVisualStyleBackColor = true;
            btnSimpan.Click += btnSimpan_Click;
            // 
            // btnUnitVariant
            // 
            btnUnitVariant.Location = new Point(24, 24);
            btnUnitVariant.Name = "btnUnitVariant";
            btnUnitVariant.Size = new Size(192, 34);
            btnUnitVariant.TabIndex = 0;
            btnUnitVariant.Text = "Set Up Unit Variant ";
            btnUnitVariant.UseVisualStyleBackColor = true;
            btnUnitVariant.Click += btnUnitVariant_Click;
            // 
            // ItemDetailForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1752, 901);
            Controls.Add(btnSimpan);
            Controls.Add(btnCancel);
            Controls.Add(tabHarga);
            Controls.Add(panel1);
            Controls.Add(cmbSupplier);
            Controls.Add(cmbCategory);
            Controls.Add(cmbUnit);
            Controls.Add(txtNote);
            Controls.Add(txtName);
            Controls.Add(txtBarcode);
            Controls.Add(label7);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ItemDetailForm";
            Text = "ItemDetailForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabHarga.ResumeLayout(false);
            Harga.ResumeLayout(false);
            Harga.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMultiPrice).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabPageUnitVariant.ResumeLayout(false);
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
        private TabPage BoxOrPaket;
        private TextBox txtNote;
        private Label label7;
        private Label label8;
        private TextBox txtStock;
        private Label label9;
        private TextBox txtSellPrice;
        private TextBox txtBuyPrice;
        private Label label10;
        private TabPage Bahan;
        private Button btnMultiPrice;
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
    }
}