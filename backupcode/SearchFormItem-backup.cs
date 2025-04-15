/* 
 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_qu.Controllers;
using POS_qu.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace POS_qu
{



    public partial class SearchFormItem : Form
    {
        private ItemController itemController;
        public Item SelectedItem { get; private set; }

        private BindingSource bindingSource;
        private DataTable itemsDataTable;
        private int PageSize = 10; // Number of items per page
        private int currentPage = 1; // Current page number



        private void LoadItems()
        {
            DataTable dt = itemController.GetItems();
            dataGridViewSearchResults.DataSource = dt;
        }

        // Sample data (replace with your actual data source)
        //private List<Item> items = new List<Item>
        //{
        //    new Item { Id = 1, Name = "Spatu A", Price = 100000 },
        //    new Item { Id = 2, Name = "Spatu B", Price = 150000 },
        //    new Item { Id = 3, Name = "Spatu C", Price = 200000 }
        //};

        private void SetupSearchFilter(string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Filter the items by the search term
                var filteredItems = itemsDataTable.AsEnumerable()
                                                   .Where(row => row["name"].ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                                                   .CopyToDataTable();
                bindingSource.DataSource = filteredItems;
            }
            else
            {
                // If no search term is provided, bind all data
                bindingSource.DataSource = itemsDataTable;
            }

            // Optionally apply pagination after filtering
            ApplyPagination();
        }


        public SearchFormItem(string searchTerm)
        {
            InitializeComponent();
            itemController = new ItemController();
            bindingSource = new BindingSource();

            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            cmbPageSize.SelectedIndex = 0;  // Set default to 10 rows per page

            btnSearch.Click += btnSearch_Click; // If not added in the designer
            btnNext.Click += btnNext_Click;
            btnPrevious.Click += btnPrevious_Click;
            btnFirstPage.Click += BtnFirstPage_Click; // karna nama eventnya dimulai huruf besar
            btnLastPage.Click += BtnLastPage_Click;  // karna nama eventnya dimulai huruf besar



            LoadItems(searchTerm);
            //LoadItems();

            // Set up DataGridView properties
            dataGridViewSearchResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSearchResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSearchResults.KeyDown += DataGridViewSearchResults_KeyDown;

            // Bind the DataGridView to the BindingSource
            dataGridViewSearchResults.DataSource = bindingSource;
            //dataGridViewSearchResults.CellFormatting += DataGridViewSearchResults_CellFormatting;

            // Set up sorting and filtering functionality
            //SetupSearchFilter(searchTerm);
            // Initialize pagination controls
            UpdatePaginationControls();

            cmbPageSize.SelectedIndexChanged += CmbPageSize_SelectedIndexChanged;

        }

        private void LoadItems(string searchTerm)
        {
            // Fetch the items from the controller
            itemsDataTable = itemController.GetItems(searchTerm);

            // Bind the filtered data to the DataGridView
            bindingSource.DataSource = itemsDataTable;
            ApplyPagination(); // Optional: Apply pagination if needed

        }


        private void UpdatePaginationControls()
        {
            // Update the page number label
            lblPageNumber.Text = $"Page {currentPage}";

            // Update the row count label
            int totalRows = itemsDataTable.Rows.Count;
            lblRowCount.Text = $"Rows: {totalRows}";

            // Enable/Disable navigation buttons based on the current page
            btnNext.Enabled = (currentPage * PageSize) < itemsDataTable.Rows.Count;
            btnPrevious.Enabled = currentPage > 1;
            btnFirstPage.Enabled = currentPage > 1;
            btnLastPage.Enabled = (currentPage * PageSize) < itemsDataTable.Rows.Count;
        }


        private void DataGridViewSearchResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            try
            {
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

                int productId = Convert.ToInt32(dataRowView["id"]);
                int stock = Convert.ToInt32(dataRowView["stock"]);
                string name = dataRowView["name"].ToString();

                // Ambil unit variant dari database
                List<UnitVariant> unitVariants = itemController.GetUnitVariant(productId);

                // Jika tidak ada unit variant, tampilkan form quantity tanpa unit variant
                using (var quantityForm = new QuantityForm(stock, unitVariants))
                {
                    if (unitVariants == null || unitVariants.Count == 0)
                    {
                        // Tampilkan pesan jika tidak ada unit variant
                        quantityForm.ShowNoUnitVariantMessage(); // Menampilkan label pesan
                    }
                    else
                    {
                        quantityForm.ShowUnitVariants(unitVariants); // Tampilkan unit variants pada ComboBox
                    }

                    if (quantityForm.ShowDialog() != DialogResult.OK) return;

                    int quantity = quantityForm.Quantity;
                    UnitVariant selectedUnit = quantityForm.SelectedUnitVariant;

                    // Jika tidak ada unit variant yang dipilih, gunakan unit default
                    if (selectedUnit == null && unitVariants.Count > 0)
                    {
                        MessageBox.Show("Please select a valid unit variant.");
                        return;
                    }

                    // Hitung stok yang dibutuhkan berdasarkan konversi unit
                    int stockNeeded = quantity * selectedUnit?.Conversion ?? 1; // Default ke 1 jika tidak ada unit variant

                    if (stockNeeded > stock)
                        throw new InvalidOperationException("Insufficient stock. Cannot proceed with the transaction.");

                    // Mendapatkan reserved stock
                    string barcode = dataRowView["barcode"].ToString();
                    int reserved_stock = itemController.GetItemReservedStock(barcode);

                    if (reserved_stock <= 0)
                    {
                        itemController.UpdateReservedStock(barcode, stockNeeded);
                    }
                    else
                    {
                        int new_reserved_stock = reserved_stock + stockNeeded;
                        if (new_reserved_stock > stock)
                            throw new InvalidOperationException("Stock sudah terpakai.");
                        itemController.UpdateReservedStock(barcode, new_reserved_stock);
                    }

                    // Update SelectedItem dengan unit variant yang dipilih
                    SelectedItem = new Item
                    {
                        id = productId,
                        barcode = dataRowView["barcode"].ToString(),
                        name = name,
                        stock = quantity,
                        unit = selectedUnit?.UnitName ?? "pcs", // Gunakan unit default jika tidak ada unit variant
                        sell_price = selectedUnit?.SellPrice ?? Convert.ToDecimal(dataRowView["sell_price"]) // Gunakan harga jual default
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



        private void ApplyPagination()
        {
            // Check if there are any rows in the itemsDataTable
            if (itemsDataTable.Rows.Count == 0)
            {
                // Show an alert if no data is available
                MessageBox.Show("No data available to display.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Exit the method as there's no data to paginate
            }

            // KOLOM No dan row2 nya disini 
            // Ensure the "No" column exists in the DataTable
            if (!itemsDataTable.Columns.Contains("No"))
            {
                itemsDataTable.Columns.Add("No", typeof(int)); // Add a "No" column to the DataTable if not present
                                                               // Move the "No" column to the first position
                itemsDataTable.Columns["No"].SetOrdinal(0);
            }

            // Apply pagination by skipping and taking the appropriate rows
            var pageItems = itemsDataTable.AsEnumerable()
                                          .Skip((currentPage - 1) * PageSize)
                                          .Take(PageSize)
                                          .ToList(); // ToList to perform operations on the rows

            // If there are rows to paginate
            if (pageItems.Any())
            {
                // Create a new DataTable to hold the paginated rows
                DataTable pageItemsTable = itemsDataTable.Clone(); // Clone the schema (structure) of the original table

                // Add rows to the new DataTable
                foreach (var row in pageItems)
                {
                    pageItemsTable.ImportRow(row);
                }

                // Update the row numbers in the "No" column based on pagination

                //MessageBox.Show("pageItemsTable.Rows.Count." + pageItemsTable.Rows.Count);
                for (int i = 0; i < pageItemsTable.Rows.Count; i++)
                {
                    //MessageBox.Show("rows" + i);
                    pageItemsTable.Rows[i]["No"] = (currentPage - 1) * PageSize + i + 1;
                }

                // Bind the new DataTable to the BindingSource
                bindingSource.DataSource = pageItemsTable;
            }
            else
            {
                // If no rows for this page, set an empty DataTable
                bindingSource.DataSource = itemsDataTable.Clone(); // Create an empty table with the same structure
            }

            // Update pagination controls (e.g., disable/enable buttons)
            UpdatePaginationControls();

            // Ensure the "No" column appears first in the DataGridView
            if (dataGridViewSearchResults.Columns.Contains("No"))
            {
                dataGridViewSearchResults.Columns["No"].DisplayIndex = 0; // Set the "No" column to the first position in the DataGridView
            }
        }



        // First page button click handler
        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            ApplyPagination();

        }
        // Last page button click handler
        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            int totalRows = itemsDataTable.Rows.Count;
            int totalPages = (int)Math.Ceiling((double)totalRows / PageSize);
            currentPage = totalPages;
            ApplyPagination();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if ((currentPage * PageSize) < itemsDataTable.Rows.Count)
            {
                currentPage++;
                ApplyPagination();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ApplyPagination();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            MessageBox.Show("btnSearch_Click." + txtSearch.Text);
            var searchTerm = txtSearch.Text;
            LoadItems(searchTerm); // Reload items with the new search term
            ApplyPagination();
        }

        private void dataGridViewSearchResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Sort the data when a column header is clicked
            string columnName = dataGridViewSearchResults.Columns[e.ColumnIndex].Name;
            if (bindingSource.Sort == $"{columnName} ASC")
            {
                bindingSource.Sort = $"{columnName} DESC";
            }
            else
            {
                bindingSource.Sort = $"{columnName} ASC";
            }
        }

        // ComboBox change event to change the page size
        // ComboBox change event for page size selection
        private void CmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedItem != null)
            {
                int selectedPageSize;
                if (int.TryParse(cmbPageSize.SelectedItem.ToString(), out selectedPageSize))
                {
                    PageSize = selectedPageSize;
                    currentPage = 1;
                    ApplyPagination();
                }
            }
        }


        private void SearchFormItem_Load(object sender, EventArgs e)
        {

        }

        private void btnNext_Click_1(object sender, EventArgs e)
        {

        }

        private void SearchFormItem_Load_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}

 */