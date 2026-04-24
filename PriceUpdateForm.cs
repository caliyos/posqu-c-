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

namespace POS_qu
{
    public partial class PriceUpdateForm : Form
    {
        private readonly IProductService _productService;
        private DataTable _dt;

        public PriceUpdateForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            _productService = new ProductService(new ProductRepository());

            Load += PriceUpdateForm_Load;
            dgvPrices.CurrentCellDirtyStateChanged += dgvPrices_CurrentCellDirtyStateChanged;
            dgvPrices.CellValueChanged += dgvPrices_CellValueChanged;
            dgvPrices.KeyDown += dgvPrices_KeyDown;
        }

        private void PriceUpdateForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
            ApplyGridStyle();
        }

        private void LoadGrid()
        {
            var src = _productService.GetAllProducts();

            _dt = new DataTable();
            _dt.Columns.Add("item_id", typeof(int));
            _dt.Columns.Add("barcode", typeof(string));
            _dt.Columns.Add("name", typeof(string));
            _dt.Columns.Add("unit_name", typeof(string));
            _dt.Columns.Add("buy_price", typeof(decimal));
            _dt.Columns.Add("sell_price", typeof(decimal));
            _dt.Columns.Add("buy_price_old", typeof(decimal));
            _dt.Columns.Add("sell_price_old", typeof(decimal));

            if (src != null)
            {
                foreach (DataRow r in src.Rows)
                {
                    int id = r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0;
                    if (id <= 0) continue;

                    string barcode = src.Columns.Contains("barcode") ? (r["barcode"]?.ToString() ?? "") : "";
                    string name = src.Columns.Contains("name") ? (r["name"]?.ToString() ?? "") : "";
                    string unitName = src.Columns.Contains("unit_name") ? (r["unit_name"]?.ToString() ?? "") : "";

                    decimal buy = 0m;
                    if (src.Columns.Contains("buy_price") && r["buy_price"] != DBNull.Value)
                        buy = Convert.ToDecimal(r["buy_price"]);

                    decimal sell = 0m;
                    if (src.Columns.Contains("sell_price") && r["sell_price"] != DBNull.Value)
                        sell = Convert.ToDecimal(r["sell_price"]);

                    var nr = _dt.NewRow();
                    nr["item_id"] = id;
                    nr["barcode"] = barcode;
                    nr["name"] = name;
                    nr["unit_name"] = unitName;
                    nr["buy_price"] = buy;
                    nr["sell_price"] = sell;
                    nr["buy_price_old"] = buy;
                    nr["sell_price_old"] = sell;
                    _dt.Rows.Add(nr);
                }
            }

            dgvPrices.DataSource = _dt;

            if (dgvPrices.Columns.Contains("item_id")) dgvPrices.Columns["item_id"].Visible = false;
            if (dgvPrices.Columns.Contains("buy_price_old")) dgvPrices.Columns["buy_price_old"].Visible = false;
            if (dgvPrices.Columns.Contains("sell_price_old")) dgvPrices.Columns["sell_price_old"].Visible = false;

            if (dgvPrices.Columns.Contains("barcode"))
            {
                dgvPrices.Columns["barcode"].HeaderText = "Barcode";
                dgvPrices.Columns["barcode"].ReadOnly = true;
                dgvPrices.Columns["barcode"].Width = 160;
            }

            if (dgvPrices.Columns.Contains("name"))
            {
                dgvPrices.Columns["name"].HeaderText = "Nama Item";
                dgvPrices.Columns["name"].ReadOnly = true;
                dgvPrices.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvPrices.Columns.Contains("unit_name"))
            {
                dgvPrices.Columns["unit_name"].HeaderText = "Satuan";
                dgvPrices.Columns["unit_name"].ReadOnly = true;
                dgvPrices.Columns["unit_name"].Width = 110;
            }

            if (dgvPrices.Columns.Contains("buy_price"))
            {
                dgvPrices.Columns["buy_price"].HeaderText = "Harga Pokok (HPP)";
                dgvPrices.Columns["buy_price"].ReadOnly = false;
                dgvPrices.Columns["buy_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPrices.Columns["buy_price"].DefaultCellStyle.Format = "N0";
                dgvPrices.Columns["buy_price"].Width = 150;
            }

            if (dgvPrices.Columns.Contains("sell_price"))
            {
                dgvPrices.Columns["sell_price"].HeaderText = "Harga Jual";
                dgvPrices.Columns["sell_price"].ReadOnly = false;
                dgvPrices.Columns["sell_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPrices.Columns["sell_price"].DefaultCellStyle.Format = "N0";
                dgvPrices.Columns["sell_price"].Width = 140;
            }
        }

        private void ApplyGridStyle()
        {
            dgvPrices.EnableHeadersVisualStyles = false;
            dgvPrices.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvPrices.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
            dgvPrices.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPrices.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvPrices.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgvPrices.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPrices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            dgvPrices.GridColor = Color.FromArgb(235, 235, 235);
            dgvPrices.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvPrices.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
        }

        private void dgvPrices_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPrices.IsCurrentCellDirty)
                dgvPrices.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvPrices_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (_dt == null) return;
            if (e.ColumnIndex < 0) return;

            var colName = dgvPrices.Columns[e.ColumnIndex].Name;
            if (colName != "buy_price" && colName != "sell_price") return;

            var row = dgvPrices.Rows[e.RowIndex];
            if (row?.DataBoundItem is not DataRowView drv) return;

            decimal buy = drv.Row["buy_price"] != DBNull.Value ? Convert.ToDecimal(drv.Row["buy_price"]) : 0m;
            decimal sell = drv.Row["sell_price"] != DBNull.Value ? Convert.ToDecimal(drv.Row["sell_price"]) : 0m;

            if (buy < 0) drv.Row["buy_price"] = 0m;
            if (sell < 0) drv.Row["sell_price"] = 0m;
        }

        private void dgvPrices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                PasteFromClipboard();
                e.Handled = true;
            }
        }

        private void PasteFromClipboard()
        {
            if (dgvPrices.CurrentCell == null) return;
            string text = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(text)) return;

            var rows = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int startRow = dgvPrices.CurrentCell.RowIndex;
            int startCol = dgvPrices.CurrentCell.ColumnIndex;

            for (int r = 0; r < rows.Length; r++)
            {
                var cols = rows[r].Split('\t');
                for (int c = 0; c < cols.Length; c++)
                {
                    int targetRow = startRow + r;
                    int targetCol = startCol + c;
                    if (targetRow >= dgvPrices.RowCount) continue;
                    if (targetCol >= dgvPrices.ColumnCount) continue;

                    var colName = dgvPrices.Columns[targetCol].Name;
                    if (colName != "buy_price" && colName != "sell_price") continue;

                    dgvPrices.Rows[targetRow].Cells[targetCol].Value = cols[c];
                }
            }
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
                FileName = "update_harga_item.xlsx"
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            using var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Harga");
            ws.Cell(1, 1).Value = "Barcode";
            ws.Cell(1, 2).Value = "Nama";
            ws.Cell(1, 3).Value = "Satuan";
            ws.Cell(1, 4).Value = "HPP";
            ws.Cell(1, 5).Value = "HargaJual";

            int row = 2;
            foreach (DataRow r in _dt.Rows)
            {
                ws.Cell(row, 1).Value = r["barcode"]?.ToString() ?? "";
                ws.Cell(row, 2).Value = r["name"]?.ToString() ?? "";
                ws.Cell(row, 3).Value = r["unit_name"]?.ToString() ?? "";
                ws.Cell(row, 4).Value = r["buy_price"] != DBNull.Value ? Convert.ToDecimal(r["buy_price"]) : 0m;
                ws.Cell(row, 5).Value = r["sell_price"] != DBNull.Value ? Convert.ToDecimal(r["sell_price"]) : 0m;
                row++;
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(sfd.FileName);

            MessageBox.Show("Export berhasil.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (_dt == null)
            {
                MessageBox.Show("Data belum siap.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var ofd = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };
            if (ofd.ShowDialog(this) != DialogResult.OK) return;

            var byBarcode = new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow r in _dt.Rows)
            {
                var bc = r["barcode"]?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(bc) && !byBarcode.ContainsKey(bc))
                    byBarcode.Add(bc, r);
            }

            int ok = 0;
            int miss = 0;
            int err = 0;

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
            int colHpp = headerMap.TryGetValue("HPP", out var ch) ? ch : 4;
            int colSell = headerMap.TryGetValue("HargaJual", out var cs) ? cs : 5;

            foreach (var row in used.RowsUsed().Skip(1))
            {
                string bc = row.Cell(colBarcode).GetString()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(bc)) continue;

                if (!byBarcode.TryGetValue(bc, out var dr))
                {
                    miss++;
                    continue;
                }

                try
                {
                    decimal hpp = dr["buy_price"] != DBNull.Value ? Convert.ToDecimal(dr["buy_price"]) : 0m;
                    decimal sell = dr["sell_price"] != DBNull.Value ? Convert.ToDecimal(dr["sell_price"]) : 0m;

                    if (row.Cell(colHpp).TryGetValue<double>(out var hv)) hpp = (decimal)hv;
                    if (row.Cell(colSell).TryGetValue<double>(out var sv)) sell = (decimal)sv;

                    if (hpp < 0) hpp = 0m;
                    if (sell < 0) sell = 0m;

                    dr["buy_price"] = hpp;
                    dr["sell_price"] = sell;
                    ok++;
                }
                catch
                {
                    err++;
                }
            }

            MessageBox.Show($"Import selesai. OK: {ok} | Barcode tidak ditemukan: {miss} | Error: {err}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_dt == null || _dt.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var changed = _dt.AsEnumerable()
                .Where(r =>
                {
                    var id = r["item_id"] != DBNull.Value ? Convert.ToInt32(r["item_id"]) : 0;
                    if (id <= 0) return false;

                    var buy = r["buy_price"] != DBNull.Value ? Convert.ToDecimal(r["buy_price"]) : 0m;
                    var sell = r["sell_price"] != DBNull.Value ? Convert.ToDecimal(r["sell_price"]) : 0m;
                    var buyOld = r["buy_price_old"] != DBNull.Value ? Convert.ToDecimal(r["buy_price_old"]) : 0m;
                    var sellOld = r["sell_price_old"] != DBNull.Value ? Convert.ToDecimal(r["sell_price_old"]) : 0m;
                    return buy != buyOld || sell != sellOld;
                })
                .ToList();

            if (changed.Count == 0)
            {
                MessageBox.Show("Tidak ada perubahan harga.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                foreach (var r in changed)
                {
                    int id = Convert.ToInt32(r["item_id"]);
                    decimal buy = r["buy_price"] != DBNull.Value ? Convert.ToDecimal(r["buy_price"]) : 0m;
                    decimal sell = r["sell_price"] != DBNull.Value ? Convert.ToDecimal(r["sell_price"]) : 0m;

                    using var cmd = new NpgsqlCommand(@"
UPDATE items
SET buy_price = @buy,
    sell_price = @sell,
    updated_at = NOW()
WHERE id = @id
", con, tran);
                    cmd.Parameters.AddWithValue("@buy", buy);
                    cmd.Parameters.AddWithValue("@sell", sell);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    r["buy_price_old"] = buy;
                    r["sell_price_old"] = sell;
                }

                tran.Commit();
                MessageBox.Show($"Berhasil simpan {changed.Count} item.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                try { tran.Rollback(); } catch { }
                MessageBox.Show("Gagal simpan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
