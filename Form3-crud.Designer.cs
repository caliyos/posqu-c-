namespace POS_qu
{
    partial class Form3_crud
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
            txtNoTransaksi = new TextBox();
            dataGridViewCart = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnAdd = new Button();
            dateTimePicker1 = new DateTimePicker();
            comboBoxPelanggan = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart).BeginInit();
            SuspendLayout();
            // 
            // txtNoTransaksi
            // 
            txtNoTransaksi.Location = new Point(152, 32);
            txtNoTransaksi.Name = "txtNoTransaksi";
            txtNoTransaksi.Size = new Size(216, 31);
            txtNoTransaksi.TabIndex = 0;
            // 
            // dataGridViewCart
            // 
            dataGridViewCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCart.Location = new Point(56, 216);
            dataGridViewCart.Name = "dataGridViewCart";
            dataGridViewCart.RowHeadersWidth = 62;
            dataGridViewCart.Size = new Size(1328, 400);
            dataGridViewCart.TabIndex = 2;
            dataGridViewCart.CellContentClick += dataGridViewCart_CellContentClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 40);
            label1.Name = "label1";
            label1.Size = new Size(111, 25);
            label1.TabIndex = 3;
            label1.Text = "No Transaksi";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 72);
            label2.Name = "label2";
            label2.Size = new Size(73, 25);
            label2.TabIndex = 4;
            label2.Text = "Tanggal";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 120);
            label3.Name = "label3";
            label3.Size = new Size(94, 25);
            label3.TabIndex = 4;
            label3.Text = "Pelanggan";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 160);
            label4.Name = "label4";
            label4.Size = new Size(67, 25);
            label4.TabIndex = 4;
            label4.Text = "Jumlah";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(264, 160);
            label5.Name = "label5";
            label5.Size = new Size(94, 25);
            label5.TabIndex = 4;
            label5.Text = "Kode Item";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(472, 160);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(240, 34);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Add To Cart";
            btnAdd.UseVisualStyleBackColor = true;
            //btnAdd.Click += btnAdd_Click;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(152, 72);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(328, 31);
            dateTimePicker1.TabIndex = 6;
            // 
            // comboBoxPelanggan
            // 
            comboBoxPelanggan.FormattingEnabled = true;
            comboBoxPelanggan.Location = new Point(152, 120);
            comboBoxPelanggan.Name = "comboBoxPelanggan";
            comboBoxPelanggan.Size = new Size(182, 33);
            comboBoxPelanggan.TabIndex = 7;
            // 
            // Form3_crud
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1490, 668);
            Controls.Add(comboBoxPelanggan);
            Controls.Add(dateTimePicker1);
            Controls.Add(btnAdd);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridViewCart);
            Controls.Add(txtNoTransaksi);
            Name = "Form3_crud";
            Text = "Form3_crud";
            Load += Form3_crud_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNoTransaksi;
        private DataGridView dataGridViewCart;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btnAdd;
        private DateTimePicker dateTimePicker1;
        private ComboBox comboBoxPelanggan;
    }
}