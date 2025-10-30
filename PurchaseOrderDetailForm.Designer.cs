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
            lblPOId = new Label();
            lblSupplier = new Label();
            lblDate = new Label();
            lblStatus = new Label();
            lblTotal = new Label();
            lblNote = new Label();
            dgvItems = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // lblPOId
            // 
            lblPOId.AutoSize = true;
            lblPOId.Location = new Point(72, 48);
            lblPOId.Name = "lblPOId";
            lblPOId.Size = new Size(59, 25);
            lblPOId.TabIndex = 0;
            lblPOId.Text = "label1";
            // 
            // lblSupplier
            // 
            lblSupplier.AutoSize = true;
            lblSupplier.Location = new Point(72, 96);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(59, 25);
            lblSupplier.TabIndex = 0;
            lblSupplier.Text = "label1";
            lblSupplier.Click += label2_Click;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(72, 144);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(59, 25);
            lblDate.TabIndex = 0;
            lblDate.Text = "label1";
            lblDate.Click += label2_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(72, 192);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(59, 25);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "label1";
            lblStatus.Click += label2_Click;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(72, 240);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(59, 25);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "label1";
            lblTotal.Click += label2_Click;
            // 
            // lblNote
            // 
            lblNote.AutoSize = true;
            lblNote.Location = new Point(72, 288);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(59, 25);
            lblNote.TabIndex = 0;
            lblNote.Text = "label1";
            lblNote.Click += label2_Click;
            // 
            // dgvItems
            // 
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Location = new Point(80, 336);
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersWidth = 62;
            dgvItems.Size = new Size(1072, 304);
            dgvItems.TabIndex = 1;
            // 
            // PurchaseOrderDetailForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1511, 892);
            Controls.Add(dgvItems);
            Controls.Add(lblNote);
            Controls.Add(lblTotal);
            Controls.Add(lblStatus);
            Controls.Add(lblDate);
            Controls.Add(lblSupplier);
            Controls.Add(lblPOId);
            Name = "PurchaseOrderDetailForm";
            Text = "PurchaseOrderDetailForm";
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPOId;
        private Label lblSupplier;
        private Label lblDate;
        private Label lblStatus;
        private Label lblTotal;
        private Label lblNote;
        private DataGridView dgvItems;
    }
}