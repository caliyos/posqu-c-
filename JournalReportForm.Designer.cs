namespace POS_qu
{
    partial class JournalReportForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvEntries;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblSummary;

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
            btnClose = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            dtTo = new System.Windows.Forms.DateTimePicker();
            lblTo = new System.Windows.Forms.Label();
            dtFrom = new System.Windows.Forms.DateTimePicker();
            lblFrom = new System.Windows.Forms.Label();
            lblTitle = new System.Windows.Forms.Label();
            splitMain = new System.Windows.Forms.SplitContainer();
            dgvEntries = new System.Windows.Forms.DataGridView();
            dgvDetails = new System.Windows.Forms.DataGridView();
            panelSummary = new System.Windows.Forms.Panel();
            lblSummary = new System.Windows.Forms.Label();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEntries).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            panelSummary.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(dtTo);
            panelHeader.Controls.Add(lblTo);
            panelHeader.Controls.Add(dtFrom);
            panelHeader.Controls.Add(lblFrom);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(1200, 78);
            panelHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new System.Drawing.Point(1064, 22);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(120, 34);
            btnClose.TabIndex = 6;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.White;
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnRefresh.Location = new System.Drawing.Point(938, 22);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(120, 34);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dtTo
            // 
            dtTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtTo.Location = new System.Drawing.Point(710, 24);
            dtTo.Name = "dtTo";
            dtTo.Size = new System.Drawing.Size(160, 30);
            dtTo.TabIndex = 4;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblTo.Location = new System.Drawing.Point(650, 28);
            lblTo.Name = "lblTo";
            lblTo.Size = new System.Drawing.Size(54, 23);
            lblTo.TabIndex = 3;
            lblTo.Text = "Sampai";
            // 
            // dtFrom
            // 
            dtFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtFrom.Location = new System.Drawing.Point(478, 24);
            dtFrom.Name = "dtFrom";
            dtFrom.Size = new System.Drawing.Size(160, 30);
            dtFrom.TabIndex = 2;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblFrom.Location = new System.Drawing.Point(420, 28);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new System.Drawing.Size(46, 23);
            lblFrom.TabIndex = 1;
            lblFrom.Text = "Dari";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(16, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(163, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Laporan Jurnal";
            // 
            // splitMain
            // 
            splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitMain.Location = new System.Drawing.Point(0, 78);
            splitMain.Name = "splitMain";
            splitMain.Panel1.Controls.Add(dgvEntries);
            splitMain.Panel2.Controls.Add(dgvDetails);
            splitMain.Size = new System.Drawing.Size(1200, 592);
            splitMain.SplitterDistance = 650;
            splitMain.TabIndex = 1;
            // 
            // dgvEntries
            // 
            dgvEntries.AllowUserToAddRows = false;
            dgvEntries.AllowUserToDeleteRows = false;
            dgvEntries.BackgroundColor = System.Drawing.Color.White;
            dgvEntries.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvEntries.Location = new System.Drawing.Point(0, 0);
            dgvEntries.MultiSelect = false;
            dgvEntries.Name = "dgvEntries";
            dgvEntries.ReadOnly = true;
            dgvEntries.RowHeadersVisible = false;
            dgvEntries.RowHeadersWidth = 62;
            dgvEntries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvEntries.Size = new System.Drawing.Size(650, 592);
            dgvEntries.TabIndex = 0;
            // 
            // dgvDetails
            // 
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.BackgroundColor = System.Drawing.Color.White;
            dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvDetails.Location = new System.Drawing.Point(0, 0);
            dgvDetails.MultiSelect = false;
            dgvDetails.Name = "dgvDetails";
            dgvDetails.ReadOnly = true;
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.RowHeadersWidth = 62;
            dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.Size = new System.Drawing.Size(546, 592);
            dgvDetails.TabIndex = 0;
            // 
            // panelSummary
            // 
            panelSummary.BackColor = System.Drawing.Color.White;
            panelSummary.Controls.Add(lblSummary);
            panelSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelSummary.Location = new System.Drawing.Point(0, 670);
            panelSummary.Name = "panelSummary";
            panelSummary.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            panelSummary.Size = new System.Drawing.Size(1200, 50);
            panelSummary.TabIndex = 2;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSummary.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblSummary.Location = new System.Drawing.Point(16, 14);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new System.Drawing.Size(145, 23);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Total Debit: 0 | Kredit: 0";
            // 
            // JournalReportForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(1200, 720);
            Controls.Add(splitMain);
            Controls.Add(panelHeader);
            Controls.Add(panelSummary);
            Name = "JournalReportForm";
            Text = "Laporan Jurnal";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEntries).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            panelSummary.ResumeLayout(false);
            panelSummary.PerformLayout();
            ResumeLayout(false);
        }
    }
}

