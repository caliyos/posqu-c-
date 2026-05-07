
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

            if (MessageBox.Show($"Hapus {selectedItems.Count} item terpilih?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (int id in selectedItems)
                {
                    _productService.DeleteProduct(id, out string msg);
                }
                LoadItems(); // reload grid
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
                dataGridView1.Columns["stock_value"].DefaultCellStyle.Format = "N0";
                dataGridView1.Columns["stock_value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dataGridView1.Columns.Contains("retail_value"))
            {
                dataGridView1.Columns["retail_value"].HeaderText = "Nilai Jual";
                dataGridView1.Columns["retail_value"].DefaultCellStyle.Format = "N0";
                dataGridView1.Columns["retail_value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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

            cmbPageSize.Items.AddRange(new object[] { "10", "50", "100", "200", "500", "1000" });
            cmbPageSize.SelectedIndex = 3;


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
                var dtAll = _productService.GetAllProducts();
                if (dtAll != null)
                {
                    if (!dtAll.Columns.Contains("warehouse_id")) dtAll.Columns.Add("warehouse_id", typeof(int));
                    if (!dtAll.Columns.Contains("warehouse_name")) dtAll.Columns.Add("warehouse_name", typeof(string));
                    foreach (DataRow r in dtAll.Rows)
                    {
                        r["warehouse_id"] = 0;
                        r["warehouse_name"] = "Semua Gudang";
                    }
                }
                return dtAll;
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
                items.buy_price,
                items.sell_price,
                COALESCE(s.qty, 0) AS stock,
                COALESCE(s.min_qty, 0) AS min_qty,
                0 AS reserved_qty,
                items.valuation_method,
                items.unit AS unit_id,
                units.name AS unit_name,
                COALESCE(uvbase.minqty, 0) AS min_stock,
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
                w.id AS warehouse_id,
                w.name AS warehouse_name
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
                // Ambil data dari database lewat controller
                DataTable dt = _productService.GetAllProducts();

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diexport.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Pilih lokasi simpan file
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "Items_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = sfd.FileName;
                    ExportDataTableToExcel(dt, filePath);

                    MessageBox.Show("Data berhasil diexport ke Excel:\n" + filePath, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal export Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportDataTableToExcel(DataTable dt, string filePath)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Items");
                worksheet.Cell(1, 1).InsertTable(dt);
                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }
        }


        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            Form importForm = new Form();
            importForm.Text = "Import Item";
            importForm.StartPosition = FormStartPosition.CenterParent;
            importForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            importForm.Width = 450;
            importForm.Height = 350;
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
                Width = 180,
                Height = 30
            };
            btnDownloadTemplate.Click += (s, ev) =>
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = "template_import_item.xlsx"
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
                Width = 280,
                ReadOnly = true
            };
            importForm.Controls.Add(txtFile);

            // Tombol browse
            Button btnBrowse = new Button()
            {
                Text = "Browse...",
                Location = new Point(310, 123),
                Width = 100,
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

            // Tombol Import
            Button btnDoImport = new Button()
            {
                Text = "Import Sekarang",
                BackColor = System.Drawing.Color.FromArgb(0, 123, 255),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(230, 170),
                Width = 120,
                Height = 35
            };
            btnDoImport.Click += (s, ev) =>
            {
                if (string.IsNullOrEmpty(txtFile.Text))
                {
                    MessageBox.Show("Pilih file terlebih dahulu!");
                    return;
                }

                try
                {
                    ImportItemsFromExcelImpl(txtFile.Text);
                    MessageBox.Show("Import berhasil!");
                    importForm.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal import: " + ex.Message);
                }
            };
            importForm.Controls.Add(btnDoImport);

            // Tombol Batal
            Button btnCancel = new Button()
            {
                Text = "Batal",
                Location = new Point(360, 170),
                Width = 60,
                Height = 35
            };
            btnCancel.Click += (s, ev) => importForm.Close();
            importForm.Controls.Add(btnCancel);

            importForm.ShowDialog();
        }


        private void CreateTemplateExcel(string path)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                // === SHEET 1: Template Utama ===
                var ws = workbook.Worksheets.Add("Template");
                ws.Cell(1, 1).Value = "name";
                ws.Cell(1, 2).Value = "buy_price";
                ws.Cell(1, 3).Value = "sell_price";
                ws.Cell(1, 4).Value = "barcode";
                ws.Cell(1, 5).Value = "stock";
                ws.Cell(1, 6).Value = "unit";           // pakai ID dari tabel units
                ws.Cell(1, 7).Value = "group";          // ID group item
                ws.Cell(1, 8).Value = "supplier_id";    // ID supplier
                ws.Cell(1, 9).Value = "note";
                ws.Cell(1, 10).Value = "is_inventory_p";    // Y/N
                ws.Cell(1, 11).Value = "is_changeprice_p";  // Y/N

                ws.Row(1).Style.Font.Bold = true;
                ws.Columns().AdjustToContents();

                // Tambahkan keterangan di baris bawah
                ws.Cell(3, 1).Value = "⚠️ Petunjuk:";
                ws.Cell(4, 1).Value = "1. Kolom 'unit', 'group', dan 'supplier_id' harus diisi sesuai referensi di sheet 'Referensi'.";
                ws.Cell(5, 1).Value = "2. Gunakan 'Y' atau 'N' untuk kolom boolean (is_inventory_p, is_changeprice_p).";
                ws.Cell(6, 1).Value = "3. Harga dalam format angka tanpa titik pemisah ribuan (contoh: 15000).";
                ws.Cell(7, 1).Value = "4. Barcode boleh dikosongkan jika tidak digunakan.";

                // === SHEET 2: Referensi ===
                var refSheet = workbook.Worksheets.Add("Referensi");

                refSheet.Cell(1, 1).Value = "Referensi Unit";
                refSheet.Cell(1, 1).Style.Font.Bold = true;
                refSheet.Cell(2, 1).Value = "ID";
                refSheet.Cell(2, 2).Value = "Name";
                refSheet.Cell(2, 3).Value = "Abbr";

                // contoh data unit
                refSheet.Cell(3, 1).Value = 1;
                refSheet.Cell(3, 2).Value = "Buah";
                refSheet.Cell(3, 3).Value = "buah";

                refSheet.Cell(4, 1).Value = 2;
                refSheet.Cell(4, 2).Value = "Dus";
                refSheet.Cell(4, 3).Value = "dus";

                refSheet.Cell(5, 1).Value = 3;
                refSheet.Cell(5, 2).Value = "Gram";
                refSheet.Cell(5, 3).Value = "Gr";

                refSheet.Cell(6, 1).Value = 4;
                refSheet.Cell(6, 2).Value = "Kilogram";
                refSheet.Cell(6, 3).Value = "Kg";

                refSheet.Cell(7, 1).Value = 5;
                refSheet.Cell(7, 2).Value = "Pieces";
                refSheet.Cell(7, 3).Value = "pcs";

                // bagian referensi group
                refSheet.Cell(9, 1).Value = "Referensi Group";
                refSheet.Cell(9, 1).Style.Font.Bold = true;
                refSheet.Cell(10, 1).Value = "ID";
                refSheet.Cell(10, 2).Value = "Nama Group";

                refSheet.Cell(11, 1).Value = 1;
                refSheet.Cell(11, 2).Value = "Makanan";
                refSheet.Cell(12, 1).Value = 2;
                refSheet.Cell(12, 2).Value = "Minuman";
                refSheet.Cell(13, 1).Value = 3;
                refSheet.Cell(13, 2).Value = "Peralatan";
                refSheet.Cell(14, 1).Value = 4;
                refSheet.Cell(14, 2).Value = "Lainnya";

                // bagian referensi supplier
                refSheet.Cell(16, 1).Value = "Referensi Supplier";
                refSheet.Cell(16, 1).Style.Font.Bold = true;
                refSheet.Cell(17, 1).Value = "ID";
                refSheet.Cell(17, 2).Value = "Nama Supplier";

                refSheet.Cell(18, 1).Value = 1;
                refSheet.Cell(18, 2).Value = "PT Indofood Sukses Makmur";
                refSheet.Cell(19, 1).Value = 2;
                refSheet.Cell(19, 2).Value = "PT Mayora Indah Tbk";
                refSheet.Cell(20, 1).Value = 3;
                refSheet.Cell(20, 2).Value = "Toko Sumber Rejeki";
                refSheet.Cell(21, 1).Value = 4;
                refSheet.Cell(21, 2).Value = "Supplier Lainnya";

                // format tabel
                refSheet.Columns().AdjustToContents();
                refSheet.Rows().Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;

                // === SHEET 3: Sample10 ===
                var smp = workbook.Worksheets.Add("Sample10");
                smp.Cell(1, 1).Value = "name";
                smp.Cell(1, 2).Value = "buy_price";
                smp.Cell(1, 3).Value = "sell_price";
                smp.Cell(1, 4).Value = "barcode";
                smp.Cell(1, 5).Value = "stock";
                smp.Cell(1, 6).Value = "unit";
                smp.Cell(1, 7).Value = "group";
                smp.Cell(1, 8).Value = "supplier_id";
                smp.Cell(1, 9).Value = "note";
                smp.Cell(1, 10).Value = "is_inventory_p";
                smp.Cell(1, 11).Value = "is_changeprice_p";
                smp.Row(1).Style.Font.Bold = true;
                var samples = new[]
                {
                    new {N="Plastik Kresek Kecil", BP=0m, SP=300m, BC="PK-KCL", ST=0, U=1, G=4, S=4, Note="Non inventory", Inv="N", Chg="N"},
                    new {N="Plastik Kresek Besar", BP=0m, SP=500m, BC="PK-BSR", ST=0, U=1, G=4, S=4, Note="Non inventory", Inv="N", Chg="N"},
                    new {N="Aqua Gelas", BP=500m, SP=1500m, BC="AG-01", ST=100, U=5, G=2, S=3, Note="", Inv="Y", Chg="N"},
                    new {N="Teh Botol", BP=2500m, SP=4000m, BC="TB-01", ST=50, U=5, G=2, S=3, Note="", Inv="Y", Chg="N"},
                    new {N="Kopi Sachet", BP=1000m, SP=2000m, BC="KS-01", ST=200, U=5, G=1, S=1, Note="", Inv="Y", Chg="N"},
                    new {N="Tissue Saku", BP=1500m, SP=2500m, BC="TS-01", ST=80, U=5, G=4, S=4, Note="", Inv="Y", Chg="N"},
                    new {N="Pulsa Telkomsel 10k", BP=9000m, SP=10000m, BC="PLS-TSEL-10", ST=0, U=5, G=4, S=4, Note="Non inventory (jasa)", Inv="N", Chg="N"},
                    new {N="Sedotan Jumbo", BP=300m, SP=500m, BC="SD-JMB", ST=100, U=5, G=4, S=4, Note="", Inv="Y", Chg="N"},
                    new {N="Gula Pasir 1kg", BP=12000m, SP=15000m, BC="GP-1KG", ST=40, U=4, G=1, S=1, Note="", Inv="Y", Chg="N"},
                    new {N="Kantong Kertas", BP=0m, SP=1000m, BC="KK-01", ST=0, U=5, G=4, S=4, Note="Non inventory", Inv="N", Chg="N"},
                };
                int r = 2;
                foreach (var x in samples)
                {
                    smp.Cell(r, 1).Value = x.N;
                    smp.Cell(r, 2).Value = x.BP;
                    smp.Cell(r, 3).Value = x.SP;
                    smp.Cell(r, 4).Value = x.BC;
                    smp.Cell(r, 5).Value = x.ST;
                    smp.Cell(r, 6).Value = x.U;
                    smp.Cell(r, 7).Value = x.G;
                    smp.Cell(r, 8).Value = x.S;
                    smp.Cell(r, 9).Value = x.Note;
                    smp.Cell(r, 10).Value = x.Inv;
                    smp.Cell(r, 11).Value = x.Chg;
                    r++;
                }
                smp.Columns().AdjustToContents();

                workbook.SaveAs(path);
            }
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
            var selectedItems = new HashSet<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chkSelect"].Value))
                    selectedItems.Add(Convert.ToInt32(row.Cells["id"].Value));
            }

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Pilih item dulu untuk di update stock.");
                return;
            }

            if (MessageBox.Show($"Apakah Akan Melakukan update stock pada  {selectedItems.Count} item terpilih?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (int id in selectedItems)
                {
                    using (StockAdjustment f = new StockAdjustment(id))
                    {
                        f.ShowDialog(); // owner = form utama
                    }

                }
                LoadItems();
            }
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
            dataGridView1.ColumnHeadersHeight = 45;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            
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
                label9.Text = $"Nilai Stock (HPP): {sumStockValue:N0} | Nilai Jual: {sumRetailValue:N0}";
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
            if (dataGridView1.Columns.Contains("warehouse_name"))
            {
                var c = dataGridView1.Columns["warehouse_name"];
                c.HeaderText = "Gudang";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 140;
                c.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            if (dataGridView1.Columns.Contains("unit_name"))
            {
                var c = dataGridView1.Columns["unit_name"];
                c.HeaderText = "Satuan";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 90;
            }
            if (dataGridView1.Columns.Contains("sell_price"))
            {
                var c = dataGridView1.Columns["sell_price"];
                c.HeaderText = "Harga";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N0";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 110;
            }
            if (dataGridView1.Columns.Contains("buy_price"))
            {
                var c = dataGridView1.Columns["buy_price"];
                c.HeaderText = "HPP";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N0";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 110;
            }
            if (dataGridView1.Columns.Contains("stock"))
            {
                var c = dataGridView1.Columns["stock"];
                c.HeaderText = "Stok";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = _viewAllUnits ? "N2" : "N0";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 90;
            }
            if (dataGridView1.Columns.Contains("min_threshold"))
            {
                var c = dataGridView1.Columns["min_threshold"];
                c.HeaderText = "Min Qty";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = _viewAllUnits ? "N2" : "N0";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 80;
                c.Visible = true;
                if (dataGridView1.Columns.Contains("stock"))
                    c.DisplayIndex = Math.Min(dataGridView1.Columns["stock"].DisplayIndex + 1, dataGridView1.Columns.Count - 1);
            }
            if (dataGridView1.Columns.Contains("stock_value"))
            {
                var c = dataGridView1.Columns["stock_value"];
                c.HeaderText = "Nilai Stok (HPP)";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N0";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 140;
            }
            if (dataGridView1.Columns.Contains("retail_value"))
            {
                var c = dataGridView1.Columns["retail_value"];
                c.HeaderText = "Nilai Jual";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                c.DefaultCellStyle.Format = "N0";
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 140;
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
        private void ImportItemsFromExcelImpl(string filePath)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook(filePath))
            {
                var ws = workbook.Worksheets.First();
                var used = ws.RangeUsed();
                if (used == null)
                {
                    MessageBox.Show("File kosong.");
                    return;
                }
                int ok = 0, fail = 0;
                foreach (var row in used.RowsUsed().Skip(1))
                {
                    try
                    {
                        var item = new Item
                        {
                            name = row.Cell(1).GetString(),
                            buy_price = (decimal)(row.Cell(2).TryGetValue<double>(out var bp) ? bp : 0),
                            sell_price = (decimal)(row.Cell(3).TryGetValue<double>(out var sp) ? sp : 0),
                            barcode = row.Cell(4).GetString(),
                            stock = (int)(row.Cell(5).TryGetValue<double>(out var st) ? st : 0),
                            unitid = row.Cell(6).TryGetValue<int>(out var u) ? u : 1,
                            category_id = row.Cell(7).TryGetValue<int>(out var g) ? g : 0,
                            supplier_id = row.Cell(8).TryGetValue<int>(out var s) ? s : 0,
                            note = row.Cell(9).GetString(),
                            is_inventory_p = ParseYN(row.Cell(10).GetString(), true),
                            is_changeprice_p = ParseYN(row.Cell(11).GetString(), false),
                            IsPurchasable = true,
                            IsSellable = true
                        };
                        bool success = _productService.SaveProduct(item, out string msg);
                        if (success) ok++; else fail++;
                    }
                    catch
                    {
                        fail++;
                    }
                }
                MessageBox.Show($"Import selesai. Sukses: {ok}, Gagal: {fail}");
                LoadItems();
            }
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
