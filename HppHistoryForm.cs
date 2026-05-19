using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.Repositories;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class HppHistoryForm : Form
    {
        private readonly int _itemId;
        private string _method = "AVG";
        private string _baseUnit = "pcs";
        private readonly bool _openStockCard;
        private int _layersPage = 1;
        private int _layersPageSize = 200;
        private int _layersTotalRows = 0;
        private int _stockPage = 1;
        private int _stockPageSize = 200;
        private int _stockTotalRows = 0;

        public HppHistoryForm(int itemId, bool openStockCard = false)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            _itemId = itemId;
            _openStockCard = openStockCard;

            Load += HppHistoryForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadData();
            btnRevalue.Click += (s, e) => RevalueSelectedLayer();
            cmbWarehouse.SelectedIndexChanged += (s, e) => LoadData();
            dgvLayers.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                RevalueSelectedLayer();
            };

            cmbPageSizeLayers.SelectedIndexChanged += (s, e) =>
            {
                if (int.TryParse(cmbPageSizeLayers.SelectedItem?.ToString(), out var v) && v > 0)
                    _layersPageSize = v;
                _layersPage = 1;
                LoadData();
            };

            cmbPageSizeStock.SelectedIndexChanged += (s, e) =>
            {
                if (int.TryParse(cmbPageSizeStock.SelectedItem?.ToString(), out var v) && v > 0)
                    _stockPageSize = v;
                _stockPage = 1;
                LoadData();
            };

            btnSearchLayers.Click += (s, e) => { _layersPage = 1; LoadData(); };
            btnResetLayers.Click += (s, e) =>
            {
                txtSearchLayers.Text = "";
                dtLayersFrom.Checked = false;
                dtLayersTo.Checked = false;
                _layersPage = 1;
                LoadData();
            };
            btnPrevLayers.Click += (s, e) => { if (_layersPage > 1) { _layersPage--; LoadData(); } };
            btnNextLayers.Click += (s, e) =>
            {
                int maxPage = GetMaxPage(_layersTotalRows, _layersPageSize);
                if (_layersPage < maxPage) { _layersPage++; LoadData(); }
            };

            btnSearchStock.Click += (s, e) => { _stockPage = 1; LoadData(); };
            btnResetStock.Click += (s, e) =>
            {
                txtSearchStock.Text = "";
                dtStockFrom.Checked = false;
                dtStockTo.Checked = false;
                chkShowAllocation.Checked = true;
                _stockPage = 1;
                LoadData();
            };
            btnPrevStock.Click += (s, e) => { if (_stockPage > 1) { _stockPage--; LoadData(); } };
            btnNextStock.Click += (s, e) =>
            {
                int maxPage = GetMaxPage(_stockTotalRows, _stockPageSize);
                if (_stockPage < maxPage) { _stockPage++; LoadData(); }
            };
            chkShowAllocation.CheckedChanged += (s, e) => { _stockPage = 1; LoadData(); };
            dtStockFrom.ValueChanged += (s, e) => { if (dtStockFrom.Checked) { _stockPage = 1; LoadData(); } };
            dtStockTo.ValueChanged += (s, e) => { if (dtStockTo.Checked) { _stockPage = 1; LoadData(); } };
            dtLayersFrom.ValueChanged += (s, e) => { if (dtLayersFrom.Checked) { _layersPage = 1; LoadData(); } };
            dtLayersTo.ValueChanged += (s, e) => { if (dtLayersTo.Checked) { _layersPage = 1; LoadData(); } };
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
    COALESCE(NULLIF(i.valuation_method,''),'AVG') AS valuation_method,
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
                _method = "AVG";
                lblMethod.Text = "Method: AVG";
                _baseUnit = "pcs";
                return;
            }

            string name = r["name"]?.ToString() ?? "-";
            _method = r["valuation_method"]?.ToString() ?? "AVG";
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

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            LoadLayersPage(con, warehouseId);
            LoadStockCard(con, warehouseId);
        }

        private static int GetMaxPage(int totalRows, int pageSize)
        {
            if (pageSize <= 0) return 1;
            if (totalRows <= 0) return 1;
            return (int)Math.Ceiling(totalRows / (double)pageSize);
        }

        private static DateTime? GetCheckedDate(DateTimePicker p)
        {
            if (p == null) return null;
            if (!p.Checked) return null;
            return p.Value.Date;
        }

        private void LoadLayersPage(NpgsqlConnection con, int warehouseId)
        {
            string orderBy = GetOrderByForMethod(_method);
            string whFilter = warehouseId > 0 ? " AND sl.warehouse_id = @wh " : "";
            string q = (txtSearchLayers.Text ?? "").Trim();
            string qFilter = string.IsNullOrWhiteSpace(q)
                ? ""
                : " AND (CAST(sl.id AS text) ILIKE @q OR CAST(sl.buy_price AS text) ILIKE @q OR CAST(sl.qty_remaining AS text) ILIKE @q OR COALESCE(w.name,'') ILIKE @q) ";

            DateTime? from = GetCheckedDate(dtLayersFrom);
            DateTime? to = GetCheckedDate(dtLayersTo);
            string fromFilter = from.HasValue ? " AND sl.created_at >= @from " : "";
            string toFilter = to.HasValue ? " AND sl.created_at < @to " : "";

            using (var cmdCount = new NpgsqlCommand($@"
SELECT COUNT(*)
FROM stock_layers sl
JOIN warehouses w ON w.id = sl.warehouse_id
WHERE sl.item_id = @item_id
  AND sl.qty_remaining <> 0
{whFilter}
{fromFilter}
{toFilter}
{qFilter}
", con))
            {
                cmdCount.Parameters.AddWithValue("@item_id", _itemId);
                if (warehouseId > 0) cmdCount.Parameters.AddWithValue("@wh", warehouseId);
                if (from.HasValue) cmdCount.Parameters.AddWithValue("@from", from.Value);
                if (to.HasValue) cmdCount.Parameters.AddWithValue("@to", to.Value.AddDays(1));
                if (!string.IsNullOrWhiteSpace(q)) cmdCount.Parameters.AddWithValue("@q", "%" + q + "%");
                var obj = cmdCount.ExecuteScalar();
                _layersTotalRows = obj != null && obj != DBNull.Value ? Convert.ToInt32(obj) : 0;
            }

            using (var cmdSum = new NpgsqlCommand($@"
SELECT
    COALESCE(SUM(sl.qty_remaining),0) AS qty_total,
    COALESCE(SUM(sl.qty_remaining * sl.buy_price),0) AS value_total
FROM stock_layers sl
JOIN warehouses w ON w.id = sl.warehouse_id
WHERE sl.item_id = @item_id
  AND sl.qty_remaining <> 0
{whFilter}
{fromFilter}
{toFilter}
{qFilter}
", con))
            {
                cmdSum.Parameters.AddWithValue("@item_id", _itemId);
                if (warehouseId > 0) cmdSum.Parameters.AddWithValue("@wh", warehouseId);
                if (from.HasValue) cmdSum.Parameters.AddWithValue("@from", from.Value);
                if (to.HasValue) cmdSum.Parameters.AddWithValue("@to", to.Value.AddDays(1));
                if (!string.IsNullOrWhiteSpace(q)) cmdSum.Parameters.AddWithValue("@q", "%" + q + "%");
                using var r = cmdSum.ExecuteReader();
                decimal qtyTotal = 0m;
                decimal valueTotal = 0m;
                if (r.Read())
                {
                    qtyTotal = r["qty_total"] != DBNull.Value ? Convert.ToDecimal(r["qty_total"]) : 0m;
                    valueTotal = r["value_total"] != DBNull.Value ? Convert.ToDecimal(r["value_total"]) : 0m;
                }
                lblSummary.Text = $"Total: Qty {qtyTotal:N2} | Rp {valueTotal:N0}";
            }

            int maxPage = GetMaxPage(_layersTotalRows, _layersPageSize);
            if (_layersPage > maxPage) _layersPage = maxPage;
            if (_layersPage < 1) _layersPage = 1;
            int offset = (_layersPage - 1) * _layersPageSize;

            string sql = $@"
SELECT
    sl.id,
    sl.warehouse_id,
    sl.qty_initial,
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
{fromFilter}
{toFilter}
{qFilter}
ORDER BY {orderBy}
LIMIT @limit OFFSET @offset";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@item_id", _itemId);
            cmd.Parameters.AddWithValue("@limit", _layersPageSize);
            cmd.Parameters.AddWithValue("@offset", offset);
            if (warehouseId > 0) cmd.Parameters.AddWithValue("@wh", warehouseId);
            if (from.HasValue) cmd.Parameters.AddWithValue("@from", from.Value);
            if (to.HasValue) cmd.Parameters.AddWithValue("@to", to.Value.AddDays(1));
            if (!string.IsNullOrWhiteSpace(q)) cmd.Parameters.AddWithValue("@q", "%" + q + "%");

            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            dgvLayers.DataSource = dt;

            if (dgvLayers.Columns.Contains("id")) dgvLayers.Columns["id"].Visible = false;
            if (dgvLayers.Columns.Contains("warehouse_id")) dgvLayers.Columns["warehouse_id"].Visible = false;
            if (dgvLayers.Columns.Contains("qty_initial")) dgvLayers.Columns["qty_initial"].Visible = false;
            if (dgvLayers.Columns.Contains("warehouse"))
            {
                dgvLayers.Columns["warehouse"].HeaderText = "Gudang";
                dgvLayers.Columns["warehouse"].Width = 180;
            }
            if (dgvLayers.Columns.Contains("created_at"))
            {
                dgvLayers.Columns["created_at"].HeaderText = "Masuk";
                dgvLayers.Columns["created_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
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

            lblPageLayers.Text = $"Page {_layersPage}/{maxPage} | {_layersTotalRows:N0} rows";
            btnPrevLayers.Enabled = _layersPage > 1;
            btnNextLayers.Enabled = _layersPage < maxPage;
        }

        private void RevalueSelectedLayer()
        {
            if (dgvLayers.DataSource == null || dgvLayers.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada stock layer untuk dikoreksi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgvLayers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih layer dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvLayers.SelectedRows[0];
            long layerId = 0;
            int warehouseId = 0;
            decimal oldPrice = 0m;
            decimal qtyRemain = 0m;
            decimal qtyInitial = 0m;
            try
            {
                if (row.Cells["id"]?.Value != null && row.Cells["id"].Value != DBNull.Value)
                    layerId = Convert.ToInt64(row.Cells["id"].Value);
                if (row.Cells["warehouse_id"]?.Value != null && row.Cells["warehouse_id"].Value != DBNull.Value)
                    warehouseId = Convert.ToInt32(row.Cells["warehouse_id"].Value);
                if (row.Cells["buy_price"]?.Value != null && row.Cells["buy_price"].Value != DBNull.Value)
                    oldPrice = Convert.ToDecimal(row.Cells["buy_price"].Value);
                if (row.Cells["qty_remaining"]?.Value != null && row.Cells["qty_remaining"].Value != DBNull.Value)
                    qtyRemain = Convert.ToDecimal(row.Cells["qty_remaining"].Value);
                if (row.Cells["qty_initial"]?.Value != null && row.Cells["qty_initial"].Value != DBNull.Value)
                    qtyInitial = Convert.ToDecimal(row.Cells["qty_initial"].Value);
            }
            catch
            {
                layerId = 0;
            }

            if (layerId <= 0 || warehouseId <= 0)
            {
                MessageBox.Show("Layer tidak valid.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var input = ShowRevalueModal(oldPrice);
            if (input == null) return;

            decimal newPrice = input.Value.newPrice;
            bool applyAllInWarehouse = input.Value.applyAllInWarehouse;
            bool updateItemDefault = input.Value.updateItemDefault;

            if (newPrice <= 0m)
            {
                MessageBox.Show("Harga baru harus lebih dari 0.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (newPrice == oldPrice && !updateItemDefault)
                return;

            var session = SessionUser.GetCurrentUser();
            int userId = session?.UserId ?? 0;
            int? loginId = session?.LoginId;

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                int affectedLayers = 0;
                decimal affectedQty = 0m;
                decimal oldMin = oldPrice;
                decimal oldMax = oldPrice;
                long? logStockLayerId = null;
                decimal qtySold = 0m;
                if (applyAllInWarehouse)
                {
                    var layers = new System.Collections.Generic.List<(long Id, decimal QtyInitial, decimal QtyRemain, decimal BuyPrice, DateTime CreatedAt, DateTime? ExpiredAt)>();
                    using (var cmd = new NpgsqlCommand(@"
SELECT id, COALESCE(qty_initial,0), COALESCE(qty_remaining,0), COALESCE(buy_price,0), created_at, expired_at
FROM stock_layers
WHERE item_id = @item AND warehouse_id = @wh AND qty_remaining <> 0
ORDER BY created_at ASC, id ASC
FOR UPDATE
", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@item", _itemId);
                        cmd.Parameters.AddWithValue("@wh", warehouseId);
                        using var r = cmd.ExecuteReader();
                        while (r.Read())
                        {
                            layers.Add((
                                r.GetInt64(0),
                                ReadDecimal(r, 1),
                                ReadDecimal(r, 2),
                                ReadDecimal(r, 3),
                                r.IsDBNull(4) ? DateTime.Now : r.GetDateTime(4),
                                r.IsDBNull(5) ? (DateTime?)null : r.GetDateTime(5)
                            ));
                        }
                    }

                    foreach (var l in layers)
                    {
                        if (l.QtyRemain <= 0m) continue;
                        affectedQty += l.QtyRemain;
                        if (affectedLayers == 0)
                        {
                            oldMin = l.BuyPrice;
                            oldMax = l.BuyPrice;
                        }
                        else
                        {
                            if (l.BuyPrice < oldMin) oldMin = l.BuyPrice;
                            if (l.BuyPrice > oldMax) oldMax = l.BuyPrice;
                        }

                        bool isPartiallyUsed = l.QtyInitial > l.QtyRemain + 0.0001m;
                        if (isPartiallyUsed)
                        {
                            long newLayerId;
                            using (var ins = new NpgsqlCommand(@"
INSERT INTO stock_layers(item_id, warehouse_id, qty_initial, qty_remaining, buy_price, expired_at, created_at)
VALUES(@item, @wh, @q, @q, @bp, @exp, @created)
RETURNING id
", con, tran))
                            {
                                ins.Parameters.AddWithValue("@item", _itemId);
                                ins.Parameters.AddWithValue("@wh", warehouseId);
                                ins.Parameters.AddWithValue("@q", l.QtyRemain);
                                ins.Parameters.AddWithValue("@bp", newPrice);
                                ins.Parameters.AddWithValue("@exp", (object?)l.ExpiredAt ?? DBNull.Value);
                                ins.Parameters.AddWithValue("@created", l.CreatedAt);
                                newLayerId = Convert.ToInt64(ins.ExecuteScalar());
                            }

                            using (var upd = new NpgsqlCommand("UPDATE stock_layers SET qty_remaining = 0 WHERE id = @id", con, tran))
                            {
                                upd.Parameters.AddWithValue("@id", l.Id);
                                upd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (var upd = new NpgsqlCommand("UPDATE stock_layers SET buy_price = @bp WHERE id = @id", con, tran))
                            {
                                upd.Parameters.AddWithValue("@bp", newPrice);
                                upd.Parameters.AddWithValue("@id", l.Id);
                                upd.ExecuteNonQuery();
                            }
                        }
                        affectedLayers++;
                    }
                }
                else
                {
                    DateTime createdAt = DateTime.Now;
                    DateTime? expiredAt = null;
                    decimal dbQtyInitial = 0m;
                    decimal dbQtyRemain = 0m;
                    decimal dbBuyPrice = 0m;
                    using (var cmdRow = new NpgsqlCommand(@"
SELECT COALESCE(qty_initial,0), COALESCE(qty_remaining,0), COALESCE(buy_price,0), created_at, expired_at
FROM stock_layers
WHERE id = @id
FOR UPDATE
LIMIT 1
", con, tran))
                    {
                        cmdRow.Parameters.AddWithValue("@id", layerId);
                        using var r = cmdRow.ExecuteReader();
                        if (!r.Read())
                        {
                            MessageBox.Show("Layer tidak ditemukan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        dbQtyInitial = ReadDecimal(r, 0);
                        dbQtyRemain = ReadDecimal(r, 1);
                        dbBuyPrice = ReadDecimal(r, 2);
                        createdAt = r.IsDBNull(3) ? DateTime.Now : r.GetDateTime(3);
                        expiredAt = r.IsDBNull(4) ? (DateTime?)null : r.GetDateTime(4);
                    }

                    if (dbQtyRemain <= 0m)
                    {
                        MessageBox.Show("Layer ini qty sisa sudah 0, tidak bisa dikoreksi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (dbQtyInitial <= 0m)
                        dbQtyInitial = dbQtyRemain;

                    bool isPartiallyUsed = dbQtyInitial > dbQtyRemain + 0.0001m;
                    if (isPartiallyUsed)
                    {
                        qtySold = dbQtyInitial - dbQtyRemain;
                        long newLayerId;
                        using (var ins = new NpgsqlCommand(@"
INSERT INTO stock_layers(item_id, warehouse_id, qty_initial, qty_remaining, buy_price, expired_at, created_at)
VALUES(@item, @wh, @q, @q, @bp, @exp, @created)
RETURNING id
", con, tran))
                        {
                            ins.Parameters.AddWithValue("@item", _itemId);
                            ins.Parameters.AddWithValue("@wh", warehouseId);
                            ins.Parameters.AddWithValue("@q", dbQtyRemain);
                            ins.Parameters.AddWithValue("@bp", newPrice);
                            ins.Parameters.AddWithValue("@exp", (object?)expiredAt ?? DBNull.Value);
                            ins.Parameters.AddWithValue("@created", createdAt);
                            newLayerId = Convert.ToInt64(ins.ExecuteScalar());
                        }

                        using (var upd = new NpgsqlCommand("UPDATE stock_layers SET qty_remaining = 0 WHERE id = @id", con, tran))
                        {
                            upd.Parameters.AddWithValue("@id", layerId);
                            upd.ExecuteNonQuery();
                        }

                        affectedLayers = 1;
                        affectedQty = dbQtyRemain;
                        oldMin = dbBuyPrice;
                        oldMax = dbBuyPrice;
                        logStockLayerId = newLayerId;
                    }
                    else
                    {
                        using (var cmd = new NpgsqlCommand("UPDATE stock_layers SET buy_price = @new WHERE id = @id", con, tran))
                        {
                            cmd.Parameters.AddWithValue("@new", newPrice);
                            cmd.Parameters.AddWithValue("@id", layerId);
                            affectedLayers = cmd.ExecuteNonQuery();
                        }
                        affectedQty = dbQtyRemain;
                        oldMin = dbBuyPrice;
                        oldMax = dbBuyPrice;
                        logStockLayerId = layerId;
                    }
                }

                if (updateItemDefault)
                {
                    decimal oldItem = 0m;
                    using (var cmdOld = new NpgsqlCommand("SELECT COALESCE(buy_price,0) FROM items WHERE id = @id LIMIT 1", con, tran))
                    {
                        cmdOld.Parameters.AddWithValue("@id", _itemId);
                        var v = cmdOld.ExecuteScalar();
                        oldItem = v != null && v != DBNull.Value ? Convert.ToDecimal(v) : 0m;
                    }
                    using (var cmdUp = new NpgsqlCommand("UPDATE items SET buy_price = @p WHERE id = @id", con, tran))
                    {
                        cmdUp.Parameters.AddWithValue("@p", newPrice);
                        cmdUp.Parameters.AddWithValue("@id", _itemId);
                        cmdUp.ExecuteNonQuery();
                    }
                    if (oldItem != newPrice)
                    {
                        try
                        {
                            using var cmdHist = new NpgsqlCommand(@"
INSERT INTO item_buy_price_histories(item_id, old_price, new_price, source_type, source_id, changed_by)
VALUES(@item, @old, @new, 'revaluation', @src, @by)
", con, tran);
                            cmdHist.Parameters.AddWithValue("@item", _itemId);
                            cmdHist.Parameters.AddWithValue("@old", oldItem);
                            cmdHist.Parameters.AddWithValue("@new", newPrice);
                            cmdHist.Parameters.AddWithValue("@src", layerId > 0 ? (object)layerId : DBNull.Value);
                            cmdHist.Parameters.AddWithValue("@by", userId > 0 ? (object)userId : DBNull.Value);
                            cmdHist.ExecuteNonQuery();
                        }
                        catch
                        {
                        }
                    }
                }

                decimal sisaStock = GetCurrentStockInWarehouse(con, tran, _itemId, warehouseId);
                string scope = applyAllInWarehouse ? $"SEMUA layer sisa (Gudang #{warehouseId})" : $"Layer #{layerId}";
                string oldRange = applyAllInWarehouse && oldMin != oldMax
                    ? $"{oldMin:N0}..{oldMax:N0}"
                    : $"{oldPrice:N0}";
                string ket = qtySold > 0m
                    ? $"Koreksi HPP {scope} (sisa saja): QtyAwal {qtySold + affectedQty:N2}, Terjual {qtySold:N2}, Sisa {affectedQty:N2} | {oldRange} → {newPrice:N0}"
                    : $"Koreksi HPP {scope}: {oldRange} → {newPrice:N0} | Qty Sisa: {affectedQty:N2}";

                var repo = new TransactionRepo();
                repo.InsertStockLog(con, tran, new StockLog
                {
                    ProductId = _itemId,
                    TipeTransaksi = "revaluation",
                    QtyMasuk = 0m,
                    QtyKeluar = 0m,
                    SisaStock = sisaStock,
                    Keterangan = ket,
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    LoginId = loginId,
                    WarehouseId = warehouseId,
                    RefType = "REVALUATION",
                    RefId = null,
                    UnitCost = newPrice,
                    Method = _method,
                    StockLayerId = applyAllInWarehouse ? null : (logStockLayerId ?? layerId),
                    IsAllocation = false,
                    ParentId = null
                });

                tran.Commit();
                LoadData();
                tabMain.SelectedTab = tabStockCard;
                MessageBox.Show("Koreksi HPP berhasil.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                try { tran.Rollback(); } catch { }
                MessageBox.Show("Gagal koreksi HPP: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static decimal ReadDecimal(NpgsqlDataReader r, int ordinal)
        {
            if (r.IsDBNull(ordinal)) return 0m;
            var v = r.GetValue(ordinal);
            try
            {
                return Convert.ToDecimal(v);
            }
            catch
            {
                return 0m;
            }
        }

        private (decimal newPrice, bool applyAllInWarehouse, bool updateItemDefault)? ShowRevalueModal(decimal currentPrice)
        {
            using var modal = new Form
            {
                Text = "Koreksi HPP (Stock Layer)",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Size = new Size(460, 320)
            };

            var lbl = new Label
            {
                Text = $"HPP Sekarang: {currentPrice:N0}\r\nMasukkan HPP baru (untuk sisa stok):",
                Dock = DockStyle.Top,
                Height = 70
            };
            var num = new NumericUpDown
            {
                Dock = DockStyle.Top,
                DecimalPlaces = 0,
                Maximum = 1000000000000m,
                Minimum = 0m,
                Value = currentPrice < 0 ? 0 : currentPrice,
                ThousandsSeparator = true,
                Height = 34
            };
            var chkAll = new CheckBox
            {
                Text = "Terapkan ke semua layer sisa di gudang ini",
                Dock = DockStyle.Top,
                Height = 30
            };
            var chkDefault = new CheckBox
            {
                Text = "Update default harga beli di master item",
                Dock = DockStyle.Top,
                Height = 30,
                Checked = true
            };

            var footer = new Panel { Dock = DockStyle.Bottom, Height = 70, Padding = new Padding(12) };
            var btnOk = new Button
            {
                Text = "Simpan",
                Width = 140,
                Height = 40,
                BackColor = Color.FromArgb(0, 122, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 460 - 12 - 140 - 110 - 10,
                Top = 14
            };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCancel = new Button
            {
                Text = "Batal",
                Width = 110,
                Height = 40,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Left = 460 - 12 - 110,
                Top = 14
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnOk.Click += (_, __) => { modal.DialogResult = DialogResult.OK; modal.Close(); };
            btnCancel.Click += (_, __) => { modal.DialogResult = DialogResult.Cancel; modal.Close(); };

            footer.Controls.Add(btnOk);
            footer.Controls.Add(btnCancel);

            modal.Controls.Add(chkDefault);
            modal.Controls.Add(chkAll);
            modal.Controls.Add(num);
            modal.Controls.Add(lbl);
            modal.Controls.Add(footer);
            modal.AcceptButton = btnOk;
            modal.CancelButton = btnCancel;

            if (modal.ShowDialog(this) != DialogResult.OK) return null;
            return (num.Value, chkAll.Checked, chkDefault.Checked);
        }

        private static decimal GetCurrentStockInWarehouse(NpgsqlConnection con, NpgsqlTransaction tran, int itemId, int warehouseId)
        {
            try
            {
                using var cmd = new NpgsqlCommand(@"
SELECT COALESCE(
    (SELECT s.qty FROM stocks s WHERE s.item_id = @item AND s.warehouse_id = @wh),
    (SELECT COALESCE(SUM(sl.qty_remaining),0) FROM stock_layers sl WHERE sl.item_id = @item AND sl.warehouse_id = @wh),
    0
)
", con, tran);
                cmd.Parameters.AddWithValue("@item", itemId);
                cmd.Parameters.AddWithValue("@wh", warehouseId);
                var v = cmd.ExecuteScalar();
                return v != null && v != DBNull.Value ? Convert.ToDecimal(v) : 0m;
            }
            catch
            {
                return 0m;
            }
        }

        private void LoadStockCard(NpgsqlConnection con, int warehouseId)
        {
            bool hasAvgHistory = false;
            try
            {
                using var cmdHas = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.tables
WHERE table_schema = current_schema()
  AND table_name = 'stock_avg_history'
LIMIT 1
", con);
                hasAvgHistory = cmdHas.ExecuteScalar() != null;
            }
            catch
            {
                hasAvgHistory = false;
            }

            bool showAlloc = chkShowAllocation.Checked;
            string q = (txtSearchStock.Text ?? "").Trim();
            DateTime? from = GetCheckedDate(dtStockFrom);
            DateTime? to = GetCheckedDate(dtStockTo);

            string where = "WHERE 1=1 ";
            if (!showAlloc) where += " AND COALESCE(is_allocation, FALSE) = FALSE ";
            if (!string.IsNullOrWhiteSpace(q))
                where += " AND (COALESCE(keterangan,'') ILIKE @q OR COALESCE(supplier,'') ILIKE @q OR COALESCE(tipe_transaksi,'') ILIKE @q OR COALESCE(method,'') ILIKE @q) ";
            if (from.HasValue) where += " AND created_at >= @from ";
            if (to.HasValue) where += " AND created_at < @to ";

            string cte = hasAvgHistory
                ? @"
WITH data AS (
    SELECT
        sl.id::bigint AS id,
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
        sl.keterangan,
        NULL::numeric AS avg_before,
        NULL::numeric AS avg_after,
        NULL::numeric AS qty_before,
        NULL::numeric AS qty_in
    FROM stock_log sl
    LEFT JOIN warehouses w ON w.id = sl.warehouse_id
    LEFT JOIN suppliers s ON s.id = sl.supplier_id
    WHERE sl.product_id = @item_id
      AND (@wh = 0 OR sl.warehouse_id = @wh)

    UNION ALL

    SELECT
        h.id::bigint AS id,
        h.created_at,
        w.name AS warehouse,
        'avg_update' AS tipe_transaksi,
        0::numeric AS qty_masuk,
        0::numeric AS qty_keluar,
        h.qty_after AS sisa_stock,
        COALESCE(s.name,'') AS supplier,
        h.unit_cost_in AS unit_cost,
        'AVG' AS method,
        FALSE AS is_allocation,
        NULL::int AS parent_id,
        COALESCE(h.note,'AVG update') AS keterangan,
        h.avg_before,
        h.avg_after,
        h.qty_before,
        h.qty_in
    FROM stock_avg_history h
    LEFT JOIN warehouses w ON w.id = h.warehouse_id
    LEFT JOIN purchase_orders po
        ON h.ref_type = 'PURCHASE'
       AND po.id = h.ref_id
    LEFT JOIN suppliers s ON s.id = po.supplier_id
    WHERE h.item_id = @item_id
      AND (@wh = 0 OR h.warehouse_id = @wh)
)
"
                : @"
WITH data AS (
    SELECT
        sl.id::bigint AS id,
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
        sl.keterangan,
        NULL::numeric AS avg_before,
        NULL::numeric AS avg_after,
        NULL::numeric AS qty_before,
        NULL::numeric AS qty_in
    FROM stock_log sl
    LEFT JOIN warehouses w ON w.id = sl.warehouse_id
    LEFT JOIN suppliers s ON s.id = sl.supplier_id
    WHERE sl.product_id = @item_id
      AND (@wh = 0 OR sl.warehouse_id = @wh)
)
";

            using (var cmdCount = new NpgsqlCommand(cte + "SELECT COUNT(*) FROM data " + where, con))
            {
                cmdCount.Parameters.AddWithValue("@item_id", _itemId);
                cmdCount.Parameters.AddWithValue("@wh", warehouseId);
                if (!string.IsNullOrWhiteSpace(q)) cmdCount.Parameters.AddWithValue("@q", "%" + q + "%");
                if (from.HasValue) cmdCount.Parameters.AddWithValue("@from", from.Value);
                if (to.HasValue) cmdCount.Parameters.AddWithValue("@to", to.Value.AddDays(1));
                var obj = cmdCount.ExecuteScalar();
                _stockTotalRows = obj != null && obj != DBNull.Value ? Convert.ToInt32(obj) : 0;
            }

            int maxPage = GetMaxPage(_stockTotalRows, _stockPageSize);
            if (_stockPage > maxPage) _stockPage = maxPage;
            if (_stockPage < 1) _stockPage = 1;
            int offset = (_stockPage - 1) * _stockPageSize;

            using var cmd = new NpgsqlCommand(cte + @"
SELECT *
FROM data
" + where + @"
ORDER BY created_at ASC, id ASC
LIMIT @limit OFFSET @offset
", con);
            cmd.Parameters.AddWithValue("@item_id", _itemId);
            cmd.Parameters.AddWithValue("@wh", warehouseId);
            cmd.Parameters.AddWithValue("@limit", _stockPageSize);
            cmd.Parameters.AddWithValue("@offset", offset);
            if (!string.IsNullOrWhiteSpace(q)) cmd.Parameters.AddWithValue("@q", "%" + q + "%");
            if (from.HasValue) cmd.Parameters.AddWithValue("@from", from.Value);
            if (to.HasValue) cmd.Parameters.AddWithValue("@to", to.Value.AddDays(1));

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

            if (_stockPage == 1 && raw.Rows.Count > 0)
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
                else if (string.Equals(tipe, "avg_update", StringComparison.OrdinalIgnoreCase))
                {
                    decimal avgBefore = r.Table.Columns.Contains("avg_before") && r["avg_before"] != DBNull.Value ? Convert.ToDecimal(r["avg_before"]) : 0m;
                    decimal avgAfter = r.Table.Columns.Contains("avg_after") && r["avg_after"] != DBNull.Value ? Convert.ToDecimal(r["avg_after"]) : 0m;
                    decimal qtyBefore = r.Table.Columns.Contains("qty_before") && r["qty_before"] != DBNull.Value ? Convert.ToDecimal(r["qty_before"]) : 0m;
                    decimal qtyIn = r.Table.Columns.Contains("qty_in") && r["qty_in"] != DBNull.Value ? Convert.ToDecimal(r["qty_in"]) : 0m;
                    string core = $"AVG: {avgBefore:N0} -> {avgAfter:N0} | stok {qtyBefore:N0} +{qtyIn:N0} @ {hpp:N0}";
                    displayKet = string.IsNullOrWhiteSpace(ket) ? core : $"{ket} | {core}";
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
                dgvStockCard.Columns["tanggal"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
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

            int maxPageStock = GetMaxPage(_stockTotalRows, _stockPageSize);
            lblPageStock.Text = $"Page {_stockPage}/{maxPageStock} | {_stockTotalRows:N0} rows";
            btnPrevStock.Enabled = _stockPage > 1;
            btnNextStock.Enabled = _stockPage < maxPageStock;
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
