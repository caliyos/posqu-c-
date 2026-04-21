namespace POS_qu
{
    partial class CustomerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

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
            lblNama = new Label();
            txtName = new TextBox();
            lblTelp = new Label();
            txtPhone = new TextBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            textNote = new TextBox();
            lblNote = new Label();
            txtEmail = new TextBox();
            txtAddress = new TextBox();
            chkMember = new CheckBox();
            txtMemberCode = new TextBox();
            cmbLevel = new ComboBox();
            txtPoints = new TextBox();
            lblEmail = new Label();
            lblAddress = new Label();
            lblMember = new Label();
            lblMemberCode = new Label();
            lblLevel = new Label();
            lblPoints = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
            SuspendLayout();
            // 
            // cmbPageSize
            // 
            cmbPageSize.Location = new Point(20, 660);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(121, 36);
            cmbPageSize.TabIndex = 24;
            // 
            // lblPageNumber
            // 
            lblPageNumber.Location = new Point(286, 664);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(100, 23);
            lblPageNumber.TabIndex = 25;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(719, 660);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(75, 36);
            btnLast.TabIndex = 30;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(619, 660);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(75, 36);
            btnNext.TabIndex = 29;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(519, 660);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(75, 36);
            btnPrev.TabIndex = 28;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(419, 660);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(75, 36);
            btnFirst.TabIndex = 27;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.Location = new Point(180, 660);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(100, 23);
            lblPagingInfo.TabIndex = 26;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(20, 20);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(300, 34);
            txtSearch.TabIndex = 1;
            // 
            // dgvUnits
            // 
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.ColumnHeadersHeight = 34;
            dgvUnits.Location = new Point(20, 70);
            dgvUnits.Name = "dgvUnits";
            dgvUnits.RowHeadersWidth = 62;
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.Size = new Size(780, 580);
            dgvUnits.TabIndex = 0;
            // 
            // lblNama
            // 
            lblNama.Location = new Point(820, 70);
            lblNama.Name = "lblNama";
            lblNama.Size = new Size(100, 23);
            lblNama.TabIndex = 2;
            lblNama.Text = "Nama";
            // 
            // txtName
            // 
            txtName.Location = new Point(940, 70);
            txtName.Name = "txtName";
            txtName.Size = new Size(260, 34);
            txtName.TabIndex = 3;
            // 
            // lblTelp
            // 
            lblTelp.Location = new Point(820, 110);
            lblTelp.Name = "lblTelp";
            lblTelp.Size = new Size(100, 23);
            lblTelp.TabIndex = 4;
            lblTelp.Text = "No HP";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(940, 110);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(260, 34);
            txtPhone.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(820, 470);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(91, 39);
            btnAdd.TabIndex = 20;
            btnAdd.Text = "Save";
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(930, 470);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(91, 39);
            btnEdit.TabIndex = 21;
            btnEdit.Text = "Edit";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(1040, 470);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(91, 39);
            btnDelete.TabIndex = 22;
            btnDelete.Text = "Delete";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1150, 470);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(91, 39);
            btnRefresh.TabIndex = 23;
            btnRefresh.Text = "Refresh";
            // 
            // textNote
            // 
            textNote.Location = new Point(940, 390);
            textNote.Multiline = true;
            textNote.Name = "textNote";
            textNote.Size = new Size(260, 60);
            textNote.TabIndex = 19;
            // 
            // lblNote
            // 
            lblNote.Location = new Point(820, 390);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(100, 23);
            lblNote.TabIndex = 18;
            lblNote.Text = "Catatan";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(940, 150);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(260, 34);
            txtEmail.TabIndex = 7;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(940, 190);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(260, 34);
            txtAddress.TabIndex = 9;
            // 
            // chkMember
            // 
            chkMember.Location = new Point(940, 230);
            chkMember.Name = "chkMember";
            chkMember.Size = new Size(104, 34);
            chkMember.TabIndex = 11;
            chkMember.Text = "Active";
            // 
            // txtMemberCode
            // 
            txtMemberCode.Location = new Point(940, 270);
            txtMemberCode.Name = "txtMemberCode";
            txtMemberCode.Size = new Size(260, 34);
            txtMemberCode.TabIndex = 13;
            // 
            // cmbLevel
            // 
            cmbLevel.Items.AddRange(new object[] { "bronze", "silver", "gold", "platinum" });
            cmbLevel.Location = new Point(940, 310);
            cmbLevel.Name = "cmbLevel";
            cmbLevel.Size = new Size(160, 36);
            cmbLevel.TabIndex = 15;
            // 
            // txtPoints
            // 
            txtPoints.Location = new Point(940, 350);
            txtPoints.Name = "txtPoints";
            txtPoints.ReadOnly = true;
            txtPoints.Size = new Size(260, 34);
            txtPoints.TabIndex = 17;
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(820, 150);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 23);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Email";
            // 
            // lblAddress
            // 
            lblAddress.Location = new Point(820, 190);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(100, 23);
            lblAddress.TabIndex = 8;
            lblAddress.Text = "Alamat";
            // 
            // lblMember
            // 
            lblMember.Location = new Point(820, 230);
            lblMember.Name = "lblMember";
            lblMember.Size = new Size(100, 23);
            lblMember.TabIndex = 10;
            lblMember.Text = "Member";
            // 
            // lblMemberCode
            // 
            lblMemberCode.Location = new Point(820, 270);
            lblMemberCode.Name = "lblMemberCode";
            lblMemberCode.Size = new Size(100, 23);
            lblMemberCode.TabIndex = 12;
            lblMemberCode.Text = "Code";
            // 
            // lblLevel
            // 
            lblLevel.Location = new Point(820, 310);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(100, 23);
            lblLevel.TabIndex = 14;
            lblLevel.Text = "Level";
            // 
            // lblPoints
            // 
            lblPoints.Location = new Point(820, 350);
            lblPoints.Name = "lblPoints";
            lblPoints.Size = new Size(100, 23);
            lblPoints.TabIndex = 16;
            lblPoints.Text = "Points";
            // 
            // CustomerForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(1280, 720);
            Controls.Add(dgvUnits);
            Controls.Add(txtSearch);
            Controls.Add(lblNama);
            Controls.Add(txtName);
            Controls.Add(lblTelp);
            Controls.Add(txtPhone);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblMember);
            Controls.Add(chkMember);
            Controls.Add(lblMemberCode);
            Controls.Add(txtMemberCode);
            Controls.Add(lblLevel);
            Controls.Add(cmbLevel);
            Controls.Add(lblPoints);
            Controls.Add(txtPoints);
            Controls.Add(lblNote);
            Controls.Add(textNote);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Controls.Add(cmbPageSize);
            Controls.Add(lblPageNumber);
            Controls.Add(lblPagingInfo);
            Controls.Add(btnFirst);
            Controls.Add(btnPrev);
            Controls.Add(btnNext);
            Controls.Add(btnLast);
            Font = new Font("Segoe UI", 10F);
            Name = "CustomerForm";
            Text = "Customer POS";
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

        private Label lblNama;
        private TextBox txtName;

        private Label lblTelp;
        private TextBox txtPhone;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        private TextBox textNote;
        private Label lblNote;

        private TextBox txtEmail;
        private TextBox txtAddress;

        private CheckBox chkMember;
        private TextBox txtMemberCode;

        private ComboBox cmbLevel;
        private TextBox txtPoints;

        private Label lblEmail;
        private Label lblAddress;
        private Label lblMember;
        private Label lblMemberCode;
        private Label lblLevel;
        private Label lblPoints;
    }
}