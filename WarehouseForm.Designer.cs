namespace POS_qu
{
    partial class WarehouseForm
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

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            btnClose = new System.Windows.Forms.Button();
            pnlContent = new System.Windows.Forms.Panel();
            cmbPageSize = new System.Windows.Forms.ComboBox();
            lblPageNumber = new System.Windows.Forms.Label();
            btnLast = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            btnPrev = new System.Windows.Forms.Button();
            btnFirst = new System.Windows.Forms.Button();
            lblPagingInfo = new System.Windows.Forms.Label();
            txtSearch = new System.Windows.Forms.TextBox();
            dgvWarehouses = new System.Windows.Forms.DataGridView();
            lblName = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            lblType = new System.Windows.Forms.Label();
            cmbType = new System.Windows.Forms.ComboBox();
            chkIsActive = new System.Windows.Forms.CheckBox();
            btnAdd = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            pnlHeader.SuspendLayout();
            pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvWarehouses).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.White;
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1551, 70);
            pnlHeader.TabIndex = 100;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(127, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Gudang";
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClose.BackColor = System.Drawing.Color.White;
            btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnClose.Location = new System.Drawing.Point(1391, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(140, 40);
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
            pnlContent.Controls.Add(dgvWarehouses);
            pnlContent.Controls.Add(lblName);
            pnlContent.Controls.Add(txtName);
            pnlContent.Controls.Add(lblType);
            pnlContent.Controls.Add(cmbType);
            pnlContent.Controls.Add(chkIsActive);
            pnlContent.Controls.Add(btnAdd);
            pnlContent.Controls.Add(btnEdit);
            pnlContent.Controls.Add(btnDelete);
            pnlContent.Controls.Add(btnRefresh);
            pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlContent.Location = new System.Drawing.Point(0, 70);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new System.Drawing.Size(1551, 647);
            pnlContent.TabIndex = 101;
            
            // cmbPageSize
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new System.Drawing.Point(160, 595);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new System.Drawing.Size(182, 33);
            
            // lblPageNumber
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new System.Drawing.Point(368, 99);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new System.Drawing.Size(59, 25);
            lblPageNumber.Text = "label1";
            
            // btnLast
            btnLast.Location = new System.Drawing.Point(776, 555);
            btnLast.Name = "btnLast";
            btnLast.Size = new System.Drawing.Size(112, 34);
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            
            // btnNext
            btnNext.Location = new System.Drawing.Point(648, 555);
            btnNext.Name = "btnNext";
            btnNext.Size = new System.Drawing.Size(112, 34);
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            
            // btnPrev
            btnPrev.Location = new System.Drawing.Point(520, 555);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new System.Drawing.Size(112, 34);
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            
            // btnFirst
            btnFirst.Location = new System.Drawing.Point(384, 555);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new System.Drawing.Size(112, 34);
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            
            // lblPagingInfo
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new System.Drawing.Point(160, 555);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new System.Drawing.Size(103, 25);
            lblPagingInfo.Text = "Paging Info";
            
            // txtSearch
            txtSearch.Location = new System.Drawing.Point(160, 99);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(192, 31);
            
            // dgvWarehouses
            dgvWarehouses.AllowUserToAddRows = false;
            dgvWarehouses.AllowUserToDeleteRows = false;
            dgvWarehouses.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvWarehouses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvWarehouses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvWarehouses.Location = new System.Drawing.Point(160, 139);
            dgvWarehouses.MultiSelect = false;
            dgvWarehouses.Name = "dgvWarehouses";
            dgvWarehouses.RowHeadersWidth = 62;
            dgvWarehouses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvWarehouses.Size = new System.Drawing.Size(725, 400);
            
            // lblName
            lblName.Location = new System.Drawing.Point(960, 89);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(100, 30);
            lblName.Text = "Nama Gudang";
            
            // txtName
            txtName.Location = new System.Drawing.Point(1100, 89);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(250, 31);
            
            // lblType
            lblType.Location = new System.Drawing.Point(960, 139);
            lblType.Name = "lblType";
            lblType.Size = new System.Drawing.Size(116, 30);
            lblType.Text = "Tipe";
            
            // cmbType
            cmbType.Location = new System.Drawing.Point(1100, 137);
            cmbType.Name = "cmbType";
            cmbType.Size = new System.Drawing.Size(250, 31);
            cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbType.Items.AddRange(new object[] { "store", "warehouse", "kitchen" });
            
            // chkIsActive
            chkIsActive.Location = new System.Drawing.Point(1100, 180);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new System.Drawing.Size(150, 30);
            chkIsActive.Text = "Is Active";
            chkIsActive.Checked = true;
            
            // btnAdd
            btnAdd.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.Location = new System.Drawing.Point(960, 230);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(100, 40);
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            
            // btnEdit
            btnEdit.BackColor = System.Drawing.Color.White;
            btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEdit.Location = new System.Drawing.Point(1070, 230);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(100, 40);
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            
            // btnDelete
            btnDelete.BackColor = System.Drawing.Color.White;
            btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDelete.Location = new System.Drawing.Point(1180, 230);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(100, 40);
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            
            // btnRefresh
            btnRefresh.BackColor = System.Drawing.Color.White;
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Location = new System.Drawing.Point(1290, 230);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(100, 40);
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            
            // WarehouseForm
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(1551, 717);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Name = "WarehouseForm";
            Text = "Master Data Gudang";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvWarehouses).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.ComboBox cmbPageSize;
        private System.Windows.Forms.Label lblPageNumber;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Label lblPagingInfo;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvWarehouses;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
    }
}
