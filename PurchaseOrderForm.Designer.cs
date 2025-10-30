namespace POS_qu
{
    partial class PurchaseOrderForm
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
            cmbSupplier = new ComboBox();
            dtpOrderDate = new DateTimePicker();
            cmbStatus = new ComboBox();
            dgvItems = new DataGridView();
            btnSave = new Button();
            btnAddItem = new Button();
            lblPagingInfo = new Label();
            txtSearch = new TextBox();
            btnLast = new Button();
            btnNext = new Button();
            btnPrev = new Button();
            btnFirst = new Button();
            cmbPageSize = new ComboBox();
            lblPageNumber = new Label();
            dgvOrderDetails = new DataGridView();
            txtNote = new RichTextBox();
            label2 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderDetails).BeginInit();
            SuspendLayout();
            // 
            // cmbSupplier
            // 
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Location = new Point(72, 24);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(182, 33);
            cmbSupplier.TabIndex = 0;
            // 
            // dtpOrderDate
            // 
            dtpOrderDate.Location = new Point(72, 80);
            dtpOrderDate.Name = "dtpOrderDate";
            dtpOrderDate.Size = new Size(300, 31);
            dtpOrderDate.TabIndex = 1;
            // 
            // cmbStatus
            // 
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(72, 136);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(182, 33);
            cmbStatus.TabIndex = 2;
            // 
            // dgvItems
            // 
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Location = new Point(72, 240);
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersWidth = 62;
            dgvItems.Size = new Size(1168, 800);
            dgvItems.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(1328, 1264);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnAddItem
            // 
            btnAddItem.Location = new Point(72, 200);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(112, 34);
            btnAddItem.TabIndex = 5;
            btnAddItem.Text = "Add Item";
            btnAddItem.UseVisualStyleBackColor = true;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(1144, 1112);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(103, 25);
            lblPagingInfo.TabIndex = 6;
            lblPagingInfo.Text = "Paging Info";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(928, 192);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(310, 31);
            txtSearch.TabIndex = 7;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(720, 1064);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(112, 34);
            btnLast.TabIndex = 12;
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(592, 1064);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 13;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(464, 1064);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(112, 34);
            btnPrev.TabIndex = 14;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(328, 1064);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(112, 34);
            btnFirst.TabIndex = 15;
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(1064, 1064);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 19;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(656, 192);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(59, 25);
            lblPageNumber.TabIndex = 20;
            lblPageNumber.Text = "label1";
            // 
            // dgvOrderDetails
            // 
            dgvOrderDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrderDetails.Location = new Point(1320, 240);
            dgvOrderDetails.Name = "dgvOrderDetails";
            dgvOrderDetails.RowHeadersWidth = 62;
            dgvOrderDetails.Size = new Size(880, 800);
            dgvOrderDetails.TabIndex = 21;
            // 
            // txtNote
            // 
            txtNote.Location = new Point(1328, 1096);
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(608, 144);
            txtNote.TabIndex = 22;
            txtNote.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1328, 1064);
            label2.Name = "label2";
            label2.Size = new Size(72, 25);
            label2.TabIndex = 23;
            label2.Text = "Catatan";
            // 
            // button1
            // 
            button1.Location = new Point(1968, 24);
            button1.Name = "button1";
            button1.Size = new Size(208, 34);
            button1.TabIndex = 24;
            button1.Text = "Daftar Pembelian";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // PurchaseOrderForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2244, 1370);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(txtNote);
            Controls.Add(dgvOrderDetails);
            Controls.Add(lblPageNumber);
            Controls.Add(cmbPageSize);
            Controls.Add(btnLast);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnFirst);
            Controls.Add(txtSearch);
            Controls.Add(lblPagingInfo);
            Controls.Add(btnAddItem);
            Controls.Add(btnSave);
            Controls.Add(dgvItems);
            Controls.Add(cmbStatus);
            Controls.Add(dtpOrderDate);
            Controls.Add(cmbSupplier);
            Name = "PurchaseOrderForm";
            Text = "PurchaseOrderForm";
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbSupplier;
        private DateTimePicker dtpOrderDate;
        private ComboBox cmbStatus;
        private DataGridView dgvItems;
        private Button btnSave;
        private Button btnAddItem;
        private Label lblPagingInfo;
        private TextBox txtSearch;
        private Button btnLast;
        private Button btnNext;
        private Button btnPrev;
        private Button btnFirst;
        private ComboBox cmbPageSize;
        private Label lblPageNumber;
        private DataGridView dgvOrderDetails;
        private RichTextBox txtNote;
        private Label label2;
        private Button button1;
    }
}