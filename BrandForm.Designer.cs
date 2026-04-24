namespace POS_qu
{
    partial class BrandForm
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
            panelHeader = new Panel();
            btnClose = new Button();
            lblTitle = new Label();
            panelContent = new Panel();
            lblInfo = new Label();
            dgvBrands = new DataGridView();
            panelHeader.SuspendLayout();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBrands).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(938, 70);
            panelHeader.TabIndex = 100;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.White;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(778, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(140, 40);
            btnClose.TabIndex = 1;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(96, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Merk";
            // 
            // panelContent
            // 
            panelContent.Controls.Add(lblInfo);
            panelContent.Controls.Add(dgvBrands);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 70);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(12);
            panelContent.Size = new Size(938, 452);
            panelContent.TabIndex = 101;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Dock = DockStyle.Top;
            lblInfo.Location = new Point(12, 12);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(809, 25);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Ketik pada baris kosong di bawah untuk menambah merk baru. Perubahan akan otomatis tersimpan.";
            // 
            // dgvBrands
            // 
            dgvBrands.BackgroundColor = Color.White;
            dgvBrands.BorderStyle = BorderStyle.None;
            dgvBrands.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBrands.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvBrands.ColumnHeadersHeight = 45;
            dgvBrands.Dock = DockStyle.Fill;
            dgvBrands.Location = new Point(12, 12);
            dgvBrands.Name = "dgvBrands";
            dgvBrands.RowHeadersVisible = false;
            dgvBrands.RowHeadersWidth = 62;
            dgvBrands.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBrands.Size = new Size(914, 428);
            dgvBrands.TabIndex = 0;
            dgvBrands.RowValidated += dgvBrands_RowValidated;
            dgvBrands.UserDeletingRow += dgvBrands_UserDeletingRow;
            // 
            // BrandForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(938, 522);
            Controls.Add(panelContent);
            Controls.Add(panelHeader);
            Name = "BrandForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Master Merk (Brand)";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBrands).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.DataGridView dgvBrands;
        private System.Windows.Forms.Label lblInfo;
    }
}
