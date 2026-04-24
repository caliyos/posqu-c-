namespace POS_qu
{
    partial class TokoSetting
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
            panel1 = new Panel();
            button2 = new Button();
            button1 = new Button();
            pictureBox = new PictureBox();
            richTextBox1 = new RichTextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            chkPKP = new CheckBox();
            numPpnRate = new NumericUpDown();
            txtPurchasePrefix = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPpnRate).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.HighlightText;
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(pictureBox);
            panel1.Controls.Add(txtPurchasePrefix);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(numPpnRate);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(chkPKP);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(richTextBox1);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(24, 40);
            panel1.Name = "panel1";
            panel1.Size = new Size(1024, 720);
            panel1.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new Point(192, 500);
            button2.Name = "button2";
            button2.Size = new Size(112, 34);
            button2.TabIndex = 4;
            button2.Text = "upload";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(760, 640);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 5;
            button1.Text = "Update";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox
            // 
            pictureBox.BackColor = SystemColors.GradientInactiveCaption;
            pictureBox.Location = new Point(336, 500);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(344, 192);
            pictureBox.TabIndex = 1;
            pictureBox.TabStop = false;
            // 
            // chkPKP
            // 
            chkPKP.AutoSize = true;
            chkPKP.Location = new Point(192, 336);
            chkPKP.Name = "chkPKP";
            chkPKP.Size = new Size(172, 29);
            chkPKP.TabIndex = 6;
            chkPKP.Text = "PKP (aktifkan PPN)";
            chkPKP.UseVisualStyleBackColor = true;
            // 
            // numPpnRate
            // 
            numPpnRate.DecimalPlaces = 2;
            numPpnRate.Location = new Point(192, 376);
            numPpnRate.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numPpnRate.Name = "numPpnRate";
            numPpnRate.Size = new Size(112, 31);
            numPpnRate.TabIndex = 7;
            // 
            // txtPurchasePrefix
            // 
            txtPurchasePrefix.Location = new Point(192, 424);
            txtPurchasePrefix.Name = "txtPurchasePrefix";
            txtPurchasePrefix.Size = new Size(168, 31);
            txtPurchasePrefix.TabIndex = 8;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(192, 96);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(672, 152);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(192, 288);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(544, 31);
            textBox2.TabIndex = 3;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(192, 32);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(544, 31);
            textBox1.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 500);
            label4.Name = "label4";
            label4.Size = new Size(53, 25);
            label4.TabIndex = 1;
            label4.Text = "Logo";
            label4.Click += label1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 288);
            label3.Name = "label3";
            label3.Size = new Size(62, 25);
            label3.TabIndex = 1;
            label3.Text = "NPWP";
            label3.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 88);
            label2.Name = "label2";
            label2.Size = new Size(68, 25);
            label2.TabIndex = 1;
            label2.Text = "Alamat";
            label2.Click += label1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 32);
            label1.Name = "label1";
            label1.Size = new Size(102, 25);
            label1.TabIndex = 1;
            label1.Text = "Nama Toko";
            label1.Click += label1_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 336);
            label5.Name = "label5";
            label5.Size = new Size(122, 25);
            label5.TabIndex = 6;
            label5.Text = "Jenis Pedagang";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 376);
            label6.Name = "label6";
            label6.Size = new Size(62, 25);
            label6.TabIndex = 7;
            label6.Text = "PPN %";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 424);
            label7.Name = "label7";
            label7.Size = new Size(162, 25);
            label7.TabIndex = 8;
            label7.Text = "Prefix No Pembelian";
            // 
            // TokoSetting
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1761, 1191);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "TokoSetting";
            Text = "TokoSetting";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPpnRate).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Label label3;
        private Label label2;
        private RichTextBox richTextBox1;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button button1;
        private PictureBox pictureBox;
        private Label label4;
        private Button button2;
        private CheckBox chkPKP;
        private NumericUpDown numPpnRate;
        private TextBox txtPurchasePrefix;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}
