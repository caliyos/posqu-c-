namespace POS_qu
{
    partial class TaxTypeForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.DataGridView dgvTaxTypes;
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
            dgvTaxTypes = new System.Windows.Forms.DataGridView();
            lblHint = new System.Windows.Forms.Label();
            panelHeader.SuspendLayout();
            panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTaxTypes).BeginInit();
            SuspendLayout();
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(900, 70);
            panelHeader.TabIndex = 0;
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClose.BackColor = System.Drawing.Color.White;
            btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnClose.Location = new System.Drawing.Point(730, 15);
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
            btnRefresh.Location = new System.Drawing.Point(580, 15);
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
            lblTitle.Size = new System.Drawing.Size(214, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Master Jenis PPN";
            panelBody.Controls.Add(dgvTaxTypes);
            panelBody.Controls.Add(lblHint);
            panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            panelBody.Location = new System.Drawing.Point(0, 70);
            panelBody.Name = "panelBody";
            panelBody.Padding = new System.Windows.Forms.Padding(16);
            panelBody.Size = new System.Drawing.Size(900, 530);
            panelBody.TabIndex = 1;
            dgvTaxTypes.BackgroundColor = System.Drawing.Color.White;
            dgvTaxTypes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvTaxTypes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTaxTypes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvTaxTypes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTaxTypes.ColumnHeadersHeight = 45;
            dgvTaxTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvTaxTypes.EnableHeadersVisualStyles = false;
            dgvTaxTypes.Location = new System.Drawing.Point(16, 52);
            dgvTaxTypes.Name = "dgvTaxTypes";
            dgvTaxTypes.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            dgvTaxTypes.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvTaxTypes.RowTemplate.Height = 45;
            dgvTaxTypes.Size = new System.Drawing.Size(868, 462);
            dgvTaxTypes.TabIndex = 1;
            lblHint.AutoSize = true;
            lblHint.Dock = System.Windows.Forms.DockStyle.Top;
            lblHint.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblHint.Location = new System.Drawing.Point(16, 16);
            lblHint.Name = "lblHint";
            lblHint.Size = new System.Drawing.Size(367, 23);
            lblHint.TabIndex = 0;
            lblHint.Text = "Edit Nama/Status. Kode tidak bisa diubah.";
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(900, 600);
            Controls.Add(panelBody);
            Controls.Add(panelHeader);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            Name = "TaxTypeForm";
            Text = "Master Jenis PPN";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelBody.ResumeLayout(false);
            panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTaxTypes).EndInit();
            ResumeLayout(false);
        }
    }
}

