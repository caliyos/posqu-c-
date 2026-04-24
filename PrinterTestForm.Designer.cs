namespace POS_qu
{
    partial class PrinterTestForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblPrinter;
        private System.Windows.Forms.ComboBox cmbPrinters;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpPaper;
        private System.Windows.Forms.RadioButton rb58;
        private System.Windows.Forms.RadioButton rb80;
        private System.Windows.Forms.Button btnTestPrint;
        private System.Windows.Forms.Button btnTestRaw;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClose;

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
            panelHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblDesc = new System.Windows.Forms.Label();
            lblPrinter = new System.Windows.Forms.Label();
            cmbPrinters = new System.Windows.Forms.ComboBox();
            btnRefresh = new System.Windows.Forms.Button();
            lblStatus = new System.Windows.Forms.Label();
            grpPaper = new System.Windows.Forms.GroupBox();
            rb58 = new System.Windows.Forms.RadioButton();
            rb80 = new System.Windows.Forms.RadioButton();
            btnTestPrint = new System.Windows.Forms.Button();
            btnTestRaw = new System.Windows.Forms.Button();
            txtLog = new System.Windows.Forms.TextBox();
            btnClose = new System.Windows.Forms.Button();
            panelHeader.SuspendLayout();
            grpPaper.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(lblDesc);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(900, 80);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(16, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(244, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Printer Test Connectivity";
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblDesc.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            lblDesc.Location = new System.Drawing.Point(18, 46);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new System.Drawing.Size(547, 23);
            lblDesc.TabIndex = 1;
            lblDesc.Text = "Pilih printer (Epson 58mm / printer lain), cek status, lalu test print.";
            // 
            // lblPrinter
            // 
            lblPrinter.AutoSize = true;
            lblPrinter.Location = new System.Drawing.Point(18, 96);
            lblPrinter.Name = "lblPrinter";
            lblPrinter.Size = new System.Drawing.Size(64, 25);
            lblPrinter.TabIndex = 1;
            lblPrinter.Text = "Printer";
            // 
            // cmbPrinters
            // 
            cmbPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPrinters.FormattingEnabled = true;
            cmbPrinters.Location = new System.Drawing.Point(18, 124);
            cmbPrinters.Name = "cmbPrinters";
            cmbPrinters.Size = new System.Drawing.Size(560, 33);
            cmbPrinters.TabIndex = 2;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.White;
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Location = new System.Drawing.Point(590, 124);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(120, 34);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblStatus.Location = new System.Drawing.Point(18, 166);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(103, 23);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Status: -";
            // 
            // grpPaper
            // 
            grpPaper.BackColor = System.Drawing.Color.White;
            grpPaper.Controls.Add(rb80);
            grpPaper.Controls.Add(rb58);
            grpPaper.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            grpPaper.Location = new System.Drawing.Point(18, 200);
            grpPaper.Name = "grpPaper";
            grpPaper.Size = new System.Drawing.Size(330, 86);
            grpPaper.TabIndex = 5;
            grpPaper.TabStop = false;
            grpPaper.Text = "Paper";
            // 
            // rb58
            // 
            rb58.AutoSize = true;
            rb58.Font = new System.Drawing.Font("Segoe UI", 10F);
            rb58.Location = new System.Drawing.Point(16, 36);
            rb58.Name = "rb58";
            rb58.Size = new System.Drawing.Size(114, 27);
            rb58.TabIndex = 0;
            rb58.TabStop = true;
            rb58.Text = "58mm (EPOS)";
            rb58.UseVisualStyleBackColor = true;
            // 
            // rb80
            // 
            rb80.AutoSize = true;
            rb80.Font = new System.Drawing.Font("Segoe UI", 10F);
            rb80.Location = new System.Drawing.Point(160, 36);
            rb80.Name = "rb80";
            rb80.Size = new System.Drawing.Size(75, 27);
            rb80.TabIndex = 1;
            rb80.TabStop = true;
            rb80.Text = "80mm";
            rb80.UseVisualStyleBackColor = true;
            // 
            // btnTestPrint
            // 
            btnTestPrint.BackColor = System.Drawing.Color.FromArgb(0, 122, 255);
            btnTestPrint.FlatAppearance.BorderSize = 0;
            btnTestPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTestPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnTestPrint.ForeColor = System.Drawing.Color.White;
            btnTestPrint.Location = new System.Drawing.Point(370, 216);
            btnTestPrint.Name = "btnTestPrint";
            btnTestPrint.Size = new System.Drawing.Size(170, 40);
            btnTestPrint.TabIndex = 6;
            btnTestPrint.Text = "Test Print (Driver)";
            btnTestPrint.UseVisualStyleBackColor = false;
            // 
            // btnTestRaw
            // 
            btnTestRaw.BackColor = System.Drawing.Color.White;
            btnTestRaw.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnTestRaw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTestRaw.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnTestRaw.Location = new System.Drawing.Point(550, 216);
            btnTestRaw.Name = "btnTestRaw";
            btnTestRaw.Size = new System.Drawing.Size(170, 40);
            btnTestRaw.TabIndex = 7;
            btnTestRaw.Text = "Test Print (RAW)";
            btnTestRaw.UseVisualStyleBackColor = false;
            // 
            // txtLog
            // 
            txtLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtLog.Location = new System.Drawing.Point(18, 304);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtLog.Size = new System.Drawing.Size(864, 260);
            txtLog.TabIndex = 8;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new System.Drawing.Point(730, 216);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(152, 40);
            btnClose.TabIndex = 9;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // PrinterTestForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(900, 590);
            Controls.Add(btnClose);
            Controls.Add(txtLog);
            Controls.Add(btnTestRaw);
            Controls.Add(btnTestPrint);
            Controls.Add(grpPaper);
            Controls.Add(lblStatus);
            Controls.Add(btnRefresh);
            Controls.Add(cmbPrinters);
            Controls.Add(lblPrinter);
            Controls.Add(panelHeader);
            Name = "PrinterTestForm";
            Text = "Printer";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            grpPaper.ResumeLayout(false);
            grpPaper.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

