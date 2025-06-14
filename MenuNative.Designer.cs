namespace POSqu_menu
{
    partial class MenuNative
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
            panel1 = new Panel();
            panelWelcome = new Panel();
            label2 = new Label();
            label1 = new Label();
            menuStrip1 = new MenuStrip();
            produkToolStripMenuItem = new ToolStripMenuItem();
            casherToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            logoToolStripMenuItem = new ToolStripMenuItem();
            networkingToolStripMenuItem = new ToolStripMenuItem();
            databaseToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem1 = new ToolStripMenuItem();
            panel2 = new Panel();
            logoPictureBox = new PictureBox();
            copyrightLabel = new Label();
            penjualanToolStripMenuItem = new ToolStripMenuItem();
            stockToolStripMenuItem = new ToolStripMenuItem();
            pengeluaranToolStripMenuItem = new ToolStripMenuItem();
            pembayaranToolStripMenuItem = new ToolStripMenuItem();
            panel1.SuspendLayout();
            panelWelcome.SuspendLayout();
            menuStrip1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonFace;
            panel1.Controls.Add(panelWelcome);
            panel1.Dock = DockStyle.Left;
            panel1.ForeColor = SystemColors.ControlDark;
            panel1.Location = new Point(0, 33);
            panel1.Name = "panel1";
            panel1.Size = new Size(1560, 910);
            panel1.TabIndex = 0;
            // 
            // panelWelcome
            // 
            panelWelcome.BackColor = SystemColors.ControlLight;
            panelWelcome.Controls.Add(label2);
            panelWelcome.Controls.Add(label1);
            panelWelcome.Location = new Point(112, 96);
            panelWelcome.Name = "panelWelcome";
            panelWelcome.Size = new Size(936, 344);
            panelWelcome.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ButtonShadow;
            label2.Location = new Point(64, 104);
            label2.Name = "label2";
            label2.Size = new Size(59, 25);
            label2.TabIndex = 1;
            label2.Text = "label2";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(56, 48);
            label1.Name = "label1";
            label1.Size = new Size(185, 54);
            label1.TabIndex = 0;
            label1.Text = "Welcome";
            label1.Click += label1_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { produkToolStripMenuItem, casherToolStripMenuItem, reportsToolStripMenuItem, settingsToolStripMenuItem, helpToolStripMenuItem, aboutToolStripMenuItem, aboutToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1882, 33);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // produkToolStripMenuItem
            // 
            produkToolStripMenuItem.Name = "produkToolStripMenuItem";
            produkToolStripMenuItem.Size = new Size(124, 29);
            produkToolStripMenuItem.Text = "Master Data";
            produkToolStripMenuItem.Click += produkToolStripMenuItem_Click;
            // 
            // casherToolStripMenuItem
            // 
            casherToolStripMenuItem.Name = "casherToolStripMenuItem";
            casherToolStripMenuItem.Size = new Size(90, 29);
            casherToolStripMenuItem.Text = "Product";
            casherToolStripMenuItem.Click += casherToolStripMenuItem_Click;
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(81, 29);
            reportsToolStripMenuItem.Text = "Casher";
            reportsToolStripMenuItem.Click += reportsToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { penjualanToolStripMenuItem, stockToolStripMenuItem, pengeluaranToolStripMenuItem, pembayaranToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(89, 29);
            settingsToolStripMenuItem.Text = "Reports";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { logoToolStripMenuItem, networkingToolStripMenuItem, databaseToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(92, 29);
            helpToolStripMenuItem.Text = "Settings";
            // 
            // logoToolStripMenuItem
            // 
            logoToolStripMenuItem.Name = "logoToolStripMenuItem";
            logoToolStripMenuItem.Size = new Size(206, 34);
            logoToolStripMenuItem.Text = "App Data";
            // 
            // networkingToolStripMenuItem
            // 
            networkingToolStripMenuItem.Name = "networkingToolStripMenuItem";
            networkingToolStripMenuItem.Size = new Size(206, 34);
            networkingToolStripMenuItem.Text = "Networking";
            // 
            // databaseToolStripMenuItem
            // 
            databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            databaseToolStripMenuItem.Size = new Size(206, 34);
            databaseToolStripMenuItem.Text = "Database";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(65, 29);
            aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            aboutToolStripMenuItem1.Size = new Size(78, 29);
            aboutToolStripMenuItem1.Text = "About";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel2.BackColor = SystemColors.Control;
            panel2.Controls.Add(logoPictureBox);
            panel2.Location = new Point(1560, 33);
            panel2.Name = "panel2";
            panel2.Size = new Size(322, 910);
            panel2.TabIndex = 0;
            // 
            // logoPictureBox
            // 
            logoPictureBox.Location = new Point(200, 100);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(100, 50);
            logoPictureBox.TabIndex = 0;
            logoPictureBox.TabStop = false;
            // 
            // copyrightLabel
            // 
            copyrightLabel.Location = new Point(0, 0);
            copyrightLabel.Name = "copyrightLabel";
            copyrightLabel.Size = new Size(100, 23);
            copyrightLabel.TabIndex = 3;
            // 
            // penjualanToolStripMenuItem
            // 
            penjualanToolStripMenuItem.Name = "penjualanToolStripMenuItem";
            penjualanToolStripMenuItem.Size = new Size(270, 34);
            penjualanToolStripMenuItem.Text = "Penjualan";
            penjualanToolStripMenuItem.Click += penjualanToolStripMenuItem_Click;
            // 
            // stockToolStripMenuItem
            // 
            stockToolStripMenuItem.Name = "stockToolStripMenuItem";
            stockToolStripMenuItem.Size = new Size(270, 34);
            stockToolStripMenuItem.Text = "Stock";
            // 
            // pengeluaranToolStripMenuItem
            // 
            pengeluaranToolStripMenuItem.Name = "pengeluaranToolStripMenuItem";
            pengeluaranToolStripMenuItem.Size = new Size(270, 34);
            pengeluaranToolStripMenuItem.Text = "Pengeluaran";
            // 
            // pembayaranToolStripMenuItem
            // 
            pembayaranToolStripMenuItem.Name = "pembayaranToolStripMenuItem";
            pembayaranToolStripMenuItem.Size = new Size(270, 34);
            pembayaranToolStripMenuItem.Text = "Pembayaran";
            // 
            // MenuNative
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1882, 943);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            Controls.Add(copyrightLabel);
            MainMenuStrip = menuStrip1;
            Name = "MenuNative";
            Text = "MenuNative";
            panel1.ResumeLayout(false);
            panelWelcome.ResumeLayout(false);
            panelWelcome.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem produkToolStripMenuItem;
        private ToolStripMenuItem casherToolStripMenuItem;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Panel panel2;
        private ToolStripMenuItem logoToolStripMenuItem;
        private ToolStripMenuItem networkingToolStripMenuItem;
        private ToolStripMenuItem databaseToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private Label label1;
        private PictureBox logoPictureBox;
        private Label copyrightLabel;
        private Panel panelWelcome;
        private Label label2;
        private ToolStripMenuItem penjualanToolStripMenuItem;
        private ToolStripMenuItem stockToolStripMenuItem;
        private ToolStripMenuItem pengeluaranToolStripMenuItem;
        private ToolStripMenuItem pembayaranToolStripMenuItem;
    }
}
