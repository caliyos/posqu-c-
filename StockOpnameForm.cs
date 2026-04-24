using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using Npgsql;
using POS_qu.Controllers;
using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.Repositories;
using POS_qu.Services;
using POS_qu.Core.Interfaces;

namespace POS_qu
{
    public partial class StockOpnameForm : Form
    {
        private readonly IProductService _productService;
        private readonly WarehouseController _warehouseController;

        private DataTable _warehouses;
        private readonly List<OpnameRow> _rows = new List<OpnameRow>();

        private readonly PrintDocument _printDoc = new PrintDocument();
        private List<PrintRow> _printRows = new List<PrintRow>();
        private int _printRowCursor;
        private bool _printAllUnits;

        private DataTable _importPreview;
        private readonly Dictionary<string, int> _warehouseIdByKey = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<int, ItemUnitsInfo> _unitsInfoByItemId = new Dictionary<int, ItemUnitsInfo>();

        public StockOpnameForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            _productService = new ProductService(new ProductRepository());
            _warehouseController = new WarehouseController();

            Load += StockOpnameForm_Load;

            rdoNoAuto.Checked = true;
            txtOpnameNo.ReadOnly = true;
            rdoNoAuto.CheckedChanged += (s, e) => UpdateNoMode();
            rdoNoManual.CheckedChanged += (s, e) => UpdateNoMode();

            cmbWarehouse.SelectedIndexChanged += (s, e) => RefreshSystemQtyForManualRows();

            dgvSatuan.DataError += (s, e) => { e.ThrowException = false; };
            dgvSatuan.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvSatuan.IsCurrentCellDirty)
                    dgvSatuan.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgvSatuan.CellValueChanged += dgvSatuan_CellValueChanged;
            dgvSatuan.CellBeginEdit += dgvSatuan_CellBeginEdit;

            dgvMassalPreview.DataError += (s, e) => { e.ThrowException = false; };

