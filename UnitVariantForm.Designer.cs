namespace POS_qu
{
    partial class UnitVariantForm
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
            lblUnitvariant = new Label();
            label1 = new Label();
            txtConvertionRate = new TextBox();
            cmbUnitVariant = new ComboBox();
            lblConvertionRate = new Label();
            label2 = new Label();
            txtSellingPrice = new TextBox();
            minStock = new Label();
            txtMinQty = new TextBox();
            flpVariantLog = new FlowLayoutPanel();
            btnSaveVariant = new Button();
            btnDone = new Button();
            SuspendLayout();
            // 
            // lblUnitvariant
            // 
            lblUnitvariant.AutoSize = true;
            lblUnitvariant.Location = new Point(24, 56);
            lblUnitvariant.Name = "lblUnitvariant";
            lblUnitvariant.Size = new Size(22, 25);
            lblUnitvariant.TabIndex = 1;
            lblUnitvariant.Text = "1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(168, 56);
            label1.Name = "label1";
            label1.Size = new Size(24, 25);
            label1.TabIndex = 2;
            label1.Text = "=";
            // 
            // txtConvertionRate
            // 
            txtConvertionRate.Location = new Point(208, 56);
            txtConvertionRate.Name = "txtConvertionRate";
            txtConvertionRate.Size = new Size(96, 31);
            txtConvertionRate.TabIndex = 1;
            txtConvertionRate.KeyPress += TxtConvertionRate_KeyPress;
            // 
            // cmbUnitVariant
            // 
            cmbUnitVariant.FormattingEnabled = true;
            cmbUnitVariant.Location = new Point(64, 56);
            cmbUnitVariant.Name = "cmbUnitVariant";
            cmbUnitVariant.Size = new Size(96, 33);
            cmbUnitVariant.TabIndex = 0;
            // 
            // lblConvertionRate
            // 
            lblConvertionRate.AutoSize = true;
            lblConvertionRate.Location = new Point(320, 56);
            lblConvertionRate.Name = "lblConvertionRate";
            lblConvertionRate.Size = new Size(59, 25);
            lblConvertionRate.TabIndex = 4;
            lblConvertionRate.Text = "label2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 112);
            label2.Name = "label2";
            label2.Size = new Size(94, 25);
            label2.TabIndex = 5;
            label2.Text = "Harga Jual";
            // 
            // txtSellingPrice
            // 
            txtSellingPrice.Location = new Point(152, 112);
            txtSellingPrice.Name = "txtSellingPrice";
            txtSellingPrice.Size = new Size(150, 31);
            txtSellingPrice.TabIndex = 2;
            // 
            // minStock
            // 
            minStock.AutoSize = true;
            minStock.Location = new Point(384, 112);
            minStock.Name = "minStock";
            minStock.Size = new Size(123, 25);
            minStock.TabIndex = 7;
            minStock.Text = "Minimal Stock";
            minStock.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtMinQty
            // 
            txtMinQty.Location = new Point(520, 112);
            txtMinQty.Name = "txtMinQty";
            txtMinQty.Size = new Size(150, 31);
            txtMinQty.TabIndex = 3;
            // 
            // flpVariantLog
            // 
            flpVariantLog.AutoScroll = true;
            flpVariantLog.FlowDirection = FlowDirection.TopDown;
            flpVariantLog.Location = new Point(24, 192);
            flpVariantLog.Name = "flpVariantLog";
            flpVariantLog.Size = new Size(740, 416);
            flpVariantLog.TabIndex = 6;
            flpVariantLog.WrapContents = false;
            // 
            // btnSaveVariant
            // 
            btnSaveVariant.Location = new Point(672, 112);
            btnSaveVariant.Name = "btnSaveVariant";
            btnSaveVariant.Size = new Size(72, 32);
            btnSaveVariant.TabIndex = 4;
            btnSaveVariant.Text = "Save Variant";
            btnSaveVariant.Click += BtnSaveVariant_Click;
            // 
            // btnDone
            // 
            btnDone.Location = new Point(672, 144);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(112, 34);
            btnDone.TabIndex = 5;
            btnDone.Text = "Done";
            btnDone.UseVisualStyleBackColor = true;
            btnDone.Click += btnDone_Click;
            // 
            // UnitVariantForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(799, 651);
            Controls.Add(btnDone);
            Controls.Add(flpVariantLog);
            Controls.Add(btnSaveVariant);
            Controls.Add(txtMinQty);
            Controls.Add(minStock);
            Controls.Add(txtSellingPrice);
            Controls.Add(label2);
            Controls.Add(lblConvertionRate);
            Controls.Add(cmbUnitVariant);
            Controls.Add(label1);
            Controls.Add(lblUnitvariant);
            Controls.Add(txtConvertionRate);
            Name = "UnitVariantForm";
            Text = "UnitVariantForm";
            Load += UnitVariantForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FlowLayoutPanel flpVariantLog;
        private Label lblUnitvariant;
        private Label label1;
        private TextBox txtConvertionRate;
        private ComboBox cmbUnitVariant;
        private Label lblConvertionRate;
        private Label label2;
        private TextBox txtSellingPrice;
        private Label minStock;
        private TextBox txtMinQty;
        private Button btnSaveVariant;
        private Button btnDone;
    }
}