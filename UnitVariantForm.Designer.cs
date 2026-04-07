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
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblUnitvariant
            // 
            lblUnitvariant.AutoSize = true;
            lblUnitvariant.Location = new Point(19, 45);
            lblUnitvariant.Margin = new Padding(2, 0, 2, 0);
            lblUnitvariant.Name = "lblUnitvariant";
            lblUnitvariant.Size = new Size(17, 20);
            lblUnitvariant.TabIndex = 1;
            lblUnitvariant.Text = "1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(134, 45);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(19, 20);
            label1.TabIndex = 2;
            label1.Text = "=";
            // 
            // txtConvertionRate
            // 
            txtConvertionRate.Location = new Point(166, 45);
            txtConvertionRate.Margin = new Padding(2);
            txtConvertionRate.Name = "txtConvertionRate";
            txtConvertionRate.Size = new Size(78, 27);
            txtConvertionRate.TabIndex = 1;
            txtConvertionRate.KeyPress += TxtConvertionRate_KeyPress;
            // 
            // cmbUnitVariant
            // 
            cmbUnitVariant.FormattingEnabled = true;
            cmbUnitVariant.Location = new Point(51, 45);
            cmbUnitVariant.Margin = new Padding(2);
            cmbUnitVariant.Name = "cmbUnitVariant";
            cmbUnitVariant.Size = new Size(78, 28);
            cmbUnitVariant.TabIndex = 0;
            // 
            // lblConvertionRate
            // 
            lblConvertionRate.AutoSize = true;
            lblConvertionRate.Location = new Point(256, 45);
            lblConvertionRate.Margin = new Padding(2, 0, 2, 0);
            lblConvertionRate.Name = "lblConvertionRate";
            lblConvertionRate.Size = new Size(50, 20);
            lblConvertionRate.TabIndex = 4;
            lblConvertionRate.Text = "label2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 90);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 5;
            label2.Text = "Harga Jual";
            // 
            // txtSellingPrice
            // 
            txtSellingPrice.Location = new Point(122, 90);
            txtSellingPrice.Margin = new Padding(2);
            txtSellingPrice.Name = "txtSellingPrice";
            txtSellingPrice.Size = new Size(121, 27);
            txtSellingPrice.TabIndex = 2;
            // 
            // minStock
            // 
            minStock.AutoSize = true;
            minStock.Location = new Point(307, 90);
            minStock.Margin = new Padding(2, 0, 2, 0);
            minStock.Name = "minStock";
            minStock.Size = new Size(103, 20);
            minStock.TabIndex = 7;
            minStock.Text = "Minimal Stock";
            minStock.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtMinQty
            // 
            txtMinQty.Location = new Point(416, 90);
            txtMinQty.Margin = new Padding(2);
            txtMinQty.Name = "txtMinQty";
            txtMinQty.Size = new Size(121, 27);
            txtMinQty.TabIndex = 3;
            // 
            // flpVariantLog
            // 
            flpVariantLog.AutoScroll = true;
            flpVariantLog.FlowDirection = FlowDirection.TopDown;
            flpVariantLog.Location = new Point(19, 154);
            flpVariantLog.Margin = new Padding(2);
            flpVariantLog.Name = "flpVariantLog";
            flpVariantLog.Size = new Size(1007, 345);
            flpVariantLog.TabIndex = 6;
            flpVariantLog.WrapContents = false;
            // 
            // btnSaveVariant
            // 
            btnSaveVariant.Location = new Point(560, 90);
            btnSaveVariant.Margin = new Padding(2);
            btnSaveVariant.Name = "btnSaveVariant";
            btnSaveVariant.Size = new Size(163, 39);
            btnSaveVariant.TabIndex = 4;
            btnSaveVariant.Text = "Save Variant";
            btnSaveVariant.Click += BtnSaveVariant_Click;
            // 
            // btnDone
            // 
            btnDone.Location = new Point(936, 503);
            btnDone.Margin = new Padding(2);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(90, 27);
            btnDone.TabIndex = 5;
            btnDone.Text = "Simpan";
            btnDone.UseVisualStyleBackColor = true;
            btnDone.Click += btnDone_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(842, 503);
            btnCancel.Margin = new Padding(2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 27);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Batal";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // UnitVariantForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1109, 568);
            Controls.Add(btnCancel);
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
            Margin = new Padding(2);
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
        private Button btnCancel;
    }
}