namespace POS_qu
{
    partial class LicenseActivationForm
    {
        private System.ComponentModel.IContainer components = null;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtLicenseKey;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Label lblDeviceId;
        private System.Windows.Forms.Label lblStatus;
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
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelRight = new System.Windows.Forms.Panel();
            btnClose = new System.Windows.Forms.Button();
            lblStatus = new System.Windows.Forms.Label();
            lblDeviceId = new System.Windows.Forms.Label();
            btnActivate = new System.Windows.Forms.Button();
            txtLicenseKey = new System.Windows.Forms.TextBox();
            lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            panelRight.SuspendLayout();
            SuspendLayout();
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = System.Drawing.Color.White;
            webView.Dock = System.Windows.Forms.DockStyle.Fill;
            webView.Location = new System.Drawing.Point(0, 0);
            webView.Name = "webView";
            webView.Size = new System.Drawing.Size(820, 680);
            webView.TabIndex = 0;
            webView.ZoomFactor = 1D;
            // 
            // panelRight
            // 
            panelRight.BackColor = System.Drawing.Color.White;
            panelRight.Controls.Add(btnClose);
            panelRight.Controls.Add(lblStatus);
            panelRight.Controls.Add(lblDeviceId);
            panelRight.Controls.Add(btnActivate);
            panelRight.Controls.Add(txtLicenseKey);
            panelRight.Controls.Add(lblTitle);
            panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            panelRight.Location = new System.Drawing.Point(820, 0);
            panelRight.Name = "panelRight";
            panelRight.Padding = new System.Windows.Forms.Padding(16);
            panelRight.Size = new System.Drawing.Size(420, 680);
            panelRight.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new System.Drawing.Point(19, 603);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(382, 44);
            btnClose.TabIndex = 5;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // lblStatus
            // 
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblStatus.Location = new System.Drawing.Point(19, 245);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(382, 340);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Status: -";
            // 
            // lblDeviceId
            // 
            lblDeviceId.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblDeviceId.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            lblDeviceId.Location = new System.Drawing.Point(19, 72);
            lblDeviceId.Name = "lblDeviceId";
            lblDeviceId.Size = new System.Drawing.Size(382, 44);
            lblDeviceId.TabIndex = 3;
            lblDeviceId.Text = "Device ID: -";
            // 
            // btnActivate
            // 
            btnActivate.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            btnActivate.FlatAppearance.BorderSize = 0;
            btnActivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnActivate.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnActivate.ForeColor = System.Drawing.Color.White;
            btnActivate.Location = new System.Drawing.Point(19, 188);
            btnActivate.Name = "btnActivate";
            btnActivate.Size = new System.Drawing.Size(382, 44);
            btnActivate.TabIndex = 2;
            btnActivate.Text = "Activate";
            btnActivate.UseVisualStyleBackColor = false;
            // 
            // txtLicenseKey
            // 
            txtLicenseKey.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtLicenseKey.Location = new System.Drawing.Point(19, 136);
            txtLicenseKey.Name = "txtLicenseKey";
            txtLicenseKey.PlaceholderText = "Masukkan license key, contoh: TEST-XXXX-XXXX";
            txtLicenseKey.Size = new System.Drawing.Size(382, 30);
            txtLicenseKey.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(19, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(214, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "License Activation";
            // 
            // LicenseActivationForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(1240, 680);
            Controls.Add(webView);
            Controls.Add(panelRight);
            Name = "LicenseActivationForm";
            Text = "License Activation";
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            panelRight.ResumeLayout(false);
            panelRight.PerformLayout();
            ResumeLayout(false);
        }
    }
}

