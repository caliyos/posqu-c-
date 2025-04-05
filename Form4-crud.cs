using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using POS_qu.Controllers;
using POS_qu.Models;
using System.Diagnostics; // Add this at the top

namespace POS_qu
{
    public partial class Form4_crud : Form
    {
        private ItemController itemController;

        public Form4_crud()
        {
            InitializeComponent();
            itemController = new ItemController();
            SetupDataGridView();
            txtCariBarang.KeyDown += TxtCariBarang_KeyDown;
        }

        private void TxtCariBarang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchAndAddItem();
            }   
        }

        //private void SearchAndAddItem()
        //{
        //    string searchTerm = txtCariBarang.Text.Trim();
        //    if (string.IsNullOrEmpty(searchTerm))
        //    {
        //        MessageBox.Show("Please enter a search term.");
        //        return;
        //    }

        //    using (var searchForm = new SearchFormItem(searchTerm))
        //    {
        //        if (searchForm.ShowDialog() == DialogResult.OK && searchForm.SelectedItem != null)
        //        {
        //            var selectedItem = searchForm.SelectedItem;
        //            dataGridViewCart4.Rows.Add(selectedItem.id,selectedItem.barcode, selectedItem.name, selectedItem.stock, selectedItem.unit, selectedItem.sell_price,null ,null , selectedItem.stock * selectedItem.sell_price);
        //            CalculateAllTotals();
        //        }
        //        else
        //        {
        //            MessageBox.Show("No item selected.");
        //        }
        //    }
        //}

        private void SearchAndAddItem()
        {
            string searchTerm = txtCariBarang.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            using (var searchForm = new SearchFormItem(searchTerm))
            {
                if (searchForm.ShowDialog() == DialogResult.OK && searchForm.SelectedItem != null)
                {
                    var selectedItem = searchForm.SelectedItem;

                    // Try to update existing row
                    if (UpdateExistingItem(selectedItem))
                    {
                        MessageBox.Show("Item quantity updated.");
                        return;
                    }

                    // Insert new transaction into the database
                    if (!InsertPendingTransaction(selectedItem))
                    {
                        MessageBox.Show("Failed to insert transaction.");
                        return;
                    }

                    // Add a new row in DataGridView
                    AddNewItemToGrid(selectedItem);
                    MessageBox.Show("Transaction inserted successfully.");
                }
                else
                {
                    MessageBox.Show("No item selected.");
                }
            }
        }

        /// <summary>
        /// Updates the quantity and total price of an existing item in the DataGridView.
        /// Returns true if the item exists and was updated, false otherwise.
        /// </summary>
        private bool UpdateExistingItem(dynamic selectedItem)
        {
            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
            {
                if (row.Cells["id"].Value != null && row.Cells["id"].Value.ToString() == selectedItem.id)
                {
                    decimal currentQty = Convert.ToDecimal(row.Cells["stock"].Value);
                    decimal newQty = currentQty + Convert.ToDecimal(selectedItem.stock);
                    row.Cells["stock"].Value = newQty;

                    decimal sellPrice = Convert.ToDecimal(row.Cells["sell_price"].Value);
                    row.Cells["total"].Value = newQty * sellPrice;

                    return true; // Item found and updated
                }
            }
            return false; // Item not found
        }

        /// <summary>
        /// Inserts a new pending transaction into the database.
        /// </summary>
        private bool InsertPendingTransaction(dynamic selectedItem)
        {
            return itemController.AddPendingTransaction(
                1, // terminalId
                100, // cashierId
                int.Parse(selectedItem.id), // Convert string to int
                selectedItem.barcode,
                selectedItem.unit,
                selectedItem.stock, // Quantity
                selectedItem.sell_price, // Sell Price
                0, // Discount Percentage
                0, // Discount Total
                0, // Tax
                selectedItem.stock * selectedItem.sell_price, // Total
                "" // Note
            );
        }

        /// <summary>
        /// Adds a new item as a row in the DataGridView.
        /// </summary>
        private void AddNewItemToGrid(dynamic selectedItem)
        {
            dataGridViewCart4.Rows.Add(
                selectedItem.id,
                selectedItem.barcode,
                selectedItem.name,
                selectedItem.stock, // Quantity
                selectedItem.unit,
                selectedItem.sell_price, // Sell Price
                0, // Discount Percentage
                0, // Discount Total
                selectedItem.stock * selectedItem.sell_price, // Total
                "" // Note
            );
        }



        private void SetupDataGridView()
        {
            dataGridViewCart4.Columns.Clear();
            string[] columnNames = { "id","barcode", "Nama Barang", "qty", "Satuan", "Harga", "Pot(%)", "Pajak", "Total","Keterangan Per Item" };
            string[] propertyNames = { "id","barcode", "name", "stock", "unit", "sell_price", "discount", "tax", "total","note" };
            bool[] readOnlyColumns = { true,true, true, false, true, true, false, false, true,false };

            for (int i = 0; i < columnNames.Length; i++)
            {
                dataGridViewCart4.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = propertyNames[i],
                    HeaderText = columnNames[i],
                    Name = propertyNames[i],
                    ReadOnly = readOnlyColumns[i]
                });
            }

            // Set AutoSizeMode to None to manually adjust width
            dataGridViewCart4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Set specific column widths
            dataGridViewCart4.Columns["barcode"].Width = 150;  // Adjust as needed
            dataGridViewCart4.Columns["name"].Width = 250;       // Wider for item names
            dataGridViewCart4.Columns["sell_price"].Width = 120; // Adjust for price visibility

            // Optional: Enable text wrapping if needed
            dataGridViewCart4.Columns["name"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewCart4.RowPostPaint += DataGridViewCart4_RowPostPaint;
            dataGridViewCart4.CellValueChanged += DataGridViewCart4_CellValueChanged;
            //dataGridViewCart4.CellBeginEdit += dataGridViewCart4_CellBeginEdit;
            dataGridViewCart4.UserDeletingRow += dataGridViewCart4_UserDeletingRow;
                dataGridViewCart4.RowsRemoved += DataGridViewCart4_RowsRemoved;
            dataGridViewCart4.CellEndEdit += dataGridViewCart4_CellEndEdit;
            dataGridViewCart4.KeyDown += DataGridViewCart4_KeyDown;



        }

        private void DataGridViewCart4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D) // Detect Ctrl + D
            {
                if (dataGridViewCart4.CurrentCell != null)
                {
                    int rowIndex = dataGridViewCart4.CurrentCell.RowIndex; // Get current row
                    dataGridViewCart4.Rows[rowIndex].Selected = true; // Highlight the entire row
                }
            }
            if (e.Control && e.KeyCode == Keys.S) // Detect Ctrl + S
            {
                e.SuppressKeyPress = true; // Prevent default behavior (e.g., saving)
                txtCariBarang.Focus(); // Move focus to search textbox
            }
            if (e.KeyCode == Keys.F12) // Pressing F12 triggers payment
            {
                e.SuppressKeyPress = true; // Prevents default behavior
                btnPay.PerformClick(); // Simulates button click
            }

            if (e.Control && e.KeyCode == Keys.P) // Ctrl + P also triggers payment
            {
                e.SuppressKeyPress = true;
                btnPay.PerformClick();
            }
        }


        private void DataGridViewCart4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewCart4.Columns["stock"].Index)
            {
                UpdateStock(e.RowIndex);
            }
            UpdateStock(e.RowIndex);
            //RecalculateTotalForRow(e.RowIndex);
            //CalculateAllTotals();
        }

        //private void dataGridViewCart4_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        //{
        //    if (dataGridViewCart4.Columns[e.ColumnIndex].Name == "stock")
        //    {

        //        var row = dataGridViewCart4.Rows[e.RowIndex];
        //        if (row.Cells["id"].Value == null)
        //        {
        //            e.Cancel = true;
        //            MessageBox.Show("You must select a product first!");
        //            return; // Exit the method immediately
        //        }

        //        row.Cells["stock"].Tag = row.Cells["stock"].Value; // Store the old value
        //        // If no product is selected, prevent editing

        //    }

        //}
 

        private void dataGridViewCart4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridViewCart4.Rows[e.RowIndex];

            if (dataGridViewCart4.Columns[e.ColumnIndex].Name == "discount")
            {
                decimal discountPercentage = Convert.ToDecimal(row.Cells["discount"].Value);
                int itemId = Convert.ToInt32(row.Cells["id"].Value);

                bool updateSuccess = itemController.UpdatePendingTransactionDiscount(1,itemId, discountPercentage);
                if (updateSuccess)
                {
                    RecalculateTotalForRow(row);
                    CalculateAllTotals();
                }
                else
                {
                    MessageBox.Show("Failed to update discount in database.");
                }
            }
            if (dataGridViewCart4.Columns[e.ColumnIndex].Name == "note")
            {
                string note = row.Cells["note"].Value.ToString();
                int itemId = Convert.ToInt32(row.Cells["id"].Value);

                bool updateSuccess = itemController.UpdatePendingTransactionNote(1, itemId, note);
                if (!updateSuccess)
                {
                    MessageBox.Show("Failed to update note in database.");
                }
            }
            if (dataGridViewCart4.Columns[e.ColumnIndex].Name == "stock")
            {

                int enteredQuantity = Convert.ToInt32(row.Cells["stock"].Value);
                int previousQuantity = Convert.ToInt32(row.Cells["stock"].Tag ?? row.Cells["stock"].Value); // Get previous quantity

                if (enteredQuantity == 0)
                {
                    // Confirm before deleting the row
                    DialogResult result = MessageBox.Show("Are you sure you want to remove this item?", "Confirm", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        string barcode = row.Cells["id"].Value.ToString();

                        // Restore stock when row is deleted
                        itemController.UpdateItemStock(barcode, itemController.GetItemStock(barcode) + previousQuantity);

                        dataGridViewCart4.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        row.Cells["stock"].Value = previousQuantity; // Restore old value
                    }
                }
            }
        }


        private void DataGridViewCart4_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalculateAllTotals();
            txtCariBarang.Focus();
        }

        private void dataGridViewCart4_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;
            if (row.Cells["id"].Value == null) return;

            int itemId = Convert.ToInt32(row.Cells["id"].Value);
            string barcode = row.Cells["barcode"].Value.ToString();
            int previousQuantity = Convert.ToInt32(row.Cells["stock"].Value);

            // Delete from pending_transactions
            bool deleteSuccess = itemController.DeletePendingTransaction(1,itemId);

            if (deleteSuccess)
            {
                int currentStock = itemController.GetItemStock(barcode);
                itemController.UpdateItemStock(barcode, currentStock + previousQuantity);
                MessageBox.Show("Row deleted, stock restored, and totals recalculated.");
            }
            else
            {
                MessageBox.Show("Failed to delete item from pending transactions.");
                e.Cancel = true; // Prevent row from being deleted
            }
        }

        //private void dataGridViewCart4_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        //{
        //    var row = e.Row;
        //    if (row.Cells["id"].Value == null)
        //    {
        //        return; // Exit if no product is selected (to prevent errors)
        //    }

        //    string barcode = row.Cells["id"].Value.ToString();
        //    int previousQuantity = Convert.ToInt32(row.Cells["stock"].Value);

        //    // First, restore stock in the database
        //    int currentStock = itemController.GetItemStock(barcode);
        //    itemController.UpdateItemStock(barcode, currentStock + previousQuantity);

        //    // Then, recalculate totals after stock update
        //    //RecalculateTotalForRow(row);
        //    //// Ensure total updates correctly
        //    //this.BeginInvoke(new Action(() =>
        //    //{
        //    //    CalculateAllTotals();
        //    //}));

        //    MessageBox.Show("Row deleted, stock restored, and totals recalculated.");
        //}

        //private void dataGridViewCart4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (dataGridViewCart4.Columns[e.ColumnIndex].Name == "stock")
        //    {
        //        var row = dataGridViewCart4.Rows[e.RowIndex];
        //        int enteredQuantity = Convert.ToInt32(row.Cells["stock"].Value);
        //        int previousQuantity = Convert.ToInt32(row.Cells["stock"].Tag ?? row.Cells["stock"].Value); // Get previous quantity

        //        if (enteredQuantity == 0)
        //        {
        //            // Confirm before deleting the row
        //            DialogResult result = MessageBox.Show("Are you sure you want to remove this item?", "Confirm", MessageBoxButtons.YesNo);
        //            if (result == DialogResult.Yes)
        //            {
        //                string barcode = row.Cells["id"].Value.ToString();

        //                // Restore stock when row is deleted
        //                itemController.UpdateItemStock(barcode, itemController.GetItemStock(barcode) + previousQuantity);

        //                dataGridViewCart4.Rows.RemoveAt(e.RowIndex);
        //            }
        //            else
        //            {
        //                row.Cells["stock"].Value = previousQuantity; // Restore old value
        //            }
        //        }
        //    }
        //}



        // NEEDSTO BE UPDATED
        private void UpdateStock(int rowIndex)
        {
            var row = dataGridViewCart4.Rows[rowIndex];
            string barcode = row.Cells["barcode"].Value.ToString();

            int previousQuantity = Convert.ToInt32(row.Cells["stock"].Tag ?? row.Cells["stock"].Value); // Store old value in Tag
            int enteredQuantity = Convert.ToInt32(row.Cells["stock"].Value);

            int currentStock = itemController.GetItemStock(barcode);
            int newStock = currentStock + previousQuantity - enteredQuantity;

            MessageBox.Show($"new STOCK: {newStock} : current Stock : {currentStock}");
            if (newStock < currentStock)
            {
                MessageBox.Show($"You cannot enter more than the available stock: {currentStock + previousQuantity}");
                row.Cells["stock"].Value = previousQuantity;
                return;
            }

            //MessageBox.Show($"Updating ITEM STOCK: {currentStock} | Previous Qty: {previousQuantity} | Entered Qty: {enteredQuantity}");

            // Update in pending_transactions
            bool updateSuccess = itemController.UpdatePendingTransactionStock(1,Convert.ToInt32(row.Cells["id"].Value), enteredQuantity);

            if (updateSuccess)
            {
                itemController.UpdateItemStock(barcode, newStock);
                row.Cells["stock"].Tag = enteredQuantity;
                RecalculateTotalForRow(row);
                CalculateAllTotals();
            }
            else
            {
                MessageBox.Show("Failed to update stock in database.");
            }
        }


        private void RecalculateTotalForRow(DataGridViewRow row)
        {
            if (int.TryParse(row.Cells["stock"].Value.ToString(), out int qty) &&
                decimal.TryParse(row.Cells["sell_price"].Value.ToString(), out decimal price))
            {
                decimal discount = row.Cells["discount"].Value != null ? Convert.ToDecimal(row.Cells["discount"].Value) : 0;
                decimal tax = row.Cells["tax"].Value != null ? Convert.ToDecimal(row.Cells["tax"].Value) : 0;

                decimal total = (qty * price) * (1 - discount / 100) + ((qty * price) * (tax / 100));
                row.Cells["total"].Value = total;
            }
        }

        private void CalculateAllTotals()
        {
            decimal totalCart = 0;
            Debug.WriteLine("Recalculating totals...");

            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
            {
                if (row.Cells["total"].Value != null)
                {
                    decimal rowTotal = Convert.ToDecimal(row.Cells["total"].Value);
                    Debug.WriteLine($"Row total: {rowTotal}");
                    totalCart += rowTotal;
                }
            }

            Debug.WriteLine($"New total: {totalCart}");
            label2.Text = totalCart.ToString("N2");
        }


        private void DataGridViewCart4_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dataGridViewCart4.RowHeadersWidth, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridViewCart4.Font, rect, Color.Black, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        //private void btnPay_Click(object sender, EventArgs e)
        //{
        //    decimal totalAmount = Convert.ToDecimal(label2.Text);
        //    using (PaymentModalForm paymentModal = new PaymentModalForm(totalAmount))
        //    {
        //        if (paymentModal.ShowDialog() == DialogResult.OK)
        //        {
        //            Payment payment = new Payment(paymentModal.PaymentAmount, paymentModal.PaymentMethod, DateTime.Now);
        //            itemController.InsertPayment(payment);
        //            // Clear the cart (remove all rows)
        //            dataGridViewCart4.Rows.Clear();

        //            // Reset total label
        //            label2.Text = "0.00";

        //            // Set focus back to search box
        //            txtCariBarang.Focus();
        //            MessageBox.Show("Payment successful!");

        //        }
        //    }
        //}
        private string GenerateTransactionNumber()
        {
            return "TRX-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                decimal totalAmount = Convert.ToDecimal(label2.Text);

                if (totalAmount <= 0)
                {
                    MessageBox.Show("Total amount must be greater than zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (PaymentModalForm paymentModal = new PaymentModalForm(totalAmount))
                {
                    if (paymentModal.ShowDialog() == DialogResult.OK)
                    {
                        // Validate Payment Input
                        if (paymentModal.PaymentAmount < totalAmount)
                        {
                            MessageBox.Show("Insufficient payment amount!", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Create transaction object
                        Transactions transaction = new Transactions
                        {
                            TsNumbering = GenerateTransactionNumber(),
                            TsCode = "TXN-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                            TsTotal = totalAmount,
                            TsPaymentAmount = paymentModal.PaymentAmount, // ✅ Store payment amount
                            TsCashback = 0,
                            TsMethod = paymentModal.PaymentMethod,
                            TsStatus = 1, // 1 = Paid
                            TsChange = paymentModal.PaymentAmount - totalAmount,
                            TsInternalNote = "Processed via POS system",
                            TsNote = "Test Note",
                            TsCustomer = null,
                            TsFreename = "Guest",
                            CreatedBy = 100, // Replace with GetCurrentUserId() if available
                            CreatedAt = DateTime.UtcNow
                        };

                        // Insert Transaction and Get Transaction ID
                        int transactionId = itemController.InsertTransaction(transaction);

                        if (transactionId > 0)
                        {
                            List<TransactionDetail> transactionDetails = new List<TransactionDetail>();

                            // Loop through DataGridView and add transaction details
                            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
                            {
                                if (row.Cells["id"].Value == null) continue; // Skip empty rows
                                //MessageBox.Show("PREPARING ISNRET DETAIL on DataGridViewRow.");
                                // Extract values safely
                                int itemId = Convert.ToInt32(row.Cells["id"].Value);
                                string barcode = row.Cells["barcode"].Value?.ToString() ?? ""; // Use string for alphanumeric codes
                                string itemName = row.Cells["name"]?.Value?.ToString() ?? "";
                                decimal quantity = row.Cells["stock"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["stock"].Value);
                                string unit = row.Cells["unit"]?.Value?.ToString() ?? "";
                                decimal sellPrice = decimal.TryParse(row.Cells["sell_price"]?.Value?.ToString(), out decimal sp) ? sp : 0;
                                decimal discountPercentage = decimal.TryParse(row.Cells["discount"]?.Value?.ToString(), out decimal dp) ? dp : 0;
                                decimal tax = decimal.TryParse(row.Cells["tax"]?.Value?.ToString(), out decimal tx) ? tx : 0;


                                // Calculate discount amount
                                decimal discountTotal = (discountPercentage / 100) * (sellPrice * quantity);
                                decimal finalTotal = (sellPrice * quantity) - discountTotal + tax;

                                MessageBox.Show("discountTotal : " + discountTotal + " finalTotal : " + finalTotal);
                                TransactionDetail detail = new TransactionDetail
                                {
                                    TsId = transactionId,
                                    ItemId = itemId,
                                    Barcode = barcode,
                                    TsdSellPrice = sellPrice, // Save price at the time of transaction
                                    TsdQuantity = quantity,
                                    TsdUnit = unit,
                                    TsdNote = row.Cells["note"]?.Value?.ToString() ?? "",
                                    TsdDiscountPerItem = discountTotal / quantity, // Discount per item
                                    TsdDiscountPercentage = discountPercentage,
                                    TsdDiscountTotal = discountTotal,
                                    TsdTax = tax,
                                    TsdTotal = finalTotal, // Final total for this item
                                    CreatedBy = 100, // Replace with GetCurrentUserId() if available
                                    CreatedAt = DateTime.UtcNow
                                };

                                transactionDetails.Add(detail);
                            }
                            MessageBox.Show("PREPARING ISNRET DETAIL.");
                            // Insert Transaction Details
                            if (transactionDetails.Count > 0)
                            {
                                itemController.InsertTransactionDetails(transactionDetails);
                            }

                           


                            // clear pending-transactions
                            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
                            {
                                int itemId = Convert.ToInt32(row.Cells["id"].Value);
                                string barcode = row.Cells["barcode"].Value?.ToString() ?? "";
                                string unit = row.Cells["unit"].Value?.ToString() ?? "";
                                decimal quantity = row.Cells["stock"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["stock"].Value);

                                //update product stock and reserved stock
                                int rStock = itemController.GetItemReservedStock(itemId);
                                rStock = rStock - (int)quantity;

                                int stock = itemController.GetItemStock(itemId);
                                stock = stock - (int)quantity;

                                itemController.UpdateItemStockAndReservedStock(itemId, stock, rStock);


                                // clear pending transactions
                                itemController.ClearPendingTransaction(barcode, quantity, unit);

                            }

                            // Clear the cart (remove all rows)
                            dataGridViewCart4.Rows.Clear();

                            // Reset total label
                            label2.Text = "0.00";

                            // Set focus back to search box
                            txtCariBarang.Focus();

                            MessageBox.Show("Payment successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert transaction!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid total amount format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
