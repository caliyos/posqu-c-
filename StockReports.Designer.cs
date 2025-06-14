namespace POS_qu
{
    partial class StockReports
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
            dgvStockReport = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            label4 = new Label();
            button5 = new Button();
            button6 = new Button();
            cmbPageSize = new ComboBox();
            btnLast = new Button();
            btnNext = new Button();
            btnPrev = new Button();
            btnFirst = new Button();
            lblPagingInfo = new Label();
            lblSearch = new Label();
            txtSearch = new TextBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvStockReport).BeginInit();
            SuspendLayout();
            // 
            // dgvStockReport
            // 
            dgvStockReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStockReport.Location = new Point(16, 248);
            dgvStockReport.Name = "dgvStockReport";
            dgvStockReport.RowHeadersWidth = 62;
            dgvStockReport.Size = new Size(1360, 872);
            dgvStockReport.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 128);
            label1.Name = "label1";
            label1.Size = new Size(55, 25);
            label1.TabIndex = 1;
            label1.Text = "Stock";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 160);
            label2.Name = "label2";
            label2.Size = new Size(151, 25);
            label2.TabIndex = 1;
            label2.Text = "Stock (Harga beli)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(32, 192);
            label3.Name = "label3";
            label3.Size = new Size(152, 25);
            label3.TabIndex = 1;
            label3.Text = "Stock (Harga Jual)";
            // 
            // button1
            // 
            button1.Location = new Point(504, 200);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 2;
            button1.Text = "All Stock";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(632, 200);
            button2.Name = "button2";
            button2.Size = new Size(192, 34);
            button2.TabIndex = 2;
            button2.Text = "Available Stock";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(840, 200);
            button3.Name = "button3";
            button3.Size = new Size(192, 34);
            button3.TabIndex = 3;
            button3.Text = "Empty Stock";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(1024, 1152);
            button4.Name = "button4";
            button4.Size = new Size(112, 34);
            button4.TabIndex = 4;
            button4.Text = "excel";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(912, 1152);
            label4.Name = "label4";
            label4.Size = new Size(94, 25);
            label4.TabIndex = 1;
            label4.Text = "Export to :";
            // 
            // button5
            // 
            button5.Location = new Point(1144, 1152);
            button5.Name = "button5";
            button5.Size = new Size(112, 34);
            button5.TabIndex = 4;
            button5.Text = "csv";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(1264, 1152);
            button6.Name = "button6";
            button6.Size = new Size(112, 34);
            button6.TabIndex = 4;
            button6.Text = "pdf";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // cmbPageSize
            // 
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(16, 1184);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(182, 33);
            cmbPageSize.TabIndex = 24;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(632, 1144);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(112, 34);
            btnLast.TabIndex = 20;
            btnLast.Text = "Last";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(504, 1144);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(112, 34);
            btnNext.TabIndex = 21;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(376, 1144);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(112, 34);
            btnPrev.TabIndex = 22;
            btnPrev.Text = "Prev";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(240, 1144);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(112, 34);
            btnFirst.TabIndex = 23;
            btnFirst.Text = "First";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // lblPagingInfo
            // 
            lblPagingInfo.AutoSize = true;
            lblPagingInfo.Location = new Point(16, 1144);
            lblPagingInfo.Name = "lblPagingInfo";
            lblPagingInfo.Size = new Size(103, 25);
            lblPagingInfo.TabIndex = 19;
            lblPagingInfo.Text = "Paging Info";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(1040, 208);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(64, 25);
            lblSearch.TabIndex = 26;
            lblSearch.Text = "Search";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(1120, 200);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(256, 31);
            txtSearch.TabIndex = 25;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(216, 192);
            label5.Name = "label5";
            label5.Size = new Size(16, 25);
            label5.TabIndex = 1;
            label5.Text = ".";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(216, 160);
            label6.Name = "label6";
            label6.Size = new Size(16, 25);
            label6.TabIndex = 1;
            label6.Text = ".";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(216, 128);
            label7.Name = "label7";
            label7.Size = new Size(16, 25);
            label7.TabIndex = 1;
            label7.Text = ".";
            // 
            // StockReports
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1542, 1253);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(cmbPageSize);
            Controls.Add(btnLast);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(btnFirst);
            Controls.Add(lblPagingInfo);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label6);
            Controls.Add(label2);
            Controls.Add(label7);
            Controls.Add(label1);
            Controls.Add(dgvStockReport);
            Name = "StockReports";
            Text = "StockReports";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dgvStockReport).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvStockReport;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Label label4;
        private Button button5;
        private Button button6;
        private ComboBox cmbPageSize;
        private Button btnLast;
        private Button btnNext;
        private Button btnPrev;
        private Button btnFirst;
        private Label lblPagingInfo;
        private Label lblSearch;
        private TextBox txtSearch;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}