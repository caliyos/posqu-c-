namespace POS_qu
{
    partial class PurchaseOrderListForm
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
            pnlHeader = new Panel();
            btnPO = new Button();
            lblTitle = new Label();
            pnlFilter = new Panel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblFilterStatus = new Label();
            cmbFilterStatus = new ComboBox();
            btnChangeStatus = new Button();
            btnCetakPO = new Button();
            dgvPO = new DataGridView();
            pnlFooter = new Panel();
            btnViewDetail = new Button();
            btnRefresh = new Button();
            btnPrint = new Button();
            pnlHeader.SuspendLayout();
            pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPO).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(btnPO);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(2, 2, 2, 2);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1402, 56);
            pnlHeader.TabIndex = 0;
            // 
            // btnPO
            // 
            btnPO.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPO.BackColor = Color.FromArgb(0, 120, 215);
            btnPO.FlatAppearance.BorderSize = 0;
            btnPO.FlatStyle = FlatStyle.Flat;
            btnPO.Font = new Font("Segoe UI Semibold", 10F);
            btnPO.ForeColor = Color.White;
            btnPO.Location = new Point(1202, 12);
            btnPO.Margin = new Padding(2, 2, 2, 2);
            btnPO.Name = "btnPO";
            btnPO.Size = new Size(176, 32);
            btnPO.TabIndex = 1;
            btnPO.Text = "+ Buat Pesanan";
            btnPO.UseVisualStyleBackColor = false;
            btnPO.Click += btnPO_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(16, 12);
            lblTitle.Margin = new Padding(2, 0, 2, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(336, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Daftar Pesanan Pembelian";
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.FromArgb(245, 246, 250);
            pnlFilter.Controls.Add(lblSearch);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(lblFilterStatus);
            pnlFilter.Controls.Add(cmbFilterStatus);
            pnlFilter.Controls.Add(btnChangeStatus);
            pnlFilter.Controls.Add(btnCetakPO);
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Location = new Point(0, 56);
            pnlFilter.Margin = new Padding(2, 2, 2, 2);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(1402, 64);
            pnlFilter.TabIndex = 1;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 10F);
            lblSearch.Location = new Point(16, 20);
            lblSearch.Margin = new Padding(2, 0, 2, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(112, 23);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Cari Pesanan:";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(112, 20);
            txtSearch.Margin = new Padding(2, 2, 2, 2);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(201, 30);
            txtSearch.TabIndex = 2;
            // 
            // lblFilterStatus
            // 
            lblFilterStatus.AutoSize = true;
            lblFilterStatus.Font = new Font("Segoe UI", 10F);
            lblFilterStatus.Location = new Point(328, 20);
            lblFilterStatus.Margin = new Padding(2, 0, 2, 0);
            lblFilterStatus.Name = "lblFilterStatus";
            lblFilterStatus.Size = new Size(60, 23);
            lblFilterStatus.TabIndex = 3;
            lblFilterStatus.Text = "Status:";
            // 
            // cmbFilterStatus
            // 
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterStatus.Font = new Font("Segoe UI", 10F);
            cmbFilterStatus.Location = new Point(384, 20);
            cmbFilterStatus.Margin = new Padding(2, 2, 2, 2);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new Size(161, 31);
            cmbFilterStatus.TabIndex = 3;
            // 
            // btnChangeStatus
            // 
            btnChangeStatus.BackColor = Color.White;
            btnChangeStatus.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnChangeStatus.FlatStyle = FlatStyle.Flat;
            btnChangeStatus.Font = new Font("Segoe UI", 10F);
            btnChangeStatus.Location = new Point(560, 20);
            btnChangeStatus.Margin = new Padding(2, 2, 2, 2);
            btnChangeStatus.Name = "btnChangeStatus";
            btnChangeStatus.Size = new Size(128, 28);
            btnChangeStatus.TabIndex = 4;
            btnChangeStatus.Text = "Update Status";
            btnChangeStatus.UseVisualStyleBackColor = false;
            // 
            // btnCetakPO
            // 
            btnCetakPO.BackColor = Color.White;
            btnCetakPO.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnCetakPO.FlatStyle = FlatStyle.Flat;
            btnCetakPO.Font = new Font("Segoe UI", 10F);
            btnCetakPO.Location = new Point(704, 20);
            btnCetakPO.Margin = new Padding(2, 2, 2, 2);
            btnCetakPO.Name = "btnCetakPO";
            btnCetakPO.Size = new Size(96, 28);
            btnCetakPO.TabIndex = 5;
            btnCetakPO.Text = "Cetak PO";
            btnCetakPO.UseVisualStyleBackColor = false;
            btnCetakPO.Click += btnCetakPO_Click;
            // 
            // dgvPO
            // 
            dgvPO.AllowUserToAddRows = false;
            dgvPO.AllowUserToDeleteRows = false;
            dgvPO.BackgroundColor = Color.White;
            dgvPO.BorderStyle = BorderStyle.None;
            dgvPO.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPO.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvPO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvPO.ColumnHeadersHeight = 45;
            dgvPO.Dock = DockStyle.Fill;
            dgvPO.EnableHeadersVisualStyles = false;
            dgvPO.Location = new Point(0, 120);
            dgvPO.Margin = new Padding(2, 2, 2, 2);
            dgvPO.Name = "dgvPO";
            dgvPO.ReadOnly = true;
            dgvPO.RowHeadersVisible = false;
            dgvPO.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.Padding = new Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvPO.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvPO.RowTemplate.Height = 45;
            dgvPO.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPO.Size = new Size(1402, 440);
            dgvPO.TabIndex = 6;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.White;
            pnlFooter.Controls.Add(btnViewDetail);
            pnlFooter.Controls.Add(btnRefresh);
            pnlFooter.Controls.Add(btnPrint);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 560);
            pnlFooter.Margin = new Padding(2, 2, 2, 2);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1402, 56);
            pnlFooter.TabIndex = 7;
            // 
            // btnViewDetail
            // 
            btnViewDetail.BackColor = Color.White;
            btnViewDetail.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnViewDetail.FlatStyle = FlatStyle.Flat;
            btnViewDetail.Font = new Font("Segoe UI", 10F);
            btnViewDetail.Location = new Point(16, 12);
            btnViewDetail.Margin = new Padding(2, 2, 2, 2);
            btnViewDetail.Name = "btnViewDetail";
            btnViewDetail.Size = new Size(112, 32);
            btnViewDetail.TabIndex = 8;
            btnViewDetail.Text = "Lihat Detail";
            btnViewDetail.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.Location = new Point(144, 12);
            btnRefresh.Margin = new Padding(2, 2, 2, 2);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(96, 32);
            btnRefresh.TabIndex = 9;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.White;
            btnPrint.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Font = new Font("Segoe UI", 10F);
            btnPrint.Location = new Point(256, 12);
            btnPrint.Margin = new Padding(2, 2, 2, 2);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(96, 32);
            btnPrint.TabIndex = 10;
            btnPrint.Text = "Print List";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            // 
            // PurchaseOrderListForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1402, 616);
            Controls.Add(dgvPO);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilter);
            Controls.Add(pnlHeader);
            Margin = new Padding(2, 2, 2, 2);
            Name = "PurchaseOrderListForm";
            Text = "Daftar Pesanan Pembelian";
            WindowState = FormWindowState.Maximized;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPO).EndInit();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Panel pnlFilter;
        private Label lblSearch;
        private Label lblFilterStatus;
        private Panel pnlFooter;
        private DataGridView dgvPO;
        private TextBox txtSearch;
        private ComboBox cmbFilterStatus;
        private Button btnViewDetail;
        private Button btnRefresh;
        private Button btnPrint;
        private Button btnPO;
        private Button btnChangeStatus;
        private Button btnCetakPO;
    }
}
