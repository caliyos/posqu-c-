using POS_qu.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot.Axes;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using DocumentFormat.OpenXml.Spreadsheet;

namespace POS_qu
{
    public partial class SalesReports : Form
    {
        private TransactionController _transactionController = new TransactionController();
        public SalesReports()
        {
            InitializeComponent();
            // Mengisi comboBox2 dengan nama-nama bulan
            string[] bulan = new string[]
            {
    "Januari", "Februari", "Maret", "April", "Mei", "Juni",
    "Juli", "Agustus", "September", "Oktober", "November", "Desember"
            };


            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(bulan);

            // Mengisi comboBox3 dengan 3 tahun dimulai dari tahun sekarang
            comboBox3.Items.Clear();
            int tahunSekarang = DateTime.Now.Year;
            for (int i = 0; i < 3; i++)
            {
                comboBox3.Items.Add(tahunSekarang - i);
            }

            radioPeriode.Checked = true; // default filter
            RadioFilterChanged(null, null); // trigger update tampilan

            // Dummy awal, bisa juga langsung dari GetFilteredSalesReport jika ingin menampilkan real-time awal
            var initialData = new Dictionary<string, double>
            {
                { "Kategori A", 0 },
                { "Kategori B", 0 },
                { "Kategori C", 0 }
            };

            RenderChart(initialData);

        }

        private void RadioFilterChanged(object sender, EventArgs e)
        {
            comboBoxMasa.Visible = radioPeriode.Checked;

            comboBox2.Visible = comboBox3.Visible = radioBulanTahun.Checked;

            dateTimePicker1.Visible = dateTimePicker2.Visible = radioRentangTanggal.Checked;

            if (radioPeriode.Checked)
                labelFilterAktif.Text = "Filter Aktif: Periode";
            else if (radioBulanTahun.Checked)
                labelFilterAktif.Text = "Filter Aktif: Bulan & Tahun";
            else if (radioRentangTanggal.Checked)
                labelFilterAktif.Text = "Filter Aktif: Rentang Tanggal";
        }

   
        
        private void btnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                var (dt, periodeLabel) = LoadReport(1);
                UpdateSummaryGroupBox(dt);
                UpdateCharts(dt);