            _printDoc.BeginPrint += PrintDoc_BeginPrint;
            _printDoc.PrintPage += PrintDoc_PrintPage;
        }

        private void StockOpnameForm_Load(object sender, EventArgs e)
        {
            EnsureStockOpnameTables();
            LoadWarehouses();
            SetupSatuanGrid();
            SetupMassalGrid();
            ApplyGridStyle(dgvSatuan);
            ApplyGridStyle(dgvMassalPreview);
        }

        private void UpdateNoMode()
        {
            txtOpnameNo.ReadOnly = rdoNoAuto.Checked;
            if (rdoNoAuto.Checked) txtOpnameNo.Text = "";
        }

        private void LoadWarehouses()
        {
            _warehouses = _warehouseController.GetWarehouses();
            cmbWarehouse.DataSource = _warehouses;
            cmbWarehouse.DisplayMember = "name";
            cmbWarehouse.ValueMember = "id";
            if (_warehouses != null && _warehouses.Rows.Count > 0) cmbWarehouse.SelectedIndex = 0;

            _warehouseIdByKey.Clear();
            if (_warehouses == null) return;
            foreach (DataRow r in _warehouses.Rows)
            {
                if (r["id"] == DBNull.Value) continue;
                int id = Convert.ToInt32(r["id"]);
                string name = r["name"]?.ToString() ?? "";
                _warehouseIdByKey[id.ToString()] = id;
                if (!string.IsNullOrWhiteSpace(name))
                    _warehouseIdByKey[name.Trim()] = id;
            }
        }

        private int GetSelectedWarehouseId()
        {
            if (cmbWarehouse.SelectedValue == null) return 0;
            return Convert.ToInt32(cmbWarehouse.SelectedValue);
        }

        private void SetupSatuanGrid()
        {
            dgvSatuan.Columns.Clear();
            dgvSatuan.AutoGenerateColumns = false;

            dgvSatuan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colBarcode",
                HeaderText = "Barcode",
                Width = 150,
                ReadOnly = true
            });
            dgvSatuan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Nama Item",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            var colUnit = new DataGridViewComboBoxColumn
            {
                Name = "colUnit",
                HeaderText = "Satuan",
                Width = 120,
                DisplayMember = "display",
                ValueMember = "id"
            };
            colUnit.ValueType = typeof(int);
            dgvSatuan.Columns.Add(colUnit);

            dgvSatuan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSystem",
                HeaderText = "Stock Sistem",
                Width = 130,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" }
            });

            dgvSatuan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPhysical",
                HeaderText = "Stock Fisik",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" }
            });

            dgvSatuan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDiff",
                HeaderText = "Selisih",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" }
            });

            dgvSatuan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNote",
                HeaderText = "Catatan",
                Width = 220
            });

            RefreshManualGridRows();
        }

        private void RefreshManualGridRows()
        {
            dgvSatuan.Rows.Clear();
            int whId = GetSelectedWarehouseId();
            foreach (var r in _rows)
            {
                r.SystemQtyBase = whId > 0 ? GetSystemQtyBase(r.ItemId, whId) : 0d;
                r.PhysicalQtyBase = r.PhysicalQtyInput * r.ConversionToBase;
                r.DiffQtyBase = r.PhysicalQtyBase - r.SystemQtyBase;

                int idx = dgvSatuan.Rows.Add(r.Barcode, r.Name, r.UnitId, r.SystemQtyBase, r.PhysicalQtyInput, r.DiffQtyBase, r.Note);
                dgvSatuan.Rows[idx].Tag = r;
                ApplyAllowedUnitsToRow(idx, r.ItemId, r.UnitId);
            }
        }

        private void RefreshSystemQtyForManualRows()
        {
            if (dgvSatuan.Rows.Count == 0) return;
            int whId = GetSelectedWarehouseId();
            foreach (DataGridViewRow dgvr in dgvSatuan.Rows)
            {
                if (dgvr.Tag is not OpnameRow r) continue;
                r.SystemQtyBase = whId > 0 ? GetSystemQtyBase(r.ItemId, whId) : 0d;
                r.PhysicalQtyBase = r.PhysicalQtyInput * r.ConversionToBase;
                r.DiffQtyBase = r.PhysicalQtyBase - r.SystemQtyBase;
                dgvr.Cells["colSystem"].Value = r.SystemQtyBase;
                dgvr.Cells["colDiff"].Value = r.DiffQtyBase;
            }
        }

        private void dgvSatuan_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvSatuan.Rows[e.RowIndex].Tag is not OpnameRow r) return;

            if (dgvSatuan.Columns[e.ColumnIndex].Name == "colUnit")
            {
                ApplyAllowedUnitsToRow(e.RowIndex, r.ItemId, r.UnitId);
            }
        }

        private void dgvSatuan_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvSatuan.Rows[e.RowIndex].Tag is not OpnameRow r) return;

            var colName = dgvSatuan.Columns[e.ColumnIndex].Name;
            if (colName == "colUnit")
            {
                int unitId = 0;
                var v = dgvSatuan.Rows[e.RowIndex].Cells["colUnit"].Value;
                if (v != null) int.TryParse(v.ToString(), out unitId);

                var info = GetUnitsInfo(r.ItemId);
                if (info.UnitConversionToBase.TryGetValue(unitId, out var conv))
                {
                    r.UnitId = unitId;
                    r.UnitName = info.UnitNameById.TryGetValue(unitId, out var un) ? un : r.UnitName;
                    r.ConversionToBase = conv;
                }
                else
                {
                    r.UnitId = info.BaseUnitId;
                    r.UnitName = info.UnitNameById.TryGetValue(info.BaseUnitId, out var un) ? un : r.UnitName;
                    r.ConversionToBase = 1d;
                    dgvSatuan.Rows[e.RowIndex].Cells["colUnit"].Value = r.UnitId;
                }

                r.PhysicalQtyBase = r.PhysicalQtyInput * r.ConversionToBase;
                r.DiffQtyBase = r.PhysicalQtyBase - r.SystemQtyBase;
                dgvSatuan.Rows[e.RowIndex].Cells["colDiff"].Value = r.DiffQtyBase;
            }
            else if (colName == "colPhysical")
            {
                double phy = 0d;
                var v = dgvSatuan.Rows[e.RowIndex].Cells["colPhysical"].Value;
                if (v != null) double.TryParse(v.ToString(), out phy);
                r.PhysicalQtyInput = phy;
                r.PhysicalQtyBase = r.PhysicalQtyInput * r.ConversionToBase;
                r.DiffQtyBase = r.PhysicalQtyBase - r.SystemQtyBase;
                dgvSatuan.Rows[e.RowIndex].Cells["colDiff"].Value = r.DiffQtyBase;
            }
            else if (colName == "colNote")
            {
                r.Note = dgvSatuan.Rows[e.RowIndex].Cells["colNote"].Value?.ToString() ?? "";
            }
        }

        private void ApplyAllowedUnitsToRow(int rowIndex, int itemId, int selectedUnitId)
        {
            if (rowIndex < 0 || rowIndex >= dgvSatuan.Rows.Count) return;
            if (dgvSatuan.Columns["colUnit"] is not DataGridViewComboBoxColumn) return;

            var info = GetUnitsInfo(itemId);
            if (dgvSatuan.Rows[rowIndex].Cells["colUnit"] is not DataGridViewComboBoxCell cell) return;

            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("display", typeof(string));
            foreach (var u in info.AllowedUnitIds)
            {
                dt.Rows.Add(u, info.UnitNameById.TryGetValue(u, out var n) ? n : u.ToString());
            }

            cell.DataSource = dt;
            cell.DisplayMember = "display";
            cell.ValueMember = "id";
            cell.Value = selectedUnitId > 0 ? selectedUnitId : info.BaseUnitId;
        }

        private void SetupMassalGrid()
        {
            dgvMassalPreview.AutoGenerateColumns = true;
            dgvMassalPreview.DataSource = null;
            txtImportLog.Text = "";
        }

        private void ApplyGridStyle(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            dgv.GridColor = Color.FromArgb(235, 235, 235);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (_rows.Count >= 10)
            {
                MessageBox.Show("Maksimal 10 item.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var f = new SearchFormItem("");
            if (f.ShowDialog(this) != DialogResult.OK) return;
            if (f.SelectedItem == null || f.SelectedItem.id <= 0) return;

            if (_rows.Any(x => x.ItemId == f.SelectedItem.id))
            {
                MessageBox.Show("Item sudah ada di daftar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detail = new ItemController().GetItemById(f.SelectedItem.id);
            var units = GetUnitsInfo(detail.id);

            var r = new OpnameRow
            {
                ItemId = detail.id,
                Barcode = detail.barcode ?? "",
                Name = detail.name ?? "",
                UnitId = units.BaseUnitId,
                UnitName = units.UnitNameById.TryGetValue(units.BaseUnitId, out var un) ? un : "",
                ConversionToBase = 1d,
                PhysicalQtyInput = 0d,
                Note = ""
            };

            _rows.Add(r);
            RefreshManualGridRows();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvSatuan.CurrentRow?.Tag is not OpnameRow r) return;
            _rows.RemoveAll(x => x.ItemId == r.ItemId);
            RefreshManualGridRows();
        }

        private void btnSaveSatuan_Click(object sender, EventArgs e)
        {
            int whId = GetSelectedWarehouseId();
            if (whId <= 0)
            {
                MessageBox.Show("Pilih gudang terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_rows.Count == 0)
            {
                MessageBox.Show("Tambahkan item dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string opnameNo = GetOrGenerateOpnameNo();
            if (string.IsNullOrWhiteSpace(opnameNo))
            {
                MessageBox.Show("No. opname wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var itemsToSave = _rows
                .Select(r => RecalculateRow(r, whId))
                .Where(r => Math.Abs(r.DiffQtyBase) > 0.0001 || Math.Abs(r.PhysicalQtyBase) > 0.0001)
                .ToList();

            if (itemsToSave.Count == 0)
            {
                MessageBox.Show("Tidak ada perubahan stock yang disimpan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var user = SessionUser.GetCurrentUser();
            int userId = user?.UserId ?? 0;

            EnsureStockOpnameTables();
            SaveOpnameAndApplyStock(opnameNo, dtpOpnameDate.Value.Date, whId, "manual", userId, itemsToSave);

            MessageBox.Show("Stock opname tersimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private OpnameRow RecalculateRow(OpnameRow r, int warehouseId)
        {
            r.SystemQtyBase = GetSystemQtyBase(r.ItemId, warehouseId);
            r.PhysicalQtyBase = r.PhysicalQtyInput * r.ConversionToBase;
            r.DiffQtyBase = r.PhysicalQtyBase - r.SystemQtyBase;
            return r;
        }

        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            int whId = GetSelectedWarehouseId();
            if (whId <= 0)
            {
                MessageBox.Show("Pilih gudang terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = chkMassalAllUnit.Checked ? "stock_opname_template_all_unit.xlsx" : "stock_opname_template_base.xlsx"
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            var dt = chkMassalAllUnit.Checked ? BuildAllUnitTemplateRows(whId) : BuildBaseUnitTemplateRows(whId);

            using var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Template");
            ws.Cell(1, 1).Value = "Warehouse";
            ws.Cell(1, 2).Value = "Barcode";
            ws.Cell(1, 3).Value = "Unit";
            ws.Cell(1, 4).Value = "QtyFisik";
            ws.Cell(1, 5).Value = "StockSistem";
            ws.Cell(1, 6).Value = "ConversionToBase";

            int r = 2;
            foreach (DataRow row in dt.Rows)
            {
                ws.Cell(r, 1).Value = row["warehouse"]?.ToString() ?? "";
                ws.Cell(r, 2).Value = row["barcode"]?.ToString() ?? "";
                ws.Cell(r, 3).Value = row["unit"]?.ToString() ?? "";
                ws.Cell(r, 4).Value = "";
                ws.Cell(r, 5).Value = row["stock_system"] != DBNull.Value ? Convert.ToDouble(row["stock_system"]) : 0d;
                ws.Cell(r, 6).Value = row["conversion"] != DBNull.Value ? Convert.ToDouble(row["conversion"]) : 1d;
                r++;
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(sfd.FileName);
        }

        private DataTable BuildBaseUnitTemplateRows(int warehouseId)
        {
            var dt = new DataTable();
            dt.Columns.Add("warehouse", typeof(string));
            dt.Columns.Add("barcode", typeof(string));
            dt.Columns.Add("unit", typeof(string));
            dt.Columns.Add("stock_system", typeof(double));
            dt.Columns.Add("conversion", typeof(double));

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT i.barcode, u.name as unit_name,
       COALESCE((SELECT s.qty FROM stocks s WHERE s.item_id=i.id AND s.warehouse_id=@w), 0) as stock_qty,
       i.id, i.unit as unit_id
FROM items i
LEFT JOIN units u ON u.id = i.unit
WHERE i.deleted_at IS NULL AND i.is_inventory_p = TRUE
ORDER BY i.name
", con);
            cmd.Parameters.AddWithValue("@w", warehouseId);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt.Rows.Add(warehouseId.ToString(), dr.IsDBNull(0) ? "" : dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetString(1), dr.GetDouble(2), 1d);
            }
            return dt;
        }

        private DataTable BuildAllUnitTemplateRows(int warehouseId)
        {
            var dt = new DataTable();
            dt.Columns.Add("warehouse", typeof(string));
            dt.Columns.Add("barcode", typeof(string));
            dt.Columns.Add("unit", typeof(string));
            dt.Columns.Add("stock_system", typeof(double));
            dt.Columns.Add("conversion", typeof(double));

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
WITH base_items AS (
  SELECT i.id, i.barcode, i.name, i.unit as unit_id,
         u.name as unit_name,
         COALESCE((SELECT s.qty FROM stocks s WHERE s.item_id=i.id AND s.warehouse_id=@w), 0) as stock_base
  FROM items i
  LEFT JOIN units u ON u.id=i.unit
  WHERE i.deleted_at IS NULL AND i.is_inventory_p = TRUE
),
all_units AS (
  SELECT b.id as item_id, b.barcode, b.unit_id, b.unit_name, 1::double precision as conversion, (b.stock_base) as stock_in_unit
  FROM base_items b
  UNION ALL
  SELECT b.id as item_id, b.barcode, uv.unit_id, u2.name as unit_name, uv.conversion::double precision as conversion,
         CASE WHEN uv.conversion > 0 THEN (b.stock_base / uv.conversion::double precision) ELSE b.stock_base END as stock_in_unit
  FROM base_items b
  JOIN unit_variants uv ON uv.item_id=b.id AND uv.is_active = TRUE
  LEFT JOIN units u2 ON u2.id = uv.unit_id
)
SELECT barcode, unit_name, stock_in_unit, conversion
FROM all_units
ORDER BY barcode, unit_name
", con);
            cmd.Parameters.AddWithValue("@w", warehouseId);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt.Rows.Add(warehouseId.ToString(), dr.IsDBNull(0) ? "" : dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetString(1), dr.GetDouble(2), dr.GetDouble(3));
            }
            return dt;
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            int whId = GetSelectedWarehouseId();
            if (whId <= 0)
            {
                MessageBox.Show("Pilih gudang terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var ofd = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };
            if (ofd.ShowDialog(this) != DialogResult.OK) return;

            ImportFromExcel(ofd.FileName);
        }

        private void ImportFromExcel(string filePath)
        {
            var preview = new DataTable();
            preview.Columns.Add("Row", typeof(int));
            preview.Columns.Add("Warehouse", typeof(string));
            preview.Columns.Add("Barcode", typeof(string));
            preview.Columns.Add("Unit", typeof(string));
            preview.Columns.Add("QtyFisik", typeof(double));
            preview.Columns.Add("QtyBase", typeof(double));
            preview.Columns.Add("Status", typeof(string));
            preview.Columns.Add("Message", typeof(string));
            preview.Columns.Add("WarehouseId", typeof(int));
            preview.Columns.Add("ItemId", typeof(int));
            preview.Columns.Add("UnitId", typeof(int));

            var log = new List<string>();
            int ok = 0;
            int err = 0;

            using var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheets.First();
            var used = ws.RangeUsed();
            if (used == null)
            {
                MessageBox.Show("File Excel kosong.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var rows = used.RowsUsed().Skip(1);
            int rowNo = 1;
            foreach (var row in rows)
            {
                rowNo++;
                string whKey = (row.Cell(1).GetString() ?? "").Trim();
                string barcode = (row.Cell(2).GetString() ?? "").Trim();
                string unitText = (row.Cell(3).GetString() ?? "").Trim();
                string qtyText = (row.Cell(4).GetString() ?? "").Trim();

                double qtyFisik = 0d;
                double.TryParse(qtyText.Replace(",", "."), out qtyFisik);

                var pr = preview.NewRow();
                pr["Row"] = rowNo;
                pr["Warehouse"] = whKey;
                pr["Barcode"] = barcode;
                pr["Unit"] = unitText;
                pr["QtyFisik"] = qtyFisik;
                pr["QtyBase"] = 0d;
                pr["Status"] = "ERROR";
                pr["Message"] = "";
                pr["WarehouseId"] = 0;
                pr["ItemId"] = 0;
                pr["UnitId"] = 0;

                var errors = new List<string>();

                int selectedWarehouseId = GetSelectedWarehouseId();
                string whKeyResolved = string.IsNullOrWhiteSpace(whKey) ? selectedWarehouseId.ToString() : whKey;

                if (!_warehouseIdByKey.TryGetValue(whKeyResolved, out var warehouseId))
                {
                    errors.Add("Gudang tidak valid");
                }
                else if (selectedWarehouseId > 0 && warehouseId != selectedWarehouseId)
                {
                    errors.Add("Gudang harus sama dengan pilihan form");
                }

                int itemId = 0;
                int baseUnitId = 0;
                if (string.IsNullOrWhiteSpace(barcode))
                {
                    errors.Add("Barcode kosong");
                }
                else
                {
                    (itemId, baseUnitId) = GetItemIdAndBaseUnitByBarcode(barcode);
                    if (itemId <= 0) errors.Add("Barcode tidak ditemukan");
                }

                int unitId = 0;
                double conv = 1d;
                if (itemId > 0)
                {
                    var units = GetUnitsInfo(itemId);
                    unitId = units.BaseUnitId;
                    conv = 1d;

                    if (!string.IsNullOrWhiteSpace(unitText))
                    {
                        unitId = units.ResolveUnitId(unitText);
                        if (unitId <= 0) errors.Add("Satuan tidak valid");
                        else if (!units.UnitConversionToBase.TryGetValue(unitId, out conv)) errors.Add("Satuan tidak terdaftar di item");
                    }
                }

                if (qtyFisik < 0) errors.Add("QtyFisik tidak boleh minus");

                if (errors.Count == 0)
                {
                    pr["Status"] = "OK";
                    pr["WarehouseId"] = warehouseId;
                    pr["ItemId"] = itemId;
                    pr["UnitId"] = unitId;
                    pr["QtyBase"] = qtyFisik * conv;
                    ok++;
                }
                else
                {
                    pr["Message"] = string.Join("; ", errors);
                    err++;
                }

                preview.Rows.Add(pr);
            }

            _importPreview = preview;
            dgvMassalPreview.DataSource = _importPreview;

            foreach (DataGridViewColumn c in dgvMassalPreview.Columns)
            {
                if (c.Name == "WarehouseId" || c.Name == "ItemId" || c.Name == "UnitId")
                    c.Visible = false;
            }

            log.Add($"OK: {ok}");
            log.Add($"ERROR: {err}");
            txtImportLog.Text = string.Join(Environment.NewLine, log);
        }

        private void btnSaveMassal_Click(object sender, EventArgs e)
        {
            if (_importPreview == null || _importPreview.Rows.Count == 0)
            {
                MessageBox.Show("Import dulu file Excel.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var okRows = _importPreview.AsEnumerable()
                .Where(r => (r["Status"]?.ToString() ?? "") == "OK")
                .ToList();

            if (okRows.Count == 0)
            {
                MessageBox.Show("Tidak ada baris OK untuk disimpan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string opnameNo = GetOrGenerateOpnameNo();
            if (string.IsNullOrWhiteSpace(opnameNo))
            {
                MessageBox.Show("No. opname wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var grouped = okRows
                .GroupBy(r => Convert.ToInt32(r["ItemId"]))
                .Select(g => new
                {
                    ItemId = g.Key,
                    QtyBase = g.Sum(x => Convert.ToDouble(x["QtyBase"]))
                })
                .ToList();

            var user = SessionUser.GetCurrentUser();
            int userId = user?.UserId ?? 0;

            EnsureStockOpnameTables();

            var items = grouped.Select(g => new OpnameRow
            {
                ItemId = g.ItemId,
                UnitId = GetUnitsInfo(g.ItemId).BaseUnitId,
                UnitName = GetUnitsInfo(g.ItemId).UnitNameById.TryGetValue(GetUnitsInfo(g.ItemId).BaseUnitId, out var n) ? n : "",
                ConversionToBase = 1d,
                SystemQtyBase = GetSystemQtyBase(g.ItemId, GetSelectedWarehouseId()),
                PhysicalQtyInput = g.QtyBase,
                PhysicalQtyBase = g.QtyBase,
                DiffQtyBase = g.QtyBase - GetSystemQtyBase(g.ItemId, GetSelectedWarehouseId()),
                Note = "import"
            }).ToList();

            int whId = GetSelectedWarehouseId();
            SaveOpnameAndApplyStock(opnameNo, dtpOpnameDate.Value.Date, whId, "import", userId, items);

            MessageBox.Show("Stock opname massal tersimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private (int ItemId, int BaseUnitId) GetItemIdAndBaseUnitByBarcode(string barcode)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand("SELECT id, unit FROM items WHERE deleted_at IS NULL AND barcode = @b LIMIT 1", con);
            cmd.Parameters.AddWithValue("@b", barcode);
            using var dr = cmd.ExecuteReader();
            if (!dr.Read()) return (0, 0);
            return (dr.IsDBNull(0) ? 0 : dr.GetInt32(0), dr.IsDBNull(1) ? 0 : dr.GetInt32(1));
        }

        private ItemUnitsInfo GetUnitsInfo(int itemId)
        {
            if (_unitsInfoByItemId.TryGetValue(itemId, out var cached)) return cached;

            var info = new ItemUnitsInfo();
            info.AllowedUnitIds = new List<int>();
            info.UnitNameById = new Dictionary<int, string>();
            info.UnitConversionToBase = new Dictionary<int, double>();

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using (var cmd = new NpgsqlCommand(@"
SELECT i.unit as base_unit_id, u.name as base_unit_name
FROM items i
LEFT JOIN units u ON u.id = i.unit
WHERE i.id = @id
", con))
            {
                cmd.Parameters.AddWithValue("@id", itemId);
                using var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    info.BaseUnitId = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        info.UnitNameById[info.BaseUnitId] = dr.GetString(1);
                }
            }

            if (info.BaseUnitId > 0)
            {
                info.AllowedUnitIds.Add(info.BaseUnitId);
                info.UnitConversionToBase[info.BaseUnitId] = 1d;
            }

            using (var cmd = new NpgsqlCommand(@"
SELECT uv.unit_id, u.name, uv.conversion
FROM unit_variants uv
LEFT JOIN units u ON u.id = uv.unit_id
WHERE uv.item_id = @id AND uv.is_active = TRUE
ORDER BY u.name
", con))
            {
                cmd.Parameters.AddWithValue("@id", itemId);
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int unitId = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
                    if (unitId <= 0) continue;
                    string name = dr.IsDBNull(1) ? unitId.ToString() : dr.GetString(1);
                    int conv = dr.IsDBNull(2) ? 1 : dr.GetInt32(2);
                    if (!info.AllowedUnitIds.Contains(unitId)) info.AllowedUnitIds.Add(unitId);
                    info.UnitNameById[unitId] = name;
                    info.UnitConversionToBase[unitId] = conv <= 0 ? 1d : conv;
                }
            }

            _unitsInfoByItemId[itemId] = info;
            return info;
        }

        private double GetSystemQtyBase(int itemId, int warehouseId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand("SELECT COALESCE(qty, 0) FROM stocks WHERE item_id=@i AND warehouse_id=@w", con);
            cmd.Parameters.AddWithValue("@i", itemId);
            cmd.Parameters.AddWithValue("@w", warehouseId);
            var v = cmd.ExecuteScalar();
            if (v == null || v == DBNull.Value) return 0d;
            return Convert.ToDouble(v);
        }

        private string GetOrGenerateOpnameNo()
        {
            if (rdoNoManual.Checked)
                return (txtOpnameNo.Text ?? "").Trim();

            int whId = GetSelectedWarehouseId();
            if (whId <= 0) return "";
            string no = GenerateAutoOpnameNo(dtpOpnameDate.Value.Date);
            txtOpnameNo.Text = no;
            return no;
        }

        private string GenerateAutoOpnameNo(DateTime date)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            using var cmdSel = new NpgsqlCommand("SELECT prefix, last_date, last_number FROM stock_opname_numbering WHERE id=1 FOR UPDATE", con, tran);
            using var dr = cmdSel.ExecuteReader();
            string prefix = "SO";
            DateTime? lastDate = null;
            int lastNumber = 0;
            if (dr.Read())
            {
                prefix = dr.IsDBNull(0) ? "SO" : dr.GetString(0);
                lastDate = dr.IsDBNull(1) ? null : dr.GetDateTime(1);
                lastNumber = dr.IsDBNull(2) ? 0 : dr.GetInt32(2);
            }
            dr.Close();

            if (lastDate == null || lastDate.Value.Date != date.Date)
                lastNumber = 0;
            lastNumber += 1;

            using var cmdUpd = new NpgsqlCommand("UPDATE stock_opname_numbering SET last_date=@d, last_number=@n WHERE id=1", con, tran);
            cmdUpd.Parameters.AddWithValue("@d", date.Date);
            cmdUpd.Parameters.AddWithValue("@n", lastNumber);
            cmdUpd.ExecuteNonQuery();
            tran.Commit();

            return $"{prefix}-{date:yyyyMMdd}-{lastNumber:0000}";
        }

        private void EnsureStockOpnameTables()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using (var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS stock_opnames (
  id BIGSERIAL PRIMARY KEY,
  opname_no VARCHAR(50) NOT NULL,
  opname_date DATE NOT NULL,
  warehouse_id INT NOT NULL REFERENCES warehouses(id),
  mode VARCHAR(20) NOT NULL,
  created_by INT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS stock_opname_items (
  id BIGSERIAL PRIMARY KEY,
  opname_id BIGINT NOT NULL REFERENCES stock_opnames(id) ON DELETE CASCADE,
  item_id BIGINT NOT NULL REFERENCES items(id),
  unit_id INT NOT NULL,
  conversion DOUBLE PRECISION NOT NULL DEFAULT 1,
  system_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
  physical_qty_input DOUBLE PRECISION NOT NULL DEFAULT 0,
  physical_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
  diff_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
  note TEXT
);

CREATE TABLE IF NOT EXISTS stock_opname_numbering (
  id INT PRIMARY KEY,
  prefix VARCHAR(10) NOT NULL DEFAULT 'SO',
  last_date DATE NULL,
  last_number INT NOT NULL DEFAULT 0
);
INSERT INTO stock_opname_numbering (id, prefix, last_date, last_number)
VALUES (1, 'SO', NULL, 0)
ON CONFLICT (id) DO NOTHING;
", con))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void SaveOpnameAndApplyStock(string opnameNo, DateTime opnameDate, int warehouseId, string mode, int createdBy, List<OpnameRow> items)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            try
            {
                long opnameId;
                using (var cmd = new NpgsqlCommand(@"
INSERT INTO stock_opnames (opname_no, opname_date, warehouse_id, mode, created_by)
VALUES (@no, @d, @w, @m, @u)
RETURNING id
", con, tran))
                {
                    cmd.Parameters.AddWithValue("@no", opnameNo);
                    cmd.Parameters.AddWithValue("@d", opnameDate);
                    cmd.Parameters.AddWithValue("@w", warehouseId);
                    cmd.Parameters.AddWithValue("@m", mode);
                    cmd.Parameters.AddWithValue("@u", createdBy == 0 ? (object)DBNull.Value : createdBy);
                    opnameId = Convert.ToInt64(cmd.ExecuteScalar());
                }

                foreach (var it in items)
                {
                    double systemQty = GetSystemQtyBase(it.ItemId, warehouseId);
                    double phyBase = it.PhysicalQtyBase;
                    double diff = phyBase - systemQty;

                    using (var cmd = new NpgsqlCommand(@"
INSERT INTO stock_opname_items
(opname_id, item_id, unit_id, conversion, system_qty_base, physical_qty_input, physical_qty_base, diff_qty_base, note)
VALUES
(@oid, @iid, @uid, @conv, @sys, @phy_in, @phy_base, @diff, @note)
", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@oid", opnameId);
                        cmd.Parameters.AddWithValue("@iid", it.ItemId);
                        cmd.Parameters.AddWithValue("@uid", it.UnitId);
                        cmd.Parameters.AddWithValue("@conv", it.ConversionToBase);
                        cmd.Parameters.AddWithValue("@sys", systemQty);
                        cmd.Parameters.AddWithValue("@phy_in", it.PhysicalQtyInput);
                        cmd.Parameters.AddWithValue("@phy_base", phyBase);
                        cmd.Parameters.AddWithValue("@diff", diff);
                        cmd.Parameters.AddWithValue("@note", it.Note ?? "");
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new NpgsqlCommand(@"
INSERT INTO stocks (item_id, warehouse_id, qty)
VALUES (@iid, @w, @q)
ON CONFLICT (item_id, warehouse_id)
DO UPDATE SET qty = EXCLUDED.qty
", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@iid", it.ItemId);
                        cmd.Parameters.AddWithValue("@w", warehouseId);
                        cmd.Parameters.AddWithValue("@q", phyBase);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new NpgsqlCommand("DELETE FROM stock_layers WHERE item_id=@iid AND warehouse_id=@w", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@iid", it.ItemId);
                        cmd.Parameters.AddWithValue("@w", warehouseId);
                        cmd.ExecuteNonQuery();
                    }

                    decimal buyPrice = GetBuyPrice(it.ItemId, con, tran);
                    using (var cmd = new NpgsqlCommand(@"
INSERT INTO stock_layers (item_id, warehouse_id, qty_remaining, buy_price)
VALUES (@iid, @w, @q, @buy)
", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@iid", it.ItemId);
                        cmd.Parameters.AddWithValue("@w", warehouseId);
                        cmd.Parameters.AddWithValue("@q", phyBase);
                        cmd.Parameters.AddWithValue("@buy", buyPrice);
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch
            {
                try { tran.Rollback(); } catch { }
                throw;
            }
        }

        private decimal GetBuyPrice(int itemId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand("SELECT COALESCE(buy_price,0) FROM items WHERE id=@id", con, tran);
            cmd.Parameters.AddWithValue("@id", itemId);
            var v = cmd.ExecuteScalar();
            if (v == null || v == DBNull.Value) return 0m;
            return Convert.ToDecimal(v);
        }

        private void btnPrintBase_Click(object sender, EventArgs e)
        {
            int whId = GetSelectedWarehouseId();
            if (whId <= 0)
            {
                MessageBox.Show("Pilih gudang terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _printAllUnits = false;
            _printRows = LoadPrintRows(whId, false);
            ShowPrintPreview();
        }

        private void btnPrintAll_Click(object sender, EventArgs e)
        {
            int whId = GetSelectedWarehouseId();
            if (whId <= 0)
            {
                MessageBox.Show("Pilih gudang terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _printAllUnits = true;
            _printRows = LoadPrintRows(whId, true);
            ShowPrintPreview();
        }

        private void ShowPrintPreview()
        {
            if (_printRows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk dicetak.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new PrintPreviewDialog();
            dlg.Document = _printDoc;
            dlg.WindowState = FormWindowState.Maximized;
            dlg.ShowDialog(this);
        }

        private void PrintDoc_BeginPrint(object sender, PrintEventArgs e)
        {
            _printRowCursor = 0;
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            var bounds = e.MarginBounds;

            var fontTitle = new Font("Segoe UI", 12f, FontStyle.Bold);
            var font = new Font("Segoe UI", 9f, FontStyle.Regular);
            var fontBold = new Font("Segoe UI", 9f, FontStyle.Bold);

            float y = bounds.Top;
            string title = _printAllUnits ? "Daftar Stock Opname (All Unit)" : "Daftar Stock Opname (Base Unit)";
            g.DrawString(title, fontTitle, Brushes.Black, bounds.Left, y);
            y += fontTitle.GetHeight(g) + 8;

            string wh = cmbWarehouse.Text ?? "";
            string info = $"Tanggal: {dtpOpnameDate.Value:dd/MM/yyyy} | Gudang: {wh}";
            g.DrawString(info, font, Brushes.Black, bounds.Left, y);
            y += font.GetHeight(g) + 10;

            float colNo = 40;
            float colBarcode = 140;
            float colName = bounds.Width - colNo - colBarcode - 80 - 120 - 120;
            if (colName < 180) colName = 180;
            float colUnit = 80;
            float colSystem = 120;
            float colPhysical = 120;

            float x = bounds.Left;
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), bounds.Left, y, bounds.Width, fontBold.GetHeight(g) + 6);
            g.DrawString("No", fontBold, Brushes.Black, x, y + 3); x += colNo;
            g.DrawString("Barcode", fontBold, Brushes.Black, x, y + 3); x += colBarcode;
            g.DrawString("Nama", fontBold, Brushes.Black, x, y + 3); x += colName;
            g.DrawString("Unit", fontBold, Brushes.Black, x, y + 3); x += colUnit;
            g.DrawString("Sistem", fontBold, Brushes.Black, x, y + 3); x += colSystem;
            g.DrawString("Fisik", fontBold, Brushes.Black, x, y + 3);
            y += fontBold.GetHeight(g) + 10;

            int rowNo = 1;
            while (_printRowCursor < _printRows.Count)
            {
                var r = _printRows[_printRowCursor];
                float rowH = font.GetHeight(g) + 6;
                if (y + rowH > bounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                x = bounds.Left;
                g.DrawString(rowNo.ToString(), font, Brushes.Black, x, y); x += colNo;
                g.DrawString(r.Barcode, font, Brushes.Black, x, y); x += colBarcode;
                g.DrawString(Trunc(g, r.Name, font, colName), font, Brushes.Black, x, y); x += colName;
                g.DrawString(r.UnitName, font, Brushes.Black, x, y); x += colUnit;
                g.DrawString(r.SystemQtyText, font, Brushes.Black, x, y); x += colSystem;
                g.DrawString("", font, Brushes.Black, x, y);

                y += rowH;
                _printRowCursor++;
                rowNo++;
            }

            e.HasMorePages = false;
        }

        private static string Trunc(Graphics g, string s, Font f, float maxW)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (g.MeasureString(s, f).Width <= maxW) return s;
            const string ell = "…";
            int len = s.Length;
            while (len > 0)
            {
                var cand = s.Substring(0, len) + ell;
                if (g.MeasureString(cand, f).Width <= maxW) return cand;
                len--;
            }
            return ell;
        }

        private List<PrintRow> LoadPrintRows(int warehouseId, bool allUnits)
        {
            var list = new List<PrintRow>();
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            if (!allUnits)
            {
                using var cmd = new NpgsqlCommand(@"
SELECT i.barcode, i.name, u.name as unit_name,
       COALESCE((SELECT s.qty FROM stocks s WHERE s.item_id=i.id AND s.warehouse_id=@w), 0) as stock_qty
FROM items i
LEFT JOIN units u ON u.id = i.unit
WHERE i.deleted_at IS NULL AND i.is_inventory_p = TRUE
ORDER BY i.name
", con);
                cmd.Parameters.AddWithValue("@w", warehouseId);
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string barcode = dr.IsDBNull(0) ? "" : dr.GetString(0);
                    string name = dr.IsDBNull(1) ? "" : dr.GetString(1);
                    string unit = dr.IsDBNull(2) ? "" : dr.GetString(2);
                    double qty = dr.IsDBNull(3) ? 0d : dr.GetDouble(3);
                    list.Add(new PrintRow { Barcode = barcode, Name = name, UnitName = unit, SystemQtyText = qty.ToString("N2") });
                }
                return list;
            }

            using (var cmd = new NpgsqlCommand(@"
WITH base_items AS (
  SELECT i.id, i.barcode, i.name, i.unit as unit_id,
         u.name as unit_name,
         COALESCE((SELECT s.qty FROM stocks s WHERE s.item_id=i.id AND s.warehouse_id=@w), 0) as stock_base
  FROM items i
  LEFT JOIN units u ON u.id=i.unit
  WHERE i.deleted_at IS NULL AND i.is_inventory_p = TRUE
),
all_units AS (
  SELECT b.id as item_id, b.barcode, b.name, b.unit_name, 1::double precision as conversion, (b.stock_base) as stock_in_unit
  FROM base_items b
  UNION ALL
  SELECT b.id as item_id, b.barcode, b.name, u2.name as unit_name, uv.conversion::double precision as conversion,
         CASE WHEN uv.conversion > 0 THEN (b.stock_base / uv.conversion::double precision) ELSE b.stock_base END as stock_in_unit
  FROM base_items b
  JOIN unit_variants uv ON uv.item_id=b.id AND uv.is_active = TRUE
  LEFT JOIN units u2 ON u2.id = uv.unit_id
)
SELECT barcode, name, unit_name, stock_in_unit
FROM all_units
ORDER BY name, unit_name
", con))
            {
                cmd.Parameters.AddWithValue("@w", warehouseId);
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string barcode = dr.IsDBNull(0) ? "" : dr.GetString(0);
                    string name = dr.IsDBNull(1) ? "" : dr.GetString(1);
                    string unit = dr.IsDBNull(2) ? "" : dr.GetString(2);
                    double qty = dr.IsDBNull(3) ? 0d : dr.GetDouble(3);
                    list.Add(new PrintRow { Barcode = barcode, Name = name, UnitName = unit, SystemQtyText = qty.ToString("N2") });
                }
            }
            return list;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private sealed class OpnameRow
        {
            public int ItemId { get; set; }
            public string Barcode { get; set; } = "";
            public string Name { get; set; } = "";
            public int UnitId { get; set; }
            public string UnitName { get; set; } = "";
            public double ConversionToBase { get; set; } = 1d;
            public double SystemQtyBase { get; set; }
            public double PhysicalQtyInput { get; set; }
            public double PhysicalQtyBase { get; set; }
            public double DiffQtyBase { get; set; }
            public string Note { get; set; } = "";
        }

        private sealed class PrintRow
        {
            public string Barcode { get; set; } = "";
            public string Name { get; set; } = "";
            public string UnitName { get; set; } = "";
            public string SystemQtyText { get; set; } = "";
        }

        private sealed class ItemUnitsInfo
        {
            public int BaseUnitId { get; set; }
            public List<int> AllowedUnitIds { get; set; } = new List<int>();
            public Dictionary<int, string> UnitNameById { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, double> UnitConversionToBase { get; set; } = new Dictionary<int, double>();

            public int ResolveUnitId(string unitText)
            {
                if (string.IsNullOrWhiteSpace(unitText)) return BaseUnitId;
                if (int.TryParse(unitText.Trim(), out var id) && UnitNameById.ContainsKey(id)) return id;
                foreach (var kv in UnitNameById)
                {
                    if (string.Equals(kv.Value, unitText.Trim(), StringComparison.OrdinalIgnoreCase))
                        return kv.Key;
                }
                return 0;
            }
        }
    }
}
