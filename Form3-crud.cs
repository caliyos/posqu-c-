using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using System.Xml.Linq;
using POS_qu.Models; // Import your model namespace


namespace POS_qu
{
    public partial class Form3_crud : Form
    {
        // Create a list to store items
        private List<Item> items = new List<Item>();


        //private void dataGridViewCart_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    // Check if the row index is valid
        //    if (e.RowIndex < 0)
        //    {
        //        return; // Exit if the row index is invalid
        //    }

        //    // Get the clicked row
        //    var row = dataGridViewCart.Rows[e.RowIndex];

        //    // Get the values from the row
        //    var id = row.Cells["Id"].Value?.ToString();
        //    var name = row.Cells["Name"].Value?.ToString();
        //    var price = row.Cells["Price"].Value?.ToString();
        //    var noTransaksi = row.Cells["NoTransaksi"].Value?.ToString();
        //    var tanggal = row.Cells["Tanggal"].Value?.ToString();
        //    var pelanggan = row.Cells["Pelanggan"].Value?.ToString();

        //    // Display an alert with the row's data
        //    MessageBox.Show(
        //        $"Row Header Clicked:\n" +
        //        $"ID: {id}\n" +
        //        $"Name: {name}\n" +
        //        $"Price: {price}\n" +
        //        $"No Transaksi: {noTransaksi}\n" +
        //        $"Tanggal: {tanggal}\n" +
        //        $"Pelanggan: {pelanggan}",
        //        "Row Header Clicked",
        //        MessageBoxButtons.OK,
        //        MessageBoxIcon.Information
        //    );
        //}

        private void InitializeDataGridView()
        {
            // Set up DataGridView columns with data binding
            SetupDataGridViewColumns();

            //// Enable editing
            //dataGridViewCart.ReadOnly = false;

            //// Enable full row selection (optional)
            //dataGridViewCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //// Enable auto-sizing columns
            //dataGridViewCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            //// Enable sorting
            //foreach (DataGridViewColumn column in dataGridViewCart.Columns)
            //{
            //    column.SortMode = DataGridViewColumnSortMode.Automatic;
            //}

            //// Attach event handlers
            //dataGridViewCart.KeyDown += dataGridViewCart_KeyDown;
            //dataGridViewCart.CellEndEdit += dataGridViewCart_CellEndEdit;
        }

        //private void dataGridViewCart_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        // Check if a cell is selected
        //        if (dataGridViewCart.CurrentCell == null)
        //        {
        //            return; // Exit if no cell is selected
        //        }

        //        // Move to the next cell to the right instead of down
        //        int col = dataGridViewCart.CurrentCell.ColumnIndex;
        //        int row = dataGridViewCart.CurrentCell.RowIndex;

        //        if (col < dataGridViewCart.Columns.Count - 1)
        //        {
        //            // Move to the next cell in the same row
        //            dataGridViewCart.CurrentCell = dataGridViewCart.Rows[row].Cells[col + 1];
        //        }
        //        else if (row < dataGridViewCart.Rows.Count - 1)
        //        {
        //            // Move to the first cell in the next row
        //            dataGridViewCart.CurrentCell = dataGridViewCart.Rows[row + 1].Cells[0];
        //        }

        //        e.Handled = true; // Prevent default behavior
        //    }
        //}

        //private void dataGridViewCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    // Check if the row index is valid
        //    if (e.RowIndex < 0 || e.RowIndex >= dataGridViewCart.Rows.Count)
        //    {
        //        return; // Exit if the row index is invalid
        //    }

        //    // Check if the column index is valid
        //    if (e.ColumnIndex < 0 || e.ColumnIndex >= dataGridViewCart.Columns.Count)
        //    {
        //        return; // Exit if the column index is invalid
        //    }

        //    // Get the updated value
        //    var updatedValue = dataGridViewCart.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

