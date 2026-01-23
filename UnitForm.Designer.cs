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
            ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
            SuspendLayout();
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(160, 595);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 35;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(368, 99);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(59, 25);
            lblPageNumber.TabIndex = 34;
            lblPageNumber.Text = "label1";
            // 
            // btnLast
            // 
            btnLast.Location = new Point(776, 555);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(112, 34);
            btnLast.TabIndex = 33;
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(648, 555);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 32;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(520, 555);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(112, 34);
            btnPrev.TabIndex = 31;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(384, 555);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(112, 34);
            btnFirst.TabIndex = 30;
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(160, 555);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(103, 25);
            lblPagingInfo.TabIndex = 29;
            lblPagingInfo.Text = "Paging Info";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(160, 99);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(192, 31);
            txtSearch.TabIndex = 28;
            // 
            // dgvUnits
            // 
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.AllowUserToDeleteRows = false;
            dgvUnits.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUnits.Location = new Point(160, 139);
            dgvUnits.MultiSelect = false;
            dgvUnits.Name = "dgvUnits";
            dgvUnits.RowHeadersWidth = 62;
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.Size = new Size(725, 400);
            dgvUnits.TabIndex = 19;
            // 
            // lblUnit
            // 
            lblUnit.Location = new Point(960, 89);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(100, 30);
            lblUnit.TabIndex = 20;
            lblUnit.Text = "Nama Unit";
            // 
            // txtUnitName
            // 
            txtUnitName.Location = new Point(1100, 89);
            txtUnitName.Name = "txtUnitName";
            txtUnitName.Size = new Size(250, 31);
            txtUnitName.TabIndex = 21;
            // 
            // lblAbbr
            // 
            lblAbbr.Location = new Point(960, 139);
            lblAbbr.Name = "lblAbbr";
            lblAbbr.Size = new Size(116, 30);
            lblAbbr.TabIndex = 22;
            lblAbbr.Text = "Singkatan";
            // 
            // txtAbbr
            // 
            txtAbbr.Location = new Point(1100, 137);
            txtAbbr.Name = "txtAbbr";
            txtAbbr.Size = new Size(250, 31);
            txtAbbr.TabIndex = 23;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(960, 199);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 40);
            btnAdd.TabIndex = 24;
            btnAdd.Text = "Add";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(1070, 199);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 40);
            btnEdit.TabIndex = 25;
            btnEdit.Text = "Edit";
            btnEdit.Click  += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(1180, 199);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 40);
            btnDelete.TabIndex = 26;
            btnDelete.Text = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1290, 199);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 40);
            btnRefresh.TabIndex = 27;
            btnRefresh.Text = "Refresh";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // UnitForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1551, 717);
            Controls.Add(cmbPageSize);
            Controls.Add(lblPageNumber);
            Controls.Add(btnLast);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnFirst);
            Controls.Add(lblPagingInfo);
            Controls.Add(txtSearch);
            Controls.Add(dgvUnits);
            Controls.Add(lblUnit);
            Controls.Add(txtUnitName);
            Controls.Add(lblAbbr);
            Controls.Add(txtAbbr);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Name = "UnitForm";
            Text = "UnitForm";
            ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
    }
}