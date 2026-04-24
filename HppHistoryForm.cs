using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class HppHistoryForm : Form
    {
        private readonly int _itemId;
        private string _method = "FIFO";
        private string _baseUnit = "pcs";
        private readonly bool _openStockCard;

        public HppHistoryForm(int itemId, bool openStockCard = false)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            _itemId = itemId;
            _openStockCard = openStockCard;

            Load += HppHistoryForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadData();
            cmbWarehouse.SelectedIndexChanged += (s, e) => LoadData();
        }

        private void HppHistoryForm_Load(object sender, EventArgs e)
        {
            ApplyGridStyle(dgvLayers);
            ApplyGridStyle(dgvStockCard);
            LoadItemInfo();
            LoadWarehouses();
            if (_openStockCard)
                tabMain.SelectedTab = tabStockCard;
            LoadData();
        }

        private void LoadItemInfo()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT 
    i.name, 
    COALESCE(NULLIF(i.valuation_method,''),'FIFO') AS valuation_method,
    COALESCE(NULLIF(u.abbr,''), NULLIF(u.name,''), 'pcs') AS unit_name
FROM items i
LEFT JOIN units u ON u.id = i.unit
WHERE i.id=@id
LIMIT 1", con);
            cmd.Parameters.AddWithValue("@id", _itemId);
            using var r = cmd.ExecuteReader();
            if (!r.Read())
            {
                lblItem.Text = "Item: -";
                _method = "FIFO";
                lblMethod.Text = "Method: FIFO";
                _baseUnit = "pcs";
                return;
            }

            string name = r["name"]?.ToString() ?? "-";
            _method = r["valuation_method"]?.ToString() ?? "FIFO";
            _method = _method.Trim().ToUpperInvariant();
            _baseUnit = r["unit_name"]?.ToString() ?? "pcs";
            lblItem.Text = "Item: " + name;
            lblMethod.Text = "Method: " + _method;
        }

        private void LoadWarehouses()
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));

            var drAll = dt.NewRow();
            drAll["id"] = 0;
            drAll["name"] = "Semua Gudang";
            dt.Rows.Add(drAll);

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand("SELECT id, name FROM warehouses WHERE COALESCE(is_active, TRUE) = TRUE ORDER BY id ASC", con);
            using var da = new NpgsqlDataAdapter(cmd);
            var dtWh = new DataTable();
            da.Fill(dtWh);
            foreach (DataRow r in dtWh.Rows)
            {
                var dr = dt.NewRow();
                dr["id"] = Convert.ToInt32(r["id"]);
                dr["name"] = r["name"]?.ToString() ?? "";
                dt.Rows.Add(dr);
            }

            cmbWarehouse.DisplayMember = "name";
            cmbWarehouse.ValueMember = "id";
            cmbWarehouse.DataSource = dt;
            cmbWarehouse.SelectedValue = 0;
        }

        private void LoadData()
        {
            int warehouseId = 0;
            if (cmbWarehouse.SelectedValue != null)
                warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue);

            string orderBy = GetOrderByForMethod(_method);
            string whFilter = warehouseId > 0 ? " AND sl.warehouse_id = @wh " : "";

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = $@"
SELECT
    sl.id,
    w.name AS warehouse,
    sl.created_at,
    sl.expired_at,
    sl.qty_remaining,
    sl.buy_price,
    (sl.qty_remaining * sl.buy_price) AS value_remaining
