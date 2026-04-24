namespace POS_qu
{
    partial class UnitForm
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
            pnlHeader = new Panel();
            lblTitle = new Label();
            btnClose = new Button();
            pnlContent = new Panel();
            cmbPageSize = new ComboBox();
            lblPageNumber = new Label();
            btnLast = new Button();
            btnNext = new Button();
            btnPrev = new Button();
            btnFirst = new Button();
            lblPagingInfo = new Label();
            txtSearch = new TextBox();
            dgvUnits = new DataGridView();
            lblUnit = new Label();
            txtUnitName = new TextBox();
            lblAbbr = new Label();
            txtAbbr = new TextBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            txtOrder = new TextBox();
            label1 = new Label();
            pnlHeader.SuspendLayout();
            pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1241, 70);
            pnlHeader.TabIndex = 100;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(80, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Unit";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.White;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(1081, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(140, 40);
            btnClose.TabIndex = 1;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(cmbPageSize);
            pnlContent.Controls.Add(lblPageNumber);
            pnlContent.Controls.Add(btnLast);
            pnlContent.Controls.Add(btnNext);
            pnlContent.Controls.Add(btnPrev);
            pnlContent.Controls.Add(btnFirst);
            pnlContent.Controls.Add(lblPagingInfo);
            pnlContent.Controls.Add(txtSearch);
            pnlContent.Controls.Add(dgvUnits);
            pnlContent.Controls.Add(lblUnit);
            pnlContent.Controls.Add(txtUnitName);
            pnlContent.Controls.Add(label1);
            pnlContent.Controls.Add(lblAbbr);
            pnlContent.Controls.Add(txtOrder);
            pnlContent.Controls.Add(txtAbbr);
            pnlContent.Controls.Add(btnAdd);
            pnlContent.Controls.Add(btnEdit);
            pnlContent.Controls.Add(btnDelete);
            pnlContent.Controls.Add(btnRefresh);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 70);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1241, 504);
            pnlContent.TabIndex = 101;
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(128, 476);
            cmbPageSize.Margin = new Padding(2, 2, 2, 2);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(146, 28);
            cmbPageSize.TabIndex = 35;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(294, 79);
            lblPageNumber.Margin = new Padding(2, 0, 2, 0);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(50, 20);
            lblPageNumber.TabIndex = 34;
            lblPageNumber.Text = "label1";
            // 
            // btnLast
            // 
            btnLast.Location = new Point(621, 444);
            btnLast.Margin = new Padding(2, 2, 2, 2);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(90, 27);
            btnLast.TabIndex = 33;
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(518, 444);
            btnNext.Margin = new Padding(2, 2, 2, 2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(90, 27);
            btnNext.TabIndex = 32;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(416, 444);
            btnPrev.Margin = new Padding(2, 2, 2, 2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(90, 27);
            btnPrev.TabIndex = 31;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(307, 444);
            btnFirst.Margin = new Padding(2, 2, 2, 2);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(90, 27);
            btnFirst.TabIndex = 30;
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(128, 444);
            lblPagingInfo.Margin = new Padding(2, 0, 2, 0);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(84, 20);
            lblPagingInfo.TabIndex = 29;
            lblPagingInfo.Text = "Paging Info";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(128, 79);
            txtSearch.Margin = new Padding(2, 2, 2, 2);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(154, 27);
            txtSearch.TabIndex = 28;
            // 
            // dgvUnits
            // 
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.AllowUserToDeleteRows = false;
            dgvUnits.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUnits.Location = new Point(128, 111);
            dgvUnits.Margin = new Padding(2, 2, 2, 2);
            dgvUnits.MultiSelect = false;
            dgvUnits.Name = "dgvUnits";
            dgvUnits.RowHeadersWidth = 62;
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.Size = new Size(580, 320);
            dgvUnits.TabIndex = 19;
            // 
            // lblUnit
            // 
            lblUnit.Location = new Point(768, 71);
            lblUnit.Margin = new Padding(2, 0, 2, 0);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(80, 24);
            lblUnit.TabIndex = 20;
            lblUnit.Text = "Nama Unit";
            // 
            // txtUnitName
            // 
            txtUnitName.Location = new Point(880, 71);
            txtUnitName.Margin = new Padding(2, 2, 2, 2);
            txtUnitName.Name = "txtUnitName";
            txtUnitName.Size = new Size(201, 27);
            txtUnitName.TabIndex = 21;
            // 
            // lblAbbr
            // 
            lblAbbr.Location = new Point(768, 112);
            lblAbbr.Margin = new Padding(2, 0, 2, 0);
            lblAbbr.Name = "lblAbbr";
            lblAbbr.Size = new Size(93, 24);
            lblAbbr.TabIndex = 22;
            lblAbbr.Text = "Singkatan";
            // 
            // txtAbbr
            // 
            txtAbbr.Location = new Point(880, 111);
            txtAbbr.Margin = new Padding(2, 2, 2, 2);
            txtAbbr.Name = "txtAbbr";
            txtAbbr.Size = new Size(201, 27);
            txtAbbr.TabIndex = 23;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(0, 120, 215);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(768, 223);
            btnAdd.Margin = new Padding(2, 2, 2, 2);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(80, 32);
            btnAdd.TabIndex = 24;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.White;
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Location = new Point(856, 223);
            btnEdit.Margin = new Padding(2, 2, 2, 2);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(80, 32);
            btnEdit.TabIndex = 25;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.White;
            btnDelete.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Location = new Point(944, 223);
            btnDelete.Margin = new Padding(2, 2, 2, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(80, 32);
            btnDelete.TabIndex = 26;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Location = new Point(1032, 223);
            btnRefresh.Margin = new Padding(2, 2, 2, 2);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(80, 32);
            btnRefresh.TabIndex = 27;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // txtOrder
            // 
            txtOrder.Location = new Point(880, 157);
            txtOrder.Margin = new Padding(2);
            txtOrder.Name = "txtOrder";
            txtOrder.Size = new Size(201, 27);
            txtOrder.TabIndex = 23;
            // 
            // label1
            // 
            label1.Location = new Point(768, 158);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(93, 24);
            label1.TabIndex = 22;
            label1.Text = "Urutan";
            // 
            // UnitForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(1241, 574);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Margin = new Padding(2, 2, 2, 2);
            Name = "UnitForm";
            Text = "Master Unit";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Button btnClose;
        private Panel pnlContent;
        private ComboBox cmbPageSize;
        private Label lblPageNumber;
        private Button btnLast;
        private Button btnNext;
        private Button btnPrev;
        private Button btnFirst;
        private Label lblPagingInfo;
        private TextBox txtSearch;
        private DataGridView dgvUnits;
        private Label lblUnit;
        private TextBox txtUnitName;
        private Label lblAbbr;
        private TextBox txtAbbr;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private TextBox txtOrder;
        private Label label1;
    }
}
