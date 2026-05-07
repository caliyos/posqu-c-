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
            panelHeader = new Panel();
            btnClose = new Button();
            lblTitle = new Label();
            panelContent = new Panel();
            lblInfo = new Label();
            dgvRacks = new DataGridView();
            panelHeader.SuspendLayout();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRacks).BeginInit();
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
            panelHeader.Size = new Size(951, 79);
            panelHeader.TabIndex = 100;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.White;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(791, 15);
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
            lblTitle.Size = new Size(74, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Rak";
            // 
            // panelContent
            // 
            panelContent.Controls.Add(lblInfo);
            panelContent.Controls.Add(dgvRacks);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 79);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(12);
            panelContent.Size = new Size(951, 541);
            panelContent.TabIndex = 101;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Dock = DockStyle.Top;
            lblInfo.Location = new Point(12, 12);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(793, 25);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Ketik pada baris kosong di bawah untuk menambah rak baru. Perubahan akan otomatis tersimpan.";
            // 
            // dgvRacks
            // 
            dgvRacks.BackgroundColor = Color.White;
            dgvRacks.BorderStyle = BorderStyle.None;
            dgvRacks.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRacks.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvRacks.ColumnHeadersHeight = 45;
            dgvRacks.Dock = DockStyle.Bottom;
            dgvRacks.Location = new Point(12, 55);
            dgvRacks.Name = "dgvRacks";
            dgvRacks.RowHeadersVisible = false;
            dgvRacks.RowHeadersWidth = 62;
            dgvRacks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRacks.Size = new Size(927, 474);
            dgvRacks.TabIndex = 0;
            dgvRacks.RowValidated += dgvRacks_RowValidated;
            dgvRacks.UserDeletingRow += dgvRacks_UserDeletingRow;
            // 
            // RackForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(951, 620);
            Controls.Add(panelContent);
            Controls.Add(panelHeader);
            Name = "RackForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Master Rak (Rack)";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRacks).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.DataGridView dgvRacks;
        private System.Windows.Forms.Label lblInfo;
    }
}
