using Npgsql;
using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class ReceivePOForm : Form
    {
        private enum ReceivePurchaseMode
        {
            FromApprovedPo = 0,
            DirectPurchase = 1
        }

        private readonly ItemController _itemController;

        private DataGridViewManager? _dgvItemManager;

        // Tombol navigasi
        private void btnNext_Click(object? sender, EventArgs e) => _dgvItemManager?.NextPage();
        private void btnPrev_Click(object? sender, EventArgs e) => _dgvItemManager?.PreviousPage();
        private void btnFirst_Click(object? sender, EventArgs e) => _dgvItemManager?.FirstPage();
        private void btnLast_Click(object? sender, EventArgs e) => _dgvItemManager?.LastPage();


        private int _poId = 0;
        private ReceivePurchaseMode _mode = ReceivePurchaseMode.FromApprovedPo;

        public ReceivePOForm()
        {
            _itemController = new ItemController(); // inisialisasi sekali
            InitializeComponent();
            LoadSuppliers();
            LoadStatuses();
            LoadWarehouses();
            InitOrderDetailsGrid();

            cmbMode.Items.Clear();
            cmbMode.Items.Add("Dari Pesanan Pembelian (PO Approved)");
            cmbMode.Items.Add("Beli Langsung (Tanpa PO)");
            cmbMode.SelectedIndexChanged += cmbMode_SelectedIndexChanged;
            cmbMode.SelectedIndex = 0;

            btnAddItem.Click += btnAddItem_Click;

            txtSearch.TextChanged += txtSearch_TextChanged;
            btnSave.Click += btnSave_Click;

            this.WindowState = FormWindowState.Maximized;
            dgvOrderDetails.CellEndEdit += dgvOrderDetails_CellEndEdit;

            ApplyMode();
        }

        private void LoadApprovedPOs()
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            // Ambil PO yang APPROVED untuk diterima
            string query = @"
                SELECT po.id, s.name as supplier_name, po.order_date, po.total_amount
                FROM purchase_orders po
                LEFT JOIN suppliers s ON po.supplier_id = s.id
                WHERE po.status = 'APPROVED'
                  AND (@search = '' OR s.name ILIKE @search OR po.id::text ILIKE @search)
                ORDER BY po.id DESC";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

            using var adapter = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);

            dgvItems.DataSource = dt;
            
            // Atur header
            if(dgvItems.Columns.Contains("id")) dgvItems.Columns["id"].HeaderText = "ID PO";
            if(dgvItems.Columns.Contains("supplier_name")) dgvItems.Columns["supplier_name"].HeaderText = "Supplier";
            if(dgvItems.Columns.Contains("order_date")) dgvItems.Columns["order_date"].HeaderText = "Tanggal";
            if(dgvItems.Columns.Contains("total_amount")) 
            {
                dgvItems.Columns["total_amount"].HeaderText = "Total";
                dgvItems.Columns["total_amount"].DefaultCellStyle.Format = "N0";
            }
        }

        private void txtSearch_TextChanged(object? sender, EventArgs e)
        {
            if (_mode == ReceivePurchaseMode.FromApprovedPo) LoadApprovedPOs();
            else LoadPurchasableItems();
        }

        private void cmbMode_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var idx = cmbMode.SelectedIndex;
            _mode = idx == 1 ? ReceivePurchaseMode.DirectPurchase : ReceivePurchaseMode.FromApprovedPo;
            ApplyMode();
        }

        private void ApplyMode()
        {
            _poId = 0;
            dgvOrderDetails.Rows.Clear();
            txtNote.Text = "";
            dtpOrderDate.Value = DateTime.Today;

            if (_mode == ReceivePurchaseMode.FromApprovedPo)
            {
                lblTitle.Text = "Pembelian / Penerimaan Barang (Dari PO)";
                Text = lblTitle.Text;

                lblSearch.Text = "Cari PO (Approved):";
                btnAddItem.Text = "Muat Data PO >";
                btnSave.Text = "Terima & Update Stok";
                button1.Text = "Daftar Pembelian";

                cmbSupplier.Enabled = false;
                dtpOrderDate.Enabled = false;

                LoadApprovedPOs();
            }
            else
            {
                lblTitle.Text = "Pembelian / Penerimaan Barang (Beli Langsung)";
                Text = lblTitle.Text;

                lblSearch.Text = "Cari Produk:";
                btnAddItem.Text = "Tambah Item >";
                btnSave.Text = "Simpan Pembelian & Update Stok";
                button1.Text = "Daftar Pembelian";

                cmbSupplier.Enabled = true;
                dtpOrderDate.Enabled = true;

                LoadPurchasableItems();
            }
        }

        private void btnLoadPO_Click(object? sender, EventArgs e)
        {
            if (dgvItems.CurrentRow == null)
            {
                MessageBox.Show("Pilih PO dari daftar sebelah kiri terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _poId = Convert.ToInt32(dgvItems.CurrentRow.Cells["id"].Value);
            
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            // Load Header
            string qHeader = "SELECT supplier_id, order_date, note FROM purchase_orders WHERE id = @id";
            using var cmdH = new NpgsqlCommand(qHeader, conn);
            cmdH.Parameters.AddWithValue("@id", _poId);
            using var reader = cmdH.ExecuteReader();
            if (reader.Read())
            {
                if (reader["supplier_id"] != DBNull.Value)
                    cmbSupplier.SelectedValue = reader["supplier_id"];
                
                if (reader["order_date"] != DBNull.Value)
                    dtpOrderDate.Value = Convert.ToDateTime(reader["order_date"]);
                    
                txtNote.Text = reader["note"].ToString();
            }
            reader.Close();

            // Load Items
            dgvOrderDetails.Rows.Clear();
            string qItems = @"
                SELECT poi.item_id, i.name, poi.quantity, poi.unit, poi.unit_price
                FROM purchase_order_items poi
                JOIN items i ON poi.item_id = i.id
                WHERE poi.po_id = @po_id";
                
            using var cmdI = new NpgsqlCommand(qItems, conn);
            cmdI.Parameters.AddWithValue("@po_id", _poId);
            using var rItems = cmdI.ExecuteReader();
            while (rItems.Read())
            {
                long itemId = Convert.ToInt64(rItems["item_id"]);
                string name = rItems["name"].ToString();
                decimal qty = Convert.ToDecimal(rItems["quantity"]);
                string unit = rItems["unit"].ToString();
                decimal unitPrice = Convert.ToDecimal(rItems["unit_price"]);
                decimal subtotal = qty * unitPrice;

                dgvOrderDetails.Rows.Add(itemId, name, qty, unit, unitPrice, subtotal, "");
            }
            UpdateTotalAmount();
        }

        private void LoadSuppliers()
        {
            try
            {
                DataTable dt = _itemController.GetSuppliers();
                cmbSupplier.DataSource = dt;
                cmbSupplier.DisplayMember = "display";
                cmbSupplier.ValueMember = "id";
                cmbSupplier.SelectedIndex = -1;
            }
            catch
            {
                cmbSupplier.DataSource = null;
            }
        }

        private void LoadStatuses()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("RECEIVED");
            cmbStatus.SelectedIndex = 0;
            cmbStatus.Enabled = false; // Karena form ini khusus untuk terima barang
        }

        private void LoadWarehouses()
        {
            WarehouseController wc = new WarehouseController();
            DataTable dt = wc.GetWarehouses();
            cmbWarehouse.DataSource = dt;
            cmbWarehouse.DisplayMember = "name";
            cmbWarehouse.ValueMember = "id";
            if (dt.Rows.Count > 0)
                cmbWarehouse.SelectedIndex = 0;
        }

        private void LoadItems()
        {
            DataTable dt = _itemController.GetItems();

            // Init manager
            _dgvItemManager = new DataGridViewManager(dgvItems, dt, 10);
            _dgvItemManager.PagingInfoLabel = lblPagingInfo; // Label untuk info "Showing x–y of z"
            _dgvItemManager.OnAfterLoadPage += () =>
            {
                // bisa sembunyikan kolom ID otomatis
                _dgvItemManager.ToggleColumnVisibility("id", false);
                _dgvItemManager.ToggleColumnVisibility("barcode", false);

                // bikin semua kolom readonly
                foreach (DataGridViewColumn col in dgvItems.Columns)
                {
                    col.ReadOnly = true;
                }
            };
        }

        private void LoadPurchasableItems()
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string query = @"
SELECT
    items.id,
    items.barcode,
    items.name,
    units.name AS unit_name,
    items.buy_price
FROM items
LEFT JOIN units ON items.unit = units.id
WHERE items.deleted_at IS NULL
  AND items.is_purchasable = TRUE
  AND (@search = '' OR items.name ILIKE @search OR items.barcode ILIKE @search)
ORDER BY items.name ASC";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@search", "%" + (txtSearch.Text ?? "") + "%");
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            dgvItems.DataSource = dt;
            if (dgvItems.Columns.Contains("id")) dgvItems.Columns["id"].Visible = false;

            if (dgvItems.Columns.Contains("barcode"))
            {
                dgvItems.Columns["barcode"].HeaderText = "Barcode";
                dgvItems.Columns["barcode"].Width = 160;
            }

            if (dgvItems.Columns.Contains("name"))
            {
                dgvItems.Columns["name"].HeaderText = "Nama";
                dgvItems.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvItems.Columns.Contains("unit_name"))
            {
                dgvItems.Columns["unit_name"].HeaderText = "Satuan";
                dgvItems.Columns["unit_name"].Width = 100;
            }

            if (dgvItems.Columns.Contains("buy_price"))
            {
                dgvItems.Columns["buy_price"].HeaderText = "Harga Beli";
                dgvItems.Columns["buy_price"].DefaultCellStyle.Format = "N0";
                dgvItems.Columns["buy_price"].Width = 120;
            }
        }

        private void InitOrderDetailsGrid()
        {
            dgvOrderDetails.Columns.Clear();

            // ID barang (hidden)
            dgvOrderDetails.Columns.Add("item_id", "Item ID");
            dgvOrderDetails.Columns["item_id"].Visible = false;

            // Nama barang
            dgvOrderDetails.Columns.Add("name", "Name");

            // Quantity
            var qtyCol = new DataGridViewTextBoxColumn();
            qtyCol.Name = "quantity";
            qtyCol.HeaderText = "Qty";
            dgvOrderDetails.Columns.Add(qtyCol);

            var unit = new DataGridViewTextBoxColumn();
            unit.Name = "unit";
            unit.HeaderText = "Unit";
            dgvOrderDetails.Columns.Add(unit);


            // Harga per unit
            var priceCol = new DataGridViewTextBoxColumn();
            priceCol.Name = "unit_price";
            priceCol.HeaderText = "Unit Price";
            dgvOrderDetails.Columns.Add(priceCol);

            // Subtotal (Qty * Price, readonly)
            var subtotalCol = new DataGridViewTextBoxColumn();
            subtotalCol.Name = "subtotal";
            subtotalCol.HeaderText = "Subtotal";
            subtotalCol.ReadOnly = true;
            dgvOrderDetails.Columns.Add(subtotalCol);

            // Note
            var noteCol = new DataGridViewTextBoxColumn();
            noteCol.Name = "note";
            noteCol.HeaderText = "Note";
            dgvOrderDetails.Columns.Add(noteCol);
        }

        private void dgvOrderDetails_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvOrderDetails.Columns["quantity"].Index ||
                e.ColumnIndex == dgvOrderDetails.Columns["unit_price"].Index)
            {
                var row = dgvOrderDetails.Rows[e.RowIndex];

                decimal qty = 0;
                decimal price = 0;

                decimal.TryParse(row.Cells["quantity"].Value?.ToString(), out qty);
                decimal.TryParse(row.Cells["unit_price"].Value?.ToString(), out price);

                row.Cells["subtotal"].Value = qty * price;
                UpdateTotalAmount();
            }
        }

        private void UpdateTotalAmount()
        {
            decimal totalAmount = dgvOrderDetails.Rows.Cast<DataGridViewRow>()
                .Sum(r => Convert.ToDecimal(r.Cells["subtotal"].Value ?? 0));
            lblTotalAmount.Text = $"Total: Rp {totalAmount:N0}";
        }


        private void btnAddItem_Click(object? sender, EventArgs e)
        {
            if (_mode == ReceivePurchaseMode.FromApprovedPo)
            {
                btnLoadPO_Click(sender, e);
                return;
            }

            if (dgvItems.CurrentRow == null)
            {
                MessageBox.Show("Pilih item dari daftar sebelah kiri.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            long itemId = Convert.ToInt64(dgvItems.CurrentRow.Cells["id"].Value);
            string name = dgvItems.CurrentRow.Cells["name"].Value?.ToString() ?? "";
            string unit = dgvItems.CurrentRow.Cells["unit_name"].Value?.ToString() ?? "";
            decimal unitPrice = 0m;
            if (dgvItems.CurrentRow.Cells["buy_price"].Value != null && dgvItems.CurrentRow.Cells["buy_price"].Value != DBNull.Value)
                unitPrice = Convert.ToDecimal(dgvItems.CurrentRow.Cells["buy_price"].Value);

            var existing = dgvOrderDetails.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => !r.IsNewRow && Convert.ToInt64(r.Cells["item_id"].Value) == itemId);

            if (existing != null)
            {
                decimal qtyOld = 0m;
                decimal.TryParse(existing.Cells["quantity"].Value?.ToString(), out qtyOld);
                existing.Cells["quantity"].Value = qtyOld + 1m;
                existing.Cells["unit_price"].Value = unitPrice;
                existing.Cells["unit"].Value = unit;
                existing.Cells["subtotal"].Value = (qtyOld + 1m) * unitPrice;
                UpdateTotalAmount();
                return;
            }

            decimal quantity = 1m;
            decimal subtotal = quantity * unitPrice;
            dgvOrderDetails.Rows.Add(itemId, name, quantity, unit, unitPrice, subtotal, "");
            UpdateTotalAmount();
        }


        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (dgvOrderDetails.Rows.Count == 0)
            {
                MessageBox.Show("Pastikan ada item!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using var tran = conn.BeginTransaction();

                // 1. Update status PO jadi RECEIVED dan update catatan/total kalau ada perubahan
                decimal totalAmount = dgvOrderDetails.Rows.Cast<DataGridViewRow>()
                    .Sum(r => Convert.ToDecimal(r.Cells["subtotal"].Value ?? 0));

                int warehouseId = 1; // default fallback
                if (cmbWarehouse.SelectedValue != null && int.TryParse(cmbWarehouse.SelectedValue.ToString(), out int wid))
                {
                    warehouseId = wid;
                }

                var user = POS_qu.Models.SessionUser.GetCurrentUser();
                int createdBy = user?.UserId ?? 0;
                if (createdBy <= 0)
                    throw new InvalidOperationException("Session user tidak ditemukan. Silakan login ulang.");

                if (_mode == ReceivePurchaseMode.DirectPurchase)
                {
                    long? supplierId = null;
                    if (cmbSupplier.SelectedValue != null && long.TryParse(cmbSupplier.SelectedValue.ToString(), out var sid) && sid > 0)
                        supplierId = sid;

                    bool hasWhCol = HasColumn(conn, tran, "purchase_orders", "warehouse_id");
                    string insertPoSql = hasWhCol
                        ? @"INSERT INTO purchase_orders (supplier_id, order_date, status, total_amount, note, created_by, warehouse_id, created_at, updated_at)
                           VALUES (@supplier_id, @order_date, 'RECEIVED', @total_amount, @note, @created_by, @warehouse_id, NOW(), NOW())
                           RETURNING id"
                        : @"INSERT INTO purchase_orders (supplier_id, order_date, status, total_amount, note, created_by, created_at, updated_at)
                           VALUES (@supplier_id, @order_date, 'RECEIVED', @total_amount, @note, @created_by, NOW(), NOW())
                           RETURNING id";

                    using var cmdIns = new NpgsqlCommand(insertPoSql, conn, tran);
                    cmdIns.Parameters.AddWithValue("@supplier_id", (object?)supplierId ?? DBNull.Value);
                    cmdIns.Parameters.AddWithValue("@order_date", dtpOrderDate.Value.Date);
                    cmdIns.Parameters.AddWithValue("@total_amount", totalAmount);
                    cmdIns.Parameters.AddWithValue("@note", txtNote.Text ?? "");
                    cmdIns.Parameters.AddWithValue("@created_by", createdBy);
                    if (hasWhCol) cmdIns.Parameters.AddWithValue("@warehouse_id", warehouseId);

                    var newIdObj = cmdIns.ExecuteScalar();
                    _poId = newIdObj != null ? Convert.ToInt32(newIdObj) : 0;
                    if (_poId <= 0) throw new InvalidOperationException("Gagal membuat pembelian baru.");
                }
                else
                {
                    if (_poId == 0)
                    {
                        MessageBox.Show("Muat PO terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string updatePO = @"
                        UPDATE purchase_orders 
                        SET status = 'RECEIVED', 
                            total_amount = @total, 
                            note = @note,
                            warehouse_id = @warehouse_id,
                            updated_at = NOW()
                        WHERE id = @id";

                    using var cmdPO = new NpgsqlCommand(updatePO, conn, tran);
                    cmdPO.Parameters.AddWithValue("@id", _poId);
                    cmdPO.Parameters.AddWithValue("@total", totalAmount);
                    cmdPO.Parameters.AddWithValue("@note", txtNote.Text ?? "");
                    cmdPO.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    cmdPO.ExecuteNonQuery();

                    // 2. Hapus item lama, ganti dengan yang baru (karena bisa jadi qty/harga diubah saat terima)
                    string delItems = "DELETE FROM purchase_order_items WHERE po_id = @po_id";
                    using var cmdDel = new NpgsqlCommand(delItems, conn, tran);
                    cmdDel.Parameters.AddWithValue("@po_id", _poId);
                    cmdDel.ExecuteNonQuery();
                }

                // Insert details dan Update Stock
                foreach (DataGridViewRow row in dgvOrderDetails.Rows)
                {
                    if (row.IsNewRow) continue;

                    long itemId = Convert.ToInt64(row.Cells["item_id"].Value);
                    decimal qty = Convert.ToDecimal(row.Cells["quantity"].Value);
                    decimal unitPrice = Convert.ToDecimal(row.Cells["unit_price"].Value);
                    string unit = row.Cells["unit"].Value?.ToString() ?? "";
                    string note = row.Cells["note"].Value?.ToString() ?? "";

                    // Re-insert item
                    string insertItem = @"
                        INSERT INTO purchase_order_items (po_id, item_id, quantity, unit, unit_price, note)
                        VALUES (@po_id, @item_id, @qty, @unit, @unit_price, @note);";

                    using var cmdItem = new NpgsqlCommand(insertItem, conn, tran);
                    cmdItem.Parameters.AddWithValue("@po_id", _poId);
                    cmdItem.Parameters.AddWithValue("@item_id", itemId);
                    cmdItem.Parameters.AddWithValue("@qty", qty);
                    cmdItem.Parameters.AddWithValue("@unit", unit);
                    cmdItem.Parameters.AddWithValue("@unit_price", unitPrice);
                    cmdItem.Parameters.AddWithValue("@note", note);
                    cmdItem.ExecuteNonQuery();

                    // ==========================================
                    // LOGIKA UPDATE STOK MULTI GUDANG & FIFO/AVG
                    // ==========================================

                    // A. Update Master Stock (Stocks table)
                    string checkStockSql = "SELECT COUNT(*) FROM stocks WHERE item_id = @item_id AND warehouse_id = @warehouse_id";
                    using var checkStockCmd = new NpgsqlCommand(checkStockSql, conn, tran);
                    checkStockCmd.Parameters.AddWithValue("@item_id", itemId);
                    checkStockCmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    long stockExists = (long)checkStockCmd.ExecuteScalar();

                    if (stockExists > 0)
                    {
                        string updateStockSql = "UPDATE stocks SET qty = qty + @qty WHERE item_id = @item_id AND warehouse_id = @warehouse_id";
                        using var updateStockCmd = new NpgsqlCommand(updateStockSql, conn, tran);
                        updateStockCmd.Parameters.AddWithValue("@qty", qty);
                        updateStockCmd.Parameters.AddWithValue("@item_id", itemId);
                        updateStockCmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                        updateStockCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string insertStockSql = "INSERT INTO stocks (item_id, warehouse_id, qty, reserved_qty) VALUES (@item_id, @warehouse_id, @qty, 0)";
                        using var insertStockCmd = new NpgsqlCommand(insertStockSql, conn, tran);
                        insertStockCmd.Parameters.AddWithValue("@item_id", itemId);
                        insertStockCmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                        insertStockCmd.Parameters.AddWithValue("@qty", qty);
                        insertStockCmd.ExecuteNonQuery();
                    }

                    // B. Insert into Stock Layers (Untuk FIFO/AVG)
                    string insertLayerSql = "INSERT INTO stock_layers (item_id, warehouse_id, qty_remaining, buy_price) VALUES (@item_id, @warehouse_id, @qty, @buy_price)";
                    using var layerCmd = new NpgsqlCommand(insertLayerSql, conn, tran);
                    layerCmd.Parameters.AddWithValue("@item_id", itemId);
                    layerCmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    layerCmd.Parameters.AddWithValue("@qty", qty);
                    layerCmd.Parameters.AddWithValue("@buy_price", unitPrice);
                    layerCmd.ExecuteNonQuery();
                }

                var journal = new POS_qu.Services.JournalService();
                journal.CreateJournalFromPurchase(_poId, conn, tran);

                tran.Commit();
                MessageBox.Show("Berhasil simpan dan stok telah diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close(); // Tutup form setelah selesai
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool HasColumn(NpgsqlConnection con, NpgsqlTransaction tran, string table, string column)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.columns
WHERE table_schema = current_schema()
  AND table_name = @t
  AND column_name = @c
LIMIT 1", con, tran);
            cmd.Parameters.AddWithValue("@t", table);
            cmd.Parameters.AddWithValue("@c", column);
            var obj = cmd.ExecuteScalar();
            return obj != null;
        }

        private void button1_Click(object? sender, EventArgs e)
        {
            PurchaseOrderListForm POForm = new PurchaseOrderListForm();
            this.Hide();
            POForm.FormClosed += (s, args) => this.Show();
            POForm.Show();
        }
    }
}
