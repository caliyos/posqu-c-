/* 
 * using System;
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


namespace POS_qu
{
    public partial class Form4_crud : Form
    {

        public Form4_crud()
        {
            InitializeComponent();
            SetupDataGridViewColumns();
            //SetupCartTotalPanel();
            txtCariBarang.KeyDown += TxtCariBarang_KeyDown;

        }

        private void TxtCariBarang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchTerm = txtCariBarang.Text.Trim();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    // Open the SearchForm as a modal dialog
                    using (var searchForm = new SearchFormItem(searchTerm))
                    {
                        if (searchForm.ShowDialog() == DialogResult.OK)
                        {
                            // Get the selected item from the SearchForm
                            var selectedItem = searchForm.SelectedItem;
                            MessageBox.Show("EHEERE Selected Item: " + selectedItem.name);

                            if (selectedItem != null)
                            {
                                // Display the selected item's name
                                MessageBox.Show("Selected Item: " + selectedItem.name);

                                // Add the selected item to dataGridViewCart
                                dataGridViewCart4.Rows.Add(
                                    selectedItem.barcode,
                                    selectedItem.name,
                                    selectedItem.stock,
                                    selectedItem.unit,
                                    //selectedItem.buy_price, 
                                    selectedItem.sell_price

                                    );
                                CalculateAllTotals();
                            }
                            else
                            {
                                MessageBox.Show("No item selected.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Dialog closed without selecting an item.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a search term.");
                }
            }
        }
        private void SetupDataGridViewColumns()
        {
            // Clear existing columns
            dataGridViewCart4.Columns.Clear();

            // Add columns with DataPropertyName set to the corresponding property in the Item class
            //dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            //{

            //    HeaderText = "No",
            //    Name = "No",
            //    ReadOnly = true    // Make the column read-only
            //});

            dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "kode_item",
                HeaderText = "Kode Item",
                Name = "kode_item",
                ReadOnly = true
            });

            dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "name",
                HeaderText = "Nama Barang",
                Name = "name",
                ReadOnly = true
            });

            //dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "keterangan",
            //    HeaderText = "Keterangan",
            //    Name = "keterangan"
            //});

            //dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            //{
            //    DataPropertyName = "jenis",
            //    HeaderText = "Jenis",
            //    Name = "jenis"
            //});

            dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "stock",
                HeaderText = "qty",
                Name = "stock"
            });

            dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "unit",
                HeaderText = "Satuan",
                Name = "unit",
                ReadOnly = true
            });
            dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "sell_price",
                HeaderText = "Harga",
                Name = "sell_price",
                ReadOnly = true
            });
            dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "discount",
                HeaderText = "Pot(%)",
                Name = "discount"
            });
            dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "tax",
                HeaderText = "Pajak",
                Name = "tax"
            });
            dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "total",
                HeaderText = "Total",
                Name = "total",
                ReadOnly = true
            });

            // Bind the list to the DataGridView
            //dataGridViewCart.DataSource = items;
            dataGridViewCart4.RowPostPaint += dataGridViewCart4_RowPostPaint;
            dataGridViewCart4.CellValueChanged += DataGridViewCart4_CellValueChanged;


        }
        private void DataGridViewCart4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Only handle changes in the 'stock' (qty) column
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewCart4.Columns["stock"].Index)
            {
                DataGridView dgv = (DataGridView)sender;
                var barcode = dgv.Rows[e.RowIndex].Cells["kode_item"].Value.ToString();
                var stockCell = dgv.Rows[e.RowIndex].Cells["stock"];
                int enteredQuantity = Convert.ToInt32(stockCell.Value);

                var itemController = new ItemController();

                // Step 1: Get the current stock from the database
                int currentStock = itemController.GetItemStock(barcode);

                // Step 2: Restore previous cart stock to the database
                int previousCartStock = GetPreviousCartStock(stockCell);
                RestoreStockToDatabase(itemController, barcode, currentStock, previousCartStock);

                // Step 3: Validate and update stock
                int updatedStock = itemController.GetItemStock(barcode); // Get fresh stock after restoring
                UpdateStock(itemController, barcode, updatedStock, enteredQuantity, stockCell, e.RowIndex);
            }
        }

        private int GetPreviousCartStock(DataGridViewCell stockCell)
        {
            object previousStockObj = stockCell.Tag;
            return (previousStockObj != null) ? Convert.ToInt32(previousStockObj) : 0;
        }

        private void RestoreStockToDatabase(ItemController itemController, string barcode, int currentStock, int previousCartStock)
        {
            if (previousCartStock > 0)
            {
                int restoredStock = currentStock + previousCartStock;
                bool restoreSuccess = itemController.UpdateItemStock(barcode, restoredStock);

                if (!restoreSuccess)
                {
                    MessageBox.Show("Failed to restore the previous stock to the database.");
                }
            }
        }

        private void UpdateStock(ItemController itemController, string barcode, int updatedStock, int enteredQuantity, DataGridViewCell stockCell, int rowIndex)
        {
            if (enteredQuantity > updatedStock)
            {
                MessageBox.Show($"You cannot enter more than the available stock. Available stock: {updatedStock}");
                stockCell.Value = updatedStock;  // Reset to max available stock
            }
            else
            {
                bool updateSuccess = itemController.UpdateItemStock(barcode, updatedStock - enteredQuantity);
                if (updateSuccess)
                {
                    stockCell.Tag = enteredQuantity; // Store new cart stock for future use
                    RecalculateTotalForRow(rowIndex);
                    CalculateAllTotals();
                }
                else
                {
                    MessageBox.Show("Failed to update stock in the database.");
                }
            }
        }




        private void RecalculateTotalForRow(int rowIndex)
        {
            var qtyCell = dataGridViewCart4.Rows[rowIndex].Cells["stock"];
            var priceCell = dataGridViewCart4.Rows[rowIndex].Cells["sell_price"];
            var discountCell = dataGridViewCart4.Rows[rowIndex].Cells["discount"];
            var taxCell = dataGridViewCart4.Rows[rowIndex].Cells["tax"];
            var totalCell = dataGridViewCart4.Rows[rowIndex].Cells["total"];

            if (qtyCell.Value != null && priceCell.Value != null &&
                int.TryParse(qtyCell.Value.ToString(), out int qty) &&
                decimal.TryParse(priceCell.Value.ToString(), out decimal price))
            {
                decimal discountAmount = 0;
                decimal taxAmount = 0;

                // Check if discount value is present and valid
                if (discountCell.Value != null && decimal.TryParse(discountCell.Value.ToString(), out decimal discount))
                {
                    // Calculate the discount amount
                    discountAmount = (qty * price) * (discount / 100);
                }

                // Check if tax value is present and valid
                if (taxCell.Value != null && decimal.TryParse(taxCell.Value.ToString(), out decimal tax))
                {
                    // Calculate the tax amount
                    taxAmount = (qty * price) * (tax / 100);
                }

                // Calculate the total: (Qty * Price) - Discount + Tax
                decimal total = (qty * price) - discountAmount + taxAmount;

                // Set the calculated total in the 'total' column
                totalCell.Value = total;
            }
        }



        //private void DataGridViewCart4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    // Only calculate the total if the changed cell is in the quantity, price, discount, or tax column
        //    if (e.RowIndex >= 0)
        //    {
        //        DataGridView dgv = (DataGridView)sender;

        //        var qtyCell = dgv.Rows[e.RowIndex].Cells["stock"];
        //        var priceCell = dgv.Rows[e.RowIndex].Cells["sell_price"];
        //        var discountCell = dgv.Rows[e.RowIndex].Cells["discount"];
        //        var taxCell = dgv.Rows[e.RowIndex].Cells["tax"];
        //        var totalCell = dgv.Rows[e.RowIndex].Cells["total"];

        //        if (qtyCell.Value != null && priceCell.Value != null &&
        //            int.TryParse(qtyCell.Value.ToString(), out int qty) &&
        //            decimal.TryParse(priceCell.Value.ToString(), out decimal price))
        //        {
        //            decimal discountAmount = 0;
        //            decimal taxAmount = 0;

        //            // Check if discount value is present and valid
        //            if (discountCell.Value != null && decimal.TryParse(discountCell.Value.ToString(), out decimal discount))
        //            {
        //                // Calculate the discount amount
        //                discountAmount = (qty * price) * (discount / 100);
        //            }

        //            // Check if tax value is present and valid
        //            if (taxCell.Value != null && decimal.TryParse(taxCell.Value.ToString(), out decimal tax))
        //            {
        //                // Calculate the tax amount
        //                taxAmount = (qty * price) * (tax / 100);
        //            }

        //            // Calculate the total: (Qty * Price) - Discount + Tax
        //            decimal total = (qty * price) - discountAmount + taxAmount;

        //            // Update the total cell
        //            totalCell.Value = total;
        //            CalculateAllTotals();
        //        }
        //    }
        //}

        private void CalculateAllTotals()
        {
            decimal totalCart = 0; // Initialize the total sum of all row totals

            // Loop through all rows to calculate totals for the first time when the DataGridView is loaded
            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
            {
                var qtyCell = row.Cells["stock"];
                var priceCell = row.Cells["sell_price"];
                var discountCell = row.Cells["discount"];
                var taxCell = row.Cells["tax"];
                var totalCell = row.Cells["total"];

                if (qtyCell.Value != null && priceCell.Value != null &&
                    int.TryParse(qtyCell.Value.ToString(), out int qty) &&
                    decimal.TryParse(priceCell.Value.ToString(), out decimal price))
                {
                    decimal discountAmount = 0;
                    decimal taxAmount = 0;

                    // Check if discount value is present and valid
                    if (discountCell.Value != null && decimal.TryParse(discountCell.Value.ToString(), out decimal discount))
                    {
                        // Calculate the discount amount
                        discountAmount = (qty * price) * (discount / 100);
                    }

                    // Check if tax value is present and valid
                    if (taxCell.Value != null && decimal.TryParse(taxCell.Value.ToString(), out decimal tax))
                    {
                        // Calculate the tax amount
                        taxAmount = (qty * price) * (tax / 100);
                    }

                    // Calculate the total: (Qty * Price) - Discount + Tax
                    decimal total = (qty * price) - discountAmount + taxAmount;

                    // Set the calculated total in the 'total' column
                    totalCell.Value = total;

                    // Add the row total to the cart total
                    totalCart += total;
                }
            }

            // Update the grand total label
            label2.Text = totalCart.ToString("N2");
        }


        private void dataGridViewCart4_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Get the DataGridView
            var dgv = (DataGridView)sender;

            // Create a rectangle to draw the row number
            Rectangle rect = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dgv.RowHeadersWidth, e.RowBounds.Height);

            // Draw the row number in the first column
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgv.Font, rect, Color.Black, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridViewCart4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private Panel panelTotalCart;
        private Label lblTotalCart;

        //private void SetupCartTotalPanel()
        //{
        //    // Create a panel to display the total
        //    panelTotalCart = new Panel();
        //    panelTotalCart.Location = new Point(10, 500); // Position it appropriately
        //    panelTotalCart.Size = new Size(320, 60);  // Size of the panel

        //    // Set the panel's background color and border
        //    panelTotalCart.BackColor = Color.LightGray;  // Background color
        //    panelTotalCart.BorderStyle = BorderStyle.FixedSingle;  // Border around the panel

        //    // Create a label to display the total inside the panel
        //    lblTotalCart = new Label();
        //    lblTotalCart.Font = new Font("Arial", 24, FontStyle.Bold);  // Large font
        //    lblTotalCart.ForeColor = Color.Black;
        //    lblTotalCart.Location = new Point(10, 15);  // Position the label inside the panel
        //    lblTotalCart.Size = new Size(300, 40);  // Set label size
        //    lblTotalCart.Text = "Total: 0";  // Initial text

        //    // Add the label to the panel
        //    panelTotalCart.Controls.Add(lblTotalCart);

        //    // Add the panel to the form's controls
        //    this.Controls.Add(panelTotalCart);
        //}


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            // Get the total from the main form (e.g., txtTotalCart)
            decimal totalAmount = Convert.ToDecimal(label2.Text);

            // Create and show the payment modal
            PaymentModalForm paymentModal = new PaymentModalForm(totalAmount);

            // Show the modal as a dialog (blocking, user must close it to proceed)
            DialogResult result = paymentModal.ShowDialog();

            // If the user clicked 'Pay' (DialogResult is OK)
            if (result == DialogResult.OK)
            {
                decimal paymentAmount = paymentModal.PaymentAmount;
                decimal cashback = paymentModal.Cashback;
                string paymentMethod = paymentModal.PaymentMethod;

                // Create a new Payment object
                Payment payment = new Payment(paymentAmount, paymentMethod, DateTime.Now);

                // Create the controller and insert the payment
                ItemController controller = new ItemController();
                controller.InsertPayment(payment);

                // Optionally, show a success message
                MessageBox.Show("Payment successful!");

                // You can reset the cart or perform other actions after payment
            }
        }

        private void Form4_crud_Load(object sender, EventArgs e)
        {

        }
    }
}
*/