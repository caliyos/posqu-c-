namespace POS_qu
{
    partial class DatabaseSetting
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelRoot;
        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblSubHeader;
        private System.Windows.Forms.Label lblConnTitle;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtDb;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnCreateDb;
        private System.Windows.Forms.Button btnResetSchema;
        private System.Windows.Forms.Label lblCurrentDb;
        private System.Windows.Forms.Label lblDbListTitle;
        private System.Windows.Forms.ComboBox cmbDbList;
        private System.Windows.Forms.Button btnRefreshDbList;
        private System.Windows.Forms.Button btnBackupDb;
        private System.Windows.Forms.Button btnRestoreDb;
        private System.Windows.Forms.Label lblSetupTitle;
        private System.Windows.Forms.Button btnRunPhpMigrations;
        private System.Windows.Forms.Button btnRunPhpSeeders;
        private System.Windows.Forms.Button btnSetupAll;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label lblLogTitle;
        private System.Windows.Forms.TextBox txtLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelRoot = new Panel();
            panelCard = new Panel();
            panelRight = new Panel();
            txtLog = new TextBox();
            lblLogTitle = new Label();
            progress = new ProgressBar();
            btnSetupAll = new Button();
            btnRunPhpSeeders = new Button();
            btnRunPhpMigrations = new Button();
            lblSetupTitle = new Label();
            lblSubHeader = new Label();
            lblHeader = new Label();
            panelLeft = new Panel();
            btnResetSchema = new Button();
            btnCreateDb = new Button();
            btnSaveConfig = new Button();
            btnTest = new Button();
            btnRestoreDb = new Button();
            btnBackupDb = new Button();
            btnRefreshDbList = new Button();
            cmbDbList = new ComboBox();
            lblDbListTitle = new Label();
            lblCurrentDb = new Label();
            txtDb = new TextBox();
            txtPass = new TextBox();
            txtUser = new TextBox();
            txtPort = new TextBox();
            txtHost = new TextBox();
            lblDb = new Label();
            lblPass = new Label();
            lblUser = new Label();
            lblPort = new Label();
            lblHost = new Label();
            lblConnTitle = new Label();
            panelRoot.SuspendLayout();
            panelCard.SuspendLayout();
            panelRight.SuspendLayout();
            panelLeft.SuspendLayout();
            SuspendLayout();
            // 
            // panelRoot
            // 
            panelRoot.BackColor = Color.FromArgb(245, 246, 250);
            panelRoot.Controls.Add(panelCard);
            panelRoot.Dock = DockStyle.Fill;
            panelRoot.Location = new Point(0, 0);
            panelRoot.Name = "panelRoot";
            panelRoot.Padding = new Padding(18);
            panelRoot.Size = new Size(1296, 735);
            panelRoot.TabIndex = 0;
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.Controls.Add(panelRight);
            panelCard.Controls.Add(panelLeft);
            panelCard.Dock = DockStyle.Fill;
            panelCard.Location = new Point(18, 18);
            panelCard.Name = "panelCard";
            panelCard.Padding = new Padding(18);
            panelCard.Size = new Size(1260, 699);
            panelCard.TabIndex = 0;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(txtLog);
            panelRight.Controls.Add(lblLogTitle);
            panelRight.Controls.Add(progress);
            panelRight.Controls.Add(btnSetupAll);
            panelRight.Controls.Add(btnRunPhpSeeders);
            panelRight.Controls.Add(btnRunPhpMigrations);
            panelRight.Controls.Add(lblSetupTitle);
            panelRight.Controls.Add(lblSubHeader);
            panelRight.Controls.Add(lblHeader);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(538, 18);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(18, 0, 0, 0);
            panelRight.Size = new Size(704, 663);
            panelRight.TabIndex = 1;
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.Font = new Font("Consolas", 9.5F);
            txtLog.Location = new Point(18, 408);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(668, 237);
            txtLog.TabIndex = 8;
            // 
            // lblLogTitle
            // 
            lblLogTitle.AutoSize = true;
            lblLogTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblLogTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblLogTitle.Location = new Point(18, 372);
            lblLogTitle.Name = "lblLogTitle";
            lblLogTitle.Size = new Size(54, 32);
            lblLogTitle.TabIndex = 7;
            lblLogTitle.Text = "Log";
            // 
            // progress
            // 
            progress.Location = new Point(18, 332);
            progress.Name = "progress";
            progress.Size = new Size(492, 18);
            progress.TabIndex = 6;
            // 
            // btnSetupAll
            // 
            btnSetupAll.BackColor = Color.FromArgb(40, 167, 69);
            btnSetupAll.FlatAppearance.BorderSize = 0;
            btnSetupAll.FlatStyle = FlatStyle.Flat;
            btnSetupAll.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSetupAll.ForeColor = Color.White;
            btnSetupAll.Location = new Point(18, 278);
            btnSetupAll.Name = "btnSetupAll";
            btnSetupAll.Size = new Size(492, 44);
            btnSetupAll.TabIndex = 5;
            btnSetupAll.Text = "Setup Otomatis";
            btnSetupAll.UseVisualStyleBackColor = false;
            // 
            // btnRunPhpSeeders
            // 
            btnRunPhpSeeders.BackColor = Color.FromArgb(0, 120, 215);
            btnRunPhpSeeders.FlatAppearance.BorderSize = 0;
            btnRunPhpSeeders.FlatStyle = FlatStyle.Flat;
            btnRunPhpSeeders.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRunPhpSeeders.ForeColor = Color.White;
            btnRunPhpSeeders.Location = new Point(18, 228);
            btnRunPhpSeeders.Name = "btnRunPhpSeeders";
            btnRunPhpSeeders.Size = new Size(492, 44);
            btnRunPhpSeeders.TabIndex = 4;
            btnRunPhpSeeders.Text = "Run Seeders (PHP)";
            btnRunPhpSeeders.UseVisualStyleBackColor = false;
            // 
            // btnRunPhpMigrations
            // 
            btnRunPhpMigrations.BackColor = Color.FromArgb(0, 120, 215);
            btnRunPhpMigrations.FlatAppearance.BorderSize = 0;
            btnRunPhpMigrations.FlatStyle = FlatStyle.Flat;
            btnRunPhpMigrations.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRunPhpMigrations.ForeColor = Color.White;
            btnRunPhpMigrations.Location = new Point(18, 178);
            btnRunPhpMigrations.Name = "btnRunPhpMigrations";
            btnRunPhpMigrations.Size = new Size(492, 44);
            btnRunPhpMigrations.TabIndex = 3;
            btnRunPhpMigrations.Text = "Run Migrations (PHP)";
            btnRunPhpMigrations.UseVisualStyleBackColor = false;
            // 
            // lblSetupTitle
            // 
            lblSetupTitle.AutoSize = true;
            lblSetupTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblSetupTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblSetupTitle.Location = new Point(18, 136);
            lblSetupTitle.Name = "lblSetupTitle";
            lblSetupTitle.Size = new Size(168, 38);
            lblSetupTitle.TabIndex = 2;
            lblSetupTitle.Text = "Initial Setup";
            // 
            // lblSubHeader
            // 
            lblSubHeader.Font = new Font("Segoe UI", 10F);
            lblSubHeader.ForeColor = Color.FromArgb(90, 90, 90);
            lblSubHeader.Location = new Point(18, 46);
            lblSubHeader.Name = "lblSubHeader";
            lblSubHeader.Size = new Size(492, 74);
            lblSubHeader.TabIndex = 1;
            lblSubHeader.Text = "Atur koneksi database dan jalankan setup awal (migrations + seeders). Form ini tampil saat pertama install aplikasi.";
            // 
            // lblHeader
            // 
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(51, 51, 51);
            lblHeader.Location = new Point(18, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(209, 48);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "POS-qu Init";
            // 
            // panelLeft
            // 
            panelLeft.AutoScroll = true;
            panelLeft.Controls.Add(btnResetSchema);
            panelLeft.Controls.Add(btnCreateDb);
            panelLeft.Controls.Add(btnSaveConfig);
            panelLeft.Controls.Add(btnTest);
            panelLeft.Controls.Add(btnRestoreDb);
            panelLeft.Controls.Add(btnBackupDb);
            panelLeft.Controls.Add(btnRefreshDbList);
            panelLeft.Controls.Add(cmbDbList);
            panelLeft.Controls.Add(lblDbListTitle);
            panelLeft.Controls.Add(lblCurrentDb);
            panelLeft.Controls.Add(txtDb);
            panelLeft.Controls.Add(txtPass);
            panelLeft.Controls.Add(txtUser);
            panelLeft.Controls.Add(txtPort);
            panelLeft.Controls.Add(txtHost);
            panelLeft.Controls.Add(lblDb);
            panelLeft.Controls.Add(lblPass);
            panelLeft.Controls.Add(lblUser);
            panelLeft.Controls.Add(lblPort);
            panelLeft.Controls.Add(lblHost);
            panelLeft.Controls.Add(lblConnTitle);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(18, 18);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(0, 54, 18, 0);
            panelLeft.Size = new Size(520, 663);
            panelLeft.TabIndex = 0;
            // 
            // btnResetSchema
            // 
            btnResetSchema.BackColor = Color.FromArgb(220, 53, 69);
            btnResetSchema.FlatAppearance.BorderSize = 0;
            btnResetSchema.FlatStyle = FlatStyle.Flat;
            btnResetSchema.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnResetSchema.ForeColor = Color.White;
            btnResetSchema.Location = new Point(248, 514);
            btnResetSchema.Name = "btnResetSchema";
            btnResetSchema.Size = new Size(236, 44);
            btnResetSchema.TabIndex = 14;
            btnResetSchema.Text = "Reset Schema";
            btnResetSchema.UseVisualStyleBackColor = false;
            // 
            // btnCreateDb
            // 
            btnCreateDb.BackColor = Color.FromArgb(108, 117, 125);
            btnCreateDb.FlatAppearance.BorderSize = 0;
            btnCreateDb.FlatStyle = FlatStyle.Flat;
            btnCreateDb.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCreateDb.ForeColor = Color.White;
            btnCreateDb.Location = new Point(0, 514);
            btnCreateDb.Name = "btnCreateDb";
            btnCreateDb.Size = new Size(236, 44);
            btnCreateDb.TabIndex = 13;
            btnCreateDb.Text = "Create DB";
            btnCreateDb.UseVisualStyleBackColor = false;
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.BackColor = Color.FromArgb(40, 167, 69);
            btnSaveConfig.FlatAppearance.BorderSize = 0;
            btnSaveConfig.FlatStyle = FlatStyle.Flat;
            btnSaveConfig.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSaveConfig.ForeColor = Color.White;
            btnSaveConfig.Location = new Point(248, 460);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.Size = new Size(236, 44);
            btnSaveConfig.TabIndex = 12;
            btnSaveConfig.Text = "Save Config";
            btnSaveConfig.UseVisualStyleBackColor = false;
            // 
            // btnTest
            // 
            btnTest.BackColor = Color.FromArgb(0, 120, 215);
            btnTest.FlatAppearance.BorderSize = 0;
            btnTest.FlatStyle = FlatStyle.Flat;
            btnTest.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnTest.ForeColor = Color.White;
            btnTest.Location = new Point(0, 460);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(236, 44);
            btnTest.TabIndex = 11;
            btnTest.Text = "Test Connection";
            btnTest.UseVisualStyleBackColor = false;
            // 
            // btnRestoreDb
            // 
            btnRestoreDb.BackColor = Color.FromArgb(255, 193, 7);
            btnRestoreDb.FlatAppearance.BorderSize = 0;
            btnRestoreDb.FlatStyle = FlatStyle.Flat;
            btnRestoreDb.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRestoreDb.ForeColor = Color.FromArgb(33, 37, 41);
            btnRestoreDb.Location = new Point(0, 738);
            btnRestoreDb.Name = "btnRestoreDb";
            btnRestoreDb.Size = new Size(484, 44);
            btnRestoreDb.TabIndex = 17;
            btnRestoreDb.Text = "Import / Restore Database";
            btnRestoreDb.UseVisualStyleBackColor = false;
            // 
            // btnBackupDb
            // 
            btnBackupDb.BackColor = Color.FromArgb(23, 162, 184);
            btnBackupDb.FlatAppearance.BorderSize = 0;
            btnBackupDb.FlatStyle = FlatStyle.Flat;
            btnBackupDb.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnBackupDb.ForeColor = Color.White;
            btnBackupDb.Location = new Point(248, 684);
            btnBackupDb.Name = "btnBackupDb";
            btnBackupDb.Size = new Size(236, 44);
            btnBackupDb.TabIndex = 16;
            btnBackupDb.Text = "Backup Database";
            btnBackupDb.UseVisualStyleBackColor = false;
            // 
            // btnRefreshDbList
            // 
            btnRefreshDbList.BackColor = Color.FromArgb(108, 117, 125);
            btnRefreshDbList.FlatAppearance.BorderSize = 0;
            btnRefreshDbList.FlatStyle = FlatStyle.Flat;
            btnRefreshDbList.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnRefreshDbList.ForeColor = Color.White;
            btnRefreshDbList.Location = new Point(0, 684);
            btnRefreshDbList.Name = "btnRefreshDbList";
            btnRefreshDbList.Size = new Size(236, 44);
            btnRefreshDbList.TabIndex = 15;
            btnRefreshDbList.Text = "Refresh DB List";
            btnRefreshDbList.UseVisualStyleBackColor = false;
            // 
            // cmbDbList
            // 
            cmbDbList.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDbList.Font = new Font("Segoe UI", 11F);
            cmbDbList.FormattingEnabled = true;
            cmbDbList.Location = new Point(0, 638);
            cmbDbList.Name = "cmbDbList";
            cmbDbList.Size = new Size(484, 38);
            cmbDbList.TabIndex = 14;
            // 
            // lblDbListTitle
            // 
            lblDbListTitle.AutoSize = true;
            lblDbListTitle.Font = new Font("Segoe UI", 10F);
            lblDbListTitle.ForeColor = Color.FromArgb(90, 90, 90);
            lblDbListTitle.Location = new Point(0, 608);
            lblDbListTitle.Name = "lblDbListTitle";
            lblDbListTitle.Size = new Size(191, 28);
            lblDbListTitle.TabIndex = 13;
            lblDbListTitle.Text = "Daftar Database (PG)";
            // 
            // lblCurrentDb
            // 
            lblCurrentDb.Font = new Font("Segoe UI", 9F);
            lblCurrentDb.ForeColor = Color.FromArgb(90, 90, 90);
            lblCurrentDb.Location = new Point(0, 40);
            lblCurrentDb.Name = "lblCurrentDb";
            lblCurrentDb.Size = new Size(484, 44);
            lblCurrentDb.TabIndex = 1;
            lblCurrentDb.Text = "Sekarang memakai database: -";
            // 
            // txtDb
            // 
            txtDb.Font = new Font("Segoe UI", 11F);
            txtDb.Location = new Point(0, 404);
            txtDb.Name = "txtDb";
            txtDb.Size = new Size(484, 37);
            txtDb.TabIndex = 10;
            // 
            // txtPass
            // 
            txtPass.Font = new Font("Segoe UI", 11F);
            txtPass.Location = new Point(0, 326);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(484, 37);
            txtPass.TabIndex = 8;
            txtPass.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            txtUser.Font = new Font("Segoe UI", 11F);
            txtUser.Location = new Point(0, 248);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(484, 37);
            txtUser.TabIndex = 6;
            txtUser.Text = "postgres";
            // 
            // txtPort
            // 
            txtPort.Font = new Font("Segoe UI", 11F);
            txtPort.Location = new Point(0, 170);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(484, 37);
            txtPort.TabIndex = 4;
            txtPort.Text = "5432";
            // 
            // txtHost
            // 
            txtHost.Font = new Font("Segoe UI", 11F);
            txtHost.Location = new Point(0, 92);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(484, 37);
            txtHost.TabIndex = 2;
            txtHost.Text = "localhost";
            // 
            // lblDb
            // 
            lblDb.AutoSize = true;
            lblDb.Font = new Font("Segoe UI", 10F);
            lblDb.ForeColor = Color.FromArgb(90, 90, 90);
            lblDb.Location = new Point(0, 374);
            lblDb.Name = "lblDb";
            lblDb.Size = new Size(93, 28);
            lblDb.TabIndex = 9;
            lblDb.Text = "Database";
            // 
            // lblPass
            // 
            lblPass.AutoSize = true;
            lblPass.Font = new Font("Segoe UI", 10F);
            lblPass.ForeColor = Color.FromArgb(90, 90, 90);
            lblPass.Location = new Point(0, 296);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(93, 28);
            lblPass.TabIndex = 7;
            lblPass.Text = "Password";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Font = new Font("Segoe UI", 10F);
            lblUser.ForeColor = Color.FromArgb(90, 90, 90);
            lblUser.Location = new Point(0, 218);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(51, 28);
            lblUser.TabIndex = 5;
            lblUser.Text = "User";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI", 10F);
            lblPort.ForeColor = Color.FromArgb(90, 90, 90);
            lblPort.Location = new Point(0, 140);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(48, 28);
            lblPort.TabIndex = 3;
            lblPort.Text = "Port";
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Font = new Font("Segoe UI", 10F);
            lblHost.ForeColor = Color.FromArgb(90, 90, 90);
            lblHost.Location = new Point(0, 62);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(77, 28);
            lblHost.TabIndex = 1;
            lblHost.Text = "Host/IP";
            // 
            // lblConnTitle
            // 
            lblConnTitle.AutoSize = true;
            lblConnTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblConnTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblConnTitle.Location = new Point(0, 0);
            lblConnTitle.Name = "lblConnTitle";
            lblConnTitle.Size = new Size(243, 38);
            lblConnTitle.TabIndex = 0;
            lblConnTitle.Text = "Koneksi Database";
            // 
            // DatabaseSetting
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1296, 735);
            Controls.Add(panelRoot);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            Name = "DatabaseSetting";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Initial Setup - POS-qu";
            Load += DatabaseSetting_Load;
            panelRoot.ResumeLayout(false);
            panelCard.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            panelRight.PerformLayout();
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            ResumeLayout(false);
        }
    }
}

