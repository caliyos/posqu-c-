namespace POS_qu
{
    partial class CasherNew
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            btnOpenCashier = new Button();
            btnCloseCashier = new Button();
            button6 = new Button();
            btnPendingList = new Button();
            btnCustomer = new Button();
            lblShiftInfo = new Label();
            lblDateTime = new Label();
            lblSessionCode = new Label();
            tlpMain = new TableLayoutPanel();
            pnlLeft = new Panel();
            dataGridViewCart4 = new DataGridView();
            pnlLeftTop = new Panel();
            txtCariBarang = new TextBox();
            btnCustomTransaction = new Button();
            pnlRight = new Panel();
            flpInvoice = new FlowLayoutPanel();
            pnlRightBottom = new Panel();
            lblNumItemsText = new Label();
            labelNumOfItems = new Label();
            lblKembalianText = new Label();
            lblKembalian = new Label();
            lblTotalText = new Label();
            lblTotal = new Label();
            tlpButtons = new TableLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            pnlRightTop = new Panel();
            lblCurrentOrder = new Label();
            pnlHeader.SuspendLayout();
            tlpMain.SuspendLayout();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart4).BeginInit();
            pnlLeftTop.SuspendLayout();
            pnlRight.SuspendLayout();
            pnlRightBottom.SuspendLayout();
            tlpButtons.SuspendLayout();
            pnlRightTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(btnOpenCashier);
            pnlHeader.Controls.Add(btnCloseCashier);
            pnlHeader.Controls.Add(button6);
            pnlHeader.Controls.Add(btnPendingList);
            pnlHeader.Controls.Add(btnCustomer);
            pnlHeader.Controls.Add(lblShiftInfo);
            pnlHeader.Controls.Add(lblDateTime);
            pnlHeader.Controls.Add(lblSessionCode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(10);
            pnlHeader.Size = new Size(1366, 60);
            pnlHeader.TabIndex = 0;
            // 
            // btnOpenCashier
            // 
            btnOpenCashier.BackColor = Color.FromArgb(240, 240, 240);
            btnOpenCashier.FlatAppearance.BorderSize = 0;
            btnOpenCashier.FlatStyle = FlatStyle.Flat;
            btnOpenCashier.Location = new Point(10, 10);
            btnOpenCashier.Name = "btnOpenCashier";
            btnOpenCashier.Size = new Size(100, 40);
            btnOpenCashier.TabIndex = 0;
            btnOpenCashier.Text = "Buka Kasir";
            btnOpenCashier.UseVisualStyleBackColor = false;
            // 
            // btnCloseCashier
            // 
            btnCloseCashier.BackColor = Color.FromArgb(240, 240, 240);
            btnCloseCashier.FlatAppearance.BorderSize = 0;
            btnCloseCashier.FlatStyle = FlatStyle.Flat;
            btnCloseCashier.Location = new Point(115, 10);
            btnCloseCashier.Name = "btnCloseCashier";
            btnCloseCashier.Size = new Size(100, 40);
            btnCloseCashier.TabIndex = 1;
            btnCloseCashier.Text = "Tutup Kasir";
            btnCloseCashier.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(240, 240, 240);
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Location = new Point(220, 10);
            button6.Name = "button6";
            button6.Size = new Size(100, 40);
            button6.TabIndex = 2;
            button6.Text = "Shortcuts";
            button6.UseVisualStyleBackColor = false;
            // 
            // btnPendingList
            // 
            btnPendingList.BackColor = Color.FromArgb(240, 240, 240);
            btnPendingList.FlatAppearance.BorderSize = 0;
            btnPendingList.FlatStyle = FlatStyle.Flat;
            btnPendingList.Location = new Point(325, 10);
            btnPendingList.Name = "btnPendingList";
            btnPendingList.Size = new Size(100, 40);
            btnPendingList.TabIndex = 3;
            btnPendingList.Text = "Pending List";
            btnPendingList.UseVisualStyleBackColor = false;
            // 
            // btnCustomer
            // 
            btnCustomer.BackColor = Color.FromArgb(255, 243, 205);
            btnCustomer.FlatAppearance.BorderSize = 0;
            btnCustomer.FlatStyle = FlatStyle.Flat;
            btnCustomer.Location = new Point(430, 10);
            btnCustomer.Name = "btnCustomer";
            btnCustomer.Size = new Size(120, 40);
            btnCustomer.TabIndex = 4;
            btnCustomer.Text = "Pelanggan (F4)";
            btnCustomer.UseVisualStyleBackColor = false;
            // 
            // lblShiftInfo
            // 
            lblShiftInfo.AutoSize = true;
            lblShiftInfo.Font = new Font("Segoe UI", 11F);
            lblShiftInfo.Location = new Point(560, 20);
            lblShiftInfo.Name = "lblShiftInfo";
            lblShiftInfo.Size = new Size(67, 25);
            lblShiftInfo.TabIndex = 5;
            lblShiftInfo.Text = "Shift: -";
            // 
            // lblDateTime
            // 
            lblDateTime.AutoSize = true;
            lblDateTime.Dock = DockStyle.Right;
            lblDateTime.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblDateTime.Location = new Point(1027, 10);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Padding = new Padding(0, 10, 20, 0);
            lblDateTime.Size = new Size(217, 35);
            lblDateTime.TabIndex = 6;
            lblDateTime.Text = "2026-03-27 12:00:00";
            // 
            // lblSessionCode
            // 
            lblSessionCode.AutoSize = true;
            lblSessionCode.Dock = DockStyle.Right;
            lblSessionCode.Font = new Font("Segoe UI", 11F);
            lblSessionCode.ForeColor = Color.Gray;
            lblSessionCode.Location = new Point(1244, 10);
            lblSessionCode.Name = "lblSessionCode";
            lblSessionCode.Padding = new Padding(0, 10, 20, 0);
            lblSessionCode.Size = new Size(112, 35);
            lblSessionCode.TabIndex = 7;
            lblSessionCode.Text = "Session: -";
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpMain.Controls.Add(pnlLeft, 0, 0);
            tlpMain.Controls.Add(pnlRight, 1, 0);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 60);
            tlpMain.Name = "tlpMain";
            tlpMain.Padding = new Padding(10);
            tlpMain.RowCount = 1;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.Size = new Size(1366, 708);
            tlpMain.TabIndex = 0;
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(dataGridViewCart4);
            pnlLeft.Controls.Add(pnlLeftTop);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Location = new Point(10, 10);
            pnlLeft.Margin = new Padding(0, 0, 10, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(864, 688);
            pnlLeft.TabIndex = 0;
            // 
            // dataGridViewCart4
            // 
            dataGridViewCart4.AllowUserToAddRows = false;
            dataGridViewCart4.AllowUserToDeleteRows = false;
            dataGridViewCart4.AllowUserToResizeColumns = false;
            dataGridViewCart4.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(252, 252, 252);
            dataGridViewCart4.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCart4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCart4.BackgroundColor = Color.White;
            dataGridViewCart4.BorderStyle = BorderStyle.None;
            dataGridViewCart4.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCart4.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(80, 80, 80);
            dataGridViewCellStyle2.Padding = new Padding(5, 0, 5, 0);
            dataGridViewCart4.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCart4.ColumnHeadersHeight = 50;
            dataGridViewCart4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(50, 50, 50);
            dataGridViewCellStyle3.Padding = new Padding(5, 0, 5, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(240, 248, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridViewCart4.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCart4.Dock = DockStyle.Fill;
            dataGridViewCart4.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewCart4.EnableHeadersVisualStyles = false;
            dataGridViewCart4.GridColor = Color.FromArgb(235, 235, 235);
            dataGridViewCart4.Location = new Point(0, 60);
            dataGridViewCart4.MultiSelect = false;
            dataGridViewCart4.Name = "dataGridViewCart4";
            dataGridViewCart4.ReadOnly = true;
            dataGridViewCart4.RowHeadersVisible = false;
            dataGridViewCart4.RowHeadersWidth = 51;
            dataGridViewCart4.RowTemplate.Height = 45;
            dataGridViewCart4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCart4.Size = new Size(864, 628);
            dataGridViewCart4.TabIndex = 0;
            // 
            // pnlLeftTop
            // 
            pnlLeftTop.Controls.Add(txtCariBarang);
            pnlLeftTop.Controls.Add(btnCustomTransaction);
            pnlLeftTop.Dock = DockStyle.Top;
            pnlLeftTop.Location = new Point(0, 0);
            pnlLeftTop.Name = "pnlLeftTop";
            pnlLeftTop.Padding = new Padding(0, 0, 0, 10);
            pnlLeftTop.Size = new Size(864, 60);
            pnlLeftTop.TabIndex = 1;
            // 
            // txtCariBarang
            // 
            txtCariBarang.BorderStyle = BorderStyle.FixedSingle;
            txtCariBarang.Dock = DockStyle.Fill;
            txtCariBarang.Font = new Font("Segoe UI", 18F);
            txtCariBarang.Location = new Point(0, 0);
            txtCariBarang.Name = "txtCariBarang";
            txtCariBarang.PlaceholderText = "Ketik nama barang atau scan barcode...";
            txtCariBarang.Size = new Size(714, 47);
            txtCariBarang.TabIndex = 0;
            // 
            // btnCustomTransaction
            // 
            btnCustomTransaction.BackColor = Color.FromArgb(108, 117, 125);
            btnCustomTransaction.Dock = DockStyle.Right;
            btnCustomTransaction.FlatAppearance.BorderSize = 0;
            btnCustomTransaction.FlatStyle = FlatStyle.Flat;
            btnCustomTransaction.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCustomTransaction.ForeColor = Color.White;
            btnCustomTransaction.Location = new Point(714, 0);
            btnCustomTransaction.Margin = new Padding(10, 0, 0, 0);
            btnCustomTransaction.Name = "btnCustomTransaction";
            btnCustomTransaction.Size = new Size(150, 50);
            btnCustomTransaction.TabIndex = 1;
            btnCustomTransaction.Text = "Custom Item";
            btnCustomTransaction.UseVisualStyleBackColor = false;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(flpInvoice);
            pnlRight.Controls.Add(pnlRightBottom);
            pnlRight.Controls.Add(pnlRightTop);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(887, 13);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(466, 682);
            pnlRight.TabIndex = 1;
            // 
            // flpInvoice
            // 
            flpInvoice.AutoScroll = true;
            flpInvoice.BackColor = Color.White;
            flpInvoice.Dock = DockStyle.Fill;
            flpInvoice.FlowDirection = FlowDirection.TopDown;
            flpInvoice.Location = new Point(0, 50);
            flpInvoice.Name = "flpInvoice";
            flpInvoice.Padding = new Padding(15);
            flpInvoice.Size = new Size(466, 352);
            flpInvoice.TabIndex = 0;
            flpInvoice.WrapContents = false;
            // 
            // pnlRightBottom
            // 
            pnlRightBottom.BackColor = Color.White;
            pnlRightBottom.Controls.Add(lblNumItemsText);
            pnlRightBottom.Controls.Add(labelNumOfItems);
            pnlRightBottom.Controls.Add(lblKembalianText);
            pnlRightBottom.Controls.Add(lblKembalian);
            pnlRightBottom.Controls.Add(lblTotalText);
            pnlRightBottom.Controls.Add(lblTotal);
            pnlRightBottom.Controls.Add(tlpButtons);
            pnlRightBottom.Dock = DockStyle.Bottom;
            pnlRightBottom.Location = new Point(0, 402);
            pnlRightBottom.Name = "pnlRightBottom";
            pnlRightBottom.Size = new Size(466, 280);
            pnlRightBottom.TabIndex = 1;
            // 
            // lblNumItemsText
            // 
            lblNumItemsText.AutoSize = true;
            lblNumItemsText.Font = new Font("Segoe UI", 11F);
            lblNumItemsText.Location = new Point(20, 15);
            lblNumItemsText.Name = "lblNumItemsText";
            lblNumItemsText.Size = new Size(61, 25);
            lblNumItemsText.TabIndex = 0;
            lblNumItemsText.Text = "Items:";
            // 
            // labelNumOfItems
            // 
            labelNumOfItems.AutoSize = true;
            labelNumOfItems.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            labelNumOfItems.Location = new Point(80, 15);
            labelNumOfItems.Name = "labelNumOfItems";
            labelNumOfItems.Size = new Size(23, 25);
            labelNumOfItems.TabIndex = 1;
            labelNumOfItems.Text = "0";
            // 
            // lblKembalianText
            // 
            lblKembalianText.AutoSize = true;
            lblKembalianText.Font = new Font("Segoe UI", 11F);
            lblKembalianText.Location = new Point(20, 45);
            lblKembalianText.Name = "lblKembalianText";
            lblKembalianText.Size = new Size(105, 25);
            lblKembalianText.TabIndex = 2;
            lblKembalianText.Text = "Kembalian:";
            // 
            // lblKembalian
            // 
            lblKembalian.AutoSize = true;
            lblKembalian.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblKembalian.ForeColor = Color.FromArgb(40, 167, 69);
            lblKembalian.Location = new Point(120, 45);
            lblKembalian.Name = "lblKembalian";
            lblKembalian.Size = new Size(23, 25);
            lblKembalian.TabIndex = 3;
            lblKembalian.Text = "0";
            // 
            // lblTotalText
            // 
            lblTotalText.AutoSize = true;
            lblTotalText.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalText.Location = new Point(20, 80);
            lblTotalText.Name = "lblTotalText";
            lblTotalText.Size = new Size(82, 37);
            lblTotalText.TabIndex = 4;
            lblTotalText.Text = "Total";
            // 
            // lblTotal
            // 
            lblTotal.Dock = DockStyle.Top;
            lblTotal.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(0, 122, 255);
            lblTotal.Location = new Point(0, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Padding = new Padding(0, 70, 20, 0);
            lblTotal.Size = new Size(466, 23);
            lblTotal.TabIndex = 5;
            lblTotal.Text = "0";
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tlpButtons
            // 
            tlpButtons.ColumnCount = 2;
            tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpButtons.Controls.Add(button1, 0, 0);
            tlpButtons.Controls.Add(button2, 0, 1);
            tlpButtons.Controls.Add(button3, 1, 1);
            tlpButtons.Controls.Add(button4, 0, 2);
            tlpButtons.Controls.Add(button5, 1, 2);
            tlpButtons.Dock = DockStyle.Bottom;
            tlpButtons.Location = new Point(0, 130);
            tlpButtons.Name = "tlpButtons";
            tlpButtons.Padding = new Padding(10);
            tlpButtons.RowCount = 3;
            tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlpButtons.Size = new Size(466, 150);
            tlpButtons.TabIndex = 6;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 122, 255);
            tlpButtons.SetColumnSpan(button1, 2);
            button1.Dock = DockStyle.Fill;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(12, 12);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(442, 48);
            button1.TabIndex = 0;
            button1.Text = "Bayar (Ctrl+P)";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 193, 7);
            button2.Dock = DockStyle.Fill;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            button2.ForeColor = Color.Black;
            button2.Location = new Point(12, 64);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(219, 35);
            button2.TabIndex = 1;
            button2.Text = "Simpan Draft";
            button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(220, 53, 69);
            button3.Dock = DockStyle.Fill;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            button3.ForeColor = Color.White;
            button3.Location = new Point(235, 64);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(219, 35);
            button3.TabIndex = 2;
            button3.Text = "Simpan Bon";
            button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(233, 236, 239);
            button4.Dock = DockStyle.Fill;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI", 10F);
            button4.ForeColor = Color.Black;
            button4.Location = new Point(12, 103);
            button4.Margin = new Padding(2);
            button4.Name = "button4";
            button4.Size = new Size(219, 35);
            button4.TabIndex = 3;
            button4.Text = "Lihat Draft";
            button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            button5.BackColor = Color.FromArgb(233, 236, 239);
            button5.Dock = DockStyle.Fill;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Segoe UI", 10F);
            button5.ForeColor = Color.Black;
            button5.Location = new Point(235, 103);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.Size = new Size(219, 35);
            button5.TabIndex = 4;
            button5.Text = "Lihat Bon";
            button5.UseVisualStyleBackColor = false;
            // 
            // pnlRightTop
            // 
            pnlRightTop.BackColor = Color.White;
            pnlRightTop.Controls.Add(lblCurrentOrder);
            pnlRightTop.Dock = DockStyle.Top;
            pnlRightTop.Location = new Point(0, 0);
            pnlRightTop.Name = "pnlRightTop";
            pnlRightTop.Padding = new Padding(15, 10, 15, 0);
            pnlRightTop.Size = new Size(466, 50);
            pnlRightTop.TabIndex = 2;
            // 
            // lblCurrentOrder
            // 
            lblCurrentOrder.AutoSize = true;
            lblCurrentOrder.Dock = DockStyle.Left;
            lblCurrentOrder.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblCurrentOrder.Location = new Point(15, 10);
            lblCurrentOrder.Name = "lblCurrentOrder";
            lblCurrentOrder.Size = new Size(174, 32);
            lblCurrentOrder.TabIndex = 0;
            lblCurrentOrder.Text = "Current Order";
            // 
            // CasherNew
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 246, 249);
            ClientSize = new Size(1366, 768);
            Controls.Add(tlpMain);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "CasherNew";
            Text = "Cashier - Moka POS Style";
            Load += CasherNew_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            tlpMain.ResumeLayout(false);
            pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewCart4).EndInit();
            pnlLeftTop.ResumeLayout(false);
            pnlLeftTop.PerformLayout();
            pnlRight.ResumeLayout(false);
            pnlRightBottom.ResumeLayout(false);
            pnlRightBottom.PerformLayout();
            tlpButtons.ResumeLayout(false);
            pnlRightTop.ResumeLayout(false);
            pnlRightTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblShiftInfo;
        private System.Windows.Forms.Label lblSessionCode;
        private System.Windows.Forms.Button btnOpenCashier;
        private System.Windows.Forms.Button btnCloseCashier;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnPendingList;
        private System.Windows.Forms.Button btnCustomer;

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlLeftTop;
        private System.Windows.Forms.TextBox txtCariBarang;
        private System.Windows.Forms.Button btnCustomTransaction;
        private System.Windows.Forms.DataGridView dataGridViewCart4;

        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlRightTop;
        private System.Windows.Forms.Label lblCurrentOrder;
        private System.Windows.Forms.FlowLayoutPanel flpInvoice;
        private System.Windows.Forms.Panel pnlRightBottom;
        
        private System.Windows.Forms.Label lblTotalText;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblKembalianText;
        private System.Windows.Forms.Label lblKembalian;
        private System.Windows.Forms.Label lblNumItemsText;
        private System.Windows.Forms.Label labelNumOfItems;

        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}
