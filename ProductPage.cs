
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

namespace POS_qu
{
    public partial class ProductPage : Form
    {
        private ItemController itemController;
        private string selectedImagePath = "";
        private DataGridViewManager dgvManager;
        private List<UnitVariant> unitVariantsFromForm = new List<UnitVariant>(); // Store globally if needed

        public ProductPage()
        {
            InitializeComponent();
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
                string name = txtName.Text.Trim();
                decimal buyPrice = decimal.Parse(txtBuyPrice.Text);
                decimal sellPrice = decimal.Parse(txtSellPrice.Text);
                int stock = int.Parse(txtStock.Text);
                string barcode = txtBarcode.Text.Trim();
                int unitId = Convert.ToInt32(cmbUnit.SelectedValue);
                int groupId = Convert.ToInt32(cmbGroup.SelectedValue);
                string description = txtDescription.Text.Trim();

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
            MessageBox.Show("btnUpdate_Click ");
            try
            {
                if (dataGridView1.CurrentRow == null) return;

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

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
                    unit = unitId.ToString(),
                    group = groupId,
                    note = description,
                    reserved_stock = 0, // or pull from another control if needed
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
                    // Delete all previous variants
                    itemController.DeleteUnitVariantsByItemId(id);

                    // Re-insert updated variants
                    foreach (UnitVariant variant in unitVariantsFromForm)
                    {
                        bool variantResult = itemController.InsertUnitVariant(id, variant);
                        if (!variantResult)
                        {
                            MessageBox.Show("Failed to update one of the unit variants.");
                        }
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
                cmbUnit.SelectedValue = Convert.ToInt32(row.Cells["unit"].Value);
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
    }
}
