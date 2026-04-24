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
            panelTop = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblDate = new System.Windows.Forms.Label();
            dtpDate = new System.Windows.Forms.DateTimePicker();
            lblWarehouse = new System.Windows.Forms.Label();
            cmbWarehouse = new System.Windows.Forms.ComboBox();
            lblReason = new System.Windows.Forms.Label();
            cmbReason = new System.Windows.Forms.ComboBox();
            lblNote = new System.Windows.Forms.Label();
            txtNote = new System.Windows.Forms.TextBox();
            btnAddItem = new System.Windows.Forms.Button();
            btnRemoveItem = new System.Windows.Forms.Button();
            btnSave = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            dgvItems = new System.Windows.Forms.DataGridView();
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
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new System.Windows.Forms.Padding(12);
            panelTop.Size = new System.Drawing.Size(1280, 150);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(12, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(183, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Inventory Adjust";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new System.Drawing.Point(15, 48);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(67, 20);
            lblDate.TabIndex = 1;
            lblDate.Text = "Tanggal:";
            // 
            // dtpDate
            // 
            dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpDate.Location = new System.Drawing.Point(90, 45);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new System.Drawing.Size(160, 27);
            dtpDate.TabIndex = 2;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new System.Drawing.Point(270, 48);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new System.Drawing.Size(60, 20);
            lblWarehouse.TabIndex = 3;
            lblWarehouse.Text = "Gudang:";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbWarehouse.Location = new System.Drawing.Point(338, 45);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new System.Drawing.Size(260, 28);
            cmbWarehouse.TabIndex = 4;
            // 
            // lblReason
            // 
            lblReason.AutoSize = true;
            lblReason.Location = new System.Drawing.Point(15, 84);
            lblReason.Name = "lblReason";
            lblReason.Size = new System.Drawing.Size(54, 20);
            lblReason.TabIndex = 5;
            lblReason.Text = "Alasan:";
            // 
            // cmbReason
            // 
            cmbReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmbReason.Location = new System.Drawing.Point(90, 81);
            cmbReason.Name = "cmbReason";
            cmbReason.Size = new System.Drawing.Size(508, 28);
            cmbReason.TabIndex = 6;
            // 
            // lblNote
            // 
            lblNote.AutoSize = true;
            lblNote.Location = new System.Drawing.Point(15, 118);
            lblNote.Name = "lblNote";
            lblNote.Size = new System.Drawing.Size(58, 20);
            lblNote.TabIndex = 7;
            lblNote.Text = "Catatan:";
            // 
            // txtNote
            // 
            txtNote.Location = new System.Drawing.Point(90, 115);
            txtNote.Name = "txtNote";
            txtNote.Size = new System.Drawing.Size(508, 27);
            txtNote.TabIndex = 8;
            // 
            // btnAddItem
            // 
            btnAddItem.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAddItem.Location = new System.Drawing.Point(760, 45);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new System.Drawing.Size(150, 34);
            btnAddItem.TabIndex = 9;
            btnAddItem.Text = "Tambah Item";
            btnAddItem.UseVisualStyleBackColor = true;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRemoveItem.Location = new System.Drawing.Point(920, 45);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new System.Drawing.Size(150, 34);
            btnRemoveItem.TabIndex = 10;
            btnRemoveItem.Text = "Hapus Item";
            btnRemoveItem.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSave.Location = new System.Drawing.Point(760, 85);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(150, 34);
            btnSave.TabIndex = 11;
            btnSave.Text = "Simpan";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClose.Location = new System.Drawing.Point(920, 85);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(150, 34);
            btnClose.TabIndex = 12;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // dgvItems
            // 
            dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvItems.Location = new System.Drawing.Point(0, 150);
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersWidth = 51;
            dgvItems.Size = new System.Drawing.Size(1280, 570);
            dgvItems.TabIndex = 1;
            // 
            // InventoryAdjustmentEntryForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1280, 720);
            Controls.Add(dgvItems);
            Controls.Add(panelTop);
            Name = "InventoryAdjustmentEntryForm";
            Text = "InventoryAdjustmentEntryForm";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
        }
    }
}

