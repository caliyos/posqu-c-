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

        private Panel quickBar;
        private Button btnExportQuick;
        private Button btnImportQuick;

        private Panel panelSummary;
        private Label lblSumItems;
        private Label lblSumQty;
        private Label lblSumStockValue;
        private Label lblSumRetailValue;
        private Label lblSumInvRatio;

        private Panel actionPanel;
        private CheckBox chkActionSelectAll;
        private Label lblCari;
        private TextBox txtActionSearch;
        private Button btnActionRefresh;
        private Button btnViewBase;
        private Button btnViewAll;
        private Button btnLowStock;
        private Button btnPrintBarcode;
        private Label lblWarehouseFilter;
        private ComboBox cmbWarehouseFilter;

        private void InitializeComponent()
        {
            tablePanel = new Panel();
            dataGridView1 = new DataGridView();
            bottomContainer = new Panel();
            lblPagingInfo = new Label();
            cmbPageSize = new ComboBox();
            btnFirstPage = new Button();
            btnPrevious = new Button();
            btnNext = new Button();
            btnLastPage = new Button();
            topContainer = new Panel();
            actionPanel = new Panel();
            chkActionSelectAll = new CheckBox();
            lblCari = new Label();
            txtActionSearch = new TextBox();
            btnActionRefresh = new Button();
            btnViewBase = new Button();
            btnViewAll = new Button();
            btnLowStock = new Button();
            btnPrintBarcode = new Button();
            panelSummary = new Panel();
            lblWarehouseFilter = new Label();
            cmbWarehouseFilter = new ComboBox();
            label9 = new Label();
            label10 = new Label();
            quickBar = new Panel();
            btnAddProduct = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnExportQuick = new Button();
            btnImportQuick = new Button();
            chkSelectAll = new CheckBox();
            btnStockAdjs = new Button();
            btnRefresh = new Button();
            btnImportExcel = new Button();
            btnExportExcel = new Button();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblSumItems = new Label();
            lblSumQty = new Label();
            lblSumStockValue = new Label();
            lblSumRetailValue = new Label();
            lblSumInvRatio = new Label();
            tablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            bottomContainer.SuspendLayout();
            topContainer.SuspendLayout();
            actionPanel.SuspendLayout();
            panelSummary.SuspendLayout();
            quickBar.SuspendLayout();
            SuspendLayout();
            // 
            // tablePanel
            // 
            tablePanel.BackColor = Color.White;
            tablePanel.Controls.Add(dataGridView1);
            tablePanel.Controls.Add(bottomContainer);
            tablePanel.Controls.Add(topContainer);
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.Location = new Point(0, 0);
            tablePanel.Name = "tablePanel";
            tablePanel.Size = new Size(1200, 800);
            tablePanel.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersHeight = 34;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 175);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1200, 565);
            dataGridView1.TabIndex = 0;
            // 
            // bottomContainer
            // 
            bottomContainer.BackColor = Color.WhiteSmoke;
            bottomContainer.Controls.Add(lblPagingInfo);
            bottomContainer.Controls.Add(cmbPageSize);
            bottomContainer.Controls.Add(btnFirstPage);
            bottomContainer.Controls.Add(btnPrevious);
            bottomContainer.Controls.Add(btnNext);
            bottomContainer.Controls.Add(btnLastPage);
            bottomContainer.Dock = DockStyle.Bottom;
            bottomContainer.Location = new Point(0, 740);
            bottomContainer.Name = "bottomContainer";
            bottomContainer.Size = new Size(1200, 60);
            bottomContainer.TabIndex = 1;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Font = new Font("Segoe UI", 10F);
            lblPagingInfo.Location = new Point(15, 20);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(202, 28);
            lblPagingInfo.TabIndex = 0;
            lblPagingInfo.Text = "Menampilkan 0 dari 0";
            // 
            // cmbPageSize
            // 
            cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPageSize.Font = new Font("Segoe UI", 10F);
            cmbPageSize.Location = new Point(180, 17);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(80, 36);
            cmbPageSize.TabIndex = 1;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            // 
            // btnFirstPage
            // 
            btnFirstPage.BackColor = Color.White;
            btnFirstPage.FlatStyle = FlatStyle.Flat;
            btnFirstPage.Location = new Point(280, 15);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(70, 30);
            btnFirstPage.TabIndex = 2;
            btnFirstPage.Text = "First";
            btnFirstPage.UseVisualStyleBackColor = false;
            btnFirstPage.Click += btnFirstPage_Click;
            // 
            // btnPrevious
            // 
            btnPrevious.BackColor = Color.White;
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Location = new Point(360, 15);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(70, 30);
            btnPrevious.TabIndex = 3;
            btnPrevious.Text = "Prev";
            btnPrevious.UseVisualStyleBackColor = false;
            btnPrevious.Click += btnPrevious_Click;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.White;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Location = new Point(440, 15);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(70, 30);
            btnNext.TabIndex = 4;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;
            // 
            // btnLastPage
            // 
            btnLastPage.BackColor = Color.White;
            btnLastPage.FlatStyle = FlatStyle.Flat;
            btnLastPage.Location = new Point(520, 15);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(70, 30);
            btnLastPage.TabIndex = 5;
            btnLastPage.Text = "Last";
            btnLastPage.UseVisualStyleBackColor = false;
            btnLastPage.Click += btnLastPage_Click;
            // 
            // topContainer
            // 
            topContainer.Controls.Add(actionPanel);
            topContainer.Controls.Add(panelSummary);
            topContainer.Controls.Add(quickBar);
            topContainer.Dock = DockStyle.Top;
            topContainer.Location = new Point(0, 0);
            topContainer.Name = "topContainer";
            topContainer.Size = new Size(1200, 175);
            topContainer.TabIndex = 2;
            // 
            // actionPanel
            // 
            actionPanel.BackColor = Color.FromArgb(250, 250, 250);
            actionPanel.Controls.Add(chkActionSelectAll);
            actionPanel.Controls.Add(lblCari);
            actionPanel.Controls.Add(txtActionSearch);
            actionPanel.Controls.Add(btnActionRefresh);
            actionPanel.Controls.Add(btnViewBase);
            actionPanel.Controls.Add(btnViewAll);
            actionPanel.Controls.Add(btnLowStock);
            actionPanel.Controls.Add(btnPrintBarcode);
            actionPanel.Dock = DockStyle.Top;
            actionPanel.Location = new Point(0, 115);
            actionPanel.Name = "actionPanel";
            actionPanel.Size = new Size(1200, 60);
            actionPanel.TabIndex = 0;
            // 
            // chkActionSelectAll
            // 
            chkActionSelectAll.AutoSize = true;
            chkActionSelectAll.Font = new Font("Segoe UI", 10F);
            chkActionSelectAll.Location = new Point(15, 20);
            chkActionSelectAll.Name = "chkActionSelectAll";
            chkActionSelectAll.Size = new Size(139, 32);
            chkActionSelectAll.TabIndex = 0;
            chkActionSelectAll.Text = "Pilih Semua";
            chkActionSelectAll.CheckedChanged += chkActionSelectAll_CheckedChanged2;
            // 
            // lblCari
            // 
            lblCari.AutoSize = true;
            lblCari.Font = new Font("Segoe UI", 10F);
            lblCari.Location = new Point(130, 20);
            lblCari.Name = "lblCari";
            lblCari.Size = new Size(50, 28);
            lblCari.TabIndex = 1;
            lblCari.Text = "Cari:";
            // 
            // txtActionSearch
            // 
            txtActionSearch.Font = new Font("Segoe UI", 11F);
            txtActionSearch.Location = new Point(175, 17);
            txtActionSearch.Name = "txtActionSearch";
            txtActionSearch.Size = new Size(250, 37);
            txtActionSearch.TabIndex = 2;
            txtActionSearch.TextChanged += txtActionSearch_TextChanged;
            // 
            // btnActionRefresh
            // 
            btnActionRefresh.BackColor = Color.White;
            btnActionRefresh.FlatStyle = FlatStyle.Flat;
            btnActionRefresh.Font = new Font("Segoe UI", 10F);
            btnActionRefresh.Location = new Point(440, 9);
            btnActionRefresh.Name = "btnActionRefresh";
            btnActionRefresh.Size = new Size(90, 45);
            btnActionRefresh.TabIndex = 3;
            btnActionRefresh.Text = "Refresh";
            btnActionRefresh.UseVisualStyleBackColor = false;
            btnActionRefresh.Click += btnRefresh_Click;
            // 
            // btnViewBase
            // 
            btnViewBase.BackColor = Color.White;
            btnViewBase.FlatStyle = FlatStyle.Flat;
            btnViewBase.Font = new Font("Segoe UI", 10F);
            btnViewBase.Location = new Point(540, 9);
            btnViewBase.Name = "btnViewBase";
            btnViewBase.Size = new Size(110, 45);
            btnViewBase.TabIndex = 4;
            btnViewBase.Text = "Base View";
            btnViewBase.UseVisualStyleBackColor = false;
            btnViewBase.Click += btnViewBase_Click;
            // 
            // btnViewAll
            // 
            btnViewAll.BackColor = Color.White;
            btnViewAll.FlatStyle = FlatStyle.Flat;
            btnViewAll.Font = new Font("Segoe UI", 10F);
            btnViewAll.Location = new Point(660, 9);
            btnViewAll.Name = "btnViewAll";
            btnViewAll.Size = new Size(110, 45);
            btnViewAll.TabIndex = 5;
            btnViewAll.Text = "All View";
            btnViewAll.UseVisualStyleBackColor = false;
            btnViewAll.Click += btnViewAll_Click;
            // 
            // btnLowStock
            // 
            btnLowStock.BackColor = Color.White;
            btnLowStock.FlatStyle = FlatStyle.Flat;
            btnLowStock.Font = new Font("Segoe UI", 10F);
            btnLowStock.Location = new Point(780, 3);
            btnLowStock.Name = "btnLowStock";
            btnLowStock.Size = new Size(160, 51);
            btnLowStock.TabIndex = 6;
            btnLowStock.Text = "Order By Stock";
            btnLowStock.UseVisualStyleBackColor = false;
            btnLowStock.Click += btnLowStock_Click;
            // 
            // btnPrintBarcode
            // 
            btnPrintBarcode.BackColor = Color.White;
            btnPrintBarcode.FlatStyle = FlatStyle.Flat;
            btnPrintBarcode.Font = new Font("Segoe UI", 10F);
            btnPrintBarcode.Location = new Point(946, 7);
            btnPrintBarcode.Name = "btnPrintBarcode";
            btnPrintBarcode.Size = new Size(140, 45);
            btnPrintBarcode.TabIndex = 7;
            btnPrintBarcode.Text = "Print Barcode";
            btnPrintBarcode.UseVisualStyleBackColor = false;
            btnPrintBarcode.Click += btnPrintBarcode_Click;
            // 
            // panelSummary
            // 
            panelSummary.BackColor = Color.White;
            panelSummary.Controls.Add(cmbWarehouseFilter);
            panelSummary.Controls.Add(lblWarehouseFilter);
            panelSummary.Controls.Add(label9);
            panelSummary.Controls.Add(label10);
            panelSummary.Dock = DockStyle.Top;
            panelSummary.Location = new Point(0, 55);
            panelSummary.Name = "panelSummary";
            panelSummary.Size = new Size(1200, 60);
            panelSummary.TabIndex = 1;
            // 
            // lblWarehouseFilter
            // 
            lblWarehouseFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblWarehouseFilter.AutoSize = true;
            lblWarehouseFilter.Font = new Font("Segoe UI", 10F);
            lblWarehouseFilter.Location = new Point(840, 16);
            lblWarehouseFilter.Name = "lblWarehouseFilter";
            lblWarehouseFilter.Size = new Size(77, 28);
            lblWarehouseFilter.TabIndex = 2;
            lblWarehouseFilter.Text = "Gudang:";
            // 
            // cmbWarehouseFilter
            // 
            cmbWarehouseFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbWarehouseFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouseFilter.Font = new Font("Segoe UI", 10F);
            cmbWarehouseFilter.FormattingEnabled = true;
            cmbWarehouseFilter.Location = new Point(920, 12);
            cmbWarehouseFilter.Name = "cmbWarehouseFilter";
            cmbWarehouseFilter.Size = new Size(265, 36);
            cmbWarehouseFilter.TabIndex = 3;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.Location = new Point(10, 35);
            label9.Name = "label9";
            label9.Size = new Size(282, 28);
            label9.TabIndex = 0;
            label9.Text = "Nilai Stok (HPP): 0 | Nilai Jual: 0";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label10.Location = new Point(10, 10);
            label10.Name = "label10";
            label10.Size = new Size(324, 30);
            label10.TabIndex = 1;
            label10.Text = "Total Item: 0 | Jumlah Stock: 0";
            // 
            // quickBar
            // 
            quickBar.BackColor = Color.FromArgb(240, 240, 240);
            quickBar.Controls.Add(btnAddProduct);
            quickBar.Controls.Add(btnEdit);
            quickBar.Controls.Add(btnDelete);
            quickBar.Controls.Add(btnExportQuick);
            quickBar.Controls.Add(btnImportQuick);
            quickBar.Controls.Add(btnStockAdjs);
            quickBar.Dock = DockStyle.Top;
            quickBar.Location = new Point(0, 0);
            quickBar.Name = "quickBar";
            quickBar.Size = new Size(1200, 55);
            quickBar.TabIndex = 2;
            // 
            // btnAddProduct
            // 
            btnAddProduct.BackColor = Color.FromArgb(0, 120, 215);
            btnAddProduct.FlatStyle = FlatStyle.Flat;
            btnAddProduct.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddProduct.ForeColor = Color.White;
            btnAddProduct.Location = new Point(10, 10);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(130, 35);
            btnAddProduct.TabIndex = 0;
            btnAddProduct.Text = "+ Tambah Item";
            btnAddProduct.UseVisualStyleBackColor = false;
            btnAddProduct.Click += btnAddProduct_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(240, 173, 78);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(150, 10);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(90, 35);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(217, 83, 79);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(250, 10);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 35);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Hapus";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // btnExportQuick
            // 
            btnExportQuick.BackColor = Color.White;
            btnExportQuick.FlatStyle = FlatStyle.Flat;
            btnExportQuick.Font = new Font("Segoe UI", 10F);
            btnExportQuick.Location = new Point(350, 10);
            btnExportQuick.Name = "btnExportQuick";
            btnExportQuick.Size = new Size(110, 35);
            btnExportQuick.TabIndex = 3;
            btnExportQuick.Text = "Export Excel";
            btnExportQuick.UseVisualStyleBackColor = false;
            btnExportQuick.Click += btnExportExcel_Click;
            // 
            // btnImportQuick
            // 
            btnImportQuick.BackColor = Color.White;
            btnImportQuick.FlatStyle = FlatStyle.Flat;
            btnImportQuick.Font = new Font("Segoe UI", 10F);
            btnImportQuick.Location = new Point(470, 10);
            btnImportQuick.Name = "btnImportQuick";
            btnImportQuick.Size = new Size(110, 35);
            btnImportQuick.TabIndex = 4;
            btnImportQuick.Text = "Import Excel";
            btnImportQuick.UseVisualStyleBackColor = false;
            btnImportQuick.Click += btnImportExcel_Click;
            // 
            // btnStockAdjs
            // 
            btnStockAdjs.BackColor = Color.White;
            btnStockAdjs.FlatStyle = FlatStyle.Flat;
            btnStockAdjs.Font = new Font("Segoe UI", 10F);
            btnStockAdjs.Location = new Point(590, 10);
            btnStockAdjs.Name = "btnStockAdjs";
            btnStockAdjs.Size = new Size(150, 35);
            btnStockAdjs.TabIndex = 5;
            btnStockAdjs.Text = "Stock / Opname";
            btnStockAdjs.UseVisualStyleBackColor = false;
            btnStockAdjs.Click += btnStockAdjs_Click;
            // 
            // chkSelectAll
            // 
            chkSelectAll.Location = new Point(0, 0);
            chkSelectAll.Name = "chkSelectAll";
            chkSelectAll.Size = new Size(104, 24);
            chkSelectAll.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 0;
            // 
            // btnImportExcel
            // 
            btnImportExcel.Location = new Point(0, 0);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(75, 23);
            btnImportExcel.TabIndex = 0;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Location = new Point(0, 0);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(75, 23);
            btnExportExcel.TabIndex = 0;
            // 
            // lblSearch
            // 
            lblSearch.Location = new Point(0, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(100, 23);
            lblSearch.TabIndex = 0;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(0, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(100, 31);
            txtSearch.TabIndex = 0;
            // 
            // lblSumItems
            // 
            lblSumItems.Location = new Point(0, 0);
            lblSumItems.Name = "lblSumItems";
            lblSumItems.Size = new Size(100, 23);
            lblSumItems.TabIndex = 0;
            // 
            // lblSumQty
            // 
            lblSumQty.Location = new Point(0, 0);
            lblSumQty.Name = "lblSumQty";
            lblSumQty.Size = new Size(100, 23);
            lblSumQty.TabIndex = 0;
            // 
            // lblSumStockValue
            // 
            lblSumStockValue.Location = new Point(0, 0);
            lblSumStockValue.Name = "lblSumStockValue";
            lblSumStockValue.Size = new Size(100, 23);
            lblSumStockValue.TabIndex = 0;
            // 
            // lblSumRetailValue
            // 
            lblSumRetailValue.Location = new Point(0, 0);
            lblSumRetailValue.Name = "lblSumRetailValue";
            lblSumRetailValue.Size = new Size(100, 23);
            lblSumRetailValue.TabIndex = 0;
            // 
            // lblSumInvRatio
            // 
            lblSumInvRatio.Location = new Point(0, 0);
            lblSumInvRatio.Name = "lblSumInvRatio";
            lblSumInvRatio.Size = new Size(100, 23);
            lblSumInvRatio.TabIndex = 0;
            // 
            // ProductPage
            // 
            ClientSize = new Size(1200, 800);
            Controls.Add(tablePanel);
            Name = "ProductPage";
            Text = "Data Produk";
            Load += ProductPage_Load;
            tablePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            bottomContainer.ResumeLayout(false);
            bottomContainer.PerformLayout();
            topContainer.ResumeLayout(false);
            actionPanel.ResumeLayout(false);
            actionPanel.PerformLayout();
            panelSummary.ResumeLayout(false);
            panelSummary.PerformLayout();
            quickBar.ResumeLayout(false);
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
        private Button btnStockAdjs;
        private Panel bottomContainer;
        private Panel topContainer;

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
