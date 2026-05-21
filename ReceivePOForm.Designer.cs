namespace POS_qu
{
    partial class ReceivePOForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            button1 = new Button();
            cmbMode = new ComboBox();
            lblMode = new Label();
            lblTitle = new Label();
            pnlLeft = new Panel();
            pnlPagination = new Panel();
            cmbPageSize = new ComboBox();
            lblPageNumber = new Label();
            lblPagingInfo = new Label();
            btnFirst = new Button();
            btnPrev = new Button();
            btnNext = new Button();
            btnLast = new Button();
            btnAddItem = new Button();
            dgvItems = new DataGridView();
            txtSearch = new TextBox();
            lblSearch = new Label();
            pnlRight = new Panel();
            dgvOrderDetails = new DataGridView();
            groupBoxPO = new GroupBox();
            cmbStatus = new ComboBox();
            lblStatus = new Label();
            cmbWarehouse = new ComboBox();
            lblWarehouse = new Label();
            dtpOrderDate = new DateTimePicker();
            lblOrderDate = new Label();
            cmbSupplier = new ComboBox();
            lblSupplier = new Label();
            numTaxRate = new NumericUpDown();
            lblTaxRate = new Label();
            cmbTaxMode = new ComboBox();
            lblTaxMode = new Label();
            chkManualPurchaseNo = new CheckBox();
            txtPurchaseNo = new TextBox();
            lblPurchaseNo = new Label();
            pnlFooter = new Panel();
            label2 = new Label();
            txtNote = new RichTextBox();
            lblSubtotal = new Label();
            lblTaxAmount = new Label();
            lblTotalAmount = new Label();
            btnSave = new Button();
            pnlHeader.SuspendLayout();
            pnlLeft.SuspendLayout();
            pnlPagination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrderDetails).BeginInit();
            groupBoxPO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTaxRate).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(button1);
            pnlHeader.Controls.Add(cmbMode);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(2, 2, 2, 2);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1120, 42);
            pnlHeader.TabIndex = 0;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.BackColor = Color.White;
            button1.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10F);
            button1.Location = new Point(945, 9);
            button1.Margin = new Padding(2, 2, 2, 2);
            button1.Name = "button1";
            button1.Size = new Size(154, 24);
            button1.TabIndex = 1;
            button1.Text = "Daftar Pesanan";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // cmbMode
            // 
            cmbMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMode.Font = new Font("Segoe UI", 10F);
            cmbMode.Location = new Point(623, 12);
            cmbMode.Margin = new Padding(2, 2, 2, 2);
            cmbMode.Name = "cmbMode";
            cmbMode.Size = new Size(309, 25);
            cmbMode.TabIndex = 3;
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblMode.AutoSize = true;
            lblMode.Font = new Font("Segoe UI", 10F);
            lblMode.ForeColor = Color.FromArgb(51, 51, 51);
            lblMode.Location = new Point(574, 14);
            lblMode.Margin = new Padding(2, 0, 2, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(48, 19);
            lblMode.TabIndex = 2;
            lblMode.Text = "Mode:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(14, 9);
            lblTitle.Margin = new Padding(2, 0, 2, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(330, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Pembelian / Penerimaan Barang";
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.White;
            pnlLeft.Controls.Add(pnlPagination);
            pnlLeft.Controls.Add(btnAddItem);
            pnlLeft.Controls.Add(dgvItems);
            pnlLeft.Controls.Add(txtSearch);
            pnlLeft.Controls.Add(lblSearch);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 42);
            pnlLeft.Margin = new Padding(2, 2, 2, 2);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new Padding(14, 12, 14, 12);
            pnlLeft.Size = new Size(490, 498);
            pnlLeft.TabIndex = 1;
            // 
            // pnlPagination
            // 
            pnlPagination.Controls.Add(cmbPageSize);
            pnlPagination.Controls.Add(lblPageNumber);
            pnlPagination.Controls.Add(lblPagingInfo);
            pnlPagination.Controls.Add(btnFirst);
            pnlPagination.Controls.Add(btnPrev);
            pnlPagination.Controls.Add(btnNext);
            pnlPagination.Controls.Add(btnLast);
            pnlPagination.Location = new Point(14, 408);
            pnlPagination.Margin = new Padding(2, 2, 2, 2);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.Size = new Size(462, 60);
            pnlPagination.TabIndex = 3;
            // 
            // cmbPageSize
            // 
            cmbPageSize.Location = new Point(0, 6);
            cmbPageSize.Margin = new Padding(2, 2, 2, 2);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(57, 23);
            cmbPageSize.TabIndex = 0;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(63, 9);
            lblPageNumber.Margin = new Padding(2, 0, 2, 0);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(0, 15);
            lblPageNumber.TabIndex = 1;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(0, 36);
            lblPagingInfo.Margin = new Padding(2, 0, 2, 0);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(0, 15);
            lblPagingInfo.TabIndex = 2;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(210, 6);
            btnFirst.Margin = new Padding(2, 2, 2, 2);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(56, 21);
            btnFirst.TabIndex = 3;
            btnFirst.Text = "First";
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(273, 6);
            btnPrev.Margin = new Padding(2, 2, 2, 2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(56, 21);
            btnPrev.TabIndex = 4;
            btnPrev.Text = "Prev";
            // 
            // btnNext
            // 
            btnNext.Location = new Point(336, 6);
            btnNext.Margin = new Padding(2, 2, 2, 2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(56, 21);
            btnNext.TabIndex = 5;
            btnNext.Text = "Next";
            // 
            // btnLast
            // 
            btnLast.Location = new Point(399, 6);
            btnLast.Margin = new Padding(2, 2, 2, 2);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(56, 21);
            btnLast.TabIndex = 6;
            btnLast.Text = "Last";
            // 
            // btnAddItem
            // 
            btnAddItem.BackColor = Color.FromArgb(0, 120, 215);
            btnAddItem.FlatAppearance.BorderSize = 0;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.Font = new Font("Segoe UI Semibold", 10F);
            btnAddItem.ForeColor = Color.White;
            btnAddItem.Location = new Point(14, 372);
            btnAddItem.Margin = new Padding(2, 2, 2, 2);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(462, 27);
            btnAddItem.TabIndex = 2;
            btnAddItem.Text = "Tambahkan ke PO >";
            btnAddItem.UseVisualStyleBackColor = false;
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvItems.ColumnHeadersHeight = 45;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.Location = new Point(14, 60);
            dgvItems.Margin = new Padding(2, 2, 2, 2);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.Padding = new Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvItems.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvItems.RowTemplate.Height = 45;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(462, 300);
            dgvItems.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(14, 30);
            txtSearch.Margin = new Padding(2, 2, 2, 2);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(463, 25);
            txtSearch.TabIndex = 0;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 10F);
            lblSearch.Location = new Point(14, 12);
            lblSearch.Margin = new Padding(2, 0, 2, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(84, 19);
            lblSearch.TabIndex = 4;
            lblSearch.Text = "Cari Produk:";
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.FromArgb(245, 246, 250);
            pnlRight.Controls.Add(dgvOrderDetails);
            pnlRight.Controls.Add(groupBoxPO);
            pnlRight.Controls.Add(pnlFooter);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(490, 42);
            pnlRight.Margin = new Padding(2, 2, 2, 2);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(14, 12, 14, 12);
            pnlRight.Size = new Size(630, 498);
            pnlRight.TabIndex = 2;
            // 
            // dgvOrderDetails
            // 
            dgvOrderDetails.AllowUserToAddRows = false;
            dgvOrderDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrderDetails.BackgroundColor = Color.White;
            dgvOrderDetails.BorderStyle = BorderStyle.None;
            dgvOrderDetails.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvOrderDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle3.Font = new Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvOrderDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvOrderDetails.ColumnHeadersHeight = 45;
            dgvOrderDetails.EnableHeadersVisualStyles = false;
            dgvOrderDetails.Location = new Point(14, 168);
            dgvOrderDetails.Margin = new Padding(2, 2, 2, 2);
            dgvOrderDetails.Name = "dgvOrderDetails";
            dgvOrderDetails.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle4.Padding = new Padding(5);
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvOrderDetails.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvOrderDetails.RowTemplate.Height = 45;
            dgvOrderDetails.Size = new Size(602, 204);
            dgvOrderDetails.TabIndex = 1;
            // 
            // groupBoxPO
            // 
            groupBoxPO.BackColor = Color.White;
            groupBoxPO.Controls.Add(cmbStatus);
            groupBoxPO.Controls.Add(lblStatus);
            groupBoxPO.Controls.Add(cmbWarehouse);
            groupBoxPO.Controls.Add(lblWarehouse);
            groupBoxPO.Controls.Add(dtpOrderDate);
            groupBoxPO.Controls.Add(lblOrderDate);
            groupBoxPO.Controls.Add(cmbSupplier);
            groupBoxPO.Controls.Add(lblSupplier);
            groupBoxPO.Controls.Add(numTaxRate);
            groupBoxPO.Controls.Add(lblTaxRate);
            groupBoxPO.Controls.Add(cmbTaxMode);
            groupBoxPO.Controls.Add(lblTaxMode);
            groupBoxPO.Controls.Add(chkManualPurchaseNo);
            groupBoxPO.Controls.Add(txtPurchaseNo);
            groupBoxPO.Controls.Add(lblPurchaseNo);
            groupBoxPO.Dock = DockStyle.Top;
            groupBoxPO.Font = new Font("Segoe UI Semibold", 10F);
            groupBoxPO.Location = new Point(14, 12);
            groupBoxPO.Margin = new Padding(2, 2, 2, 2);
            groupBoxPO.Name = "groupBoxPO";
            groupBoxPO.Padding = new Padding(2, 2, 2, 2);
            groupBoxPO.Size = new Size(602, 144);
            groupBoxPO.TabIndex = 0;
            groupBoxPO.TabStop = false;
            groupBoxPO.Text = "Informasi Pesanan";
            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Location = new Point(245, 84);
            cmbStatus.Margin = new Padding(2, 2, 2, 2);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(141, 25);
            cmbStatus.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F);
            lblStatus.Location = new Point(245, 66);
            lblStatus.Margin = new Padding(2, 0, 2, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(73, 19);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status PO:";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Location = new Point(14, 84);
            cmbWarehouse.Margin = new Padding(2, 2, 2, 2);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(211, 25);
            cmbWarehouse.TabIndex = 2;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Segoe UI", 10F);
            lblWarehouse.Location = new Point(14, 66);
            lblWarehouse.Margin = new Padding(2, 0, 2, 0);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(121, 19);
            lblWarehouse.TabIndex = 3;
            lblWarehouse.Text = "Gudang Penerima:";
            // 
            // dtpOrderDate
            // 
            dtpOrderDate.Location = new Point(245, 42);
            dtpOrderDate.Margin = new Padding(2, 2, 2, 2);
            dtpOrderDate.Name = "dtpOrderDate";
            dtpOrderDate.Size = new Size(141, 25);
            dtpOrderDate.TabIndex = 4;
            // 
            // lblOrderDate
            // 
            lblOrderDate.AutoSize = true;
            lblOrderDate.Font = new Font("Segoe UI", 10F);
            lblOrderDate.Location = new Point(245, 24);
            lblOrderDate.Margin = new Padding(2, 0, 2, 0);
            lblOrderDate.Name = "lblOrderDate";
            lblOrderDate.Size = new Size(98, 19);
            lblOrderDate.TabIndex = 5;
            lblOrderDate.Text = "Tanggal Order:";
            // 
            // cmbSupplier
            // 
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplier.Location = new Point(14, 42);
            cmbSupplier.Margin = new Padding(2, 2, 2, 2);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(211, 25);
            cmbSupplier.TabIndex = 6;
            // 
            // lblSupplier
            // 
            lblSupplier.AutoSize = true;
            lblSupplier.Font = new Font("Segoe UI", 10F);
            lblSupplier.Location = new Point(14, 24);
            lblSupplier.Margin = new Padding(2, 0, 2, 0);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(61, 19);
            lblSupplier.TabIndex = 7;
            lblSupplier.Text = "Supplier:";
            // 
            // numTaxRate
            // 
            numTaxRate.DecimalPlaces = 2;
            numTaxRate.Font = new Font("Segoe UI", 10F);
            numTaxRate.Location = new Point(518, 102);
            numTaxRate.Margin = new Padding(2, 2, 2, 2);
            numTaxRate.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numTaxRate.Name = "numTaxRate";
            numTaxRate.Size = new Size(63, 25);
            numTaxRate.TabIndex = 8;
            // 
            // lblTaxRate
            // 
            lblTaxRate.AutoSize = true;
            lblTaxRate.Font = new Font("Segoe UI", 10F);
            lblTaxRate.Location = new Point(518, 84);
            lblTaxRate.Margin = new Padding(2, 0, 2, 0);
            lblTaxRate.Name = "lblTaxRate";
            lblTaxRate.Size = new Size(54, 19);
            lblTaxRate.TabIndex = 9;
            lblTaxRate.Text = "Rate %:";
            // 
            // cmbTaxMode
            // 
            cmbTaxMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTaxMode.Font = new Font("Segoe UI", 10F);
            cmbTaxMode.Location = new Point(406, 102);
            cmbTaxMode.Margin = new Padding(2, 2, 2, 2);
            cmbTaxMode.Name = "cmbTaxMode";
            cmbTaxMode.Size = new Size(106, 25);
            cmbTaxMode.TabIndex = 10;
            // 
            // lblTaxMode
            // 
            lblTaxMode.AutoSize = true;
            lblTaxMode.Font = new Font("Segoe UI", 10F);
            lblTaxMode.Location = new Point(406, 84);
            lblTaxMode.Margin = new Padding(2, 0, 2, 0);
            lblTaxMode.Name = "lblTaxMode";
            lblTaxMode.Size = new Size(38, 19);
            lblTaxMode.TabIndex = 11;
            lblTaxMode.Text = "PPN:";
            // 
            // chkManualPurchaseNo
            // 
            chkManualPurchaseNo.AutoSize = true;
            chkManualPurchaseNo.Font = new Font("Segoe UI", 10F);
            chkManualPurchaseNo.Location = new Point(406, 66);
            chkManualPurchaseNo.Margin = new Padding(2, 2, 2, 2);
            chkManualPurchaseNo.Name = "chkManualPurchaseNo";
            chkManualPurchaseNo.Size = new Size(120, 23);
            chkManualPurchaseNo.TabIndex = 12;
            chkManualPurchaseNo.Text = "Nomor manual";
            // 
            // txtPurchaseNo
            // 
            txtPurchaseNo.Location = new Point(406, 42);
            txtPurchaseNo.Margin = new Padding(2, 2, 2, 2);
            txtPurchaseNo.Name = "txtPurchaseNo";
            txtPurchaseNo.Size = new Size(176, 25);
            txtPurchaseNo.TabIndex = 13;
            // 
            // lblPurchaseNo
            // 
            lblPurchaseNo.AutoSize = true;
            lblPurchaseNo.Font = new Font("Segoe UI", 10F);
            lblPurchaseNo.Location = new Point(406, 24);
            lblPurchaseNo.Margin = new Padding(2, 0, 2, 0);
            lblPurchaseNo.Name = "lblPurchaseNo";
            lblPurchaseNo.Size = new Size(96, 19);
            lblPurchaseNo.TabIndex = 14;
            lblPurchaseNo.Text = "No Pembelian:";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.White;
            pnlFooter.Controls.Add(label2);
            pnlFooter.Controls.Add(txtNote);
            pnlFooter.Controls.Add(lblSubtotal);
            pnlFooter.Controls.Add(lblTaxAmount);
            pnlFooter.Controls.Add(lblTotalAmount);
            pnlFooter.Controls.Add(btnSave);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(14, 384);
            pnlFooter.Margin = new Padding(2, 2, 2, 2);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(602, 102);
            pnlFooter.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(14, 9);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(60, 19);
            label2.TabIndex = 0;
            label2.Text = "Catatan:";
            // 
            // txtNote
            // 
            txtNote.BorderStyle = BorderStyle.FixedSingle;
            txtNote.Location = new Point(14, 27);
            txtNote.Margin = new Padding(2, 2, 2, 2);
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(281, 62);
            txtNote.TabIndex = 1;
            txtNote.Text = "";
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Font = new Font("Segoe UI", 10F);
            lblSubtotal.Location = new Point(315, 12);
            lblSubtotal.Margin = new Padding(2, 0, 2, 0);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(95, 19);
            lblSubtotal.TabIndex = 2;
            lblSubtotal.Text = "Subtotal: Rp 0";
            // 
            // lblTaxAmount
            // 
            lblTaxAmount.AutoSize = true;
            lblTaxAmount.Font = new Font("Segoe UI", 10F);
            lblTaxAmount.Location = new Point(315, 48);
            lblTaxAmount.Margin = new Padding(2, 0, 2, 0);
            lblTaxAmount.Name = "lblTaxAmount";
            lblTaxAmount.Size = new Size(70, 19);
            lblTaxAmount.TabIndex = 3;
            lblTaxAmount.Text = "PPN: Rp 0";
            // 
            // lblTotalAmount
            // 
            lblTotalAmount.AutoSize = true;
            lblTotalAmount.Font = new Font("Microsoft Sans Serif", 16F);
            lblTotalAmount.Location = new Point(315, 27);
            lblTotalAmount.Margin = new Padding(2, 0, 2, 0);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(117, 26);
            lblTotalAmount.TabIndex = 4;
            lblTotalAmount.Text = "Total: Rp 0";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(0, 150, 136);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 12F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(455, 60);
            btnSave.Margin = new Padding(2, 2, 2, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(133, 30);
            btnSave.TabIndex = 5;
            btnSave.Text = "Terima & Update Stok";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click_1;
            // 
            // ReceivePOForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1120, 540);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Controls.Add(pnlHeader);
            Margin = new Padding(2, 2, 2, 2);
            Name = "ReceivePOForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Penerimaan Barang";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            pnlPagination.ResumeLayout(false);
            pnlPagination.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOrderDetails).EndInit();
            groupBoxPO.ResumeLayout(false);
            groupBoxPO.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTaxRate).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblMode;
        private ComboBox cmbMode;
        private Button button1;

        private Panel pnlLeft;
        private Label lblSearch;
        private TextBox txtSearch;
        private DataGridView dgvItems;
        private Button btnAddItem;
        private Panel pnlPagination;
        private ComboBox cmbPageSize;
        private Label lblPageNumber;
        private Label lblPagingInfo;
        private Button btnFirst;
        private Button btnPrev;
        private Button btnNext;
        private Button btnLast;

        private Panel pnlRight;
        private GroupBox groupBoxPO;
        private Label lblSupplier;
        private ComboBox cmbSupplier;
        private Label lblOrderDate;
        private DateTimePicker dtpOrderDate;
        private Label lblWarehouse;
        private ComboBox cmbWarehouse;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private Label lblPurchaseNo;
        private TextBox txtPurchaseNo;
        private CheckBox chkManualPurchaseNo;
        private Label lblTaxMode;
        private ComboBox cmbTaxMode;
        private Label lblTaxRate;
        private NumericUpDown numTaxRate;
        
        private DataGridView dgvOrderDetails;
        
        private Panel pnlFooter;
        private Label label2;
        private RichTextBox txtNote;
        private Label lblSubtotal;
        private Label lblTaxAmount;
        private Label lblTotalAmount;
        private Button btnSave;
    }
}
