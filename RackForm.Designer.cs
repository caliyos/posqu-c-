namespace POS_qu
{
    partial class RackForm
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
            dgvRacks = new System.Windows.Forms.DataGridView();
            lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvRacks).BeginInit();
            SuspendLayout();
            // 
            // dgvRacks
            // 
            dgvRacks.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvRacks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRacks.Location = new System.Drawing.Point(12, 48);
            dgvRacks.Name = "dgvRacks";
            dgvRacks.RowHeadersWidth = 62;
            dgvRacks.Size = new System.Drawing.Size(776, 390);
            dgvRacks.TabIndex = 0;
            dgvRacks.RowValidated += dgvRacks_RowValidated;
            dgvRacks.UserDeletingRow += dgvRacks_UserDeletingRow;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new System.Drawing.Point(12, 13);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new System.Drawing.Size(534, 25);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Ketik pada baris kosong di bawah untuk menambah rak baru. Perubahan akan otomatis tersimpan.";
            // 
            // RackForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(lblInfo);
            Controls.Add(dgvRacks);
            Name = "RackForm";
            Text = "Master Rak (Rack)";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)dgvRacks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRacks;
        private System.Windows.Forms.Label lblInfo;
    }
}