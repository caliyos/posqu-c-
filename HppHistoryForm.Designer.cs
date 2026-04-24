namespace POS_qu
{
    partial class HppHistoryForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabLayers;
        private System.Windows.Forms.TabPage tabStockCard;
        private System.Windows.Forms.DataGridView dgvLayers;
        private System.Windows.Forms.DataGridView dgvStockCard;
        private System.Windows.Forms.Panel panelBottom;
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
            lblTitle = new System.Windows.Forms.Label();
            lblItem = new System.Windows.Forms.Label();
            lblMethod = new System.Windows.Forms.Label();
            lblWarehouse = new System.Windows.Forms.Label();
            cmbWarehouse = new System.Windows.Forms.ComboBox();
            btnRefresh = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            tabMain = new System.Windows.Forms.TabControl();
            tabLayers = new System.Windows.Forms.TabPage();
            tabStockCard = new System.Windows.Forms.TabPage();
            dgvLayers = new System.Windows.Forms.DataGridView();
            dgvStockCard = new System.Windows.Forms.DataGridView();
            panelBottom = new System.Windows.Forms.Panel();
            lblSummary = new System.Windows.Forms.Label();
            panelHeader.SuspendLayout();
            tabMain.SuspendLayout();
            tabLayers.SuspendLayout();
            tabStockCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLayers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvStockCard).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.White;
            panelHeader.Controls.Add(btnClose);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(cmbWarehouse);
            panelHeader.Controls.Add(lblWarehouse);
            panelHeader.Controls.Add(lblMethod);
            panelHeader.Controls.Add(lblItem);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(1000, 120);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblTitle.Location = new System.Drawing.Point(16, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(191, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "History HPP Item";
            // 
            // lblItem
            // 
            lblItem.AutoSize = true;
            lblItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblItem.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblItem.Location = new System.Drawing.Point(18, 52);
            lblItem.Name = "lblItem";
            lblItem.Size = new System.Drawing.Size(74, 23);
            lblItem.TabIndex = 1;
            lblItem.Text = "Item: -";
            // 
            // lblMethod
            // 
            lblMethod.AutoSize = true;
            lblMethod.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblMethod.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblMethod.Location = new System.Drawing.Point(18, 80);
            lblMethod.Name = "lblMethod";
            lblMethod.Size = new System.Drawing.Size(118, 23);
            lblMethod.TabIndex = 2;
            lblMethod.Text = "Method: FIFO";
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblWarehouse.Location = new System.Drawing.Point(520, 20);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new System.Drawing.Size(86, 23);
            lblWarehouse.TabIndex = 3;
            lblWarehouse.Text = "Gudang:";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbWarehouse.FormattingEnabled = true;
            cmbWarehouse.Location = new System.Drawing.Point(612, 16);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new System.Drawing.Size(220, 31);
            cmbWarehouse.TabIndex = 4;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.White;
            btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnRefresh.Location = new System.Drawing.Point(844, 16);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(120, 34);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new System.Drawing.Point(844, 60);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(120, 34);
            btnClose.TabIndex = 6;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabLayers);
            tabMain.Controls.Add(tabStockCard);
            tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tabMain.Location = new System.Drawing.Point(0, 120);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(1000, 430);
            tabMain.TabIndex = 1;
            // 
            // tabLayers
            // 
            tabLayers.Controls.Add(dgvLayers);
            tabLayers.Location = new System.Drawing.Point(4, 34);
            tabLayers.Name = "tabLayers";
            tabLayers.Padding = new System.Windows.Forms.Padding(3);
            tabLayers.Size = new System.Drawing.Size(992, 392);
            tabLayers.TabIndex = 0;
            tabLayers.Text = "HPP Layers";
            tabLayers.UseVisualStyleBackColor = true;
            // 
            // tabStockCard
            // 
            tabStockCard.Controls.Add(dgvStockCard);
            tabStockCard.Location = new System.Drawing.Point(4, 34);
            tabStockCard.Name = "tabStockCard";
            tabStockCard.Padding = new System.Windows.Forms.Padding(3);
            tabStockCard.Size = new System.Drawing.Size(992, 392);
            tabStockCard.TabIndex = 1;
            tabStockCard.Text = "Kartu Stock";
            tabStockCard.UseVisualStyleBackColor = true;
            // 
            // dgvLayers
            // 
            dgvLayers.AllowUserToAddRows = false;
            dgvLayers.AllowUserToDeleteRows = false;
            dgvLayers.BackgroundColor = System.Drawing.Color.White;
            dgvLayers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvLayers.Location = new System.Drawing.Point(3, 3);
            dgvLayers.Name = "dgvLayers";
            dgvLayers.ReadOnly = true;
            dgvLayers.RowHeadersVisible = false;
            dgvLayers.RowHeadersWidth = 62;
            dgvLayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLayers.Size = new System.Drawing.Size(986, 386);
            dgvLayers.TabIndex = 0;
            // 
            // dgvStockCard
            // 
            dgvStockCard.AllowUserToAddRows = false;
            dgvStockCard.AllowUserToDeleteRows = false;
            dgvStockCard.BackgroundColor = System.Drawing.Color.White;
            dgvStockCard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvStockCard.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvStockCard.Location = new System.Drawing.Point(3, 3);
            dgvStockCard.Name = "dgvStockCard";
            dgvStockCard.ReadOnly = true;
            dgvStockCard.RowHeadersVisible = false;
            dgvStockCard.RowHeadersWidth = 62;
            dgvStockCard.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvStockCard.Size = new System.Drawing.Size(986, 386);
            dgvStockCard.TabIndex = 0;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = System.Drawing.Color.White;
            panelBottom.Controls.Add(lblSummary);
            panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelBottom.Location = new System.Drawing.Point(0, 550);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            panelBottom.Size = new System.Drawing.Size(1000, 60);
            panelBottom.TabIndex = 2;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSummary.ForeColor = System.Drawing.Color.FromArgb(51, 51, 51);
            lblSummary.Location = new System.Drawing.Point(16, 18);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new System.Drawing.Size(162, 23);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Total: Qty 0 | Rp 0";
            // 
            // HppHistoryForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            ClientSize = new System.Drawing.Size(1000, 610);
            Controls.Add(tabMain);
            Controls.Add(panelBottom);
            Controls.Add(panelHeader);
            Name = "HppHistoryForm";
            Text = "History HPP";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            tabMain.ResumeLayout(false);
            tabLayers.ResumeLayout(false);
            tabStockCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLayers).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvStockCard).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }
    }
}
