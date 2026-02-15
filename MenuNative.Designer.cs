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
            masterToolStripMenuItem = new ToolStripMenuItem();
            pelangganCustomerToolStripMenuItem = new ToolStripMenuItem();
            supplierToolStripMenuItem = new ToolStripMenuItem();
            kategoriBarangToolStripMenuItem = new ToolStripMenuItem();
            unitSatuanToolStripMenuItem = new ToolStripMenuItem();
            productToolStripMenuItem = new ToolStripMenuItem();
            manajemenProdukToolStripMenuItem = new ToolStripMenuItem();
            casherToolStripMenuItem = new ToolStripMenuItem();
            pembelianToolStripMenuItem = new ToolStripMenuItem();
            daftarPembelianToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            penjualanToolStripMenuItem = new ToolStripMenuItem();
            stockToolStripMenuItem = new ToolStripMenuItem();
            pengeluaranToolStripMenuItem = new ToolStripMenuItem();
            pembayaranToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            logoToolStripMenuItem = new ToolStripMenuItem();
            networkingToolStripMenuItem = new ToolStripMenuItem();
            databaseToolStripMenuItem = new ToolStripMenuItem();
            strukSettingToolStripMenuItem = new ToolStripMenuItem();
            tokoSettingToolStripMenuItem = new ToolStripMenuItem();
            terminalToolStripMenuItem = new ToolStripMenuItem();
            rolesToolStripMenuItem = new ToolStripMenuItem();
            manajemenRolesPermissionsToolStripMenuItem = new ToolStripMenuItem();
            usersToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            licensesToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            panel2 = new Panel();
            copyrightLabel = new Label();
            panel1.SuspendLayout();
            panelWelcome.SuspendLayout();
            menuStrip1.SuspendLayout();
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
            panel1.Size = new Size(1560, 1337);
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { masterToolStripMenuItem, productToolStripMenuItem, casherToolStripMenuItem, pembelianToolStripMenuItem, reportsToolStripMenuItem, settingsToolStripMenuItem, helpToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1882, 33);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // masterToolStripMenuItem
            // 
            masterToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pelangganCustomerToolStripMenuItem, supplierToolStripMenuItem, kategoriBarangToolStripMenuItem, unitSatuanToolStripMenuItem });
            masterToolStripMenuItem.Name = "masterToolStripMenuItem";
            masterToolStripMenuItem.Size = new Size(124, 29);
            masterToolStripMenuItem.Text = "Master Data";
            masterToolStripMenuItem.Click += masterToolStripMenuItem_Click;
            // 
            // pelangganCustomerToolStripMenuItem
            // 
            pelangganCustomerToolStripMenuItem.Name = "pelangganCustomerToolStripMenuItem";
            pelangganCustomerToolStripMenuItem.Size = new Size(280, 34);
            pelangganCustomerToolStripMenuItem.Text = "Pelanggan/Customer";
            pelangganCustomerToolStripMenuItem.Click += pelangganCustomerToolStripMenuItem_Click;
            // 
            // supplierToolStripMenuItem
            // 
            supplierToolStripMenuItem.Name = "supplierToolStripMenuItem";
            supplierToolStripMenuItem.Size = new Size(280, 34);
            supplierToolStripMenuItem.Text = "Supplier";
            supplierToolStripMenuItem.Click += supplierToolStripMenuItem_Click;
            // 
            // kategoriBarangToolStripMenuItem
            // 
            kategoriBarangToolStripMenuItem.Name = "kategoriBarangToolStripMenuItem";
            kategoriBarangToolStripMenuItem.Size = new Size(280, 34);
            kategoriBarangToolStripMenuItem.Text = "Kategori Barang";
            kategoriBarangToolStripMenuItem.Click += kategoriBarangToolStripMenuItem_Click;
            // 
            // unitSatuanToolStripMenuItem
            // 
            unitSatuanToolStripMenuItem.Name = "unitSatuanToolStripMenuItem";
            unitSatuanToolStripMenuItem.Size = new Size(280, 34);
            unitSatuanToolStripMenuItem.Text = "Unit/Satuan";
            unitSatuanToolStripMenuItem.Click += unitSatuanToolStripMenuItem_Click;
            // 
            // productToolStripMenuItem
            // 
            productToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { manajemenProdukToolStripMenuItem });
            productToolStripMenuItem.Name = "productToolStripMenuItem";
            productToolStripMenuItem.Size = new Size(90, 29);
            productToolStripMenuItem.Text = "Product";
            productToolStripMenuItem.Click += produkToolStripMenuItem_Click;
            // 
            // manajemenProdukToolStripMenuItem
            // 
            manajemenProdukToolStripMenuItem.Name = "manajemenProdukToolStripMenuItem";
            manajemenProdukToolStripMenuItem.Size = new Size(268, 34);
            manajemenProdukToolStripMenuItem.Text = "Manajemen Produk";
            manajemenProdukToolStripMenuItem.Click += manajemenProdukToolStripMenuItem_Click;
            // 
            // casherToolStripMenuItem
            // 
            casherToolStripMenuItem.Name = "casherToolStripMenuItem";
            casherToolStripMenuItem.Size = new Size(81, 29);
            casherToolStripMenuItem.Text = "Casher";
            casherToolStripMenuItem.Click += casherToolStripMenuItem_Click;
            // 
            // pembelianToolStripMenuItem
            // 
            pembelianToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { daftarPembelianToolStripMenuItem });
            pembelianToolStripMenuItem.Name = "pembelianToolStripMenuItem";
            pembelianToolStripMenuItem.Size = new Size(109, 29);
            pembelianToolStripMenuItem.Text = "Pembelian";
            pembelianToolStripMenuItem.Click += pembelianToolStripMenuItem_Click;
            // 
            // daftarPembelianToolStripMenuItem
            // 
            daftarPembelianToolStripMenuItem.Name = "daftarPembelianToolStripMenuItem";
            daftarPembelianToolStripMenuItem.Size = new Size(249, 34);
            daftarPembelianToolStripMenuItem.Text = "Daftar Pembelian";
            daftarPembelianToolStripMenuItem.Click += daftarPembelianToolStripMenuItem_Click;
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { penjualanToolStripMenuItem, stockToolStripMenuItem, pengeluaranToolStripMenuItem, pembayaranToolStripMenuItem });
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(89, 29);
            reportsToolStripMenuItem.Text = "Reports";
            // 
            // penjualanToolStripMenuItem
            // 
            penjualanToolStripMenuItem.Name = "penjualanToolStripMenuItem";
            penjualanToolStripMenuItem.Size = new Size(277, 34);
            penjualanToolStripMenuItem.Text = "Penjualan";
            penjualanToolStripMenuItem.Click += penjualanToolStripMenuItem_Click;
            // 
            // stockToolStripMenuItem
            // 
            stockToolStripMenuItem.Name = "stockToolStripMenuItem";
            stockToolStripMenuItem.Size = new Size(277, 34);
            stockToolStripMenuItem.Text = "Stock";
            stockToolStripMenuItem.Click += stockToolStripMenuItem_Click;
            // 
            // pengeluaranToolStripMenuItem
            // 
            pengeluaranToolStripMenuItem.Name = "pengeluaranToolStripMenuItem";
            pengeluaranToolStripMenuItem.Size = new Size(277, 34);
            pengeluaranToolStripMenuItem.Text = "History Activity Logs";
            pengeluaranToolStripMenuItem.Click += pengeluaranToolStripMenuItem_Click;
            // 
            // pembayaranToolStripMenuItem
            // 
            pembayaranToolStripMenuItem.Name = "pembayaranToolStripMenuItem";
            pembayaranToolStripMenuItem.Size = new Size(277, 34);
            pembayaranToolStripMenuItem.Text = "Print Logs";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { logoToolStripMenuItem, networkingToolStripMenuItem, databaseToolStripMenuItem, strukSettingToolStripMenuItem, tokoSettingToolStripMenuItem, terminalToolStripMenuItem, rolesToolStripMenuItem, manajemenRolesPermissionsToolStripMenuItem, usersToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(92, 29);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // logoToolStripMenuItem
            // 
            logoToolStripMenuItem.Name = "logoToolStripMenuItem";
            logoToolStripMenuItem.Size = new Size(351, 34);
            logoToolStripMenuItem.Text = "App Data";
            // 
            // networkingToolStripMenuItem
            // 
            networkingToolStripMenuItem.Name = "networkingToolStripMenuItem";
            networkingToolStripMenuItem.Size = new Size(351, 34);
            networkingToolStripMenuItem.Text = "Printer";
            // 
            // databaseToolStripMenuItem
            // 
            databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            databaseToolStripMenuItem.Size = new Size(351, 34);
            databaseToolStripMenuItem.Text = "Database";
            // 
            // strukSettingToolStripMenuItem
            // 
            strukSettingToolStripMenuItem.Name = "strukSettingToolStripMenuItem";
            strukSettingToolStripMenuItem.Size = new Size(351, 34);
            strukSettingToolStripMenuItem.Text = "Struk Setting";
            strukSettingToolStripMenuItem.Click += strukSettingToolStripMenuItem_Click;
            // 
            // tokoSettingToolStripMenuItem
            // 
            tokoSettingToolStripMenuItem.Name = "tokoSettingToolStripMenuItem";
            tokoSettingToolStripMenuItem.Size = new Size(351, 34);
            tokoSettingToolStripMenuItem.Text = "Toko Setting";
            tokoSettingToolStripMenuItem.Click += tokoSettingToolStripMenuItem_Click;
            // 
            // terminalToolStripMenuItem
            // 
            terminalToolStripMenuItem.Name = "terminalToolStripMenuItem";
            terminalToolStripMenuItem.Size = new Size(351, 34);
            terminalToolStripMenuItem.Text = "Terminal";
            terminalToolStripMenuItem.Click += terminalToolStripMenuItem_Click;
            // 
            // rolesToolStripMenuItem
            // 
            rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            rolesToolStripMenuItem.Size = new Size(351, 34);
            rolesToolStripMenuItem.Text = "Roles";
            rolesToolStripMenuItem.Click += rolesToolStripMenuItem_Click;
            // 
            // manajemenRolesPermissionsToolStripMenuItem
            // 
            manajemenRolesPermissionsToolStripMenuItem.Name = "manajemenRolesPermissionsToolStripMenuItem";
            manajemenRolesPermissionsToolStripMenuItem.Size = new Size(351, 34);
            manajemenRolesPermissionsToolStripMenuItem.Text = "Manajemen Roles Permissions";
            // 
            // usersToolStripMenuItem
            // 
            usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            usersToolStripMenuItem.Size = new Size(351, 34);
            usersToolStripMenuItem.Text = "Users";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { licensesToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(65, 29);
            helpToolStripMenuItem.Text = "Help";
            // 
            // licensesToolStripMenuItem
            // 
            licensesToolStripMenuItem.Name = "licensesToolStripMenuItem";
            licensesToolStripMenuItem.Size = new Size(270, 34);
            licensesToolStripMenuItem.Text = "Licenses/Activation";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(78, 29);
            aboutToolStripMenuItem.Text = "About";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel2.BackColor = Color.Gainsboro;
            panel2.Location = new Point(1560, 33);
            panel2.Name = "panel2";
            panel2.Size = new Size(322, 910);
            panel2.TabIndex = 0;
            // 
            // copyrightLabel
            // 
            copyrightLabel.Location = new Point(0, 0);
            copyrightLabel.Name = "copyrightLabel";
            copyrightLabel.Size = new Size(100, 23);
            copyrightLabel.TabIndex = 3;
            // 
            // MenuNative
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1882, 1370);
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem masterToolStripMenuItem;
        private ToolStripMenuItem productToolStripMenuItem;
        private ToolStripMenuItem casherToolStripMenuItem;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Panel panel2;
        private ToolStripMenuItem logoToolStripMenuItem;
        private ToolStripMenuItem networkingToolStripMenuItem;
        private ToolStripMenuItem databaseToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Label label1;
        private Label copyrightLabel;
        private Panel panelWelcome;
        private Label label2;
        private ToolStripMenuItem penjualanToolStripMenuItem;
        private ToolStripMenuItem stockToolStripMenuItem;
        private ToolStripMenuItem pengeluaranToolStripMenuItem;
        private ToolStripMenuItem pembayaranToolStripMenuItem;
        private ToolStripMenuItem pembelianToolStripMenuItem;
        private ToolStripMenuItem daftarPembelianToolStripMenuItem;
        private ToolStripMenuItem strukSettingToolStripMenuItem;
        private ToolStripMenuItem tokoSettingToolStripMenuItem;
        private ToolStripMenuItem terminalToolStripMenuItem;
        private ToolStripMenuItem rolesToolStripMenuItem;
        private ToolStripMenuItem manajemenRolesPermissionsToolStripMenuItem;
        private ToolStripMenuItem usersToolStripMenuItem;
        private ToolStripMenuItem licensesToolStripMenuItem;
        private ToolStripMenuItem pelangganCustomerToolStripMenuItem;
        private ToolStripMenuItem supplierToolStripMenuItem;
        private ToolStripMenuItem manajemenProdukToolStripMenuItem;
        private ToolStripMenuItem kategoriBarangToolStripMenuItem;
        private ToolStripMenuItem unitSatuanToolStripMenuItem;
    }
}
