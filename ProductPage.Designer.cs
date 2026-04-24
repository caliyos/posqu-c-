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
        private Button btnActionStockAdj;
        private Button btnActionRefresh;
        private Button btnViewBase;
        private Button btnViewAll;
        private Button btnPrintBarcode;

        private void InitializeComponent()
        {
            tablePanel = new Panel();
            btnDelete = new Button();
            btnEdit = new Button();
            chkSelectAll = new CheckBox();
            btnStockAdjs = new Button();
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
            
            quickBar = new Panel();
            btnExportQuick = new Button();
            btnImportQuick = new Button();
            panelSummary = new Panel();
            lblSumItems = new Label();
            lblSumQty = new Label();
            lblSumStockValue = new Label();
            lblSumRetailValue = new Label();
            lblSumInvRatio = new Label();
            actionPanel = new Panel();
            chkActionSelectAll = new CheckBox();
            lblCari = new Label();
            txtActionSearch = new TextBox();
            btnActionStockAdj = new Button();
            btnActionRefresh = new Button();
            btnViewBase = new Button();
            btnViewAll = new Button();
            btnPrintBarcode = new Button();
            
            Panel topContainer = new Panel();
            Panel bottomContainer = new Panel();
            
            tablePanel.SuspendLayout();
            quickBar.SuspendLayout();
            panelSummary.SuspendLayout();
            actionPanel.SuspendLayout();
            topContainer.SuspendLayout();
            bottomContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            // topContainer
            // 
            topContainer.Dock = DockStyle.Top;
            topContainer.Height = 175;
            topContainer.Controls.Add(actionPanel);
            topContainer.Controls.Add(panelSummary);
            topContainer.Controls.Add(quickBar);
            
            // 
            // quickBar
            // 
            quickBar.BackColor = Color.FromArgb(240, 240, 240);
            quickBar.Controls.Add(btnAddProduct);
            quickBar.Controls.Add(btnEdit);
            quickBar.Controls.Add(btnDelete);
            quickBar.Controls.Add(btnExportQuick);
            quickBar.Controls.Add(btnImportQuick);
            quickBar.Dock = DockStyle.Top;
            quickBar.Height = 55;
            quickBar.Name = "quickBar";
            
            // btnAddProduct
            btnAddProduct.BackColor = Color.FromArgb(0, 120, 215);
            btnAddProduct.ForeColor = Color.White;
            btnAddProduct.FlatStyle = FlatStyle.Flat;
            btnAddProduct.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddProduct.Location = new Point(10, 10);
            btnAddProduct.Size = new Size(130, 35);
            btnAddProduct.Text = "+ Tambah Item";
            btnAddProduct.Click += btnAddProduct_Click;
            
            // btnEdit
            btnEdit.BackColor = Color.FromArgb(240, 173, 78);
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.Location = new Point(150, 10);
            btnEdit.Size = new Size(90, 35);
            btnEdit.Text = "Edit";
            btnEdit.Click += btnEdit_Click;
            
            // btnDelete
            btnDelete.BackColor = Color.FromArgb(217, 83, 79);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.Location = new Point(250, 10);
            btnDelete.Size = new Size(90, 35);
            btnDelete.Text = "Hapus";
            btnDelete.Click += btnDelete_Click_1;
            
            // btnExportQuick
            btnExportQuick.BackColor = Color.White;
            btnExportQuick.FlatStyle = FlatStyle.Flat;
            btnExportQuick.Font = new Font("Segoe UI", 10F);
            btnExportQuick.Location = new Point(350, 10);
            btnExportQuick.Size = new Size(110, 35);
            btnExportQuick.Text = "Export Excel";
            btnExportQuick.Click += btnExportExcel_Click;
            
            // btnImportQuick
            btnImportQuick.BackColor = Color.White;
            btnImportQuick.FlatStyle = FlatStyle.Flat;
            btnImportQuick.Font = new Font("Segoe UI", 10F);
            btnImportQuick.Location = new Point(470, 10);
            btnImportQuick.Size = new Size(110, 35);
            btnImportQuick.Text = "Import Excel";
            btnImportQuick.Click += btnImportExcel_Click;
            
            // 
            // panelSummary
            // 
            panelSummary.BackColor = Color.White;
            panelSummary.Controls.Add(label9);
            panelSummary.Controls.Add(label10);
            panelSummary.Dock = DockStyle.Top;
            panelSummary.Height = 60;
            panelSummary.Name = "panelSummary";
            
            // label10 (Sum Qty / Total Items)
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label10.Location = new Point(10, 10);
            label10.Text = "Total Item: 0 | Jumlah Stock: 0";
            
            // label9 (Sum Stock Value / Retail Value)
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.Location = new Point(10, 35);
            label9.Text = "Nilai Stok (HPP): 0 | Nilai Jual: 0";
            
            // 
            // actionPanel
            // 
            actionPanel.BackColor = Color.FromArgb(250, 250, 250);
            actionPanel.Controls.Add(chkActionSelectAll);
            actionPanel.Controls.Add(lblCari);
            actionPanel.Controls.Add(txtActionSearch);
            actionPanel.Controls.Add(btnActionStockAdj);
            actionPanel.Controls.Add(btnActionRefresh);
            actionPanel.Controls.Add(btnViewBase);
            actionPanel.Controls.Add(btnViewAll);
            actionPanel.Controls.Add(btnPrintBarcode);
            actionPanel.Dock = DockStyle.Top;
            actionPanel.Height = 60;
            actionPanel.Name = "actionPanel";
            
            // chkActionSelectAll
            chkActionSelectAll.AutoSize = true;
            chkActionSelectAll.Font = new Font("Segoe UI", 10F);
            chkActionSelectAll.Location = new Point(15, 20);
            chkActionSelectAll.Text = "Pilih Semua";
            chkActionSelectAll.CheckedChanged += chkActionSelectAll_CheckedChanged2;
            
            // lblCari
            lblCari.AutoSize = true;
            lblCari.Font = new Font("Segoe UI", 10F);
            lblCari.Location = new Point(130, 20);
            lblCari.Text = "Cari:";
            
            // txtActionSearch
            txtActionSearch.Font = new Font("Segoe UI", 11F);
            txtActionSearch.Location = new Point(175, 17);
            txtActionSearch.Size = new Size(250, 27);
            txtActionSearch.TextChanged += txtActionSearch_TextChanged;
            
            // btnActionRefresh
            btnActionRefresh.BackColor = Color.White;
            btnActionRefresh.FlatStyle = FlatStyle.Flat;
            btnActionRefresh.Font = new Font("Segoe UI", 10F);
            btnActionRefresh.Location = new Point(440, 15);
            btnActionRefresh.Size = new Size(90, 32);
            btnActionRefresh.Text = "Refresh";
            btnActionRefresh.Click += btnRefresh_Click;
            
            // btnActionStockAdj
            btnActionStockAdj.BackColor = Color.White;
            btnActionStockAdj.FlatStyle = FlatStyle.Flat;
            btnActionStockAdj.Font = new Font("Segoe UI", 10F);
            btnActionStockAdj.Location = new Point(540, 15);
            btnActionStockAdj.Size = new Size(130, 32);
            btnActionStockAdj.Text = "Update Stock";
            btnActionStockAdj.Click += btnStockAdjs_Click;

            // btnViewBase
            btnViewBase.BackColor = Color.White;
            btnViewBase.FlatStyle = FlatStyle.Flat;
            btnViewBase.Font = new Font("Segoe UI", 10F);
            btnViewBase.Location = new Point(680, 15);
            btnViewBase.Size = new Size(110, 32);
            btnViewBase.Text = "Base View";
            btnViewBase.Click += btnViewBase_Click;

            // btnViewAll
            btnViewAll.BackColor = Color.White;
            btnViewAll.FlatStyle = FlatStyle.Flat;
            btnViewAll.Font = new Font("Segoe UI", 10F);
            btnViewAll.Location = new Point(800, 15);
            btnViewAll.Size = new Size(110, 32);
            btnViewAll.Text = "All View";
            btnViewAll.Click += btnViewAll_Click;

            // btnPrintBarcode
            btnPrintBarcode.BackColor = Color.White;
            btnPrintBarcode.FlatStyle = FlatStyle.Flat;
            btnPrintBarcode.Font = new Font("Segoe UI", 10F);
            btnPrintBarcode.Location = new Point(920, 15);
            btnPrintBarcode.Size = new Size(140, 32);
            btnPrintBarcode.Text = "Print Barcode";
            btnPrintBarcode.Click += btnPrintBarcode_Click;
            
            // 
            // bottomContainer
            // 
            bottomContainer.Dock = DockStyle.Bottom;
            bottomContainer.Height = 60;
            bottomContainer.BackColor = Color.WhiteSmoke;
            bottomContainer.Controls.Add(lblPagingInfo);
            bottomContainer.Controls.Add(cmbPageSize);
            bottomContainer.Controls.Add(btnFirstPage);
            bottomContainer.Controls.Add(btnPrevious);
            bottomContainer.Controls.Add(btnNext);
            bottomContainer.Controls.Add(btnLastPage);
            
            // lblPagingInfo
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Font = new Font("Segoe UI", 10F);
            lblPagingInfo.Location = new Point(15, 20);
            lblPagingInfo.Text = "Menampilkan 0 dari 0";
            
            // cmbPageSize
            cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPageSize.Font = new Font("Segoe UI", 10F);
            cmbPageSize.Location = new Point(180, 17);
            cmbPageSize.Size = new Size(80, 25);
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            
            // btnFirstPage
            btnFirstPage.BackColor = Color.White;
            btnFirstPage.FlatStyle = FlatStyle.Flat;
            btnFirstPage.Location = new Point(280, 15);
            btnFirstPage.Size = new Size(70, 30);
            btnFirstPage.Text = "First";
            btnFirstPage.Click += btnFirstPage_Click;
            
            // btnPrevious
            btnPrevious.BackColor = Color.White;
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Location = new Point(360, 15);
            btnPrevious.Size = new Size(70, 30);
            btnPrevious.Text = "Prev";
            btnPrevious.Click += btnPrevious_Click;
            
            // btnNext
            btnNext.BackColor = Color.White;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Location = new Point(440, 15);
            btnNext.Size = new Size(70, 30);
            btnNext.Text = "Next";
            btnNext.Click += btnNext_Click;
            
            // btnLastPage
            btnLastPage.BackColor = Color.White;
            btnLastPage.FlatStyle = FlatStyle.Flat;
            btnLastPage.Location = new Point(520, 15);
            btnLastPage.Size = new Size(70, 30);
            btnLastPage.Text = "Last";
            btnLastPage.Click += btnLastPage_Click;
            
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 175);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.TabIndex = 0;
            
            // 
            // ProductPage
            // 
            ClientSize = new Size(1200, 800);
            Controls.Add(tablePanel);
            Name = "ProductPage";
            Text = "Data Produk";
            Load += ProductPage_Load;
            
            tablePanel.ResumeLayout(false);
            quickBar.ResumeLayout(false);
            panelSummary.ResumeLayout(false);
            panelSummary.PerformLayout();
            actionPanel.ResumeLayout(false);
            actionPanel.PerformLayout();
            topContainer.ResumeLayout(false);
            bottomContainer.ResumeLayout(false);
            bottomContainer.PerformLayout();
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
        private Button btnStockAdjs;

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
