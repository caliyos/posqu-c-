using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using POS_qu.Core.Interfaces;
using POS_qu.Repositories;
using POS_qu.Services;

namespace POS_qu
{
    public partial class BarcodePrintForm : Form
    {
        private readonly IProductService _productService;
        private readonly HashSet<int> _preselectedItemIds;
        private DataTable _itemsTable;

        private readonly PrintDocument _doc = new PrintDocument();
        private LabelTemplate _template;
        private List<LabelJob> _jobs = new List<LabelJob>();
        private int _printCursor;

        public BarcodePrintForm(IEnumerable<int>? preselectedItemIds = null)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            _productService = new ProductService(new ProductRepository());
            _preselectedItemIds = preselectedItemIds != null ? new HashSet<int>(preselectedItemIds) : new HashSet<int>();

            _doc.BeginPrint += Doc_BeginPrint;
            _doc.PrintPage += Doc_PrintPage;

            Load += BarcodePrintForm_Load;
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            cmbTemplate.SelectedIndexChanged += (s, e) => { ApplyTemplateFromUI(); RefreshPreview(); };
            chkShowName.CheckedChanged += (s, e) => RefreshPreview();
            chkShowPrice.CheckedChanged += (s, e) => RefreshPreview();

            dgvItems.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvItems.IsCurrentCellDirty)
                    dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgvItems.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (dgvItems.Columns[e.ColumnIndex].Name == "colSelect" || dgvItems.Columns[e.ColumnIndex].Name == "colQtyLabels")
                    RefreshPreview();
            };
            dgvItems.EditingControlShowing += DgvItems_EditingControlShowing;
        }

        private void BarcodePrintForm_Load(object? sender, EventArgs e)
        {
            BuildTemplateList();
            LoadItems();
            ApplyFilter();
            ApplyGridStyle();

            chkShowName.Checked = true;
            chkShowPrice.Checked = false;

            ApplyTemplateFromUI();

            previewControl.Document = _doc;
            previewControl.Zoom = 1.0;
            RefreshPreview();
        }

        private void BuildTemplateList()
        {
            cmbTemplate.Items.Clear();
            cmbTemplate.Items.Add(new TemplateItem("Retail/POS • A4 Auto 40x30 mm", LabelTemplate.A4Auto(40f, 30f, 6f, 8f, 2f)));
            cmbTemplate.Items.Add(new TemplateItem("Retail/POS • A4 Auto 50x30 mm", LabelTemplate.A4Auto(50f, 30f, 6f, 8f, 2f)));
            cmbTemplate.Items.Add(new TemplateItem("Retail/POS • A4 Auto 60x40 mm", LabelTemplate.A4Auto(60f, 40f, 6f, 8f, 2f)));

            cmbTemplate.Items.Add(new TemplateItem("Supermarket • A4 Auto 58x40 mm", LabelTemplate.A4Auto(58f, 40f, 6f, 8f, 2f)));
            cmbTemplate.Items.Add(new TemplateItem("Supermarket • A4 Auto 70x50 mm", LabelTemplate.A4Auto(70f, 50f, 6f, 8f, 2f)));

            cmbTemplate.Items.Add(new TemplateItem("Gudang/Logistik • Roll 100x150 mm (4x6\")", LabelTemplate.Roll(100f, 150f)));
            cmbTemplate.Items.Add(new TemplateItem("Gudang/Logistik • Roll 100x100 mm", LabelTemplate.Roll(100f, 100f)));
            cmbTemplate.Items.Add(new TemplateItem("Gudang/Logistik • Roll 80x50 mm", LabelTemplate.Roll(80f, 50f)));

            cmbTemplate.Items.Add(new TemplateItem("Industri • Roll 30x20 mm", LabelTemplate.Roll(30f, 20f)));
            cmbTemplate.Items.Add(new TemplateItem("Industri • Roll 25x15 mm", LabelTemplate.Roll(25f, 15f)));

            cmbTemplate.Items.Add(new TemplateItem("Thermal Roll • 58 mm x 30 mm", LabelTemplate.Roll(58f, 30f)));
            cmbTemplate.Items.Add(new TemplateItem("Thermal Roll • 58 mm x 40 mm", LabelTemplate.Roll(58f, 40f)));
            cmbTemplate.Items.Add(new TemplateItem("Thermal Roll • 80 mm x 40 mm", LabelTemplate.Roll(80f, 40f)));
            cmbTemplate.Items.Add(new TemplateItem("Thermal Roll • 80 mm x 50 mm", LabelTemplate.Roll(80f, 50f)));

            cmbTemplate.Items.Add(new TemplateItem("A4 3x8 (38x21mm)", LabelTemplate.A4Grid(3, 8, 38f, 21f, 6f, 8f, 6f)));
            cmbTemplate.Items.Add(new TemplateItem("A4 3x10 (35x20mm)", LabelTemplate.A4Grid(3, 10, 35f, 20f, 6f, 8f, 6f)));
            cmbTemplate.Items.Add(new TemplateItem("A4 2x7 (50x30mm)", LabelTemplate.A4Grid(2, 7, 50f, 30f, 6f, 8f, 6f)));
            cmbTemplate.SelectedIndex = 0;
        }

        private void ApplyTemplateFromUI()
        {
            if (cmbTemplate.SelectedItem is not TemplateItem ti)
                return;
            _template = ti.Template;
            _doc.DefaultPageSettings.Landscape = false;
            _doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            if (_template.Kind == TemplateKind.Roll)
            {
                _doc.DefaultPageSettings.PaperSize = CreateCustomPaperSizeMm("Roll", _template.PageWidthMm, _template.PageHeightMm);
            }
            else
            {
                _doc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            }
        }

        private void LoadItems()
        {
            _itemsTable = _productService.GetAllProducts();

            dgvItems.Rows.Clear();
            if (_itemsTable == null) return;

            foreach (DataRow r in _itemsTable.Rows)
            {
                int id = r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0;
                if (id <= 0) continue;

                bool isInv = true;
                if (_itemsTable.Columns.Contains("is_inventory_p") && r["is_inventory_p"] != DBNull.Value)
                    isInv = Convert.ToBoolean(r["is_inventory_p"]);
                if (!isInv) continue;

                string barcode = _itemsTable.Columns.Contains("barcode") ? (r["barcode"]?.ToString() ?? "") : "";
                string name = _itemsTable.Columns.Contains("name") ? (r["name"]?.ToString() ?? "") : "";
                string unit = _itemsTable.Columns.Contains("unit_name") ? (r["unit_name"]?.ToString() ?? "") : "";

                decimal price = 0m;
                if (_itemsTable.Columns.Contains("sell_price") && r["sell_price"] != DBNull.Value)
                    price = Convert.ToDecimal(r["sell_price"]);

                bool isSelected = _preselectedItemIds.Contains(id);
                int idx = dgvItems.Rows.Add(isSelected, barcode, name, unit, price.ToString("N0"), isSelected ? "1" : "0");
                dgvItems.Rows[idx].Tag = id;
            }
        }

        private void ApplyFilter()
        {
            string key = (txtSearch.Text ?? "").Trim().ToLowerInvariant();
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow) continue;
                string barcode = row.Cells["colBarcode"].Value?.ToString() ?? "";
                string name = row.Cells["colName"].Value?.ToString() ?? "";
                bool visible = string.IsNullOrWhiteSpace(key) ||
                               barcode.ToLowerInvariant().Contains(key) ||
                               name.ToLowerInvariant().Contains(key);
                row.Visible = visible;
            }
        }

        private void ApplyGridStyle()
        {
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvItems.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgvItems.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvItems.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            dgvItems.GridColor = Color.FromArgb(235, 235, 235);
        }

        private void DgvItems_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvItems.CurrentCell == null) return;
            if (dgvItems.Columns[dgvItems.CurrentCell.ColumnIndex].Name != "colQtyLabels") return;
            if (e.Control is not TextBox tb) return;

            tb.KeyPress -= QtyKeyPress;
            tb.KeyPress += QtyKeyPress;
        }

        private void QtyKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void RefreshPreview()
        {
            _jobs = BuildJobsFromGrid();
            _printCursor = 0;
            try
            {
                previewControl.InvalidatePreview();
            }
            catch
            {
            }
        }

        private List<LabelJob> BuildJobsFromGrid()
        {
            var list = new List<LabelJob>();
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow) continue;
                if (!row.Visible) continue;

                bool selected = Convert.ToBoolean(row.Cells["colSelect"].Value ?? false);
                if (!selected) continue;

                string barcode = row.Cells["colBarcode"].Value?.ToString() ?? "";
                string name = row.Cells["colName"].Value?.ToString() ?? "";
                string unit = row.Cells["colUnit"].Value?.ToString() ?? "";
                string priceStr = row.Cells["colPrice"].Value?.ToString() ?? "0";
                decimal price = 0m;
                decimal.TryParse(priceStr.Replace(".", "").Replace(",", ""), out price);

                int qty = 1;
                int.TryParse(row.Cells["colQtyLabels"].Value?.ToString() ?? "1", out qty);
                if (qty <= 0) qty = 1;

                list.Add(new LabelJob
                {
                    Barcode = barcode,
                    Name = name,
                    Unit = unit,
                    Price = price,
                    QtyLabels = qty
                });
            }
            return list;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            _jobs = BuildJobsFromGrid();
            if (_jobs.Count == 0)
            {
                MessageBox.Show("Pilih item dulu (centang) untuk print barcode.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _printCursor = 0;
            printPreviewDialog1.Document = _doc;
            printPreviewDialog1.WindowState = FormWindowState.Maximized;
            printPreviewDialog1.ShowDialog(this);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            _jobs = BuildJobsFromGrid();
            if (_jobs.Count == 0)
            {
                MessageBox.Show("Pilih item dulu (centang) untuk print barcode.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _printCursor = 0;
            printDialog1.Document = _doc;
            if (printDialog1.ShowDialog(this) != DialogResult.OK) return;
            _doc.Print();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            _jobs = BuildJobsFromGrid();
            if (_jobs.Count == 0)
            {
                MessageBox.Show("Pilih item dulu (centang) untuk export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ApplyTemplateFromUI();
            if (_template == null) return;

            using var sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "barcode_labels.xlsx"
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            bool showName = chkShowName.Checked;
            bool showPrice = chkShowPrice.Checked;
            var expanded = ExpandJobs(_jobs);

            var streams = new List<MemoryStream>();
            try
            {
                using var wb = new XLWorkbook();
                var ws = wb.AddWorksheet("Labels");
                ExportLabelsToWorksheet(ws, expanded, showName, showPrice, streams);

                var wsData = wb.AddWorksheet("Data");
                ExportDataSheet(wsData, expanded);

                wb.SaveAs(sfd.FileName);
            }
            finally
            {
                foreach (var ms in streams)
                {
                    try { ms.Dispose(); } catch { }
                }
            }

            MessageBox.Show("Export Excel berhasil.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPageSetup_Click(object sender, EventArgs e)
        {
            ApplyTemplateFromUI();
            pageSetupDialog1.Document = _doc;
            pageSetupDialog1.ShowDialog(this);
            RefreshPreview();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static PaperSize CreateCustomPaperSizeMm(string name, float widthMm, float heightMm)
        {
            int w = (int)Math.Round((widthMm / 25.4f) * 100f);
            int h = (int)Math.Round((heightMm / 25.4f) * 100f);
            if (w < 100) w = 100;
            if (h < 100) h = 100;
            return new PaperSize(name, w, h);
        }

        private void Doc_BeginPrint(object? sender, PrintEventArgs e)
        {
            _printCursor = 0;
        }

        private void Doc_PrintPage(object? sender, PrintPageEventArgs e)
        {
            if (_template == null)
            {
                e.HasMorePages = false;
                return;
            }

            var jobsExpanded = ExpandJobs(_jobs);
            if (_printCursor >= jobsExpanded.Count)
            {
                e.HasMorePages = false;
                return;
            }

            var g = e.Graphics;
            g.PageUnit = GraphicsUnit.Pixel;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            var dpiX = g.DpiX;
            var dpiY = g.DpiY;

            var page = e.MarginBounds;
            var labelRects = _template.GetLabelRects(page, dpiX, dpiY);

            bool showName = chkShowName.Checked;
            bool showPrice = chkShowPrice.Checked;

            var fontName = new Font("Segoe UI", 7f, FontStyle.Regular);
            var fontPrice = new Font("Segoe UI", 7f, FontStyle.Bold);
            var fontBarcodeText = new Font("Segoe UI", 7f, FontStyle.Regular);

            foreach (var rect in labelRects)
            {
                if (_printCursor >= jobsExpanded.Count) break;
                var job = jobsExpanded[_printCursor];
                DrawOneLabel(g, rect, job, showName, showPrice, fontName, fontPrice, fontBarcodeText);
                _printCursor++;
            }

            e.HasMorePages = _printCursor < jobsExpanded.Count;
        }

        private static List<LabelJob> ExpandJobs(List<LabelJob> jobs)
        {
            var list = new List<LabelJob>();
            foreach (var j in jobs)
            {
                int qty = j.QtyLabels <= 0 ? 1 : j.QtyLabels;
                for (int i = 0; i < qty; i++)
                    list.Add(j);
            }
            return list;
        }

        private void DrawOneLabel(Graphics g, RectangleF rect, LabelJob job, bool showName, bool showPrice, Font fontName, Font fontPrice, Font fontBarcodeText)
        {
            var pad = 4f;
            var inner = RectangleF.Inflate(rect, -pad, -pad);

            string code = (job.Barcode ?? "").Trim();
            if (string.IsNullOrWhiteSpace(code)) code = " ";

            float y = inner.Top;

            float nameH = showName ? fontName.GetHeight(g) + 1f : 0f;
            float priceH = showPrice ? fontPrice.GetHeight(g) + 1f : 0f;
            float textH = fontBarcodeText.GetHeight(g) + 1f;

            float barcodeH = inner.Height - nameH - priceH - textH - 2f;
            if (barcodeH < 18f) barcodeH = 18f;

            if (showName)
            {
                var name = (job.Name ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(job.Unit))
                    name = $"{name} ({job.Unit})";
                g.DrawString(TruncateToFit(g, name, fontName, inner.Width), fontName, Brushes.Black, new RectangleF(inner.Left, y, inner.Width, nameH));
                y += nameH;
            }

            var barcodeRect = new RectangleF(inner.Left, y, inner.Width, barcodeH);
            using (var bmp = Code128Renderer.Render(code, (int)Math.Ceiling(barcodeRect.Width), (int)Math.Ceiling(barcodeRect.Height)))
            {
                g.DrawImage(bmp, barcodeRect);
            }
            y += barcodeH;

            g.DrawString(code, fontBarcodeText, Brushes.Black, new RectangleF(inner.Left, y, inner.Width, textH), new StringFormat { Alignment = StringAlignment.Center });
            y += textH;

            if (showPrice)
            {
                string p = job.Price > 0 ? $"Rp {job.Price:N0}" : "";
                g.DrawString(p, fontPrice, Brushes.Black, new RectangleF(inner.Left, y, inner.Width, priceH), new StringFormat { Alignment = StringAlignment.Center });
            }
        }

        private static string TruncateToFit(Graphics g, string text, Font font, float width)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (g.MeasureString(text, font).Width <= width) return text;

            const string ellipsis = "…";
            int lo = 0;
            int hi = text.Length;
            while (lo < hi)
            {
                int mid = (lo + hi) / 2;
                var cand = text.Substring(0, mid) + ellipsis;
                if (g.MeasureString(cand, font).Width <= width) lo = mid + 1;
                else hi = mid;
            }
            int len = Math.Max(0, lo - 1);
            return text.Substring(0, len) + ellipsis;
        }

        private sealed class TemplateItem
        {
            public string Name { get; }
            public LabelTemplate Template { get; }
            public TemplateItem(string name, LabelTemplate template)
            {
                Name = name;
                Template = template;
            }
            public override string ToString() => Name;
        }

        private sealed class LabelJob
        {
            public string Barcode { get; set; } = "";
            public string Name { get; set; } = "";
            public string Unit { get; set; } = "";
            public decimal Price { get; set; }
            public int QtyLabels { get; set; }
        }

        private sealed class LabelTemplate
        {
            public TemplateKind Kind { get; }
            public int Columns { get; }
            public int Rows { get; }
            public float LabelWidthMm { get; }
            public float LabelHeightMm { get; }
            public float MarginLeftMm { get; }
            public float MarginTopMm { get; }
            public float GapMm { get; }
            public float PageWidthMm { get; }
            public float PageHeightMm { get; }

            private LabelTemplate(TemplateKind kind, int cols, int rows, float wMm, float hMm, float marginLeftMm, float marginTopMm, float gapMm, float pageWidthMm, float pageHeightMm)
            {
                Kind = kind;
                Columns = cols;
                Rows = rows;
                LabelWidthMm = wMm;
                LabelHeightMm = hMm;
                MarginLeftMm = marginLeftMm;
                MarginTopMm = marginTopMm;
                GapMm = gapMm;
                PageWidthMm = pageWidthMm;
                PageHeightMm = pageHeightMm;
            }

            public static LabelTemplate A4Grid(int cols, int rows, float wMm, float hMm, float marginLeftMm, float marginTopMm, float gapMm)
                => new LabelTemplate(TemplateKind.A4Sheet, cols, rows, wMm, hMm, marginLeftMm, marginTopMm, gapMm, 210f, 297f);

            public static LabelTemplate A4Auto(float wMm, float hMm, float marginLeftMm, float marginTopMm, float gapMm)
            {
                float pageW = 210f;
                float pageH = 297f;

                int cols = Math.Max(1, (int)Math.Floor((pageW - marginLeftMm * 2 + gapMm) / (wMm + gapMm)));
                int rows = Math.Max(1, (int)Math.Floor((pageH - marginTopMm * 2 + gapMm) / (hMm + gapMm)));

                return new LabelTemplate(TemplateKind.A4Sheet, cols, rows, wMm, hMm, marginLeftMm, marginTopMm, gapMm, pageW, pageH);
            }

            public static LabelTemplate Roll(float widthMm, float heightMm)
                => new LabelTemplate(TemplateKind.Roll, 1, 1, widthMm, heightMm, 0f, 0f, 0f, widthMm, heightMm);

            public List<RectangleF> GetLabelRects(Rectangle marginBoundsPx, float dpiX, float dpiY)
            {
                float mmToPxX(float mm) => (mm / 25.4f) * dpiX;
                float mmToPxY(float mm) => (mm / 25.4f) * dpiY;

                float startX = marginBoundsPx.Left + mmToPxX(MarginLeftMm);
                float startY = marginBoundsPx.Top + mmToPxY(MarginTopMm);
                float w = mmToPxX(LabelWidthMm);
                float h = mmToPxY(LabelHeightMm);
                float gapX = mmToPxX(GapMm);
                float gapY = mmToPxY(GapMm);

                if (Kind == TemplateKind.Roll)
                {
                    return new List<RectangleF> { new RectangleF(marginBoundsPx.Left, marginBoundsPx.Top, marginBoundsPx.Width, marginBoundsPx.Height) };
                }

                var list = new List<RectangleF>(Columns * Rows);
                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Columns; c++)
                    {
                        float x = startX + c * (w + gapX);
                        float y = startY + r * (h + gapY);
                        list.Add(new RectangleF(x, y, w, h));
                    }
                }
                return list;
            }
        }

        private enum TemplateKind
        {
            A4Sheet = 0,
            Roll = 1
        }

        private void ExportDataSheet(IXLWorksheet ws, List<LabelJob> expanded)
        {
            ws.Cell(1, 1).Value = "Barcode";
            ws.Cell(1, 2).Value = "Nama";
            ws.Cell(1, 3).Value = "Unit";
            ws.Cell(1, 4).Value = "Harga";

            int r = 2;
            foreach (var j in expanded)
            {
                ws.Cell(r, 1).Value = j.Barcode ?? "";
                ws.Cell(r, 2).Value = j.Name ?? "";
                ws.Cell(r, 3).Value = j.Unit ?? "";
                ws.Cell(r, 4).Value = j.Price;
                r++;
            }
            ws.Columns().AdjustToContents();
        }

        private void ExportLabelsToWorksheet(IXLWorksheet ws, List<LabelJob> expanded, bool showName, bool showPrice, List<MemoryStream> streams)
        {
            ws.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            ws.PageSetup.PaperSize = XLPaperSize.A4Paper;

            float cellMm = 5f;
            int labelCols = Math.Max(4, (int)Math.Round(_template.LabelWidthMm / cellMm));
            int labelRows = Math.Max(4, (int)Math.Round(_template.LabelHeightMm / cellMm));
            int gapCols = Math.Max(0, (int)Math.Round(_template.GapMm / cellMm));
            int gapRows = Math.Max(0, (int)Math.Round(_template.GapMm / cellMm));
            int marginCols = Math.Max(0, (int)Math.Round(_template.MarginLeftMm / cellMm));
            int marginRows = Math.Max(0, (int)Math.Round(_template.MarginTopMm / cellMm));

            double pxPerCell = (cellMm / 25.4f) * 96f;
            double colWidth = pxPerCell / 7.0;
            double rowHeight = (cellMm / 25.4f) * 72f;

            int colsPerPage = _template.Kind == TemplateKind.Roll ? 1 : _template.Columns;
            int rowsPerPage = _template.Kind == TemplateKind.Roll ? expanded.Count : _template.Rows;

            int totalCols = marginCols * 2 + colsPerPage * labelCols + Math.Max(0, colsPerPage - 1) * gapCols;
            for (int c = 1; c <= totalCols; c++)
                ws.Column(c).Width = colWidth;

            int currentIndex = 0;
            int page = 0;

            while (currentIndex < expanded.Count)
            {
                int baseRow = page * (marginRows * 2 + rowsPerPage * labelRows + Math.Max(0, rowsPerPage - 1) * gapRows) + 1;
                int rowsThisPage = _template.Kind == TemplateKind.Roll ? Math.Min(expanded.Count - currentIndex, 1) : rowsPerPage;

                int totalRowsThisPage = marginRows * 2 + rowsPerPage * labelRows + Math.Max(0, rowsPerPage - 1) * gapRows;
                for (int r = baseRow; r < baseRow + totalRowsThisPage; r++)
                    ws.Row(r).Height = rowHeight;

                for (int r = 0; r < rowsPerPage; r++)
                {
                    for (int c = 0; c < colsPerPage; c++)
                    {
                        if (currentIndex >= expanded.Count) break;
                        if (_template.Kind == TemplateKind.Roll && (r != 0 || c != 0)) continue;

                        int startRow = baseRow + marginRows + r * (labelRows + gapRows);
                        int startCol = 1 + marginCols + c * (labelCols + gapCols);

                        var job = expanded[currentIndex];
                        WriteOneLabel(ws, startRow, startCol, labelRows, labelCols, job, showName, showPrice, streams);
                        currentIndex++;
                    }
                }

                page++;
            }

            var used = ws.RangeUsed();
            if (used != null)
            {
                ws.PageSetup.PrintAreas.Clear();
                ws.PageSetup.PrintAreas.Add(used.RangeAddress.ToString());
            }
        }

        private void WriteOneLabel(IXLWorksheet ws, int startRow, int startCol, int labelRows, int labelCols, LabelJob job, bool showName, bool showPrice, List<MemoryStream> streams)
        {
            int endRow = startRow + labelRows - 1;
            int endCol = startCol + labelCols - 1;

            var outer = ws.Range(startRow, startCol, endRow, endCol);
            outer.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            outer.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            string code = (job.Barcode ?? "").Trim();
            if (string.IsNullOrWhiteSpace(code)) code = "-";

            int headerRow = startRow;
            int footerRow = endRow;

            var header = ws.Range(headerRow, startCol, headerRow, endCol);
            header.Merge();
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            header.Style.Font.FontName = "Segoe UI";
            header.Style.Font.FontSize = 9;
            string headerLeft = showName ? $"{job.Name}{(string.IsNullOrWhiteSpace(job.Unit) ? "" : $" ({job.Unit})")}" : "";
            string headerRight = showPrice && job.Price > 0 ? $"Rp {job.Price:N0}" : "";
            header.Value = string.IsNullOrWhiteSpace(headerRight) ? headerLeft : $"{headerLeft} | {headerRight}";
            if (!string.IsNullOrWhiteSpace(headerRight)) header.Style.Font.Bold = true;

            int barcodeTop = startRow + 1;
            int barcodeBottom = endRow - 1;
            if (barcodeBottom <= barcodeTop) barcodeBottom = barcodeTop;

            var barcodeArea = ws.Range(barcodeTop, startCol, barcodeBottom, endCol);
            barcodeArea.Merge();
            barcodeArea.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            barcodeArea.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            int widthPx = (int)Math.Round((labelCols * 5f / 25.4f) * 96f) - 12;
            int heightPx = (int)Math.Round(((barcodeBottom - barcodeTop + 1) * 5f / 25.4f) * 96f) - 12;
            if (widthPx < 60) widthPx = 60;
            if (heightPx < 30) heightPx = 30;

            using (var bmp = Code128Renderer.Render(code, widthPx, heightPx))
            {
                var bytes = BitmapToPngBytes(bmp);
                var ms = new MemoryStream(bytes);
                streams.Add(ms);
                var pic = ws.AddPicture(ms).MoveTo(ws.Cell(barcodeTop, startCol));
                pic.WithSize(widthPx, heightPx);
            }

            var footer = ws.Range(footerRow, startCol, footerRow, endCol);
            footer.Merge();
            footer.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            footer.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            footer.Style.Font.FontName = "Segoe UI";
            footer.Style.Font.FontSize = 9;
            footer.Value = code;
        }

        private static byte[] BitmapToPngBytes(Bitmap bmp)
        {
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        private static class Code128Renderer
        {
            private static readonly string[] Patterns = new[]
            {
                "212222","222122","222221","121223","121322","131222","122213","122312","132212","221213",
                "221312","231212","112232","122132","122231","113222","123122","123221","223211","221132",
                "221231","213212","223112","312131","311222","321122","321221","312212","322112","322211",
                "212123","212321","232121","111323","131123","131321","112313","132113","132311","211313",
                "231113","231311","112133","112331","132131","113123","113321","133121","313121","211331",
                "231131","213113","213311","213131","311123","311321","331121","312113","312311","332111",
                "314111","221411","431111","111224","111422","121124","121421","141122","141221","112214",
                "112412","122114","122411","142112","142211","241211","221114","413111","241112","134111",
                "111242","121142","121241","114212","124112","124211","411212","421112","421211","212141",
                "214121","412121","111143","111341","131141","114113","114311","411113","411311","113141",
                "114131","311141","411131","211412","211214","211232","2331112"
            };

            public static Bitmap Render(string text, int targetWidthPx, int targetHeightPx)
            {
                var codes = EncodeCode128B(text);
                var modules = BuildModuleSequence(codes);
                int totalModules = modules.Sum(m => m);
                int quiet = 10;
                int width = Math.Max(targetWidthPx, totalModules + quiet * 2);
                int height = Math.Max(targetHeightPx, 20);

                var bmp = new Bitmap(width, height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                    float scale = (width - quiet * 2f) / totalModules;
                    float x = quiet;
                    bool bar = true;
                    foreach (var w in modules)
                    {
                        float wPx = w * scale;
                        if (bar)
                        {
                            g.FillRectangle(Brushes.Black, x, 0, wPx, height);
                        }
                        x += wPx;
                        bar = !bar;
                    }
                }
                return bmp;
            }

            private static List<int> EncodeCode128B(string text)
            {
                var codes = new List<int>();
                const int startB = 104;
                codes.Add(startB);

                foreach (char ch in text)
                {
                    int val = ch - 32;
                    if (val < 0 || val > 95) val = 0;
                    codes.Add(val);
                }

                int checksum = startB;
                for (int i = 1; i < codes.Count; i++)
                {
                    checksum += codes[i] * i;
                }
                checksum %= 103;
                codes.Add(checksum);
                codes.Add(106);
                return codes;
            }

            private static List<int> BuildModuleSequence(List<int> codes)
            {
                var modules = new List<int>();
                foreach (int code in codes)
                {
                    string p = Patterns[code];
                    foreach (char ch in p)
                    {
                        modules.Add(ch - '0');
                    }
                }
                return modules;
            }
        }
    }
}
