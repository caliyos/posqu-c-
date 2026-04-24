namespace POS_qu
{
    partial class ReceivePOForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblMode = new Label();
            cmbMode = new ComboBox();
            button1 = new Button();
            
            pnlLeft = new Panel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            dgvItems = new DataGridView();
            btnAddItem = new Button();
            pnlPagination = new Panel();
            cmbPageSize = new ComboBox();
            lblPageNumber = new Label();
            lblPagingInfo = new Label();
            btnFirst = new Button();
            btnPrev = new Button();
            btnNext = new Button();
            btnLast = new Button();

            pnlRight = new Panel();
            groupBoxPO = new GroupBox();
            lblSupplier = new Label();
            cmbSupplier = new ComboBox();
            lblOrderDate = new Label();
            dtpOrderDate = new DateTimePicker();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            
            dgvOrderDetails = new DataGridView();
            
            pnlFooter = new Panel();
            label2 = new Label();
            txtNote = new RichTextBox();
            lblTotalAmount = new Label();
            btnSave = new Button();

            pnlHeader.SuspendLayout();
            pnlLeft.SuspendLayout();
            pnlPagination.SuspendLayout();
            pnlRight.SuspendLayout();
            groupBoxPO.SuspendLayout();
            pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderDetails).BeginInit();
            SuspendLayout();

            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(button1);
            pnlHeader.Controls.Add(cmbMode);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1600, 70);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(335, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Pembelian / Penerimaan Barang";
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblMode.AutoSize = true;
            lblMode.Font = new Font("Segoe UI", 10F);
            lblMode.ForeColor = Color.FromArgb(51, 51, 51);
            lblMode.Location = new Point(820, 24);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(61, 28);
            lblMode.TabIndex = 2;
            lblMode.Text = "Mode:";
            // 
            // cmbMode
            // 
            cmbMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMode.Font = new Font("Segoe UI", 10F);
            cmbMode.Location = new Point(890, 20);
            cmbMode.Name = "cmbMode";
            cmbMode.Size = new Size(440, 36);
            cmbMode.TabIndex = 3;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.BackColor = Color.White;
            button1.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10F);
            button1.Location = new Point(1350, 15);
            button1.Name = "button1";
            button1.Size = new Size(220, 40);
            button1.TabIndex = 1;
            button1.Text = "Daftar Pesanan";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;

            // 
            // pnlLeft (Pilih Barang)
            // 
            pnlLeft.BackColor = Color.White;
            pnlLeft.Controls.Add(pnlPagination);
            pnlLeft.Controls.Add(btnAddItem);
            pnlLeft.Controls.Add(dgvItems);
            pnlLeft.Controls.Add(txtSearch);
            pnlLeft.Controls.Add(lblSearch);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 70);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new Padding(20);
            pnlLeft.Size = new Size(700, 830);
            pnlLeft.TabIndex = 1;

            // lblSearch
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 10F);
            lblSearch.Location = new Point(20, 20);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Cari Produk:";

            // txtSearch
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(20, 50);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(660, 34);
            txtSearch.TabIndex = 0;

            // dgvItems
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvItems.ColumnHeadersHeight = 45;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.Location = new Point(20, 100);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.Padding = new Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvItems.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvItems.RowTemplate.Height = 45;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(660, 500);
            dgvItems.TabIndex = 1;

            // btnAddItem
            btnAddItem.BackColor = Color.FromArgb(0, 120, 215);
            btnAddItem.FlatAppearance.BorderSize = 0;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.Font = new Font("Segoe UI Semibold", 10F);
            btnAddItem.ForeColor = Color.White;
            btnAddItem.Location = new Point(20, 620);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(660, 45);
            btnAddItem.TabIndex = 2;
            btnAddItem.Text = "Tambahkan ke PO >";
            btnAddItem.UseVisualStyleBackColor = false;

            // pnlPagination
            pnlPagination.Controls.Add(cmbPageSize);
            pnlPagination.Controls.Add(lblPageNumber);
            pnlPagination.Controls.Add(lblPagingInfo);
            pnlPagination.Controls.Add(btnFirst);
            pnlPagination.Controls.Add(btnPrev);
            pnlPagination.Controls.Add(btnNext);
            pnlPagination.Controls.Add(btnLast);
            pnlPagination.Location = new Point(20, 680);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.Size = new Size(660, 100);
            pnlPagination.TabIndex = 3;

            // pagination controls
            cmbPageSize.Location = new Point(0, 10);
            cmbPageSize.Size = new Size(80, 33);
            
            lblPageNumber.Location = new Point(90, 15);
            lblPageNumber.AutoSize = true;
            
            lblPagingInfo.Location = new Point(0, 60);
            lblPagingInfo.AutoSize = true;

            btnFirst.Location = new Point(300, 10);
            btnFirst.Size = new Size(80, 35);
            btnFirst.Text = "First";
            
            btnPrev.Location = new Point(390, 10);
            btnPrev.Size = new Size(80, 35);
            btnPrev.Text = "Prev";
            
            btnNext.Location = new Point(480, 10);
            btnNext.Size = new Size(80, 35);
            btnNext.Text = "Next";
            
            btnLast.Location = new Point(570, 10);
            btnLast.Size = new Size(80, 35);
            btnLast.Text = "Last";


            // 
            // pnlRight (Detail PO)
            // 
            pnlRight.BackColor = Color.FromArgb(245, 246, 250);
            pnlRight.Controls.Add(dgvOrderDetails);
            pnlRight.Controls.Add(groupBoxPO);
            pnlRight.Controls.Add(pnlFooter);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(700, 70);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(20);
            pnlRight.Size = new Size(900, 830);
            pnlRight.TabIndex = 2;

            // groupBoxPO
            groupBoxPO.BackColor = Color.White;
            groupBoxPO.Controls.Add(cmbStatus);
            groupBoxPO.Controls.Add(lblStatus);
            groupBoxPO.Controls.Add(cmbWarehouse);
            groupBoxPO.Controls.Add(lblWarehouse);
            groupBoxPO.Controls.Add(dtpOrderDate);
            groupBoxPO.Controls.Add(lblOrderDate);
            groupBoxPO.Controls.Add(cmbSupplier);
            groupBoxPO.Controls.Add(lblSupplier);
            groupBoxPO.Dock = DockStyle.Top;
            groupBoxPO.Font = new Font("Segoe UI Semibold", 10F);
            groupBoxPO.Location = new Point(20, 20);
            groupBoxPO.Name = "groupBoxPO";
            groupBoxPO.Size = new Size(860, 180);
            groupBoxPO.TabIndex = 0;
            groupBoxPO.Text = "Informasi Pesanan";

            lblSupplier.AutoSize = true;
            lblSupplier.Font = new Font("Segoe UI", 10F);
            lblSupplier.Location = new Point(20, 40);
            lblSupplier.Text = "Supplier:";
            cmbSupplier.Location = new Point(20, 70);
            cmbSupplier.Size = new Size(300, 33);
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;

            lblOrderDate.AutoSize = true;
            lblOrderDate.Font = new Font("Segoe UI", 10F);
            lblOrderDate.Location = new Point(350, 40);
            lblOrderDate.Text = "Tanggal Order:";
            dtpOrderDate.Location = new Point(350, 70);
            dtpOrderDate.Size = new Size(200, 34);

            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Segoe UI", 10F);
            lblWarehouse.Location = new Point(20, 110);
            lblWarehouse.Text = "Gudang Penerima:";
            cmbWarehouse.Location = new Point(20, 140);
            cmbWarehouse.Size = new Size(300, 33);
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;

            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F);
            lblStatus.Location = new Point(350, 110);
            lblStatus.Text = "Status PO:";
            cmbStatus.Location = new Point(350, 140);
            cmbStatus.Size = new Size(200, 33);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;


            // dgvOrderDetails
            dgvOrderDetails.AllowUserToAddRows = false;
            dgvOrderDetails.BackgroundColor = Color.White;
            dgvOrderDetails.BorderStyle = BorderStyle.None;
            dgvOrderDetails.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvOrderDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(245, 246, 250);
            dataGridViewCellStyle3.Font = new Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvOrderDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvOrderDetails.ColumnHeadersHeight = 45;
            dgvOrderDetails.EnableHeadersVisualStyles = false;
            dgvOrderDetails.Location = new Point(20, 220);
            dgvOrderDetails.Name = "dgvOrderDetails";
            dgvOrderDetails.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle4.Padding = new Padding(5);
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvOrderDetails.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvOrderDetails.RowTemplate.Height = 45;
            dgvOrderDetails.Size = new Size(860, 400);
            dgvOrderDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrderDetails.TabIndex = 1;


            // pnlFooter
            pnlFooter.BackColor = Color.White;
            pnlFooter.Controls.Add(label2);
            pnlFooter.Controls.Add(txtNote);
            pnlFooter.Controls.Add(lblTotalAmount);
            pnlFooter.Controls.Add(btnSave);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(20, 640);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(860, 170);
            pnlFooter.TabIndex = 2;

            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(20, 15);
            label2.Text = "Catatan:";
            
            txtNote.Location = new Point(20, 45);
            txtNote.Size = new Size(400, 100);
            txtNote.BorderStyle = BorderStyle.FixedSingle;

            lblTotalAmount.AutoSize = true;
            lblTotalAmount.Font = new Font("Segoe UI Bold", 16F);
            lblTotalAmount.Location = new Point(450, 45);
            lblTotalAmount.Text = "Total: Rp 0";

            btnSave.BackColor = Color.FromArgb(0, 150, 136);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 12F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(650, 100);
            btnSave.Size = new Size(190, 50);
            btnSave.Text = "Terima & Update Stok";
            btnSave.UseVisualStyleBackColor = false;


            // ReceivePOForm
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1600, 900);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Controls.Add(pnlHeader);
            Name = "ReceivePOForm";
            Text = "Penerimaan Barang";
            StartPosition = FormStartPosition.CenterScreen;

            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            pnlPagination.ResumeLayout(false);
            pnlPagination.PerformLayout();
            pnlRight.ResumeLayout(false);
            groupBoxPO.ResumeLayout(false);
            groupBoxPO.PerformLayout();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderDetails).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblMode;
        private ComboBox cmbMode;
        private Button button1;

        private Panel pnlLeft;
        private Label lblSearch;
        private TextBox txtSearch;
        private DataGridView dgvItems;
        private Button btnAddItem;
        private Panel pnlPagination;
        private ComboBox cmbPageSize;
        private Label lblPageNumber;
        private Label lblPagingInfo;
        private Button btnFirst;
        private Button btnPrev;
        private Button btnNext;
        private Button btnLast;

        private Panel pnlRight;
        private GroupBox groupBoxPO;
        private Label lblSupplier;
        private ComboBox cmbSupplier;
        private Label lblOrderDate;
        private DateTimePicker dtpOrderDate;
        private Label lblWarehouse;
        private ComboBox cmbWarehouse;
        private Label lblStatus;
        private ComboBox cmbStatus;
        
        private DataGridView dgvOrderDetails;
        
        private Panel pnlFooter;
        private Label label2;
        private RichTextBox txtNote;
        private Label lblTotalAmount;
        private Button btnSave;
    }
}
