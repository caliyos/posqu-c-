using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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
            public int Columns { get; }
            public int Rows { get; }
            public float LabelWidthMm { get; }
            public float LabelHeightMm { get; }
            public float MarginLeftMm { get; }
            public float MarginTopMm { get; }
            public float GapMm { get; }

            private LabelTemplate(int cols, int rows, float wMm, float hMm, float marginLeftMm, float marginTopMm, float gapMm)
            {
                Columns = cols;
                Rows = rows;
                LabelWidthMm = wMm;
                LabelHeightMm = hMm;
                MarginLeftMm = marginLeftMm;
                MarginTopMm = marginTopMm;
                GapMm = gapMm;
            }

            public static LabelTemplate A4Grid(int cols, int rows, float wMm, float hMm, float marginLeftMm, float marginTopMm, float gapMm)
                => new LabelTemplate(cols, rows, wMm, hMm, marginLeftMm, marginTopMm, gapMm);

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

