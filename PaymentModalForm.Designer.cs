namespace POS_qu
{
    partial class PaymentModalForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTotal;
        private Label lblPaymentAmount;
        private TextBox txtPaymentAmount;
        private Label lblPaymentMethod;
        private ComboBox cmbPaymentMethod;
        private Button btnPay;

        private Label label1;
        private ComboBox cmbMember;
        private Panel panelCardDetails;
        private Panel panelEwalletDetails;
        private Panel panelSplitPayment;
        private Panel panelBankTransfer;

        // panel card
        private Label lblCardNumber;
        private TextBox txtCardNumber;
        private Label lblCardHolder;
        private TextBox txtCardHolder;
        private Label lblExpiry;
        private TextBox txtExpiry;

        // panel ewallet
        private Label lblEwalletProvider;
        private ComboBox cmbEwalletProvider;
        private Label lblEwalletRef;
        private TextBox txtEwalletRef;

        // panel bank transfer
        private Label lblBankName;
        private ComboBox cmbBankName;
        private Label lblTransferRef;
        private TextBox txtTransferRef;

        // panel split payment
        private Label lblCashPart;
        private TextBox txtCashPart;
        private Label lblCardPart;
        private TextBox txtCardPart;

        // 🔹 Diskon keseluruhan
        private Label lblGlobalDiscount;
        private TextBox txtGlobalDiscountPercent;

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
            lblPaymentMethod = new Label();
            cmbPaymentMethod = new ComboBox();
            btnPay = new Button();
            label1 = new Label();
            cmbMember = new ComboBox();
            panelCardDetails = new Panel();
            lblCardNumber = new Label();
            txtCardNumber = new TextBox();
            lblCardHolder = new Label();
            txtCardHolder = new TextBox();
            lblExpiry = new Label();
            txtExpiry = new TextBox();
            panelEwalletDetails = new Panel();
            lblEwalletProvider = new Label();
            cmbEwalletProvider = new ComboBox();
            lblEwalletRef = new Label();
            txtEwalletRef = new TextBox();
            panelBankTransfer = new Panel();
            lblBankName = new Label();
            cmbBankName = new ComboBox();
            lblTransferRef = new Label();
            txtTransferRef = new TextBox();
            panelSplitPayment = new Panel();
            lblCashPart = new Label();
            txtCashPart = new TextBox();
            lblCardPart = new Label();
            txtCardPart = new TextBox();
            lblGlobalDiscount = new Label();
            txtGlobalDiscountPercent = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtDeliveryAmount = new TextBox();
            txtGlobalNote = new TextBox();
            panelCardDetails.SuspendLayout();
            panelEwalletDetails.SuspendLayout();
            panelBankTransfer.SuspendLayout();
            panelSplitPayment.SuspendLayout();
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
            txtPaymentAmount.TabIndex = 1;
            txtPaymentAmount.TextChanged += txtPaymentAmount_TextChanged;
            // 
            // lblPaymentMethod
            // 
            lblPaymentMethod.AutoSize = true;
            lblPaymentMethod.Location = new Point(30, 110);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new Size(157, 25);
            lblPaymentMethod.TabIndex = 5;
            lblPaymentMethod.Text = "Payment Method: ";
            // 
            // cmbPaymentMethod
            // 
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Card", "QRIS", "Bank Transfer", "Split Payment" });
            cmbPaymentMethod.Location = new Point(232, 112);
            cmbPaymentMethod.Name = "cmbPaymentMethod";
            cmbPaymentMethod.Size = new Size(150, 33);
            cmbPaymentMethod.TabIndex = 2;
            cmbPaymentMethod.SelectedIndexChanged += cmbPaymentMethod_SelectedIndexChanged;
            // 
            // btnPay
            // 
            btnPay.Location = new Point(232, 344);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(120, 40);
            btnPay.TabIndex = 7;
            btnPay.Text = "Pay";
            btnPay.UseVisualStyleBackColor = true;
            btnPay.Click += btnPay_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 152);
            label1.Name = "label1";
            label1.Size = new Size(79, 25);
            label1.TabIndex = 7;
            label1.Text = "Member";
            // 
            // cmbMember
            // 
            cmbMember.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMember.Items.AddRange(new object[] { "Yoz", "Kaleb", "Umum" });
            cmbMember.Location = new Point(232, 152);
            cmbMember.Name = "cmbMember";
            cmbMember.Size = new Size(150, 33);
            cmbMember.TabIndex = 3;
            // 
            // panelCardDetails
            // 
            panelCardDetails.Controls.Add(lblCardNumber);
            panelCardDetails.Controls.Add(txtCardNumber);
            panelCardDetails.Controls.Add(lblCardHolder);
            panelCardDetails.Controls.Add(txtCardHolder);
            panelCardDetails.Controls.Add(lblExpiry);
            panelCardDetails.Controls.Add(txtExpiry);
            panelCardDetails.Location = new Point(420, 30);
            panelCardDetails.Name = "panelCardDetails";
            panelCardDetails.Size = new Size(600, 146);
            panelCardDetails.TabIndex = 10;
            panelCardDetails.Visible = false;
            // 
            // lblCardNumber
            // 
            lblCardNumber.Location = new Point(10, 10);
            lblCardNumber.Name = "lblCardNumber";
            lblCardNumber.Size = new Size(100, 23);
            lblCardNumber.TabIndex = 0;
            lblCardNumber.Text = "Card Number:";
            // 
            // txtCardNumber
            // 
            txtCardNumber.Location = new Point(160, 10);
            txtCardNumber.Name = "txtCardNumber";
            txtCardNumber.Size = new Size(100, 31);
            txtCardNumber.TabIndex = 1;
            // 
            // lblCardHolder
            // 
            lblCardHolder.Location = new Point(10, 50);
            lblCardHolder.Name = "lblCardHolder";
            lblCardHolder.Size = new Size(100, 23);
            lblCardHolder.TabIndex = 2;
            lblCardHolder.Text = "Card Holder:";
            // 
            // txtCardHolder
            // 
            txtCardHolder.Location = new Point(160, 50);
            txtCardHolder.Name = "txtCardHolder";
            txtCardHolder.Size = new Size(100, 31);
            txtCardHolder.TabIndex = 3;
            // 
            // lblExpiry
            // 
            lblExpiry.Location = new Point(10, 90);
            lblExpiry.Name = "lblExpiry";
            lblExpiry.Size = new Size(100, 23);
            lblExpiry.TabIndex = 4;
            lblExpiry.Text = "Expiry Date:";
            // 
            // txtExpiry
            // 
            txtExpiry.Location = new Point(160, 90);
            txtExpiry.Name = "txtExpiry";
            txtExpiry.Size = new Size(100, 31);
            txtExpiry.TabIndex = 5;
            // 
            // panelEwalletDetails
            // 
            panelEwalletDetails.Controls.Add(lblEwalletProvider);
            panelEwalletDetails.Controls.Add(cmbEwalletProvider);
            panelEwalletDetails.Controls.Add(lblEwalletRef);
            panelEwalletDetails.Controls.Add(txtEwalletRef);
            panelEwalletDetails.Location = new Point(416, 184);
            panelEwalletDetails.Name = "panelEwalletDetails";
            panelEwalletDetails.Size = new Size(600, 120);
            panelEwalletDetails.TabIndex = 11;
            panelEwalletDetails.Visible = false;
            // 
            // lblEwalletProvider
            // 
            lblEwalletProvider.Location = new Point(10, 10);
            lblEwalletProvider.Name = "lblEwalletProvider";
            lblEwalletProvider.Size = new Size(100, 23);
            lblEwalletProvider.TabIndex = 0;
            lblEwalletProvider.Text = "Provider:";
            // 
            // cmbEwalletProvider
            // 
            cmbEwalletProvider.Items.AddRange(new object[] { "OVO", "GoPay", "ShopeePay" });
            cmbEwalletProvider.Location = new Point(160, 10);
            cmbEwalletProvider.Name = "cmbEwalletProvider";
            cmbEwalletProvider.Size = new Size(121, 33);
            cmbEwalletProvider.TabIndex = 1;
            // 
            // lblEwalletRef
            // 
            lblEwalletRef.Location = new Point(10, 50);
            lblEwalletRef.Name = "lblEwalletRef";
            lblEwalletRef.Size = new Size(100, 23);
            lblEwalletRef.TabIndex = 2;
            lblEwalletRef.Text = "Reference Code:";
            // 
            // txtEwalletRef
            // 
            txtEwalletRef.Location = new Point(160, 50);
            txtEwalletRef.Name = "txtEwalletRef";
            txtEwalletRef.Size = new Size(100, 31);
            txtEwalletRef.TabIndex = 3;
            // 
            // panelBankTransfer
            // 
            panelBankTransfer.Controls.Add(lblBankName);
            panelBankTransfer.Controls.Add(cmbBankName);
            panelBankTransfer.Controls.Add(lblTransferRef);
            panelBankTransfer.Controls.Add(txtTransferRef);
            panelBankTransfer.Location = new Point(416, 320);
            panelBankTransfer.Name = "panelBankTransfer";
            panelBankTransfer.Size = new Size(600, 120);
            panelBankTransfer.TabIndex = 12;
            panelBankTransfer.Visible = false;
            // 
            // lblBankName
            // 
            lblBankName.Location = new Point(10, 10);
            lblBankName.Name = "lblBankName";
            lblBankName.Size = new Size(100, 23);
            lblBankName.TabIndex = 0;
            lblBankName.Text = "Bank Name:";
            // 
            // cmbBankName
            // 
            cmbBankName.Items.AddRange(new object[] { "BCA", "BNI", "BRI", "Mandiri" });
            cmbBankName.Location = new Point(160, 10);
            cmbBankName.Name = "cmbBankName";
            cmbBankName.Size = new Size(121, 33);
            cmbBankName.TabIndex = 1;
            // 
            // lblTransferRef
            // 
            lblTransferRef.Location = new Point(10, 50);
            lblTransferRef.Name = "lblTransferRef";
            lblTransferRef.Size = new Size(100, 23);
            lblTransferRef.TabIndex = 2;
            lblTransferRef.Text = "Reference Number:";
            // 
            // txtTransferRef
            // 
            txtTransferRef.Location = new Point(160, 50);
            txtTransferRef.Name = "txtTransferRef";
            txtTransferRef.Size = new Size(100, 31);
            txtTransferRef.TabIndex = 3;
            // 
            // panelSplitPayment
            // 
            panelSplitPayment.Controls.Add(lblCashPart);
            panelSplitPayment.Controls.Add(txtCashPart);
            panelSplitPayment.Controls.Add(lblCardPart);
            panelSplitPayment.Controls.Add(txtCardPart);
            panelSplitPayment.Location = new Point(416, 448);
            panelSplitPayment.Name = "panelSplitPayment";
            panelSplitPayment.Size = new Size(600, 120);
            panelSplitPayment.TabIndex = 13;
            panelSplitPayment.Visible = false;
            // 
            // lblCashPart
            // 
            lblCashPart.Location = new Point(10, 10);
            lblCashPart.Name = "lblCashPart";
            lblCashPart.Size = new Size(100, 23);
            lblCashPart.TabIndex = 0;
            lblCashPart.Text = "Cash Amount:";
            // 
            // txtCashPart
            // 
            txtCashPart.Location = new Point(160, 10);
            txtCashPart.Name = "txtCashPart";
            txtCashPart.Size = new Size(100, 31);
            txtCashPart.TabIndex = 1;
            // 
            // lblCardPart
            // 
            lblCardPart.Location = new Point(10, 50);
            lblCardPart.Name = "lblCardPart";
            lblCardPart.Size = new Size(100, 23);
            lblCardPart.TabIndex = 2;
            lblCardPart.Text = "Card Amount:";
            // 
            // txtCardPart
            // 
            txtCardPart.Location = new Point(160, 50);
            txtCardPart.Name = "txtCardPart";
            txtCardPart.Size = new Size(100, 31);
            txtCardPart.TabIndex = 3;
            // 
            // lblGlobalDiscount
            // 
            lblGlobalDiscount.AutoSize = true;
            lblGlobalDiscount.Location = new Point(30, 192);
            lblGlobalDiscount.Name = "lblGlobalDiscount";
            lblGlobalDiscount.Size = new Size(172, 25);
            lblGlobalDiscount.TabIndex = 14;
            lblGlobalDiscount.Text = "Global Discount (%):";
            // 
            // txtGlobalDiscountPercent
            // 
            txtGlobalDiscountPercent.Location = new Point(232, 192);
            txtGlobalDiscountPercent.Name = "txtGlobalDiscountPercent";
            txtGlobalDiscountPercent.Size = new Size(150, 31);
            txtGlobalDiscountPercent.TabIndex = 4;
            txtGlobalDiscountPercent.Text = "0";
            txtGlobalDiscountPercent.TextChanged += txtGlobalDiscountPercent_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 248);
            label2.Name = "label2";
            label2.Size = new Size(145, 25);
            label2.TabIndex = 14;
            label2.Text = "Delivery Amount";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(32, 288);
            label3.Name = "label3";
            label3.Size = new Size(107, 25);
            label3.TabIndex = 14;
            label3.Text = "Global Note";
            // 
            // txtDeliveryAmount
            // 
            txtDeliveryAmount.Location = new Point(232, 248);
            txtDeliveryAmount.Name = "txtDeliveryAmount";
            txtDeliveryAmount.Size = new Size(150, 31);
            txtDeliveryAmount.TabIndex = 5;
            txtDeliveryAmount.TextChanged += txtDeliveryAmount_TextChanged;
            // 
            // txtGlobalNote
            // 
            txtGlobalNote.Location = new Point(232, 288);
            txtGlobalNote.Name = "txtGlobalNote";
            txtGlobalNote.Size = new Size(150, 31);
            txtGlobalNote.TabIndex = 6;
            txtGlobalNote.TextChanged += txtGlobalNote_TextChanged;
            // 
            // PaymentModalForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1100, 600);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblGlobalDiscount);
            Controls.Add(txtGlobalDiscountPercent);
            Controls.Add(lblTotal);
            Controls.Add(lblPaymentAmount);
            Controls.Add(txtGlobalNote);
            Controls.Add(txtDeliveryAmount);
            Controls.Add(txtPaymentAmount);
            Controls.Add(lblPaymentMethod);
            Controls.Add(cmbPaymentMethod);
            Controls.Add(label1);
            Controls.Add(cmbMember);
            Controls.Add(btnPay);
            Controls.Add(panelCardDetails);
            Controls.Add(panelEwalletDetails);
            Controls.Add(panelBankTransfer);
            Controls.Add(panelSplitPayment);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimumSize = new Size(800, 480);
            Name = "PaymentModalForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Payment Modal";
            Load += PaymentModalForm_Load;
            panelCardDetails.ResumeLayout(false);
            panelCardDetails.PerformLayout();
            panelEwalletDetails.ResumeLayout(false);
            panelEwalletDetails.PerformLayout();
            panelBankTransfer.ResumeLayout(false);
            panelBankTransfer.PerformLayout();
            panelSplitPayment.ResumeLayout(false);
            panelSplitPayment.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private Label label2;
        private Label label3;
        private TextBox txtDeliveryAmount;
        private TextBox txtGlobalNote;
    }
}
