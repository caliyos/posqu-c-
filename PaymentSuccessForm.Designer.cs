namespace POS_qu
{
    partial class PaymentSuccessForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.TableLayoutPanel tableActions;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSavePng;
        private System.Windows.Forms.Button btnSavePdf;
        private System.Windows.Forms.Button btnWaText;
        private System.Windows.Forms.Button btnWaPng;
        private System.Windows.Forms.Button btnEmailText;
        private System.Windows.Forms.Panel panelEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnEmailPng;

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
            panelHeader = new Panel();
            btnDone = new Button();
            lblDesc = new Label();
            lblTitle = new Label();
            panelBody = new Panel();
            panelEmail = new Panel();
            btnEmailPng = new Button();
            btnEmailText = new Button();
            txtEmail = new TextBox();
            tableActions = new TableLayoutPanel();
            btnPrint = new Button();
            btnSavePng = new Button();
            btnSavePdf = new Button();
            btnWaText = new Button();
            btnWaPng = new Button();
            panelHeader.SuspendLayout();
            panelBody.SuspendLayout();
            panelEmail.SuspendLayout();
            tableActions.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(btnDone);
            panelHeader.Controls.Add(lblDesc);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(4, 5, 4, 5);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(26, 30, 26, 30);
            panelHeader.Size = new Size(1229, 183);
            panelHeader.TabIndex = 0;
            // 
            // btnDone
            // 
            btnDone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDone.BackColor = Color.FromArgb(40, 167, 69);
            btnDone.FlatAppearance.BorderSize = 0;
            btnDone.FlatStyle = FlatStyle.Flat;
            btnDone.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDone.ForeColor = Color.White;
            btnDone.Location = new Point(1003, 40);
            btnDone.Margin = new Padding(4, 5, 4, 5);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(200, 73);
            btnDone.TabIndex = 8;
            btnDone.Text = "Selesai";
            btnDone.UseVisualStyleBackColor = false;
            btnDone.Click += btnDone_Click;
            // 
            // lblDesc
            // 
            lblDesc.Dock = DockStyle.Bottom;
            lblDesc.Font = new Font("Segoe UI", 11F);
            lblDesc.ForeColor = Color.FromArgb(90, 90, 90);
            lblDesc.Location = new Point(26, 116);
            lblDesc.Margin = new Padding(4, 0, 4, 0);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(1177, 37);
            lblDesc.TabIndex = 1;
            lblDesc.Text = "Pilih aksi: cetak / simpan / kirim nota.";
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(40, 167, 69);
            lblTitle.Location = new Point(26, 30);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1177, 73);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Pembayaran Berhasil";
            // 
            // panelBody
            // 
            panelBody.BackColor = Color.FromArgb(245, 246, 250);
            panelBody.Controls.Add(panelEmail);
            panelBody.Controls.Add(tableActions);
            panelBody.Dock = DockStyle.Fill;
            panelBody.Location = new Point(0, 183);
            panelBody.Margin = new Padding(4, 5, 4, 5);
            panelBody.Name = "panelBody";
            panelBody.Padding = new Padding(26, 30, 26, 30);
            panelBody.Size = new Size(1229, 750);
            panelBody.TabIndex = 1;
            // 
            // panelEmail
            // 
            panelEmail.BackColor = Color.White;
            panelEmail.Controls.Add(btnEmailPng);
            panelEmail.Controls.Add(btnEmailText);
            panelEmail.Controls.Add(txtEmail);
            panelEmail.Dock = DockStyle.Top;
            panelEmail.Location = new Point(26, 430);
            panelEmail.Margin = new Padding(4, 5, 4, 5);
            panelEmail.Name = "panelEmail";
            panelEmail.Padding = new Padding(17, 20, 17, 20);
            panelEmail.Size = new Size(1177, 107);
            panelEmail.TabIndex = 7;
            // 
            // btnEmailPng
            // 
            btnEmailPng.BackColor = Color.FromArgb(0, 122, 255);
            btnEmailPng.Dock = DockStyle.Right;
            btnEmailPng.FlatAppearance.BorderSize = 0;
            btnEmailPng.FlatStyle = FlatStyle.Flat;
            btnEmailPng.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnEmailPng.ForeColor = Color.White;
            btnEmailPng.Location = new Point(714, 20);
            btnEmailPng.Margin = new Padding(4, 5, 4, 5);
            btnEmailPng.Name = "btnEmailPng";
            btnEmailPng.Size = new Size(223, 67);
            btnEmailPng.TabIndex = 7;
            btnEmailPng.Text = "Email PNG";
            btnEmailPng.UseVisualStyleBackColor = false;
            btnEmailPng.Click += btnEmailPng_Click;
            // 
            // btnEmailText
            // 
            btnEmailText.BackColor = Color.FromArgb(0, 122, 255);
            btnEmailText.Dock = DockStyle.Right;
            btnEmailText.FlatAppearance.BorderSize = 0;
            btnEmailText.FlatStyle = FlatStyle.Flat;
            btnEmailText.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnEmailText.ForeColor = Color.White;
            btnEmailText.Location = new Point(937, 20);
            btnEmailText.Margin = new Padding(4, 5, 4, 5);
            btnEmailText.Name = "btnEmailText";
            btnEmailText.Size = new Size(223, 67);
            btnEmailText.TabIndex = 6;
            btnEmailText.Text = "Email Text";
            btnEmailText.UseVisualStyleBackColor = false;
            btnEmailText.Click += btnEmailText_Click;
            // 
            // txtEmail
            // 
            txtEmail.Dock = DockStyle.Fill;
            txtEmail.Font = new Font("Segoe UI", 12F);
            txtEmail.Location = new Point(17, 20);
            txtEmail.Margin = new Padding(4, 5, 4, 5);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Email tujuan (contoh: kasir@toko.com)";
            txtEmail.Size = new Size(1143, 39);
            txtEmail.TabIndex = 5;
            // 
            // tableActions
            // 
            tableActions.BackColor = Color.White;
            tableActions.ColumnCount = 2;
            tableActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableActions.Controls.Add(btnPrint, 0, 0);
            tableActions.Controls.Add(btnSavePng, 1, 0);
            tableActions.Controls.Add(btnSavePdf, 0, 1);
            tableActions.Controls.Add(btnWaText, 1, 1);
            tableActions.Controls.Add(btnWaPng, 0, 2);
            tableActions.Dock = DockStyle.Top;
            tableActions.Location = new Point(26, 30);
            tableActions.Margin = new Padding(4, 5, 4, 5);
            tableActions.Name = "tableActions";
            tableActions.Padding = new Padding(14, 17, 14, 17);
            tableActions.RowCount = 3;
            tableActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableActions.Size = new Size(1177, 400);
            tableActions.TabIndex = 2;
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.FromArgb(0, 122, 255);
            btnPrint.Dock = DockStyle.Fill;
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            btnPrint.ForeColor = Color.White;
            btnPrint.Location = new Point(25, 30);
            btnPrint.Margin = new Padding(11, 13, 11, 13);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(552, 95);
            btnPrint.TabIndex = 0;
            btnPrint.Text = "Print Nota";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            // 
            // btnSavePng
            // 
            btnSavePng.BackColor = Color.FromArgb(108, 117, 125);
            btnSavePng.Dock = DockStyle.Fill;
            btnSavePng.FlatAppearance.BorderSize = 0;
            btnSavePng.FlatStyle = FlatStyle.Flat;
            btnSavePng.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            btnSavePng.ForeColor = Color.White;
            btnSavePng.Location = new Point(599, 30);
            btnSavePng.Margin = new Padding(11, 13, 11, 13);
            btnSavePng.Name = "btnSavePng";
            btnSavePng.Size = new Size(553, 95);
            btnSavePng.TabIndex = 1;
            btnSavePng.Text = "Save PNG";
            btnSavePng.UseVisualStyleBackColor = false;
            btnSavePng.Click += btnSavePng_Click;
            // 
            // btnSavePdf
            // 
            btnSavePdf.BackColor = Color.FromArgb(108, 117, 125);
            btnSavePdf.Dock = DockStyle.Fill;
            btnSavePdf.FlatAppearance.BorderSize = 0;
            btnSavePdf.FlatStyle = FlatStyle.Flat;
            btnSavePdf.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            btnSavePdf.ForeColor = Color.White;
            btnSavePdf.Location = new Point(25, 151);
            btnSavePdf.Margin = new Padding(11, 13, 11, 13);
            btnSavePdf.Name = "btnSavePdf";
            btnSavePdf.Size = new Size(552, 95);
            btnSavePdf.TabIndex = 2;
            btnSavePdf.Text = "Save PDF";
            btnSavePdf.UseVisualStyleBackColor = false;
            btnSavePdf.Click += btnSavePdf_Click;
            // 
            // btnWaText
            // 
            btnWaText.BackColor = Color.FromArgb(37, 211, 102);
            btnWaText.Dock = DockStyle.Fill;
            btnWaText.FlatAppearance.BorderSize = 0;
            btnWaText.FlatStyle = FlatStyle.Flat;
            btnWaText.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            btnWaText.ForeColor = Color.White;
            btnWaText.Location = new Point(599, 151);
            btnWaText.Margin = new Padding(11, 13, 11, 13);
            btnWaText.Name = "btnWaText";
            btnWaText.Size = new Size(553, 95);
            btnWaText.TabIndex = 3;
            btnWaText.Text = "WA (Text)";
            btnWaText.UseVisualStyleBackColor = false;
            btnWaText.Click += btnWaText_Click;
            // 
            // btnWaPng
            // 
            btnWaPng.BackColor = Color.FromArgb(37, 211, 102);
            btnWaPng.Dock = DockStyle.Fill;
            btnWaPng.FlatAppearance.BorderSize = 0;
            btnWaPng.FlatStyle = FlatStyle.Flat;
            btnWaPng.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            btnWaPng.ForeColor = Color.White;
            btnWaPng.Location = new Point(25, 272);
            btnWaPng.Margin = new Padding(11, 13, 11, 13);
            btnWaPng.Name = "btnWaPng";
            btnWaPng.Size = new Size(552, 98);
            btnWaPng.TabIndex = 4;
            btnWaPng.Text = "WA (PNG)";
            btnWaPng.UseVisualStyleBackColor = false;
            btnWaPng.Click += btnWaPng_Click;
            // 
            // PaymentSuccessForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(1229, 933);
            Controls.Add(panelBody);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PaymentSuccessForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pembayaran Berhasil";
            panelHeader.ResumeLayout(false);
            panelBody.ResumeLayout(false);
            panelEmail.ResumeLayout(false);
            panelEmail.PerformLayout();
            tableActions.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}

