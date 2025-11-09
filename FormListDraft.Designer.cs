namespace POS_qu
{
    partial class FormListDraft
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
            dataGridViewDrafts = new DataGridView();
            btnRefresh = new Button();
            btnLoadDraft = new Button();
            btnDeleteDraft = new Button();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDrafts).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewDrafts
            // 
            dataGridViewDrafts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDrafts.Location = new Point(16, 80);
            dataGridViewDrafts.Name = "dataGridViewDrafts";
            dataGridViewDrafts.RowHeadersWidth = 62;
            dataGridViewDrafts.Size = new Size(1512, 544);
            dataGridViewDrafts.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(432, 32);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(112, 34);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnLoadDraft
            // 
            btnLoadDraft.Location = new Point(16, 32);
            btnLoadDraft.Name = "btnLoadDraft";
            btnLoadDraft.Size = new Size(112, 34);
            btnLoadDraft.TabIndex = 1;
            btnLoadDraft.Text = "Load Draft";
            btnLoadDraft.UseVisualStyleBackColor = true;
            btnLoadDraft.Click += btnLoadDraft_Click;
            // 
            // btnDeleteDraft
            // 
            btnDeleteDraft.Location = new Point(144, 32);
            btnDeleteDraft.Name = "btnDeleteDraft";
            btnDeleteDraft.Size = new Size(144, 34);
            btnDeleteDraft.TabIndex = 1;
            btnDeleteDraft.Text = "Delete Draft";
            btnDeleteDraft.UseVisualStyleBackColor = true;

            // btnClose
            // 
            btnClose.Location = new Point(304, 32);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(112, 34);
            btnClose.TabIndex = 1;
            btnClose.Text = "close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // FormListDraft
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1564, 648);
            Controls.Add(btnClose);
            Controls.Add(btnDeleteDraft);
            Controls.Add(btnLoadDraft);
            Controls.Add(btnRefresh);
            Controls.Add(dataGridViewDrafts);
            Name = "FormListDraft";
            Text = "FormListDraft";
            ((System.ComponentModel.ISupportInitialize)dataGridViewDrafts).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewDrafts;
        private Button btnRefresh;
        private Button btnLoadDraft;
        private Button btnDeleteDraft;
        private Button btnClose;
    }
}