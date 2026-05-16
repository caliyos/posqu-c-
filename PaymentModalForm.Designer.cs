namespace POS_qu
{
    partial class PaymentModalForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelRoot;
        private Panel panelCard;
        private Label lblTitle;
        private Label lblSubTitle;
        private Label lblTotalCaption;
        private Label lblCustomerCaption;
        private Label lblCustomerValue;
        private Label lblChangeCaption;
        private Label lblChangeValue;

        private Label lblTotal;
        private Label lblPaymentAmount;
        private TextBox txtPaymentAmount;
        private Label lblPaymentMethod;
        private ComboBox cmbPaymentMethod;
        private Button btnPay;

        private Panel panelCardDetails;
        private Panel panelEwalletDetails;
        private Panel panelSplitPayment;
        private Panel panelBankTransfer;

        // panel card
        private Label lblCardNumber;
        private TextBox txtCardNumber;
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
        private RadioButton rdoGlobalPercent;
        private RadioButton rdoGlobalAmount;
        private TextBox txtGlobalDiscountAmount;
        private Panel panelKeypad;

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
            panelRoot = new Panel();
            panelCard = new Panel();
            panelSplitPayment = new Panel();
            lblCashPart = new Label();
            txtCashPart = new TextBox();
            lblCardPart = new Label();
            txtCardPart = new TextBox();
            panelBankTransfer = new Panel();
            lblBankName = new Label();
            cmbBankName = new ComboBox();
            lblTransferRef = new Label();
            txtTransferRef = new TextBox();
            panelEwalletDetails = new Panel();
            lblEwalletProvider = new Label();
            cmbEwalletProvider = new ComboBox();
            lblEwalletRef = new Label();
            txtEwalletRef = new TextBox();
            panelCardDetails = new Panel();
            lblCardNumber = new Label();
            txtCardNumber = new TextBox();
            lblExpiry = new Label();
            txtExpiry = new TextBox();
            panelKeypad = new Panel();
            btnPay = new Button();
            cmbPaymentMethod = new ComboBox();
            lblPaymentMethod = new Label();
            txtPaymentAmount = new TextBox();
            lblPaymentAmount = new Label();
            lblChangeValue = new Label();
            lblChangeCaption = new Label();
            txtGlobalNote = new TextBox();
            txtDeliveryAmount = new TextBox();
            label3 = new Label();
            label2 = new Label();
            txtGlobalDiscountPercent = new TextBox();
            txtGlobalDiscountAmount = new TextBox();
            rdoGlobalAmount = new RadioButton();
            rdoGlobalPercent = new RadioButton();
            lblGlobalDiscount = new Label();
            lblCustomerValue = new Label();
            lblCustomerCaption = new Label();
            lblTotal = new Label();
            lblTotalCaption = new Label();
            lblSubTitle = new Label();
            lblTitle = new Label();
            panelRoot.SuspendLayout();
            panelCard.SuspendLayout();
            panelSplitPayment.SuspendLayout();
            panelBankTransfer.SuspendLayout();
            panelEwalletDetails.SuspendLayout();
            panelCardDetails.SuspendLayout();
            SuspendLayout();
            // 
            // panelRoot
            // 
            panelRoot.BackColor = Color.FromArgb(245, 246, 250);
            panelRoot.Controls.Add(panelCard);
            panelRoot.Dock = DockStyle.Fill;
            panelRoot.Location = new Point(0, 0);
            panelRoot.Name = "panelRoot";
            panelRoot.Padding = new Padding(17, 15, 17, 15);
            panelRoot.Size = new Size(1296, 689);
            panelRoot.TabIndex = 0;
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.Controls.Add(panelSplitPayment);
            panelCard.Controls.Add(panelBankTransfer);
            panelCard.Controls.Add(panelEwalletDetails);
            panelCard.Controls.Add(panelCardDetails);
            panelCard.Controls.Add(panelKeypad);
            panelCard.Controls.Add(btnPay);
            panelCard.Controls.Add(cmbPaymentMethod);
            panelCard.Controls.Add(lblPaymentMethod);
            panelCard.Controls.Add(txtPaymentAmount);
            panelCard.Controls.Add(lblPaymentAmount);
            panelCard.Controls.Add(lblChangeValue);
            panelCard.Controls.Add(lblChangeCaption);
            panelCard.Controls.Add(txtGlobalNote);
            panelCard.Controls.Add(txtDeliveryAmount);
            panelCard.Controls.Add(label3);
            panelCard.Controls.Add(label2);
            panelCard.Controls.Add(txtGlobalDiscountPercent);
            panelCard.Controls.Add(txtGlobalDiscountAmount);
            panelCard.Controls.Add(rdoGlobalAmount);
            panelCard.Controls.Add(rdoGlobalPercent);
            panelCard.Controls.Add(lblGlobalDiscount);
            panelCard.Controls.Add(lblCustomerValue);
            panelCard.Controls.Add(lblCustomerCaption);
            panelCard.Controls.Add(lblTotal);
            panelCard.Controls.Add(lblTotalCaption);
            panelCard.Controls.Add(lblSubTitle);
            panelCard.Controls.Add(lblTitle);
            panelCard.Dock = DockStyle.Fill;
            panelCard.Location = new Point(17, 15);
            panelCard.Name = "panelCard";
            panelCard.Padding = new Padding(17, 15, 17, 15);
            panelCard.Size = new Size(1262, 659);
            panelCard.TabIndex = 0;
            // 
            // panelSplitPayment
            // 
            panelSplitPayment.Controls.Add(lblCashPart);
            panelSplitPayment.Controls.Add(txtCashPart);
            panelSplitPayment.Controls.Add(lblCardPart);
            panelSplitPayment.Controls.Add(txtCardPart);
            panelSplitPayment.Location = new Point(513, 101);
            panelSplitPayment.Name = "panelSplitPayment";
            panelSplitPayment.Size = new Size(315, 111);
            panelSplitPayment.TabIndex = 13;
            panelSplitPayment.Visible = false;
            // 
            // lblCashPart
            // 
            lblCashPart.Location = new Point(9, 8);
            lblCashPart.Name = "lblCashPart";
            lblCashPart.Size = new Size(90, 20);
            lblCashPart.TabIndex = 0;
            lblCashPart.Text = "Cash Amount:";
            // 
            // txtCashPart
            // 
            txtCashPart.Location = new Point(144, 8);
            txtCashPart.Name = "txtCashPart";
            txtCashPart.Size = new Size(90, 29);
            txtCashPart.TabIndex = 1;
            // 
            // lblCardPart
            // 
            lblCardPart.Location = new Point(9, 42);
            lblCardPart.Name = "lblCardPart";
            lblCardPart.Size = new Size(90, 20);
            lblCardPart.TabIndex = 2;
            lblCardPart.Text = "Card Amount:";
            // 
            // txtCardPart
            // 
            txtCardPart.Location = new Point(144, 42);
            txtCardPart.Name = "txtCardPart";
            txtCardPart.Size = new Size(90, 29);
            txtCardPart.TabIndex = 3;
            // 
            // panelBankTransfer
            // 
            panelBankTransfer.Controls.Add(lblBankName);
            panelBankTransfer.Controls.Add(cmbBankName);
            panelBankTransfer.Controls.Add(lblTransferRef);
            panelBankTransfer.Controls.Add(txtTransferRef);
            panelBankTransfer.Location = new Point(513, 101);
            panelBankTransfer.Name = "panelBankTransfer";
            panelBankTransfer.Size = new Size(315, 111);
            panelBankTransfer.TabIndex = 12;
            panelBankTransfer.Visible = false;
            // 
            // lblBankName
            // 
            lblBankName.Location = new Point(9, 8);
            lblBankName.Name = "lblBankName";
            lblBankName.Size = new Size(90, 20);
            lblBankName.TabIndex = 0;
            lblBankName.Text = "Bank Name:";
            // 
            // cmbBankName
            // 
            cmbBankName.Items.AddRange(new object[] { "BCA", "BNI", "BRI", "Mandiri" });
            cmbBankName.Location = new Point(144, 8);
            cmbBankName.Name = "cmbBankName";
            cmbBankName.Size = new Size(109, 29);
            cmbBankName.TabIndex = 1;
            // 
            // lblTransferRef
            // 
            lblTransferRef.Location = new Point(9, 42);
            lblTransferRef.Name = "lblTransferRef";
            lblTransferRef.Size = new Size(90, 20);
            lblTransferRef.TabIndex = 2;
            lblTransferRef.Text = "Reference Number:";
            // 
            // txtTransferRef
            // 
            txtTransferRef.Location = new Point(144, 42);
            txtTransferRef.Name = "txtTransferRef";
            txtTransferRef.Size = new Size(90, 29);
            txtTransferRef.TabIndex = 3;
            // 
            // panelEwalletDetails
            // 
            panelEwalletDetails.Controls.Add(lblEwalletProvider);
            panelEwalletDetails.Controls.Add(cmbEwalletProvider);
            panelEwalletDetails.Controls.Add(lblEwalletRef);
            panelEwalletDetails.Controls.Add(txtEwalletRef);
            panelEwalletDetails.Location = new Point(513, 101);
            panelEwalletDetails.Name = "panelEwalletDetails";
            panelEwalletDetails.Size = new Size(315, 111);
            panelEwalletDetails.TabIndex = 11;
            panelEwalletDetails.Visible = false;
            // 
            // lblEwalletProvider
            // 
            lblEwalletProvider.Location = new Point(9, 8);
            lblEwalletProvider.Name = "lblEwalletProvider";
            lblEwalletProvider.Size = new Size(90, 20);
            lblEwalletProvider.TabIndex = 0;
            lblEwalletProvider.Text = "Provider:";
            // 
            // cmbEwalletProvider
            // 
            cmbEwalletProvider.Items.AddRange(new object[] { "OVO", "GoPay", "ShopeePay" });
            cmbEwalletProvider.Location = new Point(144, 8);
            cmbEwalletProvider.Name = "cmbEwalletProvider";
            cmbEwalletProvider.Size = new Size(109, 29);
            cmbEwalletProvider.TabIndex = 1;
            // 
            // lblEwalletRef
            // 
            lblEwalletRef.Location = new Point(9, 42);
            lblEwalletRef.Name = "lblEwalletRef";
            lblEwalletRef.Size = new Size(90, 20);
            lblEwalletRef.TabIndex = 2;
            lblEwalletRef.Text = "Reference Code:";
            // 
            // txtEwalletRef
            // 
            txtEwalletRef.Location = new Point(144, 42);
            txtEwalletRef.Name = "txtEwalletRef";
            txtEwalletRef.Size = new Size(90, 29);
            txtEwalletRef.TabIndex = 3;
            // 
            // panelCardDetails
            // 
            panelCardDetails.Controls.Add(lblCardNumber);
            panelCardDetails.Controls.Add(txtCardNumber);
            panelCardDetails.Controls.Add(lblExpiry);
            panelCardDetails.Controls.Add(txtExpiry);
            panelCardDetails.Location = new Point(513, 101);
            panelCardDetails.Name = "panelCardDetails";
            panelCardDetails.Size = new Size(315, 111);
            panelCardDetails.TabIndex = 10;
            panelCardDetails.Visible = false;
            // 
            // lblCardNumber
            // 
            lblCardNumber.Location = new Point(9, 8);
            lblCardNumber.Name = "lblCardNumber";
            lblCardNumber.Size = new Size(90, 20);
            lblCardNumber.TabIndex = 0;
            lblCardNumber.Text = "Card Number:";
            // 
            // txtCardNumber
            // 
            txtCardNumber.Location = new Point(144, 8);
            txtCardNumber.Name = "txtCardNumber";
            txtCardNumber.Size = new Size(202, 29);
            txtCardNumber.TabIndex = 1;
            // 
            // lblExpiry
            // 
            lblExpiry.Location = new Point(8, 48);
            lblExpiry.Name = "lblExpiry";
            lblExpiry.Size = new Size(90, 20);
            lblExpiry.TabIndex = 4;
            lblExpiry.Text = "Expiry Date:";
            // 
            // txtExpiry
            // 
            txtExpiry.Location = new Point(143, 48);
            txtExpiry.Name = "txtExpiry";
            txtExpiry.Size = new Size(211, 29);
            txtExpiry.TabIndex = 5;
            // 
            // panelKeypad
            // 
            panelKeypad.BackColor = Color.FromArgb(245, 246, 250);
            panelKeypad.BorderStyle = BorderStyle.FixedSingle;
            panelKeypad.Dock = DockStyle.Right;
            panelKeypad.Location = new Point(866, 15);
            panelKeypad.Name = "panelKeypad";
            panelKeypad.Padding = new Padding(9, 8, 9, 8);
            panelKeypad.Size = new Size(379, 629);
            panelKeypad.TabIndex = 30;
            // 
            // btnPay
            // 
            btnPay.BackColor = Color.FromArgb(40, 167, 69);
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.FlatStyle = FlatStyle.Flat;
            btnPay.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnPay.ForeColor = Color.White;
            btnPay.Location = new Point(19, 499);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(468, 43);
            btnPay.TabIndex = 30;
            btnPay.Text = "Bayar";
            btnPay.UseVisualStyleBackColor = true;
            btnPay.Click += btnPay_Click;
            // 
            // cmbPaymentMethod
            // 
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMethod.Font = new Font("Segoe UI", 11F);
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Card", "QRIS", "Bank Transfer", "Split Bill" });
            cmbPaymentMethod.Location = new Point(19, 406);
            cmbPaymentMethod.Name = "cmbPaymentMethod";
            cmbPaymentMethod.Size = new Size(468, 28);
            cmbPaymentMethod.TabIndex = 14;
            cmbPaymentMethod.SelectedIndexChanged += cmbPaymentMethod_SelectedIndexChanged;
            // 
            // lblPaymentMethod
            // 
            lblPaymentMethod.AutoSize = true;
            lblPaymentMethod.Font = new Font("Segoe UI", 11F);
            lblPaymentMethod.Location = new Point(19, 381);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new Size(102, 20);
            lblPaymentMethod.TabIndex = 8;
            lblPaymentMethod.Text = "Metode Bayar";
            // 
            // txtPaymentAmount
            // 
            txtPaymentAmount.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            txtPaymentAmount.Location = new Point(19, 280);
            txtPaymentAmount.Name = "txtPaymentAmount";
            txtPaymentAmount.Size = new Size(468, 32);
            txtPaymentAmount.TabIndex = 7;
            txtPaymentAmount.TextAlign = HorizontalAlignment.Right;
            txtPaymentAmount.TextChanged += txtPaymentAmount_TextChanged;
            // 
            // lblPaymentAmount
            // 
            lblPaymentAmount.AutoSize = true;
            lblPaymentAmount.Font = new Font("Segoe UI", 11F);
            lblPaymentAmount.Location = new Point(19, 255);
            lblPaymentAmount.Name = "lblPaymentAmount";
            lblPaymentAmount.Size = new Size(46, 20);
            lblPaymentAmount.TabIndex = 6;
            lblPaymentAmount.Text = "Bayar";
            // 
            // lblChangeValue
            // 
            lblChangeValue.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblChangeValue.ForeColor = Color.FromArgb(40, 167, 69);
            lblChangeValue.Location = new Point(19, 343);
            lblChangeValue.Name = "lblChangeValue";
            lblChangeValue.Size = new Size(468, 25);
            lblChangeValue.TabIndex = 10;
            lblChangeValue.Text = "Rp 0";
            lblChangeValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblChangeCaption
            // 
            lblChangeCaption.AutoSize = true;
            lblChangeCaption.Font = new Font("Segoe UI", 10F);
            lblChangeCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblChangeCaption.Location = new Point(19, 322);
            lblChangeCaption.Name = "lblChangeCaption";
            lblChangeCaption.Size = new Size(72, 19);
            lblChangeCaption.TabIndex = 9;
            lblChangeCaption.Text = "Kembalian";
            // 
            // txtGlobalNote
            // 
            txtGlobalNote.Location = new Point(513, 402);
            txtGlobalNote.Name = "txtGlobalNote";
            txtGlobalNote.Size = new Size(315, 29);
            txtGlobalNote.TabIndex = 13;
            txtGlobalNote.TextChanged += txtGlobalNote_TextChanged;
            // 
            // txtDeliveryAmount
            // 
            txtDeliveryAmount.Location = new Point(513, 343);
            txtDeliveryAmount.Name = "txtDeliveryAmount";
            txtDeliveryAmount.Size = new Size(315, 29);
            txtDeliveryAmount.TabIndex = 12;
            txtDeliveryAmount.TextChanged += txtDeliveryAmount_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(513, 377);
            label3.Name = "label3";
            label3.Size = new Size(93, 21);
            label3.TabIndex = 14;
            label3.Text = "Global Note";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(513, 318);
            label2.Name = "label2";
            label2.Size = new Size(127, 21);
            label2.TabIndex = 14;
            label2.Text = "Delivery Amount";
            // 
            // txtGlobalDiscountPercent
            // 
            txtGlobalDiscountPercent.Location = new Point(513, 280);
            txtGlobalDiscountPercent.Name = "txtGlobalDiscountPercent";
            txtGlobalDiscountPercent.Size = new Size(315, 29);
            txtGlobalDiscountPercent.TabIndex = 11;
            txtGlobalDiscountPercent.Text = "0";
            txtGlobalDiscountPercent.TextChanged += txtGlobalDiscountPercent_TextChanged;
            // 
            // txtGlobalDiscountAmount
            // 
            txtGlobalDiscountAmount.Location = new Point(513, 280);
            txtGlobalDiscountAmount.Name = "txtGlobalDiscountAmount";
            txtGlobalDiscountAmount.Size = new Size(315, 29);
            txtGlobalDiscountAmount.TabIndex = 24;
            txtGlobalDiscountAmount.Text = "0";
            txtGlobalDiscountAmount.TextChanged += txtGlobalDiscountAmount_TextChanged;
            // 
            // rdoGlobalAmount
            // 
            rdoGlobalAmount.AutoSize = true;
            rdoGlobalAmount.Location = new Point(603, 252);
            rdoGlobalAmount.Name = "rdoGlobalAmount";
            rdoGlobalAmount.Size = new Size(88, 25);
            rdoGlobalAmount.TabIndex = 10;
            rdoGlobalAmount.TabStop = true;
            rdoGlobalAmount.Text = "Nominal";
            rdoGlobalAmount.UseVisualStyleBackColor = true;
            // 
            // rdoGlobalPercent
            // 
            rdoGlobalPercent.AutoSize = true;
            rdoGlobalPercent.Location = new Point(513, 252);
            rdoGlobalPercent.Name = "rdoGlobalPercent";
            rdoGlobalPercent.Size = new Size(79, 25);
            rdoGlobalPercent.TabIndex = 9;
            rdoGlobalPercent.TabStop = true;
            rdoGlobalPercent.Text = "Percent";
            rdoGlobalPercent.UseVisualStyleBackColor = true;
            // 
            // lblGlobalDiscount
            // 
            lblGlobalDiscount.AutoSize = true;
            lblGlobalDiscount.Location = new Point(513, 227);
            lblGlobalDiscount.Name = "lblGlobalDiscount";
            lblGlobalDiscount.Size = new Size(120, 21);
            lblGlobalDiscount.TabIndex = 14;
            lblGlobalDiscount.Text = "Global Discount";
            // 
            // lblCustomerValue
            // 
            lblCustomerValue.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblCustomerValue.ForeColor = Color.FromArgb(51, 51, 51);
            lblCustomerValue.Location = new Point(19, 209);
            lblCustomerValue.Name = "lblCustomerValue";
            lblCustomerValue.Size = new Size(468, 25);
            lblCustomerValue.TabIndex = 5;
            lblCustomerValue.Text = "Umum";
            // 
            // lblCustomerCaption
            // 
            lblCustomerCaption.AutoSize = true;
            lblCustomerCaption.Font = new Font("Segoe UI", 10F);
            lblCustomerCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblCustomerCaption.Location = new Point(19, 185);
            lblCustomerCaption.Name = "lblCustomerCaption";
            lblCustomerCaption.Size = new Size(69, 19);
            lblCustomerCaption.TabIndex = 4;
            lblCustomerCaption.Text = "Customer";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(0, 122, 255);
            lblTotal.Location = new Point(17, 125);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(100, 51);
            lblTotal.TabIndex = 3;
            lblTotal.Text = "Rp 0";
            // 
            // lblTotalCaption
            // 
            lblTotalCaption.AutoSize = true;
            lblTotalCaption.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblTotalCaption.Location = new Point(19, 101);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new Size(50, 19);
            lblTotalCaption.TabIndex = 2;
            lblTotalCaption.Text = "TOTAL";
            // 
            // lblSubTitle
            // 
            lblSubTitle.Font = new Font("Segoe UI", 10F);
            lblSubTitle.ForeColor = Color.FromArgb(90, 90, 90);
            lblSubTitle.Location = new Point(19, 52);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(468, 36);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Pastikan nominal sesuai. Split Bill untuk pembayaran kombinasi.";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(17, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(149, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Pembayaran";
            // 
            // PaymentModalForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1296, 689);
            Controls.Add(panelRoot);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(1082, 612);
            Name = "PaymentModalForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pembayaran";
            Load += PaymentModalForm_Load;
            panelRoot.ResumeLayout(false);
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            panelSplitPayment.ResumeLayout(false);
            panelSplitPayment.PerformLayout();
            panelBankTransfer.ResumeLayout(false);
            panelBankTransfer.PerformLayout();
            panelEwalletDetails.ResumeLayout(false);
            panelEwalletDetails.PerformLayout();
            panelCardDetails.ResumeLayout(false);
            panelCardDetails.PerformLayout();
            ResumeLayout(false);
        }
        private Label label2;
        private Label label3;
        private TextBox txtDeliveryAmount;
        private TextBox txtGlobalNote;
    }
}
