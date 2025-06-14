namespace POS_qu
{
    partial class Terminal
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
            dgv = new DataGridView();
            lblTerminalName = new Label();
            txtTerminalName = new TextBox();
            lblPcId = new Label();
            txtPcId = new TextBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            txtDesc = new TextBox();
            lblDesc = new Label();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(16, 520);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 35;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(224, 24);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(59, 25);
            lblPageNumber.TabIndex = 34;
            lblPageNumber.Text = "label1";
            lblPageNumber.Click += lblPageNumber_Click;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(616, 496);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(112, 34);
            btnLast.TabIndex = 33;
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(488, 496);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 32;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(360, 496);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(112, 34);
            btnPrev.TabIndex = 31;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(224, 496);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(112, 34);
            btnFirst.TabIndex = 30;
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(16, 480);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(103, 25);
            lblPagingInfo.TabIndex = 29;
            lblPagingInfo.Text = "Paging Info";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(16, 24);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(192, 31);
            txtSearch.TabIndex = 28;
            // 
            // dgv
            // 
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Location = new Point(16, 64);
            dgv.MultiSelect = false;
            dgv.Name = "dgv";
            dgv.RowHeadersWidth = 62;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Size = new Size(698, 400);
            dgv.TabIndex = 19;
            // 
            // lblTerminalName
            // 
            lblTerminalName.Location = new Point(872, 24);
            lblTerminalName.Name = "lblTerminalName";
            lblTerminalName.Size = new Size(100, 30);
            lblTerminalName.TabIndex = 20;
            lblTerminalName.Text = "Role Name:";
            // 
            // txtTerminalName
            // 
            txtTerminalName.Location = new Point(1012, 24);
            txtTerminalName.Name = "txtTerminalName";
            txtTerminalName.Size = new Size(250, 31);
            txtTerminalName.TabIndex = 21;
            // 
            // lblPcId
            // 
            lblPcId.Location = new Point(872, 74);
            lblPcId.Name = "lblPcId";
            lblPcId.Size = new Size(116, 30);
            lblPcId.TabIndex = 22;
            lblPcId.Text = "Description:";
            // 
            // txtPcId
            // 
            txtPcId.Location = new Point(1012, 72);
            txtPcId.Name = "txtPcId";
            txtPcId.Size = new Size(250, 31);
            txtPcId.TabIndex = 23;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(872, 208);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 40);
            btnAdd.TabIndex = 24;
            btnAdd.Text = "Add";
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(982, 208);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 40);
            btnEdit.TabIndex = 25;
            btnEdit.Text = "Edit";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(1092, 208);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 40);
            btnDelete.TabIndex = 26;
            btnDelete.Text = "Delete";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(1202, 208);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 40);
            btnRefresh.TabIndex = 27;
            btnRefresh.Text = "Refresh";
            // 
            // txtDesc
            // 
            txtDesc.Location = new Point(1012, 118);
            txtDesc.Name = "txtDesc";
            txtDesc.Size = new Size(250, 31);
            txtDesc.TabIndex = 23;
            txtDesc.TextChanged += textBox1_TextChanged;
            // 
            // lblDesc
            // 
            lblDesc.Location = new Point(872, 120);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(116, 30);
            lblDesc.TabIndex = 22;
            lblDesc.Text = "Description:";
            // 
            // Terminal
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1306, 681);
            Controls.Add(cmbPageSize);
            Controls.Add(lblPageNumber);
            Controls.Add(btnLast);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnFirst);
            Controls.Add(lblPagingInfo);
            Controls.Add(txtSearch);
            Controls.Add(dgv);
            Controls.Add(lblTerminalName);
            Controls.Add(txtTerminalName);
            Controls.Add(lblDesc);
            Controls.Add(txtDesc);
            Controls.Add(lblPcId);
            Controls.Add(txtPcId);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Name = "Terminal";
            Text = "Terminal";
            Load += Terminal_Load;
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
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
        private DataGridView dgv;
        private Label lblTerminalName;
        private TextBox txtTerminalName;
        private Label lblPcId;
        private TextBox txtPcId;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private TextBox txtDesc;
        private Label lblDesc;
    }
}