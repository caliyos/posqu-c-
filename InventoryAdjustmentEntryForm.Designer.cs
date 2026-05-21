namespace POS_qu
{
    partial class InventoryAdjustmentEntryForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.ComboBox cmbReason;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvItems;

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
            panelTop = new Panel();
            btnClose = new Button();
            btnSave = new Button();
            btnRemoveItem = new Button();
            btnAddItem = new Button();
            txtNote = new TextBox();
            lblNote = new Label();
            cmbReason = new ComboBox();
            lblReason = new Label();
            cmbWarehouse = new ComboBox();
            lblWarehouse = new Label();
            dtpDate = new DateTimePicker();
            lblDate = new Label();
            lblTitle = new Label();
            dgvItems = new DataGridView();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(btnClose);
            panelTop.Controls.Add(btnSave);
            panelTop.Controls.Add(btnRemoveItem);
            panelTop.Controls.Add(btnAddItem);
            panelTop.Controls.Add(txtNote);
            panelTop.Controls.Add(lblNote);
            panelTop.Controls.Add(cmbReason);
            panelTop.Controls.Add(lblReason);
            panelTop.Controls.Add(cmbWarehouse);
            panelTop.Controls.Add(lblWarehouse);
            panelTop.Controls.Add(dtpDate);
            panelTop.Controls.Add(lblDate);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(3, 2, 3, 2);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(10, 9, 10, 9);
            panelTop.Size = new Size(1120, 112);
            panelTop.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(805, 64);
            btnClose.Margin = new Padding(3, 2, 3, 2);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(131, 26);
            btnClose.TabIndex = 12;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(665, 64);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(131, 26);
            btnSave.TabIndex = 11;
            btnSave.Text = "Simpan";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click_1;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemoveItem.Location = new Point(805, 34);
            btnRemoveItem.Margin = new Padding(3, 2, 3, 2);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(131, 26);
            btnRemoveItem.TabIndex = 10;
            btnRemoveItem.Text = "Hapus Item";
            btnRemoveItem.UseVisualStyleBackColor = true;
            // 
            // btnAddItem
            // 
            btnAddItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddItem.Location = new Point(665, 34);
            btnAddItem.Margin = new Padding(3, 2, 3, 2);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(131, 26);
            btnAddItem.TabIndex = 9;
            btnAddItem.Text = "Tambah Item";
            btnAddItem.UseVisualStyleBackColor = true;
            // 
            // txtNote
            // 
            txtNote.Location = new Point(79, 86);
            txtNote.Margin = new Padding(3, 2, 3, 2);
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(445, 23);
            txtNote.TabIndex = 8;
            // 
            // lblNote
            // 
            lblNote.AutoSize = true;
            lblNote.Location = new Point(13, 88);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(51, 15);
            lblNote.TabIndex = 7;
            lblNote.Text = "Catatan:";
            // 
            // cmbReason
            // 
            cmbReason.Location = new Point(79, 61);
            cmbReason.Margin = new Padding(3, 2, 3, 2);
            cmbReason.Name = "cmbReason";
            cmbReason.Size = new Size(445, 23);
            cmbReason.TabIndex = 6;
            // 
            // lblReason
            // 
            lblReason.AutoSize = true;
            lblReason.Location = new Point(13, 63);
            lblReason.Name = "lblReason";
            lblReason.Size = new Size(45, 15);
            lblReason.TabIndex = 5;
            lblReason.Text = "Alasan:";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Location = new Point(296, 34);
            cmbWarehouse.Margin = new Padding(3, 2, 3, 2);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(228, 23);
            cmbWarehouse.TabIndex = 4;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(236, 36);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(52, 15);
            lblWarehouse.TabIndex = 3;
            lblWarehouse.Text = "Gudang:";
            // 
            // dtpDate
            // 
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(79, 34);
            dtpDate.Margin = new Padding(3, 2, 3, 2);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(140, 23);
            dtpDate.TabIndex = 2;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(13, 36);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(51, 15);
            lblDate.TabIndex = 1;
            lblDate.Text = "Tanggal:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.Location = new Point(10, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(138, 21);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Inventory Adjust";
            // 
            // dgvItems
            // 
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.Location = new Point(0, 112);
            dgvItems.Margin = new Padding(3, 2, 3, 2);
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersWidth = 51;
            dgvItems.Size = new Size(1120, 428);
            dgvItems.TabIndex = 1;
            // 
            // InventoryAdjustmentEntryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1120, 540);
            Controls.Add(dgvItems);
            Controls.Add(panelTop);
            Margin = new Padding(3, 2, 3, 2);
            Name = "InventoryAdjustmentEntryForm";
            Text = "InventoryAdjustmentEntryForm";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
        }
    }
}

