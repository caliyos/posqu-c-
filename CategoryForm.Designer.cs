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
            SuspendLayout();
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
            btnAdd.Location = new Point(32, 320);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(112, 34);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "add";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(152, 320);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(112, 34);
            btnUpdate.TabIndex = 6;
            btnUpdate.Text = "update";
            btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(272, 320);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(112, 34);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "delete";
            btnDelete.UseVisualStyleBackColor = true;
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
            treeViewCategories.Location = new Point(584, 16);
            treeViewCategories.Name = "treeViewCategories";
            treeViewCategories.Size = new Size(440, 464);
            treeViewCategories.TabIndex = 4;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(408, 320);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 34);
            btnRefresh.TabIndex = 8;
            btnRefresh.Text = "refresh";
            btnRefresh.UseVisualStyleBackColor = true;
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
            ClientSize = new Size(1101, 599);
            Controls.Add(cmbParentCategory);
            Controls.Add(treeViewCategories);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lbl2);
            Controls.Add(lbl);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(txtDescription);
            Controls.Add(txtKode);
            Controls.Add(txtName);
            Name = "CategoryForm";
            Text = "Category";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
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