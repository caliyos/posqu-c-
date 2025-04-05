namespace POS_qu
{
    partial class Form2_crud
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
            label1 = new Label();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            textBox1 = new TextBox();
            label2 = new Label();
            textBox2 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            textBox3search = new TextBox();
            button6 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(120, 56);
            label1.Name = "label1";
            label1.Size = new Size(124, 25);
            label1.TabIndex = 0;
            label1.Text = "product name";
            // 
            // button1
            // 
            button1.Location = new Point(120, 416);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 1;
            button1.Text = "add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(696, 56);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(632, 384);
            dataGridView1.TabIndex = 2;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(264, 56);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(344, 31);
            textBox1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(120, 112);
            label2.Name = "label2";
            label2.Size = new Size(50, 25);
            label2.TabIndex = 0;
            label2.Text = "price";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(264, 112);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(344, 31);
            textBox2.TabIndex = 3;
            // 
            // button2
            // 
            button2.Location = new Point(248, 416);
            button2.Name = "button2";
            button2.Size = new Size(112, 34);
            button2.TabIndex = 1;
            button2.Text = "edit";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(384, 416);
            button3.Name = "button3";
            button3.Size = new Size(112, 34);
            button3.TabIndex = 1;
            button3.Text = "delete";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(512, 416);
            button4.Name = "button4";
            button4.Size = new Size(112, 34);
            button4.TabIndex = 1;
            button4.Text = "getdata";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(120, 464);
            button5.Name = "button5";
            button5.Size = new Size(112, 34);
            button5.TabIndex = 1;
            button5.Text = "clear";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // textBox3search
            // 
            textBox3search.Location = new Point(696, 456);
            textBox3search.Name = "textBox3search";
            textBox3search.Size = new Size(208, 31);
            textBox3search.TabIndex = 4;
            // 
            // button6
            // 
            button6.Location = new Point(928, 456);
            button6.Name = "button6";
            button6.Size = new Size(112, 34);
            button6.TabIndex = 5;
            button6.Text = "search";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // Form2_crud
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1401, 673);
            Controls.Add(button6);
            Controls.Add(textBox3search);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(dataGridView1);
            Controls.Add(label2);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "Form2_crud";
            Text = "Form2_crud";
            Load += Form2_crud_Load_1;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label label1;
        private Button button1;
        private DataGridView dataGridView1;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private TextBox textBox3search;
        private Button button6;
    }
}