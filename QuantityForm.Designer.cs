namespace POS_qu
{
    partial class QuantityForm
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
            lblStockAvailable = new Label();
            txtQuantity = new TextBox();
            btnSubmit = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblStockAvailable
            // 
            lblStockAvailable.AutoSize = true;
            lblStockAvailable.Location = new Point(24, 32);
            lblStockAvailable.Name = "lblStockAvailable";
            lblStockAvailable.Size = new Size(59, 25);
            lblStockAvailable.TabIndex = 0;
            lblStockAvailable.Text = "label1";
            // 
            // txtQuantity
            // 
            txtQuantity.Location = new Point(24, 72);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(150, 31);
            txtQuantity.TabIndex = 1;
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(24, 128);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(112, 34);
            btnSubmit.TabIndex = 2;
            btnSubmit.Text = "ok";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(160, 128);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 34);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "batal";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnSubmit_Click;
            // 
            // QuantityForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnSubmit);
            Controls.Add(txtQuantity);
            Controls.Add(lblStockAvailable);
            Name = "QuantityForm";
            Text = "QuantityForm";
            Load += QuantityForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblStockAvailable;
        private TextBox txtQuantity;
        private Button btnSubmit;
        private Button btnCancel;
    }
}