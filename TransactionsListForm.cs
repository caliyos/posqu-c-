using POS_qu.Services;
using POS_qu.Repositories;
using POS_qu.Core;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class TransactionsListForm : Form
    {
        private readonly TransactionRepo _repo;
        private readonly TransactionService _service;

        private readonly PrintDocument _printDoc = new PrintDocument();
        private SaleInvoicePrintData _printData;
        private int _printRowCursor;

        public TransactionsListForm()
        {
            _repo = new TransactionRepo();
            _service = new TransactionService(_repo, new ActivityService());
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = true;

            cbStatus.Items.Clear();
            cbStatus.Items.AddRange(new object[] { "Semua", "Sukses", "Retur", "Dibatalkan" });
            cbStatus.SelectedIndex = 1;

            dgvTransactions.SelectionChanged += dgvTransactions_SelectionChanged;
            btnRefresh.Click += (s, e) => LoadData();
            btnExport.Click += BtnExport_Click;
            btnExportDetail.Click += BtnExportDetail_Click;
            btnCancel.Click += BtnCancel_Click;
            btnReturn.Click += BtnReturn_Click;
            btnClose.Click += (s, e) => Close();
            btnPrintPreview.Click += BtnPrintPreview_Click;
            btnExportPdf.Click += BtnExportPdf_Click;
            cbStatus.SelectedIndexChanged += (s, e) => LoadData();

            _printDoc.BeginPrint += PrintDoc_BeginPrint;
            _printDoc.PrintPage += PrintDoc_PrintPage;

            Load += (_, __) => LoadData();
        }

        private void LoadData()
        {
            string filter = "Sukses";
            if (cbStatus != null && cbStatus.SelectedItem != null)
                filter = cbStatus.SelectedItem.ToString();
            DataTable dt = _repo.GetTransactionsByFilter(filter);
            dgvTransactions.DataSource = dt;
            if (dgvTransactions.Columns["ts_id"] != null) dgvTransactions.Columns["ts_id"].Visible = false;

            if (dgvTransactions.Columns["ts_numbering"] != null)
            {
                dgvTransactions.Columns["ts_numbering"].HeaderText = "No Transaksi";
                dgvTransactions.Columns["ts_numbering"].Width = 180;
            }

            if (dgvTransactions.Columns["ts_grand_total"] != null)
            {
                dgvTransactions.Columns["ts_grand_total"].HeaderText = "Grand Total";
                dgvTransactions.Columns["ts_grand_total"].DefaultCellStyle.Format = "N0";
                dgvTransactions.Columns["ts_grand_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvTransactions.Columns["ts_grand_total"].Width = 140;
            }

            if (dgvTransactions.Columns["ts_method"] != null)
            {
                dgvTransactions.Columns["ts_method"].HeaderText = "Metode";
                dgvTransactions.Columns["ts_method"].Width = 120;
            }

            if (dgvTransactions.Columns["ts_status"] != null)
            {
                dgvTransactions.Columns["ts_status"].HeaderText = "Status";
                dgvTransactions.Columns["ts_status"].Width = 90;
            }

            if (dgvTransactions.Columns["created_at"] != null)
            {
                dgvTransactions.Columns["created_at"].HeaderText = "Tanggal";
                dgvTransactions.Columns["created_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvTransactions.Columns["created_at"].Width = 160;
            }

            if (dgvTransactions.Columns["user_id"] != null)
            {
                dgvTransactions.Columns["user_id"].HeaderText = "User";
                dgvTransactions.Columns["user_id"].Width = 90;
            }

            if (dgvTransactions.Columns.Contains("deleted_at"))
            {
                dgvTransactions.Columns["deleted_at"].HeaderText = "Dibatalkan";
                dgvTransactions.Columns["deleted_at"].Width = 160;
            }

            if (dgvTransactions.Rows.Count > 0)
            {
                dgvTransactions.Rows[0].Selected = true;
                LoadDetailsForSelected();
            }
            else
            {
                dgvDetails.DataSource = new DataTable();
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            if (dgvTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih transaksi dulu.");
                return;
            }
            int tsId = Convert.ToInt32(dgvTransactions.SelectedRows[0].Cells["ts_id"].Value);
            var confirm = MessageBox.Show("Batalkan transaksi ini? Stok akan dikembalikan.", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = _service.CancelTransaction(tsId);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message);
                return;
            }
            MessageBox.Show("Transaksi dibatalkan dan stok dikembalikan.");
            LoadData();
        }

        private void dgvTransactions_SelectionChanged(object sender, EventArgs e)
        {
            LoadDetailsForSelected();
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            if (dgvTransactions.DataSource is DataTable dt)
            {
                ExportDataTableToCsv(dt);
            }
            else
            {
                MessageBox.Show("Tidak ada data untuk diexport.");
            }
        }

        private void BtnExportDetail_Click(object? sender, EventArgs e)
        {
            if (!(dgvTransactions.DataSource is DataTable dt) || dt.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk diexport.");
                return;
            }
            using var sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            sfd.FileName = "transaksi_dengan_detail.csv";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;
            using var w = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
            // header friendly
            w.WriteLine("No Transaksi,Tanggal,User,Metode,Status,Nama Barang,Barcode,Jumlah,Satuan,Harga Satuan,Subtotal,Diskon,Pajak");
            foreach (DataRow tr in dt.Rows)
            {
                int tsId = Convert.ToInt32(tr["ts_id"]);
                string tsNum = tr["ts_numbering"]?.ToString() ?? "";
                string createdAt = tr["created_at"]?.ToString() ?? "";
                string userId = tr["user_id"]?.ToString() ?? "";
                string method = tr.Table.Columns.Contains("ts_method") ? (tr["ts_method"]?.ToString() ?? "") : "";
                string status = tr.Table.Columns.Contains("ts_status") ? (tr["ts_status"]?.ToString() ?? "") : "1";
                var details = _repo.GetTransactionDetailsById(tsId);
                if (details.Rows.Count == 0)
                {
                    // tetap tulis header transaksi, detail kosong
                    w.WriteLine($"{Csv(tsNum)},{Csv(createdAt)},{Csv(userId)},{Csv(method)},{Csv(status)},,,,,,,");
                    continue;
                }
                foreach (DataRow d in details.Rows)
                {
                    string name = d["name"]?.ToString() ?? "";
                    string barcode = d["barcode"]?.ToString() ?? "";
                    string qty = ToRaw(d, "tsd_quantity");
                    string unit = d["tsd_unit"]?.ToString() ?? "";
                    string price = ToRaw(d, "tsd_sell_price");
                    string total = details.Columns.Contains("tsd_total") ? ToRaw(d, "tsd_total") : "";
                    string disc = details.Columns.Contains("tsd_discount_total") ? ToRaw(d, "tsd_discount_total") : "0";
                    string tax = details.Columns.Contains("tsd_tax") ? ToRaw(d, "tsd_tax") : "0";
                    w.WriteLine($"{Csv(tsNum)},{Csv(createdAt)},{Csv(userId)},{Csv(method)},{Csv(status)},{Csv(name)},{Csv(barcode)},{qty},{Csv(unit)},{price},{total},{disc},{tax}");
                }
            }
            w.Flush();
            MessageBox.Show("Export dengan detail selesai.");
        }

        private static string Csv(string s)
        {
            s ??= "";
            if (s.Contains(",") || s.Contains("\""))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
        private static string ToRaw(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return "0";
            // tulis raw tanpa format ribuan agar Excel mengenali numerik
            return Convert.ToDecimal(r[col]).ToString(CultureInfo.InvariantCulture);
        }

        private void ExportDataTableToCsv(DataTable dt)
        {
            using var sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            sfd.FileName = "export.csv";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;
            using var w = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                w.Write(dt.Columns[i].ColumnName);
                if (i < dt.Columns.Count - 1) w.Write(",");
            }
            w.WriteLine();
            foreach (DataRow r in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var val = r[i]?.ToString() ?? "";
                    if (val.Contains(",") || val.Contains("\""))
                        val = "\"" + val.Replace("\"", "\"\"") + "\"";
                    w.Write(val);
                    if (i < dt.Columns.Count - 1) w.Write(",");
                }
                w.WriteLine();
            }
            w.Flush();
            MessageBox.Show("Export selesai.");
        }

        private void BtnReturn_Click(object? sender, EventArgs e)
        {
            if (dgvTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih transaksi dulu.");
                return;
            }
            if (!dgvTransactions.Columns.Contains("ts_numbering"))
            {
                MessageBox.Show("Nomor transaksi tidak tersedia.");
                return;
            }
            string tsNumber = dgvTransactions.SelectedRows[0].Cells["ts_numbering"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(tsNumber))
            {
                MessageBox.Show("Nomor transaksi tidak valid.");
                return;
            }
            using var frm = new ReturnForm();
            frm.PreloadTransaction(tsNumber, autoLoad: true);
            frm.ShowDialog(this);
        }

        private void LoadDetailsForSelected()
        {
            if (dgvTransactions.SelectedRows.Count == 0)
            {
                dgvDetails.DataSource = new DataTable();
                return;
            }

            int tsId = Convert.ToInt32(dgvTransactions.SelectedRows[0].Cells["ts_id"].Value);
            var dt = _repo.GetTransactionDetailsById(tsId);
            var friendly = new DataTable();
            friendly.Columns.Add("Nama Barang");
            friendly.Columns.Add("Barcode");
            friendly.Columns.Add("Jumlah");
            friendly.Columns.Add("Satuan");
            friendly.Columns.Add("Harga Satuan");
            friendly.Columns.Add("Subtotal");
            friendly.Columns.Add("Diskon");
            friendly.Columns.Add("Pajak");
            friendly.Columns.Add("Catatan");

            foreach (DataRow r in dt.Rows)
            {
                var name = r["name"]?.ToString() ?? "";
                var barcode = r["barcode"]?.ToString() ?? "";
                var qty = Convert.ToDecimal(r["tsd_quantity"]);
                var unit = r["tsd_unit"]?.ToString() ?? "";
                var price = Convert.ToDecimal(r["tsd_sell_price"]);
                var total = Convert.ToDecimal(r["tsd_total"]);
                var disc = Convert.ToDecimal(r["tsd_discount_total"]);
                var tax = Convert.ToDecimal(r["tsd_tax"]);
                var note = r["tsd_note"]?.ToString() ?? "";

                friendly.Rows.Add(name, barcode, qty.ToString("N0"), unit, price.ToString("N0"), total.ToString("N0"), disc.ToString("N0"), tax.ToString("N0"), note);
            }

            dgvDetails.DataSource = friendly;

            if (dgvDetails.Columns.Contains("Nama Barang"))
                dgvDetails.Columns["Nama Barang"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dgvDetails.Columns.Contains("Subtotal"))
                dgvDetails.Columns["Subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            if (dgvDetails.Columns.Contains("Harga Satuan"))
                dgvDetails.Columns["Harga Satuan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            if (dgvDetails.Columns.Contains("Jumlah"))
                dgvDetails.Columns["Jumlah"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private int GetSelectedTransactionId()
        {
            if (dgvTransactions.SelectedRows.Count == 0) return 0;
            return Convert.ToInt32(dgvTransactions.SelectedRows[0].Cells["ts_id"].Value);
        }

        private void BtnPrintPreview_Click(object sender, EventArgs e)
        {
            var tsId = GetSelectedTransactionId();
            if (tsId <= 0)
            {
                MessageBox.Show("Pilih transaksi dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var data = LoadSaleInvoicePrintData(tsId);
            if (data == null)
            {
                MessageBox.Show("Data faktur tidak ditemukan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _printData = data;
            using var dlg = new PrintPreviewDialog();
            dlg.Document = _printDoc;
            dlg.WindowState = FormWindowState.Maximized;
            dlg.ShowDialog(this);
        }

        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            var tsId = GetSelectedTransactionId();
            if (tsId <= 0)
            {
                MessageBox.Show("Pilih transaksi dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var data = LoadSaleInvoicePrintData(tsId);
            if (data == null)
            {
                MessageBox.Show("Data faktur tidak ditemukan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog();
            sfd.Filter = "PDF Files (*.pdf)|*.pdf";
            sfd.FileName = "faktur_penjualan_" + (data.SaleNumber ?? "invoice") + ".pdf";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _printData = data;
                PrintToPdf(sfd.FileName);
                MessageBox.Show("PDF berhasil dibuat.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuat PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintToPdf(string filePath)
        {
            var printerName = FindPdfPrinterName();
            if (string.IsNullOrWhiteSpace(printerName))
                throw new Exception("Printer PDF tidak ditemukan. Install/aktifkan 'Microsoft Print to PDF'.");

            _printDoc.PrinterSettings = new PrinterSettings
            {
                PrinterName = printerName,
                PrintToFile = true,
                PrintFileName = filePath
            };
            _printDoc.Print();
        }

        private static string FindPdfPrinterName()
        {
            foreach (string p in PrinterSettings.InstalledPrinters)
            {
                if (p != null && p.IndexOf("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase) >= 0)
                    return p;
            }
            return "";
        }

        private void PrintDoc_BeginPrint(object sender, PrintEventArgs e)
        {
            _printRowCursor = 0;
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_printData == null)
            {
                e.HasMorePages = false;
                return;
            }

            var g = e.Graphics;
            var bounds = e.MarginBounds;

            using var fontHeader = new Font("Segoe UI", 14, FontStyle.Bold);
            using var fontSub = new Font("Segoe UI", 10, FontStyle.Regular);
            using var fontBold = new Font("Segoe UI", 10, FontStyle.Bold);
            using var fontMono = new Font("Consolas", 9, FontStyle.Regular);

            int y = bounds.Top;
            int line = 20;

            if (!string.IsNullOrWhiteSpace(_printData.StoreName))
            {
                g.DrawString(_printData.StoreName, fontHeader, Brushes.Black, bounds.Left, y);
                y += line + 10;
            }

            if (!string.IsNullOrWhiteSpace(_printData.StoreAddress))
            {
                g.DrawString(_printData.StoreAddress, fontSub, Brushes.Black, bounds.Left, y);
                y += line;
            }
            if (!string.IsNullOrWhiteSpace(_printData.StorePhone))
            {
                g.DrawString(_printData.StorePhone, fontSub, Brushes.Black, bounds.Left, y);
                y += line;
            }

            y += 8;
            g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
            y += 10;

            g.DrawString("No: " + (_printData.SaleNumber ?? ""), fontBold, Brushes.Black, bounds.Left, y);
            y += line;
            g.DrawString("Tanggal: " + _printData.SaleDate.ToString("dd/MM/yyyy HH:mm"), fontSub, Brushes.Black, bounds.Left, y);
            y += line;
            g.DrawString("Kasir: " + (_printData.Cashier ?? ""), fontSub, Brushes.Black, bounds.Left, y);
            y += line;
            g.DrawString("Metode: " + (_printData.PaymentMethod ?? ""), fontSub, Brushes.Black, bounds.Left, y);
            y += line + 6;

            g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
            y += 10;

            int colName = bounds.Left;
            int colQty = bounds.Right - 220;
            int colPrice = bounds.Right - 140;
            int colTotal = bounds.Right - 10;

            g.DrawString("Item", fontBold, Brushes.Black, colName, y);
            g.DrawString("Qty", fontBold, Brushes.Black, colQty, y);
            g.DrawString("Harga", fontBold, Brushes.Black, colPrice, y);
            g.DrawString("Total", fontBold, Brushes.Black, colTotal - 80, y);
            y += line;

            g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
            y += 8;

            while (_printRowCursor < _printData.Items.Rows.Count)
            {
                if (y > bounds.Bottom - 220)
                {
                    e.HasMorePages = true;
                    return;
                }

                var r = _printData.Items.Rows[_printRowCursor];
                string name = r["name"]?.ToString() ?? "";
                string qty = Convert.ToDecimal(r["qty"]).ToString("N0");
                string price = Convert.ToDecimal(r["price"]).ToString("N0");
                string total = Convert.ToDecimal(r["total"]).ToString("N0");

                g.DrawString(name, fontSub, Brushes.Black, colName, y);
                g.DrawString(qty, fontMono, Brushes.Black, colQty, y);
                g.DrawString(price, fontMono, Brushes.Black, colPrice, y);
                var sizeTotal = g.MeasureString(total, fontMono);
                g.DrawString(total, fontMono, Brushes.Black, colTotal - sizeTotal.Width, y);
                y += line;

                _printRowCursor++;
            }

            y += 6;
            g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
            y += 10;

            DrawMoneyLine(g, "Subtotal", _printData.TotalBeforeTax, fontSub, fontMono, bounds, ref y);
            if (_printData.TaxAmount > 0)
                DrawMoneyLine(g, $"PPN ({_printData.TaxMode})", _printData.TaxAmount, fontSub, fontMono, bounds, ref y);
            if (_printData.GlobalDiscount > 0)
                DrawMoneyLine(g, "Diskon", -_printData.GlobalDiscount, fontSub, fontMono, bounds, ref y);
            DrawMoneyLine(g, "Grand Total", _printData.GrandTotal, fontBold, fontMono, bounds, ref y);

            y += 6;
            DrawMoneyLine(g, "Bayar", _printData.PaidAmount, fontSub, fontMono, bounds, ref y);
            DrawMoneyLine(g, "Kembalian", _printData.ChangeAmount, fontSub, fontMono, bounds, ref y);

            y += 10;
            if (!string.IsNullOrWhiteSpace(_printData.Footer))
            {
                g.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
                y += 10;
                g.DrawString(_printData.Footer, fontSub, Brushes.Black, bounds.Left, y);
            }

            e.HasMorePages = false;
        }

        private static void DrawMoneyLine(Graphics g, string label, decimal value, Font fontLabel, Font fontValue, Rectangle bounds, ref int y)
        {
            int line = 20;
            g.DrawString(label, fontLabel, Brushes.Black, bounds.Left, y);
            string v = value.ToString("N0");
            var sz = g.MeasureString(v, fontValue);
            g.DrawString(v, fontValue, Brushes.Black, bounds.Right - sz.Width, y);
            y += line;
        }

        private SaleInvoicePrintData LoadSaleInvoicePrintData(int tsId)
        {
            using var con = new Npgsql.NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            var dtHeader = new DataTable();
            using (var cmd = new Npgsql.NpgsqlCommand(@"
SELECT
    t.ts_id,
    COALESCE(t.ts_numbering,'') AS ts_numbering,
    t.created_at,
    COALESCE(t.ts_method,'') AS ts_method,
    COALESCE(t.ts_grand_total,0) AS ts_grand_total,
    COALESCE(t.ts_payment_amount,0) AS ts_payment_amount,
    COALESCE(t.ts_change,0) AS ts_change,
    COALESCE(t.ts_total_before_tax,0) AS ts_total_before_tax,
    COALESCE(t.ts_tax_mode,'NON') AS ts_tax_mode,
    COALESCE(t.ts_tax_amount,0) AS ts_tax_amount,
    COALESCE(t.ts_global_discount_amount,0) AS ts_global_discount_amount,
    COALESCE(u.username,'') AS cashier
FROM transactions t
LEFT JOIN users u ON u.id = t.user_id
WHERE t.ts_id = @id
LIMIT 1
", con))
            {
                cmd.Parameters.AddWithValue("@id", tsId);
                using var da = new Npgsql.NpgsqlDataAdapter(cmd);
                da.Fill(dtHeader);
            }

            if (dtHeader.Rows.Count == 0) return null;
            var h = dtHeader.Rows[0];

            var details = _repo.GetTransactionDetailsById(tsId);
            var items = new DataTable();
            items.Columns.Add("name", typeof(string));
            items.Columns.Add("qty", typeof(decimal));
            items.Columns.Add("price", typeof(decimal));
            items.Columns.Add("total", typeof(decimal));

            foreach (DataRow r in details.Rows)
            {
                items.Rows.Add(
                    r["name"]?.ToString() ?? "",
                    Convert.ToDecimal(r["tsd_quantity"]),
                    Convert.ToDecimal(r["tsd_sell_price"]),
                    Convert.ToDecimal(r["tsd_total"])
                );
            }

            string storeName = "", storeAddress = "", storePhone = "", footer = "";
            try
            {
                using (var cmd = new Npgsql.NpgsqlCommand("SELECT * FROM struk_setting ORDER BY updated_at DESC LIMIT 1", con))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        storeName = reader["judul"]?.ToString()?.Trim() ?? "";
                        storeAddress = reader["alamat"]?.ToString()?.Trim() ?? "";
                        storePhone = reader["telepon"]?.ToString()?.Trim() ?? "";
                        footer = reader["footer"]?.ToString()?.Trim() ?? "";
                    }
                }
            }
            catch
            {
            }

            return new SaleInvoicePrintData
            {
                SaleId = tsId,
                SaleNumber = h["ts_numbering"]?.ToString() ?? "",
                SaleDate = Convert.ToDateTime(h["created_at"]),
                PaymentMethod = h["ts_method"]?.ToString() ?? "",
                Cashier = h["cashier"]?.ToString() ?? "",
                StoreName = storeName,
                StoreAddress = storeAddress,
                StorePhone = storePhone,
                Footer = footer,
                Items = items,
                TotalBeforeTax = Convert.ToDecimal(h["ts_total_before_tax"]),
                TaxMode = h["ts_tax_mode"]?.ToString() ?? "NON",
                TaxAmount = Convert.ToDecimal(h["ts_tax_amount"]),
                GlobalDiscount = Convert.ToDecimal(h["ts_global_discount_amount"]),
                GrandTotal = Convert.ToDecimal(h["ts_grand_total"]),
                PaidAmount = Convert.ToDecimal(h["ts_payment_amount"]),
                ChangeAmount = Convert.ToDecimal(h["ts_change"])
            };
        }

        private sealed class SaleInvoicePrintData
        {
            public int SaleId { get; set; }
            public string SaleNumber { get; set; }
            public DateTime SaleDate { get; set; }
            public string PaymentMethod { get; set; }
            public string Cashier { get; set; }
            public string StoreName { get; set; }
            public string StoreAddress { get; set; }
            public string StorePhone { get; set; }
            public string Footer { get; set; }
            public DataTable Items { get; set; }
            public decimal TotalBeforeTax { get; set; }
            public string TaxMode { get; set; }
            public decimal TaxAmount { get; set; }
            public decimal GlobalDiscount { get; set; }
            public decimal GrandTotal { get; set; }
            public decimal PaidAmount { get; set; }
            public decimal ChangeAmount { get; set; }
        }
    }
}
