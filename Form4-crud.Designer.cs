namespace POS_qu
{
    partial class Form4_crud
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
            panelTop = new Panel();
            labelCari = new Label();
            txtCariBarang = new TextBox();
            panelMid = new Panel();
            dataGridViewCart4 = new DataGridView();
            panelBottom = new Panel();
            label2 = new Label();
            btnPay = new Button();
            panelTop.SuspendLayout();
            panelMid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart4).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(labelCari);
            panelTop.Controls.Add(txtCariBarang);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(20);
            panelTop.Size = new Size(1672, 80);
            panelTop.TabIndex = 2;
            // 
            // labelCari
            // 
            labelCari.AutoSize = true;
            labelCari.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelCari.Location = new Point(20, 20);
            labelCari.Name = "labelCari";
            labelCari.Size = new Size(68, 38);
            labelCari.TabIndex = 0;
            labelCari.Text = "Cari";
            // 
            // txtCariBarang
            // 
            txtCariBarang.Font = new Font("Segoe UI", 14F);
            txtCariBarang.Location = new Point(100, 18);
            txtCariBarang.Name = "txtCariBarang";
            txtCariBarang.Size = new Size(400, 45);
            txtCariBarang.TabIndex = 1;
            // 
            // panelMid
            // 
            panelMid.BackColor = Color.White;
            panelMid.Controls.Add(dataGridViewCart4);
            panelMid.Dock = DockStyle.Fill;
            panelMid.Location = new Point(0, 80);
            panelMid.Name = "panelMid";
            panelMid.Padding = new Padding(20);
            panelMid.Size = new Size(1672, 700);
            panelMid.TabIndex = 0;
            // 
            // dataGridViewCart4
            // 
            dataGridViewCart4.BackgroundColor = Color.White;
            dataGridViewCart4.BorderStyle = BorderStyle.None;
            dataGridViewCart4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCart4.Dock = DockStyle.Fill;
            dataGridViewCart4.Location = new Point(20, 20);
            dataGridViewCart4.Name = "dataGridViewCart4";
            dataGridViewCart4.RowHeadersWidth = 62;
            dataGridViewCart4.Size = new Size(1632, 660);
            dataGridViewCart4.TabIndex = 0;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(245, 245, 245);
            panelBottom.Controls.Add(label2);
            panelBottom.Controls.Add(btnPay);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 780);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(20);
            panelBottom.Size = new Size(1672, 120);
            panelBottom.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            label2.Location = new Point(20, 20);
            label2.Name = "label2";
            label2.Size = new Size(193, 96);
            label2.TabIndex = 0;
            label2.Text = "Rp 0";
            // 
            // btnPay
            // 
            btnPay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPay.BackColor = Color.FromArgb(0, 123, 255);
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.FlatStyle = FlatStyle.Flat;
            btnPay.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnPay.ForeColor = Color.White;
            btnPay.Location = new Point(1672, 30);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(200, 60);
            btnPay.TabIndex = 1;
            btnPay.Text = "Bayar (F12)";
            btnPay.UseVisualStyleBackColor = false;
            btnPay.Click += btnPay_Click;
            // 
            // Form4_crud
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1672, 900);
            Controls.Add(panelMid);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "Form4_crud";
            Text = "Kasir POS";
            WindowState = FormWindowState.Maximized;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelMid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart4).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Label labelCari;
        private TextBox txtCariBarang;
        private Panel panelMid;
        private DataGridView dataGridViewCart4;
        private Panel panelBottom;
        private Label label2;
        private Button btnPay;
    }
}