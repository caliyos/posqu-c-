using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_qu.Helpers;
using POS_qu.Controllers;
//using DocumentFormat.OpenXml.Wordprocessing;
using ClosedXML.Excel;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace POS_qu
{
    public partial class StockReports : Form
    {
        private DataGridViewManager dgvManager;
        private ItemController itemController;
        DataTable dt;
        //private TransactionController _transactionController = new TransactionController();
        public StockReports()
        {

            InitializeComponent();

            itemController = new ItemController();
            dt = itemController.GetItems();


            dgvManager = new DataGridViewManager(dgvStockReport, dt, 10);
            dgvManager.PagingInfoLabel = lblPagingInfo;

            dgvManager.OnAfterLoadPage += calculateStocks;

            cmbPageSize.Items.AddRange(new object[] { "10", "50", "100", "200", "500", "1000" });
            cmbPageSize.SelectedIndex = 0; // Default to 10

            btnNext.Click += btnNext_Click;
            btnPrev.Click += btnPrevious_Click;
            btnFirst.Click += btnFirstPage_Click;
            btnLast.Click += btnLastPage_Click;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;



            dgvStockReport.ReadOnly = true;

            dgvManager.LoadPage();

        }

        private void btnNext_Click(object sender, EventArgs e) => dgvManager.NextPage();
        private void btnPrevious_Click(object sender, EventArgs e) => dgvManager.PreviousPage();
        private void btnFirstPage_Click(object sender, EventArgs e) => dgvManager.FirstPage();
        private void btnLastPage_Click(object sender, EventArgs e) => dgvManager.LastPage();

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


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvManager.Filter(txtSearch.Text, "name"); // or any searchable column
        }

        private void button1_Click(object sender, EventArgs e)
        {
            itemController = new ItemController();
            dt = itemController.GetItems();
            dgvManager.reloadData(dt);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            itemController = new ItemController();
            dt = itemController.GetAvailableItems();
            dgvManager.reloadData(dt);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            itemController = new ItemController();
            dt = itemController.GetNonAvailableItems();
            dgvManager.reloadData(dt);

        }

        private void calculateStocks()
        {
            // Pastikan dt tidak null dan punya data
            if (dt == null || dt.Rows.Count == 0) return;

            var grouped = dt.AsEnumerable()
                .Where(row => row["stock"] != DBNull.Value && row["unit"] != DBNull.Value)
                .GroupBy(row => row["unit"].ToString())
                .Select(g => new
                {
                    Unit = g.Key,
                    Total = g.Sum(row => Convert.ToInt32(row["stock"]))
                });

            // Gabungkan jadi string seperti "3 dus, 10 pieces"
            string result = string.Join(", ", grouped.Select(g => $"{g.Total} {g.Unit}"));




            object buy = dt.Compute("SUM(buy_price)", "");
            int totalBuy = Convert.ToInt32(buy);

            object sell = dt.Compute("SUM(sell_price)", "");
            int totalSell = Convert.ToInt32(sell);

            label7.Text = result;
            label6.Text = totalBuy.ToString();
            label5.Text = totalSell.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
            string tanggalHariIni = DateTime.Now.ToString("dd-MM-yyyy");
            sfd.FileName = $"Laporan_Stock_{tanggalHariIni}.xlsx";

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

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv";
            string tanggalHariIni = DateTime.Now.ToString("dd-MM-yyyy");
            sfd.FileName = $"Laporan_Stock_{tanggalHariIni}.csv";


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

        private void button6_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF files (*.pdf)|*.pdf";
                string tanggalHariIni = DateTime.Now.ToString("dd-MM-yyyy");
                sfd.FileName = $"Laporan_Stock_{tanggalHariIni}.pdf";

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
                                    .Text($"Laporan Penjualan - {tanggalHariIni}")
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
    }
}
