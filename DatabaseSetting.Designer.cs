namespace POS_qu
{
    partial class DatabaseSetting
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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label5 = new Label();
            textBox5 = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(96, 160);
            label1.Name = "label1";
            label1.Size = new Size(72, 25);
            label1.TabIndex = 0;
            label1.Text = "Host/IP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(96, 216);
            label2.Name = "label2";
            label2.Size = new Size(44, 25);
            label2.TabIndex = 0;
            label2.Text = "Port";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(96, 272);
            label3.Name = "label3";
            label3.Size = new Size(47, 25);
            label3.TabIndex = 0;
            label3.Text = "User";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(96, 328);
            label4.Name = "label4";
            label4.Size = new Size(87, 25);
            label4.TabIndex = 0;
            label4.Text = "Password";
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.ScrollBar;
            textBox1.Location = new Point(248, 152);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(304, 31);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.ScrollBar;
            textBox2.Location = new Point(248, 208);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(304, 31);
            textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            textBox3.BackColor = SystemColors.ScrollBar;
            textBox3.Location = new Point(248, 264);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(304, 31);
            textBox3.TabIndex = 1;
            // 
            // textBox4
            // 
            textBox4.BackColor = SystemColors.ScrollBar;
            textBox4.Location = new Point(248, 320);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(304, 31);
            textBox4.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(96, 408);
            button1.Name = "button1";
            button1.Size = new Size(184, 34);
            button1.TabIndex = 2;
            button1.Text = "Test Connection";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(1232, 216);
            button2.Name = "button2";
            button2.Size = new Size(184, 34);
            button2.TabIndex = 2;
            button2.Text = "Create Database";
            button2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(936, 176);
            label5.Name = "label5";
            label5.Size = new Size(135, 25);
            label5.TabIndex = 0;
            label5.Text = "Database name";
            // 
            // textBox5
            // 
            textBox5.BackColor = SystemColors.ScrollBar;
            textBox5.Location = new Point(1112, 168);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(304, 31);
            textBox5.TabIndex = 1;
            // 
            // DatabaseSetting
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1562, 700);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox5);
            Controls.Add(textBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label5);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "DatabaseSetting";
            Text = "DatabaseSetting";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button1;
        private Button button2;
        private Label label5;
        private TextBox textBox5;
    }
}