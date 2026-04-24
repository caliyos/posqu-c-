namespace POS_qu
{
    partial class PurchaseOrderDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelHeader = new Panel();
            lblTitle = new Label();
            btnPreview = new Button();
            btnSavePdf = new Button();
            btnClose = new Button();
            panelInfo = new Panel();
            tableInfo = new TableLayoutPanel();
            lblPoNoKey = new Label();
            lblSupplierKey = new Label();
            lblDateKey = new Label();
            lblStatusKey = new Label();
            lblWarehouseKey = new Label();
            lblNoteKey = new Label();
            lblPoNoValue = new Label();
            lblSupplierValue = new Label();
            lblDateValue = new Label();
            lblStatusValue = new Label();
            lblWarehouseValue = new Label();
            lblNoteValue = new Label();
            dgvItems = new DataGridView();
            panelFooter = new Panel();
            lblTotalKey = new Label();
            lblTotalValue = new Label();
            panelHeader.SuspendLayout();
            panelInfo.SuspendLayout();
            tableInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnSavePdf);
            panelHeader.Controls.Add(btnPreview);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(16, 14, 16, 14);
            panelHeader.Size = new Size(1200, 70);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(16, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(234, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Faktur Pembelian";
            // 
            // btnPreview
            // 
            btnPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPreview.BackColor = Color.White;
            btnPreview.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnPreview.FlatStyle = FlatStyle.Flat;
            btnPreview.Font = new Font("Segoe UI", 10F);
            btnPreview.Location = new Point(700, 16);
            btnPreview.Name = "btnPreview";
            btnPreview.Size = new Size(150, 38);
            btnPreview.TabIndex = 1;
            btnPreview.Text = "Preview";
            btnPreview.UseVisualStyleBackColor = false;
            // 
            // btnSavePdf
            // 
            btnSavePdf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSavePdf.BackColor = Color.White;
            btnSavePdf.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnSavePdf.FlatStyle = FlatStyle.Flat;
            btnSavePdf.Font = new Font("Segoe UI", 10F);
            btnSavePdf.Location = new Point(860, 16);
            btnSavePdf.Name = "btnSavePdf";
            btnSavePdf.Size = new Size(160, 38);
            btnSavePdf.TabIndex = 2;
            btnSavePdf.Text = "Download PDF";
            btnSavePdf.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.White;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(1030, 16);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(150, 38);
            btnClose.TabIndex = 3;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // panelInfo
            // 
            panelInfo.BackColor = Color.White;
            panelInfo.Controls.Add(tableInfo);
            panelInfo.Dock = DockStyle.Top;
            panelInfo.Location = new Point(0, 70);
            panelInfo.Name = "panelInfo";
            panelInfo.Padding = new Padding(16);
            panelInfo.Size = new Size(1200, 140);
            panelInfo.TabIndex = 1;
            // 
            // tableInfo
            // 
            tableInfo.ColumnCount = 4;
            tableInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableInfo.Controls.Add(lblPoNoKey, 0, 0);
            tableInfo.Controls.Add(lblPoNoValue, 1, 0);
            tableInfo.Controls.Add(lblDateKey, 2, 0);
            tableInfo.Controls.Add(lblDateValue, 3, 0);
            tableInfo.Controls.Add(lblSupplierKey, 0, 1);
            tableInfo.Controls.Add(lblSupplierValue, 1, 1);
            tableInfo.Controls.Add(lblStatusKey, 2, 1);
            tableInfo.Controls.Add(lblStatusValue, 3, 1);
            tableInfo.Controls.Add(lblWarehouseKey, 0, 2);
            tableInfo.Controls.Add(lblWarehouseValue, 1, 2);
            tableInfo.Controls.Add(lblNoteKey, 2, 2);
            tableInfo.Controls.Add(lblNoteValue, 3, 2);
            tableInfo.Dock = DockStyle.Fill;
            tableInfo.Location = new Point(16, 16);
            tableInfo.Name = "tableInfo";
            tableInfo.RowCount = 3;
            tableInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableInfo.Size = new Size(1168, 108);
            tableInfo.TabIndex = 0;
            // 
            // lblPoNoKey
            // 
            lblPoNoKey.AutoSize = true;
            lblPoNoKey.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPoNoKey.ForeColor = Color.FromArgb(80, 80, 80);
            lblPoNoKey.Location = new Point(3, 0);
            lblPoNoKey.Name = "lblPoNoKey";
            lblPoNoKey.Size = new Size(97, 23);
            lblPoNoKey.TabIndex = 0;
            lblPoNoKey.Text = "No PO:";
            // 
            // lblPoNoValue
            // 
            lblPoNoValue.AutoSize = true;
            lblPoNoValue.Font = new Font("Segoe UI", 10F);
            lblPoNoValue.Location = new Point(153, 0);
            lblPoNoValue.Name = "lblPoNoValue";
            lblPoNoValue.Size = new Size(17, 23);
            lblPoNoValue.TabIndex = 1;
            lblPoNoValue.Text = "-";
            // 
            // lblDateKey
            // 
            lblDateKey.AutoSize = true;
            lblDateKey.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDateKey.ForeColor = Color.FromArgb(80, 80, 80);
            lblDateKey.Location = new Point(587, 0);
            lblDateKey.Name = "lblDateKey";
            lblDateKey.Size = new Size(77, 23);
            lblDateKey.TabIndex = 2;
            lblDateKey.Text = "Tanggal:";
            // 
            // lblDateValue
            // 
            lblDateValue.AutoSize = true;
            lblDateValue.Font = new Font("Segoe UI", 10F);
            lblDateValue.Location = new Point(737, 0);
            lblDateValue.Name = "lblDateValue";
            lblDateValue.Size = new Size(17, 23);
            lblDateValue.TabIndex = 3;
            lblDateValue.Text = "-";
            // 
            // lblSupplierKey
            // 
            lblSupplierKey.AutoSize = true;
            lblSupplierKey.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSupplierKey.ForeColor = Color.FromArgb(80, 80, 80);
            lblSupplierKey.Location = new Point(3, 30);
            lblSupplierKey.Name = "lblSupplierKey";
            lblSupplierKey.Size = new Size(84, 23);
            lblSupplierKey.TabIndex = 4;
            lblSupplierKey.Text = "Supplier:";
            // 
            // lblSupplierValue
            // 
            lblSupplierValue.AutoSize = true;
            lblSupplierValue.Font = new Font("Segoe UI", 10F);
            lblSupplierValue.Location = new Point(153, 30);
            lblSupplierValue.Name = "lblSupplierValue";
            lblSupplierValue.Size = new Size(17, 23);
            lblSupplierValue.TabIndex = 5;
            lblSupplierValue.Text = "-";
            // 
            // lblStatusKey
            // 
            lblStatusKey.AutoSize = true;
            lblStatusKey.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatusKey.ForeColor = Color.FromArgb(80, 80, 80);
            lblStatusKey.Location = new Point(587, 30);
            lblStatusKey.Name = "lblStatusKey";
            lblStatusKey.Size = new Size(65, 23);
            lblStatusKey.TabIndex = 6;
            lblStatusKey.Text = "Status:";
            // 
            // lblStatusValue
            // 
            lblStatusValue.AutoSize = true;
            lblStatusValue.Font = new Font("Segoe UI", 10F);
            lblStatusValue.Location = new Point(737, 30);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new Size(17, 23);
            lblStatusValue.TabIndex = 7;
            lblStatusValue.Text = "-";
            // 
            // lblWarehouseKey
            // 
            lblWarehouseKey.AutoSize = true;
            lblWarehouseKey.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblWarehouseKey.ForeColor = Color.FromArgb(80, 80, 80);
            lblWarehouseKey.Location = new Point(3, 60);
            lblWarehouseKey.Name = "lblWarehouseKey";
            lblWarehouseKey.Size = new Size(79, 23);
            lblWarehouseKey.TabIndex = 8;
            lblWarehouseKey.Text = "Gudang:";
            // 
            // lblWarehouseValue
            // 
            lblWarehouseValue.AutoSize = true;
            lblWarehouseValue.Font = new Font("Segoe UI", 10F);
            lblWarehouseValue.Location = new Point(153, 60);
            lblWarehouseValue.Name = "lblWarehouseValue";
            lblWarehouseValue.Size = new Size(17, 23);
            lblWarehouseValue.TabIndex = 9;
            lblWarehouseValue.Text = "-";
            // 
            // lblNoteKey
            // 
            lblNoteKey.AutoSize = true;
            lblNoteKey.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNoteKey.ForeColor = Color.FromArgb(80, 80, 80);
            lblNoteKey.Location = new Point(587, 60);
            lblNoteKey.Name = "lblNoteKey";
            lblNoteKey.Size = new Size(76, 23);
            lblNoteKey.TabIndex = 10;
            lblNoteKey.Text = "Catatan:";
            // 
            // lblNoteValue
            // 
            lblNoteValue.AutoSize = true;
            lblNoteValue.Font = new Font("Segoe UI", 10F);
            lblNoteValue.Location = new Point(737, 60);
            lblNoteValue.Name = "lblNoteValue";
            lblNoteValue.Size = new Size(17, 23);
            lblNoteValue.TabIndex = 11;
            lblNoteValue.Text = "-";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.Location = new Point(0, 210);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersWidth = 62;
            dgvItems.RowHeadersVisible = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(1200, 470);
            dgvItems.TabIndex = 2;
            // 
            // panelFooter
            // 
            panelFooter.BackColor = Color.White;
            panelFooter.Controls.Add(lblTotalValue);
            panelFooter.Controls.Add(lblTotalKey);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 680);
            panelFooter.Name = "panelFooter";
            panelFooter.Padding = new Padding(16);
            panelFooter.Size = new Size(1200, 70);
            panelFooter.TabIndex = 3;
            // 
            // lblTotalKey
            // 
            lblTotalKey.AutoSize = true;
            lblTotalKey.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalKey.ForeColor = Color.FromArgb(80, 80, 80);
            lblTotalKey.Location = new Point(16, 20);
            lblTotalKey.Name = "lblTotalKey";
            lblTotalKey.Size = new Size(62, 28);
            lblTotalKey.TabIndex = 0;
            lblTotalKey.Text = "Total:";
            // 
            // lblTotalValue
            // 
            lblTotalValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTotalValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalValue.Location = new Point(700, 16);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(484, 36);
            lblTotalValue.TabIndex = 1;
            lblTotalValue.Text = "Rp 0";
            lblTotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // PurchaseOrderDetailForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(1200, 750);
            Controls.Add(dgvItems);
            Controls.Add(panelFooter);
            Controls.Add(panelInfo);
            Controls.Add(panelHeader);
            MinimumSize = new Size(1100, 700);
            Name = "PurchaseOrderDetailForm";
            Text = "Faktur Pembelian";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelInfo.ResumeLayout(false);
            tableInfo.ResumeLayout(false);
            tableInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panelHeader;
        private Label lblTitle;
        private Button btnPreview;
        private Button btnSavePdf;
        private Button btnClose;
        private Panel panelInfo;
        private TableLayoutPanel tableInfo;
        private Label lblPoNoKey;
        private Label lblSupplierKey;
        private Label lblDateKey;
        private Label lblStatusKey;
        private Label lblWarehouseKey;
        private Label lblNoteKey;
        private Label lblPoNoValue;
        private Label lblSupplierValue;
        private Label lblDateValue;
        private Label lblStatusValue;
        private Label lblWarehouseValue;
        private Label lblNoteValue;
        private DataGridView dgvItems;
        private Panel panelFooter;
        private Label lblTotalKey;
        private Label lblTotalValue;
    }
}
