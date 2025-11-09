namespace POS_qu
{
    partial class OrderLIst
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
            dgvOrders = new DataGridView();
            btnAddToCart = new Button();
            buttonDelOrder = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            SuspendLayout();
            // 
            // dgvOrders
            // 
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Location = new Point(32, 256);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.RowHeadersWidth = 62;
            dgvOrders.Size = new Size(1440, 464);
            dgvOrders.TabIndex = 0;
            // 
            // btnAddToCart
            // 
            btnAddToCart.Location = new Point(32, 192);
            btnAddToCart.Name = "btnAddToCart";
            btnAddToCart.Size = new Size(160, 56);
            btnAddToCart.TabIndex = 1;
            btnAddToCart.Text = "ADDTOCART";
            btnAddToCart.UseVisualStyleBackColor = true;
            btnAddToCart.Click += btnAddToCart_Click;
            // 
            // buttonDelOrder
            // 
            buttonDelOrder.Location = new Point(208, 192);
            buttonDelOrder.Name = "buttonDelOrder";
            buttonDelOrder.Size = new Size(160, 56);
            buttonDelOrder.TabIndex = 1;
            buttonDelOrder.Text = "DELETE_ORDER";
            buttonDelOrder.UseVisualStyleBackColor = true;
            buttonDelOrder.Click += btnDelOrder_Click;
            // 
            // OrderLIst
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1487, 737);
            Controls.Add(buttonDelOrder);
            Controls.Add(btnAddToCart);
            Controls.Add(dgvOrders);
            Name = "OrderLIst";
            Text = "OrderLIst";
            Load += OrderLIst_Load;
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvOrders;
        private Button btnAddToCart;
        private Button buttonDelOrder;
    }
}