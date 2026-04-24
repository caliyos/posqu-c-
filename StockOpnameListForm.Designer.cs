namespace POS_qu
{
    partial class StockOpnameListForm
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
            panelTop = new Panel();
            lblTitle = new Label();
            btnRefresh = new Button();
            btnClose = new Button();
            splitContainer1 = new SplitContainer();
            dgvOpnames = new DataGridView();
            dgvItems = new DataGridView();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOpnames).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(btnRefresh);
            panelTop.Controls.Add(btnClose);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(12);
            panelTop.Size = new Size(1280, 58);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(176, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Daftar Stock Opname";
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.Location = new Point(996, 12);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(110, 34);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.Location = new Point(1116, 12);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(140, 34);
            btnClose.TabIndex = 2;
            btnClose.Text = "Tutup";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 58);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dgvOpnames);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dgvItems);
            splitContainer1.Size = new Size(1280, 662);
            splitContainer1.SplitterDistance = 300;
            splitContainer1.TabIndex = 1;
            // 
            // dgvOpnames
            // 
            dgvOpnames.AllowUserToAddRows = false;
            dgvOpnames.BackgroundColor = Color.White;
            dgvOpnames.BorderStyle = BorderStyle.None;
            dgvOpnames.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvOpnames.ColumnHeadersHeight = 42;
            dgvOpnames.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvOpnames.Dock = DockStyle.Fill;
            dgvOpnames.EnableHeadersVisualStyles = false;
            dgvOpnames.Location = new Point(0, 0);
            dgvOpnames.Name = "dgvOpnames";
            dgvOpnames.ReadOnly = true;
            dgvOpnames.RowHeadersVisible = false;
            dgvOpnames.RowTemplate.Height = 40;
            dgvOpnames.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOpnames.Size = new Size(1280, 300);
            dgvOpnames.TabIndex = 0;
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersHeight = 42;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.Location = new Point(0, 0);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowTemplate.Height = 40;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(1280, 358);
            dgvItems.TabIndex = 0;
            // 
            // StockOpnameListForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(1280, 720);
            Controls.Add(splitContainer1);
            Controls.Add(panelTop);
            Name = "StockOpnameListForm";
            Text = "Daftar Stock Opname";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOpnames).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Label lblTitle;
        private Button btnRefresh;
        private Button btnClose;
        private SplitContainer splitContainer1;
        private DataGridView dgvOpnames;
        private DataGridView dgvItems;
    }
}

