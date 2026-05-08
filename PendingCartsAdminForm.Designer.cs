namespace POS_qu
{
    partial class PendingCartsAdminForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvCarts;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.DataGridView dgvItems;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            panelHeader = new System.Windows.Forms.Panel();
            btnClose = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            lblTitle = new System.Windows.Forms.Label();
            splitContainer = new System.Windows.Forms.SplitContainer();
            dgvCarts = new System.Windows.Forms.DataGridView();
            panelDetail = new System.Windows.Forms.Panel();
            dgvItems = new System.Windows.Forms.DataGridView();
            lblDetail = new System.Windows.Forms.Label();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCarts).BeginInit();
            panelDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(1400, 70);
            panelHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClose.BackColor = System.Drawing.Color.White;
            btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnClose.Location = new System.Drawing.Point(1250, 20);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(130, 36);
            btnClose.TabIndex = 2;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.White;
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnRefresh.Location = new System.Drawing.Point(20, 20);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(120, 36);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(160, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(416, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Pending (Admin) — Belum Checkout";
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(0, 70);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            splitContainer.Panel1.Controls.Add(dgvCarts);
            splitContainer.Panel2.Controls.Add(panelDetail);
            splitContainer.Panel1MinSize = 220;
            splitContainer.Panel2MinSize = 260;
            splitContainer.Size = new System.Drawing.Size(1400, 780);
            splitContainer.TabIndex = 1;
            // 
            // dgvCarts
            // 
            dgvCarts.AllowUserToAddRows = false;
            dgvCarts.BackgroundColor = System.Drawing.Color.White;
            dgvCarts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvCarts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCarts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvCarts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCarts.ColumnHeadersHeight = 45;
            dgvCarts.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvCarts.EnableHeadersVisualStyles = false;
            dgvCarts.Location = new System.Drawing.Point(0, 0);
            dgvCarts.MultiSelect = false;
            dgvCarts.Name = "dgvCarts";
            dgvCarts.ReadOnly = true;
            dgvCarts.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dgvCarts.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvCarts.RowTemplate.Height = 45;
            dgvCarts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvCarts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvCarts.TabIndex = 0;
            // 
            // panelDetail
            // 
            panelDetail.BackColor = System.Drawing.Color.White;
            panelDetail.Controls.Add(dgvItems);
            panelDetail.Controls.Add(lblDetail);
            panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            panelDetail.Location = new System.Drawing.Point(0, 0);
            panelDetail.Name = "panelDetail";
            panelDetail.Size = new System.Drawing.Size(1400, 516);
            panelDetail.TabIndex = 0;
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.BackgroundColor = System.Drawing.Color.White;
            dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvItems.ColumnHeadersHeight = 40;
            dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.Location = new System.Drawing.Point(0, 45);
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dgvItems.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvItems.RowTemplate.Height = 42;
            dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.TabIndex = 1;
            // 
            // lblDetail
            // 
            lblDetail.AutoSize = true;
            lblDetail.Dock = System.Windows.Forms.DockStyle.Top;
            lblDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            lblDetail.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblDetail.Location = new System.Drawing.Point(0, 0);
            lblDetail.Name = "lblDetail";
            lblDetail.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            lblDetail.Size = new System.Drawing.Size(145, 45);
            lblDetail.TabIndex = 0;
            lblDetail.Text = "Detail Keranjang";
            // 
            // PendingCartsAdminForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(1400, 850);
            Controls.Add(splitContainer);
            Controls.Add(panelHeader);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            MinimumSize = new System.Drawing.Size(1000, 700);
            Name = "PendingCartsAdminForm";
            Text = "Pending (Admin)";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCarts).EndInit();
            panelDetail.ResumeLayout(false);
            panelDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
        }
    }
}