        //    // Update the underlying data source (e.g., the `items` list)
        //    var item = items[e.RowIndex];
        //    switch (dataGridViewCart.Columns[e.ColumnIndex].Name)
        //    {
        //        case "Name":
        //            item.Name = updatedValue?.ToString();
        //            break;
        //        case "Price":
        //            if (decimal.TryParse(updatedValue?.ToString(), out decimal price))
        //            {
        //                item.Price = price;
        //            }
        //            break;
        //        case "NoTransaksi":
        //            item.NoTransaksi = updatedValue?.ToString();
        //            break;
        //        case "Tanggal":
        //            if (DateTime.TryParse(updatedValue?.ToString(), out DateTime tanggal))
        //            {
        //                item.Tanggal = tanggal;
        //            }
        //            break;
        //        case "Pelanggan":
        //            item.Pelanggan = updatedValue?.ToString();
        //            break;
        //    }

        //    // Debug: Print the updated item
        //    Debug.WriteLine($"Updated Item: Id={item.Id}, Name={item.Name}, Price={item.Price}, NoTransaksi={item.NoTransaksi}, Tanggal={item.Tanggal}, Pelanggan={item.Pelanggan}");
        //}
        private void SetupDataGridViewColumns()
        {
            // Clear existing columns
            dataGridViewCart.Columns.Clear();

            // Add columns with DataPropertyName set to the corresponding property in the Item class
            dataGridViewCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Name = "Id"
            });

            dataGridViewCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                Name = "Name"
            });

            dataGridViewCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Price",
                Name = "Price"
            });

            dataGridViewCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NoTransaksi",
                HeaderText = "No Transaksi",
                Name = "NoTransaksi"
            });

            dataGridViewCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Tanggal",
                HeaderText = "Tanggal",
                Name = "Tanggal"
            });

            dataGridViewCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Pelanggan",
                HeaderText = "Pelanggan",
                Name = "Pelanggan"
            });

            // Bind the list to the DataGridView
            //dataGridViewCart.DataSource = items;
        }


        public Form3_crud()
        {
            InitializeComponent();
            //dataGridViewCart.CellClick += dataGridViewCart_CellClick;
            //dataGridViewCart.RowHeaderMouseClick += dataGridViewCart_RowHeaderMouseClick;

            // Set up DataGridView columns with data binding
            InitializeDataGridView();

            // Set up DataGridView columns (if not done in the designer)
            dataGridViewCart.AutoGenerateColumns = false; // Disable auto-generation
            dataGridViewCart.Columns.Add("Id", "ID");
            dataGridViewCart.Columns.Add("Name", "Name");
            dataGridViewCart.Columns.Add("Price", "Price");
            dataGridViewCart.Columns.Add("NoTransaksi", "No Transaksi");
            dataGridViewCart.Columns.Add("Tanggal", "Tanggal");
            dataGridViewCart.Columns.Add("Pelanggan", "Pelanggan");


            //dataGridViewCart.DataSource = items;
            // Set up DateTimePicker to include time
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm"; // Date and time format
            dateTimePicker1.ShowUpDown = true; // Allow time selection

            // Set up ComboBox for Pelanggan
            comboBoxPelanggan.Items.AddRange(new string[] { "Pelanggan 1", "Pelanggan 2", "Pelanggan 3" });
            comboBoxPelanggan.DropDownStyle = ComboBoxStyle.DropDownList; // Prevent user from typing


        }

        private void Form3_crud_Load(object sender, EventArgs e)
        {

        }

        /* DIRECTLY INPUT FROM TEXT DANGER */
        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    // Get input from text boxes or other controls
        //    string noTransaksi = txtNoTransaksi.Text;
        //    string tanggal = dateTimePicker1.Value.ToString("dd/MM/yyyy");
        //    string pelanggan = comboBoxPelanggan.SelectedItem?.ToString();

        //    //        NoTransaksi = txtNoTransaksi.Text,
        //    //        Tanggal = dateTimePicker1.Value, // Get selected date and time
        //    //        Pelanggan = comboBoxPelanggan.SelectedItem?.ToString() // Get selected Pelanggan

        //    // Add a new row to the DataGridView
        //    dataGridViewCart.Rows.Add(noTransaksi, tanggal, pelanggan);

        //    // Clear input fields (optional)
        //    txtNoTransaksi.Clear();
        //    //txtTanggal.Clear();
        //    //txtPelanggan.Clear();
        //}

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Validate input (e.g., ensure Price is a valid decimal)
        //        if (!decimal.TryParse("10000", out decimal price))
        //        {
        //            MessageBox.Show("Invalid price format.");
        //            return;
        //        }

        //        // Validate ComboBox selection
        //        if (comboBoxPelanggan.SelectedIndex == -1)
        //        {
        //            MessageBox.Show("Please select a Pelanggan.");
        //            return;
        //        }

        //        // Create a new Item object from user input
        //        var newItem = new Item
        //        {
        //            Id = items.Count + 1, // Auto-generate ID (or use a better logic)
        //            Name = "TEST NAME",
        //            Price = price,
        //            NoTransaksi = txtNoTransaksi.Text,
        //            Tanggal = dateTimePicker1.Value, // Get selected date and time
        //            Pelanggan = comboBoxPelanggan.SelectedItem?.ToString() ?? "Unknown" // Handle null
        //        };

        //        // Debug: Inspect newItem properties
        //        Debug.WriteLine($"New Item: Id={newItem.Id}, Name={newItem.Name}, Price={newItem.Price}, NoTransaksi={newItem.NoTransaksi}, Tanggal={newItem.Tanggal}, Pelanggan={newItem.Pelanggan}");

        //        // Add the new item to the list
        //        items.Add(newItem);

        //        // Refresh the DataGridView
        //        dataGridViewCart.DataSource = null; // Clear the current data source
        //        dataGridViewCart.DataSource = items; // Rebind the updated list
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred: " + ex.Message);
        //    }
        //}

        private void dataGridViewCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    // Validate input (e.g., ensure Price is a valid decimal)
        //    //if (!decimal.TryParse(txtPrice.Text, out decimal price))
        //    if (!decimal.TryParse("10000", out decimal price))
        //    {
        //        MessageBox.Show("Invalid price format.");
        //        return;
        //    }

        //    // Create a new Item object from user input
        //    var newItem = new Item
        //    {
        //        Id = items.Count + 1, // Auto-generate ID (or use a better logic)
        //        //Name = txtName.Text,
        //        Name = "TEST NAME",
        //        Price = price,
        //        NoTransaksi = txtNoTransaksi.Text,
        //        Tanggal = dateTimePicker1.Value, // Get selected date and time
        //        Pelanggan = comboBoxPelanggan.SelectedItem?.ToString() // Get selected Pelanggan
        //    };

        //    // Debug: Inspect newItem properties
        //    Debug.WriteLine($"New Item: Id={newItem.Id}, Name={newItem.Name}, Price={newItem.Price}, NoTransaksi={newItem.NoTransaksi}, Tanggal={newItem.Tanggal}, Pelanggan={newItem.Pelanggan}");

        //    // Add the new item to the list
        //    items.Add(newItem);

        //    // Refresh the DataGridView
        //    dataGridViewCart.DataSource = null; // Clear the current data source
        //    dataGridViewCart.DataSource = items; // Rebind the updated list
        //}



        //private void dataGridViewCart_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    // Check if the row index is valid (ignore header clicks)
        //    if (e.RowIndex < 0 || e.ColumnIndex < 0)
        //    {
        //        // Alert if the click is on the header or empty area
        //        MessageBox.Show("Please click on a valid row.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }


        //    // Get the clicked row
        //    var row = dataGridViewCart.Rows[e.RowIndex];

        //    // Extract the row data
        //    var id = row.Cells["Id"].Value?.ToString();
        //    var name = row.Cells["Name"].Value?.ToString();
        //    var price = row.Cells["Price"].Value?.ToString();
        //    var noTransaksi = row.Cells["NoTransaksi"].Value?.ToString();
        //    var tanggal = row.Cells["Tanggal"].Value?.ToString();
        //    var pelanggan = row.Cells["Pelanggan"].Value?.ToString();

        //    // Display the row data in an alert
        //    MessageBox.Show(
        //        $"Row Data:\n" +
        //        $"ID: {id}\n" +
        //        $"Name: {name}\n" +
        //        $"Price: {price}\n" +
        //        $"No Transaksi: {noTransaksi}\n" +
        //        $"Tanggal: {tanggal}\n" +
        //        $"Pelanggan: {pelanggan}",
        //        "Row Clicked",
        //        MessageBoxButtons.OK,
        //        MessageBoxIcon.Information
        //    );

        //}
    }
}
