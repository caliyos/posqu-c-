namespace POS_qu
{
    partial class FormUpdateItem
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
            panelRoot = new Panel();
            panelCard = new Panel();
            lblTitle = new Label();
            lblName = new Label();
            lblPriceCaption = new Label();
            lblPrice = new Label();
            lblQtyCaption = new Label();
            numQty = new NumericUpDown();
            lblUnitCaption = new Label();
            lblSatuan = new Label();
            lblDiscCaption = new Label();
            numDiscPercent = new NumericUpDown();
            lblDiscAmountCaption = new Label();
            rdoDiscPercent = new RadioButton();
            rdoDiscAmount = new RadioButton();
            numDiscAmount = new NumericUpDown();
            lblNoteCaption = new Label();
            txtNote = new TextBox();
            lblLineTotalCaption = new Label();
            lblLineTotal = new Label();
            panelButtons = new Panel();
            btnDelete = new Button();
            btnCancel = new Button();
            btnUpdate = new Button();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDiscPercent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDiscAmount).BeginInit();
            panelRoot.SuspendLayout();
            panelCard.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelRoot
            // 
            panelRoot.BackColor = Color.FromArgb(245, 246, 250);
            panelRoot.Controls.Add(panelCard);
            panelRoot.Dock = DockStyle.Fill;
            panelRoot.Location = new Point(0, 0);
            panelRoot.Name = "panelRoot";
            panelRoot.Padding = new Padding(16);
            panelRoot.Size = new Size(520, 520);
            panelRoot.TabIndex = 0;
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.Controls.Add(panelButtons);
            panelCard.Controls.Add(lblLineTotal);
            panelCard.Controls.Add(lblLineTotalCaption);
            panelCard.Controls.Add(txtNote);
            panelCard.Controls.Add(lblNoteCaption);
            panelCard.Controls.Add(numDiscAmount);
            panelCard.Controls.Add(rdoDiscAmount);
            panelCard.Controls.Add(rdoDiscPercent);
            panelCard.Controls.Add(lblDiscAmountCaption);
            panelCard.Controls.Add(numDiscPercent);
            panelCard.Controls.Add(lblDiscCaption);
            panelCard.Controls.Add(lblSatuan);
            panelCard.Controls.Add(lblUnitCaption);
            panelCard.Controls.Add(numQty);
            panelCard.Controls.Add(lblQtyCaption);
            panelCard.Controls.Add(lblPrice);
            panelCard.Controls.Add(lblPriceCaption);
            panelCard.Controls.Add(lblName);
            panelCard.Controls.Add(lblTitle);
            panelCard.Dock = DockStyle.Fill;
            panelCard.Location = new Point(16, 16);
            panelCard.Name = "panelCard";
            panelCard.Padding = new Padding(16);
            panelCard.Size = new Size(488, 488);
            panelCard.TabIndex = 0;
            // 
            // numQty
            // 
            numQty.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            numQty.Location = new Point(18, 188);
            numQty.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new Size(220, 34);
            numQty.TabIndex = 3;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.FromArgb(40, 167, 69);
            btnUpdate.Dock = DockStyle.Right;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(140, 46);
            btnUpdate.TabIndex = 2;
            btnUpdate.Text = "Simpan";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click_1;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);
            btnDelete.Dock = DockStyle.Left;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 46);
            btnDelete.TabIndex = 0;
            btnDelete.Text = "Hapus";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click_1;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.Dock = DockStyle.Right;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(110, 46);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Batal";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.Location = new Point(18, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(117, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Edit Item";
            // 
            // lblName
            // 
            lblName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(51, 51, 51);
            lblName.Location = new Point(18, 62);
            lblName.Name = "lblName";
            lblName.Size = new Size(452, 32);
            lblName.TabIndex = 1;
            lblName.Text = "-";
            // 
            // lblPriceCaption
            // 
            lblPriceCaption.AutoSize = true;
            lblPriceCaption.Font = new Font("Segoe UI", 10F);
            lblPriceCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblPriceCaption.Location = new Point(18, 104);
            lblPriceCaption.Name = "lblPriceCaption";
            lblPriceCaption.Size = new Size(52, 23);
            lblPriceCaption.TabIndex = 2;
            lblPriceCaption.Text = "Harga";
            // 
            // lblPrice
            // 
            lblPrice.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblPrice.ForeColor = Color.FromArgb(0, 122, 255);
            lblPrice.Location = new Point(18, 128);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(220, 30);
            lblPrice.TabIndex = 3;
            lblPrice.Text = "Rp 0";
            // 
            // lblQtyCaption
            // 
            lblQtyCaption.AutoSize = true;
            lblQtyCaption.Font = new Font("Segoe UI", 10F);
            lblQtyCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblQtyCaption.Location = new Point(18, 162);
            lblQtyCaption.Name = "lblQtyCaption";
            lblQtyCaption.Size = new Size(36, 23);
            lblQtyCaption.TabIndex = 4;
            lblQtyCaption.Text = "Qty";
            // 
            // lblUnitCaption
            // 
            lblUnitCaption.AutoSize = true;
            lblUnitCaption.Font = new Font("Segoe UI", 10F);
            lblUnitCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblUnitCaption.Location = new Point(260, 162);
            lblUnitCaption.Name = "lblUnitCaption";
            lblUnitCaption.Size = new Size(56, 23);
            lblUnitCaption.TabIndex = 5;
            lblUnitCaption.Text = "Satuan";
            // 
            // lblSatuan
            // 
            lblSatuan.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblSatuan.ForeColor = Color.FromArgb(51, 51, 51);
            lblSatuan.Location = new Point(260, 188);
            lblSatuan.Name = "lblSatuan";
            lblSatuan.Size = new Size(210, 34);
            lblSatuan.TabIndex = 6;
            lblSatuan.Text = "-";
            lblSatuan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiscCaption
            // 
            lblDiscCaption.AutoSize = true;
            lblDiscCaption.Font = new Font("Segoe UI", 10F);
            lblDiscCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblDiscCaption.Location = new Point(18, 238);
            lblDiscCaption.Name = "lblDiscCaption";
            lblDiscCaption.Size = new Size(139, 23);
            lblDiscCaption.TabIndex = 7;
            lblDiscCaption.Text = "Diskon Item (%)";
            // 
            // numDiscPercent
            // 
            numDiscPercent.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            numDiscPercent.Location = new Point(18, 264);
            numDiscPercent.Name = "numDiscPercent";
            numDiscPercent.Size = new Size(220, 34);
            numDiscPercent.TabIndex = 8;
            // 
            // rdoDiscPercent
            // 
            rdoDiscPercent.AutoSize = true;
            rdoDiscPercent.Font = new Font("Segoe UI", 9F);
            rdoDiscPercent.ForeColor = Color.FromArgb(90, 90, 90);
            rdoDiscPercent.Location = new Point(160, 238);
            rdoDiscPercent.Name = "rdoDiscPercent";
            rdoDiscPercent.Size = new Size(78, 24);
            rdoDiscPercent.TabIndex = 16;
            rdoDiscPercent.TabStop = true;
            rdoDiscPercent.Text = "Percent";
            rdoDiscPercent.UseVisualStyleBackColor = true;
            // 
            // lblDiscAmountCaption
            // 
            lblDiscAmountCaption.AutoSize = true;
            lblDiscAmountCaption.Font = new Font("Segoe UI", 10F);
            lblDiscAmountCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblDiscAmountCaption.Location = new Point(260, 238);
            lblDiscAmountCaption.Name = "lblDiscAmountCaption";
            lblDiscAmountCaption.Size = new Size(107, 23);
            lblDiscAmountCaption.TabIndex = 9;
            lblDiscAmountCaption.Text = "Diskon (Rp)";
            // 
            // rdoDiscAmount
            // 
            rdoDiscAmount.AutoSize = true;
            rdoDiscAmount.Font = new Font("Segoe UI", 9F);
            rdoDiscAmount.ForeColor = Color.FromArgb(90, 90, 90);
            rdoDiscAmount.Location = new Point(372, 238);
            rdoDiscAmount.Name = "rdoDiscAmount";
            rdoDiscAmount.Size = new Size(86, 24);
            rdoDiscAmount.TabIndex = 17;
            rdoDiscAmount.TabStop = true;
            rdoDiscAmount.Text = "Nominal";
            rdoDiscAmount.UseVisualStyleBackColor = true;
            // 
            // numDiscAmount
            // 
            numDiscAmount.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            numDiscAmount.Location = new Point(260, 264);
            numDiscAmount.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numDiscAmount.Name = "numDiscAmount";
            numDiscAmount.Size = new Size(210, 34);
            numDiscAmount.TabIndex = 10;
            // 
            // lblNoteCaption
            // 
            lblNoteCaption.AutoSize = true;
            lblNoteCaption.Font = new Font("Segoe UI", 10F);
            lblNoteCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblNoteCaption.Location = new Point(18, 314);
            lblNoteCaption.Name = "lblNoteCaption";
            lblNoteCaption.Size = new Size(141, 23);
            lblNoteCaption.TabIndex = 11;
            lblNoteCaption.Text = "Keterangan Item";
            // 
            // txtNote
            // 
            txtNote.Font = new Font("Segoe UI", 11F);
            txtNote.Location = new Point(18, 340);
            txtNote.Multiline = true;
            txtNote.Name = "txtNote";
            txtNote.PlaceholderText = "contoh: tanpa sambal / catatan kasir";
            txtNote.Size = new Size(452, 58);
            txtNote.TabIndex = 12;
            // 
            // lblLineTotalCaption
            // 
            lblLineTotalCaption.AutoSize = true;
            lblLineTotalCaption.Font = new Font("Segoe UI", 10F);
            lblLineTotalCaption.ForeColor = Color.FromArgb(90, 90, 90);
            lblLineTotalCaption.Location = new Point(18, 414);
            lblLineTotalCaption.Name = "lblLineTotalCaption";
            lblLineTotalCaption.Size = new Size(86, 23);
            lblLineTotalCaption.TabIndex = 13;
            lblLineTotalCaption.Text = "Total Item";
            // 
            // lblLineTotal
            // 
            lblLineTotal.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblLineTotal.ForeColor = Color.FromArgb(40, 167, 69);
            lblLineTotal.Location = new Point(18, 440);
            lblLineTotal.Name = "lblLineTotal";
            lblLineTotal.Size = new Size(452, 32);
            lblLineTotal.TabIndex = 14;
            lblLineTotal.Text = "Rp 0";
            lblLineTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnUpdate);
            panelButtons.Controls.Add(btnDelete);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(16, 426);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(456, 46);
            panelButtons.TabIndex = 15;
            // 
            // FormUpdateItem
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 520);
            Controls.Add(panelRoot);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormUpdateItem";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Item";
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDiscPercent).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDiscAmount).EndInit();
            panelRoot.ResumeLayout(false);
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelRoot;
        private Panel panelCard;
        private Panel panelButtons;
        private Label lblTitle;
        private Label lblName;
        private Label lblPriceCaption;
        private Label lblPrice;
        private Label lblQtyCaption;
        private NumericUpDown numQty;
        private Label lblUnitCaption;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnCancel;
        private Label lblSatuan;
        private Label lblDiscCaption;
        private NumericUpDown numDiscPercent;
        private Label lblDiscAmountCaption;
        private RadioButton rdoDiscPercent;
        private RadioButton rdoDiscAmount;
        private NumericUpDown numDiscAmount;
        private Label lblNoteCaption;
        private TextBox txtNote;
        private Label lblLineTotalCaption;
        private Label lblLineTotal;
    }
}