FROM stock_layers sl
JOIN warehouses w ON w.id = sl.warehouse_id
WHERE sl.item_id = @item_id
  AND sl.qty_remaining <> 0
{whFilter}
ORDER BY {orderBy}";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@item_id", _itemId);
            if (warehouseId > 0)
                cmd.Parameters.AddWithValue("@wh", warehouseId);

            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            dgvLayers.DataSource = dt;

            if (dgvLayers.Columns.Contains("id")) dgvLayers.Columns["id"].Visible = false;
            if (dgvLayers.Columns.Contains("warehouse"))
            {
                dgvLayers.Columns["warehouse"].HeaderText = "Gudang";
                dgvLayers.Columns["warehouse"].Width = 180;
            }
            if (dgvLayers.Columns.Contains("created_at"))
            {
                dgvLayers.Columns["created_at"].HeaderText = "Masuk";
                dgvLayers.Columns["created_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvLayers.Columns["created_at"].Width = 160;
            }
            if (dgvLayers.Columns.Contains("expired_at"))
            {
                dgvLayers.Columns["expired_at"].HeaderText = "Expired";
                dgvLayers.Columns["expired_at"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvLayers.Columns["expired_at"].Width = 120;
            }
            if (dgvLayers.Columns.Contains("qty_remaining"))
            {
                dgvLayers.Columns["qty_remaining"].HeaderText = "Sisa Qty";
                dgvLayers.Columns["qty_remaining"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvLayers.Columns["qty_remaining"].DefaultCellStyle.Format = "N2";
                dgvLayers.Columns["qty_remaining"].Width = 110;
            }
            if (dgvLayers.Columns.Contains("buy_price"))
            {
                dgvLayers.Columns["buy_price"].HeaderText = "HPP";
                dgvLayers.Columns["buy_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvLayers.Columns["buy_price"].DefaultCellStyle.Format = "N0";
                dgvLayers.Columns["buy_price"].Width = 120;
            }
            if (dgvLayers.Columns.Contains("value_remaining"))
            {
                dgvLayers.Columns["value_remaining"].HeaderText = "Nilai (HPP)";
                dgvLayers.Columns["value_remaining"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvLayers.Columns["value_remaining"].DefaultCellStyle.Format = "N0";
                dgvLayers.Columns["value_remaining"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            decimal totalQty = 0m;
            decimal totalValue = 0m;
            foreach (DataRow row in dt.Rows)
            {
                if (row["qty_remaining"] != DBNull.Value)
                    totalQty += Convert.ToDecimal(row["qty_remaining"]);
                if (row["value_remaining"] != DBNull.Value)
                    totalValue += Convert.ToDecimal(row["value_remaining"]);
            }

            lblSummary.Text = $"Total: Qty {totalQty:N2} | Rp {totalValue:N0}";
            LoadStockCard(warehouseId);
        }

        private void LoadStockCard(int warehouseId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
SELECT
    sl.id,
    sl.created_at,
    w.name AS warehouse,
    sl.tipe_transaksi,
    sl.qty_masuk,
    sl.qty_keluar,
    sl.sisa_stock,
    COALESCE(s.name,'') AS supplier,
    sl.unit_cost,
    sl.method,
    sl.is_allocation,
    sl.parent_id,
    sl.keterangan
FROM stock_log sl
LEFT JOIN warehouses w ON w.id = sl.warehouse_id
LEFT JOIN suppliers s ON s.id = sl.supplier_id
WHERE sl.product_id = @item_id
  AND (@wh = 0 OR sl.warehouse_id = @wh)
ORDER BY sl.created_at ASC, sl.id ASC";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@item_id", _itemId);
            cmd.Parameters.AddWithValue("@wh", warehouseId);
            using var da = new NpgsqlDataAdapter(cmd);
            var raw = new DataTable();
            da.Fill(raw);

            var view = new DataTable();
            view.Columns.Add("tanggal", typeof(DateTime));
            view.Columns.Add("gudang", typeof(string));
            view.Columns.Add("keterangan", typeof(string));
            view.Columns.Add("masuk", typeof(decimal));
            view.Columns.Add("keluar", typeof(decimal));
            view.Columns.Add("saldo", typeof(decimal));
            view.Columns.Add("unit", typeof(string));
            view.Columns.Add("supplier", typeof(string));
            view.Columns.Add("hpp", typeof(decimal));
            view.Columns.Add("method", typeof(string));
            view.Columns.Add("is_allocation", typeof(bool));

            if (raw.Rows.Count > 0)
            {
                DataRow? firstSummary = null;
                foreach (DataRow r in raw.Rows)
                {
                    bool isAlloc = r["is_allocation"] != DBNull.Value && Convert.ToBoolean(r["is_allocation"]);
                    if (!isAlloc)
                    {
                        firstSummary = r;
                        break;
                    }
                }

                if (firstSummary != null)
                {
                    decimal firstSaldo = firstSummary["sisa_stock"] == DBNull.Value ? 0m : Convert.ToDecimal(firstSummary["sisa_stock"]);
                    decimal firstMasuk = firstSummary["qty_masuk"] == DBNull.Value ? 0m : Convert.ToDecimal(firstSummary["qty_masuk"]);
                    decimal firstKeluar = firstSummary["qty_keluar"] == DBNull.Value ? 0m : Convert.ToDecimal(firstSummary["qty_keluar"]);
                    decimal saldoAwal = firstSaldo - firstMasuk + firstKeluar;

                    var dr0 = view.NewRow();
                    dr0["tanggal"] = Convert.ToDateTime(firstSummary["created_at"]);
                    dr0["gudang"] = firstSummary["warehouse"]?.ToString() ?? "";
                    dr0["keterangan"] = "Saldo awal";
                    dr0["masuk"] = 0m;
                    dr0["keluar"] = 0m;
                    dr0["saldo"] = saldoAwal;
                    dr0["unit"] = _baseUnit;
                    dr0["supplier"] = "";
                    dr0["hpp"] = 0m;
                    dr0["method"] = "";
                    dr0["is_allocation"] = false;
                    view.Rows.Add(dr0);
                }
            }

            foreach (DataRow r in raw.Rows)
            {
                bool isAlloc = r["is_allocation"] != DBNull.Value && Convert.ToBoolean(r["is_allocation"]);
                string tipe = r["tipe_transaksi"]?.ToString() ?? "";
                string ket = r["keterangan"]?.ToString() ?? "";
                decimal masuk = r["qty_masuk"] == DBNull.Value ? 0m : Convert.ToDecimal(r["qty_masuk"]);
                decimal keluar = r["qty_keluar"] == DBNull.Value ? 0m : Convert.ToDecimal(r["qty_keluar"]);
                decimal saldo = r["sisa_stock"] == DBNull.Value ? 0m : Convert.ToDecimal(r["sisa_stock"]);
                decimal hpp = r["unit_cost"] == DBNull.Value ? 0m : Convert.ToDecimal(r["unit_cost"]);
                string method = r["method"]?.ToString() ?? "";
                string supplier = r["supplier"]?.ToString() ?? "";
                string wh = r["warehouse"]?.ToString() ?? "";

                string displayKet;
                if (isAlloc)
                {
                    string m = string.IsNullOrWhiteSpace(method) ? _method : method;
                    displayKet = $"   -{keluar:N0} @ {hpp:N0} | {m}";
                }
                else
                {
                    displayKet = !string.IsNullOrWhiteSpace(ket) ? ket : tipe;
                }

                var dr = view.NewRow();
                dr["tanggal"] = Convert.ToDateTime(r["created_at"]);
                dr["gudang"] = wh;
                dr["keterangan"] = displayKet;
                dr["masuk"] = masuk;
                dr["keluar"] = keluar;
                dr["saldo"] = saldo;
                dr["unit"] = _baseUnit;
                dr["supplier"] = supplier;
                dr["hpp"] = hpp;
                dr["method"] = method;
                dr["is_allocation"] = isAlloc;
                view.Rows.Add(dr);
            }

            dgvStockCard.DataSource = view;

            if (dgvStockCard.Columns.Contains("is_allocation"))
                dgvStockCard.Columns["is_allocation"].Visible = false;

            if (dgvStockCard.Columns.Contains("tanggal"))
            {
                dgvStockCard.Columns["tanggal"].HeaderText = "Tanggal";
                dgvStockCard.Columns["tanggal"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvStockCard.Columns["tanggal"].Width = 160;
            }
            if (dgvStockCard.Columns.Contains("gudang"))
            {
                dgvStockCard.Columns["gudang"].HeaderText = "Gudang";
                dgvStockCard.Columns["gudang"].Width = 160;
            }
            if (dgvStockCard.Columns.Contains("keterangan"))
            {
                dgvStockCard.Columns["keterangan"].HeaderText = "Keterangan";
                dgvStockCard.Columns["keterangan"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvStockCard.Columns.Contains("masuk"))
            {
                dgvStockCard.Columns["masuk"].HeaderText = "Masuk";
                dgvStockCard.Columns["masuk"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvStockCard.Columns["masuk"].DefaultCellStyle.Format = "N2";
                dgvStockCard.Columns["masuk"].Width = 110;
            }
            if (dgvStockCard.Columns.Contains("keluar"))
            {
                dgvStockCard.Columns["keluar"].HeaderText = "Keluar";
                dgvStockCard.Columns["keluar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvStockCard.Columns["keluar"].DefaultCellStyle.Format = "N2";
                dgvStockCard.Columns["keluar"].Width = 110;
            }
            if (dgvStockCard.Columns.Contains("saldo"))
            {
                dgvStockCard.Columns["saldo"].HeaderText = "Saldo";
                dgvStockCard.Columns["saldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvStockCard.Columns["saldo"].DefaultCellStyle.Format = "N2";
                dgvStockCard.Columns["saldo"].Width = 110;
            }
            if (dgvStockCard.Columns.Contains("unit"))
            {
                dgvStockCard.Columns["unit"].HeaderText = "Unit";
                dgvStockCard.Columns["unit"].Width = 80;
            }
            if (dgvStockCard.Columns.Contains("supplier"))
            {
                dgvStockCard.Columns["supplier"].HeaderText = "Supplier";
                dgvStockCard.Columns["supplier"].Width = 160;
            }
            if (dgvStockCard.Columns.Contains("hpp"))
            {
                dgvStockCard.Columns["hpp"].HeaderText = "HPP";
                dgvStockCard.Columns["hpp"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvStockCard.Columns["hpp"].DefaultCellStyle.Format = "N0";
                dgvStockCard.Columns["hpp"].Width = 120;
            }
            if (dgvStockCard.Columns.Contains("method"))
            {
                dgvStockCard.Columns["method"].HeaderText = "Method";
                dgvStockCard.Columns["method"].Width = 100;
            }

            foreach (DataGridViewRow row in dgvStockCard.Rows)
            {
                if (row.DataBoundItem is DataRowView drv)
                {
                    bool isAlloc = drv.Row["is_allocation"] != DBNull.Value && Convert.ToBoolean(drv.Row["is_allocation"]);
                    if (isAlloc)
                    {
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 120, 120);
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Italic);
                    }
                }
            }
        }

        private static string GetOrderByForMethod(string method)
        {
            method = (method ?? "FIFO").Trim().ToUpperInvariant();
            if (method == "LIFO") return "sl.created_at DESC, sl.id DESC";
            if (method == "FEFO") return "sl.expired_at ASC NULLS LAST, sl.created_at ASC, sl.id ASC";
            return "sl.created_at ASC, sl.id ASC";
        }

        private static void ApplyGridStyle(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.RowsDefaultCellStyle.Padding = new Padding(5);
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgv.RowTemplate.Height = 40;
        }
    }
}
