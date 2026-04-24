using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.Repositories;
using POS_qu.Services;
using POS_qu.Services.StockValuation;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_qu
{
    public sealed class PurchaseReturnEntryForm : Form
    {
        private TextBox txtReturnNo;
        private CheckBox chkManualNo;
        private DateTimePicker dtpDate;
        private ComboBox cmbSupplier;
        private ComboBox cmbWarehouse;
        private TextBox txtNote;
        private DataGridView dgvItems;
        private Button btnAddItem;
        private Button btnRemoveItem;
        private Button btnSave;
        private Button btnCancel;

        public int CreatedReturnId { get; private set; }

        public PurchaseReturnEntryForm()
        {
            Text = "Retur Pembelian ke Supplier";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1100, 700);
            BackColor = Color.FromArgb(245, 246, 250);

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.White };
            var lblTitle = new Label
            {
                Text = "Retur Pembelian ke Supplier",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Left = 20,
                Top = 18
            };
            panelHeader.Controls.Add(lblTitle);
            Controls.Add(panelHeader);

            var panelBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };
            Controls.Add(panelBody);

            var formPanel = new Panel { Dock = DockStyle.Top, Height = 170, BackColor = Color.Transparent };
            panelBody.Controls.Add(formPanel);

            var lblNo = new Label { Text = "No Retur", Left = 0, Top = 0, AutoSize = true };
            txtReturnNo = new TextBox { Left = 0, Top = 26, Width = 260 };
            chkManualNo = new CheckBox { Left = 270, Top = 28, AutoSize = true, Text = "Nomor Manual" };

            var lblDate = new Label { Text = "Tanggal", Left = 0, Top = 62, AutoSize = true };
            dtpDate = new DateTimePicker { Left = 0, Top = 88, Width = 260, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };

            var lblSupplier = new Label { Text = "Supplier", Left = 360, Top = 0, AutoSize = true };
            cmbSupplier = new ComboBox { Left = 360, Top = 26, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblWh = new Label { Text = "Gudang", Left = 360, Top = 62, AutoSize = true };
            cmbWarehouse = new ComboBox { Left = 360, Top = 88, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblNote = new Label { Text = "Catatan", Left = 720, Top = 0, AutoSize = true };
            txtNote = new TextBox { Left = 720, Top = 26, Width = 340, Height = 88, Multiline = true };

            formPanel.Controls.Add(lblNo);
            formPanel.Controls.Add(txtReturnNo);
            formPanel.Controls.Add(chkManualNo);
            formPanel.Controls.Add(lblDate);
            formPanel.Controls.Add(dtpDate);
            formPanel.Controls.Add(lblSupplier);
            formPanel.Controls.Add(cmbSupplier);
            formPanel.Controls.Add(lblWh);
            formPanel.Controls.Add(cmbWarehouse);
            formPanel.Controls.Add(lblNote);
            formPanel.Controls.Add(txtNote);

            var actionPanel = new Panel { Dock = DockStyle.Top, Height = 54 };
            panelBody.Controls.Add(actionPanel);

            btnAddItem = new Button { Text = "Tambah Item", Width = 140, Height = 40, Left = 0, Top = 6, BackColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnAddItem.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnRemoveItem = new Button { Text = "Hapus Item", Width = 140, Height = 40, Left = 150, Top = 6, BackColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnRemoveItem.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

            btnSave = new Button { Text = "Simpan", Width = 140, Height = 40, Anchor = AnchorStyles.Top | AnchorStyles.Right, BackColor = Color.FromArgb(22, 163, 74), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnSave.FlatAppearance.BorderSize = 0;
            btnCancel = new Button { Text = "Batal", Width = 140, Height = 40, Anchor = AnchorStyles.Top | AnchorStyles.Right, BackColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

            actionPanel.Controls.Add(btnAddItem);
            actionPanel.Controls.Add(btnRemoveItem);
            actionPanel.Controls.Add(btnSave);
            actionPanel.Controls.Add(btnCancel);

            btnSave.Left = actionPanel.Width - btnSave.Width - 0;
            btnCancel.Left = actionPanel.Width - btnSave.Width - btnCancel.Width - 10;

            actionPanel.Resize += (s, e) =>
            {
                btnSave.Left = actionPanel.Width - btnSave.Width;
                btnCancel.Left = actionPanel.Width - btnSave.Width - btnCancel.Width - 10;
            };

            dgvItems = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                RowTemplate = { Height = 42 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false
            };
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvItems.ColumnHeadersHeight = 40;
            dgvItems.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvItems.RowsDefaultCellStyle.Padding = new Padding(5);
            dgvItems.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgvItems.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            panelBody.Controls.Add(dgvItems);

            BuildGridColumns();

            Load += PurchaseReturnEntryForm_Load;
            chkManualNo.CheckedChanged += chkManualNo_CheckedChanged;
            btnAddItem.Click += btnAddItem_Click;
            btnRemoveItem.Click += btnRemoveItem_Click;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;
        }

        private void PurchaseReturnEntryForm_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            LoadWarehouses();
            ApplyNumberingMode();
        }

        private void chkManualNo_CheckedChanged(object sender, EventArgs e)
        {
            ApplyNumberingMode();
        }

        private void ApplyNumberingMode()
        {
            if (chkManualNo.Checked)
            {
                txtReturnNo.ReadOnly = false;
                if (string.IsNullOrWhiteSpace(txtReturnNo.Text)) txtReturnNo.Text = "";
            }
            else
            {
                txtReturnNo.ReadOnly = true;
                txtReturnNo.Text = new DocumentNumberingService().PeekNext("PURCHASE_RETURN");
            }
        }

        private void BuildGridColumns()
        {
            dgvItems.Columns.Clear();
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "item_id", HeaderText = "ID", Visible = false });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "supplier_id", HeaderText = "SupplierId", Visible = false });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "barcode", HeaderText = "Barcode", Width = 160, ReadOnly = true });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "name", HeaderText = "Nama", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true, MinimumWidth = 240 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "unit", HeaderText = "Satuan", Width = 110, ReadOnly = true });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "qty", HeaderText = "Qty", Width = 90 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "buy_price", HeaderText = "Harga Beli", Width = 120 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "note", HeaderText = "Catatan", Width = 220 });
            dgvItems.Columns["qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItems.Columns["buy_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItems.Columns["buy_price"].DefaultCellStyle.Format = "N0";
        }

        private void LoadSuppliers()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var da = new NpgsqlDataAdapter("SELECT id, name FROM suppliers ORDER BY name ASC", con);
            var dt = new DataTable();
            da.Fill(dt);
            cmbSupplier.DisplayMember = "name";
            cmbSupplier.ValueMember = "id";
            cmbSupplier.DataSource = dt;
            if (cmbSupplier.Items.Count > 0) cmbSupplier.SelectedIndex = 0;
        }

        private void LoadWarehouses()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var da = new NpgsqlDataAdapter("SELECT id, name FROM warehouses ORDER BY id ASC", con);
            var dt = new DataTable();
            da.Fill(dt);
            cmbWarehouse.DisplayMember = "name";
            cmbWarehouse.ValueMember = "id";
            cmbWarehouse.DataSource = dt;
            if (cmbWarehouse.Items.Count > 0) cmbWarehouse.SelectedIndex = 0;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (cmbSupplier.SelectedValue == null || cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Pilih Supplier dan Gudang dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var f = new SearchFormItem("");
            if (f.ShowDialog(this) != DialogResult.OK) return;
            if (f.SelectedItem == null) return;

            var selectedSupplierId = Convert.ToInt64(cmbSupplier.SelectedValue);
            var itemId = f.SelectedItem.id;

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT id, COALESCE(barcode,''), COALESCE(name,''), COALESCE(buy_price,0), COALESCE(supplier_id,0) AS supplier_id, COALESCE(u.name,'') AS unit_name
FROM items i
LEFT JOIN units u ON u.id = i.unit
WHERE i.id = @id
LIMIT 1
", con);
            cmd.Parameters.AddWithValue("@id", itemId);
            using var r = cmd.ExecuteReader();
            if (!r.Read())
            {
                MessageBox.Show("Item tidak ditemukan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var itemSupplierId = Convert.ToInt64(r["supplier_id"]);
            if (itemSupplierId != selectedSupplierId)
            {
                MessageBox.Show("Item ini bukan milik supplier yang dipilih. Retur pembelian hanya boleh 1 supplier.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells["item_id"].Value != null && Convert.ToInt64(row.Cells["item_id"].Value) == itemId)
                {
                    MessageBox.Show("Item sudah ada di daftar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            dgvItems.Rows.Add(
                itemId,
                itemSupplierId,
                r["barcode"]?.ToString() ?? "",
                r["name"]?.ToString() ?? "",
                r["unit_name"]?.ToString() ?? "",
                1,
                Convert.ToDecimal(r["buy_price"]),
                ""
            );
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0) return;
            dgvItems.Rows.RemoveAt(dgvItems.SelectedRows[0].Index);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbSupplier.SelectedValue == null || cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Supplier/Gudang wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgvItems.Rows.Count == 0)
            {
                MessageBox.Show("Item retur masih kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            long supplierId = Convert.ToInt64(cmbSupplier.SelectedValue);
            int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue);
            string returnNo = (txtReturnNo.Text ?? "").Trim();
            if (chkManualNo.Checked && string.IsNullOrWhiteSpace(returnNo))
            {
                MessageBox.Show("No retur wajib diisi (mode manual).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sessionUser = SessionUser.GetCurrentUser();
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                if (!chkManualNo.Checked)
                {
                    returnNo = new DocumentNumberingService().Generate("PURCHASE_RETURN", dtpDate.Value.Date, con, tran);
                    txtReturnNo.Text = returnNo;
                }

                int returnId;
                using (var cmd = new NpgsqlCommand(@"
INSERT INTO purchase_returns (return_number, return_date, supplier_id, warehouse_id, note, created_by, created_at, updated_at)
VALUES (@no, @d, @supplier, @wh, @note, @created_by, NOW(), NOW())
RETURNING id
", con, tran))
                {
                    cmd.Parameters.AddWithValue("@no", returnNo);
                    cmd.Parameters.AddWithValue("@d", dtpDate.Value.Date);
                    cmd.Parameters.AddWithValue("@supplier", supplierId);
                    cmd.Parameters.AddWithValue("@wh", warehouseId);
                    cmd.Parameters.AddWithValue("@note", string.IsNullOrWhiteSpace(txtNote.Text) ? (object)DBNull.Value : txtNote.Text.Trim());
                    cmd.Parameters.AddWithValue("@created_by", sessionUser.UserId);
                    returnId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.IsNewRow) continue;
                    int itemId = Convert.ToInt32(row.Cells["item_id"].Value);
                    long rowSupplierId = Convert.ToInt64(row.Cells["supplier_id"].Value);
                    if (rowSupplierId != supplierId)
                        throw new Exception("Ada item yang suppliernya tidak sesuai (hanya boleh 1 supplier).");

                    int qty = 0;
                    int.TryParse(row.Cells["qty"].Value?.ToString() ?? "0", out qty);
                    if (qty <= 0)
                        throw new Exception("Qty retur harus > 0.");

                    decimal buyPrice = 0;
                    if (row.Cells["buy_price"].Value != null && row.Cells["buy_price"].Value != DBNull.Value)
                        buyPrice = Convert.ToDecimal(row.Cells["buy_price"].Value);

                    string note = row.Cells["note"].Value?.ToString() ?? "";

                    using (var cmd = new NpgsqlCommand(@"
INSERT INTO purchase_return_items (purchase_return_id, item_id, qty, buy_price, note, created_at)
VALUES (@rid, @item, @qty, @price, @note, NOW())
", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@rid", returnId);
                        cmd.Parameters.AddWithValue("@item", itemId);
                        cmd.Parameters.AddWithValue("@qty", qty);
                        cmd.Parameters.AddWithValue("@price", buyPrice);
                        cmd.Parameters.AddWithValue("@note", string.IsNullOrWhiteSpace(note) ? (object)DBNull.Value : note.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    var method = GetItemValuationMethod(itemId, con, tran);
                    var strat = CreateStrategy(method);
                    var deduction = strat.CalculateCOGSAndDeductStock(itemId, warehouseId, qty, con, tran);

                    var newStock = GetStockQty(itemId, warehouseId, con, tran);
                    var repo = new TransactionRepo();
                    int parentId = repo.InsertStockLog(con, tran, new StockLog
                    {
                        ProductId = itemId,
                        TipeTransaksi = "purchase_return",
                        QtyMasuk = 0,
                        QtyKeluar = qty,
                        SisaStock = newStock,
                        Keterangan = $"Retur Pembelian #{returnNo}",
                        UserId = sessionUser.UserId,
                        CreatedAt = DateTime.Now,
                        LoginId = sessionUser.LoginId,
                        WarehouseId = warehouseId,
                        RefType = "PURCHASE_RETURN",
                        RefId = returnId,
                        SupplierId = Convert.ToInt32(supplierId),
                        Method = method,
                        IsAllocation = false
                    });

                    foreach (var line in deduction.Lines)
                    {
                        if (line.Qty == 0) continue;
                        repo.InsertStockLog(con, tran, new StockLog
                        {
                            ProductId = itemId,
                            TipeTransaksi = "purchase_return",
                            QtyMasuk = 0,
                            QtyKeluar = line.Qty,
                            SisaStock = newStock,
                            Keterangan = null,
                            UserId = sessionUser.UserId,
                            CreatedAt = DateTime.Now,
                            LoginId = sessionUser.LoginId,
                            WarehouseId = warehouseId,
                            RefType = "PURCHASE_RETURN",
                            RefId = returnId,
                            SupplierId = Convert.ToInt32(supplierId),
                            UnitCost = line.UnitCost,
                            Method = method,
                            StockLayerId = line.StockLayerId,
                            IsAllocation = true,
                            ParentId = parentId
                        });
                    }
                }

                tran.Commit();
                CreatedReturnId = returnId;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetItemValuationMethod(int itemId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand("SELECT COALESCE(valuation_method,'FIFO') FROM items WHERE id=@id", con, tran);
            cmd.Parameters.AddWithValue("@id", itemId);
            var res = cmd.ExecuteScalar();
            return res != null && res != DBNull.Value ? res.ToString() : "FIFO";
        }

        private static Core.Interfaces.IStockValuationStrategy CreateStrategy(string method)
        {
            method = (method ?? "FIFO").Trim().ToUpperInvariant();
            if (method == "AVG") return new AverageStrategy();
            if (method == "LIFO") return new LifoStrategy();
            if (method == "FEFO") return new FefoStrategy();
            return new FifoStrategy();
        }

        private static decimal GetStockQty(int itemId, int warehouseId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand("SELECT COALESCE(qty,0) FROM stocks WHERE item_id=@item AND warehouse_id=@wh", con, tran);
            cmd.Parameters.AddWithValue("@item", itemId);
            cmd.Parameters.AddWithValue("@wh", warehouseId);
            var res = cmd.ExecuteScalar();
            if (res == null || res == DBNull.Value) return 0;
            return Convert.ToDecimal(res);
        }
    }
}
