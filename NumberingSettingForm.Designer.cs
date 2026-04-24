namespace POS_qu
{
    partial class NumberingSettingForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.DataGridView dgvNumbering;
        private System.Windows.Forms.Label lblHint;

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
            btnClose = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            lblTitle = new System.Windows.Forms.Label();
            panelBody = new System.Windows.Forms.Panel();
            dgvNumbering = new System.Windows.Forms.DataGridView();
            lblHint = new System.Windows.Forms.Label();
            panelHeader.SuspendLayout();
            panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNumbering).BeginInit();
            SuspendLayout();
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(1100, 70);
            panelHeader.TabIndex = 0;
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClose.BackColor = System.Drawing.Color.White;
            btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnClose.Location = new System.Drawing.Point(930, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(150, 40);
            btnClose.TabIndex = 2;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.BackColor = System.Drawing.Color.White;
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnRefresh.Location = new System.Drawing.Point(780, 15);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(140, 40);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(250, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Setting Penomoran";
            panelBody.Controls.Add(dgvNumbering);
            panelBody.Controls.Add(lblHint);
            panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            panelBody.Location = new System.Drawing.Point(0, 70);
            panelBody.Name = "panelBody";
            panelBody.Padding = new System.Windows.Forms.Padding(16);
            panelBody.Size = new System.Drawing.Size(1100, 580);
            panelBody.TabIndex = 1;
            dgvNumbering.BackgroundColor = System.Drawing.Color.White;
            dgvNumbering.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvNumbering.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvNumbering.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvNumbering.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvNumbering.ColumnHeadersHeight = 45;
            dgvNumbering.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvNumbering.EnableHeadersVisualStyles = false;
            dgvNumbering.Location = new System.Drawing.Point(16, 52);
            dgvNumbering.Name = "dgvNumbering";
            dgvNumbering.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dgvNumbering.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvNumbering.RowTemplate.Height = 45;
            dgvNumbering.Size = new System.Drawing.Size(1068, 512);
            dgvNumbering.TabIndex = 1;
            lblHint.AutoSize = true;
            lblHint.Dock = System.Windows.Forms.DockStyle.Top;
            lblHint.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblHint.Location = new System.Drawing.Point(16, 16);
            lblHint.Name = "lblHint";
            lblHint.Size = new System.Drawing.Size(566, 23);
            lblHint.TabIndex = 0;
            lblHint.Text = "DocType tidak bisa diubah. Prefix/Pad/Format bisa diubah. Format: {prefix}-{yyyyMMdd}-{seq}";
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(1100, 650);
            Controls.Add(panelBody);
            Controls.Add(panelHeader);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            Name = "NumberingSettingForm";
            Text = "Setting Penomoran";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelBody.ResumeLayout(false);
            panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNumbering).EndInit();
            ResumeLayout(false);
        }
    }
}

