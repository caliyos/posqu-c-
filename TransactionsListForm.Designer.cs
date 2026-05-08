namespace POS_qu
{
    partial class TransactionsListForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnExportDetail;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.Label lblDetail;

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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            panelHeader = new Panel();
            btnClose = new Button();
            btnReturn = new Button();
            btnCancel = new Button();
            btnExportDetail = new Button();
            btnExport = new Button();
            btnExportPdf = new Button();
            btnPrintPreview = new Button();
            btnRefresh = new Button();
            cbStatus = new ComboBox();
            lblTitle = new Label();
            panelBody = new Panel();
            splitContainer = new SplitContainer();
            dgvTransactions = new DataGridView();
            dgvDetails = new DataGridView();
            lblDetail = new Label();
            panelHeader.SuspendLayout();
            panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnReturn);
            panelHeader.Controls.Add(btnCancel);
            panelHeader.Controls.Add(btnExportDetail);
            panelHeader.Controls.Add(btnExport);
            panelHeader.Controls.Add(btnExportPdf);
            panelHeader.Controls.Add(btnPrintPreview);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(cbStatus);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1400, 82);
            panelHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.White;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(1250, 20);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(130, 42);
            btnClose.TabIndex = 9;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // btnReturn
            // 
            btnReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReturn.BackColor = Color.DarkOrange;
            btnReturn.FlatAppearance.BorderSize = 0;
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.Font = new Font("Segoe UI", 10F);
            btnReturn.ForeColor = Color.White;
            btnReturn.Location = new Point(1100, 20);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(140, 42);
            btnReturn.TabIndex = 8;
            btnReturn.Text = "Retur Barang";
            btnReturn.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.BackColor = Color.IndianRed;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(950, 20);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(140, 42);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Batal";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnExportDetail
            // 
            btnExportDetail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportDetail.BackColor = Color.White;
            btnExportDetail.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnExportDetail.FlatStyle = FlatStyle.Flat;
            btnExportDetail.Font = new Font("Segoe UI", 10F);
            btnExportDetail.Location = new Point(780, 20);
            btnExportDetail.Name = "btnExportDetail";
            btnExportDetail.Size = new Size(160, 42);
            btnExportDetail.TabIndex = 6;
            btnExportDetail.Text = "Export + Detail";
            btnExportDetail.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.BackColor = Color.White;
            btnExport.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F);
            btnExport.Location = new Point(650, 20);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(120, 42);
            btnExport.TabIndex = 5;
            btnExport.Text = "Export CSV";
            btnExport.UseVisualStyleBackColor = false;
            // 
            // btnExportPdf
            // 
            btnExportPdf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportPdf.BackColor = Color.White;
            btnExportPdf.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnExportPdf.FlatStyle = FlatStyle.Flat;
            btnExportPdf.Font = new Font("Segoe UI", 10F);
            btnExportPdf.Location = new Point(520, 20);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(120, 42);
            btnExportPdf.TabIndex = 4;
            btnExportPdf.Text = "Download PDF";
            btnExportPdf.UseVisualStyleBackColor = false;
            // 
            // btnPrintPreview
            // 
            btnPrintPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPrintPreview.BackColor = Color.White;
            btnPrintPreview.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnPrintPreview.FlatStyle = FlatStyle.Flat;
            btnPrintPreview.Font = new Font("Segoe UI", 10F);
            btnPrintPreview.Location = new Point(380, 20);
            btnPrintPreview.Name = "btnPrintPreview";
            btnPrintPreview.Size = new Size(130, 42);
            btnPrintPreview.TabIndex = 3;
            btnPrintPreview.Text = "Preview / Print";
            btnPrintPreview.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.Location = new Point(240, 20);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 42);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // cbStatus
            // 
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.Font = new Font("Segoe UI", 10F);
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(20, 28);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(210, 36);
            cbStatus.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(20, -6);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(255, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Daftar Transaksi";
            // 
            // panelBody
            // 
            panelBody.Controls.Add(splitContainer);
            panelBody.Dock = DockStyle.Fill;
            panelBody.Location = new Point(0, 82);
            panelBody.Name = "panelBody";
            panelBody.Padding = new Padding(16);
            panelBody.Size = new Size(1400, 768);
            panelBody.TabIndex = 1;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(16, 16);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(dgvTransactions);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(dgvDetails);
            splitContainer.Panel2.Controls.Add(lblDetail);
            splitContainer.Size = new Size(1368, 736);
            splitContainer.SplitterDistance = 522;
            splitContainer.TabIndex = 0;
            // 
            // dgvTransactions
            // 
            dgvTransactions.BackgroundColor = Color.White;
            dgvTransactions.BorderStyle = BorderStyle.None;
            dgvTransactions.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTransactions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTransactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTransactions.ColumnHeadersHeight = 45;
            dgvTransactions.Dock = DockStyle.Fill;
            dgvTransactions.EnableHeadersVisualStyles = false;
            dgvTransactions.Location = new Point(0, 0);
            dgvTransactions.MultiSelect = false;
            dgvTransactions.Name = "dgvTransactions";
            dgvTransactions.ReadOnly = true;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.RowHeadersWidth = 62;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.Padding = new Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvTransactions.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvTransactions.RowTemplate.Height = 45;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.Size = new Size(1368, 522);
            dgvTransactions.TabIndex = 0;
            // 
            // dgvDetails
            // 
            dgvDetails.BackgroundColor = Color.White;
            dgvDetails.BorderStyle = BorderStyle.None;
            dgvDetails.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle3.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvDetails.ColumnHeadersHeight = 40;
            dgvDetails.Dock = DockStyle.Fill;
            dgvDetails.EnableHeadersVisualStyles = false;
            dgvDetails.Location = new Point(0, 50);
            dgvDetails.MultiSelect = false;
            dgvDetails.Name = "dgvDetails";
            dgvDetails.ReadOnly = true;
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.RowHeadersWidth = 62;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle4.Padding = new Padding(5);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvDetails.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvDetails.RowTemplate.Height = 42;
            dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.Size = new Size(1368, 160);
            dgvDetails.TabIndex = 1;
            // 
            // lblDetail
            // 
            lblDetail.AutoSize = true;
            lblDetail.Dock = DockStyle.Top;
            lblDetail.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblDetail.ForeColor = Color.FromArgb(51, 51, 51);
            lblDetail.Location = new Point(0, 0);
            lblDetail.Name = "lblDetail";
            lblDetail.Padding = new Padding(0, 10, 0, 10);
            lblDetail.Size = new Size(167, 50);
            lblDetail.TabIndex = 0;
            lblDetail.Text = "Detail Transaksi";
            // 
            // TransactionsListForm
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(1400, 850);
            Controls.Add(panelBody);
            Controls.Add(panelHeader);
            Font = new Font("Segoe UI", 10F);
            Name = "TransactionsListForm";
            Text = "Daftar Transaksi";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelBody.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            ResumeLayout(false);
        }
    }
}

