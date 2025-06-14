namespace POS_qu
{
    partial class Roles
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvRoles;
        private TextBox txtRoleName;
        private TextBox txtDescription;
        private Label lblRoleName;
        private Label lblDescription;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

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
            dgvRoles = new DataGridView();
            txtRoleName = new TextBox();
            txtDescription = new TextBox();
            lblRoleName = new Label();
            lblDescription = new Label();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            txtSearch = new TextBox();
            lblPagingInfo = new Label();
            btnFirst = new Button();
            btnPrev = new Button();
            btnNext = new Button();
            btnLast = new Button();
            lblPageNumber = new Label();
            cmbPageSize = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            SuspendLayout();
            // 
            // dgvRoles
            // 
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.AllowUserToDeleteRows = false;
            dgvRoles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRoles.Location = new Point(16, 88);
            dgvRoles.MultiSelect = false;
            dgvRoles.Name = "dgvRoles";
            dgvRoles.RowHeadersWidth = 62;
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.Size = new Size(725, 400);
            dgvRoles.TabIndex = 0;
            // 
            // txtRoleName
            // 
            txtRoleName.Location = new Point(956, 38);
            txtRoleName.Name = "txtRoleName";
            txtRoleName.Size = new Size(250, 31);
            txtRoleName.TabIndex = 2;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(956, 86);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(250, 31);
            txtDescription.TabIndex = 4;
            // 
            // lblRoleName
            // 
            lblRoleName.Location = new Point(816, 38);
            lblRoleName.Name = "lblRoleName";
            lblRoleName.Size = new Size(100, 30);
            lblRoleName.TabIndex = 1;
            lblRoleName.Text = "Role Name:";
            // 
            // lblDescription
            // 
            lblDescription.Location = new Point(816, 88);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(116, 30);
            lblDescription.TabIndex = 3;
            lblDescription.Text = "Description:";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(816, 148);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 40);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Add";
            btnAdd.Click += BtnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(926, 148);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 40);
            btnEdit.TabIndex = 6;
            btnEdit.Text = "Edit";
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(1036, 148);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 40);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Delete";
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1146, 148);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 40);
            btnRefresh.TabIndex = 8;
            btnRefresh.Text = "Refresh";
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(16, 48);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(192, 31);
            txtSearch.TabIndex = 9;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(16, 504);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(103, 25);
            lblPagingInfo.TabIndex = 10;
            lblPagingInfo.Text = "Paging Info";
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(240, 504);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(112, 34);
            btnFirst.TabIndex = 11;
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(376, 504);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(112, 34);
            btnPrev.TabIndex = 11;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(504, 504);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 11;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(632, 504);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(112, 34);
            btnLast.TabIndex = 11;
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(224, 48);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(59, 25);
            lblPageNumber.TabIndex = 12;
            lblPageNumber.Text = "label1";
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(16, 544);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 18;
            // 
            // Roles
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1275, 601);
            Controls.Add(cmbPageSize);
            Controls.Add(lblPageNumber);
            Controls.Add(btnLast);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnFirst);
            Controls.Add(lblPagingInfo);
            Controls.Add(txtSearch);
            Controls.Add(dgvRoles);
            Controls.Add(lblRoleName);
            Controls.Add(txtRoleName);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Name = "Roles";
            Text = "Manage User Roles";
            Load += Roles_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private TextBox txtSearch;
        private Label lblPagingInfo;
        private Button btnFirst;
        private Button btnPrev;
        private Button btnNext;
        private Button btnLast;
        private Label lblPageNumber;
        private ComboBox cmbPageSize;
    }
}
