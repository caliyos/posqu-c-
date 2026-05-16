
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_qu.Models;
using POS_qu.Helpers;
using POS_qu.Controllers;
using POS_qu.Core;
using POS_qu.Core.Interfaces;
using POS_qu.Services;
using POS_qu.Repositories;
using System.Transactions;
using System.Security.Cryptography.Xml;
using Npgsql;

namespace POS_qu
{
    public partial class ProductPage : Form
    {
        private ProductService _productService;
        private int currentPage = 1;
        private int pageSize = 15;
        private string selectedImagePath = "";
        private DataGridViewManager dgvManager;
        private List<UnitVariant> unitVariantsFromForm = new List<UnitVariant>(); // Store globally if needed
        private System.Data.DataTable _dtFull;
        private bool _viewAllUnits;
        private bool _lowStockSort;
        private bool _isLoadingWarehouses;
        private readonly Dictionary<int, string> _warehouseNameById = new Dictionary<int, string>();

        private readonly IActivityService _activityService;
        private readonly IStockAdjustmentService _stockService;

        public ProductPage()
        {
            InitializeComponent();  

            ILogger fileLogger = new FileLogger(); // kalau mau ke file
            ILogger dbLogger = new DbLogger();


            // Gabungkan keduanya ke ActivityService
            _activityService = new ActivityService(fileLogger, dbLogger);

            _stockService = new StockAdjustmentService();



            string imageFolder = Path.Combine(Application.StartupPath, "images");
            if (!Directory.Exists(imageFolder))
            {
                Directory.CreateDirectory(imageFolder);
            }

            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;

        }

        //private void SetFormMode(bool isEditing)
        //{
        //    btnSave.Enabled = !isEditing;
        //    btnUpdate.Enabled = isEditing;
        //    btnDelete.Enabled = isEditing;

        //}
        private void TxtBuyPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtSellPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TxtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //private string selectedImagePath = "";



