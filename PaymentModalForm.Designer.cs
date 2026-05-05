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
            lblTitle = new Label();
            lblSubTitle = new Label();
            lblTotalCaption = new Label();
            lblCustomerCaption = new Label();
            lblCustomerValue = new Label();
            lblChangeCaption = new Label();
            lblChangeValue = new Label();
            lblTotal = new Label();
            lblPaymentAmount = new Label();
            txtPaymentAmount = new TextBox();
            lblPaymentMethod = new Label();
            cmbPaymentMethod = new ComboBox();
            btnPay = new Button();
            panelCardDetails = new Panel();
            lblCardNumber = new Label();
            txtCardNumber = new TextBox();
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
            panelRoot.SuspendLayout();
            panelCard.SuspendLayout();
            panelCardDetails.SuspendLayout();
            panelEwalletDetails.SuspendLayout();
            panelBankTransfer.SuspendLayout();
            panelSplitPayment.SuspendLayout();
            SuspendLayout();
            // 
            // panelRoot
            // 
            panelRoot.BackColor = Color.FromArgb(245, 246, 250);
            panelRoot.Controls.Add(panelCard);
            panelRoot.Dock = DockStyle.Fill;
            panelRoot.Location = new Point(0, 0);
            panelRoot.Name = "panelRoot";
            panelRoot.Padding = new Padding(18);
            panelRoot.Size = new Size(980, 620);
            panelRoot.TabIndex = 0;
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.Controls.Add(panelSplitPayment);
            panelCard.Controls.Add(panelBankTransfer);
            panelCard.Controls.Add(panelEwalletDetails);
            panelCard.Controls.Add(panelCardDetails);
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
            panelCard.Controls.Add(lblGlobalDiscount);
            panelCard.Controls.Add(lblCustomerValue);
            panelCard.Controls.Add(lblCustomerCaption);
            panelCard.Controls.Add(lblTotal);
            panelCard.Controls.Add(lblTotalCaption);
            panelCard.Controls.Add(lblSubTitle);
            panelCard.Controls.Add(lblTitle);
            panelCard.Dock = DockStyle.Fill;
            panelCard.Location = new Point(18, 18);
            panelCard.Name = "panelCard";
            panelCard.Padding = new Padding(18);
            panelCard.Size = new Size(944, 584);
            panelCard.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(18, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(173, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Pembayaran";
            // 
            // lblSubTitle
            // 
            lblSubTitle.Font = new Font("Segoe UI", 10F);
            lblSubTitle.ForeColor = Color.FromArgb(90, 90, 90);
            lblSubTitle.Location = new Point(22, 62);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(520, 44);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Pastikan nominal sesuai. Split Bill untuk pembayaran kombinasi.";
            // 
            // lblTotalCaption
            // 
            lblTotalCaption.AutoSize = true;
            lblTotalCaption.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotalCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblTotalCaption.Location = new Point(22, 120);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new Size(58, 23);
            lblTotalCaption.TabIndex = 2;
            lblTotalCaption.Text = "TOTAL";
            // 
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(0, 122, 255);
            lblTotal.Location = new Point(18, 148);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(146, 62);
            lblTotal.TabIndex = 3;
            lblTotal.Text = "Rp 0";
            // 
            // lblCustomerCaption
            // 
            lblCustomerCaption.AutoSize = true;
            lblCustomerCaption.Font = new Font("Segoe UI", 10F);
            lblCustomerCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblCustomerCaption.Location = new Point(22, 220);
            lblCustomerCaption.Name = "lblCustomerCaption";
            lblCustomerCaption.Size = new Size(83, 23);
            lblCustomerCaption.TabIndex = 4;
            lblCustomerCaption.Text = "Customer";
            // 
            // lblCustomerValue
            // 
            lblCustomerValue.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblCustomerValue.ForeColor = Color.FromArgb(51, 51, 51);
            lblCustomerValue.Location = new Point(22, 248);
            lblCustomerValue.Name = "lblCustomerValue";
            lblCustomerValue.Size = new Size(520, 30);
            lblCustomerValue.TabIndex = 5;
            lblCustomerValue.Text = "Umum";
            // 
            // lblChangeCaption
            // 
            lblChangeCaption.AutoSize = true;
            lblChangeCaption.Font = new Font("Segoe UI", 10F);
            lblChangeCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblChangeCaption.Location = new Point(22, 384);
            lblChangeCaption.Name = "lblChangeCaption";
            lblChangeCaption.Size = new Size(85, 23);
            lblChangeCaption.TabIndex = 9;
            lblChangeCaption.Text = "Kembalian";
            // 
            // lblChangeValue
            // 
            lblChangeValue.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblChangeValue.ForeColor = Color.FromArgb(40, 167, 69);
            lblChangeValue.Location = new Point(22, 408);
            lblChangeValue.Name = "lblChangeValue";
            lblChangeValue.Size = new Size(520, 30);
            lblChangeValue.TabIndex = 10;
            lblChangeValue.Text = "Rp 0";
            lblChangeValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblPaymentAmount
            // 
            lblPaymentAmount.AutoSize = true;
            lblPaymentAmount.Font = new Font("Segoe UI", 11F);
            lblPaymentAmount.Location = new Point(22, 304);
            lblPaymentAmount.Name = "lblPaymentAmount";
            lblPaymentAmount.Size = new Size(62, 25);
            lblPaymentAmount.TabIndex = 6;
            lblPaymentAmount.Text = "Bayar";
            // 
            // txtPaymentAmount
            // 
            txtPaymentAmount.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            txtPaymentAmount.Location = new Point(22, 334);
            txtPaymentAmount.Name = "txtPaymentAmount";
            txtPaymentAmount.Size = new Size(520, 39);
            txtPaymentAmount.TabIndex = 7;
            txtPaymentAmount.TextAlign = HorizontalAlignment.Right;
            txtPaymentAmount.TextChanged += txtPaymentAmount_TextChanged;
            // 
            // lblPaymentMethod
            // 
            lblPaymentMethod.AutoSize = true;
            lblPaymentMethod.Font = new Font("Segoe UI", 11F);
            lblPaymentMethod.Location = new Point(22, 454);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new Size(124, 25);
            lblPaymentMethod.TabIndex = 8;
            lblPaymentMethod.Text = "Metode Bayar";
            // 
            // cmbPaymentMethod
            // 
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMethod.Font = new Font("Segoe UI", 11F);
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Card", "QRIS", "Bank Transfer", "Split Bill" });
            cmbPaymentMethod.Location = new Point(22, 484);
            cmbPaymentMethod.Name = "cmbPaymentMethod";
            cmbPaymentMethod.Size = new Size(520, 33);
            cmbPaymentMethod.TabIndex = 9;
            cmbPaymentMethod.SelectedIndexChanged += cmbPaymentMethod_SelectedIndexChanged;
            // 
            // btnPay
            // 
            btnPay.BackColor = Color.FromArgb(40, 167, 69);
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.FlatStyle = FlatStyle.Flat;
            btnPay.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnPay.ForeColor = Color.White;
            btnPay.Location = new Point(22, 524);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(520, 52);
            btnPay.TabIndex = 15;
            btnPay.Text = "Bayar";
            btnPay.UseVisualStyleBackColor = true;
            btnPay.Click += btnPay_Click;
            // 
            // panelCardDetails
            // 
            panelCardDetails.Controls.Add(lblCardNumber);
            panelCardDetails.Controls.Add(txtCardNumber);
            panelCardDetails.Controls.Add(lblExpiry);
            panelCardDetails.Controls.Add(txtExpiry);
            panelCardDetails.Location = new Point(570, 120);
            panelCardDetails.Name = "panelCardDetails";
            panelCardDetails.Size = new Size(350, 132);
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
            txtCardNumber.Size = new Size(224, 31);
            txtCardNumber.TabIndex = 1;
            // 
            // lblExpiry
            // 
            lblExpiry.Location = new Point(8, 56);
            lblExpiry.Name = "lblExpiry";
            lblExpiry.Size = new Size(100, 23);
            lblExpiry.TabIndex = 4;
            lblExpiry.Text = "Expiry Date:";
            // 
            // txtExpiry
            // 
            txtExpiry.Location = new Point(158, 56);
            txtExpiry.Name = "txtExpiry";
            txtExpiry.Size = new Size(234, 31);
            txtExpiry.TabIndex = 5;
            // 
            // panelEwalletDetails
            // 
            panelEwalletDetails.Controls.Add(lblEwalletProvider);
            panelEwalletDetails.Controls.Add(cmbEwalletProvider);
            panelEwalletDetails.Controls.Add(lblEwalletRef);
            panelEwalletDetails.Controls.Add(txtEwalletRef);
            panelEwalletDetails.Location = new Point(570, 120);
            panelEwalletDetails.Name = "panelEwalletDetails";
            panelEwalletDetails.Size = new Size(350, 132);
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
            panelBankTransfer.Location = new Point(570, 120);
            panelBankTransfer.Name = "panelBankTransfer";
            panelBankTransfer.Size = new Size(350, 132);
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
            panelSplitPayment.Location = new Point(570, 120);
            panelSplitPayment.Name = "panelSplitPayment";
            panelSplitPayment.Size = new Size(350, 132);
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
            lblGlobalDiscount.Location = new Point(570, 270);
            lblGlobalDiscount.Name = "lblGlobalDiscount";
            lblGlobalDiscount.Size = new Size(172, 25);
            lblGlobalDiscount.TabIndex = 14;
            lblGlobalDiscount.Text = "Global Discount (%):";
            // 
            // txtGlobalDiscountPercent
            // 
            txtGlobalDiscountPercent.Location = new Point(570, 300);
            txtGlobalDiscountPercent.Name = "txtGlobalDiscountPercent";
            txtGlobalDiscountPercent.Size = new Size(350, 31);
            txtGlobalDiscountPercent.TabIndex = 11;
            txtGlobalDiscountPercent.Text = "0";
            txtGlobalDiscountPercent.TextChanged += txtGlobalDiscountPercent_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(570, 344);
            label2.Name = "label2";
            label2.Size = new Size(145, 25);
            label2.TabIndex = 14;
            label2.Text = "Delivery Amount";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(570, 414);
            label3.Name = "label3";
            label3.Size = new Size(107, 25);
            label3.TabIndex = 14;
            label3.Text = "Global Note";
            // 
            // txtDeliveryAmount
            // 
            txtDeliveryAmount.Location = new Point(570, 374);
            txtDeliveryAmount.Name = "txtDeliveryAmount";
            txtDeliveryAmount.Size = new Size(350, 31);
            txtDeliveryAmount.TabIndex = 12;
            txtDeliveryAmount.TextChanged += txtDeliveryAmount_TextChanged;
            // 
            // txtGlobalNote
            // 
            txtGlobalNote.Location = new Point(570, 444);
            txtGlobalNote.Name = "txtGlobalNote";
            txtGlobalNote.Size = new Size(350, 31);
            txtGlobalNote.TabIndex = 13;
            txtGlobalNote.TextChanged += txtGlobalNote_TextChanged;
            // 
            // PaymentModalForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 620);
            Controls.Add(panelRoot);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimumSize = new Size(980, 620);
            Name = "PaymentModalForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pembayaran";
            Load += PaymentModalForm_Load;
            panelRoot.ResumeLayout(false);
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            panelCardDetails.ResumeLayout(false);
            panelCardDetails.PerformLayout();
            panelEwalletDetails.ResumeLayout(false);
            panelEwalletDetails.PerformLayout();
            panelBankTransfer.ResumeLayout(false);
            panelBankTransfer.PerformLayout();
            panelSplitPayment.ResumeLayout(false);
            panelSplitPayment.PerformLayout();
            ResumeLayout(false);
        }
        private Label label2;
        private Label label3;
        private TextBox txtDeliveryAmount;
        private TextBox txtGlobalNote;
    }
}
