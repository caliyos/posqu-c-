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
            dataGridViewSearchResults = new DataGridView();
            txtSearch = new TextBox();
            btnSearch = new Button();
            cmbPageSize = new ComboBox();
            btnFirstPage = new Button();
            lblPagingInfo = new Label();
            lblPageNumber = new Label();
            btnNext = new Button();
            btnPrevious = new Button();
            btnLastPage = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewSearchResults
            // 
            dataGridViewSearchResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewSearchResults.BackgroundColor = Color.White;
            dataGridViewSearchResults.BorderStyle = BorderStyle.None;
            dataGridViewSearchResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSearchResults.Location = new Point(40, 80);
            dataGridViewSearchResults.Name = "dataGridViewSearchResults";
            dataGridViewSearchResults.RowHeadersWidth = 62;
            dataGridViewSearchResults.Size = new Size(1320, 560);
            dataGridViewSearchResults.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 14F);
            txtSearch.Location = new Point(40, 20);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(350, 45);
            txtSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(10, 223, 255);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(410, 20);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(120, 40);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Cari";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(40, 696);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 40);
            cmbPageSize.TabIndex = 17;
            // 
            // btnFirstPage
            // 
            btnFirstPage.Location = new Point(608, 680);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(112, 34);
            btnFirstPage.TabIndex = 15;
            btnFirstPage.Text = "First";
            btnFirstPage.UseVisualStyleBackColor = true;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(32, 664);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(135, 32);
            lblPagingInfo.TabIndex = 13;
            lblPagingInfo.Text = "Paging Info";
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(296, 672);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(181, 32);
            lblPageNumber.TabIndex = 13;
            lblPageNumber.Text = "Paging Number";
            // 
            // btnNext
            // 
            btnNext.Location = new Point(896, 680);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 15;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new Point(752, 680);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(112, 34);
            btnPrevious.TabIndex = 15;
            btnPrevious.Text = "Prev";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnLastPage
            // 
            btnLastPage.Location = new Point(1048, 680);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(112, 34);
            btnLastPage.TabIndex = 15;
            btnLastPage.Text = "Last";
            btnLastPage.UseVisualStyleBackColor = true;
            // 
            // SearchFormItem
            // 
            BackColor = Color.White;
            ClientSize = new Size(1400, 800);
            Controls.Add(cmbPageSize);
            Controls.Add(btnLastPage);
            Controls.Add(btnPrevious);
            Controls.Add(btnNext);
            Controls.Add(btnFirstPage);
            Controls.Add(lblPageNumber);
            Controls.Add(lblPagingInfo);
            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(dataGridViewSearchResults);
            Font = new Font("Segoe UI", 12F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "SearchFormItem";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cari Barang";
            ((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
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

        // Event handlers for pagination buttons

    }
}
