namespace POS_qu
{
    partial class StockOpnameForm
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
            panelTop = new Panel();
            lblDate = new Label();
            dtpOpnameDate = new DateTimePicker();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblNo = new Label();
            rdoNoAuto = new RadioButton();
            rdoNoManual = new RadioButton();
            txtOpnameNo = new TextBox();
            btnPrintBase = new Button();
            btnPrintAll = new Button();
            tabMain = new TabControl();
            tabSatuan = new TabPage();
            panelSatuanTop = new Panel();
            btnAddItem = new Button();
            btnRemoveItem = new Button();
            btnSaveSatuan = new Button();
            dgvSatuan = new DataGridView();
            tabMassal = new TabPage();
            panelMassalTop = new Panel();
            btnExportTemplate = new Button();
            btnImportExcel = new Button();
            btnSaveMassal = new Button();
            chkMassalAllUnit = new CheckBox();
            splitMassal = new SplitContainer();
            dgvMassalPreview = new DataGridView();
            txtImportLog = new TextBox();
            btnClose = new Button();
            panelTop.SuspendLayout();
            tabMain.SuspendLayout();
            tabSatuan.SuspendLayout();
            panelSatuanTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSatuan).BeginInit();
            tabMassal.SuspendLayout();
            panelMassalTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMassal).BeginInit();
            splitMassal.Panel1.SuspendLayout();
            splitMassal.Panel2.SuspendLayout();
            splitMassal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMassalPreview).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblDate);
            panelTop.Controls.Add(dtpOpnameDate);
            panelTop.Controls.Add(lblWarehouse);
            panelTop.Controls.Add(cmbWarehouse);
            panelTop.Controls.Add(lblNo);
            panelTop.Controls.Add(rdoNoAuto);
            panelTop.Controls.Add(rdoNoManual);
            panelTop.Controls.Add(txtOpnameNo);
            panelTop.Controls.Add(btnPrintBase);
            panelTop.Controls.Add(btnPrintAll);
            panelTop.Controls.Add(btnClose);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(12);
            panelTop.Size = new Size(1280, 86);
            panelTop.TabIndex = 0;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 10F);
            lblDate.Location = new Point(12, 12);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(63, 23);
            lblDate.TabIndex = 0;
            lblDate.Text = "Tanggal";
            // 
            // dtpOpnameDate
            // 
            dtpOpnameDate.Font = new Font("Segoe UI", 10F);
            dtpOpnameDate.Format = DateTimePickerFormat.Short;
            dtpOpnameDate.Location = new Point(12, 38);
            dtpOpnameDate.Name = "dtpOpnameDate";
            dtpOpnameDate.Size = new Size(140, 30);
            dtpOpnameDate.TabIndex = 1;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Segoe UI", 10F);
            lblWarehouse.Location = new Point(168, 12);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(64, 23);
            lblWarehouse.TabIndex = 2;
            lblWarehouse.Text = "Gudang";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new Font("Segoe UI", 10F);
            cmbWarehouse.FormattingEnabled = true;
            cmbWarehouse.Location = new Point(168, 38);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(250, 31);
            cmbWarehouse.TabIndex = 3;
            // 
            // lblNo
            // 
            lblNo.AutoSize = true;
            lblNo.Font = new Font("Segoe UI", 10F);
            lblNo.Location = new Point(434, 12);
            lblNo.Name = "lblNo";
            lblNo.Size = new Size(89, 23);
            lblNo.TabIndex = 4;
            lblNo.Text = "No. Opname";
            // 
            // rdoNoAuto
            // 
            rdoNoAuto.AutoSize = true;
            rdoNoAuto.Font = new Font("Segoe UI", 10F);
            rdoNoAuto.Location = new Point(434, 38);
            rdoNoAuto.Name = "rdoNoAuto";
            rdoNoAuto.Size = new Size(62, 27);
            rdoNoAuto.TabIndex = 5;
            rdoNoAuto.TabStop = true;
            rdoNoAuto.Text = "Auto";
            rdoNoAuto.UseVisualStyleBackColor = true;
            // 
            // rdoNoManual
            // 
            rdoNoManual.AutoSize = true;
            rdoNoManual.Font = new Font("Segoe UI", 10F);
            rdoNoManual.Location = new Point(504, 38);
            rdoNoManual.Name = "rdoNoManual";
            rdoNoManual.Size = new Size(78, 27);
            rdoNoManual.TabIndex = 6;
            rdoNoManual.TabStop = true;
            rdoNoManual.Text = "Manual";
            rdoNoManual.UseVisualStyleBackColor = true;
            // 
            // txtOpnameNo
            // 
            txtOpnameNo.Font = new Font("Segoe UI", 10F);
            txtOpnameNo.Location = new Point(590, 38);
            txtOpnameNo.Name = "txtOpnameNo";
            txtOpnameNo.Size = new Size(210, 30);
            txtOpnameNo.TabIndex = 7;
            // 
            // btnPrintBase
            // 
            btnPrintBase.BackColor = Color.White;
            btnPrintBase.FlatStyle = FlatStyle.Flat;
            btnPrintBase.Font = new Font("Segoe UI", 10F);
            btnPrintBase.Location = new Point(818, 36);
            btnPrintBase.Name = "btnPrintBase";
            btnPrintBase.Size = new Size(170, 34);
            btnPrintBase.TabIndex = 8;
            btnPrintBase.Text = "Print List (Base)";
            btnPrintBase.UseVisualStyleBackColor = false;
            btnPrintBase.Click += btnPrintBase_Click;
            // 
            // btnPrintAll
            // 
            btnPrintAll.BackColor = Color.White;
            btnPrintAll.FlatStyle = FlatStyle.Flat;
            btnPrintAll.Font = new Font("Segoe UI", 10F);
            btnPrintAll.Location = new Point(996, 36);
            btnPrintAll.Name = "btnPrintAll";
            btnPrintAll.Size = new Size(190, 34);
            btnPrintAll.TabIndex = 9;
            btnPrintAll.Text = "Print List (All Unit)";
            btnPrintAll.UseVisualStyleBackColor = false;
            btnPrintAll.Click += btnPrintAll_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(1196, 36);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(72, 34);
            btnClose.TabIndex = 10;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabSatuan);
            tabMain.Controls.Add(tabMassal);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(0, 86);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1280, 634);
            tabMain.TabIndex = 1;
            // 
            // tabSatuan
            // 
            tabSatuan.Controls.Add(dgvSatuan);
            tabSatuan.Controls.Add(panelSatuanTop);
            tabSatuan.Location = new Point(4, 29);
            tabSatuan.Name = "tabSatuan";
            tabSatuan.Padding = new Padding(3);
            tabSatuan.Size = new Size(1272, 601);
            tabSatuan.TabIndex = 0;
            tabSatuan.Text = "Opname Satuan (1-10 Item)";
            tabSatuan.UseVisualStyleBackColor = true;
            // 
            // panelSatuanTop
            // 
            panelSatuanTop.BackColor = Color.White;
            panelSatuanTop.Controls.Add(btnAddItem);
            panelSatuanTop.Controls.Add(btnRemoveItem);
            panelSatuanTop.Controls.Add(btnSaveSatuan);
            panelSatuanTop.Dock = DockStyle.Top;
            panelSatuanTop.Location = new Point(3, 3);
            panelSatuanTop.Name = "panelSatuanTop";
            panelSatuanTop.Padding = new Padding(12);
            panelSatuanTop.Size = new Size(1266, 58);
            panelSatuanTop.TabIndex = 0;
            // 
            // btnAddItem
            // 
            btnAddItem.BackColor = Color.White;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.Font = new Font("Segoe UI", 10F);
            btnAddItem.Location = new Point(12, 12);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(130, 34);
            btnAddItem.TabIndex = 0;
            btnAddItem.Text = "Tambah Item";
            btnAddItem.UseVisualStyleBackColor = false;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.BackColor = Color.White;
            btnRemoveItem.FlatStyle = FlatStyle.Flat;
            btnRemoveItem.Font = new Font("Segoe UI", 10F);
            btnRemoveItem.Location = new Point(150, 12);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(130, 34);
            btnRemoveItem.TabIndex = 1;
            btnRemoveItem.Text = "Hapus Item";
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // btnSaveSatuan
            // 
            btnSaveSatuan.BackColor = Color.FromArgb(0, 120, 215);
            btnSaveSatuan.FlatStyle = FlatStyle.Flat;
            btnSaveSatuan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveSatuan.ForeColor = Color.White;
            btnSaveSatuan.Location = new Point(288, 12);
            btnSaveSatuan.Name = "btnSaveSatuan";
            btnSaveSatuan.Size = new Size(130, 34);
            btnSaveSatuan.TabIndex = 2;
            btnSaveSatuan.Text = "Simpan";
            btnSaveSatuan.UseVisualStyleBackColor = false;
            btnSaveSatuan.Click += btnSaveSatuan_Click;
            // 
            // dgvSatuan
            // 
            dgvSatuan.AllowUserToAddRows = false;
            dgvSatuan.BackgroundColor = Color.White;
            dgvSatuan.BorderStyle = BorderStyle.None;
            dgvSatuan.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSatuan.ColumnHeadersHeight = 42;
            dgvSatuan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSatuan.Dock = DockStyle.Fill;
            dgvSatuan.EnableHeadersVisualStyles = false;
            dgvSatuan.Location = new Point(3, 61);
            dgvSatuan.Name = "dgvSatuan";
            dgvSatuan.RowHeadersVisible = false;
            dgvSatuan.RowTemplate.Height = 40;
            dgvSatuan.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvSatuan.Size = new Size(1266, 537);
            dgvSatuan.TabIndex = 1;
            // 
            // tabMassal
            // 
            tabMassal.Controls.Add(splitMassal);
            tabMassal.Controls.Add(panelMassalTop);
            tabMassal.Location = new Point(4, 29);
            tabMassal.Name = "tabMassal";
            tabMassal.Padding = new Padding(3);
            tabMassal.Size = new Size(1272, 601);
            tabMassal.TabIndex = 1;
            tabMassal.Text = "Opname Massal (Excel)";
            tabMassal.UseVisualStyleBackColor = true;
            // 
            // panelMassalTop
            // 
            panelMassalTop.BackColor = Color.White;
            panelMassalTop.Controls.Add(chkMassalAllUnit);
            panelMassalTop.Controls.Add(btnExportTemplate);
            panelMassalTop.Controls.Add(btnImportExcel);
            panelMassalTop.Controls.Add(btnSaveMassal);
            panelMassalTop.Dock = DockStyle.Top;
            panelMassalTop.Location = new Point(3, 3);
            panelMassalTop.Name = "panelMassalTop";
            panelMassalTop.Padding = new Padding(12);
            panelMassalTop.Size = new Size(1266, 58);
            panelMassalTop.TabIndex = 0;
            // 
            // btnExportTemplate
            // 
            btnExportTemplate.BackColor = Color.White;
            btnExportTemplate.FlatStyle = FlatStyle.Flat;
            btnExportTemplate.Font = new Font("Segoe UI", 10F);
            btnExportTemplate.Location = new Point(12, 12);
            btnExportTemplate.Name = "btnExportTemplate";
            btnExportTemplate.Size = new Size(140, 34);
            btnExportTemplate.TabIndex = 0;
            btnExportTemplate.Text = "Export Excel";
            btnExportTemplate.UseVisualStyleBackColor = false;
            btnExportTemplate.Click += btnExportTemplate_Click;
            // 
            // btnImportExcel
            // 
            btnImportExcel.BackColor = Color.White;
            btnImportExcel.FlatStyle = FlatStyle.Flat;
            btnImportExcel.Font = new Font("Segoe UI", 10F);
            btnImportExcel.Location = new Point(160, 12);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(140, 34);
            btnImportExcel.TabIndex = 1;
            btnImportExcel.Text = "Import Excel";
            btnImportExcel.UseVisualStyleBackColor = false;
            btnImportExcel.Click += btnImportExcel_Click;
            // 
            // btnSaveMassal
            // 
            btnSaveMassal.BackColor = Color.FromArgb(0, 120, 215);
            btnSaveMassal.FlatStyle = FlatStyle.Flat;
            btnSaveMassal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveMassal.ForeColor = Color.White;
            btnSaveMassal.Location = new Point(308, 12);
            btnSaveMassal.Name = "btnSaveMassal";
            btnSaveMassal.Size = new Size(140, 34);
            btnSaveMassal.TabIndex = 2;
            btnSaveMassal.Text = "Simpan";
            btnSaveMassal.UseVisualStyleBackColor = false;
            btnSaveMassal.Click += btnSaveMassal_Click;
            // 
            // chkMassalAllUnit
            // 
            chkMassalAllUnit.AutoSize = true;
            chkMassalAllUnit.Font = new Font("Segoe UI", 10F);
            chkMassalAllUnit.Location = new Point(466, 16);
            chkMassalAllUnit.Name = "chkMassalAllUnit";
            chkMassalAllUnit.Size = new Size(186, 27);
            chkMassalAllUnit.TabIndex = 3;
            chkMassalAllUnit.Text = "All Unit / Multi Variant";
            chkMassalAllUnit.UseVisualStyleBackColor = true;
            // 
            // splitMassal
            // 
            splitMassal.Dock = DockStyle.Fill;
            splitMassal.Location = new Point(3, 61);
            splitMassal.Name = "splitMassal";
            splitMassal.Orientation = Orientation.Horizontal;
            // 
            // splitMassal.Panel1
            // 
            splitMassal.Panel1.Controls.Add(dgvMassalPreview);
            // 
            // splitMassal.Panel2
            // 
            splitMassal.Panel2.Controls.Add(txtImportLog);
            splitMassal.Size = new Size(1266, 537);
            splitMassal.SplitterDistance = 360;
            splitMassal.TabIndex = 1;
            // 
            // dgvMassalPreview
            // 
            dgvMassalPreview.AllowUserToAddRows = false;
            dgvMassalPreview.BackgroundColor = Color.White;
            dgvMassalPreview.BorderStyle = BorderStyle.None;
            dgvMassalPreview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMassalPreview.ColumnHeadersHeight = 42;
            dgvMassalPreview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvMassalPreview.Dock = DockStyle.Fill;
            dgvMassalPreview.EnableHeadersVisualStyles = false;
            dgvMassalPreview.Location = new Point(0, 0);
            dgvMassalPreview.Name = "dgvMassalPreview";
            dgvMassalPreview.ReadOnly = true;
            dgvMassalPreview.RowHeadersVisible = false;
            dgvMassalPreview.RowTemplate.Height = 40;
            dgvMassalPreview.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvMassalPreview.Size = new Size(1266, 360);
            dgvMassalPreview.TabIndex = 0;
            // 
            // txtImportLog
            // 
            txtImportLog.BackColor = Color.White;
            txtImportLog.Dock = DockStyle.Fill;
            txtImportLog.Font = new Font("Consolas", 9F);
            txtImportLog.Location = new Point(0, 0);
            txtImportLog.Multiline = true;
            txtImportLog.Name = "txtImportLog";
            txtImportLog.ReadOnly = true;
            txtImportLog.ScrollBars = ScrollBars.Vertical;
            txtImportLog.Size = new Size(1266, 173);
            txtImportLog.TabIndex = 0;
            // 
            // StockOpnameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(1280, 720);
            Controls.Add(tabMain);
            Controls.Add(panelTop);
            Name = "StockOpnameForm";
            Text = "Stock Opname";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            tabMain.ResumeLayout(false);
            tabSatuan.ResumeLayout(false);
            panelSatuanTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSatuan).EndInit();
            tabMassal.ResumeLayout(false);
            panelMassalTop.ResumeLayout(false);
            panelMassalTop.PerformLayout();
            splitMassal.Panel1.ResumeLayout(false);
            splitMassal.Panel2.ResumeLayout(false);
            splitMassal.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitMassal).EndInit();
            splitMassal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMassalPreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Label lblDate;
        private DateTimePicker dtpOpnameDate;
        private Label lblWarehouse;
        private ComboBox cmbWarehouse;
        private Label lblNo;
        private RadioButton rdoNoAuto;
        private RadioButton rdoNoManual;
        private TextBox txtOpnameNo;
        private Button btnPrintBase;
        private Button btnPrintAll;
        private Button btnClose;
        private TabControl tabMain;
        private TabPage tabSatuan;
        private Panel panelSatuanTop;
        private Button btnAddItem;
        private Button btnRemoveItem;
        private Button btnSaveSatuan;
        private DataGridView dgvSatuan;
        private TabPage tabMassal;
        private Panel panelMassalTop;
        private Button btnExportTemplate;
        private Button btnImportExcel;
        private Button btnSaveMassal;
        private CheckBox chkMassalAllUnit;
        private SplitContainer splitMassal;
        private DataGridView dgvMassalPreview;
        private TextBox txtImportLog;
    }
}

