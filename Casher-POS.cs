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


            //var user = SessionUser.GetCurrentUser();
            //string pcId = Utility.GetPcId();
            //labelSessionInfo.Text = $"User: {user.Username} | Terminal: {user.TerminalId} | Shift: {user.ShiftId} |pcid: {pcId}";
            //txtCariBarang.Focus();

            this.Shown += Casher_POS_Shown;
            infoPanel.Visible = false;
            UpdateOrderBadge();
        }

        private void Casher_POS_Shown(object sender, EventArgs e)
        {
            txtCariBarang.Focus();
        }

        public static string GetCurrentShift()
        {
            var hour = DateTime.Now.Hour;
            if (hour >= 6 && hour < 14) return "1";    // Shift Pagi
            if (hour >= 14 && hour < 22) return "2";   // Shift Siang
            return "3";                                // Shift Malam
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

        private void print(Transactions t)
        {
            var sessionUser = SessionUser.GetCurrentUser();

            string receipt = GenerateReceiptText(
                terminal: SessionUser.GetCurrentUser().TerminalName,
                user: SessionUser.GetCurrentUser().Username,
                shift: "shift " + SessionUser.GetCurrentUser().ShiftId,
                statusBayar: "status : " + t.TsStatus,
                jumlahBayar: t.TsPaymentAmount.ToString(),
                metodeBayar: t.TsMethod + " ",
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
            //SimpanReceiptKeDatabase(receipt);
            LogReceipt(receipt);
        }

        private string lastReceipt = ""; // Untuk simpan isi struk terakhir jika perlu akses dari luar

        //private string GenerateReceiptText(string terminal, string user, string shift, string statusBayar,string jumlahBayar, string metodeBayar, string kembalian)
        //{
        //    StringBuilder receipt = new StringBuilder();

        //    // Header toko
        //    receipt.AppendLine("TOKO KITA");
        //    receipt.AppendLine("Jl. Contoh No. 1");
        //    receipt.AppendLine("Telp: 08123456789");
        //    receipt.AppendLine("------------------------------");

        //    // Info transaksi
        //    receipt.AppendLine($"Terminal : {terminal}");
        //    receipt.AppendLine($"Kasir    : {user}");
        //    receipt.AppendLine($"Shift    : {shift}");
        //    receipt.AppendLine($"Tanggal  : {DateTime.Now:yyyy-MM-dd}");
        //    receipt.AppendLine($"Waktu    : {DateTime.Now:HH:mm:ss}");
        //    receipt.AppendLine("------------------------------");

        //    // Jumlah item
        //    receipt.AppendLine($"Jumlah Barang: {labelNumOfItems.Text}");
        //    receipt.AppendLine("------------------------------");

        //    // Isi dari flowLayoutPanel
        //    foreach (Control ctrl in flowLayoutPanel.Controls)
        //    {
        //        if (ctrl is TableLayoutPanel panel)
        //        {
        //            int rowCount = panel.RowCount;
        //            for (int i = 0; i < rowCount; i++)
        //            {
        //                string leftText = "";
        //                string rightText = "";

        //                var leftLabel = panel.GetControlFromPosition(0, i) as Label;
        //                var rightLabel = panel.GetControlFromPosition(1, i) as Label;

        //                if (leftLabel != null) leftText = leftLabel.Text;
        //                if (rightLabel != null) rightText = rightLabel.Text;

        //                if (!string.IsNullOrWhiteSpace(leftText) || !string.IsNullOrWhiteSpace(rightText))
        //                {
        //                    receipt.AppendLine(leftText.PadRight(20).Substring(0, Math.Min(20, leftText.Length)) + rightText.PadLeft(10));
        //                }
        //            }
        //        }
        //        else if (ctrl is Label separator && separator.Height == 1)
        //        {
        //            receipt.AppendLine("------------------------------");
        //        }
        //    }

        //    // Status pembayaran
        //    receipt.AppendLine($"Status Bayar : {statusBayar}");
        //    receipt.AppendLine($"Metode Bayar: {metodeBayar}");
        //    receipt.AppendLine($"Jumlah Bayar: {jumlahBayar}");
        //    receipt.AppendLine($"Kembalian   : {kembalian}");
        //    receipt.AppendLine("------------------------------");

        //    // Footer
        //    receipt.AppendLine("Terima kasih!");
        //    receipt.AppendLine("~ Kunjungi kembali ~");
        //    receipt.AppendLine("");

        //    return receipt.ToString();
        //}

        private string GenerateReceiptText(
    string terminal,
    string user,
    string shift,
    string statusBayar,
    string jumlahBayar,
    string metodeBayar,
    string kembalian)
        {
            StringBuilder receipt = new StringBuilder();

            // === 1️⃣ Ambil data dari database ===
            string judul = "";
            string alamat = "";
            string telepon = "";
            string footer = "";
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
            if (showNamaToko && !string.IsNullOrEmpty(judul))
                receipt.AppendLine(judul);
            if (showAlamat && !string.IsNullOrEmpty(alamat))
                receipt.AppendLine(alamat);
            if (showTelepon && !string.IsNullOrEmpty(telepon))
                receipt.AppendLine(telepon);

            receipt.AppendLine("------------------------------");

            // === 3️⃣ Info transaksi ===
            receipt.AppendLine($"Terminal : {terminal}");
            receipt.AppendLine($"Kasir    : {user}");
            receipt.AppendLine($"Shift    : {shift}");
            receipt.AppendLine($"Tanggal  : {DateTime.Now:yyyy-MM-dd}");
            receipt.AppendLine($"Waktu    : {DateTime.Now:HH:mm:ss}");
            receipt.AppendLine("------------------------------");

            // === 4️⃣ Jumlah item ===
            receipt.AppendLine($"Jumlah Barang: {labelNumOfItems.Text}");
            receipt.AppendLine("------------------------------");

            // === 5️⃣ Detail item (loop flowLayoutPanel) ===
            foreach (Control ctrl in flowLayoutPanel.Controls)
            {
                if (ctrl is TableLayoutPanel panel)
                {
                    int rowCount = panel.RowCount;
                    for (int i = 0; i < rowCount; i++)
                    {
                        var leftLabel = panel.GetControlFromPosition(0, i) as Label;
                        var rightLabel = panel.GetControlFromPosition(1, i) as Label;

                        string leftText = leftLabel?.Text?.Trim() ?? "";
                        string rightText = rightLabel?.Text?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(leftText) || !string.IsNullOrEmpty(rightText))
                        {
                            string left = leftText.Length > 20 ? leftText.Substring(0, 20) : leftText.PadRight(20);
                            string right = rightText.PadLeft(10);
                            receipt.AppendLine(left + right);
                        }
                    }
                }
                else if (ctrl is Label separator && separator.Height == 1)
                {
                    receipt.AppendLine("------------------------------");
                }
            }

            // === 6️⃣ Pembayaran ===
            receipt.AppendLine($"Status Bayar : {statusBayar}");
            receipt.AppendLine($"Metode Bayar : {metodeBayar}");
            receipt.AppendLine($"Jumlah Bayar : {jumlahBayar}");
            receipt.AppendLine($"Kembalian    : {kembalian}");
            receipt.AppendLine("------------------------------");

            // === 7️⃣ Footer dari database ===
            if (showFooter && !string.IsNullOrEmpty(footer))
            {
                receipt.AppendLine(footer);
            }

            receipt.AppendLine("");

            return receipt.ToString();
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
                //MessageBox.Show("Random item quantity updated.");
                CalculateAllTotals();
                return;
            }

            // Insert new pending transaction
            if (!InsertPendingTransaction(randomItem))
            {
                MessageBox.Show("Failed to insert random item transaction.");
                return;
            }

            // Add to DataGridView
            AddNewItemToGrid(randomItem);
            CalculateAllTotals();
            //MessageBox.Show("Random item transaction inserted successfully.");
        }

        private void ProcessRandomItem()
        {
            var randomItem = itemController.GetRandomItemByBarcode();
            if (randomItem == null)
            {
                MessageBox.Show("No random item found.");
                return;
            }

            // Try to update existing row
            if (UpdateExistingItem(randomItem))
            {
                MessageBox.Show("Random item quantity updated.");
                CalculateAllTotals();
                return;
            }

            // Insert new pending transaction
            if (!InsertPendingTransaction(randomItem))
            {
                MessageBox.Show("Failed to insert random item transaction.");
                return;
            }

            // Add to DataGridView
            AddNewItemToGrid(randomItem);
            CalculateAllTotals();
            //MessageBox.Show("Random item transaction inserted successfully.");
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
                    CalculateAllTotals();
                    //UpdateExistingItem(selectedItem);
                    MessageBox.Show("Transaction inserted successfully.");
                }
                else
                {
                    MessageBox.Show("No item selected.");
                }
            }
        }


        /// <summary>
        /// Inserts a new pending transaction into the database.
        /// </summary>
        private bool InsertPendingTransaction(dynamic selectedItem)
        {
            try
            {
                return itemController.AddPendingTransaction(
                    1,  // ini bakal error karena lebih besar dari int.MaxValue!
                    100,
                    (int)selectedItem.id,
                    selectedItem.barcode,
                    selectedItem.unit,
                    selectedItem.stock,
                    selectedItem.sell_price,
                    0,
                    0,
                    0,
                    selectedItem.stock * selectedItem.sell_price,
                    ""
                );
            }
            catch (Exception ex)
            {
                // Log error, tampilkan pesan, atau lakukan fallback
                //activityService.LogAction(ActivityType.Error.ToString(), $" Failed to insert transaction: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Adds a new item as a row in the DataGridView.
        /// </summary>
        private void AddNewItemToGrid(dynamic selectedItem)
        {
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
                desc: $"Successfully added Item: {selectedItem.name} to cart. ID: {selectedItem.id}, Barcode: {selectedItem.barcode}",
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
        false, true, false, true
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
            //MessageBox.Show("DataGridViewCart4_CellValueChanged before");
            if (isProgrammaticChange) return;
            //MessageBox.Show("DataGridViewCart4_CellValueChanged after");
            if (e.RowIndex < 0 || e.ColumnIndex != dataGridViewCart4.Columns["stock"].Index) return;

            try
            {
                var row = dataGridViewCart4.Rows[e.RowIndex];
                int manualInput = Convert.ToInt32(row.Cells["stock"].Value);
                ProcessItemStockUpdate(row, manualInput, allowAppend: false);
                CalculateAllTotals();
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
            //MessageBox.Show("im called");
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
            bool updateSuccess = itemController.UpdatePendingTransactionStock(1, itemId, enteredQuantity, calculatedTotal, unit);
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
                desc: $"Successfully update Item in cart: {itemId} newreqstock : {newReservedStock}, prevreservedstock: {stockNeededOld}",
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
                    //TsCode = transaction.TsCode,
                    //TotalAmount = transaction.TsTotal,
                    //PaymentMethod = transaction.TsMethod,
                    //OrderId = transaction.OrderId
                }
            );

            CalculateAllTotals();
            return true;
        }   



        private void dataGridViewCart4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridViewCart4.Rows[e.RowIndex];

            if (dataGridViewCart4.Columns[e.ColumnIndex].Name == "discount")
            {
                decimal discountPercentage = Convert.ToDecimal(row.Cells["discount"].Value);
                int itemId = Convert.ToInt32(row.Cells["id"].Value);

                bool updateSuccess = itemController.UpdatePendingTransactionDiscount(1, itemId, discountPercentage);
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
                          loginId = SessionUser.GetCurrentUser().LoginId

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

        private void CalculateAllTotals()
        {
            decimal totalCart = 0;
            int numOfItems = 0;
            decimal subTotal = 0;
            decimal totalTax = 0;
            decimal totalDiscount = 0;

            Debug.WriteLine("Recalculating totals...");

            // Bersihkan panel ringkasan biar nggak dobel
            flowLayoutPanel.Controls.Clear();

            // Buat TableLayoutPanel untuk mengatur kolom
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.ColumnCount = 2;  // Kolom pertama untuk nama item, kedua untuk harga
            tableLayoutPanel.RowCount = 0;     // Menyesuaikan dengan jumlah item
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.Dock = DockStyle.Top;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            // Set ukuran kolom: kolom pertama untuk nama item (kiri), kolom kedua untuk harga (kanan)
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70)); // 70% untuk nama item
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // 30% untuk harga

            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
            {
                if (row.Cells["name"].Value == null) continue;

                string itemName = row.Cells["name"].Value.ToString();

                int rowQty = 0;
                if (row.Cells["stock"].Value != null)
                    rowQty = Convert.ToInt32(row.Cells["stock"].Value);

                decimal rowTotal = 0;
                if (row.Cells["total"].Value != null)
                    rowTotal = Convert.ToDecimal(row.Cells["total"].Value);

                decimal rowDiscount = 0;
                if (row.Cells["discount"].Value != null)
                    rowDiscount = Convert.ToDecimal(row.Cells["discount"].Value);

                decimal rowTax = 0;
                if (row.Cells["tax"].Value != null)
                    rowTax = Convert.ToDecimal(row.Cells["tax"].Value);

                // Akumulasi total
                numOfItems += rowQty;
                totalCart += rowTotal;
                totalDiscount += rowDiscount;
                totalTax += rowTax;

                // Tambahkan baris untuk setiap item
                Label itemNameLabel = new Label();
                itemNameLabel.AutoSize = true;
                itemNameLabel.Text = $"{rowQty}x {itemName}"; // Nama item dan qty
                itemNameLabel.TextAlign = ContentAlignment.MiddleLeft; // Rata kiri
                itemNameLabel.Padding = new Padding(5, 2, 5, 2); // Padding sedikit untuk jarak

                Label itemTotalLabel = new Label();
                itemTotalLabel.AutoSize = true;
                itemTotalLabel.Text = $"{rowTotal:N0}"; // Total harga per item
                itemTotalLabel.TextAlign = ContentAlignment.MiddleRight; // Rata kanan
                itemTotalLabel.Padding = new Padding(5, 2, 5, 2); // Padding sedikit untuk jarak

                // Tambahkan ke TableLayoutPanel
                tableLayoutPanel.RowCount++;
                tableLayoutPanel.Controls.Add(itemNameLabel, 0, tableLayoutPanel.RowCount - 1);
                tableLayoutPanel.Controls.Add(itemTotalLabel, 1, tableLayoutPanel.RowCount - 1);

                Debug.WriteLine($"Item: {itemName}, Qty: {rowQty}, Total: {rowTotal:N0}");
            }

            flowLayoutPanel.Controls.Add(tableLayoutPanel); // Menambahkan tableLayoutPanel ke flowLayoutPanel

            // Hitung subtotal sebelum diskon dan pajak
            subTotal = totalCart;

            Debug.WriteLine($"Subtotal: {subTotal}, Discount: {totalDiscount}, Tax: {totalTax}, Grand Total: {totalCart}");

            // Tambahkan pemisah (garis horizontal) setelah list item
            Label separator = new Label();
            separator.AutoSize = false;
            separator.Height = 1;
            separator.Width = flowLayoutPanel.Width;
            separator.BackColor = System.Drawing.Color.Gray; // Warna pemisah
            flowLayoutPanel.Controls.Add(separator);

            // Buat TableLayoutPanel lagi untuk menampilkan subtotal, diskon, pajak, dan total
            TableLayoutPanel totalsTable = new TableLayoutPanel();
            totalsTable.ColumnCount = 2;  // Kolom pertama untuk label (Subtotal, Diskon, dll), kedua untuk nilai
            totalsTable.RowCount = 4;     // 4 baris: Subtotal, Diskon, Pajak, Grand Total
            totalsTable.AutoSize = true;
            totalsTable.Dock = DockStyle.Top;
            totalsTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            // Set ukuran kolom: kolom pertama untuk label, kolom kedua untuk nilai
            totalsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70)); // 70% untuk label
            totalsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // 30% untuk nilai

            // Tambahkan label dan nilai subtotal
            Label subTotalLabel = new Label();
            subTotalLabel.AutoSize = true;
            subTotalLabel.TextAlign = ContentAlignment.MiddleLeft;
            subTotalLabel.Text = $"Subtotal";
            subTotalLabel.Padding = new Padding(10, 5, 10, 5);

            Label subTotalValueLabel = new Label();
            subTotalValueLabel.AutoSize = true;
            subTotalValueLabel.TextAlign = ContentAlignment.MiddleRight;
            subTotalValueLabel.Text = $"{subTotal:N0}";
            subTotalValueLabel.Padding = new Padding(10, 5, 10, 5);

            // Tambahkan label dan nilai diskon
            Label discountLabel = new Label();
            discountLabel.AutoSize = true;
            discountLabel.TextAlign = ContentAlignment.MiddleLeft;
            discountLabel.Text = $"Discount";
            discountLabel.Padding = new Padding(10, 5, 10, 5);

            Label discountValueLabel = new Label();
            discountValueLabel.AutoSize = true;
            discountValueLabel.TextAlign = ContentAlignment.MiddleRight;
            discountValueLabel.Text = $"{totalDiscount:N0}";
            discountValueLabel.Padding = new Padding(10, 5, 10, 5);

            // Tambahkan label dan nilai pajak
            Label taxLabel = new Label();
            taxLabel.AutoSize = true;
            taxLabel.TextAlign = ContentAlignment.MiddleLeft;
            taxLabel.Text = $"Tax";
            taxLabel.Padding = new Padding(10, 5, 10, 5);

            Label taxValueLabel = new Label();
            taxValueLabel.AutoSize = true;
            taxValueLabel.TextAlign = ContentAlignment.MiddleRight;
            taxValueLabel.Text = $"{totalTax:N0}";
            taxValueLabel.Padding = new Padding(10, 5, 10, 5);

            // Tambahkan label dan nilai grand total
            Label grandTotalLabel = new Label();
            grandTotalLabel.AutoSize = true;
            grandTotalLabel.TextAlign = ContentAlignment.MiddleLeft;
            grandTotalLabel.Font = new Font(grandTotalLabel.Font, FontStyle.Bold | FontStyle.Underline);
            grandTotalLabel.Text = $"Grand Total";
            grandTotalLabel.Padding = new Padding(10, 5, 10, 5);

            Label grandTotalValueLabel = new Label();
            grandTotalValueLabel.AutoSize = true;
            grandTotalValueLabel.TextAlign = ContentAlignment.MiddleRight;
            grandTotalValueLabel.Font = new Font(grandTotalValueLabel.Font, FontStyle.Bold | FontStyle.Underline);
            grandTotalValueLabel.Text = $"{totalCart:N0}";
            grandTotalValueLabel.Padding = new Padding(10, 5, 10, 5);

            // Tambahkan semua label dan nilai ke TableLayoutPanel untuk totals
            totalsTable.Controls.Add(subTotalLabel, 0, 0);
            totalsTable.Controls.Add(subTotalValueLabel, 1, 0);
            totalsTable.Controls.Add(discountLabel, 0, 1);
            totalsTable.Controls.Add(discountValueLabel, 1, 1);
            totalsTable.Controls.Add(taxLabel, 0, 2);
            totalsTable.Controls.Add(taxValueLabel, 1, 2);
            totalsTable.Controls.Add(grandTotalLabel, 0, 3);
            totalsTable.Controls.Add(grandTotalValueLabel, 1, 3);

            // Tambahkan totalsTable ke flowLayoutPanel
            flowLayoutPanel.Controls.Add(totalsTable);

            // Update label di layar utama
            labelNumOfItems.Text = numOfItems.ToString();
            label2.Text = $"{totalCart:N0}";
        }




        private void DataGridViewCart4_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dataGridViewCart4.RowHeadersWidth, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridViewCart4.Font, rect, System.Drawing.Color.Black, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }


        private string GenerateTransactionNumber()
        {
            return "TRX-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Apakah Anda ingin menyimpan data sebagai Pesanan/Bon/Utang/Nota ?", "Konfirmasi", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                return; // Batalkan
            }
           
            // Kalau No, lanjut buka form Order untuk input data order baru tanpa simpan dulu

            using (OrderForm orderModal = new OrderForm())
            {
                if (orderModal.ShowDialog() == DialogResult.OK)
                {
                    // Ambil data order dari form
                    Orders order = orderModal.GetOrder();

                    OrderController controller = new OrderController();
                    if (controller.SaveOrderWithDetails(order,1,100, out string errMsg))
                    {

                        // Clear the cart (remove all rows)
                        dataGridViewCart4.Rows.Clear();

                        // Reset total label
                        label2.Text = "0.00";

                        // Set focus back to search box
                        txtCariBarang.Focus();
                        MessageBox.Show("Order berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Lakukan refresh list order atau logic lainnya jika perlu
                    }
                    else
                    {
                        MessageBox.Show("Gagal menyimpan order: " + errMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


            UpdateOrderBadge();
        }

        private void LoadOrderToCart(int orderId)
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

            CalculateAllTotals();
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
                        LoadOrderToCart(frm.SelectedOrderId);
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

                        // Ambil data sesi pengguna
                        var sessionUser = SessionUser.GetCurrentUser();

                        // Tentukan apakah ini transaksi dari pending order
                        int? orderId = null;
                        if (SelectedOrderId > 0)
                        {
                            orderId = SelectedOrderId;

                            // Update order jadi PAID + set metode pembayaran
                            itemController.UpdateOrderPayment(orderId.Value, 1, paymentModal.PaymentMethod);
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
                            //TerminalId = sessionUser.TerminalId,
                            //ShiftId = sessionUser.ShiftId,
                            //UserId = SessionUser.GetCurrentUser().UserId,
                            //CreatedBy = SessionUser.GetCurrentUser().UserId,
                            UserId = 1,
                            CreatedBy = 1, // Replace with GetCurrentUserId() if available
                            TerminalId = 1,
                            ShiftId = 1,
                            CreatedAt = DateTime.UtcNow,
                            OrderId = orderId // ✅ simpan order_id di transaksi
                        };

                        // Log: memulai transaksi
                        activityService.LogAction(
                            userId: sessionUser.UserId.ToString(),
                            actionType: "Transaction_Start",
                            referenceId: null,
                            details: new
                            {
                                loginId = SessionUser.GetCurrentUser().LoginId,
                                TsCode = transaction.TsCode,
                                TotalAmount = transaction.TsTotal,
                                PaymentMethod = transaction.TsMethod,
                                OrderId = transaction.OrderId
                            }
                        );

                        // Insert Transaction and Get Transaction ID
                        int transactionId = itemController.InsertTransaction(transaction);

                        if (transactionId > 0)
                        {
                            List<TransactionDetail> transactionDetails = new List<TransactionDetail>();

                            // Loop through DataGridView and add transaction details
                            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
                            {
                                if (row.Cells["id"].Value == null) continue; // Skip empty rows

                                // Extract values safely
                                int itemId = Convert.ToInt32(row.Cells["id"].Value);
                                string barcode = row.Cells["barcode"].Value?.ToString() ?? ""; // Use string for alphanumeric codes
                                string itemName = row.Cells["name"]?.Value?.ToString() ?? "";
                                decimal quantity = row.Cells["stock"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["stock"].Value);
                                string unit = row.Cells["unit"]?.Value?.ToString() ?? "";
                                decimal sellPrice = decimal.TryParse(row.Cells["sell_price"]?.Value?.ToString(), out decimal sp) ? sp : 0;
                                decimal discountPercentage = decimal.TryParse(row.Cells["discount"]?.Value?.ToString(), out decimal dp) ? dp : 0;
                                decimal tax = decimal.TryParse(row.Cells["tax"]?.Value?.ToString(), out decimal tx) ? tx : 0;
                                int conversionRate = row.Cells["conversion"] != null && int.TryParse(row.Cells["conversion"].Value?.ToString(), out int conv) ? conv : 1;



                                // Calculate discount amount
                                decimal discountTotal = (discountPercentage / 100) * (sellPrice * quantity);
                                decimal finalTotal = (sellPrice * quantity) - discountTotal + tax;

                                // Calculate total quantity based on conversion rate
                                decimal totalQuantityInPcs = quantity * conversionRate;

                                // jika ada conversion
                                // tsdUnit jadi unit utama (misal pcs)
                                // unit varian jadi dus
                                // jika tidak ada conversion unit
                                // tsdUnit tetap unit utama, tapi unit varian 0
                                // conversion 0
                                // tsdpriceperunit diambil dari harga per pcs dari table item
                                // tsdsellprice harga jual yang ada di cart .. misal 1 dus 8000

                                // dapatkan unit utama
                                string mainUnit = "";
                                if (conversionRate != null && conversionRate >= 0)
                                {
                                    mainUnit = itemController.GetItemUnit(itemId);

                                }
                                else
                                {
                                    mainUnit = unit;
                                }


                                decimal pricePerUnit = itemController.GetItemPrice(itemId); // Misalnya, jika unit dus, harga per unit bisa lebih besar

                                // Create TransactionDetail object
                                TransactionDetail detail = new TransactionDetail
                                {
                                    TsId = transactionId,
                                    ItemId = itemId,
                                    Barcode = barcode,
                                    TsdSellPrice = sellPrice, // Save price at the time of transaction
                                    TsdQuantity = quantity, // Save the quantity as entered (e.g., dus)
                                                            //TsdUnit = unit,
                                    TsdUnit = mainUnit,
                                    TsdConversionRate = conversionRate, // Store conversion rate for reference
                                    TsdPricePerUnit = pricePerUnit, // Store price per unit based on unit selected
                                    TsdUnitVariant = unit,


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

                            // Insert Transaction Details
                            if (transactionDetails.Count > 0)
                            {
                                itemController.InsertTransactionDetails(transactionDetails);
                            }

                             // Log: transaksi berhasil
    activityService.LogAction(
        userId: sessionUser.UserId.ToString(),
        actionType: "Transaction_Complete",
        referenceId: transactionId,
        details: new
        {
            loginId = SessionUser.GetCurrentUser().LoginId,
            TsCode = transaction.TsCode,
            TotalAmount = transaction.TsTotal,
            PaymentAmount = transaction.TsPaymentAmount,
            Change = transaction.TsChange,
            Items = transactionDetails.Select(d => new
            {
                d.ItemId,
                d.TsdUnit,
                d.TsdQuantity,
                d.TsdSellPrice
            }).ToList()
        }
    );

                            //////////////////////////////PRINT TRANSACTION//////////////////////

                            print(transaction);  
                          
                            //////////////////////////////END PRINT TRANSACTION//////////////////////



                            // re roll the datagridview
                            // Clear pending-transactions
                            foreach (DataGridViewRow row in dataGridViewCart4.Rows)
                            {
                                if (row.Cells["id"].Value == null) continue; // Skip empty rows
                                int itemId = Convert.ToInt32(row.Cells["id"].Value);
                                string barcode = row.Cells["barcode"].Value?.ToString() ?? "";
                                string unit = row.Cells["unit"].Value?.ToString() ?? "";
                                decimal quantity = row.Cells["stock"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["stock"].Value);
                                int conversion = Convert.ToInt32(row.Cells["conversion"].Value);

                                // Update product stock and reserved stock
                                int rStock = itemController.GetItemReservedStock(itemId);

                                /* 
                                 jika main unit tidak sama dengan unit
                                unit quantity convert ke base
                                cari konversion rate
                                quantity = quantity * conversion rate

                                 */
                                quantity = quantity * conversion;
                                //MessageBox.Show("rstock : " + rStock);
                                rStock = rStock - (int)quantity;

                                int stock = itemController.GetItemStock(itemId);
                                //if ()

                                stock = stock - (int)quantity;

                                itemController.UpdateItemStockAndReservedStock(itemId, stock, rStock);

                                // Clear pending transactions
                                // restore quaantity to unit selected (not base unit or main unit)
                                quantity = quantity / conversion;
                                itemController.ClearPendingTransaction(barcode, quantity, unit);
                            }

                            //print(transaction);

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





    }
}
