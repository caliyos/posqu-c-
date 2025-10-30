namespace POS_qu
{
    partial class ProductPage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        private Panel inputPanel;
        private Panel tablePanel;
        private TextBox txtName, txtBuyPrice, txtSellPrice, txtStock, txtBarcode, txtDescription;
        private ComboBox cmbUnit, cmbGroup;
        private PictureBox pictureBox;
        private Button btnUploadImage, btnSave, btnUpdate, btnDelete, btnClose;
        private DataGridView dataGridView1;

        private void InitializeComponent()
        {
            inputPanel = new Panel();
            btnCancelEdit = new Button();
            btnUnitVariant = new Button();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtName = new TextBox();
            txtBuyPrice = new TextBox();
            txtSellPrice = new TextBox();
            txtStock = new TextBox();
            txtBarcode = new TextBox();
            cmbUnit = new ComboBox();
            cmbGroup = new ComboBox();
            txtDescription = new TextBox();
            pictureBox = new PictureBox();
            btnUploadImage = new Button();
            btnSave = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            tablePanel = new Panel();
            btnImportExcel = new Button();
            btnExportExcel = new Button();
            cmbPageSize = new ComboBox();
            lblSearch = new Label();
            btnLastPage = new Button();
            btnFirstPage = new Button();
            lblPagingInfo = new Label();
            txtSearch = new TextBox();
            btnPrevious = new Button();
            btnNext = new Button();
            btnClose = new Button();
            dataGridView1 = new DataGridView();
            inputPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            tablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // inputPanel
            // 
            inputPanel.BackColor = Color.LightGray;
            inputPanel.Controls.Add(btnCancelEdit);
            inputPanel.Controls.Add(btnUnitVariant);
            inputPanel.Controls.Add(label8);
            inputPanel.Controls.Add(label7);
            inputPanel.Controls.Add(label6);
            inputPanel.Controls.Add(label5);
            inputPanel.Controls.Add(label4);
            inputPanel.Controls.Add(label3);
            inputPanel.Controls.Add(label2);
            inputPanel.Controls.Add(label1);
            inputPanel.Controls.Add(txtName);
            inputPanel.Controls.Add(txtBuyPrice);
            inputPanel.Controls.Add(txtSellPrice);
            inputPanel.Controls.Add(txtStock);
            inputPanel.Controls.Add(txtBarcode);
            inputPanel.Controls.Add(cmbUnit);
            inputPanel.Controls.Add(cmbGroup);
            inputPanel.Controls.Add(txtDescription);
            inputPanel.Controls.Add(pictureBox);
            inputPanel.Controls.Add(btnUploadImage);
            inputPanel.Controls.Add(btnSave);
            inputPanel.Controls.Add(btnUpdate);
            inputPanel.Controls.Add(btnDelete);
            inputPanel.Dock = DockStyle.Left;
            inputPanel.Location = new Point(0, 0);
            inputPanel.Name = "inputPanel";
            inputPanel.Padding = new Padding(20);
            inputPanel.Size = new Size(400, 1370);
            inputPanel.TabIndex = 1;
            // 
            // btnCancelEdit
            // 
            btnCancelEdit.Location = new Point(280, 784);
            btnCancelEdit.Name = "btnCancelEdit";
            btnCancelEdit.Size = new Size(112, 34);
            btnCancelEdit.TabIndex = 23;
            btnCancelEdit.Text = "Cancel Edit";
            btnCancelEdit.UseVisualStyleBackColor = true;
            btnCancelEdit.Click += btnCancelEdit_Click;
            // 
            // btnUnitVariant
            // 
            btnUnitVariant.Location = new Point(16, 672);
            btnUnitVariant.Name = "btnUnitVariant";
            btnUnitVariant.Size = new Size(112, 34);
            btnUnitVariant.TabIndex = 22;
            btnUnitVariant.Text = "Unit Variant";
            btnUnitVariant.UseVisualStyleBackColor = true;
            btnUnitVariant.Click += btnUnitVariant_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(16, 576);
            label8.Name = "label8";
            label8.Size = new Size(101, 25);
            label8.TabIndex = 21;
            label8.Text = "Keterangan";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 496);
            label7.Name = "label7";
            label7.Size = new Size(62, 25);
            label7.TabIndex = 21;
            label7.Text = "Group";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 416);
            label6.Name = "label6";
            label6.Size = new Size(44, 25);
            label6.TabIndex = 21;
            label6.Text = "Unit";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 328);
            label5.Name = "label5";
            label5.Size = new Size(76, 25);
            label5.TabIndex = 21;
            label5.Text = "Barcode";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 256);
            label4.Name = "label4";
            label4.Size = new Size(55, 25);
            label4.TabIndex = 21;
            label4.Text = "Stock";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 176);
            label3.Name = "label3";
            label3.Size = new Size(94, 25);
            label3.TabIndex = 21;
            label3.Text = "Harga Jual";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 96);
            label2.Name = "label2";
            label2.Size = new Size(92, 25);
            label2.TabIndex = 21;
            label2.Text = "Harga Beli";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 16);
            label1.Name = "label1";
            label1.Size = new Size(59, 25);
            label1.TabIndex = 21;
            label1.Text = "Name";
            // 
            // txtName
            // 
            txtName.Location = new Point(20, 45);
            txtName.Name = "txtName";
            txtName.Size = new Size(340, 31);
            txtName.TabIndex = 1;
            // 
            // txtBuyPrice
            // 
            txtBuyPrice.Location = new Point(20, 125);
            txtBuyPrice.Name = "txtBuyPrice";
            txtBuyPrice.Size = new Size(340, 31);
            txtBuyPrice.TabIndex = 3;
            txtBuyPrice.KeyPress += TxtBuyPrice_KeyPress;
            // 
            // txtSellPrice
            // 
            txtSellPrice.Location = new Point(20, 205);
            txtSellPrice.Name = "txtSellPrice";
            txtSellPrice.Size = new Size(340, 31);
            txtSellPrice.TabIndex = 5;
            txtSellPrice.KeyPress += TxtSellPrice_KeyPress;
            // 
            // txtStock
            // 
            txtStock.Location = new Point(20, 285);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(340, 31);
            txtStock.TabIndex = 7;
            txtStock.KeyPress += TxtStock_KeyPress;
            // 
            // txtBarcode
            // 
            txtBarcode.Location = new Point(20, 365);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(340, 31);
            txtBarcode.TabIndex = 9;
            // 
            // cmbUnit
            // 
            cmbUnit.Location = new Point(20, 445);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(340, 33);
            cmbUnit.TabIndex = 11;
            // 
            // cmbGroup
            // 
            cmbGroup.Location = new Point(20, 525);
            cmbGroup.Name = "cmbGroup";
            cmbGroup.Size = new Size(340, 33);
            cmbGroup.TabIndex = 13;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(20, 605);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(340, 31);
            txtDescription.TabIndex = 15;
            // 
            // pictureBox
            // 
            pictureBox.BackColor = Color.White;
            pictureBox.Location = new Point(16, 728);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(150, 100);
            pictureBox.TabIndex = 16;
            pictureBox.TabStop = false;
            // 
            // btnUploadImage
            // 
            btnUploadImage.Location = new Point(16, 840);
            btnUploadImage.Name = "btnUploadImage";
            btnUploadImage.Size = new Size(120, 30);
            btnUploadImage.TabIndex = 17;
            btnUploadImage.Text = "Upload Image";
            btnUploadImage.Click += btnUploadImage_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(184, 736);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 35);
            btnSave.TabIndex = 18;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(184, 784);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(90, 35);
            btnUpdate.TabIndex = 19;
            btnUpdate.Text = "Update";
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(184, 832);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 35);
            btnDelete.TabIndex = 20;
            btnDelete.Text = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // tablePanel
            // 
            tablePanel.BackColor = Color.White;
            tablePanel.Controls.Add(btnImportExcel);
            tablePanel.Controls.Add(btnExportExcel);
            tablePanel.Controls.Add(cmbPageSize);
            tablePanel.Controls.Add(lblSearch);
            tablePanel.Controls.Add(btnLastPage);
            tablePanel.Controls.Add(btnFirstPage);
            tablePanel.Controls.Add(lblPagingInfo);
            tablePanel.Controls.Add(txtSearch);
            tablePanel.Controls.Add(btnPrevious);
            tablePanel.Controls.Add(btnNext);
            tablePanel.Controls.Add(btnClose);
            tablePanel.Controls.Add(dataGridView1);
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.Location = new Point(400, 0);
            tablePanel.Name = "tablePanel";
            tablePanel.Padding = new Padding(10);
            tablePanel.Size = new Size(1784, 1370);
            tablePanel.TabIndex = 0;
            // 
            // btnImportExcel
            // 
            btnImportExcel.Location = new Point(1600, 1312);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(168, 34);
            btnImportExcel.TabIndex = 8;
            btnImportExcel.Text = "Import Excel";
            btnImportExcel.UseVisualStyleBackColor = true;
            btnImportExcel.Click += btnImportExcel_Click;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Location = new Point(1600, 1256);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(168, 34);
            btnExportExcel.TabIndex = 8;
            btnExportExcel.Text = "Export Excel";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(16, 1272);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 7;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(24, 32);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(64, 25);
            lblSearch.TabIndex = 6;
            lblSearch.Text = "Search";
            // 
            // btnLastPage
            // 
            btnLastPage.Location = new Point(1432, 1248);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(112, 34);
            btnLastPage.TabIndex = 5;
            btnLastPage.Text = "Last";
            btnLastPage.UseVisualStyleBackColor = true;
            btnLastPage.Click += btnLastPage_Click;
            // 
            // btnFirstPage
            // 
            btnFirstPage.Location = new Point(1024, 1248);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(112, 34);
            btnFirstPage.TabIndex = 5;
            btnFirstPage.Text = "First";
            btnFirstPage.UseVisualStyleBackColor = true;
            btnFirstPage.Click += btnFirstPage_Click;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(16, 1240);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(103, 25);
            lblPagingInfo.TabIndex = 4;
            lblPagingInfo.Text = "Paging Info";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(104, 24);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(256, 31);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new Point(1296, 1248);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(112, 34);
            btnPrevious.TabIndex = 1;
            btnPrevious.Text = "Prev";
            btnPrevious.UseVisualStyleBackColor = true;
            btnPrevious.Click += btnPrevious_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(1152, 1248);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 1;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Red;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Arial", 14F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1720, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(50, 40);
            btnClose.TabIndex = 0;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += BtnClose_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(16, 64);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(1760, 1168);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // ProductPage
            // 
            ClientSize = new Size(2184, 1370);
            Controls.Add(tablePanel);
            Controls.Add(inputPanel);
            Name = "ProductPage";
            Text = "Product Page";
            Load += ProductPage_Load;
            inputPanel.ResumeLayout(false);
            inputPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            tablePanel.ResumeLayout(false);
            tablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }
        private Label label1;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private TextBox txtSearch;
        private Button btnPrevious;
        private Button btnNext;
        private Label lblPagingInfo;
        private Button btnLastPage;
        private Button btnFirstPage;
        private Label lblSearch;
        private ComboBox cmbPageSize;
        private Button btnUnitVariant;
        private Button btnCancelEdit;
        private Button btnExportExcel;
        private Button btnImportExcel;

        //private void SetupLabelAndControl(string labelText, Control control, int y)
        //{
        //    Label label = new Label();
        //    label.Text = labelText;
        //    label.Location = new Point(20, y);
        //    label.Size = new Size(150, 25);
        //    inputPanel.Controls.Add(label);

        //    control.Location = new Point(20, y + 25);
        //    control.Size = new Size(340, 30);
        //    inputPanel.Controls.Add(control);
        //}

    }
}
