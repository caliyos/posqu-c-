using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using Npgsql;
using POS_qu.Core.Interfaces;
using POS_qu.Helpers;
using POS_qu.Repositories;
using POS_qu.Services;
using POS_qu.Controllers;

namespace POS_qu
{
    public partial class SaldoAwalForm : Form
    {
        private readonly IProductService _productService;
        private DataTable _dt;

        public SaldoAwalForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            _productService = new ProductService(new ProductRepository());

            Load += SaldoAwalForm_Load;
            dgvSaldo.CellValueChanged += dgvSaldo_CellValueChanged;
            dgvSaldo.CurrentCellDirtyStateChanged += dgvSaldo_CurrentCellDirtyStateChanged;
            dgvSaldo.KeyDown += dgvSaldo_KeyDown;
        }

        private void SaldoAwalForm_Load(object sender, EventArgs e)
        {
            if (HasAnyTransactions())
            {
                MessageBox.Show("Menu Saldo Awal hanya bisa dipakai jika belum ada transaksi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            LoadWarehouses();
            LoadGrid();
            ApplyGridStyle();
        }

        private void LoadWarehouses()
        {
            var wc = new WarehouseController();
            var dt = wc.GetWarehouses();
            cmbWarehouse.DataSource = dt;
            cmbWarehouse.DisplayMember = "name";
            cmbWarehouse.ValueMember = "id";
            if (dt.Rows.Count > 0) cmbWarehouse.SelectedIndex = 0;
        }

        private void LoadGrid()
        {
            var products = _productService.GetAllProducts();
            _dt = new DataTable();
            _dt.Columns.Add("item_id", typeof(int));
            _dt.Columns.Add("barcode", typeof(string));
            _dt.Columns.Add("name", typeof(string));
            _dt.Columns.Add("unit_name", typeof(string));
            _dt.Columns.Add("stock", typeof(double));
            _dt.Columns.Add("qty_awal", typeof(double));
            _dt.Columns.Add("hpp", typeof(decimal));
            _dt.Columns.Add("total", typeof(decimal));

            if (products != null)
            {
                foreach (DataRow r in products.Rows)
                {
                    bool isInv = true;
                    if (products.Columns.Contains("is_inventory_p") && r["is_inventory_p"] != DBNull.Value)
                        isInv = Convert.ToBoolean(r["is_inventory_p"]);
                    if (!isInv) continue;

                    int id = r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0;
                    if (id <= 0) continue;

                    string barcode = r.Table.Columns.Contains("barcode") ? (r["barcode"]?.ToString() ?? "") : "";
                    string name = r.Table.Columns.Contains("name") ? (r["name"]?.ToString() ?? "") : "";
                    string unitName = r.Table.Columns.Contains("unit_name") ? (r["unit_name"]?.ToString() ?? "") : "";

                    double stock = 0;
                    if (r.Table.Columns.Contains("stock") && r["stock"] != DBNull.Value)
                        double.TryParse(r["stock"].ToString(), out stock);

                    decimal buy = 0m;
                    if (r.Table.Columns.Contains("buy_price") && r["buy_price"] != DBNull.Value)
                        buy = Convert.ToDecimal(r["buy_price"]);

                    var nr = _dt.NewRow();
                    nr["item_id"] = id;
                    nr["barcode"] = barcode;
                    nr["name"] = name;
                    nr["unit_name"] = unitName;
                    nr["stock"] = stock;
                    nr["qty_awal"] = 0d;
                    nr["hpp"] = buy;
                    nr["total"] = 0m;
                    _dt.Rows.Add(nr);
                }
            }

            dgvSaldo.DataSource = _dt;

            if (dgvSaldo.Columns.Contains("item_id")) dgvSaldo.Columns["item_id"].Visible = false;
            if (dgvSaldo.Columns.Contains("barcode"))
            {
                dgvSaldo.Columns["barcode"].HeaderText = "Barcode";
                dgvSaldo.Columns["barcode"].ReadOnly = true;
                dgvSaldo.Columns["barcode"].Width = 160;
            }
            if (dgvSaldo.Columns.Contains("name"))
            {
                dgvSaldo.Columns["name"].HeaderText = "Nama Item";
                dgvSaldo.Columns["name"].ReadOnly = true;
                dgvSaldo.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSaldo.Columns["name"].FillWeight = 320;
            }
            if (dgvSaldo.Columns.Contains("unit_name"))
            {
                dgvSaldo.Columns["unit_name"].HeaderText = "Satuan";
                dgvSaldo.Columns["unit_name"].ReadOnly = true;
                dgvSaldo.Columns["unit_name"].Width = 110;
            }
            if (dgvSaldo.Columns.Contains("stock"))
            {
                dgvSaldo.Columns["stock"].HeaderText = "Stock Sekarang";
                dgvSaldo.Columns["stock"].ReadOnly = true;
                dgvSaldo.Columns["stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSaldo.Columns["stock"].DefaultCellStyle.Format = "N2";
                dgvSaldo.Columns["stock"].Width = 140;
            }
            if (dgvSaldo.Columns.Contains("qty_awal"))
            {
                dgvSaldo.Columns["qty_awal"].HeaderText = "Saldo Awal (Qty)";
                dgvSaldo.Columns["qty_awal"].ReadOnly = false;
                dgvSaldo.Columns["qty_awal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSaldo.Columns["qty_awal"].DefaultCellStyle.Format = "N2";
                dgvSaldo.Columns["qty_awal"].Width = 150;
            }
            if (dgvSaldo.Columns.Contains("hpp"))
            {
                dgvSaldo.Columns["hpp"].HeaderText = "HPP";
                dgvSaldo.Columns["hpp"].ReadOnly = false;
                dgvSaldo.Columns["hpp"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSaldo.Columns["hpp"].DefaultCellStyle.Format = "N0";
                dgvSaldo.Columns["hpp"].Width = 130;
            }
            if (dgvSaldo.Columns.Contains("total"))
            {
                dgvSaldo.Columns["total"].HeaderText = "Total";
                dgvSaldo.Columns["total"].ReadOnly = true;
                dgvSaldo.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSaldo.Columns["total"].DefaultCellStyle.Format = "N0";
                dgvSaldo.Columns["total"].Width = 140;
            }
        }

        private void ApplyGridStyle()
        {
            dgvSaldo.EnableHeadersVisualStyles = false;
            dgvSaldo.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvSaldo.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
            dgvSaldo.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSaldo.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvSaldo.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgvSaldo.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSaldo.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            dgvSaldo.GridColor = Color.FromArgb(235, 235, 235);
            dgvSaldo.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvSaldo.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
        }

        private void dgvSaldo_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSaldo.IsCurrentCellDirty)
                dgvSaldo.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvSaldo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSaldo.Rows[e.RowIndex];
            if (row?.DataBoundItem is not DataRowView drv) return;

            double qty = 0d;
            decimal hpp = 0m;

            if (drv.Row.Table.Columns.Contains("qty_awal") && drv.Row["qty_awal"] != DBNull.Value)
                double.TryParse(drv.Row["qty_awal"].ToString(), out qty);
            if (drv.Row.Table.Columns.Contains("hpp") && drv.Row["hpp"] != DBNull.Value)
                decimal.TryParse(drv.Row["hpp"].ToString(), out hpp);

            drv.Row["total"] = (decimal)qty * hpp;
        }

        private void dgvSaldo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                PasteFromClipboard();
                e.Handled = true;
            }
        }

        private void PasteFromClipboard()
        {
            if (dgvSaldo.CurrentCell == null) return;
            string text = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(text)) return;

            var rows = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int startRow = dgvSaldo.CurrentCell.RowIndex;
            int startCol = dgvSaldo.CurrentCell.ColumnIndex;

            for (int r = 0; r < rows.Length; r++)
            {
                var cols = rows[r].Split('\t');
                for (int c = 0; c < cols.Length; c++)
                {
                    int targetRow = startRow + r;
                    int targetCol = startCol + c;
                    if (targetRow >= dgvSaldo.RowCount) continue;
                    if (targetCol >= dgvSaldo.ColumnCount) continue;

                    var colName = dgvSaldo.Columns[targetCol].Name;
                    if (colName != "qty_awal" && colName != "hpp") continue;

                    dgvSaldo.Rows[targetRow].Cells[targetCol].Value = cols[c];
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_dt == null || _dt.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "saldo_awal_stock.xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            using var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("SaldoAwal");
            ws.Cell(1, 1).Value = "Barcode";
            ws.Cell(1, 2).Value = "Nama";
            ws.Cell(1, 3).Value = "Satuan";
            ws.Cell(1, 4).Value = "Qty";
            ws.Cell(1, 5).Value = "HPP";

            int row = 2;
            foreach (DataRow r in _dt.Rows)
            {
                ws.Cell(row, 1).Value = r["barcode"]?.ToString() ?? "";
                ws.Cell(row, 2).Value = r["name"]?.ToString() ?? "";
                ws.Cell(row, 3).Value = r["unit_name"]?.ToString() ?? "";
                ws.Cell(row, 4).Value = r["qty_awal"] != DBNull.Value ? Convert.ToDouble(r["qty_awal"]) : 0d;
                ws.Cell(row, 5).Value = r["hpp"] != DBNull.Value ? Convert.ToDecimal(r["hpp"]) : 0m;
                row++;
            }
            ws.Columns().AdjustToContents();
            wb.SaveAs(sfd.FileName);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            if (_dt == null) return;

            var byBarcode = new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow r in _dt.Rows)
            {
                var bc = r["barcode"]?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(bc) && !byBarcode.ContainsKey(bc))
                    byBarcode.Add(bc, r);
            }

            using var wb = new XLWorkbook(ofd.FileName);
            var ws = wb.Worksheets.First();
            var used = ws.RangeUsed();
            if (used == null) return;

            var header = used.FirstRowUsed();
            var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var cell in header.CellsUsed())
            {
                var key = (cell.GetString() ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(key) && !headerMap.ContainsKey(key))
                    headerMap.Add(key, cell.Address.ColumnNumber);
            }

