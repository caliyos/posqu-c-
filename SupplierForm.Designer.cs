namespace POS_qu
{
    partial class SupplierForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            txtName = new TextBox();
            txtKode = new TextBox();
            txtContact = new TextBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();
            txtAddress = new TextBox();
            txtNote = new TextBox();
            dataGridViewSuppliers = new DataGridView();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSuppliers).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 24);
            label1.Name = "label1";
            label1.Size = new Size(129, 25);
            label1.TabIndex = 0;
            label1.Text = "Nama Supplier";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 72);
            label2.Name = "label2";
            label2.Size = new Size(123, 25);
            label2.TabIndex = 0;
            label2.Text = "Kode Supplier";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(32, 112);
            label3.Name = "label3";
            label3.Size = new Size(189, 25);
            label3.TabIndex = 0;
            label3.Text = "Nama Kontak Supplier";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(32, 168);
            label4.Name = "label4";
            label4.Size = new Size(64, 25);
            label4.TabIndex = 0;
            label4.Text = "No HP";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(32, 224);
            label5.Name = "label5";
            label5.Size = new Size(54, 25);
            label5.TabIndex = 0;
            label5.Text = "Email";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(32, 272);
            label6.Name = "label6";
            label6.Size = new Size(68, 25);
            label6.TabIndex = 0;
            label6.Text = "Alamat";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(32, 320);
            label7.Name = "label7";
            label7.Size = new Size(101, 25);
            label7.TabIndex = 0;
            label7.Text = "Keterangan";
            // 
            // txtName
            // 
            txtName.Location = new Point(296, 24);
            txtName.Name = "txtName";
            txtName.Size = new Size(384, 31);
            txtName.TabIndex = 1;
            // 
            // txtKode
            // 
            txtKode.Location = new Point(296, 72);
            txtKode.Name = "txtKode";
            txtKode.Size = new Size(384, 31);
            txtKode.TabIndex = 2;
            // 
            // txtContact
            // 
            txtContact.Location = new Point(296, 112);
            txtContact.Name = "txtContact";
            txtContact.Size = new Size(384, 31);
            txtContact.TabIndex = 3;
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(296, 160);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(384, 31);
            txtPhone.TabIndex = 4;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(296, 216);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(384, 31);
            txtEmail.TabIndex = 5;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(296, 264);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(384, 31);
            txtAddress.TabIndex = 6;
            // 
            // txtNote
            // 
            txtNote.Location = new Point(296, 320);
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(384, 31);
            txtNote.TabIndex = 7;
            // 
            // dataGridViewSuppliers
            // 
            dataGridViewSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSuppliers.Location = new Point(760, 32);
            dataGridViewSuppliers.Name = "dataGridViewSuppliers";
            dataGridViewSuppliers.RowHeadersWidth = 62;
            dataGridViewSuppliers.Size = new Size(776, 528);
            dataGridViewSuppliers.TabIndex = 2;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(40, 400);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(112, 34);
            btnAdd.TabIndex = 8;
            btnAdd.Text = "add";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(168, 400);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(112, 34);
            btnUpdate.TabIndex = 9;
            btnUpdate.Text = "update";
            btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(304, 400);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(112, 34);
            btnDelete.TabIndex = 10;
            btnDelete.Text = "delete";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(440, 400);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 34);
            btnRefresh.TabIndex = 11;
            btnRefresh.Text = "refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // SupplierForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1559, 675);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dataGridViewSuppliers);
            Controls.Add(txtNote);
            Controls.Add(txtAddress);
            Controls.Add(txtEmail);
            Controls.Add(txtPhone);
            Controls.Add(txtContact);
            Controls.Add(txtKode);
            Controls.Add(txtName);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SupplierForm";
            Text = "SupplierForm";
            ((System.ComponentModel.ISupportInitialize)dataGridViewSuppliers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox txtName;
        private TextBox txtKode;
        private TextBox txtContact;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private TextBox txtNote;
        private DataGridView dataGridViewSuppliers;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnRefresh;
    }
}