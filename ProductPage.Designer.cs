using System.Windows.Forms;

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
        private Panel tablePanel;
        private DataGridView dataGridView1;

        private void InitializeComponent()
        {
            tablePanel = new Panel();
            btnDelete = new Button();
            btnEdit = new Button();
            chkSelectAll = new CheckBox();
            btnRefresh = new Button();
            btnAddProduct = new Button();
            label10 = new Label();
            label9 = new Label();
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
            dataGridView1 = new DataGridView();
            tablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // tablePanel
            // 
            tablePanel.BackColor = Color.White;
            tablePanel.Controls.Add(btnDelete);
            tablePanel.Controls.Add(btnEdit);
            tablePanel.Controls.Add(chkSelectAll);
            tablePanel.Controls.Add(btnRefresh);
            tablePanel.Controls.Add(btnAddProduct);
            tablePanel.Controls.Add(label10);
            tablePanel.Controls.Add(label9);
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
            tablePanel.Controls.Add(dataGridView1);
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.Location = new Point(0, 0);
            tablePanel.Name = "tablePanel";
            tablePanel.Padding = new Padding(10);
            tablePanel.Size = new Size(2184, 1370);
            tablePanel.TabIndex = 0;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(992, 184);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(112, 34);
            btnDelete.TabIndex = 27;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(856, 184);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(112, 34);
            btnEdit.TabIndex = 27;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // chkSelectAll
            // 
            chkSelectAll.AutoSize = true;
            chkSelectAll.Location = new Point(40, 184);
            chkSelectAll.Name = "chkSelectAll";
            chkSelectAll.Size = new Size(109, 29);
            chkSelectAll.TabIndex = 26;
            chkSelectAll.Text = "Select All";
            chkSelectAll.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1232, 184);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 34);
            btnRefresh.TabIndex = 25;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnAddProduct
            // 
            btnAddProduct.Location = new Point(616, 184);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(208, 34);
            btnAddProduct.TabIndex = 24;
            btnAddProduct.Text = "Tambah Item";
            btnAddProduct.UseVisualStyleBackColor = true;
            btnAddProduct.Click += btnAddProduct_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(168, 24);
            label10.Name = "label10";
            label10.Size = new Size(115, 25);
            label10.TabIndex = 9;
            label10.Text = "Jumlah Stock";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(40, 24);
            label9.Name = "label9";
            label9.Size = new Size(94, 25);
            label9.TabIndex = 9;
            label9.Text = "Nilai Stock";
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
            cmbPageSize.Location = new Point(32, 1288);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 7;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(216, 192);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(64, 25);
            lblSearch.TabIndex = 6;
            lblSearch.Text = "Search";
            // 
            // btnLastPage
            // 
            btnLastPage.Location = new Point(968, 1248);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(112, 34);
            btnLastPage.TabIndex = 5;
            btnLastPage.Text = "Last";
            btnLastPage.UseVisualStyleBackColor = true;
            btnLastPage.Click += btnLastPage_Click;
            // 
            // btnFirstPage
            // 
            btnFirstPage.Location = new Point(560, 1248);
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
            lblPagingInfo.Location = new Point(32, 1256);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(103, 25);
            lblPagingInfo.TabIndex = 4;
            lblPagingInfo.Text = "Paging Info";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(296, 184);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(256, 31);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new Point(832, 1248);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(112, 34);
            btnPrevious.TabIndex = 1;
            btnPrevious.Text = "Prev";
            btnPrevious.UseVisualStyleBackColor = true;
            btnPrevious.Click += btnPrevious_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(688, 1248);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 1;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(24, 232);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(2136, 1000);
            dataGridView1.TabIndex = 0;
            // 
            // ProductPage
            // 
            ClientSize = new Size(2184, 1370);
            Controls.Add(tablePanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ProductPage";
            Text = "Product Page";
            Load += ProductPage_Load;
            tablePanel.ResumeLayout(false);
            tablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }


        private TextBox txtSearch;
        private Button btnPrevious;
        private Button btnNext;
        private Label lblPagingInfo;
        private Button btnLastPage;
        private Button btnFirstPage;
        private Label lblSearch;
        private ComboBox cmbPageSize;
        private Button btnExportExcel;
        private Button btnImportExcel;
        private Label label10;
        private Label label9;
        private Button btnAddProduct;
        private Button btnRefresh;
        private CheckBox chkSelectAll;
        private Button btnDelete;
        private Button btnEdit;

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
