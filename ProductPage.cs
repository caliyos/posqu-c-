
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
using POS_qu.Services;
using System.Transactions;
using System.Security.Cryptography.Xml;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace POS_qu
{
    public partial class ProductPage : Form
    {
        private ItemController itemController;
        private string selectedImagePath = "";
        private DataGridViewManager dgvManager;
        private List<UnitVariant> unitVariantsFromForm = new List<UnitVariant>(); // Store globally if needed
        private Panel panelSummary;
        private Label lblSumItems, lblSumQty, lblSumStockValue, lblSumRetailValue, lblSumInvRatio;
        private System.Data.DataTable _dtFull;

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

        private void ProductPage_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized; // Fullscreen
            //this.FormBorderStyle = FormBorderStyle.None; // Optional: Hide title bar

            // Position the close button top-right after full size is known

            //CUSTOM BUTTON CLOSE
            //Close Button
            //btnClose = new Button();
            //btnClose.Text = "X";
            //btnClose.Font = new Font("Arial", 14, FontStyle.Bold);
            //btnClose.ForeColor = Color.White;
            //btnClose.BackColor = Color.Red;
            //btnClose.FlatStyle = FlatStyle.Flat;
            //btnClose.FlatAppearance.BorderSize = 0;
            //btnClose.Size = new Size(50, 40);
            //btnClose.Location = new Point(this.ClientSize.Width - 10, 10); // Position top-right
            //btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            //btnClose.Click += BtnClose_Click;
            //inputPanel.Controls.Add(btnClose);
            //this.Controls.Add(btnClose);
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.DataBindingComplete += (s, e) => { ConfigureGridColumns(); UpdateSummaryFromGrid(); };
            dataGridView1.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var row = dataGridView1.Rows[e.RowIndex];
                OpenEditForRow(row);
            };

            // Toolbar cepat Export/Import agar selalu terlihat
            var quickBar = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = System.Drawing.Color.FromArgb(245, 245, 245) };
            var btnExportQuick = new Button
            {
                Text = "Export Items",
                Width = 130,
                Height = 36,
                Left = 10,
                Top = 8,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            var btnImportQuick = new Button
            {
                Text = "Import Item",
                Width = 130,
                Height = 36,
                Left = 150,
                Top = 8,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnExportQuick.Click += btnExportExcel_Click;
            btnImportQuick.Click += btnImportExcel_Click;
            quickBar.Controls.Add(btnExportQuick);
            quickBar.Controls.Add(btnImportQuick);

            // Panel ringkasan di bawah toolbar
            panelSummary = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = System.Drawing.Color.White };
            lblSumItems = new Label { Left = 10, Top = 8, Width = 260, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            lblSumQty = new Label { Left = 280, Top = 8, Width = 260, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            lblSumStockValue = new Label { Left = 10, Top = 34, Width = 260, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            lblSumRetailValue = new Label { Left = 280, Top = 34, Width = 260, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            lblSumInvRatio = new Label { Left = 550, Top = 8, Width = 320, Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            panelSummary.Controls.Add(lblSumItems);
            panelSummary.Controls.Add(lblSumQty);
            panelSummary.Controls.Add(lblSumStockValue);
            panelSummary.Controls.Add(lblSumRetailValue);
            panelSummary.Controls.Add(lblSumInvRatio);

            // Panel actions: Select All, Search, Stock Adjustment, Refresh
            var actionPanel = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = System.Drawing.Color.FromArgb(250, 250, 250) };
            var chkActionSelectAll = new CheckBox
            {
                Text = "Pilih Semua",
                Left = 12,
                Top = 18,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            chkActionSelectAll.CheckedChanged += (s, ev) =>
            {
                bool checkAll = chkActionSelectAll.Checked;
                foreach (DataGridViewRow row2 in dataGridView1.Rows)
                {
                    if (row2.IsNewRow) continue;
                    if (row2.Cells["chkSelect"] != null)
                        row2.Cells["chkSelect"].Value = checkAll;
                }
            };
            var lblCari = new Label
            {
                Text = "Cari:",
                Left = 150,
                Top = 20,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            var txtActionSearch = new TextBox
            {
                Left = 195,
                Top = 16,
                Width = 260,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            txtActionSearch.TextChanged += (s, ev) =>
            {
                if (dgvManager != null)
                {
                    dgvManager.Filter(txtActionSearch.Text, "name");
                }
            };
            var btnActionStockAdj = new Button
            {
                Text = "Update Stock",
                Left = 470,
                Top = 12,
                Width = 140,
                Height = 32,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnActionStockAdj.Click += btnStockAdjs_Click;
            var btnActionRefresh = new Button
            {
                Text = "Refresh",
                Left = 620,
                Top = 12,
                Width = 100,
                Height = 32,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            btnActionRefresh.Click += btnRefresh_Click;
            actionPanel.Controls.Add(chkActionSelectAll);
            actionPanel.Controls.Add(lblCari);
            actionPanel.Controls.Add(txtActionSearch);
            actionPanel.Controls.Add(btnActionStockAdj);
            actionPanel.Controls.Add(btnActionRefresh);

            // Tambahkan ke tablePanel agar selalu terlihat
            tablePanel.Controls.Add(quickBar);
            tablePanel.Controls.Add(panelSummary);
            tablePanel.Controls.Add(actionPanel);
            tablePanel.Controls.SetChildIndex(quickBar, 0);
            tablePanel.Controls.SetChildIndex(panelSummary, 1);
            tablePanel.Controls.SetChildIndex(actionPanel, 2);
            dataGridView1.Dock = DockStyle.Fill;
            LoadItems();
            ApplyProfessionalGridStyle();
            dataGridView1.Dock = DockStyle.Fill;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // header diklik

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

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

            // Mapping manual dari DataGridView ke Item seperti sebelumnya
            Item selectedItem = new Item
            {
                id = itemId,
                name = rowSelected.Cells["name"].Value.ToString(),
                buy_price = Convert.ToDecimal(rowSelected.Cells["buy_price"].Value),
                sell_price = Convert.ToDecimal(rowSelected.Cells["sell_price"].Value),
                stock = Convert.ToInt32(rowSelected.Cells["stock"].Value),
                barcode = rowSelected.Cells["barcode"].Value.ToString(),
                unitid = rowSelected.Cells["unit_id"].Value == DBNull.Value ? 1 : Convert.ToInt32(rowSelected.Cells["unit_id"].Value),
                unit = rowSelected.Cells["unit_name"].Value.ToString(),
                category_id = rowSelected.Cells["category_id"].Value == DBNull.Value ? 0 : Convert.ToInt32(rowSelected.Cells["category_id"].Value),
                supplier_id = rowSelected.Cells["supplier_id"].Value == DBNull.Value ? 0 : Convert.ToInt32(rowSelected.Cells["supplier_id"].Value),
                note = rowSelected.Cells["note"].Value.ToString(),
                picture = rowSelected.Cells["picture"].Value?.ToString(),
                is_inventory_p = Convert.ToBoolean(rowSelected.Cells["is_inventory_p"].Value),
                IsPurchasable = Convert.ToBoolean(rowSelected.Cells["is_purchasable"].Value),
                IsSellable = Convert.ToBoolean(rowSelected.Cells["is_sellable"].Value),
                RequireNotePayment = Convert.ToBoolean(rowSelected.Cells["is_note_payment"].Value),
                is_changeprice_p = Convert.ToBoolean(rowSelected.Cells["is_changeprice_p"].Value),
                discount_formula = rowSelected.Cells["discount_formula"].Value.ToString(),
                HasMaterials = Convert.ToBoolean(rowSelected.Cells["is_have_bahan"].Value),
                IsPackage = Convert.ToBoolean(rowSelected.Cells["is_box"].Value),
                IsProduced = Convert.ToBoolean(rowSelected.Cells["is_produksi"].Value)
            };

            selectedItem.UnitVariants = itemController.GetUnitVariants(itemId);

            using (var detailForm = new ItemDetailForm(selectedItem))
            {
                if (detailForm.ShowDialog() == DialogResult.OK)
                    LoadItems(); // reload grid setelah edit
            }
        }


        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            var selectedItems = new List<int>();

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
                    itemController.DeleteItem(id);
                }
                LoadItems(); // reload grid
            }
        }

        //private void DataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // header diklik

        //    dataGridView1.ReadOnly = true;

        //    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
        //    int itemId = Convert.ToInt32(row.Cells["id"].Value);

        //    // cek apakah tombol variant diklik
        //    if (dataGridView1.Columns[e.ColumnIndex].Name == "btnVariantButton")
        //    {
        //        //// Ambil item & variants
        //        //Item selectedItem = itemController.GetItemById(itemId);
        //        //selectedItem.UnitVariants = itemController.GetUnitVariants(itemId);

        //        //// Bisa tampilkan di modal khusus variant
        //        //using (var variantForm = new UnitVariantForm(selectedItem))
        //        //{
        //        //    if (variantForm.ShowDialog() == DialogResult.OK)
        //        //    {
        //        //        // jika ada perubahan variant, reload item grid
        //        //        LoadItems();
        //        //    }
        //        //}
        //    }
        //    else
        //    {
        //        // klik row biasa -> buka detail form seperti biasa
        //        Item selectedItem = new Item
        //        {
        //            id = itemId,
        //            name = row.Cells["name"].Value.ToString(),
        //            buy_price = Convert.ToDecimal(row.Cells["buy_price"].Value),
        //            sell_price = Convert.ToDecimal(row.Cells["sell_price"].Value),
        //            stock = Convert.ToInt32(row.Cells["stock"].Value),
        //            barcode = row.Cells["barcode"].Value.ToString(),
        //            unitid = Convert.ToInt32(row.Cells["unit_id"].Value),
        //            unit = row.Cells["unit_name"].Value.ToString(),
        //            category_id = Convert.ToInt32(row.Cells["category_id"].Value),
        //            supplier_id = Convert.ToInt32(row.Cells["supplier_id"].Value),
        //            note = row.Cells["note"].Value.ToString(),
        //            picture = row.Cells["picture"].Value?.ToString(),
        //            is_inventory_p = Convert.ToBoolean(row.Cells["is_inventory_p"].Value),
        //            IsPurchasable = Convert.ToBoolean(row.Cells["is_purchasable"].Value),
        //            IsSellable = Convert.ToBoolean(row.Cells["is_sellable"].Value),
        //            RequireNotePayment = Convert.ToBoolean(row.Cells["is_note_payment"].Value),
        //            is_changeprice_p = Convert.ToBoolean(row.Cells["is_changeprice_p"].Value),
        //            discount_formula = row.Cells["discount_formula"].Value.ToString(),
        //            HasMaterials = Convert.ToBoolean(row.Cells["is_have_bahan"].Value),
        //            IsPackage = Convert.ToBoolean(row.Cells["is_box"].Value),
        //            IsProduced = Convert.ToBoolean(row.Cells["is_produksi"].Value)
        //        };
        //        selectedItem.UnitVariants = itemController.GetUnitVariants(itemId);

        //        using (var detailForm = new ItemDetailForm(selectedItem))
        //        {
        //            if (detailForm.ShowDialog() == DialogResult.OK)
        //            {
        //                LoadItems(); // reload grid setelah simpan
        //            }
        //        }
        //    }
        //}


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

            itemController = new ItemController();
            DataTable dt = itemController.GetItems();
            _dtFull = dt?.Copy();

            // Tambahkan kolom dummy untuk variant
            if (!dt.Columns.Contains("btnVariant"))
                dt.Columns.Add("btnVariant", typeof(string));

            // 1️⃣ Tambahkan kolom dummy di DataTable
            if (!dt.Columns.Contains("UnitVariant"))
                dt.Columns.Add("UnitVariant", typeof(string));
            // 1b️⃣ Tambahkan kolom nilai stok dan nilai jual
            if (!dt.Columns.Contains("stock_value"))
                dt.Columns.Add("stock_value", typeof(decimal));
            if (!dt.Columns.Contains("retail_value"))
                dt.Columns.Add("retail_value", typeof(decimal));

            // 2️⃣ Isi "+" jika item punya variant
            foreach (DataRow row in dt.Rows)
            {
                int itemId = Convert.ToInt32(row["id"]);
                var variants = itemController.GetUnitVariants(itemId);
                row["UnitVariant"] = variants.Count > 0 ? "+" : "";
                // Hitung nilai
                try
                {
                    decimal buy = row.Table.Columns.Contains("buy_price") && row["buy_price"] != DBNull.Value ? Convert.ToDecimal(row["buy_price"]) : 0m;
                    decimal sell = row.Table.Columns.Contains("sell_price") && row["sell_price"] != DBNull.Value ? Convert.ToDecimal(row["sell_price"]) : 0m;
                    int stok = row.Table.Columns.Contains("stock") && row["stock"] != DBNull.Value ? Convert.ToInt32(row["stock"]) : 0;
                    row["stock_value"] = buy * stok;
                    row["retail_value"] = sell * stok;
                }
                catch
                {
                    row["stock_value"] = 0m;
                    row["retail_value"] = 0m;
                }
            }



            // 3️⃣ Bind ke DataGridView
            dataGridView1.DataSource = dt;

            // 4️⃣ Pindahkan kolom UnitVariant ke posisi pertama
            if (dataGridView1.Columns.Contains("UnitVariant"))
                dataGridView1.Columns["UnitVariant"].DisplayIndex = 1;
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

            if (!dataGridView1.Columns.Contains("chkSelect"))
            {
                var chkColumn = new DataGridViewCheckBoxColumn();
                chkColumn.Name = "chkSelect";
                chkColumn.HeaderText = ""; // bisa tambahkan "Pilih"
                chkColumn.Width = 30;
                dataGridView1.Columns.Insert(0, chkColumn);
            }

            dgvManager = new DataGridViewManager(dataGridView1, dt, 10);
            dgvManager.PagingInfoLabel = lblPagingInfo;
            dgvManager.LoadPage();

            cmbPageSize.Items.AddRange(new object[] { "10", "50", "100", "200", "500", "1000" });
            cmbPageSize.SelectedIndex = 0;


            ApplyProfessionalGridStyle();
            UpdateSummaryFromGrid();
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

        //private void CheckUnitVariantEligibility()
        //{
        //    // Assuming txtItemName and cmbUnit are your controls
        //    bool isItemNameValid = !string.IsNullOrWhiteSpace(txtName.Text);
        //    bool isUnitSelected = cmbUnit.SelectedIndex >= 0;

        //    btnUnitVariant.Enabled = isItemNameValid && isUnitSelected;
        //}


        //private void btnUnitVariant_Click(object sender, EventArgs e)
        //{
        //    bool isItemNameValid = !string.IsNullOrWhiteSpace(txtName.Text);
        //    bool isUnitSelected = cmbUnit.SelectedIndex >= 0;

        //    if (!isItemNameValid || !isUnitSelected)
        //    {
        //        btnUnitVariant.Cursor = Cursors.No; // Stop sign cursor
        //        MessageBox.Show("Please enter item name and select a unit first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    btnUnitVariant.Cursor = Cursors.Default;

        //    int itemId = 2; // Replace with how you actually get it
        //    string baseUnitName = cmbUnit.Text; // get selected base unit name
        //    using (var variantForm = new UnitVariantForm(itemId, baseUnitName, unitVariantsFromForm))
        //    {
        //        var result = variantForm.ShowDialog();
        //        if (result == DialogResult.OK)
        //        {
        //            unitVariantsFromForm = variantForm.UnitVariants;
        //        }
        //    }
        //}

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
                DataTable dt = itemController.GetItems();

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
            var selectedItems = new List<int>();

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
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 248, 248);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(220, 235, 255);
            dataGridView1.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
        }

        private void UpdateSummaryFromGrid()
        {
            try
            {
                int rows = dataGridView1.Rows.Count;
                long sumQty = 0;
                decimal sumStockValue = 0m, sumRetailValue = 0m;
                int invCount = 0, nonInvCount = 0;
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if (r.IsNewRow) continue;
                    int q = 0;
                    if (r.Cells["stock"]?.Value != null && int.TryParse(r.Cells["stock"].Value.ToString(), out var qtmp)) q = qtmp;
                    sumQty += q;
                    if (r.Cells["stock_value"]?.Value != null && decimal.TryParse(r.Cells["stock_value"].Value.ToString(), out var sv)) sumStockValue += sv;
                    if (r.Cells["retail_value"]?.Value != null && decimal.TryParse(r.Cells["retail_value"].Value.ToString(), out var rv)) sumRetailValue += rv;
                    bool inv = false;
                    if (r.Cells["is_inventory_p"]?.Value != null) bool.TryParse(r.Cells["is_inventory_p"].Value.ToString(), out inv);
                    if (inv) invCount++; else nonInvCount++;
                }
                // Gunakan label dari designer
                label9.Text = $"Nilai Stock (HPP): {sumStockValue:N0} | Nilai Jual: {sumRetailValue:N0}";
                label10.Text = $"Jumlah Stock: {sumQty:N0} | Total Item: {rows:N0} (Inv:{invCount:N0}/Non:{nonInvCount:N0})";
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
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = 90;
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
                "created_at","updated_at","category_name","supplier_name","btnVariant"
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
            Item selectedItem = new Item
            {
                id = itemId,
                name = rowSelected.Cells["name"].Value.ToString(),
                buy_price = Convert.ToDecimal(rowSelected.Cells["buy_price"].Value),
                sell_price = Convert.ToDecimal(rowSelected.Cells["sell_price"].Value),
                stock = Convert.ToInt32(rowSelected.Cells["stock"].Value),
                barcode = rowSelected.Cells["barcode"].Value.ToString(),
                unitid = rowSelected.Cells["unit_id"].Value == DBNull.Value ? 1 : Convert.ToInt32(rowSelected.Cells["unit_id"].Value),
                unit = rowSelected.Cells["unit_name"].Value.ToString(),
                category_id = rowSelected.Cells["category_id"].Value == DBNull.Value ? 0 : Convert.ToInt32(rowSelected.Cells["category_id"].Value),
                supplier_id = rowSelected.Cells["supplier_id"].Value == DBNull.Value ? 0 : Convert.ToInt32(rowSelected.Cells["supplier_id"].Value),
                note = rowSelected.Cells["note"].Value.ToString(),
                picture = rowSelected.Cells["picture"].Value?.ToString(),
                is_inventory_p = Convert.ToBoolean(rowSelected.Cells["is_inventory_p"].Value),
                IsPurchasable = Convert.ToBoolean(rowSelected.Cells["is_purchasable"].Value),
                IsSellable = Convert.ToBoolean(rowSelected.Cells["is_sellable"].Value),
                RequireNotePayment = Convert.ToBoolean(rowSelected.Cells["is_note_payment"].Value),
                is_changeprice_p = Convert.ToBoolean(rowSelected.Cells["is_changeprice_p"].Value),
                discount_formula = rowSelected.Cells["discount_formula"].Value.ToString(),
                HasMaterials = Convert.ToBoolean(rowSelected.Cells["is_have_bahan"].Value),
                IsPackage = Convert.ToBoolean(rowSelected.Cells["is_box"].Value),
                IsProduced = Convert.ToBoolean(rowSelected.Cells["is_produksi"].Value)
            };
            selectedItem.UnitVariants = itemController.GetUnitVariants(itemId);
            using (var detailForm = new ItemDetailForm(selectedItem))
            {
                if (detailForm.ShowDialog() == DialogResult.OK)
                    LoadItems();
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
                        var id = itemController.InsertItem(item);
                        if (id != null) ok++; else fail++;
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
