namespace POS_qu
{
    partial class QuantityForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblStockAvailable = new Label();
            lblQuantity = new Label();
            txtQuantity = new TextBox();
            lblUnitVariant = new Label();
            cbUnitVariant = new ComboBox();
            btnSubmit = new Button();
            btnCancel = new Button();
            lblNoUnitVariant = new Label();
            lblConversionRate = new Label();
            lblSellPrice = new Label();
            SuspendLayout();

            // lblStockAvailable
            lblStockAvailable.AutoSize = true;
            lblStockAvailable.Location = new Point(24, 20);
            lblStockAvailable.Name = "lblStockAvailable";
            lblStockAvailable.Size = new Size(131, 25);
            lblStockAvailable.TabIndex = 0;
            lblStockAvailable.Text = "Stock Available: -";

            // lblQuantity
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new Point(24, 60);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(84, 25);
            lblQuantity.TabIndex = 1;
            lblQuantity.Text = "Quantity:";

            // txtQuantity
            txtQuantity.Location = new Point(140, 57);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(150, 31);
            txtQuantity.TabIndex = 2;

            // lblUnitVariant
            lblUnitVariant.AutoSize = true;
            lblUnitVariant.Location = new Point(24, 105);
            lblUnitVariant.Name = "lblUnitVariant";
            lblUnitVariant.Size = new Size(107, 25);
            lblUnitVariant.TabIndex = 3;
            lblUnitVariant.Text = "Unit Variant:";

            // cbUnitVariant
            cbUnitVariant.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUnitVariant.Location = new Point(140, 102);
            cbUnitVariant.Name = "cbUnitVariant";
            cbUnitVariant.Size = new Size(150, 33);
            cbUnitVariant.TabIndex = 4;
            cbUnitVariant.SelectedIndexChanged += cbUnitVariant_SelectedIndexChanged;

            // lblConversionRate
            lblConversionRate.AutoSize = true;
            lblConversionRate.Location = new Point(24, 135);
            lblConversionRate.Name = "lblConversionRate";
            lblConversionRate.Size = new Size(150, 25);
            lblConversionRate.TabIndex = 5;
            lblConversionRate.Text = "Conversion Rate: -";

            // lblSellPrice
            lblSellPrice.AutoSize = true;
            lblSellPrice.Location = new Point(24, 165);
            lblSellPrice.Name = "lblSellPrice";
            lblSellPrice.Size = new Size(100, 25);
            lblSellPrice.TabIndex = 6;
            lblSellPrice.Text = "Sell Price: -";

            // btnSubmit
            btnSubmit.Location = new Point(40, 200);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(100, 34);
            btnSubmit.TabIndex = 7;
            btnSubmit.Text = "OK";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;

            // btnCancel
            btnCancel.Location = new Point(160, 200);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 34);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Batal";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            // lblNoUnitVariant
            lblNoUnitVariant.AutoSize = true;
            lblNoUnitVariant.ForeColor = Color.Red;
            lblNoUnitVariant.Location = new Point(140, 105);
            lblNoUnitVariant.Name = "lblNoUnitVariant";
            lblNoUnitVariant.Size = new Size(172, 25);
            lblNoUnitVariant.TabIndex = 9;
            lblNoUnitVariant.Text = "No unit variants available.";
            lblNoUnitVariant.Visible = false;

            // QuantityForm
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(848, 496);
            Controls.Add(lblNoUnitVariant);
            Controls.Add(lblStockAvailable);
            Controls.Add(lblQuantity);
            Controls.Add(txtQuantity);
            Controls.Add(lblUnitVariant);
            Controls.Add(cbUnitVariant);
            Controls.Add(lblConversionRate);
            Controls.Add(lblSellPrice);
            Controls.Add(btnSubmit);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "QuantityForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Input Quantity";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblStockAvailable;
        private Label lblQuantity;
        private TextBox txtQuantity;
        private Label lblUnitVariant;
        private ComboBox cbUnitVariant;
        private Button btnSubmit;
        private Button btnCancel;
        private Label lblNoUnitVariant;
        private Label lblConversionRate;
        private Label lblSellPrice;
    }
}
