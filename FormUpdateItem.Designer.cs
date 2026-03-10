namespace POS_qu
{
    partial class FormUpdateItem
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
            lblName = new Label();
            lblPrice = new Label();
            numQty = new NumericUpDown();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnCancel = new Button();
            lblSatuan = new Label();
            lblQty = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(58, 51);
            lblName.Margin = new Padding(2, 0, 2, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(50, 20);
            lblName.TabIndex = 0;
            lblName.Text = "label1";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(58, 83);
            lblPrice.Margin = new Padding(2, 0, 2, 0);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(50, 20);
            lblPrice.TabIndex = 0;
            lblPrice.Text = "label1";
            // 
            // numQty
            // 
            numQty.Location = new Point(58, 115);
            numQty.Margin = new Padding(2, 2, 2, 2);
            numQty.Name = "numQty";
            numQty.Size = new Size(144, 27);
            numQty.TabIndex = 1;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(58, 192);
            btnUpdate.Margin = new Padding(2, 2, 2, 2);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(90, 27);
            btnUpdate.TabIndex = 2;
            btnUpdate.Text = "update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click_1;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(160, 192);
            btnDelete.Margin = new Padding(2, 2, 2, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 27);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(269, 192);
            btnCancel.Margin = new Padding(2, 2, 2, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 27);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblSatuan
            // 
            lblSatuan.AutoSize = true;
            lblSatuan.Location = new Point(236, 51);
            lblSatuan.Margin = new Padding(2, 0, 2, 0);
            lblSatuan.Name = "lblSatuan";
            lblSatuan.Size = new Size(50, 20);
            lblSatuan.TabIndex = 0;
            lblSatuan.Text = "label1";
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Location = new Point(160, 51);
            lblQty.Margin = new Padding(2, 0, 2, 0);
            lblQty.Name = "lblQty";
            lblQty.Size = new Size(50, 20);
            lblQty.TabIndex = 0;
            lblQty.Text = "label1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(236, 117);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // FormUpdateItem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(402, 318);
            Controls.Add(btnCancel);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(numQty);
            Controls.Add(lblQty);
            Controls.Add(label1);
            Controls.Add(lblSatuan);
            Controls.Add(lblPrice);
            Controls.Add(lblName);
            Margin = new Padding(2, 2, 2, 2);
            Name = "FormUpdateItem";
            Text = "FormUpdateItem";
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblName;
        private Label lblPrice;
        private NumericUpDown numQty;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnCancel;
        private Label lblSatuan;
        private Label lblQty;
        private Label label1;
    }
}