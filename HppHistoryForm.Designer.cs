namespace POS_qu
{
    partial class HppHistoryForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnRevalue;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabLayers;
        private System.Windows.Forms.TabPage tabStockCard;
        private System.Windows.Forms.Panel panelLayersTools;
        private System.Windows.Forms.Label lblLayersCari;
        private System.Windows.Forms.TextBox txtSearchLayers;
        private System.Windows.Forms.Button btnSearchLayers;
        private System.Windows.Forms.Button btnResetLayers;
        private System.Windows.Forms.Label lblLayersFrom;
        private System.Windows.Forms.DateTimePicker dtLayersFrom;
        private System.Windows.Forms.Label lblLayersTo;
        private System.Windows.Forms.DateTimePicker dtLayersTo;
        private System.Windows.Forms.ComboBox cmbPageSizeLayers;
        private System.Windows.Forms.Button btnPrevLayers;
        private System.Windows.Forms.Label lblPageLayers;
        private System.Windows.Forms.Button btnNextLayers;
        private System.Windows.Forms.Panel panelStockTools;
        private System.Windows.Forms.Label lblStockCari;
        private System.Windows.Forms.TextBox txtSearchStock;
        private System.Windows.Forms.Button btnSearchStock;
        private System.Windows.Forms.Button btnResetStock;
        private System.Windows.Forms.Label lblStockFrom;
        private System.Windows.Forms.DateTimePicker dtStockFrom;
        private System.Windows.Forms.Label lblStockTo;
        private System.Windows.Forms.DateTimePicker dtStockTo;
        private System.Windows.Forms.CheckBox chkShowAllocation;
        private System.Windows.Forms.ComboBox cmbPageSizeStock;
        private System.Windows.Forms.Button btnPrevStock;
        private System.Windows.Forms.Label lblPageStock;
        private System.Windows.Forms.Button btnNextStock;
        private System.Windows.Forms.DataGridView dgvLayers;
        private System.Windows.Forms.DataGridView dgvStockCard;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblSummary;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelHeader = new Panel();
            btnClose = new Button();
            btnRevalue = new Button();
            btnRefresh = new Button();
            cmbWarehouse = new ComboBox();
            lblWarehouse = new Label();
            lblMethod = new Label();
            lblItem = new Label();
            lblTitle = new Label();
            tabMain = new TabControl();
            tabLayers = new TabPage();
            panelLayersTools = new Panel();
            btnNextLayers = new Button();
            lblPageLayers = new Label();
            btnPrevLayers = new Button();
            cmbPageSizeLayers = new ComboBox();
            dtLayersTo = new DateTimePicker();
            lblLayersTo = new Label();
            dtLayersFrom = new DateTimePicker();
            lblLayersFrom = new Label();
            btnResetLayers = new Button();
            btnSearchLayers = new Button();
            txtSearchLayers = new TextBox();
            lblLayersCari = new Label();
            dgvLayers = new DataGridView();
            tabStockCard = new TabPage();
            panelStockTools = new Panel();
            btnNextStock = new Button();
            lblPageStock = new Label();
            btnPrevStock = new Button();
            cmbPageSizeStock = new ComboBox();
            chkShowAllocation = new CheckBox();
            dtStockTo = new DateTimePicker();
            lblStockTo = new Label();
            dtStockFrom = new DateTimePicker();
            lblStockFrom = new Label();
            btnResetStock = new Button();
            btnSearchStock = new Button();
            txtSearchStock = new TextBox();
            lblStockCari = new Label();
            dgvStockCard = new DataGridView();
            panelBottom = new Panel();
            lblSummary = new Label();
            panelHeader.SuspendLayout();
            tabMain.SuspendLayout();
            tabLayers.SuspendLayout();
            panelLayersTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLayers).BeginInit();
            tabStockCard.SuspendLayout();
            panelStockTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStockCard).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnRevalue);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(cmbWarehouse);
            panelHeader.Controls.Add(lblWarehouse);
            panelHeader.Controls.Add(lblMethod);
            panelHeader.Controls.Add(lblItem);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(2);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(983, 72);
            panelHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(689, 8);
            btnClose.Margin = new Padding(2);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(84, 28);
            btnClose.TabIndex = 7;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // btnRevalue
            // 
            btnRevalue.BackColor = Color.FromArgb(255, 193, 7);
            btnRevalue.FlatAppearance.BorderSize = 0;
            btnRevalue.FlatStyle = FlatStyle.Flat;
            btnRevalue.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRevalue.ForeColor = Color.Black;
            btnRevalue.Location = new Point(428, 36);
            btnRevalue.Margin = new Padding(2);
            btnRevalue.Name = "btnRevalue";
            btnRevalue.Size = new Size(155, 31);
            btnRevalue.TabIndex = 6;
            btnRevalue.Text = "Koreksi HPP (Layer)";
            btnRevalue.UseVisualStyleBackColor = false;
            btnRevalue.Visible = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(591, 10);
            btnRefresh.Margin = new Padding(2);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(84, 25);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new Font("Segoe UI", 10F);
            cmbWarehouse.FormattingEnabled = true;
            cmbWarehouse.Location = new Point(428, 10);
            cmbWarehouse.Margin = new Padding(2);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(155, 25);
            cmbWarehouse.TabIndex = 4;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Segoe UI", 10F);
            lblWarehouse.Location = new Point(364, 12);
            lblWarehouse.Margin = new Padding(2, 0, 2, 0);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(61, 19);
            lblWarehouse.TabIndex = 3;
            lblWarehouse.Text = "Gudang:";
            // 
            // lblMethod
            // 
            lblMethod.AutoSize = true;
            lblMethod.Font = new Font("Segoe UI", 10F);
            lblMethod.ForeColor = Color.FromArgb(80, 80, 80);
            lblMethod.Location = new Point(13, 48);
            lblMethod.Margin = new Padding(2, 0, 2, 0);
            lblMethod.Name = "lblMethod";
            lblMethod.Size = new Size(94, 19);
            lblMethod.TabIndex = 2;
            lblMethod.Text = "Method: FIFO";
            // 
            // lblItem
            // 
            lblItem.AutoSize = true;
            lblItem.Font = new Font("Segoe UI", 10F);
            lblItem.ForeColor = Color.FromArgb(80, 80, 80);
            lblItem.Location = new Point(13, 31);
            lblItem.Margin = new Padding(2, 0, 2, 0);
            lblItem.Name = "lblItem";
            lblItem.Size = new Size(50, 19);
            lblItem.TabIndex = 1;
            lblItem.Text = "Item: -";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(11, 7);
            lblTitle.Margin = new Padding(2, 0, 2, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(161, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "History HPP Item";
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabLayers);
            tabMain.Controls.Add(tabStockCard);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(0, 72);
            tabMain.Margin = new Padding(2);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(983, 378);
            tabMain.TabIndex = 1;
            // 
            // tabLayers
            // 
            tabLayers.Controls.Add(panelLayersTools);
            tabLayers.Controls.Add(dgvLayers);
            tabLayers.Location = new Point(4, 24);
            tabLayers.Margin = new Padding(2);
            tabLayers.Name = "tabLayers";
            tabLayers.Padding = new Padding(2);
            tabLayers.Size = new Size(975, 350);
            tabLayers.TabIndex = 0;
            tabLayers.Text = "HPP Layers";
            tabLayers.UseVisualStyleBackColor = true;
            // 
            // panelLayersTools
            // 
            panelLayersTools.BackColor = Color.White;
            panelLayersTools.Controls.Add(btnNextLayers);
            panelLayersTools.Controls.Add(lblPageLayers);
            panelLayersTools.Controls.Add(btnPrevLayers);
            panelLayersTools.Controls.Add(cmbPageSizeLayers);
            panelLayersTools.Controls.Add(dtLayersTo);
            panelLayersTools.Controls.Add(lblLayersTo);
            panelLayersTools.Controls.Add(dtLayersFrom);
            panelLayersTools.Controls.Add(lblLayersFrom);
            panelLayersTools.Controls.Add(btnResetLayers);
            panelLayersTools.Controls.Add(btnSearchLayers);
            panelLayersTools.Controls.Add(txtSearchLayers);
            panelLayersTools.Controls.Add(lblLayersCari);
            panelLayersTools.Dock = DockStyle.Top;
            panelLayersTools.Location = new Point(2, 2);
            panelLayersTools.Margin = new Padding(2);
            panelLayersTools.Name = "panelLayersTools";
            panelLayersTools.Padding = new Padding(8, 6, 8, 6);
            panelLayersTools.Size = new Size(971, 41);
            panelLayersTools.TabIndex = 1;
            // 
            // btnNextLayers
            // 
            btnNextLayers.BackColor = Color.White;
            btnNextLayers.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnNextLayers.FlatStyle = FlatStyle.Flat;
            btnNextLayers.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnNextLayers.Location = new Point(771, 9);
            btnNextLayers.Margin = new Padding(2);
            btnNextLayers.Name = "btnNextLayers";
            btnNextLayers.Size = new Size(49, 23);
            btnNextLayers.TabIndex = 11;
            btnNextLayers.Text = ">";
            btnNextLayers.UseVisualStyleBackColor = false;
            // 
            // lblPageLayers
            // 
            lblPageLayers.AutoSize = true;
            lblPageLayers.Font = new Font("Segoe UI", 10F);
            lblPageLayers.Location = new Point(843, 11);
            lblPageLayers.Margin = new Padding(2, 0, 2, 0);
            lblPageLayers.Name = "lblPageLayers";
            lblPageLayers.Size = new Size(39, 19);
            lblPageLayers.TabIndex = 10;
            lblPageLayers.Text = "Page";
            // 
            // btnPrevLayers
            // 
            btnPrevLayers.BackColor = Color.White;
            btnPrevLayers.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnPrevLayers.FlatStyle = FlatStyle.Flat;
            btnPrevLayers.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPrevLayers.Location = new Point(718, 8);
            btnPrevLayers.Margin = new Padding(2);
            btnPrevLayers.Name = "btnPrevLayers";
            btnPrevLayers.Size = new Size(49, 23);
            btnPrevLayers.TabIndex = 9;
            btnPrevLayers.Text = "<";
            btnPrevLayers.UseVisualStyleBackColor = false;
            // 
            // cmbPageSizeLayers
            // 
            cmbPageSizeLayers.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPageSizeLayers.Font = new Font("Segoe UI", 10F);
            cmbPageSizeLayers.FormattingEnabled = true;
            cmbPageSizeLayers.Items.AddRange(new object[] { "50", "100", "200", "500" });
            cmbPageSizeLayers.Location = new Point(651, 9);
            cmbPageSizeLayers.Margin = new Padding(2);
            cmbPageSizeLayers.Name = "cmbPageSizeLayers";
            cmbPageSizeLayers.Size = new Size(57, 25);
            cmbPageSizeLayers.TabIndex = 8;
            // 
            // dtLayersTo
            // 
            dtLayersTo.Checked = false;
            dtLayersTo.Font = new Font("Segoe UI", 10F);
            dtLayersTo.Format = DateTimePickerFormat.Short;
            dtLayersTo.Location = new Point(548, 8);
            dtLayersTo.Margin = new Padding(2);
            dtLayersTo.Name = "dtLayersTo";
            dtLayersTo.ShowCheckBox = true;
            dtLayersTo.Size = new Size(99, 25);
            dtLayersTo.TabIndex = 7;
            // 
            // lblLayersTo
            // 
            lblLayersTo.AutoSize = true;
            lblLayersTo.Font = new Font("Segoe UI", 10F);
            lblLayersTo.Location = new Point(484, 12);
            lblLayersTo.Margin = new Padding(2, 0, 2, 0);
            lblLayersTo.Name = "lblLayersTo";
            lblLayersTo.Size = new Size(56, 19);
            lblLayersTo.TabIndex = 6;
            lblLayersTo.Text = "Sampai:";
            // 
            // dtLayersFrom
            // 
            dtLayersFrom.Checked = false;
            dtLayersFrom.Font = new Font("Segoe UI", 10F);
            dtLayersFrom.Format = DateTimePickerFormat.Short;
            dtLayersFrom.Location = new Point(381, 10);
            dtLayersFrom.Margin = new Padding(2);
            dtLayersFrom.Name = "dtLayersFrom";
            dtLayersFrom.ShowCheckBox = true;
            dtLayersFrom.Size = new Size(99, 25);
            dtLayersFrom.TabIndex = 5;
            // 
            // lblLayersFrom
            // 
            lblLayersFrom.AutoSize = true;
            lblLayersFrom.Font = new Font("Segoe UI", 10F);
            lblLayersFrom.Location = new Point(346, 12);
            lblLayersFrom.Margin = new Padding(2, 0, 2, 0);
            lblLayersFrom.Name = "lblLayersFrom";
            lblLayersFrom.Size = new Size(37, 19);
            lblLayersFrom.TabIndex = 4;
            lblLayersFrom.Text = "Dari:";
            // 
            // btnResetLayers
            // 
            btnResetLayers.BackColor = Color.White;
            btnResetLayers.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnResetLayers.FlatStyle = FlatStyle.Flat;
            btnResetLayers.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnResetLayers.Location = new Point(272, 8);
            btnResetLayers.Margin = new Padding(2);
            btnResetLayers.Name = "btnResetLayers";
            btnResetLayers.Size = new Size(63, 27);
            btnResetLayers.TabIndex = 3;
            btnResetLayers.Text = "Reset";
            btnResetLayers.UseVisualStyleBackColor = false;
            // 
            // btnSearchLayers
            // 
            btnSearchLayers.BackColor = Color.White;
            btnSearchLayers.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnSearchLayers.FlatStyle = FlatStyle.Flat;
            btnSearchLayers.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSearchLayers.Location = new Point(204, 8);
            btnSearchLayers.Margin = new Padding(2);
            btnSearchLayers.Name = "btnSearchLayers";
            btnSearchLayers.Size = new Size(63, 27);
            btnSearchLayers.TabIndex = 2;
            btnSearchLayers.Text = "Cari";
            btnSearchLayers.UseVisualStyleBackColor = false;
            // 
            // txtSearchLayers
            // 
            txtSearchLayers.Font = new Font("Segoe UI", 10F);
            txtSearchLayers.Location = new Point(43, 10);
            txtSearchLayers.Margin = new Padding(2);
            txtSearchLayers.Name = "txtSearchLayers";
            txtSearchLayers.Size = new Size(155, 25);
            txtSearchLayers.TabIndex = 1;
            // 
            // lblLayersCari
            // 
            lblLayersCari.AutoSize = true;
            lblLayersCari.Font = new Font("Segoe UI", 10F);
            lblLayersCari.Location = new Point(8, 12);
            lblLayersCari.Margin = new Padding(2, 0, 2, 0);
            lblLayersCari.Name = "lblLayersCari";
            lblLayersCari.Size = new Size(36, 19);
            lblLayersCari.TabIndex = 0;
            lblLayersCari.Text = "Cari:";
            // 
            // dgvLayers
            // 
            dgvLayers.AllowUserToAddRows = false;
            dgvLayers.AllowUserToDeleteRows = false;
            dgvLayers.BackgroundColor = Color.White;
            dgvLayers.BorderStyle = BorderStyle.None;
            dgvLayers.Location = new Point(2, 41);
            dgvLayers.Margin = new Padding(2);
            dgvLayers.Name = "dgvLayers";
            dgvLayers.ReadOnly = true;
            dgvLayers.RowHeadersVisible = false;
            dgvLayers.RowHeadersWidth = 62;
            dgvLayers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLayers.Size = new Size(971, 307);
            dgvLayers.TabIndex = 0;
            // 
            // tabStockCard
            // 
            tabStockCard.Controls.Add(panelStockTools);
            tabStockCard.Controls.Add(dgvStockCard);
            tabStockCard.Location = new Point(4, 24);
            tabStockCard.Margin = new Padding(2);
            tabStockCard.Name = "tabStockCard";
            tabStockCard.Padding = new Padding(2);
            tabStockCard.Size = new Size(975, 350);
            tabStockCard.TabIndex = 1;
            tabStockCard.Text = "Kartu Stock";
            tabStockCard.UseVisualStyleBackColor = true;
            // 
            // panelStockTools
            // 
            panelStockTools.BackColor = Color.White;
            panelStockTools.Controls.Add(btnNextStock);
            panelStockTools.Controls.Add(lblPageStock);
            panelStockTools.Controls.Add(btnPrevStock);
            panelStockTools.Controls.Add(cmbPageSizeStock);
            panelStockTools.Controls.Add(chkShowAllocation);
            panelStockTools.Controls.Add(dtStockTo);
            panelStockTools.Controls.Add(lblStockTo);
            panelStockTools.Controls.Add(dtStockFrom);
            panelStockTools.Controls.Add(lblStockFrom);
            panelStockTools.Controls.Add(btnResetStock);
            panelStockTools.Controls.Add(btnSearchStock);
            panelStockTools.Controls.Add(txtSearchStock);
            panelStockTools.Controls.Add(lblStockCari);
            panelStockTools.Dock = DockStyle.Top;
            panelStockTools.Location = new Point(2, 2);
            panelStockTools.Margin = new Padding(2);
            panelStockTools.Name = "panelStockTools";
            panelStockTools.Padding = new Padding(8, 6, 8, 6);
            panelStockTools.Size = new Size(971, 48);
            panelStockTools.TabIndex = 1;
            // 
            // btnNextStock
            // 
            btnNextStock.BackColor = Color.White;
            btnNextStock.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnNextStock.FlatStyle = FlatStyle.Flat;
            btnNextStock.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnNextStock.Location = new Point(828, 8);
            btnNextStock.Margin = new Padding(2);
            btnNextStock.Name = "btnNextStock";
            btnNextStock.Size = new Size(33, 33);
            btnNextStock.TabIndex = 12;
            btnNextStock.Text = ">";
            btnNextStock.UseVisualStyleBackColor = false;
            // 
            // lblPageStock
            // 
            lblPageStock.AutoSize = true;
            lblPageStock.Font = new Font("Segoe UI", 10F);
            lblPageStock.Location = new Point(886, 12);
            lblPageStock.Margin = new Padding(2, 0, 2, 0);
            lblPageStock.Name = "lblPageStock";
            lblPageStock.Size = new Size(39, 19);
            lblPageStock.TabIndex = 11;
            lblPageStock.Text = "Page";
            // 
            // btnPrevStock
            // 
            btnPrevStock.BackColor = Color.White;
            btnPrevStock.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnPrevStock.FlatStyle = FlatStyle.Flat;
            btnPrevStock.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPrevStock.Location = new Point(791, 8);
            btnPrevStock.Margin = new Padding(2);
            btnPrevStock.Name = "btnPrevStock";
            btnPrevStock.Size = new Size(33, 33);
            btnPrevStock.TabIndex = 10;
            btnPrevStock.Text = "<";
            btnPrevStock.UseVisualStyleBackColor = false;
            // 
            // cmbPageSizeStock
            // 
            cmbPageSizeStock.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPageSizeStock.Font = new Font("Segoe UI", 10F);
            cmbPageSizeStock.FormattingEnabled = true;
            cmbPageSizeStock.Items.AddRange(new object[] { "50", "100", "200", "500" });
            cmbPageSizeStock.Location = new Point(728, 10);
            cmbPageSizeStock.Margin = new Padding(2);
            cmbPageSizeStock.Name = "cmbPageSizeStock";
            cmbPageSizeStock.Size = new Size(57, 25);
            cmbPageSizeStock.TabIndex = 9;
            // 
            // chkShowAllocation
            // 
            chkShowAllocation.AutoSize = true;
            chkShowAllocation.Checked = true;
            chkShowAllocation.CheckState = CheckState.Checked;
            chkShowAllocation.Font = new Font("Segoe UI", 10F);
            chkShowAllocation.Location = new Point(644, 11);
            chkShowAllocation.Margin = new Padding(2);
            chkShowAllocation.Name = "chkShowAllocation";
            chkShowAllocation.Size = new Size(88, 23);
            chkShowAllocation.TabIndex = 8;
            chkShowAllocation.Text = "Allocation";
            chkShowAllocation.UseVisualStyleBackColor = true;
            // 
            // dtStockTo
            // 
            dtStockTo.Checked = false;
            dtStockTo.Font = new Font("Segoe UI", 10F);
            dtStockTo.Format = DateTimePickerFormat.Short;
            dtStockTo.Location = new Point(538, 10);
            dtStockTo.Margin = new Padding(2);
            dtStockTo.Name = "dtStockTo";
            dtStockTo.ShowCheckBox = true;
            dtStockTo.Size = new Size(99, 25);
            dtStockTo.TabIndex = 7;
            // 
            // lblStockTo
            // 
            lblStockTo.AutoSize = true;
            lblStockTo.Font = new Font("Segoe UI", 10F);
            lblStockTo.Location = new Point(486, 12);
            lblStockTo.Margin = new Padding(2, 0, 2, 0);
            lblStockTo.Name = "lblStockTo";
            lblStockTo.Size = new Size(56, 19);
            lblStockTo.TabIndex = 6;
            lblStockTo.Text = "Sampai:";
            // 
            // dtStockFrom
            // 
            dtStockFrom.Checked = false;
            dtStockFrom.Font = new Font("Segoe UI", 10F);
            dtStockFrom.Format = DateTimePickerFormat.Short;
            dtStockFrom.Location = new Point(381, 10);
            dtStockFrom.Margin = new Padding(2);
            dtStockFrom.Name = "dtStockFrom";
            dtStockFrom.ShowCheckBox = true;
            dtStockFrom.Size = new Size(99, 25);
            dtStockFrom.TabIndex = 5;
            // 
            // lblStockFrom
            // 
            lblStockFrom.AutoSize = true;
            lblStockFrom.Font = new Font("Segoe UI", 10F);
            lblStockFrom.Location = new Point(346, 12);
            lblStockFrom.Margin = new Padding(2, 0, 2, 0);
            lblStockFrom.Name = "lblStockFrom";
            lblStockFrom.Size = new Size(37, 19);
            lblStockFrom.TabIndex = 4;
            lblStockFrom.Text = "Dari:";
            // 
            // btnResetStock
            // 
            btnResetStock.BackColor = Color.White;
            btnResetStock.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnResetStock.FlatStyle = FlatStyle.Flat;
            btnResetStock.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnResetStock.Location = new Point(272, 8);
            btnResetStock.Margin = new Padding(2);
            btnResetStock.Name = "btnResetStock";
            btnResetStock.Size = new Size(63, 33);
            btnResetStock.TabIndex = 3;
            btnResetStock.Text = "Reset";
            btnResetStock.UseVisualStyleBackColor = false;
            // 
            // btnSearchStock
            // 
            btnSearchStock.BackColor = Color.White;
            btnSearchStock.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnSearchStock.FlatStyle = FlatStyle.Flat;
            btnSearchStock.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSearchStock.Location = new Point(204, 8);
            btnSearchStock.Margin = new Padding(2);
            btnSearchStock.Name = "btnSearchStock";
            btnSearchStock.Size = new Size(63, 33);
            btnSearchStock.TabIndex = 2;
            btnSearchStock.Text = "Cari";
            btnSearchStock.UseVisualStyleBackColor = false;
            // 
            // txtSearchStock
            // 
            txtSearchStock.Font = new Font("Segoe UI", 10F);
            txtSearchStock.Location = new Point(43, 10);
            txtSearchStock.Margin = new Padding(2);
            txtSearchStock.Name = "txtSearchStock";
            txtSearchStock.Size = new Size(155, 25);
            txtSearchStock.TabIndex = 1;
            // 
            // lblStockCari
            // 
            lblStockCari.AutoSize = true;
            lblStockCari.Font = new Font("Segoe UI", 10F);
            lblStockCari.Location = new Point(8, 12);
            lblStockCari.Margin = new Padding(2, 0, 2, 0);
            lblStockCari.Name = "lblStockCari";
            lblStockCari.Size = new Size(36, 19);
            lblStockCari.TabIndex = 0;
            lblStockCari.Text = "Cari:";
            // 
            // dgvStockCard
            // 
            dgvStockCard.AllowUserToAddRows = false;
            dgvStockCard.AllowUserToDeleteRows = false;
            dgvStockCard.BackgroundColor = Color.White;
            dgvStockCard.BorderStyle = BorderStyle.None;
            dgvStockCard.Location = new Point(2, 54);
            dgvStockCard.Margin = new Padding(2);
            dgvStockCard.Name = "dgvStockCard";
            dgvStockCard.ReadOnly = true;
            dgvStockCard.RowHeadersVisible = false;
            dgvStockCard.RowHeadersWidth = 62;
            dgvStockCard.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStockCard.Size = new Size(971, 294);
            dgvStockCard.TabIndex = 0;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.White;
            panelBottom.Controls.Add(lblSummary);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 450);
            panelBottom.Margin = new Padding(2);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(11, 6, 11, 6);
            panelBottom.Size = new Size(983, 36);
            panelBottom.TabIndex = 2;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new Font("Segoe UI", 10F);
            lblSummary.ForeColor = Color.FromArgb(51, 51, 51);
            lblSummary.Location = new Point(11, 11);
            lblSummary.Margin = new Padding(2, 0, 2, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(119, 19);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Total: Qty 0 | Rp 0";
            // 
            // HppHistoryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(983, 486);
            Controls.Add(tabMain);
            Controls.Add(panelBottom);
            Controls.Add(panelHeader);
            Margin = new Padding(2);
            Name = "HppHistoryForm";
            Text = "History HPP";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            tabMain.ResumeLayout(false);
            tabLayers.ResumeLayout(false);
            panelLayersTools.ResumeLayout(false);
            panelLayersTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLayers).EndInit();
            tabStockCard.ResumeLayout(false);
            panelStockTools.ResumeLayout(false);
            panelStockTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStockCard).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}
