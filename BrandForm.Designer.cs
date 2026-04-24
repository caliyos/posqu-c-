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
            panelHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            btnClose = new System.Windows.Forms.Button();
            panelContent = new System.Windows.Forms.Panel();
            dgvBrands = new System.Windows.Forms.DataGridView();
            lblInfo = new System.Windows.Forms.Label();
            panelHeader.SuspendLayout();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBrands).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(800, 70);
            panelHeader.TabIndex = 100;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(81, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Merk";
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClose.BackColor = System.Drawing.Color.White;
            btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnClose.Location = new System.Drawing.Point(640, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(140, 40);
            btnClose.TabIndex = 1;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(lblInfo);
            panelContent.Controls.Add(dgvBrands);
            panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            panelContent.Location = new System.Drawing.Point(0, 70);
            panelContent.Name = "panelContent";
            panelContent.Padding = new System.Windows.Forms.Padding(12);
            panelContent.Size = new System.Drawing.Size(800, 380);
            panelContent.TabIndex = 101;
            // 
            // dgvBrands
            // 
            dgvBrands.AllowUserToAddRows = true;
            dgvBrands.AllowUserToDeleteRows = true;
            dgvBrands.BackgroundColor = System.Drawing.Color.White;
            dgvBrands.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvBrands.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBrands.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgvBrands.ColumnHeadersHeight = 45;
            dgvBrands.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvBrands.Location = new System.Drawing.Point(12, 42);
            dgvBrands.Name = "dgvBrands";
            dgvBrands.RowHeadersVisible = false;
            dgvBrands.RowHeadersWidth = 62;
            dgvBrands.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvBrands.Size = new System.Drawing.Size(776, 326);
            dgvBrands.TabIndex = 0;
            dgvBrands.RowValidated += dgvBrands_RowValidated;
            dgvBrands.UserDeletingRow += dgvBrands_UserDeletingRow;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            lblInfo.Location = new System.Drawing.Point(12, 12);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new System.Drawing.Size(534, 25);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Ketik pada baris kosong di bawah untuk menambah merk baru. Perubahan akan otomatis tersimpan.";
            // 
            // BrandForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(panelContent);
            Controls.Add(panelHeader);
            Name = "BrandForm";
            Text = "Master Merk (Brand)";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
