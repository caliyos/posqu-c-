namespace POS_qu
{
    partial class PromotionListForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvList;

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
            panelHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            btnNew = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            panelBody = new System.Windows.Forms.Panel();
            dgvList = new System.Windows.Forms.DataGridView();
            txtSearch = new System.Windows.Forms.TextBox();
            lblSearch = new System.Windows.Forms.Label();
            panelHeader.SuspendLayout();
            panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvList).BeginInit();
            SuspendLayout();
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(btnNew);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(1200, 70);
            panelHeader.TabIndex = 0;
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(191, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Daftar Program";
            btnNew.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnNew.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            btnNew.FlatAppearance.BorderSize = 0;
            btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnNew.ForeColor = System.Drawing.Color.White;
            btnNew.Location = new System.Drawing.Point(720, 15);
            btnNew.Name = "btnNew";
            btnNew.Size = new System.Drawing.Size(150, 40);
            btnNew.TabIndex = 1;
            btnNew.Text = "Buat Program";
            btnNew.UseVisualStyleBackColor = false;
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.BackColor = System.Drawing.Color.White;
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnRefresh.Location = new System.Drawing.Point(880, 15);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(140, 40);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClose.BackColor = System.Drawing.Color.White;
            btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnClose.Location = new System.Drawing.Point(1030, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(150, 40);
            btnClose.TabIndex = 3;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            panelBody.Controls.Add(dgvList);
            panelBody.Controls.Add(txtSearch);
            panelBody.Controls.Add(lblSearch);
            panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            panelBody.Location = new System.Drawing.Point(0, 70);
            panelBody.Name = "panelBody";
            panelBody.Padding = new System.Windows.Forms.Padding(16);
            panelBody.Size = new System.Drawing.Size(1200, 680);
            panelBody.TabIndex = 1;
            dgvList.AllowUserToAddRows = false;
            dgvList.AllowUserToDeleteRows = false;
            dgvList.BackgroundColor = System.Drawing.Color.White;
            dgvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvList.ColumnHeadersHeight = 45;
            dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvList.EnableHeadersVisualStyles = false;
            dgvList.Location = new System.Drawing.Point(16, 86);
            dgvList.MultiSelect = false;
            dgvList.Name = "dgvList";
            dgvList.ReadOnly = true;
            dgvList.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dgvList.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvList.RowTemplate.Height = 45;
            dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvList.Size = new System.Drawing.Size(1168, 578);
            dgvList.TabIndex = 2;
            txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearch.Location = new System.Drawing.Point(110, 46);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(1074, 30);
            txtSearch.TabIndex = 1;
            lblSearch.AutoSize = true;
            lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSearch.Location = new System.Drawing.Point(16, 49);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new System.Drawing.Size(87, 23);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Cari data:";
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(1200, 750);
            Controls.Add(panelBody);
            Controls.Add(panelHeader);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            MinimumSize = new System.Drawing.Size(1000, 700);
            Name = "PromotionListForm";
            Text = "Daftar Program";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelBody.ResumeLayout(false);
            panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvList).EndInit();
            ResumeLayout(false);
        }
    }
}