            int colBarcode = headerMap.TryGetValue("Barcode", out var cb) ? cb : 1;
            int colQty = headerMap.TryGetValue("Qty", out var cq) ? cq : 4;
            int colHpp = headerMap.TryGetValue("HPP", out var ch) ? ch : 5;

            foreach (var row in used.RowsUsed().Skip(1))
            {
                var bc = row.Cell(colBarcode).GetString()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(bc)) continue;
                if (!byBarcode.TryGetValue(bc, out var dr)) continue;

                double qty = 0d;
                decimal hpp = 0m;
                if (row.Cell(colQty).TryGetValue<double>(out var qv)) qty = qv;
                if (row.Cell(colHpp).TryGetValue<double>(out var hv)) hpp = (decimal)hv;

                dr["qty_awal"] = qty;
                dr["hpp"] = hpp;
                dr["total"] = (decimal)qty * hpp;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (HasAnyTransactions())
            {
                MessageBox.Show("Saldo Awal diblokir karena sudah ada transaksi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Pilih gudang terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue);

            var rowsToSave = new List<DataRow>();
            foreach (DataRow r in _dt.Rows)
            {
                double qty = 0d;
                if (r["qty_awal"] != DBNull.Value) double.TryParse(r["qty_awal"].ToString(), out qty);
                if (qty > 0d) rowsToSave.Add(r);
            }

            if (rowsToSave.Count == 0)
            {
                MessageBox.Show("Tidak ada Qty saldo awal yang diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                foreach (var r in rowsToSave)
                {
                    int itemId = Convert.ToInt32(r["item_id"]);
                    double qty = Convert.ToDouble(r["qty_awal"]);
                    decimal hpp = r["hpp"] != DBNull.Value ? Convert.ToDecimal(r["hpp"]) : 0m;

                    using (var cmd = new NpgsqlCommand("UPDATE items SET buy_price=@buy, updated_at=NOW() WHERE id=@id", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@buy", hpp);
                        cmd.Parameters.AddWithValue("@id", itemId);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new NpgsqlCommand(@"
INSERT INTO stocks (item_id, warehouse_id, qty)
VALUES (@item_id, @w_id, @qty)
ON CONFLICT (item_id, warehouse_id)
DO UPDATE SET qty = EXCLUDED.qty
", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@item_id", itemId);
                        cmd.Parameters.AddWithValue("@w_id", warehouseId);
                        cmd.Parameters.AddWithValue("@qty", qty);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new NpgsqlCommand("DELETE FROM stock_layers WHERE item_id=@item_id AND warehouse_id=@w_id", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@item_id", itemId);
                        cmd.Parameters.AddWithValue("@w_id", warehouseId);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new NpgsqlCommand(@"
INSERT INTO stock_layers (item_id, warehouse_id, qty_remaining, buy_price)
VALUES (@item_id, @w_id, @qty, @buy_price)
", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@item_id", itemId);
                        cmd.Parameters.AddWithValue("@w_id", warehouseId);
                        cmd.Parameters.AddWithValue("@qty", qty);
                        cmd.Parameters.AddWithValue("@buy_price", hpp);
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                MessageBox.Show("Saldo awal berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                try { tran.Rollback(); } catch { }
                MessageBox.Show("Gagal simpan saldo awal: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool HasAnyTransactions()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT 1 FROM transactions WHERE deleted_at IS NULL LIMIT 1", conn);
                var v = cmd.ExecuteScalar();
                return v != null && v != DBNull.Value;
            }
            catch
            {
                return false;
            }
        }
    }
}

