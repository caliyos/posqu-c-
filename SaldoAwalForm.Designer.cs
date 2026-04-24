namespace POS_qu
{
    partial class SaldoAwalForm
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
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            btnExport = new Button();
            btnImport = new Button();
            btnSave = new Button();
            btnClose = new Button();
            lblHint = new Label();
            dgvSaldo = new DataGridView();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSaldo).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblWarehouse);
            panelTop.Controls.Add(cmbWarehouse);
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
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Segoe UI", 10F);
            lblWarehouse.Location = new Point(14, 14);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(64, 23);
            lblWarehouse.TabIndex = 0;
            lblWarehouse.Text = "Gudang";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new Font("Segoe UI", 10F);
            cmbWarehouse.FormattingEnabled = true;
            cmbWarehouse.Location = new Point(14, 40);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(260, 31);
            cmbWarehouse.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.White;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 10F);
            btnExport.Location = new Point(292, 39);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(115, 32);
            btnExport.TabIndex = 2;
            btnExport.Text = "Export Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnImport
            // 
            btnImport.BackColor = Color.White;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Font = new Font("Segoe UI", 10F);
            btnImport.Location = new Point(417, 39);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(115, 32);
            btnImport.TabIndex = 3;
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
            btnSave.Location = new Point(540, 39);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 32);
            btnSave.TabIndex = 4;
            btnSave.Text = "Simpan";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(680, 39);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 32);
            btnClose.TabIndex = 5;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblHint
            // 
            lblHint.AutoSize = true;
            lblHint.Font = new Font("Segoe UI", 9F);
            lblHint.ForeColor = Color.DimGray;
            lblHint.Location = new Point(292, 14);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(441, 20);
            lblHint.TabIndex = 6;
            lblHint.Text = "Isi Qty & HPP langsung di grid (Ctrl+V untuk paste seperti Excel)";
            // 
            // dgvSaldo
            // 
            dgvSaldo.AllowUserToAddRows = false;
            dgvSaldo.BackgroundColor = Color.White;
            dgvSaldo.BorderStyle = BorderStyle.None;
            dgvSaldo.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSaldo.ColumnHeadersHeight = 42;
            dgvSaldo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSaldo.Dock = DockStyle.Fill;
            dgvSaldo.EnableHeadersVisualStyles = false;
            dgvSaldo.Location = new Point(0, 76);
            dgvSaldo.Name = "dgvSaldo";
            dgvSaldo.RowHeadersVisible = false;
            dgvSaldo.RowTemplate.Height = 40;
            dgvSaldo.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvSaldo.Size = new Size(1200, 624);
            dgvSaldo.TabIndex = 1;
            // 
            // SaldoAwalForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(1200, 700);
            Controls.Add(dgvSaldo);
            Controls.Add(panelTop);
            Name = "SaldoAwalForm";
            Text = "Saldo Awal Stock";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSaldo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Label lblWarehouse;
        private ComboBox cmbWarehouse;
        private Button btnExport;
        private Button btnImport;
        private Button btnSave;
        private Button btnClose;
        private Label lblHint;
        private DataGridView dgvSaldo;
    }
}

