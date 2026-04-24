using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_qu
{
    public enum InventoryAdjustmentDirection
    {
        In = 1,
        Out = 2
    }

    public partial class InventoryAdjustmentListForm : Form
    {
        private readonly InventoryAdjustmentDirection _direction;
        private DataTable _dtHeader;
        private DataTable _dtDetail;
        private readonly PrintDocument _printDoc = new PrintDocument();
        private AdjustmentPrintData _printData;
        private int _printRowCursor;

        public InventoryAdjustmentListForm(InventoryAdjustmentDirection direction)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            _direction = direction;
            Load += InventoryAdjustmentListForm_Load;
            txtSearch.TextChanged += txtSearch_TextChanged;
            btnAdd.Click += btnAdd_Click;
            btnPrint.Click += btnPrint_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnClose.Click += btnClose_Click;

            _printDoc.BeginPrint += PrintDoc_BeginPrint;
            _printDoc.PrintPage += PrintDoc_PrintPage;
        }

        private void InventoryAdjustmentListForm_Load(object sender, EventArgs e)
        {
            lblTitle.Text = _direction == InventoryAdjustmentDirection.In ? "Daftar Item Masuk (Adjustment IN)" : "Daftar Item Keluar (Adjustment OUT)";
            Text = lblTitle.Text;
            EnsureTables();
            LoadHeaderData();
            ApplyGridStyle(dgvHeader);
            ApplyGridStyle(dgvDetail);
            dgvHeader.SelectionChanged += dgvHeader_SelectionChanged;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadHeaderData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using var f = new InventoryAdjustmentEntryForm(_direction);
            if (f.ShowDialog(this) != DialogResult.OK) return;
            LoadHeaderData();
            SelectHeaderById(f.CreatedAdjustmentId);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            var id = GetSelectedHeaderId();
            if (id <= 0)
            {
                MessageBox.Show("Pilih salah satu baris dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var data = LoadPrintData(id);
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadHeaderData();
        }

        private void dgvHeader_SelectionChanged(object sender, EventArgs e)
        {
            var id = GetSelectedHeaderId();
            LoadDetailData(id);
        }

        private void LoadHeaderData()
        {
            _dtHeader = new DataTable();
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string dir = _direction == InventoryAdjustmentDirection.In ? "IN" : "OUT";
            string search = (txtSearch.Text ?? "").Trim();

            using var cmd = new NpgsqlCommand(@"
SELECT
    a.id,
    a.adjustment_no,
    a.adjustment_date,
    COALESCE(w.name, '') AS warehouse_name,
    a.reason,
    COALESCE(a.note, '') AS note,
    COALESCE(u.username, '') AS created_by,
    a.created_at,
    COALESCE((SELECT SUM(it.qty) FROM inventory_adjustment_items it WHERE it.adjustment_id = a.id), 0) AS total_qty,
    COALESCE((SELECT COUNT(1) FROM inventory_adjustment_items it WHERE it.adjustment_id = a.id), 0) AS total_items
FROM inventory_adjustments a
LEFT JOIN warehouses w ON w.id = a.warehouse_id
LEFT JOIN users u ON u.id = a.created_by
WHERE a.direction = @dir
  AND (@search = '' OR a.adjustment_no ILIKE @search OR a.reason ILIKE @search OR a.note ILIKE @search)
ORDER BY a.id DESC
", con);
            cmd.Parameters.AddWithValue("@dir", dir);
            cmd.Parameters.AddWithValue("@search", string.IsNullOrWhiteSpace(search) ? "" : "%" + search + "%");

            using var da = new NpgsqlDataAdapter(cmd);
            da.Fill(_dtHeader);

            dgvHeader.DataSource = _dtHeader;
            if (dgvHeader.Columns.Contains("id")) dgvHeader.Columns["id"].Visible = false;

            if (dgvHeader.Columns.Contains("adjustment_no"))
            {
                dgvHeader.Columns["adjustment_no"].HeaderText = "No Dokumen";
                dgvHeader.Columns["adjustment_no"].Width = 170;
            }

            if (dgvHeader.Columns.Contains("adjustment_date"))
            {
                dgvHeader.Columns["adjustment_date"].HeaderText = "Tanggal";
                dgvHeader.Columns["adjustment_date"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvHeader.Columns["adjustment_date"].Width = 110;
            }

            if (dgvHeader.Columns.Contains("warehouse_name"))
            {
                dgvHeader.Columns["warehouse_name"].HeaderText = "Gudang";
                dgvHeader.Columns["warehouse_name"].Width = 160;
            }

            if (dgvHeader.Columns.Contains("total_items"))
            {
                dgvHeader.Columns["total_items"].HeaderText = "Item";
                dgvHeader.Columns["total_items"].Width = 60;
                dgvHeader.Columns["total_items"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvHeader.Columns.Contains("total_qty"))
            {
                dgvHeader.Columns["total_qty"].HeaderText = "Total Qty";
                dgvHeader.Columns["total_qty"].Width = 100;
                dgvHeader.Columns["total_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvHeader.Columns["total_qty"].DefaultCellStyle.Format = "N2";
            }

            if (dgvHeader.Columns.Contains("reason"))
            {
                dgvHeader.Columns["reason"].HeaderText = "Alasan";
                dgvHeader.Columns["reason"].Width = 220;
            }

            if (dgvHeader.Columns.Contains("note"))
            {
                dgvHeader.Columns["note"].HeaderText = "Catatan";
                dgvHeader.Columns["note"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvHeader.Columns["note"].MinimumWidth = 200;
            }

            if (dgvHeader.Columns.Contains("created_by"))
            {
                dgvHeader.Columns["created_by"].HeaderText = "User";
                dgvHeader.Columns["created_by"].Width = 120;
            }

            if (dgvHeader.Columns.Contains("created_at"))
            {
                dgvHeader.Columns["created_at"].HeaderText = "Dibuat";
                dgvHeader.Columns["created_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvHeader.Columns["created_at"].Width = 150;
            }

            if (dgvHeader.Rows.Count > 0) dgvHeader.Rows[0].Selected = true;
            LoadDetailData(GetSelectedHeaderId());
        }

        private void LoadDetailData(int adjustmentId)
        {
            _dtDetail = new DataTable();
            dgvDetail.DataSource = _dtDetail;
            if (adjustmentId <= 0) return;

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT
    it.id,
    i.barcode,
    i.name,
    COALESCE(u.name, '') AS unit_name,
    it.qty,
    it.buy_price,
    COALESCE(it.note, '') AS note
FROM inventory_adjustment_items it
JOIN items i ON i.id = it.item_id
LEFT JOIN units u ON i.unit = u.id
WHERE it.adjustment_id = @id
ORDER BY it.id ASC
", con);
            cmd.Parameters.AddWithValue("@id", adjustmentId);
            using var da = new NpgsqlDataAdapter(cmd);
            da.Fill(_dtDetail);

            dgvDetail.DataSource = _dtDetail;
            if (dgvDetail.Columns.Contains("id")) dgvDetail.Columns["id"].Visible = false;

            if (dgvDetail.Columns.Contains("barcode"))
            {
                dgvDetail.Columns["barcode"].HeaderText = "Barcode";
                dgvDetail.Columns["barcode"].Width = 160;
            }

            if (dgvDetail.Columns.Contains("name"))
            {
                dgvDetail.Columns["name"].HeaderText = "Nama";
                dgvDetail.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDetail.Columns["name"].MinimumWidth = 200;
            }

            if (dgvDetail.Columns.Contains("unit_name"))
            {
                dgvDetail.Columns["unit_name"].HeaderText = "Satuan";
                dgvDetail.Columns["unit_name"].Width = 100;
            }

            if (dgvDetail.Columns.Contains("qty"))
            {
                dgvDetail.Columns["qty"].HeaderText = "Qty";
                dgvDetail.Columns["qty"].Width = 90;
                dgvDetail.Columns["qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetail.Columns["qty"].DefaultCellStyle.Format = "N2";
            }

            if (dgvDetail.Columns.Contains("buy_price"))
            {
                dgvDetail.Columns["buy_price"].HeaderText = "HPP";
                dgvDetail.Columns["buy_price"].Width = 110;
                dgvDetail.Columns["buy_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetail.Columns["buy_price"].DefaultCellStyle.Format = "N0";
                dgvDetail.Columns["buy_price"].Visible = _direction == InventoryAdjustmentDirection.In;
            }

            if (dgvDetail.Columns.Contains("note"))
            {
                dgvDetail.Columns["note"].HeaderText = "Catatan";
                dgvDetail.Columns["note"].Width = 220;
            }
        }

        private void ApplyGridStyle(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            dgv.GridColor = Color.FromArgb(235, 235, 235);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoGenerateColumns = true;
        }

        private AdjustmentPrintData LoadPrintData(int id)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT
    a.id,
    a.adjustment_no,
    a.adjustment_date,
    COALESCE(w.name, '') AS warehouse_name,
    a.reason,
    COALESCE(a.note, '') AS note,
    COALESCE(u.username, '') AS created_by,
    a.created_at
FROM inventory_adjustments a
LEFT JOIN warehouses w ON w.id = a.warehouse_id
LEFT JOIN users u ON u.id = a.created_by
WHERE a.id = @id
LIMIT 1
", con);
            cmd.Parameters.AddWithValue("@id", id);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            var data = new AdjustmentPrintData
            {
                Id = r["id"] != DBNull.Value ? Convert.ToInt32(r["id"]) : 0,
                AdjustmentNo = r["adjustment_no"]?.ToString() ?? "",
                AdjustmentDate = r["adjustment_date"] != DBNull.Value ? Convert.ToDateTime(r["adjustment_date"]) : DateTime.Today,
                WarehouseName = r["warehouse_name"]?.ToString() ?? "",
                Reason = r["reason"]?.ToString() ?? "",
                Note = r["note"]?.ToString() ?? "",
                Username = r["created_by"]?.ToString() ?? "",
                CreatedAt = r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now,
                Items = new List<AdjustmentPrintItem>()
            };

            r.Close();

            using var cmdItems = new NpgsqlCommand(@"
SELECT
    i.barcode,
    i.name,
    COALESCE(u.name, '') AS unit_name,
    it.qty,
    it.buy_price,
    COALESCE(it.note, '') AS note
FROM inventory_adjustment_items it
JOIN items i ON i.id = it.item_id
LEFT JOIN units u ON i.unit = u.id
WHERE it.adjustment_id = @id
ORDER BY it.id ASC
", con);
            cmdItems.Parameters.AddWithValue("@id", id);
            using var rr = cmdItems.ExecuteReader();
            while (rr.Read())
            {
                data.Items.Add(new AdjustmentPrintItem
                {
                    Barcode = rr["barcode"]?.ToString() ?? "",
                    Name = rr["name"]?.ToString() ?? "",
                    UnitName = rr["unit_name"]?.ToString() ?? "",
                    Qty = rr["qty"] != DBNull.Value ? Convert.ToDouble(rr["qty"]) : 0d,
                    BuyPrice = rr["buy_price"] != DBNull.Value ? Convert.ToDecimal(rr["buy_price"]) : 0m,
                    Note = rr["note"]?.ToString() ?? ""
                });
            }
            return data;
        }

        private void PrintDoc_BeginPrint(object sender, PrintEventArgs e)
        {
            _printRowCursor = 0;
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var data = _printData;
            if (data == null)
            {
                e.HasMorePages = false;
                return;
            }

            var g = e.Graphics;
            var b = e.MarginBounds;

            using var fontTitle = new Font("Segoe UI", 14f, FontStyle.Bold);
            using var font = new Font("Segoe UI", 10f, FontStyle.Regular);
            using var fontBold = new Font("Segoe UI", 10f, FontStyle.Bold);
            using var pen = new Pen(Color.FromArgb(200, 200, 200), 1f);

            float y = b.Top;

            var title = _direction == InventoryAdjustmentDirection.In ? "FAKTUR ITEM MASUK (ADJUSTMENT IN)" : "FAKTUR ITEM KELUAR (ADJUSTMENT OUT)";
            g.DrawString(title, fontTitle, Brushes.Black, b.Left, y);
            y += fontTitle.GetHeight(g) + 8;

            g.DrawLine(pen, b.Left, y, b.Right, y);
            y += 10;

            float leftW = 140;
            float x1 = b.Left;
            float x2 = b.Left + leftW;
            float lineH = font.GetHeight(g) + 6;

            DrawField(g, fontBold, font, x1, x2, ref y, lineH, "No Dokumen", data.AdjustmentNo);
            DrawField(g, fontBold, font, x1, x2, ref y, lineH, "Tanggal", data.AdjustmentDate.ToString("dd/MM/yyyy"));
            DrawField(g, fontBold, font, x1, x2, ref y, lineH, "Gudang", data.WarehouseName);
            DrawField(g, fontBold, font, x1, x2, ref y, lineH, "Alasan", data.Reason);
            DrawField(g, fontBold, font, x1, x2, ref y, lineH, "Catatan", data.Note);
            DrawField(g, fontBold, font, x1, x2, ref y, lineH, "User", data.Username);

            y += 8;
            g.DrawLine(pen, b.Left, y, b.Right, y);
            y += 10;

            float colNo = 40;
            float colBarcode = 140;
            float colUnit = 80;
            float colQty = 90;
            float colHpp = _direction == InventoryAdjustmentDirection.In ? 110 : 0;
            float colNote = 180;
            float colName = b.Width - colNo - colBarcode - colUnit - colQty - colHpp - colNote;
            if (colName < 180) colName = 180;

            float headerH = fontBold.GetHeight(g) + 6;
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), b.Left, y, b.Width, headerH);

            float x = b.Left;
            g.DrawString("No", fontBold, Brushes.Black, x, y + 3); x += colNo;
            g.DrawString("Barcode", fontBold, Brushes.Black, x, y + 3); x += colBarcode;
            g.DrawString("Nama", fontBold, Brushes.Black, x, y + 3); x += colName;
            g.DrawString("Unit", fontBold, Brushes.Black, x, y + 3); x += colUnit;
            g.DrawString("Qty", fontBold, Brushes.Black, x, y + 3); x += colQty;
            if (_direction == InventoryAdjustmentDirection.In)
            {
                g.DrawString("HPP", fontBold, Brushes.Black, x, y + 3); x += colHpp;
            }
            g.DrawString("Catatan", fontBold, Brushes.Black, x, y + 3);
            y += headerH + 6;

            int rowNo = _printRowCursor + 1;
            while (_printRowCursor < data.Items.Count)
            {
                var it = data.Items[_printRowCursor];
                float rowH = font.GetHeight(g) + 6;
                if (y + rowH > b.Bottom - 110)
                {
                    e.HasMorePages = true;
                    return;
                }

                x = b.Left;
                g.DrawString(rowNo.ToString(), font, Brushes.Black, x, y); x += colNo;
                g.DrawString(it.Barcode, font, Brushes.Black, x, y); x += colBarcode;
                g.DrawString(Trunc(g, it.Name, font, colName), font, Brushes.Black, x, y); x += colName;
                g.DrawString(it.UnitName, font, Brushes.Black, x, y); x += colUnit;
                g.DrawString(it.Qty.ToString("N2"), font, Brushes.Black, x, y); x += colQty;
                if (_direction == InventoryAdjustmentDirection.In)
                {
                    g.DrawString(it.BuyPrice.ToString("N0"), font, Brushes.Black, x, y); x += colHpp;
                }
                g.DrawString(Trunc(g, it.Note, font, colNote), font, Brushes.Black, x, y);

                y += rowH;
                _printRowCursor++;
                rowNo++;
            }

            y += 10;
            g.DrawLine(pen, b.Left, y, b.Right, y);
            y += 18;

            float boxW = (b.Width - 20) / 2f;
            float boxH = 90;
            var rect1 = new RectangleF(b.Left, y, boxW, boxH);
            var rect2 = new RectangleF(b.Left + boxW + 20, y, boxW, boxH);
            g.DrawRectangle(pen, rect1.X, rect1.Y, rect1.Width, rect1.Height);
            g.DrawRectangle(pen, rect2.X, rect2.Y, rect2.Width, rect2.Height);
            g.DrawString("Dibuat Oleh", font, Brushes.Black, rect1.X + 8, rect1.Y + 8);
            g.DrawString("Disetujui", font, Brushes.Black, rect2.X + 8, rect2.Y + 8);

            e.HasMorePages = false;
        }

        private static void DrawField(Graphics g, Font bold, Font normal, float x1, float x2, ref float y, float lineH, string label, string value)
        {
            g.DrawString(label, bold, Brushes.Black, x1, y);
            g.DrawString(": " + (value ?? ""), normal, Brushes.Black, x2, y);
            y += lineH;
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

        private sealed class AdjustmentPrintData
        {
            public int Id { get; set; }
            public string AdjustmentNo { get; set; }
            public DateTime AdjustmentDate { get; set; }
            public string WarehouseName { get; set; }
            public string Reason { get; set; }
            public string Note { get; set; }
            public string Username { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<AdjustmentPrintItem> Items { get; set; }
        }

        private sealed class AdjustmentPrintItem
        {
            public string Barcode { get; set; }
            public string Name { get; set; }
            public string UnitName { get; set; }
            public double Qty { get; set; }
            public decimal BuyPrice { get; set; }
            public string Note { get; set; }
        }

        private int GetSelectedHeaderId()
        {
            if (dgvHeader.CurrentRow == null) return 0;
            if (dgvHeader.CurrentRow.IsNewRow) return 0;
            var v = dgvHeader.CurrentRow.Cells["id"]?.Value;
            if (v == null || v == DBNull.Value) return 0;
            return Convert.ToInt32(v);
        }

        private void SelectHeaderById(int id)
        {
            if (id <= 0) return;
            foreach (DataGridViewRow r in dgvHeader.Rows)
            {
                if (r.IsNewRow) continue;
                var v = r.Cells["id"]?.Value;
                if (v == null || v == DBNull.Value) continue;
                if (Convert.ToInt32(v) == id)
                {
                    r.Selected = true;
                    if (dgvHeader.Columns.Contains("adjustment_no"))
                        dgvHeader.CurrentCell = r.Cells["adjustment_no"];
                    else
                        dgvHeader.CurrentCell = r.Cells[0];
                    return;
                }
            }
        }

        private void EnsureTables()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS inventory_adjustments (
    id BIGSERIAL PRIMARY KEY,
    adjustment_no VARCHAR(30) NOT NULL UNIQUE,
    direction VARCHAR(10) NOT NULL,
    adjustment_date DATE NOT NULL,
    warehouse_id INT NULL REFERENCES warehouses(id) ON DELETE SET NULL,
    reason VARCHAR(100) NOT NULL,
    note TEXT NULL,
    created_by INT NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS inventory_adjustment_items (
    id BIGSERIAL PRIMARY KEY,
    adjustment_id BIGINT NOT NULL REFERENCES inventory_adjustments(id) ON DELETE CASCADE,
    item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE RESTRICT,
    qty DOUBLE PRECISION NOT NULL DEFAULT 0,
    buy_price NUMERIC(15,2) NULL,
    note TEXT NULL
);

CREATE INDEX IF NOT EXISTS idx_inventory_adjustments_dir_date ON inventory_adjustments(direction, adjustment_date);
CREATE INDEX IF NOT EXISTS idx_inventory_adjustment_items_adj ON inventory_adjustment_items(adjustment_id);
", con);
            cmd.ExecuteNonQuery();
        }
    }
}
