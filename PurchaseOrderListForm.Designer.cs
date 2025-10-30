namespace POS_qu
{
    partial class PurchaseOrderListForm
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
            dgvPO = new DataGridView();
            txtSearch = new TextBox();
            cmbFilterStatus = new ComboBox();
            btnViewDetail = new Button();
            btnRefresh = new Button();
            btnPrint = new Button();
            btnPO = new Button();
            btnChangeStatus = new Button();
            btnCetakPO = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPO).BeginInit();
            SuspendLayout();
            // 
            // dgvPO
            // 
            dgvPO.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPO.Location = new Point(176, 128);
            dgvPO.Name = "dgvPO";
            dgvPO.RowHeadersWidth = 62;
            dgvPO.Size = new Size(1896, 680);
            dgvPO.TabIndex = 0;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(176, 80);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(312, 31);
            txtSearch.TabIndex = 1;
            // 
            // cmbFilterStatus
            // 
            cmbFilterStatus.FormattingEnabled = true;
            cmbFilterStatus.Location = new Point(1584, 80);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new Size(470, 33);
            cmbFilterStatus.TabIndex = 2;
            // 
            // btnViewDetail
            // 
            btnViewDetail.Location = new Point(168, 824);
            btnViewDetail.Name = "btnViewDetail";
            btnViewDetail.Size = new Size(112, 34);
            btnViewDetail.TabIndex = 3;
            btnViewDetail.Text = "View Detail";
            btnViewDetail.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(320, 824);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 34);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            btnPrint.Location = new Point(464, 824);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(112, 34);
            btnPrint.TabIndex = 3;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;
            // 
            // btnPO
            // 
            btnPO.Location = new Point(1944, 24);
            btnPO.Name = "btnPO";
            btnPO.Size = new Size(112, 34);
            btnPO.TabIndex = 4;
            btnPO.Text = "Buat PO";
            btnPO.UseVisualStyleBackColor = true;
            btnPO.Click += btnPO_Click;
            // 
            // btnChangeStatus
            // 
            btnChangeStatus.Location = new Point(600, 80);
            btnChangeStatus.Name = "btnChangeStatus";
            btnChangeStatus.Size = new Size(224, 34);
            btnChangeStatus.TabIndex = 5;
            btnChangeStatus.Text = "Update Status";
            btnChangeStatus.UseVisualStyleBackColor = true;
            // 
            // btnCetakPO
            // 
            btnCetakPO.Location = new Point(872, 80);
            btnCetakPO.Name = "btnCetakPO";
            btnCetakPO.Size = new Size(224, 34);
            btnCetakPO.TabIndex = 5;
            btnCetakPO.Text = "Cetak PO";
            btnCetakPO.UseVisualStyleBackColor = true;
            btnCetakPO.Click += btnCetakPO_Click;
            // 
            // PurchaseOrderListForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2066, 1057);
            Controls.Add(btnCetakPO);
            Controls.Add(btnChangeStatus);
            Controls.Add(btnPO);
            Controls.Add(btnPrint);
            Controls.Add(btnRefresh);
            Controls.Add(btnViewDetail);
            Controls.Add(cmbFilterStatus);
            Controls.Add(txtSearch);
            Controls.Add(dgvPO);
            Name = "PurchaseOrderListForm";
            Text = "PurchaseOrderListForm";
            ((System.ComponentModel.ISupportInitialize)dgvPO).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvPO;
        private TextBox txtSearch;
        private ComboBox cmbFilterStatus;
        private Button btnViewDetail;
        private Button btnRefresh;
        private Button btnPrint;
        private Button btnPO;
        private Button btnChangeStatus;
        private Button btnCetakPO;
    }
}