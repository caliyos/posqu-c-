using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using POS_qu.Controllers;
using POS_qu.Helpers;
using POS_qu.Models;

namespace POS_qu
{
    public partial class SearchFormItem : Form
    {
        private ItemController itemController;
        public Item SelectedItem { get; private set; }

        private DataTable itemsDataTable;
        private DataGridViewManager dgvManager;

        public SearchFormItem(string searchTerm)
        {
            InitializeComponent();
            itemController = new ItemController();

            // Calculate 90% of the screen width
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            this.Width = (int)(screenWidth * 0.9); // Set the form width to 90% of the screen width

            // Optionally, set the height if needed. For example, 80% of the screen height.
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Height = (int)(screenHeight * 0.8); // Adjust the height as needed

            // Dapatkan data dari database
            itemsDataTable = itemController.GetItems(searchTerm);

            // DataGridView setup
            dataGridViewSearchResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSearchResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSearchResults.KeyDown += DataGridViewSearchResults_KeyDown;

            // DataGridView Manager setup
            dgvManager = new DataGridViewManager(dataGridViewSearchResults, itemsDataTable, 10);
            dgvManager.PagingInfoLabel = lblPageNumber; // Optional label for paging info

            // Bind buttons to paging methods
            btnNext.Click += btnNext_Click;
            btnPrevious.Click += btnPrevious_Click;
            btnFirstPage.Click += btnFirstPage_Click;
            btnLastPage.Click += btnLastPage_Click;

            // Dropdown page size
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            cmbPageSize.SelectedIndex = 0;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

            // Tombol search
            btnSearch.Click += btnSearch_Click;

            // Handle the DataBindingComplete event
            dataGridViewSearchResults.DataBindingComplete += (sender, e) =>
            {
                SetColumnWidths();
            };
        }

        // Method to set specific column widths
        private void SetColumnWidths()
        {


            ////// For other columns, set auto-sizing or hide if not relevant
            ////SetAutoSizeForColumn("name");
            ////SetAutoSizeForColumn("buy_price");
            ////SetAutoSizeForColumn("sell_price");
            ////SetAutoSizeForColumn("barcode");
            ////SetAutoSizeForColumn("stock");
            ////SetAutoSizeForColumn("reserved_stock");
            //SetAutoSizeForColumn("group");
            //SetAutoSizeForColumn("is_inventory_p");
            //SetAutoSizeForColumn("is_changeprice_p");
            //SetAutoSizeForColumn("materials");
            //SetAutoSizeForColumn("note");
            //SetAutoSizeForColumn("picture");
            //SetAutoSizeForColumn("created_at");
            //SetAutoSizeForColumn("updated_at");
            //SetAutoSizeForColumn("deleted_at");
            //SetAutoSizeForColumn("supplier_id");
            //SetAutoSizeForColumn("flag");

            //// Manually set a width for important columns
            //dataGridViewSearchResults.Columns["name"].Width = 300;
            //dataGridViewSearchResults.Columns["buy_price"].Width = 150;
            //dataGridViewSearchResults.Columns["sell_price"].Width = 150;
            //dataGridViewSearchResults.Columns["barcode"].Width = 250;
            //dataGridViewSearchResults.Columns["stock"].Width = 100;
            //dataGridViewSearchResults.Columns["reserved_stock"].Width = 100;
            dataGridViewSearchResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }

        // Helper method to set AutoSize for less important columns
        private void SetAutoSizeForColumn(string columnName)
        {
            if (dataGridViewSearchResults.Columns.Contains(columnName))
            {
                dataGridViewSearchResults.Columns[columnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        // Event Handlers for paging buttons
        private void btnNext_Click(object sender, EventArgs e)
        {
            dgvManager.NextPage();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            dgvManager.PreviousPage();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            dgvManager.FirstPage();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            dgvManager.LastPage();
        }

        // Event Handler for changing page size
        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedSize = Convert.ToInt32(cmbPageSize.SelectedItem);
            dgvManager.SetPageSize(selectedSize);
        }

        // Event Handler for Search button
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;

            // Dapatkan data baru dari database
            itemsDataTable = itemController.GetItems(searchTerm);

            // Reset data di DataGridViewManager
            dgvManager.Reset(itemsDataTable);
        }

        // Event Handler for selecting a row and opening QuantityForm
        private void DataGridViewSearchResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (dataGridViewSearchResults.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row.");
                return;
            }

            var selectedRow = dataGridViewSearchResults.SelectedRows[0];
            var dataRowView = selectedRow.DataBoundItem as DataRowView;

            if (dataRowView == null)
            {
                MessageBox.Show("No valid row selected.");
                return;
            }

            try
            {
                int productId = Convert.ToInt32(dataRowView["id"]);
                int stock = Convert.ToInt32(dataRowView["stock"]);
                string name = dataRowView["name"].ToString();
                List<UnitVariant> unitVariants = itemController.GetUnitVariant(productId);

                using (var quantityForm = new QuantityForm(stock, unitVariants))
                {
                    if (unitVariants == null || unitVariants.Count == 0)
                        quantityForm.ShowNoUnitVariantMessage();
                    else
                        quantityForm.ShowUnitVariants(unitVariants);

                    if (quantityForm.ShowDialog() != DialogResult.OK) return;

                    int quantity = quantityForm.Quantity;
                    UnitVariant selectedUnit = quantityForm.SelectedUnitVariant;

                    if (selectedUnit == null && unitVariants.Count > 0)
                    {
                        MessageBox.Show("Please select a valid unit variant.");
                        return;
                    }

                    int stockNeeded = quantity * (selectedUnit?.Conversion ?? 1);

                    if (stockNeeded > stock)
                        throw new InvalidOperationException("Insufficient stock. Cannot proceed with the transaction.");

                    string barcode = dataRowView["barcode"].ToString();
                    int reserved_stock = itemController.GetItemReservedStock(barcode);

                    int new_reserved_stock = reserved_stock + stockNeeded;
                    if (new_reserved_stock > stock)
                        throw new InvalidOperationException("Stock sudah terpakai.");

                    itemController.UpdateReservedStock(barcode, new_reserved_stock);

                    // getprice asli
                    decimal realprice = itemController.GetItemPrice(productId);

                    SelectedItem = new Item
                    {
                        id = productId,
                        barcode = barcode,
                        name = name,
                        stock = quantity,                          // qty user pilih
                        unit = selectedUnit?.UnitName ?? "pcs",    // unit name
                        conversion = selectedUnit?.Conversion ?? 1, // default 1 kalau tidak ada unit variant
                        sell_price = selectedUnit?.SellPrice ?? Convert.ToDecimal(dataRowView["sell_price"]),
                        price_per_pcs = selectedUnit != null
                     ? Math.Round(selectedUnit.SellPrice / selectedUnit.Conversion, 2)
                     : selectedUnit?.SellPrice ?? Convert.ToDecimal(dataRowView["sell_price"]),
                        price_per_pcs_asli = realprice
                    };



                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
