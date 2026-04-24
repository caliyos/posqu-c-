namespace POS_qu
{
    partial class HppMethodSettingForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpMethods;
        private System.Windows.Forms.RadioButton rbFifo;
        private System.Windows.Forms.RadioButton rbAvg;
        private System.Windows.Forms.RadioButton rbLifo;
        private System.Windows.Forms.RadioButton rbFefo;
        private System.Windows.Forms.Button btnSave;
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
            grpMethods = new System.Windows.Forms.GroupBox();
            rbFifo = new System.Windows.Forms.RadioButton();
            rbAvg = new System.Windows.Forms.RadioButton();
            rbLifo = new System.Windows.Forms.RadioButton();
            rbFefo = new System.Windows.Forms.RadioButton();
            btnSave = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            panelHeader.SuspendLayout();
            grpMethods.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(640, 70);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(16, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(274, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Master HPP / Valuation";
            // 
            // grpMethods
            // 
            grpMethods.BackColor = System.Drawing.Color.White;
            grpMethods.Controls.Add(rbFefo);
            grpMethods.Controls.Add(rbLifo);
            grpMethods.Controls.Add(rbAvg);
            grpMethods.Controls.Add(rbFifo);
            grpMethods.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            grpMethods.Location = new System.Drawing.Point(16, 90);
            grpMethods.Name = "grpMethods";
            grpMethods.Size = new System.Drawing.Size(608, 170);
            grpMethods.TabIndex = 1;
            grpMethods.TabStop = false;
            grpMethods.Text = "Default HPP Method (untuk item baru)";
            // 
            // rbFifo
            // 
            rbFifo.AutoSize = true;
            rbFifo.Font = new System.Drawing.Font("Segoe UI", 10F);
            rbFifo.Location = new System.Drawing.Point(18, 40);
            rbFifo.Name = "rbFifo";
            rbFifo.Size = new System.Drawing.Size(213, 27);
            rbFifo.TabIndex = 0;
            rbFifo.TabStop = true;
            rbFifo.Text = "FIFO (Urutan Masuk)";
            rbFifo.UseVisualStyleBackColor = true;
            // 
            // rbAvg
            // 
            rbAvg.AutoSize = true;
            rbAvg.Font = new System.Drawing.Font("Segoe UI", 10F);
            rbAvg.Location = new System.Drawing.Point(18, 74);
            rbAvg.Name = "rbAvg";
            rbAvg.Size = new System.Drawing.Size(240, 27);
            rbAvg.TabIndex = 1;
            rbAvg.TabStop = true;
            rbAvg.Text = "AVG (Rata-rata Harga)";
            rbAvg.UseVisualStyleBackColor = true;
            // 
            // rbLifo
            // 
            rbLifo.AutoSize = true;
            rbLifo.Font = new System.Drawing.Font("Segoe UI", 10F);
            rbLifo.Location = new System.Drawing.Point(18, 108);
            rbLifo.Name = "rbLifo";
            rbLifo.Size = new System.Drawing.Size(216, 27);
            rbLifo.TabIndex = 2;
            rbLifo.TabStop = true;
            rbLifo.Text = "LIFO (Urutan Terakhir)";
            rbLifo.UseVisualStyleBackColor = true;
            // 
            // rbFefo
            // 
            rbFefo.AutoSize = true;
            rbFefo.Font = new System.Drawing.Font("Segoe UI", 10F);
            rbFefo.Location = new System.Drawing.Point(320, 40);
            rbFefo.Name = "rbFefo";
            rbFefo.Size = new System.Drawing.Size(221, 27);
            rbFefo.TabIndex = 3;
            rbFefo.TabStop = true;
            rbFefo.Text = "FEFO (By Expired Date)";
            rbFefo.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.BackColor = System.Drawing.Color.FromArgb(0, 122, 255);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(16, 280);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(140, 40);
            btnSave.TabIndex = 2;
            btnSave.Text = "Simpan";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new System.Drawing.Point(170, 280);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(140, 40);
            btnClose.TabIndex = 3;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // HppMethodSettingForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(640, 350);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(grpMethods);
            Controls.Add(panelHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HppMethodSettingForm";
            Text = "Master HPP / Valuation";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            grpMethods.ResumeLayout(false);
            grpMethods.PerformLayout();
            ResumeLayout(false);
        }
    }
}

