using Npgsql;
using POS_qu.Controllers;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class InventoryAdjustmentEntryForm : Form
    {
        private readonly InventoryAdjustmentDirection _direction;
        private readonly WarehouseController _warehouseController = new WarehouseController();
        private readonly ItemController _itemController = new ItemController();

        private readonly DataTable _dt = new DataTable();

        public int CreatedAdjustmentId { get; private set; }

        public InventoryAdjustmentEntryForm(InventoryAdjustmentDirection direction)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            _direction = direction;

            Load += InventoryAdjustmentEntryForm_Load;
            btnAddItem.Click += btnAddItem_Click;
            btnRemoveItem.Click += btnRemoveItem_Click;
            btnSave.Click += btnSave_Click;
            btnClose.Click += btnClose_Click;
        }

        private void InventoryAdjustmentEntryForm_Load(object sender, EventArgs e)
        {
            lblTitle.Text = _direction == InventoryAdjustmentDirection.In ? "Tambah Item Masuk (Adjustment IN)" : "Tambah Item Keluar (Adjustment OUT)";
            Text = lblTitle.Text;

            LoadWarehouses();
            LoadReasons();
            SetupGrid();
        }

        private void LoadWarehouses()
        {
            var dt = _warehouseController.GetWarehouses();
            cmbWarehouse.DataSource = dt;
            cmbWarehouse.DisplayMember = "name";
            cmbWarehouse.ValueMember = "id";
            if (dt.Rows.Count > 0) cmbWarehouse.SelectedIndex = 0;
        }

        private void LoadReasons()
        {
            cmbReason.Items.Clear();

            if (_direction == InventoryAdjustmentDirection.In)
            {
                cmbReason.Items.AddRange(new object[]
                {
                    "Bonus supplier",
                    "Barang hibah",
                    "Koreksi karena salah input sebelumnya",
                    "Produksi / hasil rakitan",
                    "Return dari customer",
                    "Barang ditemukan"
                });
            }
            else
            {
                cmbReason.Items.AddRange(new object[]
                {
                    "Barang expired",
                    "Barang rusak",
                    "Barang hilang",
                    "Sampel gratis",
                    "Pemakaian internal (kantor)",
                    "Koreksi"
                });
            }

            if (cmbReason.Items.Count > 0) cmbReason.SelectedIndex = 0;
        }

        private void SetupGrid()
        {
            _dt.Columns.Clear();
            _dt.Rows.Clear();

            _dt.Columns.Add("item_id", typeof(long));
            _dt.Columns.Add("barcode", typeof(string));
            _dt.Columns.Add("name", typeof(string));
            _dt.Columns.Add("unit_name", typeof(string));
            _dt.Columns.Add("qty", typeof(double));
            _dt.Columns.Add("buy_price", typeof(decimal));
            _dt.Columns.Add("note", typeof(string));

            dgvItems.AutoGenerateColumns = true;
            dgvItems.DataSource = _dt;
            dgvItems.AllowUserToAddRows = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.MultiSelect = false;
            dgvItems.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            if (dgvItems.Columns.Contains("item_id")) dgvItems.Columns["item_id"].Visible = false;

            if (dgvItems.Columns.Contains("barcode"))
            {
                dgvItems.Columns["barcode"].HeaderText = "Barcode";
                dgvItems.Columns["barcode"].ReadOnly = true;
                dgvItems.Columns["barcode"].Width = 160;
            }

            if (dgvItems.Columns.Contains("name"))
            {
                dgvItems.Columns["name"].HeaderText = "Nama";
                dgvItems.Columns["name"].ReadOnly = true;
                dgvItems.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvItems.Columns.Contains("unit_name"))
            {
                dgvItems.Columns["unit_name"].HeaderText = "Satuan";
                dgvItems.Columns["unit_name"].ReadOnly = true;
                dgvItems.Columns["unit_name"].Width = 100;
            }

            if (dgvItems.Columns.Contains("qty"))
            {
                dgvItems.Columns["qty"].HeaderText = "Qty";
                dgvItems.Columns["qty"].ReadOnly = false;
                dgvItems.Columns["qty"].Width = 90;
                dgvItems.Columns["qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvItems.Columns["qty"].DefaultCellStyle.Format = "N2";
            }

            if (dgvItems.Columns.Contains("buy_price"))
            {
                dgvItems.Columns["buy_price"].HeaderText = "HPP";
                dgvItems.Columns["buy_price"].ReadOnly = _direction != InventoryAdjustmentDirection.In;
                dgvItems.Columns["buy_price"].Width = 120;
                dgvItems.Columns["buy_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvItems.Columns["buy_price"].DefaultCellStyle.Format = "N0";
                dgvItems.Columns["buy_price"].Visible = _direction == InventoryAdjustmentDirection.In;
            }

            if (dgvItems.Columns.Contains("note"))
            {
                dgvItems.Columns["note"].HeaderText = "Catatan";
                dgvItems.Columns["note"].ReadOnly = false;
                dgvItems.Columns["note"].Width = 220;
            }

            ApplyGridStyle();
        }

        private void ApplyGridStyle()
        {
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvItems.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgvItems.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvItems.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            dgvItems.GridColor = Color.FromArgb(235, 235, 235);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            using var f = new SearchFormItem("");
            if (f.ShowDialog(this) != DialogResult.OK) return;
            if (f.SelectedItem == null || f.SelectedItem.id <= 0) return;

            long itemId = f.SelectedItem.id;
            var detail = _itemController.GetItemById((int)itemId);
            if (detail == null) return;

            var existing = _dt.AsEnumerable().FirstOrDefault(r => Convert.ToInt64(r["item_id"]) == itemId);
            if (existing != null)
            {
                var q = existing["qty"] != DBNull.Value ? Convert.ToDouble(existing["qty"]) : 0d;
                existing["qty"] = q + 1d;
                return;
            }

            var dr = _dt.NewRow();
            dr["item_id"] = itemId;
            dr["barcode"] = detail.barcode ?? "";
            dr["name"] = detail.name ?? "";
            dr["unit_name"] = detail.unit ?? "";
            dr["qty"] = 1d;
            dr["buy_price"] = detail.buy_price;
            dr["note"] = "";
            _dt.Rows.Add(dr);
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow == null) return;
            if (dgvItems.CurrentRow.IsNewRow) return;
            dgvItems.Rows.Remove(dgvItems.CurrentRow);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int warehouseId = GetSelectedWarehouseId();
            if (warehouseId <= 0)
            {
                MessageBox.Show("Pilih gudang.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var rows = _dt.AsEnumerable()
                .Select(r => new
                {
                    ItemId = Convert.ToInt64(r["item_id"]),
                    Barcode = r["barcode"]?.ToString() ?? "",
                    Name = r["name"]?.ToString() ?? "",
                    Qty = r["qty"] != DBNull.Value ? Convert.ToDouble(r["qty"]) : 0d,
                    BuyPrice = r["buy_price"] != DBNull.Value ? Convert.ToDecimal(r["buy_price"]) : 0m,
                    Note = r["note"]?.ToString() ?? ""
                })
                .Where(r => r.ItemId > 0 && r.Qty > 0.0000001)
                .ToList();

            if (rows.Count == 0)
            {
                MessageBox.Show("Tambah item dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string reason = (cmbReason.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Alasan wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var user = SessionUser.GetCurrentUser();
            int userId = user?.UserId ?? 0;
            if (userId <= 0)
            {
                MessageBox.Show("Session user tidak ditemukan. Silakan login ulang.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                EnsureTables(con, tran);

                string prefix = _direction == InventoryAdjustmentDirection.In ? "ADJIN" : "ADJOUT";
                string adjNo = GenerateAdjustmentNo(con, tran, prefix, dtpDate.Value.Date);

                using var cmdIns = new NpgsqlCommand(@"
INSERT INTO inventory_adjustments (adjustment_no, direction, adjustment_date, warehouse_id, reason, note, created_by, created_at)
VALUES (@no, @dir, @d, @w, @reason, @note, @by, NOW())
RETURNING id
", con, tran);
                cmdIns.Parameters.AddWithValue("@no", adjNo);
                cmdIns.Parameters.AddWithValue("@dir", _direction == InventoryAdjustmentDirection.In ? "IN" : "OUT");
                cmdIns.Parameters.AddWithValue("@d", dtpDate.Value.Date);
                cmdIns.Parameters.AddWithValue("@w", warehouseId);
                cmdIns.Parameters.AddWithValue("@reason", reason);
                cmdIns.Parameters.AddWithValue("@note", txtNote.Text ?? "");
                cmdIns.Parameters.AddWithValue("@by", userId);

                var newIdObj = cmdIns.ExecuteScalar();
                int adjId = newIdObj != null ? Convert.ToInt32(newIdObj) : 0;
                if (adjId <= 0) throw new InvalidOperationException("Gagal membuat dokumen.");

                foreach (var r in rows)
                {
                    if (_direction == InventoryAdjustmentDirection.Out)
                    {
                        var sys = GetSystemQty(con, tran, r.ItemId, warehouseId);
                        if (sys + 0.0000001 < r.Qty)
                            throw new InvalidOperationException($"Stock tidak cukup: {r.Name} (Sistem: {sys:N2}, Keluar: {r.Qty:N2})");
                    }

                    using var cmdItem = new NpgsqlCommand(@"
INSERT INTO inventory_adjustment_items (adjustment_id, item_id, qty, buy_price, note)
VALUES (@aid, @iid, @q, @bp, @n)
", con, tran);
                    cmdItem.Parameters.AddWithValue("@aid", adjId);
                    cmdItem.Parameters.AddWithValue("@iid", r.ItemId);
                    cmdItem.Parameters.AddWithValue("@q", r.Qty);
                    cmdItem.Parameters.AddWithValue("@bp", r.BuyPrice);
                    cmdItem.Parameters.AddWithValue("@n", r.Note ?? "");
                    cmdItem.ExecuteNonQuery();

                    if (_direction == InventoryAdjustmentDirection.In)
                    {
                        UpsertStockAdd(con, tran, r.ItemId, warehouseId, r.Qty);
                        InsertStockLayer(con, tran, r.ItemId, warehouseId, r.Qty, r.BuyPrice);
                    }
                    else
                    {
                        UpdateStockSubtract(con, tran, r.ItemId, warehouseId, r.Qty);
                        ConsumeStockLayers(con, tran, r.ItemId, warehouseId, r.Qty);
                    }
                }

                tran.Commit();
                CreatedAdjustmentId = adjId;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                try { tran.Rollback(); } catch { }
                MessageBox.Show("Gagal simpan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetSelectedWarehouseId()
        {
            if (cmbWarehouse.SelectedValue == null) return 0;
            if (int.TryParse(cmbWarehouse.SelectedValue.ToString(), out var id)) return id;
            return 0;
        }

        private void EnsureTables(NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS inventory_adjustments (
    id BIGSERIAL PRIMARY KEY,
    adjustment_no VARCHAR(30) NOT NULL UNIQUE,
    direction VARCHAR(10) NOT NULL,
    adjustment_date DATE NOT NULL,
    warehouse_id INT NULL REFERENCES warehouses(id) ON DELETE SET NULL,
    reason VARCHAR(100) NOT NULL,
    note TEXT NULL,
    created_by INT NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS inventory_adjustment_items (
    id BIGSERIAL PRIMARY KEY,
    adjustment_id BIGINT NOT NULL REFERENCES inventory_adjustments(id) ON DELETE CASCADE,
    item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE RESTRICT,
    qty DOUBLE PRECISION NOT NULL DEFAULT 0,
    buy_price NUMERIC(15,2) NULL,
    note TEXT NULL
);

CREATE INDEX IF NOT EXISTS idx_inventory_adjustments_dir_date ON inventory_adjustments(direction, adjustment_date);
CREATE INDEX IF NOT EXISTS idx_inventory_adjustment_items_adj ON inventory_adjustment_items(adjustment_id);
", con, tran);
            cmd.ExecuteNonQuery();
        }

        private string GenerateAdjustmentNo(NpgsqlConnection con, NpgsqlTransaction tran, string prefix, DateTime date)
        {
            string ymd = date.ToString("yyyyMMdd");
            using var cmd = new NpgsqlCommand(@"
SELECT COALESCE(MAX(adjustment_no), '')
FROM inventory_adjustments
WHERE adjustment_no LIKE @p
", con, tran);
            cmd.Parameters.AddWithValue("@p", prefix + "-" + ymd + "-%");
            var last = cmd.ExecuteScalar()?.ToString() ?? "";
            int n = 0;
            if (!string.IsNullOrWhiteSpace(last))
            {
                var parts = last.Split('-');
                if (parts.Length >= 3 && int.TryParse(parts[^1], out var v)) n = v;
            }
            n++;
            return $"{prefix}-{ymd}-{n:D4}";
        }

        private double GetSystemQty(NpgsqlConnection con, NpgsqlTransaction tran, long itemId, int warehouseId)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT COALESCE(qty, 0)
FROM stocks
WHERE item_id = @iid AND warehouse_id = @w
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@iid", itemId);
            cmd.Parameters.AddWithValue("@w", warehouseId);
            var v = cmd.ExecuteScalar();
            if (v == null || v == DBNull.Value) return 0d;
            return Convert.ToDouble(v);
        }

        private void UpsertStockAdd(NpgsqlConnection con, NpgsqlTransaction tran, long itemId, int warehouseId, double qty)
        {
            using var cmd = new NpgsqlCommand(@"
INSERT INTO stocks (item_id, warehouse_id, qty, reserved_qty)
VALUES (@iid, @w, @q, 0)
ON CONFLICT (item_id, warehouse_id)
DO UPDATE SET qty = stocks.qty + EXCLUDED.qty
", con, tran);
            cmd.Parameters.AddWithValue("@iid", itemId);
            cmd.Parameters.AddWithValue("@w", warehouseId);
            cmd.Parameters.AddWithValue("@q", qty);
            cmd.ExecuteNonQuery();
        }

        private void UpdateStockSubtract(NpgsqlConnection con, NpgsqlTransaction tran, long itemId, int warehouseId, double qty)
        {
            using var cmd = new NpgsqlCommand(@"
UPDATE stocks
SET qty = qty - @q
WHERE item_id = @iid AND warehouse_id = @w
", con, tran);
            cmd.Parameters.AddWithValue("@iid", itemId);
            cmd.Parameters.AddWithValue("@w", warehouseId);
            cmd.Parameters.AddWithValue("@q", qty);
            cmd.ExecuteNonQuery();
        }

        private void InsertStockLayer(NpgsqlConnection con, NpgsqlTransaction tran, long itemId, int warehouseId, double qty, decimal buyPrice)
        {
            using var cmd = new NpgsqlCommand(@"
INSERT INTO stock_layers (item_id, warehouse_id, qty_remaining, buy_price, created_at)
VALUES (@iid, @w, @q, @bp, NOW())
", con, tran);
            cmd.Parameters.AddWithValue("@iid", itemId);
            cmd.Parameters.AddWithValue("@w", warehouseId);
            cmd.Parameters.AddWithValue("@q", qty);
            cmd.Parameters.AddWithValue("@bp", buyPrice < 0 ? 0m : buyPrice);
            cmd.ExecuteNonQuery();
        }

        private void ConsumeStockLayers(NpgsqlConnection con, NpgsqlTransaction tran, long itemId, int warehouseId, double qtyOut)
        {
            double remaining = qtyOut;

            using var cmd = new NpgsqlCommand(@"
SELECT id, qty_remaining
FROM stock_layers
WHERE item_id = @iid AND warehouse_id = @w AND qty_remaining > 0
ORDER BY created_at ASC, id ASC
", con, tran);
            cmd.Parameters.AddWithValue("@iid", itemId);
            cmd.Parameters.AddWithValue("@w", warehouseId);

            var layers = new List<(long Id, double QtyRemaining)>();
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    long id = Convert.ToInt64(r["id"]);
                    double q = r["qty_remaining"] != DBNull.Value ? Convert.ToDouble(r["qty_remaining"]) : 0d;
                    if (q > 0) layers.Add((id, q));
                }
            }

            foreach (var l in layers)
            {
                if (remaining <= 0.0000001) break;
                var take = Math.Min(l.QtyRemaining, remaining);
                var newQty = l.QtyRemaining - take;

                using var upd = new NpgsqlCommand(@"
UPDATE stock_layers
SET qty_remaining = @q
WHERE id = @id
", con, tran);
                upd.Parameters.AddWithValue("@id", l.Id);
                upd.Parameters.AddWithValue("@q", newQty);
                upd.ExecuteNonQuery();

                remaining -= take;
            }

            if (remaining > 0.0000001)
                throw new InvalidOperationException("Stock layer tidak cukup (FIFO).");
        }
    }
}
