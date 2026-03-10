namespace POS_qu
{
    partial class ShortcutForm
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
            panelMain = new Panel();
            listShortcut = new ListView();
            label1 = new Label();
            btnClose = new Button();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.AliceBlue;
            panelMain.Controls.Add(btnClose);
            panelMain.Controls.Add(listShortcut);
            panelMain.Controls.Add(label1);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(20);
            panelMain.Size = new Size(1169, 688);
            panelMain.TabIndex = 0;
            // 
            // listShortcut
            // 
            listShortcut.BorderStyle = BorderStyle.None;
            listShortcut.Dock = DockStyle.Fill;
            listShortcut.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listShortcut.FullRowSelect = true;
            listShortcut.Location = new Point(20, 20);
            listShortcut.Name = "listShortcut";
            listShortcut.Size = new Size(1129, 648);
            listShortcut.TabIndex = 1;
            listShortcut.UseCompatibleStateImageBehavior = false;
            listShortcut.View = View.Details;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(425, 61);
            label1.Name = "label1";
            label1.Size = new Size(213, 31);
            label1.TabIndex = 0;
            label1.Text = "Keyboard Shortcuts";
            // 
            // btnClose
            // 
            btnClose.Location = new Point(1052, 23);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 29);
            btnClose.TabIndex = 2;
            btnClose.Text = "close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click_1;
            // 
            // ShortcutForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LavenderBlush;
            ClientSize = new Size(1169, 688);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ShortcutForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ShortcutForm";
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private ListView listShortcut;
        private Label label1;
        private Button btnClose;
    }
}