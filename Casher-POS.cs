using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO.Ports;

using POS_qu.Controllers;
using POS_qu.Models;
using POS_qu.Helpers;
using POS_qu.Core;
using System.Text;
using Npgsql;
using QuestPDF.Infrastructure;

using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace POS_qu
{
    public partial class Casher_POS : Form
    {
        private ItemController itemController;
        private PrintDocument printDoc = new PrintDocument();
        private PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();

        private IActivityService activityService;
        private ILogger flogger = new FileLogger();
        private ILogger dlogger = new DbLogger();

        private string cart_session_code;
        public Casher_POS()
        {
            InitializeComponent();
            itemController = new ItemController();
            SetupDataGridView();
            txtCariBarang.KeyDown += TxtCariBarang_KeyDown;
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true; // Ensure the form can intercept key events
            this.KeyDown += Casher_POS_KeyDown; // Add the key down event handler



            // Inject the FileLogger into ActivityService
            activityService = new ActivityService(flogger, dlogger);
            //activityService.LogAction(ActivityType.Print.ToString(), "Printing preview", new { ItemCode = "A123", Quantity = 2 });
            // Example of logging an action

            //string terminal = SessionUser.TerminalId(pcId);
            //string shift = Utility.GetCurrentShift();

            //string pcId = Utility.GetPcId();
            //string terminal = Utility.GetTerminalName(pcId);
            //string shift = Utility.GetCurrentShift();


            var user = SessionUser.GetCurrentUser();
            string pcId = Utility.GetPcId();
            labelSessionInfo.Text = $"User: {user.Username} | Terminal: {user.TerminalId} | Shift: {user.ShiftId} |pcid: {pcId}";
            //txtCariBarang.Focus();

            this.Shown += Casher_POS_Shown;
            infoPanel.Visible = false;
            UpdateOrderBadge();
            cart_session_code = GenerateCartSessionCode();
            lblCartSession.Text = cart_session_code.ToString();
        }

        private void Casher_POS_Shown(object sender, EventArgs e)
        {
            txtCariBarang.Focus();
        }
        /// <summary>
        /// Generate a new unique cart session code for the current user.
        /// </summary>
        /// <returns>New cart session code string</returns>
        private string GenerateCartSessionCode()
        {
            var sessionUser = SessionUser.GetCurrentUser();
            return $"CSC-{sessionUser.TerminalId}-{sessionUser.UserId}-{DateTime.Now:yyyyMMddHHmmss}";
        }

        private string GenerateTransactionNumber()
        {
            return "TRX-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        private void ResetCart()
        {
            dataGridViewCart4.Rows.Clear();
            label2.Text = "0.00";
            txtCariBarang.Focus();
            GlobalDiscountPercent = 0;
            PaymentAmount = 0;
            Cashback = 0;
            UpdateInvoiceUI();

            // Kembalikan DataGridView ke mode editable
            foreach (DataGridViewColumn col in dataGridViewCart4.Columns)
            {
                col.ReadOnly = false;
                col.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            }
            dataGridViewCart4.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCart4.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            // 🔹 Generate new cart session code
            cart_session_code = GenerateCartSessionCode();
            lblCartSession.Text = cart_session_code.ToString();

        }

        //private void PrintReceipt()
        //{
        //    // Membuat objek PrintDocument
        //    PrintDocument printDoc = new PrintDocument();

        //    // Tentukan ukuran kertas (contohnya 80mm x 200mm)
        //    printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, 800); // Lebar 80mm (315 pixel), panjang 200mm (800 pixel)

        //    // Tentukan margin (jika diperlukan)
        //    printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); // Tidak ada margin

        //    // Tentukan event handler untuk PrintPage
        //    printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

        //    // Mulai pencetakan
        //    printDoc.Print();
        //    activityService.LogAction(ActivityType.Print.ToString(), "Printing preview");

        //}


        //private void PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    Font font = new Font("Arial", 10);
        //    Font boldFont = new Font("Arial", 10, FontStyle.Bold);
        //    Brush brush = Brushes.Black;
        //    int yPosition = 10; // Starting Y position for the first row
        //    int lineSpacing = 20; // Space between lines
        //    int leftColumnWidth = 300; // Set width for the left column (item names)
        //    int rightColumnWidth = 100; // Set width for the right column (prices)

        //    int rowHeight = 20; // Height of each row (to control the space between each row)
        //    int currentRowHeight = yPosition; // Start the first row at the top of the page

        //    //// Add the first label (label1)
        //    //g.DrawString(label1.Text, font, brush, new PointF(10, currentRowHeight)); // Assuming label1 is aligned as needed
        //    //currentRowHeight += lineSpacing;

        //    // Add the number of items label (labelNumOfItems)
        //    g.DrawString($"Jumlah barang: {labelNumOfItems.Text}", font, brush, new PointF(10, currentRowHeight)); // Assuming labelNumOfItems displays number of items
        //    currentRowHeight += lineSpacing;

        //    // Add a horizontal line under the subtotal
        //    Label separator = new Label();
        //    separator.AutoSize = false;
        //    separator.Height = 1;
        //    separator.Width = 400;  // Adjust width as needed
        //    separator.BackColor = Color.Gray;
        //    g.DrawLine(new Pen(Color.Gray), new Point(10, currentRowHeight), new Point(400, currentRowHeight)); // Horizontal line
        //    currentRowHeight += 5; // Move down after the line

        //    foreach (Control ctrl in flowLayoutPanel.Controls)
        //    {
        //        if (ctrl is TableLayoutPanel tableLayoutPanel)
        //        {
        //            foreach (Control tableControl in tableLayoutPanel.Controls)
        //            {
        //                if (tableControl is Label label)
        //                {
        //                    // Print labels aligned left for item name
        //                    if (label.TextAlign == ContentAlignment.MiddleLeft)
        //                    {
        //                        // Check if the label is "Grand Total" and apply bold formatting
        //                        if (label.Text == "Grand Total")
        //                        {
        //                            g.DrawString(label.Text, boldFont, brush, new PointF(10, currentRowHeight)); // Bold for Grand Total
        //                        }
        //                        else if (label.Text == "Subtotal")  // This condition places the separator right before the "Subtotal"
        //                        {
        //                            g.DrawString(label.Text, font, brush, new PointF(10, currentRowHeight)); // Regular font for Subtotal
        //                            g.DrawLine(new Pen(Color.Gray), new Point(10, currentRowHeight + rowHeight), new Point(400, currentRowHeight + rowHeight)); // Horizontal line
        //                            currentRowHeight += 5; // Move down after the line
        //                        }
        //                        else
        //                        {
        //                            g.DrawString(label.Text, font, brush, new PointF(10, currentRowHeight)); // Regular font for other labels
        //                        }
        //                    }
        //                    // Print labels aligned right for totals or prices
        //                    else if (label.TextAlign == ContentAlignment.MiddleRight)
        //                    {
        //                        // Check if the label is "Grand Total" and apply bold formatting for value
        //                        if (label.Text == "Grand Total")
        //                        {
        //                            g.DrawString(label.Text, boldFont, brush, new PointF(leftColumnWidth, currentRowHeight)); // Bold for Grand Total value
        //                        }
        //                        else
        //                        {
        //                            g.DrawString(label.Text, font, brush, new PointF(leftColumnWidth, currentRowHeight)); // Regular font for other values
        //                        }
        //                    }

        //                    // If the label is on the right column, move to the next row
        //                    if (label.TextAlign == ContentAlignment.MiddleRight)
        //                    {
        //                        currentRowHeight += rowHeight; // Move to the next row after printing the right-aligned label
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    // Print Barcode (You can replace this with actual barcode generation logic)
        //    string barcode = "1234567890"; // Replace with your barcode value
        //    g.DrawString($"Barcode: {barcode}", font, brush, new PointF(10, currentRowHeight));
        //    currentRowHeight += lineSpacing;

        //    // Print Thank You note
        //    string thankYouNote = "Thank you for your purchase!";
        //    g.DrawString(thankYouNote, font, brush, new PointF(10, currentRowHeight));
        //    currentRowHeight += lineSpacing;

        //    // Print Store Name
        //    string storeName = "Your Store Name";
        //    g.DrawString(storeName, boldFont, brush, new PointF(10, currentRowHeight));
        //    currentRowHeight += lineSpacing;

        //    string logoPath = @"D:\POS-qu\POS-qu\bin\Debug\net8.0-windows\appimages\2.png";
        //    Image logo = Image.FromFile(logoPath);
        //    g.DrawImage(logo, new PointF(10, currentRowHeight));

        //    //// Print Store Logo (Make sure the logo is an image file and has been loaded)
        //    //Image logo = Image.FromFile("path_to_logo.png"); // Specify the path to your logo file
        //    //g.DrawImage(logo, new PointF(10, currentRowHeight)); // Adjust the location as needed
        //    //currentRowHeight += logo.Height + 10; // Adjust for logo height and some space below it



        //}




        private void Casher_POS_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Control && e.KeyCode == Keys.J)
            //{
            //    print();
            //}
        }

        private void print(Transactions t, InvoiceData invoice)
        {
            var sessionUser = SessionUser.GetCurrentUser();

            string receipt = GenerateReceiptText(
                invoice: invoice,
                terminal: sessionUser.TerminalName,
                user: sessionUser.Username,
                shift: "shift " + sessionUser.ShiftId,
                statusBayar: "status : " + t.TsStatus,
                jumlahBayar: t.TsPaymentAmount.ToString(),
                metodeBayar: t.TsMethod,
                kembalian: t.TsChange.ToString()
            );

            // Simpan ke global jika dibutuhkan
            lastReceipt = receipt;

            // Print dan log
            //PrintToPOS58(receipt);

            // save pdf receipt
            SaveReceiptToPdf(receipt);

            // Simpan ke database
            SimpanReceiptKeDatabase(receipt);

            // Log receipt
            LogReceipt(receipt);
        }

        private string lastReceipt = ""; // Untuk simpan isi struk terakhir jika perlu akses dari luar

        private string GenerateReceiptText(
      InvoiceData invoice,
      string terminal,
      string user,
      string shift,
      string statusBayar,
      string jumlahBayar,
      string metodeBayar,
      string kembalian)
        {
            StringBuilder receipt = new StringBuilder();

            // === 1️⃣ Ambil data setting struk ===
            string judul = "", alamat = "", telepon = "", footer = "";
            bool showNamaToko = true, showAlamat = true, showTelepon = true, showFooter = true;

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM struk_setting ORDER BY updated_at DESC LIMIT 1;";
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        judul = reader["judul"]?.ToString()?.Trim() ?? "";
                        alamat = reader["alamat"]?.ToString()?.Trim() ?? "";
                        telepon = reader["telepon"]?.ToString()?.Trim() ?? "";
                        footer = reader["footer"]?.ToString()?.Trim() ?? "";
                        showNamaToko = reader["is_visible_nama_toko"] as bool? ?? true;
                        showAlamat = reader["is_visible_alamat"] as bool? ?? true;
                        showTelepon = reader["is_visible_telepon"] as bool? ?? true;
                        showFooter = reader["is_visible_footer"] as bool? ?? true;
                    }
                }
            }

            // === 2️⃣ Header toko ===
            if (showNamaToko && !string.IsNullOrEmpty(judul)) receipt.AppendLine(judul);
            if (showAlamat && !string.IsNullOrEmpty(alamat)) receipt.AppendLine(alamat);
            if (showTelepon && !string.IsNullOrEmpty(telepon)) receipt.AppendLine(telepon);
            receipt.AppendLine("------------------------------");

            // === 3️⃣ Info transaksi ===
            receipt.AppendLine($"Terminal : {terminal}");
            receipt.AppendLine($"Kasir    : {user}");
            receipt.AppendLine($"Shift    : {shift}");
            receipt.AppendLine($"Tanggal  : {DateTime.Now:yyyy-MM-dd}");
            receipt.AppendLine($"Waktu    : {DateTime.Now:HH:mm:ss}");
            receipt.AppendLine("------------------------------");

            // === 4️⃣ Detail item dari InvoiceData ===
            foreach (var item in invoice.Items)
            {
                string itemLine = $"{item.Qty}x {item.Name}";
                if (itemLine.Length > 20) itemLine = itemLine.Substring(0, 20);
                receipt.AppendLine(itemLine.PadRight(20) + $"{item.Total:N0}".PadLeft(10));

                if (item.DiscountPercent > 0)
                {
                    string discLine = $"  Diskon: {item.DiscountPercent:N0}%".PadRight(20) + $"-{item.DiscountAmount:N0}".PadLeft(10);
                    receipt.AppendLine(discLine);
                }

                if (!string.IsNullOrEmpty(item.Note))
                    receipt.AppendLine($"  Catatan: {item.Note}");
            }

            //receipt.AppendLine("------------------------------");

            //// === 5️⃣ Global note & delivery info ===
            //if (!string.IsNullOrEmpty(invoice.GlobalNote))
            //    receipt.AppendLine($"Global Note: {invoice.GlobalNote}");
            //if (!string.IsNullOrEmpty(invoice.DeliveryName))
            //    receipt.AppendLine($"Delivery   : {invoice.DeliveryName}");
            //if (!string.IsNullOrEmpty(invoice.DeliveryAddress))
            //    receipt.AppendLine($"Alamat     : {invoice.DeliveryAddress}");
            //if (!string.IsNullOrEmpty(invoice.DeliveryMethod))
            //    receipt.AppendLine($"Metode     : {invoice.DeliveryMethod}");
            //if (!string.IsNullOrEmpty(invoice.DeliveryPhone))
            //    receipt.AppendLine($"Telp       : {invoice.DeliveryPhone}");


            // === 6️⃣ Ringkasan totals ===
            receipt.AppendLine($"Subtotal".PadRight(20) + $"{invoice.Subtotal:N0}".PadLeft(10));
            receipt.AppendLine($"Item Discount".PadRight(20) + $"-{invoice.TotalDiscount:N0}".PadLeft(10));

            if (invoice.GlobalDiscountPercent > 0)
                receipt.AppendLine($"Global Disc ({invoice.GlobalDiscountPercent:N0}%)".PadRight(20) + $"-{invoice.GlobalDiscountValue:N0}".PadLeft(10));


            // ✅ Tambahkan Global Note jika ada
            if (!string.IsNullOrEmpty(invoice.GlobalNote))
                receipt.AppendLine($"Note".PadRight(20) + $"{invoice.GlobalNote}");

            // ✅ Tambahkan ongkir jika ada
            if (invoice.DeliveryAmount > 0)
                receipt.AppendLine($"Delivery".PadRight(20) + $"{invoice.DeliveryAmount:N0}".PadLeft(10));


            receipt.AppendLine("------------------------------");
            receipt.AppendLine($"Grand Total".PadRight(20) + $"{invoice.GrandTotal:N0}".PadLeft(10));

            // === 7️⃣ Info pembayaran & cashback ===
            if (decimal.TryParse(jumlahBayar, out decimal jmlBayar))
                jumlahBayar = jmlBayar.ToString("N0");

            if (decimal.TryParse(kembalian, out decimal kemb))
                kembalian = kemb.ToString("N0");

            receipt.AppendLine("------------------------------");
            receipt.AppendLine($"Status Bayar : {statusBayar}");
            receipt.AppendLine($"Metode Bayar : {metodeBayar}");
            receipt.AppendLine($"Jumlah Bayar : {jumlahBayar}");
            receipt.AppendLine($"Kembalian    : {kembalian}");
            receipt.AppendLine("------------------------------");

            // === 8️⃣ Footer ===
            if (showFooter && !string.IsNullOrEmpty(footer))
                receipt.AppendLine(footer);

            receipt.AppendLine("");

            return receipt.ToString();
        }



        private decimal GlobalDiscountPercent { get; set; } = 0;
        private decimal Cashback = 0;
        private decimal PaymentAmount = 0;
        private string GlobalNote = null;
        private decimal DeliveryAmount = 0;
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
                    // Event saat jumlah pembayaran berubah
                    paymentModal.PaymentAmountChanged += (paymentAmount) =>
                    {
                        PaymentAmount = paymentAmount;
                        UpdateInvoiceUI();
                    };

                    // Event saat diskon global berubah
                    paymentModal.GlobalDiscountChanged += (discountPercent) =>
                    {
                        GlobalDiscountPercent = discountPercent;
                        UpdateInvoiceUI();
                    };

                    paymentModal.GlobalNoteChanged += (note) =>
                    {
                        // Misal: update label atau simpan ke variabel global
                        GlobalNote = note;
                        UpdateInvoiceUI();
                    };

                    paymentModal.DeliveryAmountChanged += (amount) =>
                    {
                        DeliveryAmount = amount;
                        UpdateInvoiceUI();
                    };


                    if (paymentModal.ShowDialog() != DialogResult.OK) return;

                    // Ambil data dari modal
                    decimal grandTotal = paymentModal.GrandTotal > 0 ? paymentModal.GrandTotal : totalAmount;
                    PaymentAmount = paymentModal.PaymentAmount;
                    GlobalDiscountPercent = paymentModal.GlobalDiscountPercent;

                    if (PaymentAmount < grandTotal)
                    {
                        MessageBox.Show("Insufficient payment amount!", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var sessionUser = SessionUser.GetCurrentUser();
                    int? orderId = SelectedOrderId > 0 ? SelectedOrderId : null;

                    if (orderId.HasValue)
                    {
                        itemController.UpdateOrderPayment(orderId.Value, 1, paymentModal.PaymentMethod);
                    }

                    // Ambil invoice data dari cart
                    InvoiceData invoice = CalculateInvoiceDataFromCart();

                    // Buat transaksi
                    Transactions transaction = new Transactions
                    {
                        TsNumbering = GenerateTransactionNumber(),
                        TsCode = "TXN-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        TsTotal = invoice.Subtotal,
                        TsPaymentAmount = PaymentAmount,
                        TsCashback = invoice.Cashback,
                        TsMethod = paymentModal.PaymentMethod,
                        TsStatus = 1,
                        TsChange = PaymentAmount - invoice.GrandTotal,
                        TsInternalNote = "Processed via POS system",
                        TsNote = invoice.GlobalNote ?? string.Empty,
                        TsDiscountTotal = invoice.TotalDiscount + invoice.GlobalDiscountValue,
                        TsGrandTotal = invoice.GrandTotal,
                        TsCustomer = null,
                        TsDelivery = invoice.DeliveryAmount,
                        TsFreename = "Guest",
                        UserId = sessionUser.UserId,
                        CreatedBy = sessionUser.UserId,
                        TerminalId = 1,
                        ShiftId = 1,
                        CreatedAt = DateTime.UtcNow,
                        OrderId = orderId
                    };

                    // Log mulai transaksi
                    activityService.LogAction(
                        userId: sessionUser.UserId.ToString(),
                        actionType: "Transaction_Start",
                        referenceId: null,
                        details: transaction // langsung pakai transaction object
                    );

                    // Insert transaksi
                    int transactionId = itemController.InsertTransaction(transaction);

                    if (transactionId <= 0)
                    {
                        MessageBox.Show("Failed to insert transaction!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Insert transaction details dari invoice.Items
                    var transactionDetails = invoice.Items.Select(i => new TransactionDetail
                    {
                        TsId = transactionId,
                        ItemId = i.ItemId,
                        Barcode = i.Barcode,
                        TsdSellPrice = i.Price,
                        TsdQuantity = i.Qty,
                        TsdUnit = i.Unit,
                        TsdConversionRate = i.ConversionRate,
                        TsdPricePerUnit = i.Price,
                        TsdUnitVariant = i.Unit,
                        TsdDiscountPerItem = i.DiscountAmount / i.Qty,
                        TsdDiscountPercentage = i.DiscountPercent,
                        TsdDiscountTotal = i.DiscountAmount,
                        TsdTax = i.Tax,
                        TsdTotal = i.Total,
                        TsdNote = i.Note,
                        CreatedBy = sessionUser.UserId,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();

                    if (transactionDetails.Count > 0)
                        itemController.InsertTransactionDetails(transactionDetails);

                    // Log transaksi berhasil
                    activityService.LogAction(
                        userId: sessionUser.UserId.ToString(),
                        actionType: "Transaction_Complete",
                        referenceId: transactionId,
                        details: new
                        {
                            Invoice = invoice,
                            Transaction = transaction,
                            Items = transactionDetails
                        }
                    );

                    // Print
                    print(transaction, invoice);

                    // Update stock & clear pending transactions dari invoice.Items
                    foreach (var item in invoice.Items)
                    {
                        int conversion = item.ConversionRate > 0 ? item.ConversionRate : 1;
                        decimal quantityInBaseUnit = item.Qty * conversion;

                        int rStock = itemController.GetItemReservedStock(item.ItemId) - (int)quantityInBaseUnit;
                        int stock = itemController.GetItemStock(item.ItemId) - (int)quantityInBaseUnit;

                        itemController.UpdateItemStockAndReservedStock(item.ItemId, stock, rStock);
                        itemController.ClearPendingTransaction(item.Barcode, item.Qty, item.Unit);
                    }

                    // Bersihkan cart
                    dataGridViewCart4.Rows.Clear();
                    label2.Text = "0.00";
                    txtCariBarang.Focus();
                    GlobalDiscountPercent = 0;
                    PaymentAmount = 0;
                    Cashback = 0;
                    UpdateInvoiceUI();

                    // Kembalikan DataGridView ke mode editable
                    foreach (DataGridViewColumn col in dataGridViewCart4.Columns)
                    {
                        col.ReadOnly = false;
                        col.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    }
                    dataGridViewCart4.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                    dataGridViewCart4.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

                    MessageBox.Show("Payment successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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




        /// <summary>
        /// Menghitung total transaksi berdasarkan DataGridView cart.
        /// Tidak ada kode UI sama sekali.
        /// </summary>
        private InvoiceData CalculateInvoiceDataFromCart()
        {
            var invoice = new InvoiceData();

            decimal totalCart = 0;
            decimal totalDiscount = 0;
            int numOfItems = 0;

            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
            {
                if (row.IsNewRow || row.Cells["name"].Value == null) continue;

                // Ambil data dari DataGridView
                int itemId = Convert.ToInt32(row.Cells["id"].Value ?? 0);
                string barcode = row.Cells["barcode"]?.Value?.ToString() ?? "";
                string itemName = row.Cells["name"].Value.ToString();
                string unit = row.Cells["unit"]?.Value?.ToString() ?? "";
                string unitVariant = unit; // default sama dengan unit, bisa diubah jika ada variant
                int conversionRate = row.Cells["conversion"] != null && int.TryParse(row.Cells["conversion"].Value?.ToString(), out int conv) ? conv : 1;
                int qty = Convert.ToInt32(row.Cells["stock"].Value ?? 0);
                decimal price = Convert.ToDecimal(row.Cells["sell_price"].Value ?? 0);
                decimal discountPercent = Convert.ToDecimal(row.Cells["discount"].Value ?? 0);
                decimal tax = Convert.ToDecimal(row.Cells["tax"]?.Value ?? 0);
                string note = row.Cells["note"]?.Value?.ToString()?.Trim() ?? "";

                // Kalkulasi
                decimal totalBeforeDiscount = price * qty;
                decimal discountAmount = totalBeforeDiscount * (discountPercent / 100);
                decimal total = totalBeforeDiscount - discountAmount + tax;

                numOfItems += qty;
                totalCart += totalBeforeDiscount - discountAmount;
                totalDiscount += discountAmount;

                invoice.Items.Add(new InvoiceItem
                {
                    ItemId = itemId,
                    Barcode = barcode,
                    Name = itemName,
                    Unit = unit,
                    UnitVariant = unitVariant,
                    ConversionRate = conversionRate,
                    Qty = qty,
                    Price = price,
                    DiscountPercent = discountPercent,
                    DiscountAmount = discountAmount,
                    Tax = tax,
                    Total = total,
                    Note = note
                });
            }

            invoice.Subtotal = totalCart + totalDiscount;
            invoice.TotalDiscount = totalDiscount;

            // Diskon global
            if (GlobalDiscountPercent > 0)
            {
                invoice.GlobalDiscountValue = totalCart * (GlobalDiscountPercent / 100);
                invoice.GlobalDiscountPercent = GlobalDiscountPercent;
            }


            // Global info (bisa di-set dari form sebelum generate script)
            invoice.GlobalNote = GlobalNote; // bisa diisi dari TextBox
            invoice.DeliveryName = "";
            invoice.DeliveryAddress = "";
            invoice.DeliveryMethod = "";
            invoice.DeliveryPhone = "";
            invoice.DeliveryAmount = DeliveryAmount;


            invoice.GrandTotal = totalCart - invoice.GlobalDiscountValue;

            // Tambahkan biaya pengantaran (jika ada)
            if (invoice.DeliveryAmount > 0)
            {
                invoice.GrandTotal += invoice.DeliveryAmount;
            }


            // Cashback
            invoice.Cashback = Math.Max(PaymentAmount - invoice.GrandTotal, 0);

            invoice.NumOfItems = numOfItems;

            return invoice;
        }


        /// <summary>
        /// Render UI realtime invoice di flowLayoutPanelInvoice.
        /// Layout persis seperti kode yang kamu kasih.
        /// </summary>
        private void UpdateInvoiceUI()
        {
            var invoice = CalculateInvoiceDataFromCart();
            RenderInvoiceUI(invoice); // pakai layout persis kamu
        }

        /// <summary>
        /// Membangun UI realtime invoice dari data InvoiceData.
        /// Tidak menyentuh DataGridView, hanya menampilkan di flowLayoutPanelInvoice.
        /// </summary>
        private void RenderInvoiceUI(InvoiceData invoice)
        {
            // Clear lama untuk rebuild
            flowLayoutPanel.Controls.Clear();

            // ================= Items =================
            TableLayoutPanel itemsTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            itemsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            itemsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            int totalQty = 0;

            foreach (var item in invoice.Items)
            {
                // Nama + total
                var lblName = new Label
                {
                    AutoSize = true,
                    Text = $"{item.Qty}x {item.Name}",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(5, 2, 5, 0),
                    ForeColor = System.Drawing.Color.Black
                };
                var lblTotal = new Label
                {
                    AutoSize = true,
                    Text = $"{item.Total:N0}",
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(5, 2, 5, 0),
                    ForeColor = System.Drawing.Color.Black
                };
                itemsTable.RowCount++;
                itemsTable.Controls.Add(lblName, 0, itemsTable.RowCount - 1);
                itemsTable.Controls.Add(lblTotal, 1, itemsTable.RowCount - 1);

                // Diskon per item
                if (item.DiscountPercent > 0)
                {
                    var lblDiscount = new Label
                    {
                        AutoSize = true,
                        Text = $"   Diskon: {item.DiscountPercent:N0}% (-{item.DiscountAmount:N0})",
                        ForeColor = System.Drawing.Color.FromArgb(200, 50, 50),
                        Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                        Padding = new Padding(5, 0, 5, 0)
                    };
                    itemsTable.RowCount++;
                    itemsTable.Controls.Add(lblDiscount, 0, itemsTable.RowCount - 1);
                    itemsTable.SetColumnSpan(lblDiscount, 2);
                }

                // Catatan item
                if (!string.IsNullOrEmpty(item.Note))
                {
                    var lblNote = new Label
                    {
                        AutoSize = true,
                        Text = $"   Catatan: {item.Note}",
                        ForeColor = System.Drawing.Color.DimGray,
                        Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                        Padding = new Padding(5, 0, 5, 2)
                    };
                    itemsTable.RowCount++;
                    itemsTable.Controls.Add(lblNote, 0, itemsTable.RowCount - 1);
                    itemsTable.SetColumnSpan(lblNote, 2);
                }

                totalQty += item.Qty;
            }

            flowLayoutPanel.Controls.Add(itemsTable);

            // ================= Separator =================
            flowLayoutPanel.Controls.Add(new Label
            {
                AutoSize = false,
                Height = 1,
                Width = flowLayoutPanel.Width,
                BackColor = System.Drawing.Color.Gray
            });

            // ================= Totals =================
            TableLayoutPanel totalsTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Dock = DockStyle.Top
            };
            totalsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            totalsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            void AddTotalRow(string text, string value, bool bold = false)
            {
                totalsTable.RowCount++;
                totalsTable.Controls.Add(new Label
                {
                    AutoSize = true,
                    Text = text,
                    Padding = new Padding(10, 3, 10, 3),
                    Font = bold ? new Font("Segoe UI", 9, FontStyle.Bold | FontStyle.Underline) : new Font("Segoe UI", 9)
                }, 0, totalsTable.RowCount - 1);

                totalsTable.Controls.Add(new Label
                {
                    AutoSize = true,
                    Text = value,
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(10, 3, 10, 3),
                    Font = bold ? new Font("Segoe UI", 9, FontStyle.Bold | FontStyle.Underline) : new Font("Segoe UI", 9)
                }, 1, totalsTable.RowCount - 1);
            }

            AddTotalRow("Subtotal", $"{invoice.Subtotal:N0}");
            AddTotalRow("Item Discount", $"-{invoice.TotalDiscount:N0}");
            if (invoice.GlobalDiscountPercent > 0)
                AddTotalRow($"Global Discount ({invoice.GlobalDiscountPercent:N0}%)", $"-{invoice.GlobalDiscountValue:N0}");

            AddTotalRow("Grand Total", $"{invoice.GrandTotal:N0}", true);
            AddTotalRow("Cashback", $"{invoice.Cashback:N0}");


            flowLayoutPanel.Controls.Add(totalsTable);


            // ================= Global Note & Delivery =================
            if (!string.IsNullOrEmpty(invoice.GlobalNote) || invoice.DeliveryAmount > 0)
            {
                flowLayoutPanel.Controls.Add(new Label { Text = "------------------------------", AutoSize = false, Height = 1, BackColor = System.Drawing.Color.Gray });

                if (!string.IsNullOrEmpty(invoice.GlobalNote))
                    flowLayoutPanel.Controls.Add(new Label { Text = $"Note: {invoice.GlobalNote}", AutoSize = true, Padding = new Padding(5, 2, 5, 2) });

                // ✅ tampilkan nominal ongkir juga kalau ada
                if (invoice.DeliveryAmount > 0)
                    flowLayoutPanel.Controls.Add(new Label { Text = $"Delivery : {invoice.DeliveryAmount:N0}", AutoSize = true, Padding = new Padding(5, 2, 5, 2) });

                //if (!string.IsNullOrEmpty(invoice.DeliveryAddress))
                //    flowLayoutPanel.Controls.Add(new Label { Text = $"Alamat     : {invoice.DeliveryAddress}", AutoSize = true, Padding = new Padding(5, 2, 5, 2) });

                //if (!string.IsNullOrEmpty(invoice.DeliveryMethod))
                //    flowLayoutPanel.Controls.Add(new Label { Text = $"Metode     : {invoice.DeliveryMethod}", AutoSize = true, Padding = new Padding(5, 2, 5, 2) });

                //if (!string.IsNullOrEmpty(invoice.DeliveryPhone))
                //    flowLayoutPanel.Controls.Add(new Label { Text = $"Telp       : {invoice.DeliveryPhone}", AutoSize = true, Padding = new Padding(5, 2, 5, 2) });
            }

            // ================= Update label utama =================
            labelNumOfItems.Text = totalQty.ToString();
            label2.Text = $"{invoice.GrandTotal:N0}";
            labelKembalian.Text = $"{invoice.Cashback:N0}";
        }






        private void SimpanReceiptKeDatabase(string receiptMessage)
        {

            using (var conn = new Npgsql.NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();

                string sql = "INSERT INTO receipt_logs (message) VALUES (@message)";
                using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@message", receiptMessage);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void PrintToPOS58(string receiptText)
        {
            try
            {
                using (SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One))
                {
                    port.Open();
                    port.WriteLine(receiptText);

                    // ESC/POS Cut Paper command (jika printer support)
                    port.Write(new byte[] { 0x1D, 0x56, 0x01 }, 0, 3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal print: " + ex.Message);
            }
        }



        private void SaveReceiptToPdf(string receiptText, string fileName = null)
        {
            try
            {
                // Lokasi folder untuk simpan otomatis
                string receiptsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");
                if (!Directory.Exists(receiptsFolder))
                    Directory.CreateDirectory(receiptsFolder);

                // Nama file default
                string safeFileName = fileName ?? $"Receipt_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string pdfPath = Path.Combine(receiptsFolder, safeFileName);

                QuestPDF.Settings.License = LicenseType.Community;

                // Buat PDF dari text struk
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        // Ukuran thermal 58mm (sekitar 2.28 inch)
                        page.Size(150, PageSizes.A4.Height);
                        page.Margin(10);
                        page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Courier New"));

                        page.Content().Column(col =>
                        {
                            string[] lines = receiptText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                            foreach (var line in lines)
                            {
                                col.Item().Text(line).LineHeight(1.2f);
                            }
                        });
                    });
                })
                .GeneratePdf(pdfPath);

                // Bisa kamu buka otomatis:
                // Process.Start("explorer.exe", pdfPath);

                // Simpan log
                Debug.WriteLine($"Receipt PDF saved: {pdfPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuat PDF struk: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogReceipt(string receiptText)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "printlog.txt");
                File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - PRINT\n{receiptText}\n\n");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Gagal simpan log print: " + ex.Message);
            }
        }




        //private void GetRandomItemByBarcode()
        //{
        //    // Your logic here, for example:
        //    Item randomItem = itemController.GetItemByBarcode();
        //    if (randomItem != null)
        //    {
        //        MessageBox.Show($"Found item: {randomItem.name} - {randomItem.barcode}");
        //    }
        //    else
        //    {
        //        MessageBox.Show("No item found!");
        //    }
        //}
        private void TxtCariBarang_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;

                    string barcode = txtCariBarang.Text.Trim();
                    if (!string.IsNullOrEmpty(barcode))
                    {
                        ProcessItem(barcode);
                        txtCariBarang.Clear(); // Bersihkan agar siap untuk scan berikut
                    }
                }

                if (e.Control && e.KeyCode == Keys.S)
                {
                    string barcode = txtCariBarang.Text.Trim();
                    e.SuppressKeyPress = true;
                    ProcessRandomItem();
                }

                if (e.Control && e.KeyCode == Keys.F)
                {
                    MessageBox.Show("");
                    SearchAndAddItem();
                }
                if (e.Control && e.KeyCode == Keys.I)
                {
                    BtnToggleInfo.PerformClick(); // Jalankan event Click tombol
                }
                if (e.KeyCode == Keys.F12) // Pressing F12 triggers payment
                {
                    e.SuppressKeyPress = true; // Prevents default behavior
                                               //print();
                    btnPay.PerformClick(); // Simulates button click
                }

                if (e.Control && e.KeyCode == Keys.P) // Ctrl + P also triggers payment
                {
                    e.SuppressKeyPress = true;
                    //print();
                    btnPay.PerformClick();
                }

                if (e.Control && e.KeyCode == Keys.O) // Ctrl + P also triggers payment
                {
                    e.SuppressKeyPress = true;
                    //print();
                    buttonOrders.PerformClick();
                }

                if (e.Control && e.KeyCode == Keys.L) // Ctrl + P also triggers payment
                {
                    e.SuppressKeyPress = true;
                    //print();
                    buttonListOrders.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void ProcessItem(string barcode)
        {
            var randomItem = itemController.GetItemByBarcode(barcode);
            if (randomItem == null)
            {
                MessageBox.Show("No random item found.");
                return;
            }

            // Try to update existing row
            if (UpdateExistingItem(randomItem))
            {
                // Hanya update UI baru
                UpdateInvoiceUI();
                return;
            }

            // Insert new pending transaction
            if (InsertPendingTransaction(randomItem))
            {
                // Add to DataGridView
                AddNewItemToGrid(randomItem);

            }else
            {
                MessageBox.Show("Failed to insert random item transaction.");
                return;
            }



                // Update UI baru
                UpdateInvoiceUI();
        }


        private void ProcessRandomItem()
        {
            var randomItem = itemController.GetRandomItemByBarcode();
            if (randomItem == null)
            {
                MessageBox.Show("No random item found.");
                return;
            }

            bool isExistingUpdated = UpdateExistingItem(randomItem);


            //MessageBox.Show("isExistingUpdated outside" + isExistingUpdated);

            if (!isExistingUpdated)
            {
                //MessageBox.Show("isExistingUpdated" + isExistingUpdated);
                // Hanya insert baru jika item belum ada
                bool insertSuccess = InsertPendingTransaction(randomItem);
                if (insertSuccess)
                {
                    AddNewItemToGrid(randomItem); // tambah row baru
                }
                else
                {
                    MessageBox.Show("Failed to insert random item transaction.");
                    return;
                }
            }

            // Update UI selalu dijalankan, baik update existing atau insert baru
            UpdateInvoiceUI();
        }



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

                    bool isExistingUpdated = UpdateExistingItem(selectedItem);


                    //MessageBox.Show("isExistingUpdated outside" + isExistingUpdated);

                    if (!isExistingUpdated)
                    {
                        //MessageBox.Show("isExistingUpdated" + isExistingUpdated);
                        // Hanya insert baru jika item belum ada
                        bool insertSuccess = InsertPendingTransaction(selectedItem);
                        if (insertSuccess)
                        {
                            AddNewItemToGrid(selectedItem); // tambah row baru
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert serachforitem transaction.");
                            return;
                        }
                    }

                    // Add a new row in DataGridView
                    UpdateInvoiceUI(); // <-- ganti CalculateAllTotals()
                    MessageBox.Show("Transaction inserted successfully.");
                }
                else
                {
                    MessageBox.Show("No item selected.");
                }
            }
        }


        /// <summary>
        /// Insert item ke pending_transactions dengan logging jika gagal
        /// </summary>
        private bool InsertPendingTransaction(dynamic selectedItem)
        {
            try
            {
                bool result = itemController.AddPendingTransaction(
                    SessionUser.GetCurrentUser().TerminalId,
                    SessionUser.GetCurrentUser().UserId,
                    (int)selectedItem.id,
                    selectedItem.barcode,
                    selectedItem.unit,
                    selectedItem.stock,
                    selectedItem.sell_price,
                    0,
                    0,
                    0,
                    selectedItem.stock * selectedItem.sell_price,
                    "",
                    cart_session_code
                );

                if (result)
                {
                    // Log sukses tambah item ke cart
                    activityService.LogAction(
                        userId: SessionUser.GetCurrentUser().UserId.ToString(),
                        actionType: ActivityType.Cart.ToString(),
                        referenceId: null,
                        desc: $"Successfully added Item: {selectedItem.name} to cart. CSC:{cart_session_code} with ID: {selectedItem.id}, Barcode: {selectedItem.barcode}",
                        details: new
                        {
                            loginId = SessionUser.GetCurrentUser().LoginId,
                            itemId = selectedItem.id,
                            adjustmentType = "ADD_ITEM",
                            reason = "default reason",
                            referenceTable = "items",
                            terminal = SessionUser.GetCurrentUser().TerminalId,
                            shiftId = SessionUser.GetCurrentUser().ShiftId,
                            IpAddress = NetworkHelper.GetLocalIPAddress(),
                            UserAgent = GlobalContext.getAppVersion(),
                            userId = SessionUser.GetCurrentUser().UserId.ToString(),
                            selectedItem
                        }
                    );
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log error sesuai format
                activityService.LogAction(
                    userId: SessionUser.GetCurrentUser().UserId.ToString(),
                    actionType: ActivityType.Cart.ToString(),
                    referenceId: null,
                    desc: $"Failed to insert Item: {selectedItem.name} to cart. CSC:{cart_session_code} with ID: {selectedItem.id}, Barcode: {selectedItem.barcode}. Error: {ex.Message}",
                    details: new
                    {
                        loginId = SessionUser.GetCurrentUser().LoginId,
                        itemId = selectedItem.id,
                        adjustmentType = "ADD_ITEM_FAILED",
                        reason = ex.Message,
                        referenceTable = "items",
                        terminal = SessionUser.GetCurrentUser().TerminalId,
                        shiftId = SessionUser.GetCurrentUser().ShiftId,
                        IpAddress = NetworkHelper.GetLocalIPAddress(),
                        UserAgent = GlobalContext.getAppVersion(),
                        userId = SessionUser.GetCurrentUser().UserId.ToString(),
                        selectedItem,
                        Exception = ex.ToString()
                    }
                );

                return false;
            }
        }

        /// <summary>
        /// Adds a new item as a row in the DataGridView.
        /// </summary>
        private void AddNewItemToGrid(dynamic selectedItem)
        {
            //MessageBox.Show("AddNewItemToGrid called");
            // Hitung conversion text, asumsi unit variant dipakai (kalau tidak, default 1 pcs = 1 pcs)
            string conversionInfo = $"{selectedItem.unit} = {(selectedItem.conversion ?? 1)} pcs";

            // Harga per pcs — kalau tidak pakai unit variant, maka langsung ambil sell_price
            decimal pricePerPcs = (selectedItem.conversion != null && selectedItem.conversion > 0)
                ? selectedItem.sell_price / selectedItem.conversion
                : selectedItem.sell_price;

            dataGridViewCart4.Rows.Add(
                selectedItem.id,
                selectedItem.barcode,
                selectedItem.name,
                selectedItem.stock,                             // Qty (misal 2 dus)
                selectedItem.unit,                              // Dus
                $"{selectedItem.unit} = {selectedItem.conversion} pcs",  // Kolom tambahan info konversi
                selectedItem.sell_price,                        // Harga per dus
                selectedItem.price_per_pcs,                     // Harga per pcs
                selectedItem.price_per_pcs_asli,
                0,                                              // Diskon
                0,                                              // Pajak
                selectedItem.stock * selectedItem.sell_price,   // Total
                "",                                              // Note
                selectedItem.conversion

            );

            activityService.LogAction(
            userId: SessionUser.GetCurrentUser().UserId.ToString(),
            actionType: ActivityType.Cart.ToString(),
                referenceId: null,
                desc: $"Successfully added Item: {selectedItem.name} to cart. CSC:{cart_session_code} with ID: {selectedItem.id}, Barcode: {selectedItem.barcode}",
                details: new
                {
                    loginId = SessionUser.GetCurrentUser().LoginId,
                    itemId = selectedItem.id,
                    adjustmentType = "ADD_ITEM",
                    reason = "default reason",
                    referenceTable = "items",
                    terminal = SessionUser.GetCurrentUser().TerminalId,
                    shiftId = SessionUser.GetCurrentUser().ShiftId,
                    IpAddress = NetworkHelper.GetLocalIPAddress(),
                    UserAgent = GlobalContext.getAppVersion(),
                    userId = SessionUser.GetCurrentUser().UserId.ToString(),
                    selectedItem
                    //TsCode = transaction.TsCode,
                    //TotalAmount = transaction.TsTotal,
                    //PaymentMethod = transaction.TsMethod,
                    //OrderId = transaction.OrderId
                }
            );

            //activityService.LogAction(ActivityType.Cart.ToString(), $"Successfully added Item: {selectedItem.name} to cart. ID: {selectedItem.id}, Barcode: {selectedItem.barcode}", selectedItem);

        }


        private void SetupDataGridView()
        {
            dataGridViewCart4.Columns.Clear();

            string[] columnNames = {
        "id", "barcode", "Nama Barang", "qty", "Satuan",
        "Conversion", "Harga/Unit", "Harga/Satuan Utama","Harga/Satuan Utama Asli", "Pot(%)",
        "Pajak", "Total", "Keterangan Per Item","conversion"
    };

            string[] propertyNames = {
        "id", "barcode", "name", "stock", "unit",
        "conversion_info", "sell_price", "price_per_pcs","price_per_pcs_asli", "discount",
        "tax", "total", "note","conversion"
    };

            bool[] readOnlyColumns = {
        true, true, true, false, true,
        true, true, true, true, false,
        true, true, false, true
    };

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

            dataGridViewCart4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dataGridViewCart4.Columns["barcode"].Width = 150;
            dataGridViewCart4.Columns["name"].Width = 250;
            dataGridViewCart4.Columns["sell_price"].Width = 120;
            dataGridViewCart4.Columns["conversion_info"].Width = 130;
            dataGridViewCart4.Columns["price_per_pcs"].Width = 120;

            dataGridViewCart4.Columns["name"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Format angka ke Rp. xxx.xxx.xxx
            dataGridViewCart4.CellFormatting += (s, e) =>
            {
                string columnName = dataGridViewCart4.Columns[e.ColumnIndex].Name;

                if (columnName == "sell_price" ||
                    columnName == "price_per_pcs" ||
                    columnName == "price_per_pcs_asli" ||
                    columnName == "total")
                {
                    if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal number))
                    {
                        e.Value = "Rp. " + number.ToString("N0");
                        e.FormattingApplied = true;
                    }
                }
            };

            dataGridViewCart4.RowPostPaint += DataGridViewCart4_RowPostPaint;
            dataGridViewCart4.CellValueChanged += DataGridViewCart4_CellValueChanged;
            dataGridViewCart4.UserDeletingRow += dataGridViewCart4_UserDeletingRow;
            dataGridViewCart4.RowsRemoved += DataGridViewCart4_RowsRemoved;
            dataGridViewCart4.CellEndEdit += dataGridViewCart4_CellEndEdit;
            dataGridViewCart4.KeyDown += DataGridViewCart4_KeyDown;
            dataGridViewCart4.CellBeginEdit += dataGridViewCart4_CellBeginEdit;
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
                //print();
                btnPay.PerformClick(); // Simulates button click
            }

            if (e.Control && e.KeyCode == Keys.P) // Ctrl + P also triggers payment
            {
                e.SuppressKeyPress = true;
                //print();
                btnPay.PerformClick();
            }
        }


        //Set nilai Tag sebelum perubahan, biasanya di event CellBeginEdit atau CellEnter, misalnya:
        private void dataGridViewCart4_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //MessageBox.Show("dataGridViewCart4_CellBeginEdit called");
            if (e.RowIndex < 0 || e.ColumnIndex != dataGridViewCart4.Columns["stock"].Index) return;

            var row = dataGridViewCart4.Rows[e.RowIndex];
            row.Cells["stock"].Tag = row.Cells["stock"].Value;  // Simpan nilai awal sebelum edit
        }

        private void DataGridViewCart4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isProgrammaticChange) return;
            if (e.RowIndex < 0 || e.ColumnIndex != dataGridViewCart4.Columns["stock"].Index) return;

            try
            {
                var row = dataGridViewCart4.Rows[e.RowIndex];
                int manualInput = Convert.ToInt32(row.Cells["stock"].Value);
                ProcessItemStockUpdate(row, manualInput, allowAppend: false);

                // Ganti CalculateAllTotals() dengan UpdateInvoiceUI()
                UpdateInvoiceUI();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                var row = dataGridViewCart4.Rows[e.RowIndex];
                row.Cells["stock"].Value = row.Cells["stock"].Tag ?? 1;
            }
        }


        private bool UpdateExistingItem(dynamic selectedItem)
        {
           
            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
            {
                if (row.Cells["id"].Value != null && Convert.ToInt32(row.Cells["id"].Value) == selectedItem.id && row.Cells["unit"].Value.ToString() == selectedItem.unit)
                {
                    //MessageBox.Show("unit : " + row.Cells["unit"].Value.ToString());
                    int qtyToAdd = Convert.ToInt32(selectedItem.stock);
                    return ProcessItemStockUpdate(row, qtyToAdd, allowAppend: true);
                }
            }
            return false;
        }


        private bool isProgrammaticChange = false;

        private bool ProcessItemStockUpdate(DataGridViewRow row, int additionalQuantity, bool allowAppend)
        {
            //MessageBox.Show("ProcessItemStockUpdate");
            if (row.Cells["id"].Value == null) return false;

            int itemId = Convert.ToInt32(row.Cells["id"].Value);
            string barcode = row.Cells["barcode"].Value.ToString();
            string unit = row.Cells["unit"].Value.ToString();
            string discount = row.Cells["discount"].Value.ToString();
            string note = row.Cells["note"].Value.ToString();

            // untuk case edit qty dari cart
            int previousQuantity = row.Cells["stock"].Tag != null ? Convert.ToInt32(row.Cells["stock"].Tag) : 0;

            // jika allowAppend true.. (berarti tambahkan dari qty yg di ambil cari searchforitem)
            // jika false berarti user ganti dari dlm car
            int enteredQuantity = 0;
            if (allowAppend)
            {
                enteredQuantity = Convert.ToInt32(row.Cells["stock"].Value) + additionalQuantity;
            }
            else
            {
                enteredQuantity = additionalQuantity;
            }

            if (enteredQuantity <= 0)
            {
                MessageBox.Show("Quantity must be at least 1.");
                return false;
            }

            int conversionRate = 1;
            if (row.Cells["conversion"].Value != null && int.TryParse(row.Cells["conversion"].Value.ToString(), out int conv))
            {
                if (conv > 0) conversionRate = conv;
            }

            int stockNeededOld = previousQuantity * conversionRate;
            int stockNeededNew = enteredQuantity * conversionRate;

            int currentStock = itemController.GetItemStock(barcode);

            int reservedStock = itemController.GetItemReservedStock(barcode);
            int newReservedStock = 0;
            // jika ditambahkan dari searchforitem..
            // bukan di edit qtynya lansung dari cart
            if (allowAppend == true)
            {
                newReservedStock = reservedStock;
            }
            else
            {
                newReservedStock = reservedStock - stockNeededOld + stockNeededNew;
            }

            if (newReservedStock > currentStock)
            {
                MessageBox.Show("Stock tidak cukup. Jumlah ini melebihi sisa stock yang tersedia.");
                return false;
            }

            decimal pricePerUnit = Convert.ToDecimal(row.Cells["sell_price"].Value);
            decimal calculatedTotal = enteredQuantity * pricePerUnit;

            // update berdasarkan satuan yang sama
            //MessageBox.Show("about to UpdatePendingTransactionSto ck");
            bool updateSuccess = itemController.UpdatePendingTransactionStock(SessionUser.GetCurrentUser().TerminalId, itemId, enteredQuantity, calculatedTotal, unit,cart_session_code);
            if (!updateSuccess)
            {
                MessageBox.Show("Failed to update stock in pending transactions.");
                return false;
            }
   
            itemController.UpdateReservedStock(barcode, newReservedStock);

            isProgrammaticChange = true;
            row.Cells["stock"].Value = enteredQuantity;
            row.Cells["total"].Value = calculatedTotal;
            isProgrammaticChange = false;

            row.Cells["stock"].Tag = enteredQuantity;

            activityService.LogAction(
            userId: SessionUser.GetCurrentUser().UserId.ToString(),
            actionType: ActivityType.Cart.ToString(),
                referenceId: null,
                desc: $"Successfully update Item in cart: {itemId} CSC {cart_session_code} newreqstock : {newReservedStock}, prevreservedstock: {stockNeededOld}",
                details: new
                {
                    loginId = SessionUser.GetCurrentUser().LoginId,
                    itemId = itemId,
                    adjustmentType = "UPDATE_ITEM_IN_CART",
                    reason = "default reason",
                    referenceTable = "items",
                    terminal = SessionUser.GetCurrentUser().TerminalId,
                    shiftId = SessionUser.GetCurrentUser().ShiftId,
                    IpAddress = NetworkHelper.GetLocalIPAddress(),
                    UserAgent = GlobalContext.getAppVersion(),
                    Discount = discount,
                    Note = note,
                    cart_session_code = cart_session_code
                    //TsCode = transaction.TsCode,
                    //TotalAmount = transaction.TsTotal,
                    //PaymentMethod = transaction.TsMethod,
                    //OrderId = transaction.OrderId
                }
            );

            UpdateInvoiceUI();
            MessageBox.Show("done ProcessItemStockUpdate about to return true");
            return true;
        }



        private void dataGridViewCart4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridViewCart4.Rows[e.RowIndex];

            if (dataGridViewCart4.Columns[e.ColumnIndex].Name == "discount")
            {
                decimal discountPercentage = Convert.ToDecimal(row.Cells["discount"].Value);
                int itemId = Convert.ToInt32(row.Cells["id"].Value);

                bool updateSuccess = itemController.UpdatePendingTransactionDiscount(SessionUser.GetCurrentUser().TerminalId, itemId, discountPercentage);
                if (updateSuccess)
                {
                    RecalculateTotalForRow(row);
                    UpdateInvoiceUI();
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

                bool updateSuccess = itemController.UpdatePendingTransactionNote(SessionUser.GetCurrentUser().TerminalId, itemId, note);
                if (!updateSuccess)
                {
                    MessageBox.Show("Failed to update note in database.");
                }
                else
                {
                    RecalculateTotalForRow(row);
                    UpdateInvoiceUI();
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
            UpdateInvoiceUI();
            txtCariBarang.Focus();
        }

        private void dataGridViewCart4_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var row = e.Row;
            if (row.Cells["id"].Value == null) return;

            int itemId = Convert.ToInt32(row.Cells["id"].Value);
            string barcode = row.Cells["barcode"].Value.ToString();
            // Pakai nilai asli yang sebelumnya disimpan di Tag, supaya valid
            int previousQuantity = 0;
            if (row.Cells["stock"].Tag != null && int.TryParse(row.Cells["stock"].Tag.ToString(), out int parsed))
            {
                previousQuantity = parsed;
            }
            else
            {
                // fallback kalau Tag kosong
                previousQuantity = Convert.ToInt32(row.Cells["stock"].Value);
            }
            int conversionRate = 1;

            if (row.Cells["conversion"].Value != null && int.TryParse(row.Cells["conversion"].Value.ToString(), out int conv))
            {
                if (conv > 0)
                    conversionRate = conv;
            }

            int stockNeeded = previousQuantity * conversionRate;

            // Delete dari pending_transactions
            bool deleteSuccess = itemController.DeletePendingTransaction(1, itemId);

            if (deleteSuccess)
            {
                // Kembalikan reserved_stock saja
                int reservedStock = itemController.GetItemReservedStock(barcode);
                int newReservedStock = reservedStock - stockNeeded;

                if (newReservedStock < 0) newReservedStock = 0; // proteksi biar tidak minus

                itemController.UpdateReservedStock(barcode, newReservedStock);

                activityService.LogAction(
                  userId: SessionUser.GetCurrentUser().UserId.ToString(),
                  actionType: ActivityType.Cart.ToString(),
                      referenceId: null,
                      desc: $"Successfully delete Item in cart: {itemId}",
                      details: new
                      {
                          itemId = itemId,
                          adjustmentType = "DELETE_ITEM_FROM_CART",
                          reason = "default reason",
                          referenceTable = "items",
                          terminal = SessionUser.GetCurrentUser().TerminalId,
                          shiftId = SessionUser.GetCurrentUser().ShiftId,
                          IpAddress = NetworkHelper.GetLocalIPAddress(),
                          UserAgent = GlobalContext.getAppVersion(),
                          loginId = SessionUser.GetCurrentUser().LoginId,
                          cart_session_code = cart_session_code

                      }
                  );

                MessageBox.Show("Row deleted & reserved stock updated.");
            }
            else
            {
                MessageBox.Show("Failed to delete item from pending transactions.");
                e.Cancel = true; // Gagal update database, batalkan penghapusan dari DataGridView
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




        private void DataGridViewCart4_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dataGridViewCart4.RowHeadersWidth, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridViewCart4.Font, rect, System.Drawing.Color.Black, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }



        private void btnOrder_Click(object sender, EventArgs e)
        {
            // 🔍 Cek apakah keranjang kosong
            bool isCartEmpty = true;
            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
            {
                if (!row.IsNewRow && row.Cells["barcode"].Value != null)
                {
                    isCartEmpty = false;
                    break;
                }
            }

            if (isCartEmpty)
            {
                MessageBox.Show("Keranjang masih kosong. Tambahkan barang terlebih dahulu sebelum membuat order.",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCariBarang.Focus();
                return;
            }

            // ✅ Lanjut jika cart tidak kosong
            var result = MessageBox.Show("Apakah Anda ingin menyimpan data sebagai Pesanan/Bon/Utang/Nota ?",
                "Konfirmasi", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                return; // Batalkan
            }

            // Kalau No, bisa diarahkan ke mode input manual (jika kamu punya logika khusus)
            using (OrderForm orderModal = new OrderForm())
            {
                if (orderModal.ShowDialog() == DialogResult.OK)
                {
                    // Ambil data order dari form
                    Orders order = orderModal.GetOrder();

                    OrderController controller = new OrderController();
                    if (controller.SaveOrderWithDetails(order, 1, 100, out string errMsg))
                    {
                        // Bersihkan cart
                        dataGridViewCart4.Rows.Clear();
                        label2.Text = "0.00";
                        txtCariBarang.Focus();

                        MessageBox.Show("Order berhasil disimpan!", "Sukses",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh badge/order list jika perlu
                        UpdateOrderBadge();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menyimpan order: " + errMsg, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }




        private void LoadOrderToCart(int orderId, bool isReadOnly = false)
        {
            string query = @"
        SELECT 
            od.item_id,
            od.od_barcode,
            i.name,
            od.od_quantity,
            od.od_unit,
            od.od_conversion_rate,
            od.od_price_per_unit,
            (od.od_price_per_unit / NULLIF(od.od_conversion_rate,1)) AS price_per_pcs,
            (i.sell_price / NULLIF(od.od_conversion_rate,1)) AS price_per_pcs_asli,
            od.od_discount_percentage,
            od.od_tax,
            od.od_total,
            od.od_note
        FROM order_details od
        JOIN items i ON i.id = od.item_id
        WHERE od.order_id = @order_id
        ORDER BY od.order_detail_id ASC;
    ";

            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                using (var cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("order_id", orderId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dataGridViewCart4.Rows.Clear();

                        while (reader.Read())
                        {
                            dataGridViewCart4.Rows.Add(
                                reader["item_id"],                // id
                                reader["od_barcode"],             // barcode
                                reader["name"],                   // Nama Barang
                                reader["od_quantity"],            // qty
                                reader["od_unit"],                // Satuan
                                reader["od_conversion_rate"],     // Conversion
                                reader["od_price_per_unit"],      // Harga/Unit
                                reader["price_per_pcs"],          // Harga/Satuan Utama
                                reader["price_per_pcs_asli"],     // Harga/Satuan Utama Asli
                                reader["od_discount_percentage"], // Pot(%)
                                reader["od_tax"],                 // Pajak
                                reader["od_total"],               // Total
                                reader["od_note"],                // Keterangan Per Item
                                reader["od_conversion_rate"]      // conversion
                            );
                        }
                    }
                }
            }

            UpdateInvoiceUI();

            // 🔒 Jika mode readonly aktif
            if (isReadOnly)
            {
                foreach (DataGridViewColumn col in dataGridViewCart4.Columns)
                {
                    col.ReadOnly = true;
                    col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray; // opsional: biar kelihatan beda
                }

                dataGridViewCart4.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightGray;
                dataGridViewCart4.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            }
        }


        public int? SelectedOrderId = null;
        private void buttonListOrders_Click(object sender, EventArgs e)
        {
            using (OrderLIst frm = new OrderLIst())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (frm.SelectedOrderId > 0)
                    {
                        SelectedOrderId = frm.SelectedOrderId;
                        LoadOrderToCart(frm.SelectedOrderId, true);
                    }
                }
            }
        }
        private void UpdateOrderBadge()
        {
            OrderController controller = new OrderController();
            int pendingOrders = controller.GetPendingOrdersCount();

            // Kalau ada order pending, tampilkan badge
            if (pendingOrders > 0)
            {
                lblOrderBadge.Text = pendingOrders.ToString();
                lblOrderBadge.Visible = true;
            }
            else
            {
                lblOrderBadge.Visible = false;
            }
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void labelNumOfItems_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BtnToggleInfo_Click(object sender, EventArgs e)
        {
            // 🔹 Lokasi file di D:
            string filePath = @"D:\shortcuts.txt";

            // 🔹 Jika file belum ada, buat dengan isi default
            if (!System.IO.File.Exists(filePath))
            {
                string defaultShortcuts =
        @"F12  - Bayar
Ctrl+P - Bayar
Ctrl+J - Print to EPPOS
Ctrl+I - Info
Ctrl+S - Search Random Item
";

                System.IO.File.WriteAllText(filePath, defaultShortcuts);
            }

            // 🔹 Baca isi file dan tampilkan di label
            infoLabel.Text = System.IO.File.ReadAllText(filePath);

            // 🔹 Daftar posisi yang dirotasi
            Point[] positions = new Point[]
            {
        new Point(278, 244),
        new Point(50, 50),
        new Point(600, 50),
        new Point(50, 400),
        new Point(600, 400)
            };

            // 🔹 Simpan posisi index di Tag biar tetap ingat antar klik
            if (infoPanel.Tag == null) infoPanel.Tag = 0;
            int index = (int)infoPanel.Tag;
            infoPanel.Location = positions[index];
            infoPanel.Tag = (index + 1) % positions.Length;

            // 🔹 Tampilkan / sembunyikan
            infoPanel.Visible = !infoPanel.Visible;
        }

        private void buttonDraft_Click(object sender, EventArgs e)
        {
            try
            {
                decimal totalAmount = Convert.ToDecimal(label2.Text);
                if (totalAmount <= 0)
                {
                    MessageBox.Show("Cart is empty. Cannot save draft.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 🔹 Tampilkan konfirmasi dulu
                var confirmResult = MessageBox.Show(
                    "Apakah anda akan menyimpan transaksi ini sebagai draft?",
                    "Confirm Save Draft",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult != DialogResult.Yes)
                {
                    // User pilih No → batalkan
                    return;
                }

                var sessionUser = SessionUser.GetCurrentUser();

                // Ambil data invoice dari cart (tanpa menghitung payment)
                InvoiceData invoice = CalculateInvoiceDataFromCart();

                // 1️⃣ Insert ke pending_orders
                int poId = itemController.SaveDraftOrder(
                    terminalId: sessionUser.TerminalId,
                    cashierId: sessionUser.UserId,
                    customerName: "Guest",
                    note: invoice.GlobalNote,
                    total: invoice.GrandTotal,
                    globalDiscount: invoice.GlobalDiscountValue,
                    csc: cart_session_code
                );

                // 2️⃣ Reset cart dan generate session code baru
                ResetCart();

                MessageBox.Show($"Draft saved successfully!\nPO ID: {poId}", "Draft Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save draft: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void LoadDraftToCart(int poId, bool isReadOnly = false)
        {
            // 1️⃣ Ambil item-item dari pending_transactions berdasarkan draft poId
            string query = @"
    SELECT 
        pt.item_id,
        pt.barcode,
        i.name,
        pt.quantity,
        pt.unit,
        pt.total / NULLIF(pt.quantity,0) AS price_per_unit,
        pt.sell_price AS sell_price_per_unit,
        pt.discount_percentage,
        pt.tax,
        pt.total,
        pt.note
    FROM pending_transactions pt
    JOIN items i ON i.id = pt.item_id
    WHERE pt.terminal_id = @terminal_id AND pt.cashier_id = @cashier_id
    ORDER BY pt.pt_id ASC;
";

            var sessionUser = SessionUser.GetCurrentUser(); // asumsi terminal & cashier aktif

            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                using (var cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("terminal_id", sessionUser.TerminalId); // terminal aktif
                    cmd.Parameters.AddWithValue("cashier_id", sessionUser.UserId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dataGridViewCart4.Rows.Clear();

                        while (reader.Read())
                        {
                            dataGridViewCart4.Rows.Add(
                                reader["item_id"],                  // id
                                reader["barcode"],                  // barcode
                                reader["name"],                     // Nama Barang
                                reader["quantity"],                 // qty
                                reader["unit"],                     // Satuan
                                1,                                  // Conversion default = 1
                                reader["sell_price_per_unit"],      // Harga/Unit
                                reader["price_per_unit"],           // Harga/Satuan utama
                                reader["sell_price_per_unit"],      // Harga/Satuan utama asli
                                reader["discount_percentage"],      // Pot(%)
                                reader["tax"],                      // Pajak
                                reader["total"],                    // Total
                                reader["note"],                     // Keterangan Per Item
                                1                                   // conversion
                            );
                        }
                    }
                }
            }

            UpdateInvoiceUI();

            // 🔒 Jika mode readonly aktif
            if (isReadOnly)
            {
                foreach (DataGridViewColumn col in dataGridViewCart4.Columns)
                {
                    col.ReadOnly = true;
                    col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                }

                dataGridViewCart4.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightGray;
                dataGridViewCart4.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            }
        }



        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
