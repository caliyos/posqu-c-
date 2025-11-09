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
            lblCartSession = new Label();
            BtnToggleInfo = new Button();
            labelSessionInfo = new Label();
            labelCari = new Label();
            txtCariBarang = new TextBox();
            buttonDraft = new Button();
            panelMid = new Panel();
            panel1 = new Panel();
            label1 = new Label();
            labelNumOfItems = new Label();
            flowLayoutPanel = new FlowLayoutPanel();
            dataGridViewCart4 = new DataGridView();
            panelBottom = new Panel();
            lblOrderBadge = new Label();
            labelKembalian = new Label();
            label2 = new Label();
            buttonListOrders = new Button();
            buttonOrders = new Button();
            btnPay = new Button();
            infoPanel = new Panel();
            infoLabel = new Label();
            button1 = new Button();
            panelTop.SuspendLayout();
            panelMid.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart4).BeginInit();
            panelBottom.SuspendLayout();
            infoPanel.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblCartSession);
            panelTop.Controls.Add(BtnToggleInfo);
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
            // lblCartSession
            // 
            lblCartSession.AutoSize = true;
            lblCartSession.Location = new Point(1608, 40);
            lblCartSession.Name = "lblCartSession";
            lblCartSession.Size = new Size(130, 25);
            lblCartSession.TabIndex = 4;
            lblCartSession.Text = "cartsessioninfo";
            // 
            // BtnToggleInfo
            // 
            BtnToggleInfo.BackColor = Color.DeepSkyBlue;
            BtnToggleInfo.Location = new Point(2104, 23);
            BtnToggleInfo.Name = "BtnToggleInfo";
            BtnToggleInfo.Size = new Size(129, 33);
            BtnToggleInfo.TabIndex = 3;
            BtnToggleInfo.Text = "! Info (ctrl+i)";
            BtnToggleInfo.UseVisualStyleBackColor = false;
            BtnToggleInfo.Click += BtnToggleInfo_Click;
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
            // buttonDraft
            // 
            buttonDraft.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDraft.BackColor = Color.FromArgb(0, 123, 255);
            buttonDraft.FlatAppearance.BorderSize = 0;
            buttonDraft.FlatStyle = FlatStyle.Flat;
            buttonDraft.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonDraft.ForeColor = Color.White;
            buttonDraft.Location = new Point(1144, 32);
            buttonDraft.Name = "buttonDraft";
            buttonDraft.Size = new Size(176, 40);
            buttonDraft.TabIndex = 1;
            buttonDraft.Text = "Draft (Ctrl + U)";
            buttonDraft.UseVisualStyleBackColor = false;
            buttonDraft.Click += buttonDraft_Click;
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
            panelMid.Size = new Size(2244, 1170);
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
            panel1.Size = new Size(616, 1130);
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
            flowLayoutPanel.Location = new Point(0, 103);
            flowLayoutPanel.Name = "flowLayoutPanel";
            flowLayoutPanel.Size = new Size(616, 983);
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
            dataGridViewCart4.Size = new Size(1531, 1130);
            dataGridViewCart4.TabIndex = 0;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.FromArgb(245, 245, 245);
            panelBottom.Controls.Add(lblOrderBadge);
            panelBottom.Controls.Add(button1);
            panelBottom.Controls.Add(buttonDraft);
            panelBottom.Controls.Add(labelKembalian);
            panelBottom.Controls.Add(label2);
            panelBottom.Controls.Add(buttonListOrders);
            panelBottom.Controls.Add(buttonOrders);
            panelBottom.Controls.Add(btnPay);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 1250);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(20);
            panelBottom.Size = new Size(2244, 120);
            panelBottom.TabIndex = 1;
            panelBottom.Paint += panelBottom_Paint;
            // 
            // lblOrderBadge
            // 
            lblOrderBadge.AutoSize = true;
            lblOrderBadge.Location = new Point(464, 40);
            lblOrderBadge.Name = "lblOrderBadge";
            lblOrderBadge.Size = new Size(59, 25);
            lblOrderBadge.TabIndex = 0;
            lblOrderBadge.Text = "label3";
            // 
            // labelKembalian
            // 
            labelKembalian.AutoSize = true;
            labelKembalian.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            labelKembalian.Location = new Point(1736, 8);
            labelKembalian.Name = "labelKembalian";
            labelKembalian.Size = new Size(193, 96);
            labelKembalian.TabIndex = 0;
            labelKembalian.Text = "Rp 0";
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
            // buttonListOrders
            // 
            buttonListOrders.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonListOrders.BackColor = Color.FromArgb(0, 123, 255);
            buttonListOrders.FlatAppearance.BorderSize = 0;
            buttonListOrders.FlatStyle = FlatStyle.Flat;
            buttonListOrders.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonListOrders.ForeColor = Color.White;
            buttonListOrders.Location = new Point(937, 32);
            buttonListOrders.Name = "buttonListOrders";
            buttonListOrders.Size = new Size(200, 40);
            buttonListOrders.TabIndex = 1;
            buttonListOrders.Text = "List Orders (Ctrl+L)";
            buttonListOrders.UseVisualStyleBackColor = false;
            buttonListOrders.Click += buttonListOrders_Click;
            // 
            // buttonOrders
            // 
            buttonOrders.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonOrders.BackColor = Color.FromArgb(0, 123, 255);
            buttonOrders.FlatAppearance.BorderSize = 0;
            buttonOrders.FlatStyle = FlatStyle.Flat;
            buttonOrders.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonOrders.ForeColor = Color.White;
            buttonOrders.Location = new Point(752, 32);
            buttonOrders.Name = "buttonOrders";
            buttonOrders.Size = new Size(176, 40);
            buttonOrders.TabIndex = 1;
            buttonOrders.Text = "Orders (Ctrl+O)";
            buttonOrders.UseVisualStyleBackColor = false;
            buttonOrders.Click += btnOrder_Click;
            // 
            // btnPay
            // 
            btnPay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnPay.BackColor = Color.FromArgb(0, 123, 255);
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.FlatStyle = FlatStyle.Flat;
            btnPay.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPay.ForeColor = Color.White;
            btnPay.Location = new Point(560, 32);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(176, 42);
            btnPay.TabIndex = 1;
            btnPay.Text = "Bayar (Ctrl+P)";
            btnPay.UseVisualStyleBackColor = false;
            btnPay.Click += btnPay_Click;
            // 
            // infoPanel
            // 
            infoPanel.Anchor = AnchorStyles.None;
            infoPanel.BackColor = Color.LightYellow;
            infoPanel.BorderStyle = BorderStyle.FixedSingle;
            infoPanel.Controls.Add(infoLabel);
            infoPanel.Location = new Point(289, 473);
            infoPanel.Name = "infoPanel";
            infoPanel.Size = new Size(301, 200);
            infoPanel.TabIndex = 0;
            // 
            // infoLabel
            // 
            infoLabel.Dock = DockStyle.Fill;
            infoLabel.Font = new Font("Segoe UI", 10F);
            infoLabel.Location = new Point(0, 0);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new Size(299, 198);
            infoLabel.TabIndex = 0;
            infoLabel.Text = "Shortcut List:\nF12     - Bayar\nCtrl+P  - Print\nCtrl+N  - Add Item\nCtrl+E  - Edit Item\nDel     - Delete Item\nF5      - Refresh Data\n";
            infoLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.BackColor = Color.FromArgb(0, 123, 255);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(1328, 32);
            button1.Name = "button1";
            button1.Size = new Size(176, 40);
            button1.TabIndex = 1;
            button1.Text = "List Draft";
            button1.UseVisualStyleBackColor = false;
            button1.Click += buttonListDraft_Click;
            // 
            // Casher_POS
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2244, 1370);
            Controls.Add(infoPanel);
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
            infoPanel.ResumeLayout(false);
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
        private Panel infoPanel;
        private Label infoLabel;
        private Button BtnToggleInfo;
        private Button buttonOrders;
        private Button buttonListOrders;
        private Label labelKembalian;
        private Button buttonDraft;
        private FlowLayoutPanel flowLayoutPanel;
        private Label lblOrderBadge;
        private Label lblCartSession;
        private Button button1;
    }
}