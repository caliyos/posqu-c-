namespace POS_qu
{
    partial class Roles
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel panelRolesTop;
        private System.Windows.Forms.Label lblSearchRoles;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblRoleName;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.Panel panelRolesBottom;
        private System.Windows.Forms.Label lblPageNumber;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.ComboBox cmbPageSize;

        private System.Windows.Forms.SplitContainer splitRight;
        private System.Windows.Forms.Panel panelPermTop;
        private System.Windows.Forms.Label lblPermissions;
        private System.Windows.Forms.Label lblPermSearch;
        private System.Windows.Forms.TextBox txtPermSearch;
        private System.Windows.Forms.DataGridView dgvPermissions;
        private System.Windows.Forms.Panel panelPermEdit;
        private System.Windows.Forms.Label lblPermName;
        private System.Windows.Forms.TextBox txtPermName;
        private System.Windows.Forms.Label lblPermDesc;
        private System.Windows.Forms.TextBox txtPermDesc;
        private System.Windows.Forms.Button btnPermAdd;
        private System.Windows.Forms.Button btnPermEdit;
        private System.Windows.Forms.Button btnPermDelete;
        private System.Windows.Forms.Button btnPermRefresh;
        private System.Windows.Forms.Button btnSaveRolePermissions;

        private System.Windows.Forms.Panel panelUsersTop;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.Label lblUserSearch;
        private System.Windows.Forms.TextBox txtUserSearch;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Panel panelUsersBottom;
        private System.Windows.Forms.Button btnUsersRefresh;
        private System.Windows.Forms.Button btnSaveRoleUsers;

        private System.Windows.Forms.TabControl tabUsersApproval;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabSupervisorApproval;
        private System.Windows.Forms.Panel panelApprovalTop;
        private System.Windows.Forms.Label lblApproval;
        private System.Windows.Forms.Label lblApprovalSearch;
        private System.Windows.Forms.TextBox txtApprovalSearch;
        private System.Windows.Forms.DataGridView dgvApproval;
        private System.Windows.Forms.Panel panelApprovalBottom;
        private System.Windows.Forms.Button btnApprovalRefresh;
        private System.Windows.Forms.Button btnSaveApprovalSettings;

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
            panelHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            splitMain = new System.Windows.Forms.SplitContainer();
            panelRolesTop = new System.Windows.Forms.Panel();
            lblSearchRoles = new System.Windows.Forms.Label();
            txtSearch = new System.Windows.Forms.TextBox();
            lblRoleName = new System.Windows.Forms.Label();
            txtRoleName = new System.Windows.Forms.TextBox();
            lblDescription = new System.Windows.Forms.Label();
            txtDescription = new System.Windows.Forms.TextBox();
            btnAdd = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            dgvRoles = new System.Windows.Forms.DataGridView();
            panelRolesBottom = new System.Windows.Forms.Panel();
            cmbPageSize = new System.Windows.Forms.ComboBox();
            btnLast = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            btnPrev = new System.Windows.Forms.Button();
            btnFirst = new System.Windows.Forms.Button();
            lblPageNumber = new System.Windows.Forms.Label();
            splitRight = new System.Windows.Forms.SplitContainer();
            panelPermTop = new System.Windows.Forms.Panel();
            lblPermissions = new System.Windows.Forms.Label();
            lblPermSearch = new System.Windows.Forms.Label();
            txtPermSearch = new System.Windows.Forms.TextBox();
            dgvPermissions = new System.Windows.Forms.DataGridView();
            panelPermEdit = new System.Windows.Forms.Panel();
            btnSaveRolePermissions = new System.Windows.Forms.Button();
            btnPermRefresh = new System.Windows.Forms.Button();
            btnPermDelete = new System.Windows.Forms.Button();
            btnPermEdit = new System.Windows.Forms.Button();
            btnPermAdd = new System.Windows.Forms.Button();
            txtPermDesc = new System.Windows.Forms.TextBox();
            lblPermDesc = new System.Windows.Forms.Label();
            txtPermName = new System.Windows.Forms.TextBox();
            lblPermName = new System.Windows.Forms.Label();
            panelUsersTop = new System.Windows.Forms.Panel();
            lblUsers = new System.Windows.Forms.Label();
            lblUserSearch = new System.Windows.Forms.Label();
            txtUserSearch = new System.Windows.Forms.TextBox();
            dgvUsers = new System.Windows.Forms.DataGridView();
            panelUsersBottom = new System.Windows.Forms.Panel();
            btnUsersRefresh = new System.Windows.Forms.Button();
            btnSaveRoleUsers = new System.Windows.Forms.Button();
            tabUsersApproval = new System.Windows.Forms.TabControl();
            tabUsers = new System.Windows.Forms.TabPage();
            tabSupervisorApproval = new System.Windows.Forms.TabPage();
            panelApprovalTop = new System.Windows.Forms.Panel();
            lblApproval = new System.Windows.Forms.Label();
            lblApprovalSearch = new System.Windows.Forms.Label();
            txtApprovalSearch = new System.Windows.Forms.TextBox();
            dgvApproval = new System.Windows.Forms.DataGridView();
            panelApprovalBottom = new System.Windows.Forms.Panel();
            btnApprovalRefresh = new System.Windows.Forms.Button();
            btnSaveApprovalSettings = new System.Windows.Forms.Button();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            panelRolesTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            panelRolesBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitRight).BeginInit();
            splitRight.Panel1.SuspendLayout();
            splitRight.Panel2.SuspendLayout();
            splitRight.SuspendLayout();
            panelPermTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPermissions).BeginInit();
            panelPermEdit.SuspendLayout();
            panelUsersTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            panelUsersBottom.SuspendLayout();
            tabUsersApproval.SuspendLayout();
            tabUsers.SuspendLayout();
            tabSupervisorApproval.SuspendLayout();
            panelApprovalTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvApproval).BeginInit();
            panelApprovalBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(1600, 70);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(16, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(301, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Roles & Permissions";
            // 
            // splitMain
            // 
            splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitMain.Location = new System.Drawing.Point(0, 70);
            splitMain.Name = "splitMain";
            splitMain.Panel1.Controls.Add(dgvRoles);
            splitMain.Panel1.Controls.Add(panelRolesBottom);
            splitMain.Panel1.Controls.Add(panelRolesTop);
            splitMain.Panel2.Controls.Add(splitRight);
            splitMain.Size = new System.Drawing.Size(1600, 830);
            splitMain.SplitterDistance = 560;
            splitMain.TabIndex = 1;
            // 
            // panelRolesTop
            // 
            panelRolesTop.BackColor = System.Drawing.Color.White;
            panelRolesTop.Controls.Add(btnRefresh);
            panelRolesTop.Controls.Add(btnDelete);
            panelRolesTop.Controls.Add(btnEdit);
            panelRolesTop.Controls.Add(btnAdd);
            panelRolesTop.Controls.Add(txtDescription);
            panelRolesTop.Controls.Add(lblDescription);
            panelRolesTop.Controls.Add(txtRoleName);
            panelRolesTop.Controls.Add(lblRoleName);
            panelRolesTop.Controls.Add(txtSearch);
            panelRolesTop.Controls.Add(lblSearchRoles);
            panelRolesTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelRolesTop.Location = new System.Drawing.Point(0, 0);
            panelRolesTop.Name = "panelRolesTop";
            panelRolesTop.Padding = new System.Windows.Forms.Padding(12);
            panelRolesTop.Size = new System.Drawing.Size(560, 170);
            panelRolesTop.TabIndex = 0;
            // 
            // lblSearchRoles
            // 
            lblSearchRoles.AutoSize = true;
            lblSearchRoles.Location = new System.Drawing.Point(12, 12);
            lblSearchRoles.Name = "lblSearchRoles";
            lblSearchRoles.Size = new System.Drawing.Size(96, 25);
            lblSearchRoles.TabIndex = 0;
            lblSearchRoles.Text = "Cari Role:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new System.Drawing.Point(12, 40);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(250, 31);
            txtSearch.TabIndex = 1;
            // 
            // lblRoleName
            // 
            lblRoleName.AutoSize = true;
            lblRoleName.Location = new System.Drawing.Point(12, 78);
            lblRoleName.Name = "lblRoleName";
            lblRoleName.Size = new System.Drawing.Size(95, 25);
            lblRoleName.TabIndex = 2;
            lblRoleName.Text = "Role Name";
            // 
            // txtRoleName
            // 
            txtRoleName.Location = new System.Drawing.Point(12, 106);
            txtRoleName.Name = "txtRoleName";
            txtRoleName.Size = new System.Drawing.Size(250, 31);
            txtRoleName.TabIndex = 3;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new System.Drawing.Point(280, 78);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new System.Drawing.Size(97, 25);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "Deskripsi";
            // 
            // txtDescription
            // 
            txtDescription.Location = new System.Drawing.Point(280, 106);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new System.Drawing.Size(268, 31);
            txtDescription.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = System.Drawing.Color.White;
            btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdd.Location = new System.Drawing.Point(12, 138);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(90, 32);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "Tambah";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = System.Drawing.Color.White;
            btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEdit.Location = new System.Drawing.Point(110, 138);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(90, 32);
            btnEdit.TabIndex = 7;
            btnEdit.Text = "Ubah";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = System.Drawing.Color.IndianRed;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDelete.ForeColor = System.Drawing.Color.White;
            btnDelete.Location = new System.Drawing.Point(208, 138);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(90, 32);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Hapus";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.White;
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Location = new System.Drawing.Point(306, 138);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(90, 32);
            btnRefresh.TabIndex = 9;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dgvRoles
            // 
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.AllowUserToDeleteRows = false;
            dgvRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvRoles.Location = new System.Drawing.Point(0, 170);
            dgvRoles.MultiSelect = false;
            dgvRoles.Name = "dgvRoles";
            dgvRoles.ReadOnly = true;
            dgvRoles.RowHeadersVisible = false;
            dgvRoles.RowHeadersWidth = 62;
            dgvRoles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.Size = new System.Drawing.Size(560, 618);
            dgvRoles.TabIndex = 1;
            // 
            // panelRolesBottom
            // 
            panelRolesBottom.BackColor = System.Drawing.Color.White;
            panelRolesBottom.Controls.Add(cmbPageSize);
            panelRolesBottom.Controls.Add(btnLast);
            panelRolesBottom.Controls.Add(btnNext);
            panelRolesBottom.Controls.Add(btnPrev);
            panelRolesBottom.Controls.Add(btnFirst);
            panelRolesBottom.Controls.Add(lblPageNumber);
            panelRolesBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelRolesBottom.Location = new System.Drawing.Point(0, 788);
            panelRolesBottom.Name = "panelRolesBottom";
            panelRolesBottom.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            panelRolesBottom.Size = new System.Drawing.Size(560, 42);
            panelRolesBottom.TabIndex = 2;
            // 
            // cmbPageSize
            // 
            cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new System.Drawing.Point(12, 6);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new System.Drawing.Size(90, 33);
            cmbPageSize.TabIndex = 0;
            // 
            // btnLast
            // 
            btnLast.Location = new System.Drawing.Point(446, 6);
            btnLast.Name = "btnLast";
            btnLast.Size = new System.Drawing.Size(70, 30);
            btnLast.TabIndex = 5;
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new System.Drawing.Point(370, 6);
            btnNext.Name = "btnNext";
            btnNext.Size = new System.Drawing.Size(70, 30);
            btnNext.TabIndex = 4;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new System.Drawing.Point(294, 6);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new System.Drawing.Size(70, 30);
            btnPrev.TabIndex = 3;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new System.Drawing.Point(218, 6);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new System.Drawing.Size(70, 30);
            btnFirst.TabIndex = 2;
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new System.Drawing.Point(112, 9);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new System.Drawing.Size(78, 25);
            lblPageNumber.TabIndex = 1;
            lblPageNumber.Text = "0";
            // 
            // splitRight
            // 
            splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            splitRight.Location = new System.Drawing.Point(0, 0);
            splitRight.Name = "splitRight";
            splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            splitRight.Panel1.Controls.Add(dgvPermissions);
            splitRight.Panel1.Controls.Add(panelPermEdit);
            splitRight.Panel1.Controls.Add(panelPermTop);
            splitRight.Panel2.Controls.Add(tabUsersApproval);
            splitRight.Size = new System.Drawing.Size(1036, 830);
            splitRight.SplitterDistance = 420;
            splitRight.TabIndex = 0;
            // 
            // panelPermTop
            // 
            panelPermTop.BackColor = System.Drawing.Color.White;
            panelPermTop.Controls.Add(txtPermSearch);
            panelPermTop.Controls.Add(lblPermSearch);
            panelPermTop.Controls.Add(lblPermissions);
            panelPermTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelPermTop.Location = new System.Drawing.Point(0, 0);
            panelPermTop.Name = "panelPermTop";
            panelPermTop.Padding = new System.Windows.Forms.Padding(12);
            panelPermTop.Size = new System.Drawing.Size(1036, 74);
            panelPermTop.TabIndex = 0;
            // 
            // lblPermissions
            // 
            lblPermissions.AutoSize = true;
            lblPermissions.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            lblPermissions.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblPermissions.Location = new System.Drawing.Point(12, 10);
            lblPermissions.Name = "lblPermissions";
            lblPermissions.Size = new System.Drawing.Size(132, 28);
            lblPermissions.TabIndex = 0;
            lblPermissions.Text = "Permissions";
            // 
            // lblPermSearch
            // 
            lblPermSearch.AutoSize = true;
            lblPermSearch.Location = new System.Drawing.Point(12, 42);
            lblPermSearch.Name = "lblPermSearch";
            lblPermSearch.Size = new System.Drawing.Size(93, 25);
            lblPermSearch.TabIndex = 1;
            lblPermSearch.Text = "Cari izin:";
            // 
            // txtPermSearch
            // 
            txtPermSearch.Location = new System.Drawing.Point(110, 38);
            txtPermSearch.Name = "txtPermSearch";
            txtPermSearch.Size = new System.Drawing.Size(320, 31);
            txtPermSearch.TabIndex = 2;
            // 
            // dgvPermissions
            // 
            dgvPermissions.AllowUserToAddRows = false;
            dgvPermissions.AllowUserToDeleteRows = false;
            dgvPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvPermissions.Location = new System.Drawing.Point(0, 74);
            dgvPermissions.MultiSelect = false;
            dgvPermissions.Name = "dgvPermissions";
            dgvPermissions.RowHeadersVisible = false;
            dgvPermissions.RowHeadersWidth = 62;
            dgvPermissions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvPermissions.Size = new System.Drawing.Size(1036, 246);
            dgvPermissions.TabIndex = 1;
            // 
            // panelPermEdit
            // 
            panelPermEdit.BackColor = System.Drawing.Color.White;
            panelPermEdit.Controls.Add(btnSaveRolePermissions);
            panelPermEdit.Controls.Add(btnPermRefresh);
            panelPermEdit.Controls.Add(btnPermDelete);
            panelPermEdit.Controls.Add(btnPermEdit);
            panelPermEdit.Controls.Add(btnPermAdd);
            panelPermEdit.Controls.Add(txtPermDesc);
            panelPermEdit.Controls.Add(lblPermDesc);
            panelPermEdit.Controls.Add(txtPermName);
            panelPermEdit.Controls.Add(lblPermName);
            panelPermEdit.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelPermEdit.Location = new System.Drawing.Point(0, 320);
            panelPermEdit.Name = "panelPermEdit";
            panelPermEdit.Padding = new System.Windows.Forms.Padding(12);
            panelPermEdit.Size = new System.Drawing.Size(1036, 100);
            panelPermEdit.TabIndex = 2;
            // 
            // btnSaveRolePermissions
            // 
            btnSaveRolePermissions.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            btnSaveRolePermissions.FlatAppearance.BorderSize = 0;
            btnSaveRolePermissions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSaveRolePermissions.ForeColor = System.Drawing.Color.White;
            btnSaveRolePermissions.Location = new System.Drawing.Point(878, 58);
            btnSaveRolePermissions.Name = "btnSaveRolePermissions";
            btnSaveRolePermissions.Size = new System.Drawing.Size(146, 32);
            btnSaveRolePermissions.TabIndex = 8;
            btnSaveRolePermissions.Text = "Simpan Assign";
            btnSaveRolePermissions.UseVisualStyleBackColor = false;
            // 
            // btnPermRefresh
            // 
            btnPermRefresh.BackColor = System.Drawing.Color.White;
            btnPermRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnPermRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPermRefresh.Location = new System.Drawing.Point(780, 58);
            btnPermRefresh.Name = "btnPermRefresh";
            btnPermRefresh.Size = new System.Drawing.Size(90, 32);
            btnPermRefresh.TabIndex = 7;
            btnPermRefresh.Text = "Refresh";
            btnPermRefresh.UseVisualStyleBackColor = false;
            // 
            // btnPermDelete
            // 
            btnPermDelete.BackColor = System.Drawing.Color.IndianRed;
            btnPermDelete.FlatAppearance.BorderSize = 0;
            btnPermDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPermDelete.ForeColor = System.Drawing.Color.White;
            btnPermDelete.Location = new System.Drawing.Point(682, 58);
            btnPermDelete.Name = "btnPermDelete";
            btnPermDelete.Size = new System.Drawing.Size(90, 32);
            btnPermDelete.TabIndex = 6;
            btnPermDelete.Text = "Hapus";
            btnPermDelete.UseVisualStyleBackColor = false;
            // 
            // btnPermEdit
            // 
            btnPermEdit.BackColor = System.Drawing.Color.White;
            btnPermEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnPermEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPermEdit.Location = new System.Drawing.Point(584, 58);
            btnPermEdit.Name = "btnPermEdit";
            btnPermEdit.Size = new System.Drawing.Size(90, 32);
            btnPermEdit.TabIndex = 5;
            btnPermEdit.Text = "Ubah";
            btnPermEdit.UseVisualStyleBackColor = false;
            // 
            // btnPermAdd
            // 
            btnPermAdd.BackColor = System.Drawing.Color.White;
            btnPermAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnPermAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPermAdd.Location = new System.Drawing.Point(486, 58);
            btnPermAdd.Name = "btnPermAdd";
            btnPermAdd.Size = new System.Drawing.Size(90, 32);
            btnPermAdd.TabIndex = 4;
            btnPermAdd.Text = "Tambah";
            btnPermAdd.UseVisualStyleBackColor = false;
            // 
            // txtPermDesc
            // 
            txtPermDesc.Location = new System.Drawing.Point(174, 58);
            txtPermDesc.Name = "txtPermDesc";
            txtPermDesc.Size = new System.Drawing.Size(300, 31);
            txtPermDesc.TabIndex = 3;
            // 
            // lblPermDesc
            // 
            lblPermDesc.AutoSize = true;
            lblPermDesc.Location = new System.Drawing.Point(174, 30);
            lblPermDesc.Name = "lblPermDesc";
            lblPermDesc.Size = new System.Drawing.Size(97, 25);
            lblPermDesc.TabIndex = 2;
            lblPermDesc.Text = "Deskripsi";
            // 
            // txtPermName
            // 
            txtPermName.Location = new System.Drawing.Point(12, 58);
            txtPermName.Name = "txtPermName";
            txtPermName.Size = new System.Drawing.Size(150, 31);
            txtPermName.TabIndex = 1;
            // 
            // lblPermName
            // 
            lblPermName.AutoSize = true;
            lblPermName.Location = new System.Drawing.Point(12, 30);
            lblPermName.Name = "lblPermName";
            lblPermName.Size = new System.Drawing.Size(81, 25);
            lblPermName.TabIndex = 0;
            lblPermName.Text = "Nama Izin";
            // 
            // tabUsersApproval
            // 
            tabUsersApproval.Controls.Add(tabUsers);
            tabUsersApproval.Controls.Add(tabSupervisorApproval);
            tabUsersApproval.Dock = System.Windows.Forms.DockStyle.Fill;
            tabUsersApproval.Location = new System.Drawing.Point(0, 0);
            tabUsersApproval.Name = "tabUsersApproval";
            tabUsersApproval.SelectedIndex = 0;
            tabUsersApproval.Size = new System.Drawing.Size(1036, 406);
            tabUsersApproval.TabIndex = 0;
            // 
            // tabUsers
            // 
            tabUsers.Controls.Add(dgvUsers);
            tabUsers.Controls.Add(panelUsersBottom);
            tabUsers.Controls.Add(panelUsersTop);
            tabUsers.Location = new System.Drawing.Point(4, 34);
            tabUsers.Name = "tabUsers";
            tabUsers.Padding = new System.Windows.Forms.Padding(0);
            tabUsers.Size = new System.Drawing.Size(1028, 368);
            tabUsers.TabIndex = 0;
            tabUsers.Text = "Users";
            tabUsers.UseVisualStyleBackColor = true;
            // 
            // tabSupervisorApproval
            // 
            tabSupervisorApproval.Controls.Add(dgvApproval);
            tabSupervisorApproval.Controls.Add(panelApprovalBottom);
            tabSupervisorApproval.Controls.Add(panelApprovalTop);
            tabSupervisorApproval.Location = new System.Drawing.Point(4, 34);
            tabSupervisorApproval.Name = "tabSupervisorApproval";
            tabSupervisorApproval.Padding = new System.Windows.Forms.Padding(0);
            tabSupervisorApproval.Size = new System.Drawing.Size(1028, 368);
            tabSupervisorApproval.TabIndex = 1;
            tabSupervisorApproval.Text = "Supervisor Approval";
            tabSupervisorApproval.UseVisualStyleBackColor = true;
            // 
            // panelUsersTop
            // 
            panelUsersTop.BackColor = System.Drawing.Color.White;
            panelUsersTop.Controls.Add(txtUserSearch);
            panelUsersTop.Controls.Add(lblUserSearch);
            panelUsersTop.Controls.Add(lblUsers);
            panelUsersTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelUsersTop.Location = new System.Drawing.Point(0, 0);
            panelUsersTop.Name = "panelUsersTop";
            panelUsersTop.Padding = new System.Windows.Forms.Padding(12);
            panelUsersTop.Size = new System.Drawing.Size(1028, 74);
            panelUsersTop.TabIndex = 0;
            // 
            // lblUsers
            // 
            lblUsers.AutoSize = true;
            lblUsers.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            lblUsers.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblUsers.Location = new System.Drawing.Point(12, 10);
            lblUsers.Name = "lblUsers";
            lblUsers.Size = new System.Drawing.Size(64, 28);
            lblUsers.TabIndex = 0;
            lblUsers.Text = "Users";
            // 
            // lblUserSearch
            // 
            lblUserSearch.AutoSize = true;
            lblUserSearch.Location = new System.Drawing.Point(12, 42);
            lblUserSearch.Name = "lblUserSearch";
            lblUserSearch.Size = new System.Drawing.Size(94, 25);
            lblUserSearch.TabIndex = 1;
            lblUserSearch.Text = "Cari user:";
            // 
            // txtUserSearch
            // 
            txtUserSearch.Location = new System.Drawing.Point(110, 38);
            txtUserSearch.Name = "txtUserSearch";
            txtUserSearch.Size = new System.Drawing.Size(320, 31);
            txtUserSearch.TabIndex = 2;
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvUsers.Location = new System.Drawing.Point(0, 74);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.RowHeadersWidth = 62;
            dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new System.Drawing.Size(1028, 232);
            dgvUsers.TabIndex = 1;
            // 
            // panelUsersBottom
            // 
            panelUsersBottom.BackColor = System.Drawing.Color.White;
            panelUsersBottom.Controls.Add(btnUsersRefresh);
            panelUsersBottom.Controls.Add(btnSaveRoleUsers);
            panelUsersBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelUsersBottom.Location = new System.Drawing.Point(0, 306);
            panelUsersBottom.Name = "panelUsersBottom";
            panelUsersBottom.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            panelUsersBottom.Size = new System.Drawing.Size(1028, 62);
            panelUsersBottom.TabIndex = 2;
            // 
            // btnUsersRefresh
            // 
            btnUsersRefresh.BackColor = System.Drawing.Color.White;
            btnUsersRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnUsersRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnUsersRefresh.Location = new System.Drawing.Point(12, 14);
            btnUsersRefresh.Name = "btnUsersRefresh";
            btnUsersRefresh.Size = new System.Drawing.Size(120, 34);
            btnUsersRefresh.TabIndex = 0;
            btnUsersRefresh.Text = "Refresh";
            btnUsersRefresh.UseVisualStyleBackColor = false;
            // 
            // btnSaveRoleUsers
            // 
            btnSaveRoleUsers.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            btnSaveRoleUsers.FlatAppearance.BorderSize = 0;
            btnSaveRoleUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSaveRoleUsers.ForeColor = System.Drawing.Color.White;
            btnSaveRoleUsers.Location = new System.Drawing.Point(150, 14);
            btnSaveRoleUsers.Name = "btnSaveRoleUsers";
            btnSaveRoleUsers.Size = new System.Drawing.Size(180, 34);
            btnSaveRoleUsers.TabIndex = 1;
            btnSaveRoleUsers.Text = "Simpan Assign User";
            btnSaveRoleUsers.UseVisualStyleBackColor = false;
            // 
            // panelApprovalTop
            // 
            panelApprovalTop.BackColor = System.Drawing.Color.White;
            panelApprovalTop.Controls.Add(txtApprovalSearch);
            panelApprovalTop.Controls.Add(lblApprovalSearch);
            panelApprovalTop.Controls.Add(lblApproval);
            panelApprovalTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelApprovalTop.Location = new System.Drawing.Point(0, 0);
            panelApprovalTop.Name = "panelApprovalTop";
            panelApprovalTop.Padding = new System.Windows.Forms.Padding(12);
            panelApprovalTop.Size = new System.Drawing.Size(1028, 74);
            panelApprovalTop.TabIndex = 0;
            // 
            // lblApproval
            // 
            lblApproval.AutoSize = true;
            lblApproval.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            lblApproval.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblApproval.Location = new System.Drawing.Point(12, 10);
            lblApproval.Name = "lblApproval";
            lblApproval.Size = new System.Drawing.Size(216, 28);
            lblApproval.TabIndex = 0;
            lblApproval.Text = "Supervisor Approval";
            // 
            // lblApprovalSearch
            // 
            lblApprovalSearch.AutoSize = true;
            lblApprovalSearch.Location = new System.Drawing.Point(12, 42);
            lblApprovalSearch.Name = "lblApprovalSearch";
            lblApprovalSearch.Size = new System.Drawing.Size(86, 25);
            lblApprovalSearch.TabIndex = 1;
            lblApprovalSearch.Text = "Cari aksi:";
            // 
            // txtApprovalSearch
            // 
            txtApprovalSearch.Location = new System.Drawing.Point(110, 38);
            txtApprovalSearch.Name = "txtApprovalSearch";
            txtApprovalSearch.Size = new System.Drawing.Size(320, 31);
            txtApprovalSearch.TabIndex = 2;
            // 
            // dgvApproval
            // 
            dgvApproval.AllowUserToAddRows = false;
            dgvApproval.AllowUserToDeleteRows = false;
            dgvApproval.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvApproval.Location = new System.Drawing.Point(0, 74);
            dgvApproval.MultiSelect = false;
            dgvApproval.Name = "dgvApproval";
            dgvApproval.RowHeadersVisible = false;
            dgvApproval.RowHeadersWidth = 62;
            dgvApproval.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvApproval.Size = new System.Drawing.Size(1028, 232);
            dgvApproval.TabIndex = 1;
            // 
            // panelApprovalBottom
            // 
            panelApprovalBottom.BackColor = System.Drawing.Color.White;
            panelApprovalBottom.Controls.Add(btnApprovalRefresh);
            panelApprovalBottom.Controls.Add(btnSaveApprovalSettings);
            panelApprovalBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelApprovalBottom.Location = new System.Drawing.Point(0, 306);
            panelApprovalBottom.Name = "panelApprovalBottom";
            panelApprovalBottom.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            panelApprovalBottom.Size = new System.Drawing.Size(1028, 62);
            panelApprovalBottom.TabIndex = 2;
            // 
            // btnApprovalRefresh
            // 
            btnApprovalRefresh.BackColor = System.Drawing.Color.White;
            btnApprovalRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnApprovalRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnApprovalRefresh.Location = new System.Drawing.Point(12, 14);
            btnApprovalRefresh.Name = "btnApprovalRefresh";
            btnApprovalRefresh.Size = new System.Drawing.Size(120, 34);
            btnApprovalRefresh.TabIndex = 0;
            btnApprovalRefresh.Text = "Refresh";
            btnApprovalRefresh.UseVisualStyleBackColor = false;
            // 
            // btnSaveApprovalSettings
            // 
            btnSaveApprovalSettings.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            btnSaveApprovalSettings.FlatAppearance.BorderSize = 0;
            btnSaveApprovalSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSaveApprovalSettings.ForeColor = System.Drawing.Color.White;
            btnSaveApprovalSettings.Location = new System.Drawing.Point(150, 14);
            btnSaveApprovalSettings.Name = "btnSaveApprovalSettings";
            btnSaveApprovalSettings.Size = new System.Drawing.Size(220, 34);
            btnSaveApprovalSettings.TabIndex = 1;
            btnSaveApprovalSettings.Text = "Simpan Pengaturan";
            btnSaveApprovalSettings.UseVisualStyleBackColor = false;
            // 
            // Roles
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(1600, 900);
            Controls.Add(splitMain);
            Controls.Add(panelHeader);
            Name = "Roles";
            Text = "Roles & Permissions";
            Load += Roles_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            panelRolesTop.ResumeLayout(false);
            panelRolesTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            panelRolesBottom.ResumeLayout(false);
            panelRolesBottom.PerformLayout();
            splitRight.Panel1.ResumeLayout(false);
            splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitRight).EndInit();
            splitRight.ResumeLayout(false);
            panelPermTop.ResumeLayout(false);
            panelPermTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPermissions).EndInit();
            panelPermEdit.ResumeLayout(false);
            panelPermEdit.PerformLayout();
            panelUsersTop.ResumeLayout(false);
            panelUsersTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            panelUsersBottom.ResumeLayout(false);
            tabUsersApproval.ResumeLayout(false);
            tabUsers.ResumeLayout(false);
            tabSupervisorApproval.ResumeLayout(false);
            panelApprovalTop.ResumeLayout(false);
            panelApprovalTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvApproval).EndInit();
            panelApprovalBottom.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
