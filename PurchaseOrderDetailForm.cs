using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace POS_qu
{
    public partial class PurchaseOrderDetailForm : Form
    {
        private readonly PrintDocument _printDoc = new PrintDocument();
        private int _printRowCursor;
        private DataTable _dtDetail;

        private long _poId;
        private string _poNo = "";
        private string _supplier = "";
        private DateTime _orderDate = DateTime.Today;
        private string _status = "";
        private string _warehouse = "";
        private string _note = "";
        private decimal _total = 0m;

        public PurchaseOrderDetailForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            HookUi();
        }

        public PurchaseOrderDetailForm(long poId)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            _poId = poId;
            HookUi();
            LoadPODetail();
        }

        private void HookUi()
        {
            btnClose.Click += btnClose_Click;
            btnPreview.Click += btnPreview_Click;
            btnSavePdf.Click += btnSavePdf_Click;

            _printDoc.BeginPrint += PrintDoc_BeginPrint;
            _printDoc.PrintPage += PrintDoc_PrintPage;

            ApplyGridStyle();
        }

        private void LoadPODetail()
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string queryHeader = @"
SELECT
    po.id,
    COALESCE(s.name, '-') AS supplier,
    po.order_date,
    po.status,
    po.total_amount,
    COALESCE(po.note, '') AS note,
    COALESCE(w.name, '-') AS warehouse_name
