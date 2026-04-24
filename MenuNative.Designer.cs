﻿﻿﻿﻿﻿﻿namespace POSqu_menu
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
            label2 = new Label();
            panelWelcome = new Panel();
            btnAdminPending = new Button();
            btnRefreshDashboard = new Button();
            dashboardPanel = new Panel();
            lblOmzetToday = new Label();
            lblOmzetMonth = new Label();
            lblHPPMonth = new Label();
            lblProfitMonth = new Label();
            label1 = new Label();
            menuStrip1 = new MenuStrip();
            masterToolStripMenuItem = new ToolStripMenuItem();
            pelangganCustomerToolStripMenuItem = new ToolStripMenuItem();
            supplierToolStripMenuItem = new ToolStripMenuItem();
            kategoriBarangToolStripMenuItem = new ToolStripMenuItem();
            unitSatuanToolStripMenuItem = new ToolStripMenuItem();
            gudangToolStripMenuItem = new ToolStripMenuItem();
            productToolStripMenuItem = new ToolStripMenuItem();
            manajemenProdukToolStripMenuItem = new ToolStripMenuItem();
            saldoAwalToolStripMenuItem = new ToolStripMenuItem();
            casherToolStripMenuItem = new ToolStripMenuItem();
            daftarTransaksiToolStripMenuItem = new ToolStripMenuItem();
            casherToolStripMenuItem1 = new ToolStripMenuItem();
            pendingTransaksiAdminToolStripMenuItem = new ToolStripMenuItem();
            pembelianToolStripMenuItem = new ToolStripMenuItem();
            pesananPembelianToolStripMenuItem = new ToolStripMenuItem();
            penerimaanBarangToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            penjualanToolStripMenuItem = new ToolStripMenuItem();
            stockToolStripMenuItem = new ToolStripMenuItem();
            pengeluaranToolStripMenuItem = new ToolStripMenuItem();
            pembayaranToolStripMenuItem = new ToolStripMenuItem();
            printingToolStripMenuItem = new ToolStripMenuItem();
            printBarcodeToolStripMenuItem = new ToolStripMenuItem();
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
            copyrightLabel = new Label();
            panel1.SuspendLayout();
            panelWelcome.SuspendLayout();
            dashboardPanel.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();


            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonFace;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(panelWelcome);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Left;
            panel1.ForeColor = SystemColors.ControlDark;
            panel1.Location = new Point(0, 28);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1506, 816);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ButtonShadow;
            label2.Location = new Point(985, 234);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(50, 20);
            label2.TabIndex = 1;
            label2.Text = "label2";
            label2.Click += label2_Click;
            // 
            // panelWelcome
            // 
            panelWelcome.BackColor = SystemColors.ControlLight;
            panelWelcome.Controls.Add(btnAdminPending);
            panelWelcome.Controls.Add(btnRefreshDashboard);
            panelWelcome.Controls.Add(dashboardPanel);
            panelWelcome.Location = new Point(173, 176);
            panelWelcome.Margin = new Padding(2);
            panelWelcome.Name = "panelWelcome";
            panelWelcome.Size = new Size(749, 275);
            panelWelcome.TabIndex = 1;
            // 
            // btnAdminPending
            // 
            btnAdminPending.Location = new Point(480, 12);
            btnAdminPending.Name = "btnAdminPending";
            btnAdminPending.Size = new Size(130, 36);
            btnAdminPending.TabIndex = 4;
            btnAdminPending.Text = "Audit Pending";
            btnAdminPending.UseVisualStyleBackColor = true;
            // 
            // btnRefreshDashboard
            // 
            btnRefreshDashboard.Location = new Point(620, 12);
            btnRefreshDashboard.Name = "btnRefreshDashboard";
            btnRefreshDashboard.Size = new Size(115, 36);
            btnRefreshDashboard.TabIndex = 3;
            btnRefreshDashboard.Text = "Refresh";
            btnRefreshDashboard.UseVisualStyleBackColor = true;
            // 
            // dashboardPanel
            // 
            dashboardPanel.BackColor = SystemColors.ControlLightLight;
            dashboardPanel.Controls.Add(lblOmzetToday);
            dashboardPanel.Controls.Add(lblOmzetMonth);
            dashboardPanel.Controls.Add(lblHPPMonth);
            dashboardPanel.Controls.Add(lblProfitMonth);
            dashboardPanel.Dock = DockStyle.Bottom;
            dashboardPanel.Location = new Point(0, 115);
            dashboardPanel.Name = "dashboardPanel";
            dashboardPanel.Padding = new Padding(10);
            dashboardPanel.Size = new Size(749, 160);
            dashboardPanel.TabIndex = 2;
            // 
            // lblOmzetToday
            // 
            lblOmzetToday.AutoSize = true;
            lblOmzetToday.Location = new Point(20, 20);
            lblOmzetToday.Name = "lblOmzetToday";
            lblOmzetToday.Size = new Size(108, 20);
            lblOmzetToday.TabIndex = 0;
            lblOmzetToday.Text = "Omzet Hari Ini:";
            // 
            // lblOmzetMonth
            // 
            lblOmzetMonth.AutoSize = true;
            lblOmzetMonth.Location = new Point(20, 48);
            lblOmzetMonth.Name = "lblOmzetMonth";
            lblOmzetMonth.Size = new Size(117, 20);
            lblOmzetMonth.TabIndex = 1;
            lblOmzetMonth.Text = "Omzet Bulan Ini:";
            // 
            // lblHPPMonth
            // 
            lblHPPMonth.AutoSize = true;
            lblHPPMonth.Location = new Point(20, 76);
            lblHPPMonth.Name = "lblHPPMonth";
            lblHPPMonth.Size = new Size(96, 20);
            lblHPPMonth.TabIndex = 2;
            lblHPPMonth.Text = "HPP Bulanan:";
            // 
            // lblProfitMonth
            // 
            lblProfitMonth.AutoSize = true;
            lblProfitMonth.Location = new Point(20, 104);
            lblProfitMonth.Name = "lblProfitMonth";
            lblProfitMonth.Size = new Size(88, 20);
            lblProfitMonth.TabIndex = 3;
            lblProfitMonth.Text = "Laba Bersih:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(985, 176);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(160, 46);
            label1.TabIndex = 0;
            label1.Text = "Welcome";
            label1.Click += label1_Click;
            // 
            // menuStrip1
            // 
      
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { masterToolStripMenuItem, productToolStripMenuItem, casherToolStripMenuItem, pembelianToolStripMenuItem, reportsToolStripMenuItem, printingToolStripMenuItem, settingsToolStripMenuItem, helpToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(1506, 28);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            //MessageBox.Show("CTOR 1");
            // 
            // masterToolStripMenuItem
            // 
 
       
            // 
            // pelangganCustomerToolStripMenuItem
            // 
            pelangganCustomerToolStripMenuItem.Name = "pelangganCustomerToolStripMenuItem";
            pelangganCustomerToolStripMenuItem.Size = new Size(230, 26);
            pelangganCustomerToolStripMenuItem.Text = "Pelanggan/Customer";
            pelangganCustomerToolStripMenuItem.Click += pelangganCustomerToolStripMenuItem_Click;
            // 
            // supplierToolStripMenuItem
            // 
            supplierToolStripMenuItem.Name = "supplierToolStripMenuItem";
            supplierToolStripMenuItem.Size = new Size(230, 26);
            supplierToolStripMenuItem.Text = "Supplier";
            supplierToolStripMenuItem.Click += supplierToolStripMenuItem_Click;
            // 
            // kategoriBarangToolStripMenuItem
            // 
            kategoriBarangToolStripMenuItem.Name = "kategoriBarangToolStripMenuItem";
            kategoriBarangToolStripMenuItem.Size = new Size(230, 26);
            kategoriBarangToolStripMenuItem.Text = "Kategori Barang";
            kategoriBarangToolStripMenuItem.Click += kategoriBarangToolStripMenuItem_Click;
            // 
            // unitSatuanToolStripMenuItem
            // 
            unitSatuanToolStripMenuItem.Name = "unitSatuanToolStripMenuItem";
            unitSatuanToolStripMenuItem.Size = new Size(231, 26);
            unitSatuanToolStripMenuItem.Text = "Unit Satuan";
            unitSatuanToolStripMenuItem.Click += unitSatuanToolStripMenuItem_Click;
            // 
            // gudangToolStripMenuItem
            // 
            gudangToolStripMenuItem.Name = "gudangToolStripMenuItem";
            gudangToolStripMenuItem.Size = new Size(231, 26);
            gudangToolStripMenuItem.Text = "Gudang / Warehouse";
            gudangToolStripMenuItem.Click += MasterGudang_Click;
            // 
            // merkToolStripMenuItem
            // 
            merkToolStripMenuItem = new ToolStripMenuItem();
            merkToolStripMenuItem.Name = "merkToolStripMenuItem";
            merkToolStripMenuItem.Size = new Size(231, 26);
            merkToolStripMenuItem.Text = "Merk (Brand)";
            merkToolStripMenuItem.Click += merkToolStripMenuItem_Click;
            // 
            // rakToolStripMenuItem
            // 
            rakToolStripMenuItem = new ToolStripMenuItem();
            rakToolStripMenuItem.Name = "rakToolStripMenuItem";
            rakToolStripMenuItem.Size = new Size(231, 26);
            rakToolStripMenuItem.Text = "Rak (Rack)";
            rakToolStripMenuItem.Click += rakToolStripMenuItem_Click;
            // 
            // productToolStripMenuItem
            // 
            productToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { manajemenProdukToolStripMenuItem, saldoAwalToolStripMenuItem });
            productToolStripMenuItem.Name = "productToolStripMenuItem";
            productToolStripMenuItem.Size = new Size(74, 24);
            productToolStripMenuItem.Text = "Product";
            productToolStripMenuItem.Click += produkToolStripMenuItem_Click;
            // 
            // manajemenProdukToolStripMenuItem
            // 
            manajemenProdukToolStripMenuItem.Name = "manajemenProdukToolStripMenuItem";
            manajemenProdukToolStripMenuItem.Size = new Size(220, 26);
            manajemenProdukToolStripMenuItem.Text = "Manajemen Produk";
            manajemenProdukToolStripMenuItem.Click += manajemenProdukToolStripMenuItem_Click;
            // 
            // saldoAwalToolStripMenuItem
            // 
            saldoAwalToolStripMenuItem.Name = "saldoAwalToolStripMenuItem";
            saldoAwalToolStripMenuItem.Size = new Size(220, 26);
            saldoAwalToolStripMenuItem.Text = "Saldo Awal Stock";
            saldoAwalToolStripMenuItem.Visible = false;
            saldoAwalToolStripMenuItem.Click += saldoAwalToolStripMenuItem_Click;
            // 
            // casherToolStripMenuItem
            // 

            casherToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { daftarTransaksiToolStripMenuItem, casherToolStripMenuItem1, pendingTransaksiAdminToolStripMenuItem });
            casherToolStripMenuItem.Name = "casherToolStripMenuItem";
            casherToolStripMenuItem.Size = new Size(67, 24);
            casherToolStripMenuItem.Text = "Casher";
            casherToolStripMenuItem.Click += casherToolStripMenuItem_Click;
            // 
            // daftarTransaksiToolStripMenuItem
            // 
            daftarTransaksiToolStripMenuItem.Name = "daftarTransaksiToolStripMenuItem";
            daftarTransaksiToolStripMenuItem.Size = new Size(203, 26);
            daftarTransaksiToolStripMenuItem.Text = "Daftar Transaksi";
            daftarTransaksiToolStripMenuItem.Click += daftarTransaksiToolStripMenuItem_Click;
            // 
            // casherToolStripMenuItem1
            // 
            casherToolStripMenuItem1.Name = "casherToolStripMenuItem1";
            casherToolStripMenuItem1.Size = new Size(203, 26);
            casherToolStripMenuItem1.Text = "Casher";
            casherToolStripMenuItem1.Click += casherToolStripMenuItem1_Click;
            // 
            // pendingTransaksiAdminToolStripMenuItem
            // 
            pendingTransaksiAdminToolStripMenuItem.Name = "pendingTransaksiAdminToolStripMenuItem";
            pendingTransaksiAdminToolStripMenuItem.Size = new Size(203, 26);
            pendingTransaksiAdminToolStripMenuItem.Text = "Pending (Admin)";
            // 
            // pembelianToolStripMenuItem
            // 
            pembelianToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pesananPembelianToolStripMenuItem, penerimaanBarangToolStripMenuItem });
            pembelianToolStripMenuItem.Name = "pembelianToolStripMenuItem";
            pembelianToolStripMenuItem.Size = new Size(92, 24);
            pembelianToolStripMenuItem.Text = "Pembelian";
            pembelianToolStripMenuItem.Click += pembelianToolStripMenuItem_Click;
            // 
            // pesananPembelianToolStripMenuItem
            // 
            pesananPembelianToolStripMenuItem.Name = "pesananPembelianToolStripMenuItem";
            pesananPembelianToolStripMenuItem.Size = new Size(270, 34);
            pesananPembelianToolStripMenuItem.Text = "Pesanan Pembelian";
            pesananPembelianToolStripMenuItem.Click += pOToolStripMenuItem_Click;
            // 
            // penerimaanBarangToolStripMenuItem
            // 
            penerimaanBarangToolStripMenuItem.Name = "penerimaanBarangToolStripMenuItem";
            penerimaanBarangToolStripMenuItem.Size = new Size(270, 34);
            penerimaanBarangToolStripMenuItem.Text = "Penerimaan Barang";
            penerimaanBarangToolStripMenuItem.Click += penerimaanBarangToolStripMenuItem_Click;
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { penjualanToolStripMenuItem, stockToolStripMenuItem, pengeluaranToolStripMenuItem, pembayaranToolStripMenuItem });
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(74, 24);
            reportsToolStripMenuItem.Text = "Reports";
            // 
            // printingToolStripMenuItem
            // 
            printingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { printBarcodeToolStripMenuItem });
            printingToolStripMenuItem.Name = "printingToolStripMenuItem";
            printingToolStripMenuItem.Size = new Size(76, 24);
            printingToolStripMenuItem.Text = "Printing";
            // 
            // printBarcodeToolStripMenuItem
            // 
            printBarcodeToolStripMenuItem.Name = "printBarcodeToolStripMenuItem";
            printBarcodeToolStripMenuItem.Size = new Size(189, 26);
            printBarcodeToolStripMenuItem.Text = "Print Barcode";
            printBarcodeToolStripMenuItem.Click += printBarcodeToolStripMenuItem_Click;
            // 
            // penjualanToolStripMenuItem
            // 
            penjualanToolStripMenuItem.Name = "penjualanToolStripMenuItem";
            penjualanToolStripMenuItem.Size = new Size(227, 26);
            penjualanToolStripMenuItem.Text = "Penjualan";
            penjualanToolStripMenuItem.Click += penjualanToolStripMenuItem_Click;
            // 
            // stockToolStripMenuItem
            // 
            stockToolStripMenuItem.Name = "stockToolStripMenuItem";
            stockToolStripMenuItem.Size = new Size(227, 26);
            stockToolStripMenuItem.Text = "Stock";
            stockToolStripMenuItem.Click += stockToolStripMenuItem_Click;
            // 
            // pengeluaranToolStripMenuItem
            // 
            pengeluaranToolStripMenuItem.Name = "pengeluaranToolStripMenuItem";
            pengeluaranToolStripMenuItem.Size = new Size(227, 26);
            pengeluaranToolStripMenuItem.Text = "History Activity Logs";
            pengeluaranToolStripMenuItem.Click += pengeluaranToolStripMenuItem_Click;
            // 
            // pembayaranToolStripMenuItem
            // 
            pembayaranToolStripMenuItem.Name = "pembayaranToolStripMenuItem";
            pembayaranToolStripMenuItem.Size = new Size(227, 26);
            pembayaranToolStripMenuItem.Text = "Print Logs";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { logoToolStripMenuItem, networkingToolStripMenuItem, databaseToolStripMenuItem, strukSettingToolStripMenuItem, tokoSettingToolStripMenuItem, terminalToolStripMenuItem, rolesToolStripMenuItem, manajemenRolesPermissionsToolStripMenuItem, usersToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(76, 24);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // logoToolStripMenuItem
            // 
            logoToolStripMenuItem.Name = "logoToolStripMenuItem";
            logoToolStripMenuItem.Size = new Size(290, 26);
            logoToolStripMenuItem.Text = "App Data";
            // 
            // networkingToolStripMenuItem
            // 
            networkingToolStripMenuItem.Name = "networkingToolStripMenuItem";
            networkingToolStripMenuItem.Size = new Size(290, 26);
            networkingToolStripMenuItem.Text = "Printer";
            // 
            // databaseToolStripMenuItem
            // 
            databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            databaseToolStripMenuItem.Size = new Size(290, 26);
            databaseToolStripMenuItem.Text = "Database";
            databaseToolStripMenuItem.Click += databaseToolStripMenuItem_Click;
            // 
            // strukSettingToolStripMenuItem
            // 
            strukSettingToolStripMenuItem.Name = "strukSettingToolStripMenuItem";
            strukSettingToolStripMenuItem.Size = new Size(290, 26);
            strukSettingToolStripMenuItem.Text = "Struk Setting";
            strukSettingToolStripMenuItem.Click += strukSettingToolStripMenuItem_Click;
            // 
            // tokoSettingToolStripMenuItem
            // 
            tokoSettingToolStripMenuItem.Name = "tokoSettingToolStripMenuItem";
            tokoSettingToolStripMenuItem.Size = new Size(290, 26);
            tokoSettingToolStripMenuItem.Text = "Toko Setting";
            tokoSettingToolStripMenuItem.Click += tokoSettingToolStripMenuItem_Click;
            // 
            // terminalToolStripMenuItem
            // 
            terminalToolStripMenuItem.Name = "terminalToolStripMenuItem";
            terminalToolStripMenuItem.Size = new Size(290, 26);
            terminalToolStripMenuItem.Text = "Terminal";
            terminalToolStripMenuItem.Click += terminalToolStripMenuItem_Click;
            // 
            // rolesToolStripMenuItem
            // 
            rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            rolesToolStripMenuItem.Size = new Size(290, 26);
            rolesToolStripMenuItem.Text = "Roles";
            rolesToolStripMenuItem.Click += rolesToolStripMenuItem_Click;
            // 
            // manajemenRolesPermissionsToolStripMenuItem
            // 
            manajemenRolesPermissionsToolStripMenuItem.Name = "manajemenRolesPermissionsToolStripMenuItem";
            manajemenRolesPermissionsToolStripMenuItem.Size = new Size(290, 26);
            manajemenRolesPermissionsToolStripMenuItem.Text = "Manajemen Roles Permissions";
            // 
            // usersToolStripMenuItem
            // 
            usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            usersToolStripMenuItem.Size = new Size(290, 26);
            usersToolStripMenuItem.Text = "Users";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { licensesToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // licensesToolStripMenuItem
            // 
            licensesToolStripMenuItem.Name = "licensesToolStripMenuItem";
            licensesToolStripMenuItem.Size = new Size(219, 26);
            licensesToolStripMenuItem.Text = "Licenses/Activation";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(64, 24);
            aboutToolStripMenuItem.Text = "About";
            // 
            // copyrightLabel
            // 
            copyrightLabel.Location = new Point(0, 0);
            copyrightLabel.Margin = new Padding(2, 0, 2, 0);
            copyrightLabel.Name = "copyrightLabel";
            copyrightLabel.Size = new Size(80, 18);
            copyrightLabel.TabIndex = 3;
            // 
            // MenuNative
            // 

            masterToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pelangganCustomerToolStripMenuItem, supplierToolStripMenuItem, kategoriBarangToolStripMenuItem, unitSatuanToolStripMenuItem, gudangToolStripMenuItem, merkToolStripMenuItem, rakToolStripMenuItem });
            masterToolStripMenuItem.Name = "masterToolStripMenuItem";
            masterToolStripMenuItem.Size = new Size(104, 24);
            masterToolStripMenuItem.Text = "Master Data";
            masterToolStripMenuItem.Click += masterToolStripMenuItem_Click;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1506, 844);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            Controls.Add(copyrightLabel);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2);
            Name = "MenuNative";
            Text = "MenuNative";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panelWelcome.ResumeLayout(false);
            dashboardPanel.ResumeLayout(false);
            dashboardPanel.PerformLayout();
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
        private ToolStripMenuItem printingToolStripMenuItem;
        private ToolStripMenuItem printBarcodeToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
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
        private ToolStripMenuItem pesananPembelianToolStripMenuItem;
        private ToolStripMenuItem penerimaanBarangToolStripMenuItem;
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
        private ToolStripMenuItem saldoAwalToolStripMenuItem;
        private ToolStripMenuItem kategoriBarangToolStripMenuItem;
        private ToolStripMenuItem unitSatuanToolStripMenuItem;
        private ToolStripMenuItem gudangToolStripMenuItem;
        private ToolStripMenuItem merkToolStripMenuItem;
        private ToolStripMenuItem rakToolStripMenuItem;
        private ToolStripMenuItem daftarTransaksiToolStripMenuItem;
        private ToolStripMenuItem casherToolStripMenuItem1;
        private ToolStripMenuItem pendingTransaksiAdminToolStripMenuItem;
        private Panel dashboardPanel;
        private System.Windows.Forms.Label lblOmzetToday;
        private System.Windows.Forms.Label lblOmzetMonth;
        private System.Windows.Forms.Label lblHPPMonth;
        private System.Windows.Forms.Label lblProfitMonth;
        private Button btnRefreshDashboard;
        private Button btnAdminPending;
    }
}
