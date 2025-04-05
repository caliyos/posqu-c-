namespace POS_qu
{
    partial class PaymentModalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Label lblTotal;
        private Label lblPaymentAmount;
        private TextBox txtPaymentAmount;
        private Label lblCashback;
        private TextBox txtCashback;
        private Label lblPaymentMethod;
        private ComboBox cmbPaymentMethod;
        private Button btnPay;


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

        private void InitializeComponent()
        {
            lblTotal = new Label();
            lblPaymentAmount = new Label();
            txtPaymentAmount = new TextBox();
            lblCashback = new Label();
            txtCashback = new TextBox();
            lblPaymentMethod = new Label();
            cmbPaymentMethod = new ComboBox();
            btnPay = new Button();
            label1 = new Label();
            cmbMember = new ComboBox();
            SuspendLayout();
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(30, 30);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(102, 25);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Total: $0.00";
            // 
            // lblPaymentAmount
            // 
            lblPaymentAmount.AutoSize = true;
            lblPaymentAmount.Location = new Point(30, 70);
            lblPaymentAmount.Name = "lblPaymentAmount";
            lblPaymentAmount.Size = new Size(159, 25);
            lblPaymentAmount.TabIndex = 1;
            lblPaymentAmount.Text = "Payment Amount: ";
            // 
            // txtPaymentAmount
            // 
            txtPaymentAmount.Location = new Point(232, 64);
            txtPaymentAmount.Name = "txtPaymentAmount";
            txtPaymentAmount.Size = new Size(150, 31);
            txtPaymentAmount.TabIndex = 2;
            txtPaymentAmount.TextChanged += txtPaymentAmount_TextChanged;
            // 
            // lblCashback
            // 
            lblCashback.AutoSize = true;
            lblCashback.Location = new Point(30, 110);
            lblCashback.Name = "lblCashback";
            lblCashback.Size = new Size(96, 25);
            lblCashback.TabIndex = 3;
            lblCashback.Text = "Cashback: ";
            // 
            // txtCashback
            // 
            txtCashback.Location = new Point(232, 112);
            txtCashback.Name = "txtCashback";
            txtCashback.Size = new Size(150, 31);
            txtCashback.TabIndex = 4;
            // 
            // lblPaymentMethod
            // 
            lblPaymentMethod.AutoSize = true;
            lblPaymentMethod.Location = new Point(30, 150);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new Size(157, 25);
            lblPaymentMethod.TabIndex = 5;
            lblPaymentMethod.Text = "Payment Method: ";
            // 
            // cmbPaymentMethod
            // 
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Card", "QRIS" });
            cmbPaymentMethod.Location = new Point(232, 152);
            cmbPaymentMethod.Name = "cmbPaymentMethod";
            cmbPaymentMethod.Size = new Size(150, 33);
            cmbPaymentMethod.TabIndex = 6;
            cmbPaymentMethod.SelectedIndexChanged += cmbPaymentMethod_SelectedIndexChanged;
            // 
            // btnPay
            // 
            btnPay.Location = new Point(232, 232);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(75, 30);
            btnPay.TabIndex = 7;
            btnPay.Text = "Pay";
            btnPay.UseVisualStyleBackColor = true;
            btnPay.Click += btnPay_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 192);
            label1.Name = "label1";
            label1.Size = new Size(79, 25);
            label1.TabIndex = 5;
            label1.Text = "Member";
            // 
            // cmbMember
            // 
            cmbMember.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMember.Items.AddRange(new object[] { "Yoz", "Kaleb", "Umum" });
            cmbMember.Location = new Point(232, 192);
            cmbMember.Name = "cmbMember";
            cmbMember.Size = new Size(150, 33);
            cmbMember.TabIndex = 6;
            // 
            // PaymentModalForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1212, 597);
            Controls.Add(btnPay);
            Controls.Add(cmbMember);
            Controls.Add(cmbPaymentMethod);
            Controls.Add(label1);
            Controls.Add(lblPaymentMethod);
            Controls.Add(txtCashback);
            Controls.Add(lblCashback);
            Controls.Add(txtPaymentAmount);
            Controls.Add(lblPaymentAmount);
            Controls.Add(lblTotal);
            Name = "PaymentModalForm";
            Text = "Payment Modal";
            ResumeLayout(false);
            PerformLayout();
            // 
            // PaymentModalForm
            // 

            //Name = "PaymentModalForm";
            //Text = "PaymentModalForm";
            //Load += PaymentModalForm_Load;
            //ResumeLayout(false);

        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        //private void InitializeComponent()
        //{

        //}

        #endregion

        private Label label1;
        private ComboBox cmbMember;
    }
}