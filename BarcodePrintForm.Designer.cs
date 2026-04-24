namespace POS_qu
{
    partial class BarcodePrintForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            panelLeftTop = new Panel();
            lblTemplate = new Label();
            cmbTemplate = new ComboBox();
            lblSearch = new Label();
            txtSearch = new TextBox();
            chkShowName = new CheckBox();
            chkShowPrice = new CheckBox();
            dgvItems = new DataGridView();
            colSelect = new DataGridViewCheckBoxColumn();
            colBarcode = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colUnit = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colQtyLabels = new DataGridViewTextBoxColumn();
            panelLeftBottom = new Panel();
            btnPreview = new Button();
            btnPrint = new Button();
            btnExportExcel = new Button();
            btnPageSetup = new Button();
            btnClose = new Button();
            panelRightTop = new Panel();
            lblPreview = new Label();
            previewControl = new PrintPreviewControl();
            pageSetupDialog1 = new PageSetupDialog();
            printDialog1 = new PrintDialog();
            printPreviewDialog1 = new PrintPreviewDialog();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panelLeftTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            panelLeftBottom.SuspendLayout();
            panelRightTop.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dgvItems);
            splitContainer1.Panel1.Controls.Add(panelLeftBottom);
            splitContainer1.Panel1.Controls.Add(panelLeftTop);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(previewControl);
            splitContainer1.Panel2.Controls.Add(panelRightTop);
            splitContainer1.Size = new Size(1400, 820);
            splitContainer1.SplitterDistance = 620;
            splitContainer1.TabIndex = 0;
            // 
            // panelLeftTop
            // 
            panelLeftTop.BackColor = Color.White;
            panelLeftTop.Controls.Add(lblTemplate);
            panelLeftTop.Controls.Add(cmbTemplate);
            panelLeftTop.Controls.Add(lblSearch);
            panelLeftTop.Controls.Add(txtSearch);
            panelLeftTop.Controls.Add(chkShowName);
            panelLeftTop.Controls.Add(chkShowPrice);
            panelLeftTop.Dock = DockStyle.Top;
            panelLeftTop.Location = new Point(0, 0);
            panelLeftTop.Name = "panelLeftTop";
            panelLeftTop.Padding = new Padding(12);
            panelLeftTop.Size = new Size(620, 92);
            panelLeftTop.TabIndex = 0;
            // 
            // lblTemplate
            // 
            lblTemplate.AutoSize = true;
            lblTemplate.Font = new Font("Segoe UI", 10F);
            lblTemplate.Location = new Point(12, 12);
            lblTemplate.Name = "lblTemplate";
            lblTemplate.Size = new Size(74, 23);
            lblTemplate.TabIndex = 0;
            lblTemplate.Text = "Template";
            // 
            // cmbTemplate
            // 
            cmbTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTemplate.Font = new Font("Segoe UI", 10F);
            cmbTemplate.FormattingEnabled = true;
            cmbTemplate.Location = new Point(12, 38);
            cmbTemplate.Name = "cmbTemplate";
            cmbTemplate.Size = new Size(250, 31);
            cmbTemplate.TabIndex = 1;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 10F);
            lblSearch.Location = new Point(280, 12);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(38, 23);
            lblSearch.TabIndex = 2;
            lblSearch.Text = "Cari";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(280, 38);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(210, 30);
            txtSearch.TabIndex = 3;
            // 
            // chkShowName
            // 
            chkShowName.AutoSize = true;
            chkShowName.Font = new Font("Segoe UI", 10F);
            chkShowName.Location = new Point(12, 70);
            chkShowName.Name = "chkShowName";
            chkShowName.Size = new Size(115, 27);
            chkShowName.TabIndex = 4;
            chkShowName.Text = "Tampil Nama";
            chkShowName.UseVisualStyleBackColor = true;
            // 
            // chkShowPrice
            // 
            chkShowPrice.AutoSize = true;
            chkShowPrice.Font = new Font("Segoe UI", 10F);
            chkShowPrice.Location = new Point(140, 70);
            chkShowPrice.Name = "chkShowPrice";
            chkShowPrice.Size = new Size(120, 27);
            chkShowPrice.TabIndex = 5;
            chkShowPrice.Text = "Tampil Harga";
            chkShowPrice.UseVisualStyleBackColor = true;
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersHeight = 42;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { colSelect, colBarcode, colName, colUnit, colPrice, colQtyLabels });
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.Location = new Point(0, 92);
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowTemplate.Height = 40;
            dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItems.Size = new Size(620, 668);
            dgvItems.TabIndex = 1;
            // 
            // colSelect
            // 
            colSelect.HeaderText = "";
            colSelect.MinimumWidth = 6;
            colSelect.Name = "colSelect";
            colSelect.Width = 40;
            // 
            // colBarcode
            // 
            colBarcode.HeaderText = "Barcode";
            colBarcode.MinimumWidth = 6;
            colBarcode.Name = "colBarcode";
            colBarcode.ReadOnly = true;
            colBarcode.Width = 140;
            // 
            // colName
            // 
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.HeaderText = "Nama";
            colName.MinimumWidth = 6;
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colUnit
            // 
            colUnit.HeaderText = "Unit";
            colUnit.MinimumWidth = 6;
            colUnit.Name = "colUnit";
            colUnit.ReadOnly = true;
            colUnit.Width = 70;
            // 
            // colPrice
            // 
            colPrice.HeaderText = "Harga";
            colPrice.MinimumWidth = 6;
            colPrice.Name = "colPrice";
            colPrice.ReadOnly = true;
            colPrice.Width = 90;
            // 
            // colQtyLabels
            // 
            colQtyLabels.HeaderText = "Qty Label";
            colQtyLabels.MinimumWidth = 6;
            colQtyLabels.Name = "colQtyLabels";
            colQtyLabels.Width = 90;
            // 
            // panelLeftBottom
            // 
            panelLeftBottom.BackColor = Color.White;
            panelLeftBottom.Controls.Add(btnPreview);
            panelLeftBottom.Controls.Add(btnPrint);
            panelLeftBottom.Controls.Add(btnExportExcel);
            panelLeftBottom.Controls.Add(btnPageSetup);
            panelLeftBottom.Controls.Add(btnClose);
            panelLeftBottom.Dock = DockStyle.Bottom;
            panelLeftBottom.Location = new Point(0, 760);
            panelLeftBottom.Name = "panelLeftBottom";
            panelLeftBottom.Padding = new Padding(12);
            panelLeftBottom.Size = new Size(620, 60);
            panelLeftBottom.TabIndex = 2;
            // 
            // btnPreview
            // 
            btnPreview.BackColor = Color.White;
            btnPreview.FlatStyle = FlatStyle.Flat;
            btnPreview.Font = new Font("Segoe UI", 10F);
            btnPreview.Location = new Point(12, 12);
            btnPreview.Name = "btnPreview";
            btnPreview.Size = new Size(110, 36);
            btnPreview.TabIndex = 0;
            btnPreview.Text = "Preview";
            btnPreview.UseVisualStyleBackColor = false;
            btnPreview.Click += btnPreview_Click;
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.FromArgb(0, 120, 215);
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPrint.ForeColor = Color.White;
            btnPrint.Location = new Point(130, 12);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(110, 36);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            // 
            // btnExportExcel
            // 
            btnExportExcel.BackColor = Color.White;
            btnExportExcel.FlatStyle = FlatStyle.Flat;
            btnExportExcel.Font = new Font("Segoe UI", 10F);
            btnExportExcel.Location = new Point(248, 12);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(130, 36);
            btnExportExcel.TabIndex = 2;
            btnExportExcel.Text = "Export Excel";
            btnExportExcel.UseVisualStyleBackColor = false;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // btnPageSetup
            // 
            btnPageSetup.BackColor = Color.White;
            btnPageSetup.FlatStyle = FlatStyle.Flat;
            btnPageSetup.Font = new Font("Segoe UI", 10F);
            btnPageSetup.Location = new Point(386, 12);
            btnPageSetup.Name = "btnPageSetup";
            btnPageSetup.Size = new Size(120, 36);
            btnPageSetup.TabIndex = 3;
            btnPageSetup.Text = "Page Setup";
            btnPageSetup.UseVisualStyleBackColor = false;
            btnPageSetup.Click += btnPageSetup_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(514, 12);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(110, 36);
            btnClose.TabIndex = 4;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // panelRightTop
            // 
            panelRightTop.BackColor = Color.White;
            panelRightTop.Controls.Add(lblPreview);
            panelRightTop.Dock = DockStyle.Top;
            panelRightTop.Location = new Point(0, 0);
            panelRightTop.Name = "panelRightTop";
            panelRightTop.Padding = new Padding(12);
            panelRightTop.Size = new Size(776, 42);
            panelRightTop.TabIndex = 0;
            // 
            // lblPreview
            // 
            lblPreview.AutoSize = true;
            lblPreview.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPreview.Location = new Point(12, 10);
            lblPreview.Name = "lblPreview";
            lblPreview.Size = new Size(66, 23);
            lblPreview.TabIndex = 0;
            lblPreview.Text = "Preview";
            // 
            // previewControl
            // 
            previewControl.Dock = DockStyle.Fill;
            previewControl.Location = new Point(0, 42);
            previewControl.Name = "previewControl";
            previewControl.Size = new Size(776, 778);
            previewControl.TabIndex = 1;
            // 
            // BarcodePrintForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(1400, 820);
            Controls.Add(splitContainer1);
            Name = "BarcodePrintForm";
            Text = "Print Barcode";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panelLeftTop.ResumeLayout(false);
            panelLeftTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            panelLeftBottom.ResumeLayout(false);
            panelRightTop.ResumeLayout(false);
            panelRightTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel panelLeftTop;
        private Label lblTemplate;
        private ComboBox cmbTemplate;
        private Label lblSearch;
        private TextBox txtSearch;
        private CheckBox chkShowName;
        private CheckBox chkShowPrice;
        private DataGridView dgvItems;
        private Panel panelLeftBottom;
        private Button btnPreview;
        private Button btnPrint;
        private Button btnExportExcel;
        private Button btnPageSetup;
        private Button btnClose;
        private Panel panelRightTop;
        private Label lblPreview;
        private PrintPreviewControl previewControl;
        private PageSetupDialog pageSetupDialog1;
        private PrintDialog printDialog1;
        private PrintPreviewDialog printPreviewDialog1;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewTextBoxColumn colBarcode;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colUnit;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colQtyLabels;
    }
}
