namespace POS_qu
{
    partial class Form4_crud
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
            btnPay = new Button();
            panel1 = new Panel();
            dataGridViewCart4 = new DataGridView();
            txtCariBarang = new TextBox();
            label1 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart4).BeginInit();
            SuspendLayout();
            // 
            // btnPay
            // 
            btnPay.Location = new Point(296, 136);
            btnPay.Name = "btnPay";
            btnPay.Size = new Size(112, 34);
            btnPay.TabIndex = 4;
            btnPay.Text = "Pay";
            btnPay.UseVisualStyleBackColor = true;
            btnPay.Click += btnPay_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label2);
            panel1.Location = new Point(512, 24);
            panel1.Name = "panel1";
            panel1.Size = new Size(1000, 160);
            panel1.TabIndex = 3;
            //panel1.Paint += panel1_Paint;
            // 
            // dataGridViewCart4
            // 
            dataGridViewCart4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCart4.Location = new Point(40, 216);
            dataGridViewCart4.Name = "dataGridViewCart4";
            dataGridViewCart4.RowHeadersWidth = 62;
            dataGridViewCart4.Size = new Size(1472, 392);
            dataGridViewCart4.TabIndex = 2;
            //dataGridViewCart4.CellContentClick += dataGridViewCart4_CellContentClick;
            // 
            // txtCariBarang
            // 
            txtCariBarang.Location = new Point(144, 56);
            txtCariBarang.Name = "txtCariBarang";
            txtCariBarang.Size = new Size(280, 31);
            txtCariBarang.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(80, 56);
            label1.Name = "label1";
            label1.Size = new Size(42, 25);
            label1.TabIndex = 0;
            label1.Text = "Cari";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 36F, FontStyle.Bold);
            label2.Location = new Point(640, 72);
            label2.Name = "label2";
            label2.Size = new Size(0, 84);
            label2.TabIndex = 0;
            // 
            // Form4_crud
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1577, 671);
            Controls.Add(panel1);
            Controls.Add(btnPay);
            Controls.Add(dataGridViewCart4);
            Controls.Add(txtCariBarang);
            Controls.Add(label1);
            Name = "Form4_crud";
            Text = "Form4_crud";
            //Load += Form4_crud_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnPay;
        private Panel panel1;
        private Label label2;
        private DataGridView dataGridViewCart4;
        private TextBox txtCariBarang;
        private Label label1;
    }
}