namespace POS_qu
{
    partial class SearchFormItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelRoot = new Panel();
            dataGridViewSearchResults = new DataGridView();
            panelBottom = new Panel();
            btnLastPage = new Button();
            btnPrevious = new Button();
            btnNext = new Button();
            btnFirstPage = new Button();
            lblPageNumber = new Label();
            lblPagingInfo = new Label();
            panelTop = new Panel();
            lblHint = new Label();
            cmbPageSize = new ComboBox();
            btnSearch = new Button();
            txtSearch = new TextBox();
            panelRoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).BeginInit();
            panelBottom.SuspendLayout();
            panelTop.SuspendLayout();
            SuspendLayout();
            // 
            // panelRoot
            // 
            panelRoot.BackColor = Color.FromArgb(245, 246, 250);
            panelRoot.Controls.Add(dataGridViewSearchResults);
            panelRoot.Controls.Add(panelBottom);
            panelRoot.Controls.Add(panelTop);
            panelRoot.Dock = DockStyle.Fill;
            panelRoot.Location = new Point(0, 0);
            panelRoot.Name = "panelRoot";
            panelRoot.Padding = new Padding(18);
            panelRoot.Size = new Size(1351, 780);
            panelRoot.TabIndex = 0;
            // 
            // dataGridViewSearchResults
            // 
            dataGridViewSearchResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSearchResults.BackgroundColor = Color.White;
            dataGridViewSearchResults.BorderStyle = BorderStyle.None;
            dataGridViewSearchResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSearchResults.Dock = DockStyle.Fill;
            dataGridViewSearchResults.Location = new Point(18, 131);
            dataGridViewSearchResults.Name = "dataGridViewSearchResults";
            dataGridViewSearchResults.RowHeadersWidth = 62;
            dataGridViewSearchResults.Size = new Size(1315, 567);
            dataGridViewSearchResults.TabIndex = 2;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.White;
            panelBottom.Controls.Add(btnLastPage);
            panelBottom.Controls.Add(btnPrevious);
            panelBottom.Controls.Add(btnNext);
            panelBottom.Controls.Add(btnFirstPage);
            panelBottom.Controls.Add(lblPageNumber);
            panelBottom.Controls.Add(lblPagingInfo);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(18, 698);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(14);
            panelBottom.Size = new Size(1315, 64);
            panelBottom.TabIndex = 1;
            // 
            // btnLastPage
            // 
            btnLastPage.FlatStyle = FlatStyle.Flat;
            btnLastPage.Location = new Point(1006, 16);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(133, 48);
            btnLastPage.TabIndex = 15;
            btnLastPage.Text = "Last";
            btnLastPage.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Location = new Point(718, 16);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(133, 48);
            btnPrevious.TabIndex = 15;
            btnPrevious.Text = "Prev";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Location = new Point(862, 16);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(133, 48);
            btnNext.TabIndex = 15;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnFirstPage
            // 
            btnFirstPage.FlatStyle = FlatStyle.Flat;
            btnFirstPage.Location = new Point(574, 16);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(133, 48);
            btnFirstPage.TabIndex = 15;
            btnFirstPage.Text = "First";
            btnFirstPage.UseVisualStyleBackColor = true;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(214, 22);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(181, 32);
            lblPageNumber.TabIndex = 13;
            lblPageNumber.Text = "Paging Number";
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(14, 22);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(135, 32);
            lblPagingInfo.TabIndex = 13;
            lblPagingInfo.Text = "Paging Info";
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblHint);
            panelTop.Controls.Add(cmbPageSize);
            panelTop.Controls.Add(btnSearch);
            panelTop.Controls.Add(txtSearch);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(18, 18);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(14);
            panelTop.Size = new Size(1315, 113);
            panelTop.TabIndex = 0;
            // 
            // lblHint
            // 
            lblHint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblHint.ForeColor = Color.FromArgb(90, 90, 90);
            lblHint.Location = new Point(991, 10);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(310, 42);
            lblHint.TabIndex = 3;
            lblHint.Text = "Enter/DblClick = pilih item";
            lblHint.TextAlign = ContentAlignment.TopRight;
            // 
            // cmbPageSize
            // 
            cmbPageSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(1158, 56);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(140, 40);
            cmbPageSize.TabIndex = 17;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(0, 120, 215);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(646, 21);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(120, 44);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Cari";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 14F);
            txtSearch.Location = new Point(14, 22);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Cari nama / barcode...";
            txtSearch.Size = new Size(620, 45);
            txtSearch.TabIndex = 0;
            // 
            // SearchFormItem
            // 
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(1351, 780);
            Controls.Add(panelRoot);
            Font = new Font("Segoe UI", 12F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "SearchFormItem";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cari Barang";
            panelRoot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewSearchResults;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnFirstPage;
        private Label lblPagingInfo;
        private ComboBox cmbPageSize;
        private Label lblPageNumber;
        private Button btnNext;
        private Button btnPrevious;
        private Button btnLastPage;
        private Panel panelRoot;
        private Panel panelTop;
        private Panel panelBottom;
        private Label lblHint;

        // Event handlers for pagination buttons

    }
}