                pdfExport(periodeLabel, dt);
                label2Periode.Text = $"Periode: {periodeLabel} :_Laporan_penjualan";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2Pdf_Click(object sender, EventArgs e)
        {
            try
            {
                var (dt, periodeLabel) = LoadReport(2);
                UpdateTopProduk(dt);
                UpdateCharts(dt);

                pdfExport(periodeLabel, dt);
                label2Periode.Text = $"Periode: {periodeLabel} :_Laporan_top_produk";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void pdfExport (string periodeLabel, DataTable dt)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF files (*.pdf)|*.pdf";
                sfd.FileName = $"Laporan_{periodeLabel}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var filePath = sfd.FileName;

                        QuestPDF.Settings.License = LicenseType.Community;

                        Document.Create(container =>
                        {
                            container.Page(page =>
                            {
                                page.Size(PageSizes.A4);
                                page.Margin(20);
                                //page.PageColor(Colors.White);
                                page.DefaultTextStyle(x => x.FontSize(10));

                                page.Header()
                                    .Text($"Laporan Penjualan - {periodeLabel}")
                                    //.SemiBold().FontSize(14).FontColor(Colors.Blue.Medium)
                                    .AlignCenter();

                                page.Content().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        for (int i = 0; i < dt.Columns.Count; i++)
                                            columns.RelativeColumn();
                                    });

                                    // Header kolom
                                    table.Header(header =>
                                    {
                                        for (int i = 0; i < dt.Columns.Count; i++)
                                        {
                                            string colName = dt.Columns[i].ColumnName;
                                            //header.Cell().Element(CellStyle).Text(colName).SemiBold();
                                        }
                                    });

                                    foreach (DataRow row in dt.Rows)
                                    {
                                        for (int i = 0; i < dt.Columns.Count; i++)
                                        {
                                            string value = row[i]?.ToString() ?? "";
                                            table.Cell().Text(value);
                                        }
                                    }
                                });

                                page.Footer()
                                    .AlignCenter()
                                    .Text(x =>
                                    {
                                        x.Span("Halaman ");
                                        x.CurrentPageNumber();
                                    });
                            });
                        })
                        .GeneratePdf(filePath);

                        MessageBox.Show("PDF berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Process.Start("explorer.exe", filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menyimpan PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void btn2Excel_Click(object sender, EventArgs e)
        {
            try
            {
                var (dt, periodeLabel) = LoadReport(2);
                UpdateTopProduk(dt);
                UpdateCharts(dt);

                excelExport(periodeLabel, dt);
                label2Periode.Text = $"Periode: {periodeLabel}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var (dt, periodeLabel) = LoadReport(1);
                UpdateSummaryGroupBox(dt);
                UpdateCharts(dt);

                excelExport(periodeLabel, dt);
                labelPeriode.Text = $"Periode: {periodeLabel}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void excelExport(string periodeLabel, DataTable dt)
        {
       
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
            sfd.FileName = $"Laporan_{periodeLabel}.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Laporan");

                        // Header
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                        }

                        // Data rows
                        for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                        {
                            for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                            {
                                var cellValue = dt.Rows[rowIndex][colIndex];
                                worksheet.Cell(rowIndex + 2, colIndex + 1).Value = cellValue == DBNull.Value ? "" : cellValue.ToString();


                            }
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Data berhasil diekspor ke Excel.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengekspor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void csvExport(string periodeLabel, DataTable dt)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv";
            sfd.FileName = $"Laporan_{periodeLabel}.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sw.Write(dt.Columns[i].ColumnName);
                            if (i < dt.Columns.Count - 1) sw.Write(",");
                        }
                        sw.WriteLine();

                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                string cell = row[i].ToString().Replace(",", "");
                                sw.Write(cell);
                                if (i < dt.Columns.Count - 1) sw.Write(",");
                            }
                            sw.WriteLine();
                        }
                    }

                    MessageBox.Show("Data berhasil diekspor ke CSV.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengekspor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

  
        private void btnCsv_Click(object sender, EventArgs e)
        {
            try
            {
                var (dt, periodeLabel) = LoadReport(1);
                UpdateSummaryGroupBox(dt);
                UpdateCharts(dt);

                csvExport(periodeLabel, dt);
                labelPeriode.Text = $"Periode: {periodeLabel}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCsv2_Click_1(object sender, EventArgs e)
        {
            try
            {
                var (dt, periodeLabel) = LoadReport(2);
                UpdateTopProduk(dt);
                UpdateCharts(dt);

                csvExport(periodeLabel, dt);
                label2Periode.Text = $"Periode: {periodeLabel}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    


        private bool TryGetDateRange(out DateTime startDate, out DateTime endDate, out string periodeLabel)
        {
            startDate = endDate = DateTime.Today;
            periodeLabel = "";

            if (radioPeriode.Checked)
            {
                string selectedPeriode = comboBoxMasa.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(selectedPeriode))
                {
                    MessageBox.Show("Pilih Periode dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                switch (selectedPeriode)
                {
                    case "Today":
                        startDate = DateTime.Today;
                        endDate = DateTime.Today;
                        break;
                    case "Yesterday":
                        startDate = DateTime.Today.AddDays(-1);
                        endDate = startDate;
                        break;
                    case "This Week":
                        int diff = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7;
                        startDate = DateTime.Today.AddDays(-1 * diff);
                        endDate = DateTime.Today;
                        break;
                    case "Last Week":
                        int diffLast = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7;
                        endDate = DateTime.Today.AddDays(-1 * diffLast - 1);
                        startDate = endDate.AddDays(-6);
                        break;
                    case "This Month":
                        startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        endDate = DateTime.Today;
                        break;
                    case "Last Month":
                        DateTime firstDayThisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        DateTime lastMonth = firstDayThisMonth.AddMonths(-1);
                        startDate = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                        endDate = new DateTime(lastMonth.Year, lastMonth.Month, DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month));
                        break;
                    case "This year":
                        startDate = new DateTime(DateTime.Today.Year, 1, 1);
                        endDate = DateTime.Today;
                        break;
                    case "Last Year":
                        int lastYear = DateTime.Today.Year - 1;
                        startDate = new DateTime(lastYear, 1, 1);
                        endDate = new DateTime(lastYear, 12, 31);
                        break;
                    default:
                        MessageBox.Show("Periode belum dipilih atau tidak valid.");
                        return false;
                }

                periodeLabel = selectedPeriode.Replace(" ", "_");
                return true;
            }
            else if (radioBulanTahun.Checked)
            {
                if (comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
                {
                    MessageBox.Show("Pilih Bulan dan Tahun dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                string namaBulan = comboBox2.SelectedItem.ToString();
                int bulan = DateTime.ParseExact(namaBulan, "MMMM", CultureInfo.InvariantCulture).Month;
                int tahun = int.Parse(comboBox3.SelectedItem.ToString());

                startDate = new DateTime(tahun, bulan, 1);
                endDate = startDate.AddMonths(1).AddDays(-1);
                periodeLabel = $"{namaBulan}_{tahun}";
                return true;
            }
            else if (radioRentangTanggal.Checked)
            {
                startDate = dateTimePicker1.Value.Date;
                endDate = dateTimePicker2.Value.Date;

                if (endDate < startDate)
                {
                    MessageBox.Show("Tanggal akhir harus lebih besar atau sama dengan tanggal awal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                periodeLabel = $"{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                return true;
            }

            MessageBox.Show("Pilih jenis filter dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private (DataTable dt, string periodeLabel) LoadReport(int reportMode)
        {
            DateTime startDate, endDate;
            string periodeLabel;
            TransactionController controller = new TransactionController();

            if (!TryGetDateRange(out startDate, out endDate, out periodeLabel))
            {
                throw new InvalidOperationException("Rentang tanggal tidak valid atau belum dipilih.");
            }

            DataTable dt = null;

            switch (reportMode)
            {
                case 1: // Sales Report
                    dt = controller.GetSalesReport(startDate, endDate);
                    break;
                case 2: // Top Products
                    dt = controller.GetTopProducts(startDate, endDate);
                    break;
                // tambah case lain kalau ada report lain
                default:
                    MessageBox.Show("Report mode tidak dikenali.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new ArgumentException("Report mode tidak dikenali: " + reportMode);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                // Misal update UI lain sesuai reportMode
                return (dt, periodeLabel);

            }
            else
            {
                dataGridView1.DataSource = null;
                throw new InvalidOperationException("Tidak ada data transaksi untuk filter tersebut.");
            }
        }






        private void buttonView_Click(object sender, EventArgs e)
        {
            try
            {
                var (dt, periodeLabel) = LoadReport(1);
                UpdateSummaryGroupBox(dt);
                UpdateCharts(dt);

                // Misal kamu ingin update label di UI juga
                labelPeriode.Text = $"Periode: {periodeLabel}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        private void buttonView2_Click(object sender, EventArgs e)
        {
            try
            {
                var (dt, periodeLabel) = LoadReport(2);
                UpdateTopProduk(dt);
                UpdateCharts(dt);

                // Misal kamu ingin update label di UI juga
                label2Periode.Text = $"Periode: {periodeLabel}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        // Fungsi untuk update Pie dan Bar Chart berdasarkan DataTable dt
        private void UpdateCharts(DataTable dt)
        {
            // Contoh: Buat pie chart berdasarkan kategori (misal kolom "Category" dan kolom "Amount")
            //var pieModel = new PlotModel { Title = "Penjualan Produk" };
            //var pieSeries = new PieSeries();

            // Hitung total per kategori (asumsi ada kolom "Category" dan "Amount")
            var categoryTotals = new Dictionary<string, double>();
            foreach (DataRow row in dt.Rows)
            {
                string category = row["ProductName"].ToString();
                double amount = 0;
                double.TryParse(row["Profit"].ToString(), out amount);

                if (categoryTotals.ContainsKey(category))
                    categoryTotals[category] += amount;
                else
                    categoryTotals[category] = amount;
            }

            //foreach (var kvp in categoryTotals)
            //{
            //    pieSeries.Slices.Add(new PieSlice(kvp.Key, kvp.Value));
            //}

            //pieModel.Series.Add(pieSeries);

            // Buat Bar Chart (misal kategori sama, nilai sama)
            var barModel = new PlotModel { Title = "Bar Chart Penjualan" };
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom };

            barModel.Axes.Add(categoryAxis);
            barModel.Axes.Add(valueAxis);

            var barSeries = new BarSeries();

            foreach (var kvp in categoryTotals)
            {
                categoryAxis.Labels.Add(kvp.Key);
                barSeries.Items.Add(new BarItem { Value = kvp.Value });
            }
            barModel.Series.Add(barSeries);

            // Siapkan ukuran panel chart
            int halfHeight = panelChart.Height / 2;

            // Buat PlotView baru untuk Pie dan Bar Chart
            //var piePlotView = new PlotView
            //{
            //    Model = pieModel,
            //    Location = new System.Drawing.Point(0, 0),
            //    Size = new System.Drawing.Size(panelChart.Width, halfHeight),
            //    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            //};

            var barPlotView = new PlotView
            {
                Model = barModel,
                Location = new System.Drawing.Point(0, halfHeight),
                Size = new System.Drawing.Size(panelChart.Width, panelChart.Height - halfHeight),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Bersihkan panelChart dan tambahkan chart yang baru
            panelChart.Controls.Clear();
            //panelChart.Controls.Add(piePlotView);
            panelChart.Controls.Add(barPlotView);
        }

        private void RenderChart(Dictionary<string, double> data)
        {
            // PIE CHART
            //var pieModel = new PlotModel { Title = "Penjualan Produk" };
            //var pieSeries = new PieSeries();

            //foreach (var kvp in data)
            //{
            //    pieSeries.Slices.Add(new PieSlice(kvp.Key, kvp.Value));
            //}
            //pieModel.Series.Add(pieSeries);

            // BAR CHART
            var barModel = new PlotModel { Title = "Bar Chart Penjualan" };
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom };

            var barSeries = new BarSeries();
            foreach (var kvp in data)
            {
                categoryAxis.Labels.Add(kvp.Key);
                barSeries.Items.Add(new BarItem { Value = kvp.Value });
            }

            barModel.Axes.Add(categoryAxis);
            barModel.Axes.Add(valueAxis);
            barModel.Series.Add(barSeries);

            // Ukuran panel dibagi dua
            int halfHeight = panelChart.Height / 2;

            //var piePlotView = new PlotView
            //{
            //    Model = pieModel,
            //    Location = new System.Drawing.Point(0, 0),
            //    Size = new System.Drawing.Size(panelChart.Width, halfHeight),
            //    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            //};

            var barPlotView = new PlotView
            {
                Model = barModel,
                Location = new System.Drawing.Point(0, halfHeight),
                Size = new System.Drawing.Size(panelChart.Width, panelChart.Height - halfHeight),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            panelChart.Controls.Clear();
            //panelChart.Controls.Add(piePlotView);
            panelChart.Controls.Add(barPlotView);
        }


        private void UpdateSummaryGroupBox(DataTable dt)
        {
            long totalTransaksi = dt.AsEnumerable().Sum(row => row.Field<long>("Transaction"));
            decimal totalPendapatan = dt.AsEnumerable().Sum(row => row.Field<decimal>("Sales"));
            decimal totalItem = dt.AsEnumerable().Sum(row => row.Field<decimal>("Items"));
            decimal totalProfit = dt.AsEnumerable().Sum(row => row.Field<decimal>("Profit"));
            decimal totalDiscount = dt.AsEnumerable().Sum(row => row.Field<decimal>("Discount"));

            labelTransctionVal.Text = totalTransaksi.ToString("N0");
            lblSalesVal.Text = totalPendapatan.ToString("C0", new CultureInfo("id-ID"));
            labelItemsVal.Text = totalItem.ToString("N0");
            labelProfitVal.Text = totalProfit.ToString("N0");
            labelDiscountVal.Text = totalDiscount.ToString("N0");
        }

        private void UpdateTopProduk(DataTable dt)
        {
            // 1. Ambil nama produk terlaris berdasarkan total Sales
            string topProduct = dt.AsEnumerable()
                .GroupBy(row => row.Field<string>("ProductName"))
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalSales = g.Sum(r => r.Field<decimal>("Sales"))
                })
                .OrderByDescending(x => x.TotalSales)
                .Select(x => x.ProductName)
                .FirstOrDefault();

            // 2. Hitung total untuk produk tersebut
            var filteredRows = dt.AsEnumerable()
                .Where(row => row.Field<string>("ProductName") == topProduct);

            long totalTransaksi = filteredRows.Sum(row => row.Field<long>("Transaction"));
            decimal totalItem = filteredRows.Sum(row => row.Field<decimal>("Items"));
            decimal totalProfit = filteredRows.Sum(row => row.Field<decimal>("Profit"));
            decimal totalSales = filteredRows.Sum(row => row.Field<decimal>("Sales"));
            decimal totalDiscount = filteredRows.Sum(row => row.Field<decimal>("Discount"));


            labelTopP1.Text = totalTransaksi.ToString("N0");
            labelTopP2.Text = totalSales.ToString("C0", new CultureInfo("id-ID"));
            labelTopP3.Text = totalItem.ToString("N0");
            labelTopP4.Text = totalProfit.ToString("N0");
            labelTopP5.Text = totalDiscount.ToString("N0");
        }

        private void groupBoxSummary_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void SalesReports_Load(object sender, EventArgs e)
        {

        }

        private void labelPeriode_Click(object sender, EventArgs e)
        {

        }


    }
}
