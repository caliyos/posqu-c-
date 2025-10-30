using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_qu.Controllers;
using System;
using System.Drawing;
using System.Windows.Forms;
using POS_qu.Models;



namespace POS_qu
{
    public partial class OrderDetailForm : Form
    {
        public OrderDetailForm()
        {
            InitializeComponent();
        }
        private int _orderId;
        private OrderController _orderController;
        private List<OrderDetail> orderDetails;

        // Event untuk kirim OrderId ke CashierForm
        public event Action<int> OnOrderSelected;


        private void btnAddToPOS_Click(object sender, EventArgs e)
        {

            // Tutup form setelah dikirim
            this.Close();
        }

        public OrderDetailForm(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            _orderController = new OrderController();
        }

        private DataGridView dgvOrderItems;
        private Label lblOrderInfo;
        private Button btnClose;

        private void OrderDetailForm_Load(object sender, EventArgs e)
        {
            this.Text = "Detail Order";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            // Label untuk info order
            lblOrderInfo = new Label
            {
                AutoSize = false,
                Size = new Size(this.ClientSize.Width - 20, 80),
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblOrderInfo);

            // DataGridView untuk item
            dgvOrderItems = new DataGridView
            {
                Location = new Point(10, 100),
                Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 150),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvOrderItems);

            // Tombol Close
            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(this.ClientSize.Width - 100, this.ClientSize.Height - 40),
                Size = new Size(80, 30)
            };
            btnClose.Click += (s, eArgs) => this.Close();
            this.Controls.Add(btnClose);

            LoadOrderDetail();
        }

        private void LoadOrderDetail()
        {
            var order = _orderController.GetOrderById(_orderId);
            var orderDetails = _orderController.GetOrderDetails(_orderId);

            // Info order
            lblOrderInfo.Text =
                $"Order Number: {order.OrderNumber}\n" +
                $"Pelanggan: {order.CustomerName}\n" +
                $"Status: {GetStatusText(order.OrderStatus)}\n" +
                $"Pembayaran: {order.PaymentMethod}\n" +
                $"Delivery: {order.DeliveryMethod}\n" +
                $"Catatan: {order.OrderNote}";

            // DataGridView columns
            dgvOrderItems.Columns.Clear();
            dgvOrderItems.Columns.Add("barcode", "Barcode");
            dgvOrderItems.Columns.Add("unit", "Unit");
            dgvOrderItems.Columns.Add("quantity", "Qty");
            dgvOrderItems.Columns.Add("price", "Price/Unit");
            dgvOrderItems.Columns.Add("subtotal", "Subtotal");
            dgvOrderItems.Columns.Add("discount", "Discount");
            dgvOrderItems.Columns.Add("tax", "Tax");
            dgvOrderItems.Columns.Add("total", "Total");

            dgvOrderItems.Rows.Clear();
            foreach (var item in orderDetails)
            {
                dgvOrderItems.Rows.Add(
                    item.Barcode,
                    item.Unit,
                    item.Quantity.ToString("N2"),
                    item.PricePerUnit.ToString("N2"),
                    item.SellPrice.ToString("N2"),
                    item.DiscountTotal.ToString("N2"),
                    item.Tax.ToString("N2"),
                    item.Total.ToString("N2")
                );
            }
        }

        private string GetStatusText(int status)
        {
            return status switch
            {
                0 => "Pending",
                1 => "Paid",
                2 => "Cancelled",
                3 => "Processing",
                _ => "Unknown"
            };
        }

        private void btnAddToPOS_Click_1(object sender, EventArgs e)
        {
            // Kirim SelectedOrderId ke CashierForm
            OnOrderSelected?.Invoke(_orderId);
            this.Close();
        }
    }
}
