namespace POS_qu
{
    partial class InventoryAdjustmentListForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvHeader;
        private System.Windows.Forms.DataGridView dgvDetail;

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
            panelTop = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblSearch = new System.Windows.Forms.Label();
            txtSearch = new System.Windows.Forms.TextBox();
            btnAdd = new System.Windows.Forms.Button();
            btnPrint = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            dgvHeader = new System.Windows.Forms.DataGridView();
            dgvDetail = new System.Windows.Forms.DataGridView();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHeader).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetail).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(btnClose);
            panelTop.Controls.Add(btnRefresh);
            panelTop.Controls.Add(btnPrint);
            panelTop.Controls.Add(btnAdd);
            panelTop.Controls.Add(txtSearch);
            panelTop.Controls.Add(lblSearch);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new System.Windows.Forms.Padding(12);
            panelTop.Size = new System.Drawing.Size(1182, 70);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(12, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(154, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Daftar Adjust";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new System.Drawing.Point(15, 42);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new System.Drawing.Size(52, 20);
            lblSearch.TabIndex = 1;
            lblSearch.Text = "Cari :";
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.Location = new System.Drawing.Point(70, 39);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(520, 27);
            txtSearch.TabIndex = 2;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAdd.Location = new System.Drawing.Point(600, 37);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(120, 30);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Tambah";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPrint.Location = new System.Drawing.Point(730, 37);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(120, 30);
            btnPrint.TabIndex = 4;
            btnPrint.Text = "Cetak Faktur";
            btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.Location = new System.Drawing.Point(910, 37);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(120, 30);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClose.Location = new System.Drawing.Point(1040, 37);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(120, 30);
            btnClose.TabIndex = 6;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 70);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dgvHeader);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dgvDetail);
            splitContainer1.Size = new System.Drawing.Size(1182, 583);
            splitContainer1.SplitterDistance = 300;
            splitContainer1.TabIndex = 1;
            // 
            // dgvHeader
            // 
            dgvHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvHeader.Location = new System.Drawing.Point(0, 0);
            dgvHeader.Name = "dgvHeader";
            dgvHeader.RowHeadersWidth = 51;
            dgvHeader.Size = new System.Drawing.Size(1182, 300);
            dgvHeader.TabIndex = 0;
            // 
            // dgvDetail
            // 
            dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvDetail.Location = new System.Drawing.Point(0, 0);
            dgvDetail.Name = "dgvDetail";
            dgvDetail.RowHeadersWidth = 51;
            dgvDetail.Size = new System.Drawing.Size(1182, 279);
            dgvDetail.TabIndex = 0;
            // 
            // InventoryAdjustmentListForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1182, 653);
            Controls.Add(splitContainer1);
            Controls.Add(panelTop);
            Name = "InventoryAdjustmentListForm";
            Text = "InventoryAdjustmentListForm";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHeader).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetail).EndInit();
            ResumeLayout(false);
        }
    }
}
