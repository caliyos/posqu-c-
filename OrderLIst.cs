using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class OrderLIst : Form
    {
        private DataGridViewManager dgvManager;
        private OrderController orderController = new OrderController();

        public int SelectedOrderId { get; private set; } = -1;

        private TextBox txtSearch;
        private Label lblPageNumber;
        private Button btnFirst, btnPrev, btnNext, btnLast;
        private ComboBox cmbPageSize;

        public OrderLIst()
        {
            InitializeComponent();
        }

        private void OrderLIst_Load(object sender, EventArgs e)
        {
            InitializeControls();
            LoadOrders();
        }

        private void InitializeControls()
        {
            // Search box
            txtSearch = new TextBox { Location = new Point(10, 10), Width = 200 };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            this.Controls.Add(txtSearch);

            // Page size dropdown
            cmbPageSize = new ComboBox { Location = new Point(220, 10), Width = 60 };
            cmbPageSize.Items.AddRange(new object[] { 10, 25, 50, 100 });
            cmbPageSize.SelectedIndex = 0;
            cmbPageSize.SelectedIndexChanged += CmbPageSize_SelectedIndexChanged;
            this.Controls.Add(cmbPageSize);

            // Paging buttons
            btnFirst = new Button { Text = "<<", Location = new Point(300, 10), Size = new Size(40, 25) };
            btnPrev = new Button { Text = "<", Location = new Point(350, 10), Size = new Size(40, 25) };
            btnNext = new Button { Text = ">", Location = new Point(400, 10), Size = new Size(40, 25) };
            btnLast = new Button { Text = ">>", Location = new Point(450, 10), Size = new Size(40, 25) };
            this.Controls.AddRange(new Control[] { btnFirst, btnPrev, btnNext, btnLast });

            btnFirst.Click += (s, e) => dgvManager.FirstPage();
            btnPrev.Click += (s, e) => dgvManager.PreviousPage();
            btnNext.Click += (s, e) => dgvManager.NextPage();
            btnLast.Click += (s, e) => dgvManager.LastPage();

            // Paging info label
            lblPageNumber = new Label { Location = new Point(500, 10), Size = new Size(100, 25), Text = "Page 1/1" };
            this.Controls.Add(lblPageNumber);

            // DataGridView setup
            dgvOrders.Location = new Point(10, 50);
            dgvOrders.Height = this.ClientSize.Height - 100;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.CellFormatting += DgvOrders_CellFormatting;
            dgvOrders.CellDoubleClick += DgvOrders_CellDoubleClick;
            dgvOrders.CurrentCellDirtyStateChanged += dgvOrders_CurrentCellDirtyStateChanged;
            dgvOrders.CellValueChanged += dgvOrders_CellValueChanged;
        }

        private void LoadOrders()
        {
            DataTable dtOrders = orderController.GetPendingOrders();

            // Bind ke DataGridViewManager
            dgvManager = new DataGridViewManager(dgvOrders, dtOrders, Convert.ToInt32(cmbPageSize.SelectedItem));
            dgvManager.PagingInfoLabel = lblPageNumber;

            // Hide and rename columns
            if (dgvOrders.Columns.Contains("order_id"))
                dgvOrders.Columns["order_id"].Visible = false;

            dgvOrders.Columns["order_number"].HeaderText = "Nomor Order";
            dgvOrders.Columns["customer_name"].HeaderText = "Pelanggan";
            dgvOrders.Columns["customer_phone"].HeaderText = "No. HP";
            dgvOrders.Columns["order_total"].HeaderText = "Total";
            dgvOrders.Columns["order_status"].HeaderText = "Status";
            dgvOrders.Columns["payment_method"].HeaderText = "Pembayaran";
            dgvOrders.Columns["delivery_method"].HeaderText = "Metode Delivery";
            dgvOrders.Columns["delivery_time"].HeaderText = "Waktu Delivery";
            dgvOrders.Columns["order_note"].HeaderText = "Catatan";
            dgvOrders.Columns["created_at"].HeaderText = "Tanggal";

            dgvOrders.Columns["order_total"].DefaultCellStyle.Format = "N2";
            dgvOrders.Columns["order_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Tambahkan kolom checkbox di posisi pertama
            if (!dgvOrders.Columns.Contains("Select"))
            {
                var chk = new DataGridViewCheckBoxColumn
                {
                    Name = "Select",
                    HeaderText = "",
                    Width = 30,
                    ReadOnly = false
                };
                dgvOrders.Columns.Insert(0, chk);
            }

            // Set default checkbox = false
            foreach (DataGridViewRow row in dgvOrders.Rows)
                row.Cells["Select"].Value = false;

            // Kolom lain readonly
            foreach (DataGridViewColumn col in dgvOrders.Columns)
                if (col.Name != "Select") col.ReadOnly = true;
        }

        private void dgvOrders_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOrders.IsCurrentCellDirty)
                dgvOrders.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvOrders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvOrders.Columns["Select"].Index)
            {
                bool checkedValue = Convert.ToBoolean(dgvOrders.Rows[e.RowIndex].Cells["Select"].Value);
                if (checkedValue)
                {
                    for (int i = 0; i < dgvOrders.Rows.Count; i++)
                    {
                        if (i != e.RowIndex)
                            dgvOrders.Rows[i].Cells["Select"].Value = false;
                    }
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string filter = $"order_number LIKE '%{txtSearch.Text}%' OR customer_name LIKE '%{txtSearch.Text}%' OR order_note LIKE '%{txtSearch.Text}%'";
            (dgvOrders.DataSource as DataTable).DefaultView.RowFilter = filter;
        }

        private void CmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvManager.SetPageSize(Convert.ToInt32(cmbPageSize.SelectedItem));
        }

        private void DgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvOrders.Columns[e.ColumnIndex].Name == "order_status" && e.Value != null)
            {
                int status = Convert.ToInt32(e.Value);
                switch (status)
                {
                    case 0: e.Value = "Pending"; e.CellStyle.BackColor = Color.LightYellow; break;
                    case 1: e.Value = "Paid"; e.CellStyle.BackColor = Color.LightGreen; break;
                    case 2: e.Value = "Cancelled"; e.CellStyle.BackColor = Color.LightCoral; break;
                    case 3: e.Value = "Processing"; e.CellStyle.BackColor = Color.LightBlue; break;
                }
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void DgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectedOrderId = Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells["order_id"].Value);
                if (SelectedOrderId > 0)
                {
                    var detailForm = new OrderDetailForm(SelectedOrderId);
                    detailForm.OnOrderSelected += (orderId) =>
                    {
                        var cashierForm = Application.OpenForms.OfType<Casher_POS>().FirstOrDefault();
                        if (cashierForm != null) cashierForm.SelectedOrderId = orderId;
                    };
                    detailForm.ShowDialog();
                }
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            var row = dgvOrders.Rows.Cast<DataGridViewRow>()
                        .FirstOrDefault(r => Convert.ToBoolean(r.Cells["Select"].Value));

            if (row == null)
            {
                MessageBox.Show("Pilih minimal 1 order!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SelectedOrderId = Convert.ToInt32(row.Cells["order_id"].Value);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
