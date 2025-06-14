namespace POS_qu
{
    partial class Casher_POS
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
            labelSessionInfo = new Label();
            labelCari = new Label();
            txtCariBarang = new TextBox();
            panelMid = new Panel();
            panel1 = new Panel();
            label1 = new Label();
            labelNumOfItems = new Label();
            flowLayoutPanel = new FlowLayoutPanel();
            dataGridViewCart4 = new DataGridView();
            panelBottom = new Panel();
            label2 = new Label();
            btnPay = new Button();
            panelTop.SuspendLayout();
            panelMid.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart4).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(labelSessionInfo);
            panelTop.Controls.Add(labelCari);
            panelTop.Controls.Add(txtCariBarang);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(20);
            panelTop.Size = new Size(2244, 80);
            panelTop.TabIndex = 2;
            // 
            // labelSessionInfo
            // 
            labelSessionInfo.AutoSize = true;
            labelSessionInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelSessionInfo.ForeColor = Color.Gray;
            labelSessionInfo.Location = new Point(550, 25);
            labelSessionInfo.Name = "labelSessionInfo";
            labelSessionInfo.Size = new Size(333, 32);
            labelSessionInfo.TabIndex = 2;
            labelSessionInfo.Text = "User: - | Terminal: - | Shift: -";
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
            panelMid.Controls.Add(panel1);
            panelMid.Controls.Add(dataGridViewCart4);
            panelMid.Dock = DockStyle.Fill;
            panelMid.Location = new Point(0, 80);
            panelMid.Name = "panelMid";
            panelMid.Padding = new Padding(20);
            panelMid.Size = new Size(2244, 700);
            panelMid.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gainsboro;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(labelNumOfItems);
            panel1.Controls.Add(flowLayoutPanel);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(1608, 20);
            panel1.Name = "panel1";
            panel1.Size = new Size(616, 660);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10F);
            label1.Location = new Point(0, 32);
            label1.Name = "label1";
            label1.Size = new Size(135, 25);
            label1.TabIndex = 3;
            label1.Text = "jumlah barang";
            // 
            // labelNumOfItems
            // 
            labelNumOfItems.AutoSize = true;
            labelNumOfItems.BackColor = Color.IndianRed;
            labelNumOfItems.Font = new Font("Microsoft Sans Serif", 10F);
            labelNumOfItems.ForeColor = SystemColors.ActiveCaptionText;
            labelNumOfItems.Location = new Point(456, 32);
            labelNumOfItems.Name = "labelNumOfItems";
            labelNumOfItems.Size = new Size(17, 25);
            labelNumOfItems.TabIndex = 3;
            labelNumOfItems.Text = ".";
            // 
            // flowLayoutPanel
            // 
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel.Location = new Point(0, 64);
            flowLayoutPanel.Name = "flowLayoutPanel";
            flowLayoutPanel.Size = new Size(616, 596);
            flowLayoutPanel.TabIndex = 4;
            flowLayoutPanel.WrapContents = false;
            // 
            // dataGridViewCart4
            // 
            dataGridViewCart4.BackgroundColor = Color.LightGray;
            dataGridViewCart4.BorderStyle = BorderStyle.None;
            dataGridViewCart4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCart4.Dock = DockStyle.Left;
            dataGridViewCart4.Location = new Point(20, 20);
            dataGridViewCart4.Name = "dataGridViewCart4";
            dataGridViewCart4.RowHeadersWidth = 62;
            dataGridViewCart4.Size = new Size(1532, 660);
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
            panelBottom.Size = new Size(2244, 120);
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
            btnPay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnPay.BackColor = Color.FromArgb(0, 123, 255);
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.FlatStyle = FlatStyle.Flat;
            btnPay.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnPay.ForeColor = Color.White;
            btnPay.Location = new Point(622, 30);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(100, 60);
            btnPay.TabIndex = 1;
            btnPay.Text = "Bayar (F12)";
            btnPay.UseVisualStyleBackColor = false;
            btnPay.Click += btnPay_Click;
            // 
            // Casher_POS
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2244, 900);
            Controls.Add(panelMid);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "Casher_POS";
            Text = "Kasir POS";
            WindowState = FormWindowState.Maximized;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelMid.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private Label labelSessionInfo;
        private Panel panel1;
        private Label labelNumOfItems;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel;
    }
}