using POS_qu.Repositories;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class PendingCartsAdminForm : Form
    {
        private readonly CartActivity _repo = new CartActivity();
        private bool _loading;

        public PendingCartsAdminForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;

            btnClose.Click += (_, __) => Close();
            btnRefresh.Click += (_, __) => LoadCarts();
            dgvCarts.SelectionChanged += (_, __) => LoadItemsForSelected();

            Load += (_, __) =>
            {
                EnsureSplitterSafe();
                LoadCarts();
            };
        }

        private void EnsureSplitterSafe()
        {
            if (splitContainer == null) return;
            try
            {
                int total = splitContainer.Height;
                int min1 = Math.Max(0, splitContainer.Panel1MinSize);
                int min2 = Math.Max(0, splitContainer.Panel2MinSize);

                int desired = (int)(total * 0.45);
                int maxAllowed = Math.Max(min1, total - min2);
                if (desired < min1) desired = min1;
                if (desired > maxAllowed) desired = maxAllowed;
                splitContainer.SplitterDistance = desired;
            }
            catch
            {
            }
        }

        private void LoadCarts()
        {
            if (_loading) return;
            _loading = true;
            try
            {
                var dt = _repo.GetPendingCartsAdmin();
                dgvCarts.DataSource = dt;

                if (dgvCarts.Columns.Contains("cart_session_code"))
                    dgvCarts.Columns["cart_session_code"].HeaderText = "Kode Cart";
                if (dgvCarts.Columns.Contains("terminal_name"))
                    dgvCarts.Columns["terminal_name"].HeaderText = "Terminal";
                if (dgvCarts.Columns.Contains("cashier_name"))
                    dgvCarts.Columns["cashier_name"].HeaderText = "Kasir";
                if (dgvCarts.Columns.Contains("warehouse_name"))
                    dgvCarts.Columns["warehouse_name"].HeaderText = "Gudang";
                if (dgvCarts.Columns.Contains("total_items"))
                    dgvCarts.Columns["total_items"].HeaderText = "Items";
                if (dgvCarts.Columns.Contains("grand_total"))
                {
                    dgvCarts.Columns["grand_total"].HeaderText = "Total";
                    dgvCarts.Columns["grand_total"].DefaultCellStyle.Format = "N0";
                    dgvCarts.Columns["grand_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvCarts.Columns.Contains("last_update"))
                {
                    dgvCarts.Columns["last_update"].HeaderText = "Last Update";
                    dgvCarts.Columns["last_update"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }
                if (dgvCarts.Columns.Contains("terminal_id")) dgvCarts.Columns["terminal_id"].Visible = false;
                if (dgvCarts.Columns.Contains("cashier_id")) dgvCarts.Columns["cashier_id"].Visible = false;
                if (dgvCarts.Columns.Contains("warehouse_id")) dgvCarts.Columns["warehouse_id"].Visible = false;

                if (dgvCarts.Rows.Count > 0)
                    dgvCarts.Rows[0].Selected = true;
                else
                    dgvItems.DataSource = new DataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load pending carts: " + ex.Message);
                dgvCarts.DataSource = new DataTable();
                dgvItems.DataSource = new DataTable();
            }
            finally
            {
                _loading = false;
                LoadItemsForSelected();
            }
        }

        private static int ToInt(object? v)
        {
            try
            {
                if (v == null || v == DBNull.Value) return 0;
                return Convert.ToInt32(v);
            }
            catch
            {
                return 0;
            }
        }

        private void LoadItemsForSelected()
        {
            if (_loading) return;
            if (dgvCarts.SelectedRows.Count == 0)
            {
                dgvItems.DataSource = new DataTable();
                lblDetail.Text = "Detail Keranjang";
                return;
            }

            var row = dgvCarts.SelectedRows[0];
            string code = row.Cells["cart_session_code"]?.Value?.ToString() ?? "";
            int terminalId = dgvCarts.Columns.Contains("terminal_id") ? ToInt(row.Cells["terminal_id"]?.Value) : 0;
            int cashierId = dgvCarts.Columns.Contains("cashier_id") ? ToInt(row.Cells["cashier_id"]?.Value) : 0;

            try
            {
                var dt = _repo.GetPendingCartItems(code, terminalId, cashierId);
                dgvItems.DataSource = dt;

                if (dgvItems.Columns.Contains("pt_id")) dgvItems.Columns["pt_id"].Visible = false;
                if (dgvItems.Columns.Contains("cart_session_code")) dgvItems.Columns["cart_session_code"].Visible = false;
                if (dgvItems.Columns.Contains("terminal_id")) dgvItems.Columns["terminal_id"].Visible = false;
                if (dgvItems.Columns.Contains("cashier_id")) dgvItems.Columns["cashier_id"].Visible = false;
                if (dgvItems.Columns.Contains("warehouse_id")) dgvItems.Columns["warehouse_id"].Visible = false;

                if (dgvItems.Columns.Contains("warehouse_name"))
                    dgvItems.Columns["warehouse_name"].HeaderText = "Gudang";
                if (dgvItems.Columns.Contains("name"))
                    dgvItems.Columns["name"].HeaderText = "Nama";
                if (dgvItems.Columns.Contains("barcode"))
                    dgvItems.Columns["barcode"].HeaderText = "Barcode";
                if (dgvItems.Columns.Contains("quantity"))
                {
                    dgvItems.Columns["quantity"].HeaderText = "Qty";
                    dgvItems.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["quantity"].DefaultCellStyle.Format = "N2";
                }
                if (dgvItems.Columns.Contains("unit"))
                    dgvItems.Columns["unit"].HeaderText = "Satuan";
                if (dgvItems.Columns.Contains("sell_price"))
                {
                    dgvItems.Columns["sell_price"].HeaderText = "Harga";
                    dgvItems.Columns["sell_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["sell_price"].DefaultCellStyle.Format = "N0";
                }
                if (dgvItems.Columns.Contains("discount_total"))
                {
                    dgvItems.Columns["discount_total"].HeaderText = "Diskon";
                    dgvItems.Columns["discount_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["discount_total"].DefaultCellStyle.Format = "N0";
                }
                if (dgvItems.Columns.Contains("tax"))
                {
                    dgvItems.Columns["tax"].HeaderText = "Pajak";
                    dgvItems.Columns["tax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["tax"].DefaultCellStyle.Format = "N0";
                }
                if (dgvItems.Columns.Contains("total"))
                {
                    dgvItems.Columns["total"].HeaderText = "Subtotal";
                    dgvItems.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItems.Columns["total"].DefaultCellStyle.Format = "N0";
                }
                if (dgvItems.Columns.Contains("note"))
                    dgvItems.Columns["note"].HeaderText = "Catatan";
                if (dgvItems.Columns.Contains("updated_at"))
                {
                    dgvItems.Columns["updated_at"].HeaderText = "Update";
                    dgvItems.Columns["updated_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }

                lblDetail.Text = "Detail Keranjang — " + code;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load detail cart: " + ex.Message);
                dgvItems.DataSource = new DataTable();
                lblDetail.Text = "Detail Keranjang";
            }
        }
    }
}
