
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


        }

        private void SetFormMode(bool isEditing)
        {
            btnSave.Enabled = !isEditing;
            btnUpdate.Enabled = isEditing;
            btnDelete.Enabled = isEditing;

        }
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
            this.FormBorderStyle = FormBorderStyle.None; // Optional: Hide title bar

            // Position the close button top-right after full size is known
            btnClose.Location = new Point(this.ClientSize.Width - btnClose.Width - 10, 10);
            btnClose.BringToFront();
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
            dataGridView1.CellClick += DataGridView1_CellClick;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            LoadItems();

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
            //DataTable dt = itemController.GetItems();
            //dataGridView1.DataSource = dt;


            dataGridView1.ReadOnly = true; // 🔒 Membuat seluruh grid hanya bisa dibaca
            dataGridView1.AllowUserToAddRows = false; // optional: cegah baris kosong di akhir
            dataGridView1.AllowUserToDeleteRows = false; // optional: cegah hapus manual
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // klik 1 baris penuh
            dataGridView1.MultiSelect = false; // hanya bisa pilih satu baris

            itemController = new ItemController();
            DataTable dt = itemController.GetItems();
            dgvManager = new DataGridViewManager(dataGridView1, dt, 10);
            dgvManager.PagingInfoLabel = lblPagingInfo;
            dgvManager.LoadPage();
            cmbPageSize.Items.AddRange(new object[] { "10", "50", "100", "200", "500", "1000" });
            cmbPageSize.SelectedIndex = 0; // Default to 10


            DataTable unitTable = itemController.GetUnits();
            cmbUnit.DataSource = unitTable;
            cmbUnit.DisplayMember = "display"; // this shows e.g. "pieces (pcs)"
            cmbUnit.ValueMember = "id";        // this stores the unit ID
            cmbUnit.SelectedIndex = -1;        // optional: makes sure nothing is selected by default

            DataTable groupTable = itemController.GetGroups();
            cmbGroup.DataSource = groupTable;
            cmbGroup.DisplayMember = "display"; // this shows e.g. "pieces (pcs)"
            cmbGroup.ValueMember = "id";        // this stores the unit ID
            cmbGroup.SelectedIndex = -1;        // optional: makes sure nothing is selected by default

            SetFormMode(false); // Ensure buttons are reset
            // fill cmbunits
            // fill groups
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {


                // 1️⃣ Validasi dulu
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Nama produk harus diisi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (!decimal.TryParse(txtBuyPrice.Text, out decimal buyPrice))
                {
                    MessageBox.Show("Harga beli harus diisi dengan angka yang valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBuyPrice.Focus();
                    return;
                }

                if (!decimal.TryParse(txtSellPrice.Text, out decimal sellPrice))
                {
                    MessageBox.Show("Harga jual harus diisi dengan angka yang valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSellPrice.Focus();
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock))
                {
                    MessageBox.Show("Stok harus diisi dengan angka yang valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStock.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtBarcode.Text))
                {
                    MessageBox.Show("Barcode harus diisi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBarcode.Focus();
                    return;
                }

                if (cmbUnit.SelectedValue == null || Convert.ToInt32(cmbUnit.SelectedValue) <= 0)
                {
                    MessageBox.Show("Unit harus dipilih!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbUnit.Focus();
                    return;
                }

                if (cmbGroup.SelectedValue == null || Convert.ToInt32(cmbGroup.SelectedValue) <= 0)
                {
                    MessageBox.Show("Group harus dipilih!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbGroup.Focus();
                    return;
                }

                // 2️⃣ Baru isi variabel setelah semua validasi lolos
                string name = txtName.Text.Trim();
                string barcode = txtBarcode.Text.Trim();
                string description = txtDescription.Text.Trim();
                int unitId = Convert.ToInt32(cmbUnit.SelectedValue);
                int groupId = Convert.ToInt32(cmbGroup.SelectedValue);


                Item item = new Item
                {
                    name = name,
                    buy_price = buyPrice,
                    sell_price = sellPrice,
                    stock = stock,
                    barcode = barcode,
                    unit = cmbUnit.SelectedValue?.ToString(),
                    group = groupId,
                    note = description,
                    unitid = unitId,

                    reserved_stock = 0,
                    is_inventory_p = "Y",
                    is_changeprice_p = "N",
                    materials = "",
                    picture = selectedImagePath,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    deleted_at = null,
                    supplier_id = 1,
                    flag = 1
                };

                int? insertedItemId = itemController.InsertItem(item);


                if (insertedItemId == null)
                {
                    MessageBox.Show("Failed to save item.");
                    return;
                }


                // Insert unit variants
                foreach (UnitVariant variant in unitVariantsFromForm)
                {
                    bool variantResult = itemController.InsertUnitVariant(insertedItemId.Value, variant);
                    if (!variantResult)
                    {
                        MessageBox.Show("Failed to insert one of the unit variants.");
                    }
                }


                // 3️⃣ log stock adjustment untuk produk baru
                _stockService.LogAdjustment(
                    itemId: insertedItemId.Value,
                    adjustmentType: "NEW_ITEM",
                    oldStock: 0,
                    newStock: stock,
                    reason: "New product created",
                    referenceId: insertedItemId.Value,
                    referenceTable: "items",
                    userId: 1 // ganti dengan user login
                );

                _activityService.LogAction(
                userId: SessionUser.GetCurrentUser().UserId.ToString(),
                actionType: "ADD_ITEM",
                    referenceId: null,
                    details: new
                    {
                        itemId = insertedItemId.Value,
                        adjustmentType = "ADD_ITEM",
                        reason = "default reason",
                        referenceId = insertedItemId.Value,
                        referenceTable = "items",
                        terminal = SessionUser.GetCurrentUser().TerminalId,
                        shiftId = SessionUser.GetCurrentUser().ShiftId,
                        IpAddress = NetworkHelper.GetLocalIPAddress(),
                        UserAgent = GlobalContext.getAppVersion(),
                        userId = SessionUser.GetCurrentUser().UserId.ToString(),

                        //TsCode = transaction.TsCode,
                        //TotalAmount = transaction.TsTotal,
                        //PaymentMethod = transaction.TsMethod,
                        //OrderId = transaction.OrderId
                    }
                );


                MessageBox.Show("Item and unit variants saved successfully!");
                LoadItems();
                ClearInputs();
                unitVariantsFromForm.Clear(); // Clear after saving

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                var sessionUser = SessionUser.GetCurrentUser();
                if (dataGridView1.CurrentRow == null) return;

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

                // simpan stok lama buat perbandingan
                int oldStock = Convert.ToInt32(dataGridView1.CurrentRow.Cells["stock"].Value);

                string name = txtName.Text.Trim();
                decimal buyPrice = decimal.Parse(txtBuyPrice.Text);
                decimal sellPrice = decimal.Parse(txtSellPrice.Text);
                int stock = int.Parse(txtStock.Text);
                string barcode = txtBarcode.Text.Trim();
                int unitId = Convert.ToInt32(cmbUnit.SelectedValue);
                int groupId = Convert.ToInt32(cmbGroup.SelectedValue);
                string description = txtDescription.Text.Trim();

                bool result = itemController.UpdateItem(new Item
                {
                    id = id,
                    name = name,
                    buy_price = buyPrice,
                    sell_price = sellPrice,
                    stock = stock,
                    barcode = barcode,
                    unitid = unitId,
                    group = groupId,
                    note = description,
                    reserved_stock = 0,
                    is_inventory_p = "Y",
                    is_changeprice_p = "N",
                    materials = "",
                    picture = selectedImagePath,
                    updated_at = DateTime.Now,
                    deleted_at = null,
                    supplier_id = 0,
                    flag = 1
                });

                if (result)
                {
                    // 1️⃣ catat ke stock_adjustment (kalau ada perubahan stok)
                    if (oldStock != stock)
                    {
                        _stockService.LogAdjustment(
                            itemId: id,
                            adjustmentType: "UPDATE_ITEM",
                            oldStock: oldStock,
                            newStock: stock,
                            reason: "Admin update product",
                            referenceId: id,
                            referenceTable: "items",
                            userId: sessionUser.UserId
                        );
                    }


                    
                    _activityService.LogAction(
                   userId: sessionUser.UserId.ToString(),
                   actionType: "ITEM_UPDATE",
                   referenceId: id,
                   details: new
                   {
                       itemId = id,
                       adjustmentType = "UPDATE_ITEM",
                       oldStock =  oldStock,
                       newStock =  stock,
                       reason =  "default reason",
                       referenceId = id,
                       referenceTable = "items",
                       terminal = sessionUser.TerminalId,
                       shiftId = sessionUser.ShiftId,
                       IpAddress = NetworkHelper.GetLocalIPAddress(),
                       UserAgent = GlobalContext.getAppVersion(),
                       userId = sessionUser.UserId.ToString(),

                       //TsCode = transaction.TsCode,
                       //TotalAmount = transaction.TsTotal,
                       //PaymentMethod = transaction.TsMethod,
                       //OrderId = transaction.OrderId
                   }
               );

                    // Delete all previous variants
                    itemController.DeleteUnitVariantsByItemId(id);

                    // Re-insert updated variants
                    foreach (UnitVariant variant in unitVariantsFromForm)
                    {
                        itemController.InsertUnitVariant(id, variant);
                    }

                    MessageBox.Show("Item updated successfully!");
                    LoadItems();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Failed to update item.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null) return;

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

                var confirm = MessageBox.Show("Are you sure to delete this item?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm != DialogResult.Yes) return;

                bool result = itemController.DeleteItem(id);

                if (result)
                {
                    MessageBox.Show("Item deleted.");

                    _activityService.LogAction(
                        userId: SessionUser.GetCurrentUser().UserId.ToString(),
                        actionType: "DELETE_ITEM",
                        referenceId: id,
                        details: new
                        {
                            itemId = id,
                            adjustmentType = "DELETE_ITEM",
                            reason = "default reason",
                            referenceId = id,
                            referenceTable = "items",
                            terminal = SessionUser.GetCurrentUser().TerminalId,
                            shiftId = SessionUser.GetCurrentUser().ShiftId,
                            IpAddress = NetworkHelper.GetLocalIPAddress(),
                            UserAgent = GlobalContext.getAppVersion(),
                            userId = SessionUser.GetCurrentUser().UserId.ToString(),

                            //TsCode = transaction.TsCode,
                            //TotalAmount = transaction.TsTotal,
                            //PaymentMethod = transaction.TsMethod,
                            //OrderId = transaction.OrderId
                        }
                    );
                    // Kalau mau, bisa juga catat adjustment = stock menjadi 0
                    _stockService.LogAdjustment(
                        itemId: id,
                        adjustmentType: "DELETE_ITEM",
                        oldStock: Convert.ToInt32(dataGridView1.CurrentRow.Cells["stock"].Value),
                        newStock: 0,
                        reason: "Product deleted",
                        referenceId: id,
                        referenceTable: "items",
                        userId: 1 // ganti dengan user login
                    );


                    LoadItems();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Failed to delete item.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtName.Text = row.Cells["name"].Value.ToString();
                txtBuyPrice.Text = row.Cells["buy_price"].Value.ToString();
                txtSellPrice.Text = row.Cells["sell_price"].Value.ToString();
                txtStock.Text = row.Cells["stock"].Value.ToString();
                txtBarcode.Text = row.Cells["barcode"].Value.ToString();
                cmbUnit.SelectedValue = Convert.ToInt32(row.Cells["unit_id"].Value);
                cmbGroup.SelectedValue = row.Cells["group"].Value;
                txtDescription.Text = row.Cells["note"].Value.ToString();

                // Load image from saved path
                string pictureFile = row.Cells["picture"].Value?.ToString();

                if (!string.IsNullOrEmpty(pictureFile))
                {
                    string imagePath = Path.Combine(Application.StartupPath, pictureFile); // pictureFile should already include "images/filename.jpg"

                    if (File.Exists(imagePath))
                    {
                        pictureBox.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        pictureBox.Image = null;
                    }
                }
                else
                {
                    pictureBox.Image = null;
                }


                int itemid = Convert.ToInt32(row.Cells["id"].Value);

                // get unit variant
                unitVariantsFromForm = itemController.GetUnitVariant(itemid);

                SetFormMode(true); // You're now in 'edit' mode

            }
        }



        private void ClearInputs()
        {
            txtName.Clear();
            txtBuyPrice.Clear();
            txtSellPrice.Clear();
            txtStock.Clear();
            txtBarcode.Clear();
            cmbUnit.SelectedIndex = -1;
            cmbGroup.SelectedIndex = -1;
            txtDescription.Clear();
            pictureBox.Image = null;
            selectedImagePath = "";

            SetFormMode(false); // Set to 'new entry' mode
        }

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
                pictureBox.Image = Image.FromFile(destinationPath);
            }
        }

        //private void CheckUnitVariantEligibility()
        //{
        //    // Assuming txtItemName and cmbUnit are your controls
        //    bool isItemNameValid = !string.IsNullOrWhiteSpace(txtName.Text);
        //    bool isUnitSelected = cmbUnit.SelectedIndex >= 0;

        //    btnUnitVariant.Enabled = isItemNameValid && isUnitSelected;
        //}


        private void btnUnitVariant_Click(object sender, EventArgs e)
        {
            bool isItemNameValid = !string.IsNullOrWhiteSpace(txtName.Text);
            bool isUnitSelected = cmbUnit.SelectedIndex >= 0;

            if (!isItemNameValid || !isUnitSelected)
            {
                btnUnitVariant.Cursor = Cursors.No; // Stop sign cursor
                MessageBox.Show("Please enter item name and select a unit first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnUnitVariant.Cursor = Cursors.Default;

            int itemId = 2; // Replace with how you actually get it
            string baseUnitName = cmbUnit.Text; // get selected base unit name
            using (var variantForm = new UnitVariantForm(itemId, baseUnitName, unitVariantsFromForm))
            {
                var result = variantForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    unitVariantsFromForm = variantForm.UnitVariants;
                }
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClearInputs(); // Clears form & resets mode
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
                    ImportItemsFromExcel(txtFile.Text);
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



    }
}
