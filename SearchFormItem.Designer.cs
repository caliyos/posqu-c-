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
            lblPageNumber = new Label();
            btnNext = new Button();
            btnPrevious = new Button();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnFirstPage = new Button();
            btnLastPage = new Button();
            lblRowCount = new Label();
            cmbPageSize = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewSearchResults
            // 
            dataGridViewSearchResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSearchResults.Location = new Point(40, 56);
            dataGridViewSearchResults.Name = "dataGridViewSearchResults";
            dataGridViewSearchResults.RowHeadersWidth = 62;
            dataGridViewSearchResults.Size = new Size(1296, 528);
            dataGridViewSearchResults.TabIndex = 0;
            // 
            // lblPageNumber
            // 
            lblPageNumber.AutoSize = true;
            lblPageNumber.Location = new Point(608, 632);
            lblPageNumber.Name = "lblPageNumber";
            lblPageNumber.Size = new Size(115, 25);
            lblPageNumber.TabIndex = 1;
            lblPageNumber.Text = "PageNumber";
            // 
            // btnNext
            // 
            btnNext.Location = new Point(848, 632);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 2;
            btnNext.Text = "next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click_1;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new Point(968, 632);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(112, 34);
            btnPrevious.TabIndex = 2;
            btnPrevious.Text = "prev";
            btnPrevious.UseVisualStyleBackColor = true;
            btnPrevious.Click += btnNext_Click_1;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(1072, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(150, 31);
            txtSearch.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(1232, 16);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(112, 34);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnFirstPage
            // 
            btnFirstPage.Location = new Point(736, 632);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(112, 34);
            btnFirstPage.TabIndex = 2;
            btnFirstPage.Text = "first";
            btnFirstPage.UseVisualStyleBackColor = true;
            btnFirstPage.Click += btnNext_Click_1;
            // 
            // btnLastPage
            // 
            btnLastPage.Location = new Point(1088, 632);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(112, 34);
            btnLastPage.TabIndex = 2;
            btnLastPage.Text = "last";
            btnLastPage.UseVisualStyleBackColor = true;
            btnLastPage.Click += btnNext_Click_1;
            // 
            // lblRowCount
            // 
            lblRowCount.AutoSize = true;
            lblRowCount.Location = new Point(448, 624);
            lblRowCount.Name = "lblRowCount";
            lblRowCount.Size = new Size(118, 25);
            lblRowCount.TabIndex = 1;
            lblRowCount.Text = "Row Per Page";
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(224, 624);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 5;
            // 
            // SearchFormItem
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1398, 695);
            Controls.Add(cmbPageSize);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
            Controls.Add(btnPrevious);
            Controls.Add(btnLastPage);
            Controls.Add(btnFirstPage);
            Controls.Add(btnNext);
            Controls.Add(lblRowCount);
            Controls.Add(lblPageNumber);
            Controls.Add(dataGridViewSearchResults);
            Name = "SearchFormItem";
            Text = "SearchFormItem";
            Load += SearchFormItem_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewSearchResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewSearchResults;
        private Label lblPageNumber;
        private Button btnNext;
        private Button btnPrevious;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnFirstPage;
        private Button btnLastPage;
        private Label lblRowCount;
        private ComboBox cmbPageSize;
        //btnSearch.Click += btnSearch_Click; // If not added in the designer

    }
    }