FROM purchase_orders po
LEFT JOIN suppliers s ON po.supplier_id = s.id
LEFT JOIN warehouses w ON po.warehouse_id = w.id
WHERE po.id = @id";

            using var cmd = new NpgsqlCommand(queryHeader, conn);
            cmd.Parameters.AddWithValue("@id", _poId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                _poNo = reader["id"]?.ToString() ?? _poId.ToString();
                _supplier = reader["supplier"]?.ToString() ?? "-";
                _orderDate = reader["order_date"] != DBNull.Value ? Convert.ToDateTime(reader["order_date"]) : DateTime.Today;
                _status = reader["status"]?.ToString() ?? "";
                _note = reader["note"]?.ToString() ?? "";
                _warehouse = reader["warehouse_name"]?.ToString() ?? "-";

                _total = reader["total_amount"] != DBNull.Value ? Convert.ToDecimal(reader["total_amount"]) : 0m;
            }
            reader.Close();

            string queryDetail = @"
    SELECT i.name AS item, poi.quantity, poi.unit_price,poi.unit,
           (poi.quantity * poi.unit_price) AS subtotal, poi.note
    FROM purchase_order_items poi
    JOIN items i ON poi.item_id = i.id
    WHERE poi.po_id = @po_id";

            using var cmdd = new NpgsqlCommand(queryDetail, conn);
            cmdd.Parameters.AddWithValue("@po_id", _poId);

            using var da = new NpgsqlDataAdapter(cmdd);
            _dtDetail = new DataTable();
            da.Fill(_dtDetail);
            dgvItems.DataSource = _dtDetail;

            dgvItems.Columns["item"].HeaderText = "Item";
            dgvItems.Columns["quantity"].HeaderText = "Qty";
            dgvItems.Columns["unit"].HeaderText = "Unit";
            dgvItems.Columns["unit_price"].HeaderText = "Harga Beli";
            dgvItems.Columns["subtotal"].HeaderText = "Subtotal";
            dgvItems.Columns["note"].HeaderText = "Note";

            if (dgvItems.Columns.Contains("quantity"))
            {
                dgvItems.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvItems.Columns["quantity"].DefaultCellStyle.Format = "N2";
            }

            if (dgvItems.Columns.Contains("unit_price"))
            {
                dgvItems.Columns["unit_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvItems.Columns["unit_price"].DefaultCellStyle.Format = "N0";
            }

            if (dgvItems.Columns.Contains("subtotal"))
            {
                dgvItems.Columns["subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvItems.Columns["subtotal"].DefaultCellStyle.Format = "N0";
            }

            if (_dtDetail != null && _dtDetail.Columns.Contains("subtotal"))
            {
                _total = _dtDetail.AsEnumerable()
                    .Where(r => r["subtotal"] != DBNull.Value)
                    .Sum(r => Convert.ToDecimal(r["subtotal"]));
            }

            BindHeader();
        }

        private void BindHeader()
        {
            lblPoNoValue.Text = _poNo;
            lblSupplierValue.Text = _supplier;
            lblDateValue.Text = _orderDate.ToString("dd/MM/yyyy");
            lblStatusValue.Text = _status;
            lblWarehouseValue.Text = _warehouse;
            lblNoteValue.Text = string.IsNullOrWhiteSpace(_note) ? "-" : _note;
            lblTotalValue.Text = $"Rp {_total:N0}";

            lblTitle.Text = $"Faktur Pembelian #{_poNo}";
            Text = lblTitle.Text;
        }

        private void ApplyGridStyle()
        {
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dgvItems.ColumnHeadersHeight = 45;
            dgvItems.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgvItems.DefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvItems.RowTemplate.Height = 40;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (_dtDetail == null || _dtDetail.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada item.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new PrintPreviewDialog();
            dlg.Document = _printDoc;
            dlg.WindowState = FormWindowState.Maximized;
            dlg.ShowDialog(this);
        }

        private void btnSavePdf_Click(object sender, EventArgs e)
        {
            if (_dtDetail == null || _dtDetail.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada item.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog();
            sfd.Filter = "PDF files (*.pdf)|*.pdf";
            sfd.FileName = $"Faktur_Pembelian_{_poNo}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
                BuildPdf().GeneratePdf(sfd.FileName);
                MessageBox.Show("PDF berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("explorer.exe", sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuat PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Document BuildPdf()
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text($"FAKTUR PEMBELIAN #{_poNo}").FontSize(16).Bold().AlignCenter();
                        col.Item().Text($"Tanggal: {_orderDate:dd/MM/yyyy}   |   Status: {_status}").AlignCenter();
                        col.Item().Text($"Supplier: {_supplier}   |   Gudang: {_warehouse}").AlignCenter();
                    });

                    page.Content().PaddingTop(15).Column(col =>
                    {
                        if (!string.IsNullOrWhiteSpace(_note))
                            col.Item().Text($"Catatan: {_note}");

                        col.Item().PaddingTop(10).Table(t =>
                        {
                            t.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(30);
                                c.RelativeColumn(3);
                                c.ConstantColumn(70);
                                c.ConstantColumn(80);
                                c.ConstantColumn(90);
                                c.RelativeColumn(2);
                            });

                            t.Header(h =>
                            {
                                h.Cell().Text("No").Bold();
                                h.Cell().Text("Item").Bold();
                                h.Cell().Text("Qty").Bold();
                                h.Cell().Text("Unit").Bold();
                                h.Cell().Text("Harga").Bold();
                                h.Cell().Text("Subtotal").Bold();
                            });

                            int no = 1;
                            foreach (DataRow r in _dtDetail.Rows)
                            {
                                var item = r["item"]?.ToString() ?? "";
                                var qty = r["quantity"] != DBNull.Value ? Convert.ToDecimal(r["quantity"]) : 0m;
                                var unit = r["unit"]?.ToString() ?? "";
                                var price = r["unit_price"] != DBNull.Value ? Convert.ToDecimal(r["unit_price"]) : 0m;
                                var sub = r["subtotal"] != DBNull.Value ? Convert.ToDecimal(r["subtotal"]) : 0m;
                                var note = r["note"]?.ToString() ?? "";

                                t.Cell().Text(no.ToString());
                                t.Cell().Text(item);
                                t.Cell().AlignRight().Text(qty.ToString("N2"));
                                t.Cell().Text(unit);
                                t.Cell().AlignRight().Text(price.ToString("N0"));
                                t.Cell().AlignRight().Text(sub.ToString("N0"));

                                if (!string.IsNullOrWhiteSpace(note))
                                {
                                    t.Cell().ColumnSpan(6).PaddingBottom(4).Text("Catatan: " + note).FontSize(9).FontColor(Colors.Grey.Darken2);
                                }

                                no++;
                            }
                        });
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("Total: ").SemiBold();
                        txt.Span($"Rp {_total:N0}").Bold();
                    });
                });
            });
        }

        private void PrintDoc_BeginPrint(object sender, PrintEventArgs e)
        {
            _printRowCursor = 0;
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_dtDetail == null)
            {
                e.HasMorePages = false;
                return;
            }

            var g = e.Graphics;
            var b = e.MarginBounds;

            using var fontTitle = new Font("Segoe UI", 12f, FontStyle.Bold);
            using var font = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fontBold = new Font("Segoe UI", 9f, FontStyle.Bold);
            using var pen = new Pen(Color.FromArgb(200, 200, 200), 1f);

            float y = b.Top;
            g.DrawString($"FAKTUR PEMBELIAN #{_poNo}", fontTitle, Brushes.Black, b.Left, y);
            y += fontTitle.GetHeight(g) + 6;
            g.DrawString($"Tanggal: {_orderDate:dd/MM/yyyy}   |   Status: {_status}", font, Brushes.Black, b.Left, y);
            y += font.GetHeight(g) + 3;
            g.DrawString($"Supplier: {_supplier}   |   Gudang: {_warehouse}", font, Brushes.Black, b.Left, y);
            y += font.GetHeight(g) + 8;
            g.DrawLine(pen, b.Left, y, b.Right, y);
            y += 10;

            float colNo = 35;
            float colQty = 70;
            float colUnit = 70;
            float colPrice = 90;
            float colSub = 90;
            float colName = b.Width - colNo - colQty - colUnit - colPrice - colSub - 10;
            if (colName < 220) colName = 220;

            float x = b.Left;
            float headerH = fontBold.GetHeight(g) + 6;
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), b.Left, y, b.Width, headerH);
            g.DrawString("No", fontBold, Brushes.Black, x, y + 3); x += colNo;
            g.DrawString("Item", fontBold, Brushes.Black, x, y + 3); x += colName;
            g.DrawString("Qty", fontBold, Brushes.Black, x, y + 3); x += colQty;
            g.DrawString("Unit", fontBold, Brushes.Black, x, y + 3); x += colUnit;
            g.DrawString("Harga", fontBold, Brushes.Black, x, y + 3); x += colPrice;
            g.DrawString("Sub", fontBold, Brushes.Black, x, y + 3);
            y += headerH + 6;

            int rowNo = _printRowCursor + 1;
            while (_printRowCursor < _dtDetail.Rows.Count)
            {
                var r = _dtDetail.Rows[_printRowCursor];
                string item = r["item"]?.ToString() ?? "";
                decimal qty = r["quantity"] != DBNull.Value ? Convert.ToDecimal(r["quantity"]) : 0m;
                string unit = r["unit"]?.ToString() ?? "";
                decimal price = r["unit_price"] != DBNull.Value ? Convert.ToDecimal(r["unit_price"]) : 0m;
                decimal sub = r["subtotal"] != DBNull.Value ? Convert.ToDecimal(r["subtotal"]) : 0m;

                float rowH = font.GetHeight(g) + 6;
                if (y + rowH > b.Bottom - 60)
                {
                    e.HasMorePages = true;
                    return;
                }

                x = b.Left;
                g.DrawString(rowNo.ToString(), font, Brushes.Black, x, y); x += colNo;
                g.DrawString(Trunc(g, item, font, colName), font, Brushes.Black, x, y); x += colName;
                g.DrawString(qty.ToString("N2"), font, Brushes.Black, x, y); x += colQty;
                g.DrawString(unit, font, Brushes.Black, x, y); x += colUnit;
                g.DrawString(price.ToString("N0"), font, Brushes.Black, x, y); x += colPrice;
                g.DrawString(sub.ToString("N0"), font, Brushes.Black, x, y);

                y += rowH;
                _printRowCursor++;
                rowNo++;
            }

            y += 8;
            g.DrawLine(pen, b.Left, y, b.Right, y);
            y += 8;
            g.DrawString($"Total: Rp {_total:N0}", fontBold, Brushes.Black, b.Right - 200, y);

            e.HasMorePages = false;
        }

        private static string Trunc(Graphics g, string s, Font f, float maxW)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (g.MeasureString(s, f).Width <= maxW) return s;
            const string ell = "…";
            int len = s.Length;
            while (len > 0)
            {
                var cand = s.Substring(0, len) + ell;
                if (g.MeasureString(cand, f).Width <= maxW) return cand;
                len--;
            }
            return ell;
        }
    }
}
