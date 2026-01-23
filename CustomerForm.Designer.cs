namespace POS_qu
{
    partial class CustomerForm
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
            ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
            SuspendLayout();
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(162, 610);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 52;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(370, 114);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(59, 25);
            lblPageNumber.TabIndex = 51;
            lblPageNumber.Text = "label1";
            // 
            // btnLast
            // 
            btnLast.Location = new Point(778, 570);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(112, 34);
            btnLast.TabIndex = 50;
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(650, 570);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 49;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(522, 570);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(112, 34);
            btnPrev.TabIndex = 48;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(386, 570);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(112, 34);
            btnFirst.TabIndex = 47;
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(162, 570);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(103, 25);
            lblPagingInfo.TabIndex = 46;
            lblPagingInfo.Text = "Paging Info";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(162, 114);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(192, 31);
            txtSearch.TabIndex = 45;
            // 
            // dgvUnits
            // 
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.AllowUserToDeleteRows = false;
            dgvUnits.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUnits.Location = new Point(162, 154);
            dgvUnits.MultiSelect = false;
            dgvUnits.Name = "dgvUnits";
            dgvUnits.RowHeadersWidth = 62;
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.Size = new Size(725, 400);
            dgvUnits.TabIndex = 36;
            // 
            // lblNama
            // 
            lblNama.Location = new Point(962, 104);
            lblNama.Name = "lblNama";
            lblNama.Size = new Size(100, 30);
            lblNama.TabIndex = 37;
            lblNama.Text = "Nama";
            // 
            // txtName
            // 
            txtName.Location = new Point(1102, 104);
            txtName.Name = "txtName";
            txtName.Size = new Size(250, 31);
            txtName.TabIndex = 38;
            // 
            // lblTelp
            // 
            lblTelp.Location = new Point(960, 152);
            lblTelp.Name = "lblTelp";
            lblTelp.Size = new Size(116, 30);
            lblTelp.TabIndex = 39;
            lblTelp.Text = "No Telp";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(1100, 150);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(250, 31);
            txtPhone.TabIndex = 39;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(960, 280);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 40);
            btnAdd.TabIndex = 41;
            btnAdd.Text = "Add";
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(1070, 280);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 40);
            btnEdit.TabIndex = 42;
            btnEdit.Text = "Edit";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(1180, 280);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 40);
            btnDelete.TabIndex = 43;
            btnDelete.Text = "Delete";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1290, 280);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 40);
            btnRefresh.TabIndex = 44;
            btnRefresh.Text = "Refresh";
            // 
            // textNote
            // 
            textNote.Location = new Point(1100, 198);
            textNote.Name = "textNote";
            textNote.Size = new Size(250, 31);
            textNote.TabIndex = 40;
            // 
            // lblNote
            // 
            lblNote.Location = new Point(960, 200);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(116, 30);
            lblNote.TabIndex = 39;
            lblNote.Text = "Keterangan";
            // 
            // CustomerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1555, 746);
            Controls.Add(cmbPageSize);
            Controls.Add(lblPageNumber);
            Controls.Add(btnLast);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnFirst);
            Controls.Add(lblPagingInfo);
            Controls.Add(txtSearch);
            Controls.Add(dgvUnits);
            Controls.Add(lblNama);
            Controls.Add(txtName);
            Controls.Add(lblNote);
            Controls.Add(textNote);
            Controls.Add(lblTelp);
            Controls.Add(txtPhone);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Name = "CustomerForm";
            Text = "CustomerForm";
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
    }
}