        // DATATABLES FUNCTINOALITY

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cmbPageSize.SelectedItem.ToString(), out int newSize))
            {
                dgvManager.SetPageSize(newSize); // Set new page size and reload
                AdjustDataGridViewHeight(newSize); // Resize the grid visually
            }
        }

        private void AdjustDataGridViewHeight(int rowsPerPage)
        {
            //int rowHeight = dataGridView1.RowTemplate.Height; // default row height
            //int headerHeight = dataGridView1.ColumnHeadersHeight;
            //int extraPadding = 10; // optional for spacing

            //dataGridView1.Height = (rowHeight + rowsPerPage) + headerHeight + extraPadding;
        }

        private void btnNext_Click(object sender, EventArgs e) => dgvManager.NextPage();
        private void btnPrevious_Click(object sender, EventArgs e) => dgvManager.PreviousPage();
        private void btnFirstPage_Click(object sender, EventArgs e) => dgvManager.FirstPage();
        private void btnLastPage_Click(object sender, EventArgs e) => dgvManager.LastPage();

        // For search
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvManager.Filter(txtSearch.Text, "name"); // or any searchable column
        }

        // CUSTOM BUTTON CLOSE
        //private Button btnClose;

        private void chkActionSelectAll_CheckedChanged2(object sender, EventArgs e)
        {
            bool checkAll = chkActionSelectAll.Checked;
            foreach (DataGridViewRow row2 in dataGridView1.Rows)
            {
                if (row2.IsNewRow) continue;
                if (row2.Cells["chkSelect"] != null)
                    row2.Cells["chkSelect"].Value = checkAll;
            }
        }

        private void txtActionSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvManager != null)
            {
                dgvManager.Filter(txtActionSearch.Text, "name");
            }
        }

        private void ProductPage_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized; // Fullscreen

            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.DataBindingComplete += (s, ev) => { ConfigureGridColumns(); ApplyLowStockRowStyles(); UpdateSummaryFromGrid(); };
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.CellDoubleClick += (s, ev) =>
            {
                if (ev.RowIndex < 0) return;
                var row = dataGridView1.Rows[ev.RowIndex];
                OpenEditForRow(row);
            };

            LoadWarehouseFilter();
            LoadItems();
            ApplyProfessionalGridStyle();
        }

        private void LoadWarehouseFilter()
        {
            if (cmbWarehouseFilter == null) return;

            _isLoadingWarehouses = true;
            try
            {
                var wc = new WarehouseController();
                var dt = wc.GetWarehouses();

                var comboDt = new DataTable();
                comboDt.Columns.Add("id", typeof(int));
                comboDt.Columns.Add("name", typeof(string));

                comboDt.Rows.Add(0, "Semua Gudang");
                _warehouseNameById.Clear();

                if (dt != null)
                {
                    bool hasIsActive = dt.Columns.Contains("is_active");
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r == null) continue;
                        if (hasIsActive)
                        {
                            bool isActive = r["is_active"] != DBNull.Value && Convert.ToBoolean(r["is_active"]);
                            if (!isActive) continue;
                        }

                        int id = r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0;
                        if (id <= 0) continue;
                        string name = r["name"]?.ToString() ?? $"Gudang {id}";
                        comboDt.Rows.Add(id, name);
                        if (!_warehouseNameById.ContainsKey(id))
                            _warehouseNameById[id] = name;
                    }
                }

                cmbWarehouseFilter.DataSource = comboDt;
                cmbWarehouseFilter.DisplayMember = "name";
                cmbWarehouseFilter.ValueMember = "id";
                cmbWarehouseFilter.SelectedValue = 0;

                cmbWarehouseFilter.SelectedIndexChanged -= cmbWarehouseFilter_SelectedIndexChanged;
                cmbWarehouseFilter.SelectedIndexChanged += cmbWarehouseFilter_SelectedIndexChanged;
            }
            catch
            {
                cmbWarehouseFilter.DataSource = null;
                cmbWarehouseFilter.Items.Clear();
                cmbWarehouseFilter.Items.Add("Semua Gudang");
                cmbWarehouseFilter.SelectedIndex = 0;
            }
            finally
            {
                _isLoadingWarehouses = false;
            }
        }

        private void EnsureWarehouseNameColumn(DataTable dt)
        {
            if (dt == null) return;
            if (!dt.Columns.Contains("warehouse_id")) return;
            if (!dt.Columns.Contains("warehouse_name"))
                dt.Columns.Add("warehouse_name", typeof(string));

            foreach (DataRow r in dt.Rows)
            {
                int wid = 0;
                try
                {
                    if (r["warehouse_id"] != DBNull.Value)
                        wid = Convert.ToInt32(r["warehouse_id"]);
                }
                catch
                {
                    wid = 0;
                }

                string existing = "";
                try
                {
                    existing = r["warehouse_name"] == DBNull.Value ? "" : (r["warehouse_name"]?.ToString() ?? "");
                }
                catch
                {
                    existing = "";
                }

                if (wid <= 0)
                {
                    if (string.IsNullOrWhiteSpace(existing))
                        r["warehouse_name"] = "-";
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(existing))
                    continue;

                if (_warehouseNameById.TryGetValue(wid, out var name) && !string.IsNullOrWhiteSpace(name))
                    r["warehouse_name"] = name;
                else
                    r["warehouse_name"] = $"Gudang {wid}";
            }
        }

        private void cmbWarehouseFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingWarehouses) return;
            LoadItems();
        }

        private int GetSelectedWarehouseId()
        {
            try
            {
                if (cmbWarehouseFilter?.SelectedValue == null) return 0;
                return Convert.ToInt32(cmbWarehouseFilter.SelectedValue);
            }
            catch
            {
                return 0;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // header diklik

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnHppHistory")
            {
                if (row.Cells["id"]?.Value == null || row.Cells["id"].Value == DBNull.Value) return;
                int itemId = Convert.ToInt32(row.Cells["id"].Value);
                using var f = new HppHistoryForm(itemId);
                f.ShowDialog(this);
                return;
            }

            if (dataGridView1.Columns[e.ColumnIndex].Name == "stock")
            {
                if (row.Cells["id"]?.Value == null || row.Cells["id"].Value == DBNull.Value) return;
                int itemId = Convert.ToInt32(row.Cells["id"].Value);
                using var f = new HppHistoryForm(itemId, openStockCard: true);
                f.ShowDialog(this);
                return;
            }

            // jika kolom checkbox diklik, toggle value
            if (dataGridView1.Columns[e.ColumnIndex].Name == "chkSelect")
            {
                bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value);
                row.Cells["chkSelect"].Value = !isChecked;
            }
            else
            {
                // klik row selain checkbox -> toggle checkbox juga
                bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value ?? false);
                row.Cells["chkSelect"].Value = !isChecked;
            }
        }

        private void btnViewBase_Click(object sender, EventArgs e)
        {
            _viewAllUnits = false;
            LoadItems();
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            _viewAllUnits = true;
            LoadItems();
        }

        private void btnLowStock_Click(object sender, EventArgs e)
        {
            _lowStockSort = !_lowStockSort;
            LoadItems();
        }

        private void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            var ids = new HashSet<int>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (!Convert.ToBoolean(row.Cells["chkSelect"].Value ?? false)) continue;
                if (row.Cells["id"]?.Value == null) continue;
                ids.Add(Convert.ToInt32(row.Cells["id"].Value));
            }

            using var f = new BarcodePrintForm(ids);
            f.ShowDialog(this);
        }

        private void UpdateViewModeButtonsAppearance()
        {
            if (btnViewBase == null || btnViewAll == null) return;

            if (_viewAllUnits)
            {
                btnViewAll.BackColor = Color.FromArgb(0, 120, 215);
                btnViewAll.ForeColor = Color.White;
                btnViewBase.BackColor = Color.White;
                btnViewBase.ForeColor = Color.Black;
            }
            else
            {
                btnViewBase.BackColor = Color.FromArgb(0, 120, 215);
                btnViewBase.ForeColor = Color.White;
                btnViewAll.BackColor = Color.White;
                btnViewAll.ForeColor = Color.Black;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "stock") return;

            var row = dataGridView1.Rows[e.RowIndex];
            if (row.DataBoundItem is not DataRowView drv) return;
            var dr = drv.Row;

            decimal stock = 0m;
            if (dr.Table.Columns.Contains("stock") && dr["stock"] != DBNull.Value) decimal.TryParse(dr["stock"].ToString(), out stock);
            var min = GetMinThreshold(dr);

            if (min > 0m && stock <= min)
            {
                bool isVariant = false;
                if (dr.Table.Columns.Contains("is_variant") && dr["is_variant"] != DBNull.Value)
                    bool.TryParse(dr["is_variant"].ToString(), out isVariant);

                var back = isVariant ? Color.FromArgb(231, 97, 93) : Color.FromArgb(217, 83, 79);
                e.CellStyle.BackColor = back;
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.SelectionBackColor = back;
                e.CellStyle.SelectionForeColor = Color.White;
            }
        }

        private void ApplyLowStockRowStyles()
        {
            if (dataGridView1.DataSource == null) return;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.DataBoundItem is not DataRowView drv) continue;
                var dr = drv.Row;

                decimal stock = 0m;
                if (dr.Table.Columns.Contains("stock") && dr["stock"] != DBNull.Value)
                    decimal.TryParse(dr["stock"].ToString(), out stock);

                bool isVariant = false;
                if (dr.Table.Columns.Contains("is_variant") && dr["is_variant"] != DBNull.Value)
                    bool.TryParse(dr["is_variant"].ToString(), out isVariant);

                var min = GetMinThreshold(dr);
                if (min > 0m && stock <= min)
                {
                    var back = isVariant ? Color.FromArgb(231, 97, 93) : Color.FromArgb(217, 83, 79);
                    var fore = Color.White;

                    row.DefaultCellStyle.BackColor = back;
                    row.DefaultCellStyle.ForeColor = fore;
                    row.DefaultCellStyle.SelectionBackColor = back;
                    row.DefaultCellStyle.SelectionForeColor = fore;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = dataGridView1.DefaultCellStyle.ForeColor;
                    row.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.SelectionBackColor;
                    row.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.SelectionForeColor;
                }
            }
        }

        private static decimal GetMinThreshold(DataRow dr)
        {
            if (dr == null) return 0m;

            decimal minStock = 0m;
            if (dr.Table.Columns.Contains("min_stock") && dr["min_stock"] != DBNull.Value)
                decimal.TryParse(dr["min_stock"].ToString(), out minStock);

            decimal minQtyBase = 0m;
            if (dr.Table.Columns.Contains("min_qty") && dr["min_qty"] != DBNull.Value)
                decimal.TryParse(dr["min_qty"].ToString(), out minQtyBase);

            bool isVariant = false;
            if (dr.Table.Columns.Contains("is_variant") && dr["is_variant"] != DBNull.Value)
                bool.TryParse(dr["is_variant"].ToString(), out isVariant);

            if (isVariant)
            {
                return minStock;
            }

            return minStock > 0m ? minStock : minQtyBase;
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Ambil semua item yang dicentang
            var selectedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chkSelect"].Value))
                    selectedRows.Add(row);
            }

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Pilih item dulu untuk diedit.");
                return;
            }
            else if (selectedRows.Count > 1)
            {
                MessageBox.Show("Edit hanya bisa dilakukan 1 item sekaligus.");
                return;
            }

            // Ambil row tunggal yang dicentang
            DataGridViewRow rowSelected = selectedRows[0];
            int itemId = Convert.ToInt32(rowSelected.Cells["id"].Value);

            var selectedItem = _productService.GetProductDetail(itemId);
            if (selectedItem == null)
            {
                MessageBox.Show("Item tidak ditemukan.");
                return;
            }

            selectedItem.UnitVariants = _productService.GetItemUnitVariants(itemId);

            using (var detailForm = new ItemDetailForm(selectedItem))
            {
                if (detailForm.ShowDialog() == DialogResult.OK)
                    LoadItems(); // reload grid setelah edit
            }
        }


        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (!POS_qu.Helpers.Utility.EnsurePermission(this, "delete_product", "Hapus Item"))
                return;

            var selectedItems = new HashSet<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chkSelect"].Value))
                    selectedItems.Add(Convert.ToInt32(row.Cells["id"].Value));
            }

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Pilih item dulu untuk dihapus.");
                return;
            }

            string confirmText =
                $"Hapus {selectedItems.Count} item terpilih?\n\n" +
                "Aturan sistem:\n" +
                "- Tidak pernah hapus permanen\n" +
                "- Item akan dinonaktifkan (arsip)\n\n" +
                "Histori transaksi tetap aman.";

            if (MessageBox.Show(confirmText, "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int archivedCount = 0;
                int failedCount = 0;

                foreach (int id in selectedItems)
                {
                    bool ok = _productService.DeleteProduct(id, out bool archived, out string msg);
                    if (!ok)
                    {
                        failedCount++;
                        continue;
                    }

                    if (archived) archivedCount++;
                }
                LoadItems(); // reload grid

                if (failedCount == 0)
                    MessageBox.Show($"Selesai.\nDinonaktifkan (arsip): {archivedCount}");
                else
                    MessageBox.Show($"Selesai.\nDinonaktifkan (arsip): {archivedCount}\nGagal: {failedCount}");
            }
        }

      

        // CUSTOM BUTTON CLOSE
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadItems()
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            _productService = new ProductService(new ProductRepository());
            DataTable dt = _viewAllUnits ? GetAllProductsExpandedByUnitVariant() : GetBaseProductsForCurrentWarehouse();
            if (dt != null && _lowStockSort)
                dt = SortLowStock(dt);

            if (dt != null)
                EnsureWarehouseNameColumn(dt);
            _dtFull = dt?.Copy();

            if (dt == null)
            {
                dataGridView1.DataSource = null;
                return;
            }

            EnsureValueColumns(dt);
            UpdateValueColumns(dt);

            dataGridView1.DataSource = dt;
            if (dataGridView1.Columns.Contains("stock_value"))
            {
                dataGridView1.Columns["stock_value"].HeaderText = "Nilai Stok (HPP)";
                dataGridView1.Columns["stock_value"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["stock_value"].DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                dataGridView1.Columns["stock_value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dataGridView1.Columns.Contains("retail_value"))
            {
                dataGridView1.Columns["retail_value"].HeaderText = "Nilai Jual";
                dataGridView1.Columns["retail_value"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["retail_value"].DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                dataGridView1.Columns["retail_value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dataGridView1.Columns.Contains("reserved_qty"))
            {
                var col = dataGridView1.Columns["reserved_qty"];

                col.HeaderText = "Reserved Qty";

                // format angka
                col.DefaultCellStyle.Format = "N2";

                // pakai culture Indonesia
                col.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;

                col.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }
            dgvManager = new DataGridViewManager(dataGridView1, dt, 100);
            dgvManager.PagingInfoLabel = lblPagingInfo;
            dgvManager.OnAfterLoadPage += () =>
            {
                EnsureSelectCheckboxColumn();
                ApplyProfessionalGridStyle();
                ConfigureGridColumns();
                ApplyLowStockRowStyles();
                UpdateSummaryFromGrid();
            };
            dgvManager.LoadPage();

            cmbPageSize.Items.Clear();
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 200, 500, 1000 });
            if (cmbPageSize.Items.Count > 0) cmbPageSize.SelectedItem = 200;


            ApplyProfessionalGridStyle();
            UpdateSummaryFromGrid();
            UpdateViewModeButtonsAppearance();
            UpdateLowStockButtonAppearance();
        }

        private DataTable GetBaseProductsForCurrentWarehouse()
        {
            int wid = GetSelectedWarehouseId();

            if (wid <= 0)
            {
                return GetBaseProductsForAllWarehouses();
            }

            try
            {
                using var con = new NpgsqlConnection(DbConfig.ConnectionString);
                con.Open();

                string sql = @"
            SELECT 
                items.id,
                items.name,
                items.barcode,
                items.unit AS unit_id,
                units.name AS unit_name,
                CAST(COALESCE(s.qty, 0) AS NUMERIC(18,4)) AS stock,
                items.sell_price,
                items.buy_price,
                CAST(COALESCE(s.reserved_qty, 0) AS NUMERIC(18,4)) AS reserved_qty,
                CAST(COALESCE(s.min_qty, 0) AS NUMERIC(18,4)) AS min_qty,
                CAST(COALESCE(uvbase.minqty, 0) AS NUMERIC(18,4)) AS min_stock,
                w.id AS warehouse_id,
                w.name AS warehouse_name
                items.valuation_method,
              
                
                items.category_id,
                categories.name AS category_name,
                items.note,
                items.picture,
                items.is_inventory_p,
                items.is_purchasable,
                items.is_sellable,
                items.is_note_payment,
                items.is_changeprice_p,
                items.is_have_bahan,
                items.is_box,
                items.is_produksi,
                items.discount_formula,
                items.supplier_id,
                suppliers.name AS supplier_name,
                items.flag,
                items.created_at,
                items.updated_at
            FROM items
            JOIN warehouses w ON w.id = @warehouse_id
            LEFT JOIN stocks s ON s.item_id = items.id AND s.warehouse_id = w.id
            LEFT JOIN LATERAL (
                SELECT iv.minqty
                FROM unit_variants iv
                WHERE iv.item_id = items.id
                  AND iv.is_active = TRUE
                  AND iv.is_base_unit = TRUE
                LIMIT 1
            ) uvbase ON TRUE
            LEFT JOIN units       ON items.unit = units.id
            LEFT JOIN categories  ON items.category_id = categories.id
            LEFT JOIN suppliers   ON items.supplier_id = suppliers.id
            WHERE items.deleted_at IS NULL
            ORDER BY items.id ASC
        ";

                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@warehouse_id", wid);

                using var da = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                var dt = _productService.GetAllProducts();
                if (dt != null)
                {
                    if (!dt.Columns.Contains("warehouse_id")) dt.Columns.Add("warehouse_id", typeof(int));
                    if (!dt.Columns.Contains("warehouse_name")) dt.Columns.Add("warehouse_name", typeof(string));
                    foreach (DataRow r in dt.Rows)
                    {
                        r["warehouse_id"] = 0;
                        r["warehouse_name"] = "Semua Gudang";
                    }
                }
                return dt;
            }
        }

        private DataTable GetBaseProductsForAllWarehouses()
        {
            try
            {
                using var con = new NpgsqlConnection(DbConfig.ConnectionString);
                con.Open();

                string sql = @"
SELECT 
    items.id,
    items.name,
    items.barcode,
    items.unit AS unit_id,
    units.name AS unit_name,
    CAST(COALESCE(s.qty, 0) AS NUMERIC(18,4)) AS stock,
    items.sell_price,
    items.buy_price,
    COALESCE(s.reserved_qty, 0) AS NUMERIC(18,4)) AS reserved_qty,
    CAST(COALESCE(s.min_qty, 0) AS NUMERIC(18,4)) AS min_qty,
    CAST(COALESCE(uvbase.minqty, 0) AS NUMERIC(18,4)) AS min_stock,
    items.valuation_method,

    items.category_id,
    categories.name AS category_name,
    items.note,
    items.picture,
    items.is_inventory_p,
    items.is_purchasable,
    items.is_sellable,
    items.is_note_payment,
    items.is_changeprice_p,
    items.is_have_bahan,
    items.is_box,
    items.is_produksi,
    items.discount_formula,
    items.supplier_id,
    suppliers.name AS supplier_name,
    items.flag,
    items.created_at,
    items.updated_at,
    COALESCE(w.id, 0) AS warehouse_id,
    COALESCE(w.name, '-') AS warehouse_name
FROM items
LEFT JOIN stocks s ON s.item_id = items.id
LEFT JOIN warehouses w ON w.id = s.warehouse_id
LEFT JOIN LATERAL (
    SELECT iv.minqty
    FROM unit_variants iv
    WHERE iv.item_id = items.id
      AND iv.is_active = TRUE
      AND iv.is_base_unit = TRUE
    LIMIT 1
) uvbase ON TRUE
LEFT JOIN units       ON items.unit = units.id
LEFT JOIN categories  ON items.category_id = categories.id
LEFT JOIN suppliers   ON items.supplier_id = suppliers.id
WHERE items.deleted_at IS NULL
  AND (w.is_active = TRUE OR w.id IS NULL)
ORDER BY items.id ASC, COALESCE(w.id, 0) ASC
";

                using var da = new NpgsqlDataAdapter(sql, con);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                var dt = _productService.GetAllProducts();
                if (dt != null)
                {
                    if (!dt.Columns.Contains("warehouse_id")) dt.Columns.Add("warehouse_id", typeof(int));
                    if (!dt.Columns.Contains("warehouse_name")) dt.Columns.Add("warehouse_name", typeof(string));
                    foreach (DataRow r in dt.Rows)
                    {
                        r["warehouse_id"] = 0;
                        r["warehouse_name"] = "-";
                    }
                }
                return dt;
            }
        }

        private void UpdateLowStockButtonAppearance()
        {
            if (btnLowStock == null) return;
            if (_lowStockSort)
            {
                btnLowStock.BackColor = Color.FromArgb(0, 120, 215);
                btnLowStock.ForeColor = Color.White;
            }
            else
            {
                btnLowStock.BackColor = Color.White;
                btnLowStock.ForeColor = Color.Black;
            }
        }

        private static DataTable SortLowStock(DataTable dt)
        {
            if (dt == null) return dt;
            if (!dt.Columns.Contains("stock") || !dt.Columns.Contains("min_stock")) return dt;

            var rows = dt.AsEnumerable()
                .OrderBy(r =>
                {
                    decimal stock = r["stock"] != DBNull.Value ? Convert.ToDecimal(r["stock"]) : 0m;
                    decimal min = r["min_stock"] != DBNull.Value ? Convert.ToDecimal(r["min_stock"]) : 0m;
                    if (min <= 0m) return decimal.MaxValue;
                    return stock - min;
                })
                .ThenBy(r => r["stock"] != DBNull.Value ? Convert.ToDecimal(r["stock"]) : 0m);

            var sorted = dt.Clone();
            foreach (var r in rows)
                sorted.ImportRow(r);
            return sorted;
        }

        private void EnsureSelectCheckboxColumn()
        {
            if (dataGridView1.Columns.Contains("chkSelect")) return;

            var chkColumn = new DataGridViewCheckBoxColumn
            {
                Name = "chkSelect",
                HeaderText = "",
                Width = 30,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dataGridView1.Columns.Insert(0, chkColumn);
        }

        private void EnsureValueColumns(DataTable dt)
        {
            if (!dt.Columns.Contains("stock_value"))
                dt.Columns.Add("stock_value", typeof(decimal));
            if (!dt.Columns.Contains("retail_value"))
                dt.Columns.Add("retail_value", typeof(decimal));
            if (!dt.Columns.Contains("min_threshold"))
                dt.Columns.Add("min_threshold", typeof(decimal));
        }

        private void UpdateValueColumns(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    decimal buy = row.Table.Columns.Contains("buy_price") && row["buy_price"] != DBNull.Value ? Convert.ToDecimal(row["buy_price"]) : 0m;
                    decimal sell = row.Table.Columns.Contains("sell_price") && row["sell_price"] != DBNull.Value ? Convert.ToDecimal(row["sell_price"]) : 0m;
                    decimal stok = row.Table.Columns.Contains("stock") && row["stock"] != DBNull.Value ? Convert.ToDecimal(row["stock"]) : 0m;
                    row["stock_value"] = buy * stok;
                    row["retail_value"] = sell * stok;
                    row["min_threshold"] = GetMinThreshold(row);
                }
                catch
                {
                    row["stock_value"] = 0m;
                    row["retail_value"] = 0m;
                    row["min_threshold"] = 0m;
                }
            }
        }

        private DataTable GetAllProductsExpandedByUnitVariant()
        {
            var baseDt = GetBaseProductsForCurrentWarehouse();
            if (baseDt == null) return null;

            var dt = baseDt.Clone();
            if (!dt.Columns.Contains("min_stock")) dt.Columns.Add("min_stock", typeof(decimal));
            if (!dt.Columns.Contains("conversion")) dt.Columns.Add("conversion", typeof(int));
            if (!dt.Columns.Contains("is_variant")) dt.Columns.Add("is_variant", typeof(bool));
            if (!dt.Columns.Contains("base_stock")) dt.Columns.Add("base_stock", typeof(decimal));
            if (!dt.Columns.Contains("base_buy_price")) dt.Columns.Add("base_buy_price", typeof(decimal));

            foreach (DataRow src in baseDt.Rows)
            {
                int itemId = src["id"] != DBNull.Value ? Convert.ToInt32(src["id"]) : 0;
                if (itemId <= 0) continue;

                decimal baseStock = src.Table.Columns.Contains("stock") && src["stock"] != DBNull.Value ? Convert.ToDecimal(src["stock"]) : 0m;
                decimal baseBuy = src.Table.Columns.Contains("buy_price") && src["buy_price"] != DBNull.Value ? Convert.ToDecimal(src["buy_price"]) : 0m;
                int baseUnitId = src.Table.Columns.Contains("unit_id") && src["unit_id"] != DBNull.Value ? Convert.ToInt32(src["unit_id"]) : 0;

                var baseRow = dt.NewRow();
                foreach (DataColumn c in baseDt.Columns)
                    baseRow[c.ColumnName] = src[c.ColumnName];
                decimal baseMin = src.Table.Columns.Contains("min_stock") && src["min_stock"] != DBNull.Value ? Convert.ToDecimal(src["min_stock"]) : 0m;
                baseRow["min_stock"] = baseMin;
                baseRow["conversion"] = 1;
                baseRow["is_variant"] = false;
                baseRow["base_stock"] = baseStock;
                baseRow["base_buy_price"] = baseBuy;
                dt.Rows.Add(baseRow);

                var variants = _productService.GetItemUnitVariants(itemId) ?? new List<UnitVariant>();
                foreach (var v in variants)
                {
                    if (v == null) continue;
                    if (v.UnitId == baseUnitId) continue;
                    if (v.Conversion <= 0) continue;

                    var r = dt.NewRow();
                    foreach (DataColumn c in baseDt.Columns)
                        r[c.ColumnName] = src[c.ColumnName];

                    if (dt.Columns.Contains("unit_id")) r["unit_id"] = v.UnitId;
                    if (dt.Columns.Contains("unit_name")) r["unit_name"] = v.UnitName ?? "";
                    if (dt.Columns.Contains("sell_price")) r["sell_price"] = v.SellPrice;
                    if (dt.Columns.Contains("buy_price")) r["buy_price"] = baseBuy * v.Conversion;
                    if (dt.Columns.Contains("stock"))
                    {
                        var stockColType = dt.Columns["stock"].DataType;
                        var stockConv = baseStock / v.Conversion;
                        if (stockColType == typeof(int) || stockColType == typeof(long) || stockColType == typeof(short))
                        {
                            var floored = stockConv < 0m ? 0m : Math.Floor(stockConv);
                            if (stockColType == typeof(int)) r["stock"] = Convert.ToInt32(floored);
                            else if (stockColType == typeof(short)) r["stock"] = Convert.ToInt16(floored);
                            else r["stock"] = Convert.ToInt64(floored);
                        }
                        else
                        {
                            r["stock"] = stockConv;
                        }
                    }

                    r["min_stock"] = v.MinQty;
                    r["conversion"] = v.Conversion;
                    r["is_variant"] = true;
                    r["base_stock"] = baseStock;
                    r["base_buy_price"] = baseBuy;
                    dt.Rows.Add(r);
                }
            }

            return dt;
        }


        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool checkAll = chkSelectAll.Checked;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["chkSelect"].Value = checkAll;
            }
        }

        //private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    bool checkAll = chkSelectAll.Checked;
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        row.Cells["chkSelect"].Value = checkAll;
        //    }
        //}


        //private void ClearInputs()
        //{
        //    txtName.Clear();
        //    txtBuyPrice.Clear();
        //    txtSellPrice.Clear();
        //    txtStock.Clear();
        //    txtBarcode.Clear();
        //    cmbUnit.SelectedIndex = -1;
        //    cmbCategory.SelectedIndex = -1;
        //    txtDescription.Clear();
        //    pictureBox.Image = null;
        //    selectedImagePath = "";

        //    SetFormMode(false); // Set to 'new entry' mode
        //}

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Generate unique filename
                string fileExt = Path.GetExtension(ofd.FileName);
                string newFileName = Guid.NewGuid().ToString() + fileExt;
                string imagesFolder = Path.Combine(Application.StartupPath, "images");
                string destinationPath = Path.Combine(imagesFolder, newFileName);

                // Copy image
                File.Copy(ofd.FileName, destinationPath);

                // Save relative path
                selectedImagePath = Path.Combine("images", newFileName);

                // Preview
                //pictureBox.Image = Image.FromFile(destinationPath);
            }
        }

       

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            //ClearInputs(); // Clears form & resets mode
            dataGridView1.ClearSelection(); // Deselect any selected row
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Pilih lokasi simpan file
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "Products_MultiWarehouse_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = sfd.FileName;
                    ExportProductsMultiWarehouseExcel(filePath);

                    MessageBox.Show("Data berhasil diexport ke Excel:\n" + filePath, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal export Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportProductsMultiWarehouseExcel(string filePath)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            var dtItems = new DataTable();
            var dtStocks = new DataTable();

            using (var cmd = new NpgsqlCommand(@"
SELECT
    i.id,
    i.name,
    i.buy_price,
    i.sell_price,
    i.barcode,
    i.unit,
    i.category_id,
    i.supplier_id,
    i.note,
    i.is_inventory_p,
    i.is_changeprice_p
FROM items i
WHERE i.deleted_at IS NULL
ORDER BY i.id ASC
", con))
            {
                using var da = new NpgsqlDataAdapter(cmd);
                da.Fill(dtItems);
            }

            using (var cmd = new NpgsqlCommand(@"
SELECT
    i.barcode,
    i.name,
    w.id AS warehouse_id,
    w.name AS warehouse_name,
    s.qty,
    s.min_qty,
    s.reserved_qty
FROM items i
JOIN stocks s ON s.item_id = i.id
JOIN warehouses w ON w.id = s.warehouse_id
WHERE i.deleted_at IS NULL
ORDER BY i.id ASC, w.id ASC
", con))
            {
                using var da = new NpgsqlDataAdapter(cmd);
                da.Fill(dtStocks);
            }

            using var workbook = new ClosedXML.Excel.XLWorkbook();

            var wsItems = workbook.Worksheets.Add("Items");
            wsItems.Cell(1, 1).Value = "name";
            wsItems.Cell(1, 2).Value = "buy_price";
            wsItems.Cell(1, 3).Value = "sell_price";
            wsItems.Cell(1, 4).Value = "barcode";
            wsItems.Cell(1, 5).Value = "unit";
            wsItems.Cell(1, 6).Value = "group";
            wsItems.Cell(1, 7).Value = "supplier_id";
            wsItems.Cell(1, 8).Value = "note";
            wsItems.Cell(1, 9).Value = "is_inventory_p";
            wsItems.Cell(1, 10).Value = "is_changeprice_p";
            wsItems.Row(1).Style.Font.Bold = true;

            int r = 2;
            foreach (DataRow dr in dtItems.Rows)
            {
                wsItems.Cell(r, 1).Value = dr["name"]?.ToString() ?? "";
                wsItems.Cell(r, 2).Value = dr["buy_price"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["buy_price"]);
                wsItems.Cell(r, 3).Value = dr["sell_price"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["sell_price"]);
                wsItems.Cell(r, 4).Value = dr["barcode"]?.ToString() ?? "";
                wsItems.Cell(r, 5).Value = dr["unit"] == DBNull.Value ? 0 : Convert.ToInt32(dr["unit"]);
                wsItems.Cell(r, 6).Value = dr["category_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["category_id"]);
                wsItems.Cell(r, 7).Value = dr["supplier_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["supplier_id"]);
                wsItems.Cell(r, 8).Value = dr["note"]?.ToString() ?? "";
                wsItems.Cell(r, 9).Value = (dr["is_inventory_p"] != DBNull.Value && Convert.ToBoolean(dr["is_inventory_p"])) ? "Y" : "N";
                wsItems.Cell(r, 10).Value = (dr["is_changeprice_p"] != DBNull.Value && Convert.ToBoolean(dr["is_changeprice_p"])) ? "Y" : "N";
                r++;
            }
            wsItems.Columns().AdjustToContents();

            var wsStocks = workbook.Worksheets.Add("Stocks");
            wsStocks.Cell(1, 1).Value = "barcode";
            wsStocks.Cell(1, 2).Value = "warehouse_id";
            wsStocks.Cell(1, 3).Value = "warehouse_name";
            wsStocks.Cell(1, 4).Value = "qty";
            wsStocks.Cell(1, 5).Value = "min_qty";
            wsStocks.Cell(1, 6).Value = "reserved_qty";
            wsStocks.Row(1).Style.Font.Bold = true;

            r = 2;
            foreach (DataRow dr in dtStocks.Rows)
            {
                wsStocks.Cell(r, 1).Value = dr["barcode"]?.ToString() ?? "";
                wsStocks.Cell(r, 2).Value = dr["warehouse_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["warehouse_id"]);
                wsStocks.Cell(r, 3).Value = dr["warehouse_name"]?.ToString() ?? "";
                wsStocks.Cell(r, 4).Value = dr["qty"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["qty"]);
                wsStocks.Cell(r, 5).Value = dr["min_qty"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["min_qty"]);
                wsStocks.Cell(r, 6).Value = dr["reserved_qty"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["reserved_qty"]);
                r++;
            }
            wsStocks.Columns().AdjustToContents();

            CreateReferenceSheet(workbook);
            workbook.SaveAs(filePath);
        }


        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            Form importForm = new Form();
            importForm.Text = "Import Item";
            importForm.StartPosition = FormStartPosition.CenterParent;
            importForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            importForm.Width = 580;
            importForm.Height = 670;
            importForm.MaximizeBox = false;
            importForm.MinimizeBox = false;

            // Label judul
            Label lblTitle = new Label()
            {
                Text = "Import Data Item",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            importForm.Controls.Add(lblTitle);

            // Tombol unduh template
            Button btnDownloadTemplate = new Button()
            {
                Text = "📥 Unduh Template Excel",
                Location = new Point(20, 50),
                Width = 220,
                Height = 30
            };
            btnDownloadTemplate.Click += (s, ev) =>
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = "template_import_product_multi_gudang.xlsx"
                };
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    CreateTemplateExcel(saveDialog.FileName);
                    MessageBox.Show("Template berhasil dibuat di: " + saveDialog.FileName);
                }
            };
            importForm.Controls.Add(btnDownloadTemplate);

            // Label Upload
            Label lblUpload = new Label()
            {
                Text = "Pilih file Excel untuk import:",
                AutoSize = true,
                Location = new Point(20, 100)
            };
            importForm.Controls.Add(lblUpload);

            // TextBox path
            TextBox txtFile = new TextBox()
            {
                Location = new Point(20, 125),
                Width = 390,
                ReadOnly = true
            };
            importForm.Controls.Add(txtFile);

            // Tombol browse
            Button btnBrowse = new Button()
            {
                Text = "Browse...",
                Location = new Point(420, 123),
                Width = 120,
                Height = 30
            };
            btnBrowse.Click += (s, ev) =>
            {
                OpenFileDialog open = new OpenFileDialog
                {
                    Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv"
                };
                if (open.ShowDialog() == DialogResult.OK)
                {
                    txtFile.Text = open.FileName;
                }
            };
            importForm.Controls.Add(btnBrowse);

            Label lblStatus = new Label()
            {
                Text = "",
                AutoSize = false,
                Location = new Point(20, 165),
                Width = 520,
                Height = 22
            };
            importForm.Controls.Add(lblStatus);

            ProgressBar progress = new ProgressBar()
            {
                Location = new Point(20, 190),
                Width = 520,
                Height = 14,
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };
            importForm.Controls.Add(progress);

            TextBox txtLog = new TextBox()
            {
                Location = new Point(20, 215),
                Width = 520,
                Height = 360,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            importForm.Controls.Add(txtLog);

            Button btnCopyLog = new Button()
            {
                Text = "Copy Log",
                Location = new Point(20, 585),
                Width = 110,
                Height = 35
            };
            btnCopyLog.Click += (s, ev) =>
            {
                try
                {
                    Clipboard.SetText(txtLog.Text ?? "");
                    MessageBox.Show("Log berhasil di-copy.");
                }
                catch
                {
                }
            };
            importForm.Controls.Add(btnCopyLog);

            // Tombol Import
            Button btnDoImport = new Button()
            {
                Text = "Import Sekarang",
                BackColor = System.Drawing.Color.FromArgb(0, 123, 255),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(250, 585),
                Width = 170,
                Height = 35
            };
            btnDoImport.Click += async (s, ev) =>
            {
                if (string.IsNullOrEmpty(txtFile.Text))
                {
                    MessageBox.Show("Pilih file terlebih dahulu!");
                    return;
                }

                try
                {
                    btnDoImport.Enabled = false;
                    btnBrowse.Enabled = false;
                    btnDownloadTemplate.Enabled = false;
                    btnCopyLog.Enabled = false;

                    txtLog.Clear();
                    lblStatus.Text = "Memulai import...";
                    progress.Value = 0;

                    Action<string> appendLog = (msg) =>
                    {
                        if (importForm.IsDisposed) return;
                        if (txtLog.InvokeRequired)
                        {
                            txtLog.BeginInvoke((Action)(() =>
                            {
                                txtLog.AppendText(msg + Environment.NewLine);
                            }));
                        }
                        else
                        {
                            txtLog.AppendText(msg + Environment.NewLine);
                        }
                    };

                    Action<int, int> setProgress = (done, total) =>
                    {
                        if (importForm.IsDisposed) return;
                        if (progress.InvokeRequired)
                        {
                            progress.BeginInvoke((Action)(() =>
                            {
                                progress.Maximum = Math.Max(1, total);
                                progress.Value = Math.Max(0, Math.Min(done, progress.Maximum));
                                lblStatus.Text = $"Proses import: {done:N0}/{total:N0}";
                            }));
                        }
                        else
                        {
                            progress.Maximum = Math.Max(1, total);
                            progress.Value = Math.Max(0, Math.Min(done, progress.Maximum));
                            lblStatus.Text = $"Proses import: {done:N0}/{total:N0}";
                        }
                    };

                    var result = await Task.Run(() => ImportItemsFromExcelImpl(txtFile.Text, appendLog, appendLog, setProgress));

                    appendLog("");
                    appendLog("=== RINGKASAN ===");
                    appendLog($"Item Insert: {result.Inserted}");
                    appendLog($"Item Update: {result.Updated}");
                    appendLog($"Item Gagal: {result.ItemFailed}");
                    appendLog($"Stock OK: {result.StockOk}");
                    appendLog($"Stock Gagal: {result.StockFailed}");
                    appendLog($"Stock di-reset untuk item: {result.StockResetItems}");

                    if (result.Errors.Count > 0)
                    {
                        appendLog("");
                        appendLog("=== ERROR (sample) ===");
                        foreach (var er in result.Errors.Take(200))
                            appendLog(er);
                    }

                    lblStatus.Text = "Selesai.";
                    LoadItems();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal import: " + ex.Message);
                }
                finally
                {
                    btnDoImport.Enabled = true;
                    btnBrowse.Enabled = true;
                    btnDownloadTemplate.Enabled = true;
                    btnCopyLog.Enabled = true;
                }
            };
            importForm.Controls.Add(btnDoImport);

            // Tombol Batal
            Button btnCancel = new Button()
            {
                Text = "Batal",
                Location = new Point(430, 585),
                Width = 110,
                Height = 35
            };
            btnCancel.Click += (s, ev) => importForm.Close();
            importForm.Controls.Add(btnCancel);

            importForm.ShowDialog();
        }


        private void CreateTemplateExcel(string path)
        {
            _productService ??= new ProductService(new ProductRepository());

            using var workbook = new ClosedXML.Excel.XLWorkbook();

            var wsItems = workbook.Worksheets.Add("Items");
            wsItems.Cell(1, 1).Value = "name";
            wsItems.Cell(1, 2).Value = "buy_price";
            wsItems.Cell(1, 3).Value = "sell_price";
            wsItems.Cell(1, 4).Value = "barcode";
            wsItems.Cell(1, 5).Value = "unit";
            wsItems.Cell(1, 6).Value = "group";
            wsItems.Cell(1, 7).Value = "supplier_id";
            wsItems.Cell(1, 8).Value = "note";
            wsItems.Cell(1, 9).Value = "is_inventory_p";
            wsItems.Cell(1, 10).Value = "is_changeprice_p";
            wsItems.Row(1).Style.Font.Bold = true;

            wsItems.Cell(3, 1).Value = "⚠️ Petunjuk:";
            wsItems.Cell(4, 1).Value = "1. Isi master produk di sheet Items.";
            wsItems.Cell(5, 1).Value = "2. Isi stok per gudang di sheet Stocks (berdasarkan barcode).";
            wsItems.Cell(6, 1).Value = "3. Gunakan 'Y' atau 'N' untuk kolom boolean (is_inventory_p, is_changeprice_p).";
            wsItems.Cell(7, 1).Value = "4. Kolom unit/group/supplier_id gunakan ID sesuai sheet Referensi.";

            wsItems.Cell(2, 1).Value = "Aqua Gelas";
            wsItems.Cell(2, 2).Value = 500;
            wsItems.Cell(2, 3).Value = 1500;
            wsItems.Cell(2, 4).Value = "AG-01";
            wsItems.Cell(2, 5).Value = 5;
            wsItems.Cell(2, 6).Value = 2;
            wsItems.Cell(2, 7).Value = 3;
            wsItems.Cell(2, 8).Value = "";
            wsItems.Cell(2, 9).Value = "Y";
            wsItems.Cell(2, 10).Value = "N";
            wsItems.Columns().AdjustToContents();

            var wsStocks = workbook.Worksheets.Add("Stocks");
            wsStocks.Cell(1, 1).Value = "barcode";
            wsStocks.Cell(1, 2).Value = "warehouse_id";
            wsStocks.Cell(1, 3).Value = "warehouse_name";
            wsStocks.Cell(1, 4).Value = "qty";
            wsStocks.Cell(1, 5).Value = "min_qty";
            wsStocks.Cell(1, 6).Value = "reserved_qty";
            wsStocks.Row(1).Style.Font.Bold = true;

            wsStocks.Cell(2, 1).Value = "AG-01";
            wsStocks.Cell(2, 2).Value = 1;
            wsStocks.Cell(2, 3).Value = "Main Store";
            wsStocks.Cell(2, 4).Value = 5;
            wsStocks.Cell(2, 5).Value = 1;
            wsStocks.Cell(2, 6).Value = 0;

            wsStocks.Cell(3, 1).Value = "AG-01";
            wsStocks.Cell(3, 2).Value = 2;
            wsStocks.Cell(3, 3).Value = "Gudang 1";
            wsStocks.Cell(3, 4).Value = 3;
            wsStocks.Cell(3, 5).Value = 1;
            wsStocks.Cell(3, 6).Value = 0;
            wsStocks.Columns().AdjustToContents();

            CreateReferenceSheet(workbook);
            workbook.SaveAs(path);
        }


        private void ImportItemsFromExcel(string filePath)
        {
            //using (var workbook = new ClosedXML.Excel.XLWorkbook(filePath))
            //{
            //    var ws = workbook.Worksheet(1);
            //    var rows = ws.RangeUsed().RowsUsed().Skip(1); // skip header

            //    using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            //    {
            //        con.Open();
            //        foreach (var row in rows)
            //        {
            //            var item = new Item
            //            {
            //                name = row.Cell(1).GetString(),
            //                buy_price = (decimal)row.Cell(2).GetDouble(),
            //                sell_price = (decimal)row.Cell(3).GetDouble(),
            //                barcode = row.Cell(4).GetString(),
            //                stock = (float)row.Cell(5).GetDouble(),
            //                unitid = (int)row.Cell(6).GetDouble(),
            //                group = (int)row.Cell(7).GetDouble(),
            //                supplier_id = (int)row.Cell(8).GetDouble(),
            //                note = row.Cell(9).GetString(),
            //                is_inventory_p = row.Cell(10).GetString(),
            //                is_changeprice_p = row.Cell(11).GetString(),
            //                created_at = DateTime.Now,
            //                updated_at = DateTime.Now
            //            };

            //            InsertItem(item); // panggil fungsi insert yang kamu sudah buat
            //        }
            //    }
            //}
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            using (var form = new ItemDetailForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadItems(); // hanya reload setelah simpan
                }
            }
        }

        private void btnStockAdjs_Click(object sender, EventArgs e)
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Perbaikan Stok (IN)", null, (_, __) => OpenInventoryAdjustmentEntry(InventoryAdjustmentDirection.In));
            menu.Items.Add("Perbaikan Stok (OUT)", null, (_, __) => OpenInventoryAdjustmentEntry(InventoryAdjustmentDirection.Out));
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Stock Opname", null, (_, __) => OpenStockOpname());
            menu.Items.Add("Daftar Stock Opname", null, (_, __) => OpenStockOpnameList());
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Daftar Item Masuk", null, (_, __) => OpenInventoryAdjustmentList(InventoryAdjustmentDirection.In));
            menu.Items.Add("Daftar Item Keluar", null, (_, __) => OpenInventoryAdjustmentList(InventoryAdjustmentDirection.Out));

            var btn = sender as Control;
            var pt = btn != null ? new Point(0, btn.Height) : new Point(Cursor.Position.X, Cursor.Position.Y);
            menu.Show(btn ?? this, pt);
        }

        private void OpenInventoryAdjustmentEntry(InventoryAdjustmentDirection direction)
        {
            if (!EnsureWarehouseSelectedForStockAction())
                return;

            var itemIds = GetSelectedItemIdsForStockAction();
            int warehouseId = GetSelectedWarehouseIdForStockAction();
            using var f = new InventoryAdjustmentEntryForm(direction, itemIds.Count > 0 ? itemIds : null, warehouseId > 0 ? warehouseId : null);
            if (f.ShowDialog(this) == DialogResult.OK)
                LoadItems();
        }

        private void OpenInventoryAdjustmentList(InventoryAdjustmentDirection direction)
        {
            if (!EnsureWarehouseSelectedForStockAction())
                return;

            int warehouseId = GetSelectedWarehouseIdForStockAction();
            using var f = new InventoryAdjustmentListForm(direction, warehouseId);
            f.ShowDialog(this);
        }

        private void OpenStockOpname()
        {
            if (!EnsureWarehouseSelectedForStockAction())
                return;

            int warehouseId = GetSelectedWarehouseIdForStockAction();
            using var f = new StockOpnameForm(warehouseId);
            f.ShowDialog(this);
            LoadItems();
        }

        private void OpenStockOpnameList()
        {
            if (!EnsureWarehouseSelectedForStockAction())
                return;

            int warehouseId = GetSelectedWarehouseIdForStockAction();
            using var f = new StockOpnameListForm(warehouseId);
            f.ShowDialog(this);
        }

        private List<int> GetSelectedItemIdsForStockAction()
        {
            var ids = new List<int>();

            if (dataGridView1.Columns.Contains("chkSelect"))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    try
                    {
                        var v = row.Cells["chkSelect"]?.Value;
                        if (v is bool b && b)
                        {
                            if (row.Cells["id"]?.Value != null && int.TryParse(row.Cells["id"].Value.ToString(), out var id) && id > 0)
                                ids.Add(id);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            if (ids.Count == 0 && dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                if (dataGridView1.CurrentRow.Cells["id"]?.Value != null &&
                    int.TryParse(dataGridView1.CurrentRow.Cells["id"].Value.ToString(), out var id) &&
                    id > 0)
                {
                    ids.Add(id);
                }
            }

            return ids;
        }

        private int GetSelectedWarehouseIdForStockAction()
        {
            try
            {
                if (cmbWarehouseFilter?.SelectedValue != null && int.TryParse(cmbWarehouseFilter.SelectedValue.ToString(), out var id) && id > 0)
                    return id;
            }
            catch
            {
            }
            return 0;
        }

        private bool EnsureWarehouseSelectedForStockAction()
        {
            int whId = GetSelectedWarehouseIdForStockAction();
            if (whId > 0) return true;

            MessageBox.Show("Pilih gudang dulu (filter Gudang) sebelum menjalankan menu Stock/Opname.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                if (cmbWarehouseFilter != null)
                {
                    cmbWarehouseFilter.Focus();
                    cmbWarehouseFilter.DroppedDown = true;
                }
            }
            catch
            {
            }
            return false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadItems();
        }

      



        //private void ImportItemsFromExcel(string filePath)
        //{
        //    using (var workbook = new ClosedXML.Excel.XLWorkbook(filePath))
        //    {
        //        var ws = workbook.Worksheet(1);
        //        var rows = ws.RangeUsed().RowsUsed().Skip(1); // skip header

        //        using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
        //        {
        //            con.Open();
        //            foreach (var row in rows)
        //            {
        //                var item = new Item
        //                {
        //                    name = row.Cell(1).GetString(),
        //                    buy_price = (decimal)row.Cell(2).GetDouble(),
        //                    sell_price = (decimal)row.Cell(3).GetDouble(),
        //                    barcode = row.Cell(4).GetString(),
        //                    stock = (float)row.Cell(5).GetDouble(),
        //                    unitid = (int)row.Cell(6).GetDouble(),
        //                    group = (int)row.Cell(7).GetDouble(),
        //                    supplier_id = (int)row.Cell(8).GetDouble(),
        //                    note = row.Cell(9).GetString(),
        //                    is_inventory_p = row.Cell(10).GetString(),
        //                    is_changeprice_p = row.Cell(11).GetString(),
        //                    created_at = DateTime.Now,
        //                    updated_at = DateTime.Now
        //                };

        //                InsertItem(item); // panggil fungsi insert yang kamu sudah buat
        //            }
        //        }
        //    }
        //}



        private void ApplyProfessionalGridStyle()
        {
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersHeight = 64;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.RowTemplate.Height = 45;
            
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            dataGridView1.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            dataGridView1.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(252, 252, 252);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.GridColor = System.Drawing.Color.FromArgb(235, 235, 235);
            
            dataGridView1.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(240, 248, 255);
            dataGridView1.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
        }

        private void UpdateSummaryFromGrid()
        {
            try
            {
                int rows = dataGridView1.Rows.Count;
                decimal sumQty = 0m;
                decimal sumStockValue = 0m, sumRetailValue = 0m;
                int invCount = 0, nonInvCount = 0;

                if (_viewAllUnits)
                {
                    var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        if (r.IsNewRow) continue;
                        if (r.Cells["id"]?.Value == null) continue;
                        int id = Convert.ToInt32(r.Cells["id"].Value);
                        int wid = 0;
                        if (r.Cells["warehouse_id"]?.Value != null)
                            int.TryParse(r.Cells["warehouse_id"].Value.ToString(), out wid);
                        var key = $"{id}:{wid}";
                        if (!seen.Add(key)) continue;

                        decimal baseStock = 0m;
                        if (r.Cells["base_stock"]?.Value != null && decimal.TryParse(r.Cells["base_stock"].Value.ToString(), out var bs)) baseStock = bs;
                        decimal baseBuy = 0m;
                        if (r.Cells["base_buy_price"]?.Value != null && decimal.TryParse(r.Cells["base_buy_price"].Value.ToString(), out var bb)) baseBuy = bb;
                        decimal baseSell = 0m;
                        if (r.Cells["sell_price"]?.Value != null && decimal.TryParse(r.Cells["sell_price"].Value.ToString(), out var sp)) baseSell = sp;

                        sumQty += baseStock;
                        sumStockValue += baseBuy * baseStock;
                        sumRetailValue += baseSell * baseStock;

                        bool inv = false;
                        if (r.Cells["is_inventory_p"]?.Value != null) bool.TryParse(r.Cells["is_inventory_p"].Value.ToString(), out inv);
                        if (inv) invCount++; else nonInvCount++;
                    }
                }
                else
                {
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        if (r.IsNewRow) continue;
                        decimal q = 0m;
                        if (r.Cells["stock"]?.Value != null && decimal.TryParse(r.Cells["stock"].Value.ToString(), out var qtmp)) q = qtmp;
                        sumQty += q;
                        if (r.Cells["stock_value"]?.Value != null && decimal.TryParse(r.Cells["stock_value"].Value.ToString(), out var sv)) sumStockValue += sv;
                        if (r.Cells["retail_value"]?.Value != null && decimal.TryParse(r.Cells["retail_value"].Value.ToString(), out var rv)) sumRetailValue += rv;
                        bool inv = false;
                        if (r.Cells["is_inventory_p"]?.Value != null) bool.TryParse(r.Cells["is_inventory_p"].Value.ToString(), out inv);
                        if (inv) invCount++; else nonInvCount++;
                    }
                }
                // Gunakan label dari designer
                int selectedWarehouseId = GetSelectedWarehouseId();
                var now = DateTime.Now;
                var systemStart = new DateTime(1970, 1, 1);
                decimal pembelianTotal = GetPurchasesValue(systemStart, now, selectedWarehouseId);
                decimal persediaanValue = GetInventoryValueNowFromLayers(selectedWarehouseId, fallback: sumStockValue);
                decimal hppTotalSistem = pembelianTotal - persediaanValue;

                label9.Text =
                    $"Persediaan (Stock Layer): {persediaanValue:N0} | Pembelian Total: {pembelianTotal:N0} | HPP (Total Sistem): {hppTotalSistem:N0} | Nilai Jual: {sumRetailValue:N0}";
                label10.Text = _viewAllUnits
                    ? $"Mode: All Unit | Stock (Base): {sumQty:N0} | Total Item: {invCount + nonInvCount:N0} (Inv:{invCount:N0}/Non:{nonInvCount:N0})"
                    : $"Jumlah Stock: {sumQty:N0} | Total Item: {rows:N0} (Inv:{invCount:N0}/Non:{nonInvCount:N0})";
                int total = _dtFull?.Rows.Count ?? rows;
                lblPagingInfo.Text = $"Menampilkan {rows:N0} dari {total:N0}";
            }
            catch
            {
                // ignore summary errors
            }
        }

        private decimal GetInventoryValueAt(DateTime asOf, int warehouseId)
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                bool hasStockLayerId = ColumnExists(conn, "stock_log", "stock_layer_id");
                bool hasIsAllocation = ColumnExists(conn, "stock_log", "is_allocation");
                bool hasCreatedAt = ColumnExists(conn, "stock_layers", "created_at");

                if (!hasCreatedAt)
                    return 0m;

                string whFilter = warehouseId > 0 ? "AND sl.warehouse_id = @wh" : "";
                if (hasStockLayerId && hasIsAllocation)
                {
                    using var cmd = new NpgsqlCommand($@"
SELECT COALESCE(SUM( (sl.qty_remaining::numeric + COALESCE(a.qty_after,0)) * sl.buy_price ),0)
FROM stock_layers sl
LEFT JOIN (
    SELECT stock_layer_id, COALESCE(SUM(qty_keluar),0) AS qty_after
    FROM stock_log
    WHERE is_allocation = TRUE
      AND stock_layer_id IS NOT NULL
      AND created_at > @asof
    GROUP BY stock_layer_id
) a ON a.stock_layer_id = sl.id
WHERE sl.created_at <= @asof
{whFilter}
", conn);
                    cmd.Parameters.AddWithValue("@asof", asOf);
                    if (warehouseId > 0) cmd.Parameters.AddWithValue("@wh", warehouseId);
                    var obj = cmd.ExecuteScalar();
                    return obj == null || obj == DBNull.Value ? 0m : Convert.ToDecimal(obj);
                }
                else
                {
                    using var cmd = new NpgsqlCommand($@"
SELECT COALESCE(SUM(sl.qty_remaining::numeric * sl.buy_price),0)
FROM stock_layers sl
WHERE sl.created_at <= @asof
{whFilter}
", conn);
                    cmd.Parameters.AddWithValue("@asof", asOf);
                    if (warehouseId > 0) cmd.Parameters.AddWithValue("@wh", warehouseId);
                    var obj = cmd.ExecuteScalar();
                    return obj == null || obj == DBNull.Value ? 0m : Convert.ToDecimal(obj);
                }
            }
            catch
            {
                return 0m;
            }
        }

        private decimal GetPurchasesValue(DateTime startInclusive, DateTime endExclusive, int warehouseId)
        {
            decimal poTotal = 0m;
            decimal adjInTotal = 0m;

            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                bool poHasWarehouse = ColumnExists(conn, "purchase_orders", "warehouse_id");
                string whFilterPo = warehouseId > 0 && poHasWarehouse ? "AND po.warehouse_id = @wh" : "";

                using (var cmd = new NpgsqlCommand($@"
SELECT COALESCE(SUM(poi.quantity * poi.unit_price),0)
FROM purchase_order_items poi
JOIN purchase_orders po ON po.id = poi.po_id
WHERE po.order_date >= @start::date
  AND po.order_date < @end::date
  AND COALESCE(po.status,'') ILIKE 'RECEIV%'
{whFilterPo}
", conn))
                {
                    cmd.Parameters.AddWithValue("@start", startInclusive);
                    cmd.Parameters.AddWithValue("@end", endExclusive);
                    if (warehouseId > 0 && poHasWarehouse) cmd.Parameters.AddWithValue("@wh", warehouseId);
                    var obj = cmd.ExecuteScalar();
                    poTotal = obj == null || obj == DBNull.Value ? 0m : Convert.ToDecimal(obj);
                }

                string whFilterAdj = warehouseId > 0 ? "AND ia.warehouse_id = @wh" : "";
                using (var cmd = new NpgsqlCommand($@"
SELECT COALESCE(SUM(ii.qty * COALESCE(ii.buy_price,0)),0)
FROM inventory_adjustment_items ii
JOIN inventory_adjustments ia ON ia.id = ii.adjustment_id
WHERE ia.direction = 'IN'
  AND ia.adjustment_date >= @start::date
  AND ia.adjustment_date < @end::date
{whFilterAdj}
", conn))
                {
                    cmd.Parameters.AddWithValue("@start", startInclusive);
                    cmd.Parameters.AddWithValue("@end", endExclusive);
                    if (warehouseId > 0) cmd.Parameters.AddWithValue("@wh", warehouseId);
                    var obj = cmd.ExecuteScalar();
                    adjInTotal = obj == null || obj == DBNull.Value ? 0m : Convert.ToDecimal(obj);
                }
            }
            catch
            {
                return 0m;
            }

            return poTotal + adjInTotal;
        }

        private decimal GetInventoryValueNowFromLayers(int warehouseId, decimal fallback)
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                if (!ColumnExists(conn, "stock_layers", "qty_remaining") || !ColumnExists(conn, "stock_layers", "buy_price"))
                    return fallback;

                string whFilter = warehouseId > 0 ? "AND warehouse_id = @wh" : "";

                using var cmd = new NpgsqlCommand($@"
SELECT COALESCE(SUM(COALESCE(qty_remaining,0)::numeric * COALESCE(buy_price,0)),0)
FROM stock_layers
WHERE COALESCE(qty_remaining,0) > 0
{whFilter}
", conn);
                if (warehouseId > 0) cmd.Parameters.AddWithValue("@wh", warehouseId);
                var obj = cmd.ExecuteScalar();
                return obj == null || obj == DBNull.Value ? 0m : Convert.ToDecimal(obj);
            }
            catch
            {
                return fallback;
            }
        }

        private bool ColumnExists(NpgsqlConnection conn, string tableName, string columnName)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.columns
WHERE table_schema='public'
  AND table_name = @t
  AND column_name = @c
LIMIT 1
", conn);
            cmd.Parameters.AddWithValue("@t", tableName);
            cmd.Parameters.AddWithValue("@c", columnName);
            return cmd.ExecuteScalar() != null;
        }

        private void ConfigureGridColumns()
        {
            if (!dataGridView1.Columns.Contains("btnHppHistory"))
            {
                var col = new DataGridViewButtonColumn
                {
                    Name = "btnHppHistory",
                    HeaderText = "HPP",
                    Text = "History",
                    UseColumnTextForButtonValue = true,
                    Width = 90,
                    FlatStyle = FlatStyle.Flat
                };

                int insertIndex = 0;
                if (dataGridView1.Columns.Contains("chkSelect"))
                    insertIndex = Math.Min(dataGridView1.Columns["chkSelect"].Index + 1, dataGridView1.Columns.Count);

                dataGridView1.Columns.Insert(insertIndex, col);
            }

            if (dataGridView1.Columns.Contains("name"))
            {
                var c = dataGridView1.Columns["name"];
                c.HeaderText = "Nama Barang";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 320;
            }
            if (dataGridView1.Columns.Contains("barcode"))
            {
                var c = dataGridView1.Columns["barcode"];
                c.HeaderText = "Barcode";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 130;
            }
            if (dataGridView1.Columns.Contains("unit_name"))
            {
                var c = dataGridView1.Columns["unit_name"];
                c.HeaderText = "Satuan";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 90;
            }
            if (dataGridView1.Columns.Contains("stock"))
            {
                var c = dataGridView1.Columns["stock"];
                c.HeaderText = "Stok";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N2";
                c.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 90;
            }
            if (dataGridView1.Columns.Contains("warehouse_name"))
            {
                var c = dataGridView1.Columns["warehouse_name"];
                c.HeaderText = "Gudang";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 110;
                c.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            if (dataGridView1.Columns.Contains("sell_price"))
            {
                var c = dataGridView1.Columns["sell_price"];
                c.HeaderText = "Harga Jual";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N2";
                c.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 110;
            }
            if (dataGridView1.Columns.Contains("buy_price"))
            {
                var c = dataGridView1.Columns["buy_price"];
                c.HeaderText = "Harga Beli\nTerakhir";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N2";
                c.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 120;
            }
      
            if (dataGridView1.Columns.Contains("min_threshold"))
            {
                var c = dataGridView1.Columns["min_threshold"];
                c.HeaderText = "Min\nQty";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N2";
                c.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 80;
                c.Visible = true;
                if (dataGridView1.Columns.Contains("stock"))
                    c.DisplayIndex = Math.Min(dataGridView1.Columns["stock"].DisplayIndex + 1, dataGridView1.Columns.Count - 1);
            }
            if (dataGridView1.Columns.Contains("stock_value"))
            {
                var c = dataGridView1.Columns["stock_value"];
                c.HeaderText = "Nilai Stok\n(HPP)";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N2";
                c.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 140;
            }
            if (dataGridView1.Columns.Contains("retail_value"))
            {
                var c = dataGridView1.Columns["retail_value"];
                c.HeaderText = "Nilai Jual";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N2";
                c.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                c.MinimumWidth = 120;
            }

            string[] hideCols = new[]
            {
                "reserved_stock","unit_id","category_id","supplier_id","note","picture","is_purchasable","is_sellable",
                "is_note_payment","is_changeprice_p","is_have_bahan","is_box","is_produksi","discount_formula","flag",
                "created_at","updated_at","category_name","supplier_name","btnVariant",
                "min_qty","min_stock","conversion","is_variant","base_stock","base_buy_price","warehouse_id"
            };
            foreach (var key in hideCols)
            {
                if (dataGridView1.Columns.Contains(key))
                    dataGridView1.Columns[key].Visible = false;
            }
        }

        private void OpenEditForRow(DataGridViewRow rowSelected)
        {
            int itemId = Convert.ToInt32(rowSelected.Cells["id"].Value);
            int warehouseId = 0;
            if (rowSelected.Cells["warehouse_id"]?.Value != null)
                int.TryParse(rowSelected.Cells["warehouse_id"].Value.ToString(), out warehouseId);

            if (warehouseId <= 0 && ItemHasMultipleWarehouses(itemId))
            {
                var r = MessageBox.Show(
                    "Item ini punya stok di lebih dari 1 gudang.\n\n" +
                    "Baris yang Anda klik sedang mode SEMUA GUDANG (stok total).\n\n" +
                    "Klik Ok untuk pilih gudang dulu agar stok yang tampil sesuai gudang.",
                    "Info Gudang",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                if (r == DialogResult.OK)
                {
                    if (cmbWarehouseFilter != null)
                    {
                        cmbWarehouseFilter.Focus();
                        cmbWarehouseFilter.DroppedDown = true;
                    }
                    return;
                }
            }

            int stock = 0;
            if (rowSelected.Cells["stock"]?.Value != null)
                int.TryParse(rowSelected.Cells["stock"].Value.ToString(), out stock);

            int minQty = 0;
            if (rowSelected.Cells["min_qty"]?.Value != null)
                int.TryParse(rowSelected.Cells["min_qty"].Value.ToString(), out minQty);

            var itemForEdit = new Item
            {
                id = itemId,
                name = rowSelected.Cells["name"]?.Value?.ToString() ?? "",
                barcode = rowSelected.Cells["barcode"]?.Value?.ToString(),
                buy_price = rowSelected.Cells["buy_price"]?.Value != null && decimal.TryParse(rowSelected.Cells["buy_price"].Value.ToString(), out var bp) ? bp : 0m,
                sell_price = rowSelected.Cells["sell_price"]?.Value != null && decimal.TryParse(rowSelected.Cells["sell_price"].Value.ToString(), out var sp) ? sp : 0m,
                stock = stock,
                min_qty = minQty,
                initial_warehouse_id = warehouseId > 0 ? warehouseId : null
            };

            using (var detailForm = new ItemDetailForm(itemForEdit))
            {
                if (detailForm.ShowDialog() == DialogResult.OK)
                    LoadItems();
            }
        }

        private static bool ItemHasMultipleWarehouses(int itemId)
        {
            if (itemId <= 0) return false;

            try
            {
                using var con = new NpgsqlConnection(DbConfig.ConnectionString);
                con.Open();
                using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM stocks WHERE item_id = @id", con);
                cmd.Parameters.AddWithValue("@id", itemId);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 1;
            }
            catch
            {
                return false;
            }
        }

        private void CreateReferenceSheet(ClosedXML.Excel.XLWorkbook workbook)
        {
            _productService ??= new ProductService(new ProductRepository());

            ClosedXML.Excel.IXLWorksheet ws;
            if (workbook.Worksheets.Any(x => string.Equals(x.Name, "Referensi", StringComparison.OrdinalIgnoreCase)))
                ws = workbook.Worksheet("Referensi");
            else
                ws = workbook.Worksheets.Add("Referensi");

            ws.Clear();

            int r = 1;

            ws.Cell(r, 1).Value = "Referensi Gudang";
            ws.Cell(r, 1).Style.Font.Bold = true;
            r++;
            ws.Cell(r, 1).Value = "ID";
            ws.Cell(r, 2).Value = "Name";
            ws.Row(r).Style.Font.Bold = true;
            r++;
            try
            {
                var wc = new WarehouseController();
                var dtWh = wc.GetWarehouses();
                if (dtWh != null)
                {
                    foreach (DataRow dr in dtWh.Rows)
                    {
                        bool isActive = !dtWh.Columns.Contains("is_active") || (dr["is_active"] != DBNull.Value && Convert.ToBoolean(dr["is_active"]));
                        if (!isActive) continue;
                        ws.Cell(r, 1).Value = dr["id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["id"]);
                        ws.Cell(r, 2).Value = dr["name"]?.ToString() ?? "";
                        r++;
                    }
                }
            }
            catch
            {
            }

            r += 2;

            ws.Cell(r, 1).Value = "Referensi Unit";
            ws.Cell(r, 1).Style.Font.Bold = true;
            r++;
            ws.Cell(r, 1).Value = "ID";
            ws.Cell(r, 2).Value = "Name";
            ws.Row(r).Style.Font.Bold = true;
            r++;
            try
            {
                var dtUnits = _productService.GetUnits();
                if (dtUnits != null)
                {
                    foreach (DataRow dr in dtUnits.Rows)
                    {
                        ws.Cell(r, 1).Value = dr["id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["id"]);
                        ws.Cell(r, 2).Value = dr.Table.Columns.Contains("display") ? (dr["display"]?.ToString() ?? "") : (dr["name"]?.ToString() ?? "");
                        r++;
                    }
                }
            }
            catch
            {
            }

            r += 2;

            ws.Cell(r, 1).Value = "Referensi Group (Kategori)";
            ws.Cell(r, 1).Style.Font.Bold = true;
            r++;
            ws.Cell(r, 1).Value = "ID";
            ws.Cell(r, 2).Value = "Name";
            ws.Row(r).Style.Font.Bold = true;
            r++;
            try
            {
                var dtCats = _productService.GetCategories();
                if (dtCats != null)
                {
                    foreach (DataRow dr in dtCats.Rows)
                    {
                        ws.Cell(r, 1).Value = dr["id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["id"]);
                        ws.Cell(r, 2).Value = dr.Table.Columns.Contains("display") ? (dr["display"]?.ToString() ?? "") : (dr["name"]?.ToString() ?? "");
                        r++;
                    }
                }
            }
            catch
            {
            }

            r += 2;

            ws.Cell(r, 1).Value = "Referensi Supplier";
            ws.Cell(r, 1).Style.Font.Bold = true;
            r++;
            ws.Cell(r, 1).Value = "ID";
            ws.Cell(r, 2).Value = "Name";
            ws.Row(r).Style.Font.Bold = true;
            r++;
            try
            {
                var dtSup = _productService.GetSuppliers();
                if (dtSup != null)
                {
                    foreach (DataRow dr in dtSup.Rows)
                    {
                        ws.Cell(r, 1).Value = dr["id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["id"]);
                        ws.Cell(r, 2).Value = dr.Table.Columns.Contains("display") ? (dr["display"]?.ToString() ?? "") : (dr["name"]?.ToString() ?? "");
                        r++;
                    }
                }
            }
            catch
            {
            }

            ws.Columns().AdjustToContents();
            ws.Rows().Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
        }

        private static Dictionary<string, int> BuildHeaderMap(ClosedXML.Excel.IXLWorksheet ws)
        {
            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var firstRow = ws.Row(1);
            var lastCol = firstRow.LastCellUsed()?.Address.ColumnNumber ?? 0;
            for (int c = 1; c <= lastCol; c++)
            {
                var v = firstRow.Cell(c).GetString()?.Trim();
                if (string.IsNullOrWhiteSpace(v)) continue;
                if (!map.ContainsKey(v)) map[v] = c;
            }
            return map;
        }

        private static string GetCellString(ClosedXML.Excel.IXLRangeRow row, Dictionary<string, int> map, string key)
        {
            if (!map.TryGetValue(key, out var idx)) return "";
            return row.Cell(idx).GetString()?.Trim() ?? "";
        }

        private static decimal GetCellDecimal(ClosedXML.Excel.IXLRangeRow row, Dictionary<string, int> map, string key)
        {
            if (!map.TryGetValue(key, out var idx)) return 0m;
            var cell = row.Cell(idx);
            if (cell.TryGetValue<double>(out var d)) return (decimal)d;
            if (decimal.TryParse(cell.GetString(), out var x)) return x;
            return 0m;
        }

        private static int GetCellInt(ClosedXML.Excel.IXLRangeRow row, Dictionary<string, int> map, string key)
        {
            if (!map.TryGetValue(key, out var idx)) return 0;
            var cell = row.Cell(idx);
            if (cell.TryGetValue<int>(out var i)) return i;
            if (cell.TryGetValue<double>(out var d)) return Convert.ToInt32(d);
            if (int.TryParse(cell.GetString(), out var x)) return x;
            return 0;
        }

        private sealed class ImportResult
        {
            public int Inserted { get; set; }
            public int Updated { get; set; }
            public int ItemFailed { get; set; }
            public int StockOk { get; set; }
            public int StockFailed { get; set; }
            public int StockResetItems { get; set; }
            public List<string> Errors { get; } = new List<string>();
        }

        private static void InsertStockLogNoTran(NpgsqlConnection con, StockLog stockLog)
        {
            const string query = @"
INSERT INTO stock_log
(product_id, tipe_transaksi, qty_masuk, qty_keluar,
 sisa_stock, keterangan, user_id, created_at, login_id,
 warehouse_id, ref_type, ref_id, supplier_id, unit_cost, method,
 stock_layer_id, is_allocation, parent_id)
VALUES
(@product_id, @tipe_transaksi, @qty_masuk, @qty_keluar,
 @sisa_stock, @keterangan, @user_id, @created_at, @login_id,
 @warehouse_id, @ref_type, @ref_id, @supplier_id, @unit_cost, @method,
 @stock_layer_id, @is_allocation, @parent_id)
";

            using var cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@product_id", stockLog.ProductId);
            cmd.Parameters.AddWithValue("@tipe_transaksi", stockLog.TipeTransaksi ?? "");
            cmd.Parameters.AddWithValue("@qty_masuk", stockLog.QtyMasuk);
            cmd.Parameters.AddWithValue("@qty_keluar", stockLog.QtyKeluar);
            cmd.Parameters.AddWithValue("@sisa_stock", stockLog.SisaStock);
            cmd.Parameters.AddWithValue("@keterangan", stockLog.Keterangan ?? "");
            cmd.Parameters.AddWithValue("@user_id", stockLog.UserId);
            cmd.Parameters.AddWithValue("@created_at", stockLog.CreatedAt);
            cmd.Parameters.AddWithValue("@login_id",
                stockLog.LoginId.HasValue ? (object)stockLog.LoginId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@warehouse_id", stockLog.WarehouseId > 0 ? stockLog.WarehouseId : 1);
            cmd.Parameters.AddWithValue("@ref_type", string.IsNullOrWhiteSpace(stockLog.RefType) ? (object)DBNull.Value : stockLog.RefType.Trim());
            cmd.Parameters.AddWithValue("@ref_id", stockLog.RefId.HasValue ? (object)stockLog.RefId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@supplier_id", stockLog.SupplierId.HasValue ? (object)stockLog.SupplierId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@unit_cost", stockLog.UnitCost.HasValue ? (object)stockLog.UnitCost.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@method", string.IsNullOrWhiteSpace(stockLog.Method) ? (object)DBNull.Value : stockLog.Method.Trim().ToUpperInvariant());
            cmd.Parameters.AddWithValue("@stock_layer_id", stockLog.StockLayerId.HasValue ? (object)stockLog.StockLayerId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@is_allocation", stockLog.IsAllocation);
            cmd.Parameters.AddWithValue("@parent_id", stockLog.ParentId.HasValue ? (object)stockLog.ParentId.Value : DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        private static string FormatDbException(Exception ex)
        {
            try
            {
                if (ex is Npgsql.PostgresException pex)
                {
                    var detail = string.IsNullOrWhiteSpace(pex.Detail) ? "" : $" | Detail: {pex.Detail}";
                    var where = string.IsNullOrWhiteSpace(pex.Where) ? "" : $" | Where: {pex.Where}";
                    var constraint = string.IsNullOrWhiteSpace(pex.ConstraintName) ? "" : $" | Constraint: {pex.ConstraintName}";
                    return $"{pex.SqlState}: {pex.MessageText}{detail}{constraint}{where}";
                }
            }
            catch
            {
            }
            return ex.Message;
        }

        private ImportResult ImportItemsFromExcelImpl(string filePath, Action<string> log, Action<string> error, Action<int, int> progress)
        {
            var result = new ImportResult();
            _productService ??= new ProductService(new ProductRepository());

            void Log(string msg)
            {
                try { log?.Invoke(msg); } catch { }
            }

            void Err(string msg)
            {
                result.Errors.Add(msg);
                try { error?.Invoke(msg); } catch { }
            }

            ClosedXML.Excel.XLWorkbook workbook;
            try
            {
                workbook = new ClosedXML.Excel.XLWorkbook(filePath);
            }
            catch (Exception ex)
            {
                Err("Gagal membuka file: " + ex.Message);
                return result;
            }

            var wsItems = workbook.Worksheets.FirstOrDefault(w => string.Equals(w.Name, "Items", StringComparison.OrdinalIgnoreCase))
                          ?? workbook.Worksheets.FirstOrDefault(w => string.Equals(w.Name, "Template", StringComparison.OrdinalIgnoreCase))
                          ?? workbook.Worksheets.First();

            var usedItems = wsItems.RangeUsed();
            if (usedItems == null)
            {
                Err("Sheet Items kosong.");
                return result;
            }

            var itemHeader = BuildHeaderMap(wsItems);
            if (!itemHeader.ContainsKey("name") || !itemHeader.ContainsKey("buy_price") || !itemHeader.ContainsKey("sell_price"))
            {
                Err("Format sheet Items tidak valid. Gunakan template terbaru.");
                return result;
            }

            var wsStocks = workbook.Worksheets.FirstOrDefault(w => string.Equals(w.Name, "Stocks", StringComparison.OrdinalIgnoreCase));
            Dictionary<string, int> stockHeader = null;
            ClosedXML.Excel.IXLRange usedStocks = null;
            if (wsStocks != null)
            {
                usedStocks = wsStocks.RangeUsed();
                if (usedStocks != null)
                    stockHeader = BuildHeaderMap(wsStocks);
            }

            int totalItems = Math.Max(0, usedItems.RowsUsed().Count() - 1);
            int totalStocks = usedStocks != null ? Math.Max(0, usedStocks.RowsUsed().Count() - 1) : 0;
            int total = totalItems + totalStocks;
            int done = 0;
            progress?.Invoke(0, Math.Max(1, total));

            Log("Mulai import (multi gudang)...");
            Log($"Items rows: {totalItems:N0}");
            Log($"Stocks rows: {totalStocks:N0}");
            Log("Catatan: Import TIDAK menghapus produk lain. Import hanya insert/update yang ada di sheet Items.");
            Log("Catatan: Jika sheet Stocks diisi, stok untuk barcode yang ada di sheet Stocks akan DI-RESET sesuai Excel.");

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();

            var warehouseIdByName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var warehouseIds = new HashSet<int>();
            int defaultWarehouseId = 0;
            var stockLogsToInsert = new List<StockLog>();
            try
            {
                using var cmdWh = new NpgsqlCommand("SELECT id, name FROM warehouses WHERE is_active = TRUE ORDER BY id ASC", con, tran);
                using var drWh = cmdWh.ExecuteReader();
                while (drWh.Read())
                {
                    int id = drWh.IsDBNull(0) ? 0 : drWh.GetInt32(0);
                    string name = drWh.IsDBNull(1) ? "" : drWh.GetString(1);
                    if (id > 0 && !string.IsNullOrWhiteSpace(name))
                        warehouseIdByName[name.Trim()] = id;
                    if (id > 0)
                    {
                        warehouseIds.Add(id);
                        if (defaultWarehouseId <= 0) defaultWarehouseId = id;
                    }
                }
            }
            catch
            {
            }

            var itemIdByBarcode = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var buyPriceByItemId = new Dictionary<int, decimal>();

            foreach (var row in usedItems.RowsUsed().Skip(1))
            {
                string name = GetCellString(row, itemHeader, "name");
                if (string.IsNullOrWhiteSpace(name))
                {
                    done++;
                    if (done % 25 == 0) progress?.Invoke(done, Math.Max(1, total));
                    continue;
                }

                string barcode = GetCellString(row, itemHeader, "barcode");
                decimal buyPrice = GetCellDecimal(row, itemHeader, "buy_price");
                decimal sellPrice = GetCellDecimal(row, itemHeader, "sell_price");
                int unitId = GetCellInt(row, itemHeader, "unit");
                int groupId = GetCellInt(row, itemHeader, "group");
                int supplierId = GetCellInt(row, itemHeader, "supplier_id");
                string note = GetCellString(row, itemHeader, "note");
                bool inv = ParseYN(GetCellString(row, itemHeader, "is_inventory_p"), true);
                bool chg = ParseYN(GetCellString(row, itemHeader, "is_changeprice_p"), false);

                try
                {
                    int itemId = 0;
                    if (!string.IsNullOrWhiteSpace(barcode))
                    {
                        using var find = new NpgsqlCommand("SELECT id FROM items WHERE deleted_at IS NULL AND barcode = @b LIMIT 1", con, tran);
                        find.Parameters.AddWithValue("@b", barcode);
                        var res = find.ExecuteScalar();
                        itemId = res != null && res != DBNull.Value ? Convert.ToInt32(res) : 0;
                    }
                    if (itemId <= 0)
                    {
                        using var find = new NpgsqlCommand("SELECT id FROM items WHERE deleted_at IS NULL AND name ILIKE @n ORDER BY id ASC LIMIT 1", con, tran);
                        find.Parameters.AddWithValue("@n", name);
                        var res = find.ExecuteScalar();
                        itemId = res != null && res != DBNull.Value ? Convert.ToInt32(res) : 0;
                    }

                    if (itemId > 0)
                    {
                        using var upd = new NpgsqlCommand(@"
UPDATE items SET
    barcode = @barcode,
    name = @name,
    category_id = @category_id,
    unit = @unit_id,
    supplier_id = @supplier_id,
    buy_price = @buy_price,
    sell_price = @sell_price,
    note = @note,
    is_inventory_p = @is_inventory_p,
    is_changeprice_p = @is_changeprice_p,
    is_purchasable = TRUE,
    is_sellable = TRUE,
    updated_at = NOW()
WHERE id = @id
", con, tran);
                        upd.Parameters.AddWithValue("@id", itemId);
                        upd.Parameters.AddWithValue("@barcode", (object)(string.IsNullOrWhiteSpace(barcode) ? null : barcode) ?? DBNull.Value);
                        upd.Parameters.AddWithValue("@name", name);
                        upd.Parameters.AddWithValue("@category_id", groupId > 0 ? (object)groupId : DBNull.Value);
                        upd.Parameters.AddWithValue("@unit_id", unitId > 0 ? (object)unitId : DBNull.Value);
                        upd.Parameters.AddWithValue("@supplier_id", supplierId > 0 ? (object)supplierId : DBNull.Value);
                        upd.Parameters.AddWithValue("@buy_price", buyPrice);
                        upd.Parameters.AddWithValue("@sell_price", sellPrice);
                        upd.Parameters.AddWithValue("@note", (object)(string.IsNullOrWhiteSpace(note) ? null : note) ?? DBNull.Value);
                        upd.Parameters.AddWithValue("@is_inventory_p", inv);
                        upd.Parameters.AddWithValue("@is_changeprice_p", chg);
                        upd.ExecuteNonQuery();
                        result.Updated++;
                    }
                    else
                    {
                        using var ins = new NpgsqlCommand(@"
INSERT INTO items (
    barcode, name, category_id, unit, supplier_id,
    buy_price, sell_price, valuation_method, is_active, note,
    is_inventory_p, is_purchasable, is_sellable, is_note_payment, is_changeprice_p,
    is_have_bahan, is_box, is_produksi, discount_formula, expired_at,
    created_at, updated_at
) VALUES (
    @barcode, @name, @category_id, @unit_id, @supplier_id,
    @buy_price, @sell_price, 'FIFO', TRUE, @note,
    @is_inventory_p, TRUE, TRUE, FALSE, @is_changeprice_p,
    FALSE, FALSE, FALSE, '', NULL,
    NOW(), NOW()
)
RETURNING id
", con, tran);
                        ins.Parameters.AddWithValue("@barcode", (object)(string.IsNullOrWhiteSpace(barcode) ? null : barcode) ?? DBNull.Value);
                        ins.Parameters.AddWithValue("@name", name);
                        ins.Parameters.AddWithValue("@category_id", groupId > 0 ? (object)groupId : DBNull.Value);
                        ins.Parameters.AddWithValue("@unit_id", unitId > 0 ? (object)unitId : DBNull.Value);
                        ins.Parameters.AddWithValue("@supplier_id", supplierId > 0 ? (object)supplierId : DBNull.Value);
                        ins.Parameters.AddWithValue("@buy_price", buyPrice);
                        ins.Parameters.AddWithValue("@sell_price", sellPrice);
                        ins.Parameters.AddWithValue("@note", (object)(string.IsNullOrWhiteSpace(note) ? null : note) ?? DBNull.Value);
                        ins.Parameters.AddWithValue("@is_inventory_p", inv);
                        ins.Parameters.AddWithValue("@is_changeprice_p", chg);
                        itemId = Convert.ToInt32(ins.ExecuteScalar());
                        result.Inserted++;
                    }

                    if (itemId > 0)
                    {
                        buyPriceByItemId[itemId] = buyPrice;
                        if (!string.IsNullOrWhiteSpace(barcode))
                            itemIdByBarcode[barcode] = itemId;
                    }
                }
                catch (Exception ex)
                {
                    result.ItemFailed++;
                    var rowNum = row.RowNumber();
                    Err($"Items row {rowNum}: {name} / {barcode} => {ex.Message}");
                }

                done++;
                if (done % 25 == 0) progress?.Invoke(done, Math.Max(1, total));
            }

            if (wsStocks != null && usedStocks != null && stockHeader != null)
            {
                var touchedItems = new HashSet<int>();
                var stockRows = new List<(int ItemId, int WarehouseId, decimal Qty, decimal MinQty, decimal ReservedQty)>();

                foreach (var row in usedStocks.RowsUsed().Skip(1))
                {
                    string barcode = GetCellString(row, stockHeader, "barcode");
                    if (string.IsNullOrWhiteSpace(barcode))
                    {
                        done++;
                        if (done % 25 == 0) progress?.Invoke(done, Math.Max(1, total));
                        continue;
                    }

                    try
                    {
                        int itemId = itemIdByBarcode.TryGetValue(barcode, out var knownId) ? knownId : 0;
                        if (itemId <= 0)
                        {
                            using var find = new NpgsqlCommand("SELECT id, buy_price FROM items WHERE deleted_at IS NULL AND barcode = @b LIMIT 1", con, tran);
                            find.Parameters.AddWithValue("@b", barcode);
                            using var rdr = find.ExecuteReader();
                            if (rdr.Read())
                            {
                                itemId = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                                var bp = rdr.IsDBNull(1) ? 0m : rdr.GetDecimal(1);
                                buyPriceByItemId[itemId] = bp;
                            }
                        }
                        if (itemId <= 0)
                        {
                            result.StockFailed++;
                            var rowNum = row.RowNumber();
                            Err($"Stocks row {rowNum}: barcode {barcode} tidak ditemukan di items.");
                            continue;
                        }

                        int warehouseId = GetCellInt(row, stockHeader, "warehouse_id");
                        if (warehouseId <= 0)
                        {
                            string whName = GetCellString(row, stockHeader, "warehouse_name");
                            if (!string.IsNullOrWhiteSpace(whName) && warehouseIdByName.TryGetValue(whName, out var wid))
                                warehouseId = wid;
                        }
                        if (warehouseId > 0 && warehouseIds.Count > 0 && !warehouseIds.Contains(warehouseId))
                        {
                            result.StockFailed++;
                            var rowNum = row.RowNumber();
                            Err($"Stocks row {rowNum}: gudang id {warehouseId} tidak ditemukan/aktif untuk barcode {barcode}.");
                            continue;
                        }
                        if (warehouseId <= 0)
                        {
                            if (warehouseIds.Count == 1)
                            {
                                warehouseId = defaultWarehouseId;
                                var rowNum = row.RowNumber();
                                Err($"Stocks row {rowNum}: warehouse_id kosong, pakai gudang default id {defaultWarehouseId} (karena gudang aktif hanya 1).");
                            }
                            else
                            {
                                result.StockFailed++;
                                var rowNum = row.RowNumber();
                                Err($"Stocks row {rowNum}: warehouse_id/warehouse_name kosong/tidak valid untuk barcode {barcode}. Pilih gudang yang benar.");
                                continue;
                            }
                        }

                        decimal qty = GetCellDecimal(row, stockHeader, "qty");
                        decimal minQty = GetCellDecimal(row, stockHeader, "min_qty");
                        decimal reservedQty = GetCellDecimal(row, stockHeader, "reserved_qty");
                        if (qty < 0m) qty = 0m;
                        if (minQty < 0m) minQty = 0m;
                        if (reservedQty < 0m) reservedQty = 0m;

                        touchedItems.Add(itemId);
                        stockRows.Add((itemId, warehouseId, qty, minQty, reservedQty));
                    }
                    catch (Exception ex)
                    {
                        result.StockFailed++;
                        var rowNum = row.RowNumber();
                        Err($"Stocks row {rowNum}: {barcode} => {FormatDbException(ex)}");
                    }

                    done++;
                    if (done % 25 == 0) progress?.Invoke(done, Math.Max(1, total));
                }

                result.StockResetItems = touchedItems.Count;

                var rowByKey = new Dictionary<(int ItemId, int WarehouseId), (decimal Qty, decimal MinQty, decimal ReservedQty)>();
                foreach (var r in stockRows)
                    rowByKey[(r.ItemId, r.WarehouseId)] = (r.Qty, r.MinQty, r.ReservedQty);

                var oldQtyByKey = new Dictionary<(int ItemId, int WarehouseId), decimal>();
                try
                {
                    if (touchedItems.Count > 0)
                    {
                        using var cmdOld = new NpgsqlCommand(@"
SELECT item_id, warehouse_id, COALESCE(qty,0) AS qty
FROM stocks
WHERE item_id = ANY(@ids)
", con, tran);
                        cmdOld.Parameters.AddWithValue("@ids", touchedItems.ToArray());
                        using var rOld = cmdOld.ExecuteReader();
                        while (rOld.Read())
                        {
                            int iid = rOld["item_id"] != DBNull.Value ? Convert.ToInt32(rOld["item_id"]) : 0;
                            int wid = rOld["warehouse_id"] != DBNull.Value ? Convert.ToInt32(rOld["warehouse_id"]) : 0;
                            decimal q = rOld["qty"] != DBNull.Value ? Convert.ToDecimal(rOld["qty"]) : 0m;
                            if (iid > 0 && wid > 0)
                                oldQtyByKey[(iid, wid)] = q;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Err("Gagal baca stok lama untuk kartu stok import: " + FormatDbException(ex));
                }

                foreach (var itemId in touchedItems)
                {
                    try
                    {
                        using (var delStocks = new NpgsqlCommand("DELETE FROM stocks WHERE item_id = @item_id", con, tran))
                        {
                            delStocks.Parameters.AddWithValue("@item_id", itemId);
                            delStocks.ExecuteNonQuery();
                        }
                        using (var delLayers = new NpgsqlCommand("DELETE FROM stock_layers WHERE item_id = @item_id", con, tran))
                        {
                            delLayers.Parameters.AddWithValue("@item_id", itemId);
                            delLayers.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        result.StockFailed++;
                        Err($"Reset stok item_id {itemId} gagal: {FormatDbException(ex)}");
                    }
                }

                foreach (var kv in rowByKey)
                {
                    try
                    {
                        var srow = kv;
                        using (var ins = new NpgsqlCommand(@"
INSERT INTO stocks (item_id, warehouse_id, qty, min_qty, reserved_qty)
VALUES (@item_id, @warehouse_id, @qty, @min_qty, @reserved_qty)
", con, tran))
                        {
                            ins.Parameters.AddWithValue("@item_id", srow.Key.ItemId);
                            ins.Parameters.AddWithValue("@warehouse_id", srow.Key.WarehouseId);
                            ins.Parameters.AddWithValue("@qty", Convert.ToDouble(srow.Value.Qty));
                            ins.Parameters.AddWithValue("@min_qty", Convert.ToDouble(srow.Value.MinQty));
                            ins.Parameters.AddWithValue("@reserved_qty", Convert.ToDouble(srow.Value.ReservedQty));
                            ins.ExecuteNonQuery();
                        }

                        if (srow.Value.Qty > 0m)
                        {
                            var bp = buyPriceByItemId.TryGetValue(srow.Key.ItemId, out var bpx) ? bpx : 0m;
                            using var insLayer = new NpgsqlCommand(@"
INSERT INTO stock_layers (item_id, warehouse_id, qty_initial, qty_remaining, buy_price, created_at, expired_at)
VALUES (@item_id, @warehouse_id, @qty, @qty, @buy_price, NOW(), NULL)
", con, tran);
                            insLayer.Parameters.AddWithValue("@item_id", srow.Key.ItemId);
                            insLayer.Parameters.AddWithValue("@warehouse_id", srow.Key.WarehouseId);
                            insLayer.Parameters.AddWithValue("@qty", Convert.ToDouble(srow.Value.Qty));
                            insLayer.Parameters.AddWithValue("@buy_price", bp);
                            insLayer.ExecuteNonQuery();
                        }

                        result.StockOk++;
                    }
                    catch (Exception ex)
                    {
                        result.StockFailed++;
                        Err($"Insert stocks gagal: item_id {kv.Key.ItemId}, warehouse_id {kv.Key.WarehouseId} => {FormatDbException(ex)}");
                    }
                }

                try
                {
                    var session = SessionUser.GetCurrentUser();
                    int userId = session?.UserId ?? 0;
                    int? loginId = session?.LoginId;
                    if (userId <= 0)
                    {
                        Err("Kartu stok import dilewati karena session user tidak ditemukan.");
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(filePath ?? "");
                        string ketBase = string.IsNullOrWhiteSpace(fileName) ? "Import Excel (Stock)" : $"Import Excel (Stock) {fileName}";
                        if (ketBase.Length > 100) ketBase = ketBase.Substring(0, 100);

                        var allKeys = new HashSet<(int ItemId, int WarehouseId)>(oldQtyByKey.Keys);
                        foreach (var k in rowByKey.Keys)
                            allKeys.Add(k);

                        foreach (var k in allKeys)
                        {
                            decimal oldQty = oldQtyByKey.TryGetValue(k, out var oq) ? oq : 0m;
                            decimal newQty = rowByKey.TryGetValue(k, out var nr) ? nr.Qty : 0m;
                            decimal delta = newQty - oldQty;
                            if (delta == 0m) continue;

                            decimal? unitCost = null;
                            if (delta > 0m)
                                unitCost = buyPriceByItemId.TryGetValue(k.ItemId, out var bp) ? bp : 0m;

                            stockLogsToInsert.Add(new StockLog
                            {
                                ProductId = k.ItemId,
                                TipeTransaksi = "import",
                                QtyMasuk = delta > 0m ? delta : 0m,
                                QtyKeluar = delta < 0m ? Math.Abs(delta) : 0m,
                                SisaStock = newQty,
                                Keterangan = ketBase,
                                UserId = userId,
                                CreatedAt = DateTime.Now,
                                LoginId = loginId,
                                WarehouseId = k.WarehouseId,
                                RefType = "IMPORT_STOCK",
                                RefId = null,
                                UnitCost = unitCost,
                                Method = null,
                                IsAllocation = false
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Err("Gagal menyiapkan kartu stok import: " + FormatDbException(ex));
                }
            }

            tran.Commit();

            if (stockLogsToInsert.Count > 0)
            {
                try
                {
                    using var conLog = new NpgsqlConnection(DbConfig.ConnectionString);
                    conLog.Open();
                    foreach (var sl in stockLogsToInsert)
                    {
                        try
                        {
                            InsertStockLogNoTran(conLog, sl);
                        }
                        catch (Exception ex)
                        {
                            Err("Insert kartu stok import gagal: " + FormatDbException(ex));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Err("Kartu stok import gagal (connection): " + FormatDbException(ex));
                }
            }

            progress?.Invoke(Math.Max(1, total), Math.Max(1, total));
            return result;
        }

        private static bool ParseYN(string s, bool defaultVal)
        {
            if (string.IsNullOrWhiteSpace(s)) return defaultVal;
            s = s.Trim().ToUpperInvariant();
            if (s == "Y" || s == "YES" || s == "TRUE" || s == "1") return true;
            if (s == "N" || s == "NO" || s == "FALSE" || s == "0") return false;
            return defaultVal;
        }
    }
}
