namespace POS_qu
{
    partial class PriceUpdateForm
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
            panelTop = new Panel();
            lblTitle = new Label();
            btnExport = new Button();
            btnImport = new Button();
            btnSave = new Button();
            btnClose = new Button();
            lblHint = new Label();
            dgvPrices = new DataGridView();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrices).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(btnExport);
            panelTop.Controls.Add(btnImport);
            panelTop.Controls.Add(btnSave);
            panelTop.Controls.Add(btnClose);
            panelTop.Controls.Add(lblHint);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(12);
            panelTop.Size = new Size(1200, 76);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(329, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Ubah Harga Pokok & Harga Jual Item";
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.White;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F);
            btnExport.Location = new Point(12, 40);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(130, 30);
            btnExport.TabIndex = 1;
            btnExport.Text = "Export Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnImport
            // 
            btnImport.BackColor = Color.White;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Font = new Font("Segoe UI", 10F);
            btnImport.Location = new Point(150, 40);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(130, 30);
            btnImport.TabIndex = 2;
            btnImport.Text = "Import Excel";
            btnImport.UseVisualStyleBackColor = false;
            btnImport.Click += btnImport_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(288, 40);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(140, 30);
            btnSave.TabIndex = 3;
            btnSave.Text = "Simpan";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(436, 40);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 30);
            btnClose.TabIndex = 4;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblHint
            // 
            lblHint.AutoSize = true;
            lblHint.Font = new Font("Segoe UI", 9F);
            lblHint.ForeColor = Color.DimGray;
            lblHint.Location = new Point(580, 46);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(380, 20);
            lblHint.TabIndex = 5;
            lblHint.Text = "Edit langsung di grid (Ctrl+V paste seperti Excel)";
            // 
            // dgvPrices
            // 
            dgvPrices.AllowUserToAddRows = false;
            dgvPrices.BackgroundColor = Color.White;
            dgvPrices.BorderStyle = BorderStyle.None;
            dgvPrices.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPrices.ColumnHeadersHeight = 42;
            dgvPrices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPrices.Dock = DockStyle.Fill;
            dgvPrices.EnableHeadersVisualStyles = false;
            dgvPrices.Location = new Point(0, 76);
            dgvPrices.Name = "dgvPrices";
            dgvPrices.RowHeadersVisible = false;
            dgvPrices.RowTemplate.Height = 40;
            dgvPrices.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvPrices.Size = new Size(1200, 624);
            dgvPrices.TabIndex = 1;
            // 
            // PriceUpdateForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(1200, 700);
            Controls.Add(dgvPrices);
            Controls.Add(panelTop);
            Name = "PriceUpdateForm";
            Text = "Ubah Harga Item";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrices).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Label lblTitle;
        private Button btnExport;
        private Button btnImport;
        private Button btnSave;
        private Button btnClose;
        private Label lblHint;
        private DataGridView dgvPrices;
    }
}

