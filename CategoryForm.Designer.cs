namespace POS_qu
{
    partial class CategoryForm
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
            panelHeader = new Panel();
            lblTitle = new Label();
            btnClose = new Button();
            panelContent = new Panel();
            txtName = new TextBox();
            txtKode = new TextBox();
            txtDescription = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            lbl = new Label();
            lbl2 = new Label();
            label1 = new Label();
            treeViewCategories = new TreeView();
            btnRefresh = new Button();
            cmbParentCategory = new ComboBox();
            label2 = new Label();
            panelHeader.SuspendLayout();
            panelContent.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1101, 70);
            panelHeader.TabIndex = 100;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(122, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Kategori";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.White;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(941, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(140, 40);
            btnClose.TabIndex = 1;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(cmbParentCategory);
            panelContent.Controls.Add(treeViewCategories);
            panelContent.Controls.Add(label2);
            panelContent.Controls.Add(label1);
            panelContent.Controls.Add(lbl2);
            panelContent.Controls.Add(lbl);
            panelContent.Controls.Add(btnRefresh);
            panelContent.Controls.Add(btnDelete);
            panelContent.Controls.Add(btnUpdate);
            panelContent.Controls.Add(btnAdd);
            panelContent.Controls.Add(txtDescription);
            panelContent.Controls.Add(txtKode);
            panelContent.Controls.Add(txtName);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 70);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1101, 529);
            panelContent.TabIndex = 101;
            // 
            // txtName
            // 
            txtName.Location = new Point(256, 16);
            txtName.Name = "txtName";
            txtName.Size = new Size(216, 31);
            txtName.TabIndex = 1;
            // 
            // txtKode
            // 
            txtKode.Location = new Point(256, 64);
            txtKode.Name = "txtKode";
            txtKode.Size = new Size(216, 31);
            txtKode.TabIndex = 2;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(256, 112);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(216, 31);
            txtDescription.TabIndex = 3;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(0, 120, 215);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(32, 320);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(112, 34);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "add";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.White;
            btnUpdate.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Location = new Point(152, 320);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(112, 34);
            btnUpdate.TabIndex = 6;
            btnUpdate.Text = "update";
            btnUpdate.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.White;
            btnDelete.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Location = new Point(272, 320);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(112, 34);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "delete";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // lbl
            // 
            lbl.AutoSize = true;
            lbl.Location = new Point(48, 24);
            lbl.Name = "lbl";
            lbl.Size = new Size(129, 25);
            lbl.TabIndex = 3;
            lbl.Text = "Nama kategori";
            // 
            // lbl2
            // 
            lbl2.AutoSize = true;
            lbl2.Location = new Point(48, 64);
            lbl2.Name = "lbl2";
            lbl2.Size = new Size(123, 25);
            lbl2.TabIndex = 3;
            lbl2.Text = "Kode kategori";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(48, 112);
            label1.Name = "label1";
            label1.Size = new Size(101, 25);
            label1.TabIndex = 3;
            label1.Text = "Keterangan";
            // 
            // treeViewCategories
            // 
            treeViewCategories.BackColor = Color.White;
            treeViewCategories.Location = new Point(584, 16);
            treeViewCategories.Name = "treeViewCategories";
            treeViewCategories.Size = new Size(440, 464);
            treeViewCategories.TabIndex = 4;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Location = new Point(408, 320);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 34);
            btnRefresh.TabIndex = 8;
            btnRefresh.Text = "refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // cmbParentCategory
            // 
            cmbParentCategory.FormattingEnabled = true;
            cmbParentCategory.Location = new Point(256, 168);
            cmbParentCategory.Name = "cmbParentCategory";
            cmbParentCategory.Size = new Size(182, 33);
            cmbParentCategory.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(48, 168);
            label2.Name = "label2";
            label2.Size = new Size(61, 25);
            label2.TabIndex = 3;
            label2.Text = "Parent";
            // 
            // CategoryForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(1101, 599);
            Controls.Add(panelContent);
            Controls.Add(panelHeader);
            Name = "CategoryForm";
            Text = "Master Kategori";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panelHeader;
        private Label lblTitle;
        private Button btnClose;
        private Panel panelContent;
        private TextBox txtName;
        private TextBox txtKode;
        private TextBox txtDescription;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Label lbl;
        private Label lbl2;
        private Label label1;
        private TreeView treeViewCategories;
        private Button btnRefresh;
        private ComboBox cmbParentCategory;
        private Label label2;
    }
}
