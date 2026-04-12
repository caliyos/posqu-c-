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
            dgvBrands = new System.Windows.Forms.DataGridView();
            lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvBrands).BeginInit();
            SuspendLayout();
            // 
            // dgvBrands
            // 
            dgvBrands.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvBrands.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBrands.Location = new System.Drawing.Point(12, 48);
            dgvBrands.Name = "dgvBrands";
            dgvBrands.RowHeadersWidth = 62;
            dgvBrands.Size = new System.Drawing.Size(776, 390);
            dgvBrands.TabIndex = 0;
            dgvBrands.RowValidated += dgvBrands_RowValidated;
            dgvBrands.UserDeletingRow += dgvBrands_UserDeletingRow;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new System.Drawing.Point(12, 13);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new System.Drawing.Size(534, 25);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Ketik pada baris kosong di bawah untuk menambah merk baru. Perubahan akan otomatis tersimpan.";
            // 
            // BrandForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(lblInfo);
            Controls.Add(dgvBrands);
            Name = "BrandForm";
            Text = "Master Merk (Brand)";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)dgvBrands).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBrands;
        private System.Windows.Forms.Label lblInfo;
    }
}