using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace POS_qu
{
    public sealed class PurchaseReturnListForm : Form
    {
        private DataGridView dgvHeader;
        private DataGridView dgvDetail;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnRefresh;
        private Button btnClose;

        private DataTable _dtHeader;
        private DataTable _dtDetail;

        public PurchaseReturnListForm()
        {
            Text = "Daftar Retur Pembelian (Ke Supplier)";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(245, 246, 250);

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.White };
            var lblTitle = new Label
            {
                Text = "Daftar Retur Pembelian (Ke Supplier)",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Left = 20,
                Top = 18
            };

            txtSearch = new TextBox
            {
                Left = 20,
                Top = 44,
                Width = 380
            };

            btnAdd = new Button
            {
                Text = "Tambah Retur",
                Width = 150,
                Height = 40,
                Left = 420,
                Top = 16,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

            btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 120,
                Height = 40,
                Left = 580,
                Top = 16,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

            btnClose = new Button
            {
                Text = "Tutup",
                Width = 120,
                Height = 40,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(txtSearch);
            panelHeader.Controls.Add(btnAdd);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(btnClose);
            Controls.Add(panelHeader);

            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };
            Controls.Add(body);

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 380
            };
            body.Controls.Add(split);

            dgvHeader = BuildGrid();
            dgvDetail = BuildGrid();
            split.Panel1.Controls.Add(dgvHeader);

            var lblDetail = new Label
            {
                Text = "Detail Item",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Padding = new Padding(0, 10, 0, 10)
            };
            split.Panel2.Controls.Add(dgvDetail);
            split.Panel2.Controls.Add(lblDetail);

            Load += PurchaseReturnListForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadHeaderData();
            btnAdd.Click += btnAdd_Click;
            txtSearch.TextChanged += (s, e) => LoadHeaderData();
            dgvHeader.SelectionChanged += (s, e) => LoadDetailData(GetSelectedHeaderId());
        }

        private static DataGridView BuildGrid()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                RowTemplate = { Height = 42 }
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.RowsDefaultCellStyle.Padding = new Padding(5);
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);

            return dgv;
        }

        private void PurchaseReturnListForm_Load(object sender, EventArgs e)
        {
            LoadHeaderData();

            btnClose.Left = ClientSize.Width - btnClose.Width - 20;
            btnClose.Top = 16;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using var f = new PurchaseReturnEntryForm();
            if (f.ShowDialog(this) != DialogResult.OK) return;
            LoadHeaderData();
            SelectHeaderById(f.CreatedReturnId);
        }

        private void LoadHeaderData()
        {
            _dtHeader = new DataTable();
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string search = (txtSearch.Text ?? "").Trim();
            using var cmd = new NpgsqlCommand(@"
SELECT
    r.id,
    r.return_number,
    r.return_date,
    COALESCE(s.name, '') AS supplier_name,
    COALESCE(w.name, '') AS warehouse_name,
    COALESCE(r.note, '') AS note,
    COALESCE(u.username, '') AS created_by,
    r.created_at,
    COALESCE((SELECT SUM(it.qty) FROM purchase_return_items it WHERE it.purchase_return_id = r.id), 0) AS total_qty,
    COALESCE((SELECT COUNT(1) FROM purchase_return_items it WHERE it.purchase_return_id = r.id), 0) AS total_items
FROM purchase_returns r
LEFT JOIN suppliers s ON s.id = r.supplier_id
LEFT JOIN warehouses w ON w.id = r.warehouse_id
LEFT JOIN users u ON u.id = r.created_by
WHERE (@search = '' OR r.return_number ILIKE @search OR COALESCE(r.note,'') ILIKE @search)
ORDER BY r.id DESC
", con);
            cmd.Parameters.AddWithValue("@search", string.IsNullOrWhiteSpace(search) ? "" : "%" + search + "%");
            using var da = new NpgsqlDataAdapter(cmd);
            da.Fill(_dtHeader);

            dgvHeader.DataSource = _dtHeader;
            if (dgvHeader.Columns.Contains("id")) dgvHeader.Columns["id"].Visible = false;

            if (dgvHeader.Columns.Contains("return_number"))
            {
                dgvHeader.Columns["return_number"].HeaderText = "No Retur";
                dgvHeader.Columns["return_number"].Width = 170;
            }
            if (dgvHeader.Columns.Contains("return_date"))
            {
                dgvHeader.Columns["return_date"].HeaderText = "Tanggal";
                dgvHeader.Columns["return_date"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvHeader.Columns["return_date"].Width = 110;
            }
            if (dgvHeader.Columns.Contains("supplier_name"))
            {
                dgvHeader.Columns["supplier_name"].HeaderText = "Supplier";
                dgvHeader.Columns["supplier_name"].Width = 220;
            }
            if (dgvHeader.Columns.Contains("warehouse_name"))
            {
                dgvHeader.Columns["warehouse_name"].HeaderText = "Gudang";
                dgvHeader.Columns["warehouse_name"].Width = 170;
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
                dgvHeader.Columns["created_at"].Width = 160;
            }

            if (dgvHeader.Rows.Count > 0) dgvHeader.Rows[0].Selected = true;
            LoadDetailData(GetSelectedHeaderId());
        }

        private int GetSelectedHeaderId()
        {
            if (dgvHeader.SelectedRows.Count == 0) return 0;
            if (!dgvHeader.Columns.Contains("id")) return 0;
            return Convert.ToInt32(dgvHeader.SelectedRows[0].Cells["id"].Value);
        }

        private void LoadDetailData(int returnId)
        {
            _dtDetail = new DataTable();
            dgvDetail.DataSource = _dtDetail;
            if (returnId <= 0) return;

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
FROM purchase_return_items it
JOIN items i ON i.id = it.item_id
LEFT JOIN units u ON i.unit = u.id
WHERE it.purchase_return_id = @id
ORDER BY it.id ASC
", con);
            cmd.Parameters.AddWithValue("@id", returnId);
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
                dgvDetail.Columns["name"].MinimumWidth = 240;
            }

            if (dgvDetail.Columns.Contains("unit_name"))
            {
                dgvDetail.Columns["unit_name"].HeaderText = "Satuan";
                dgvDetail.Columns["unit_name"].Width = 110;
            }

            if (dgvDetail.Columns.Contains("qty"))
            {
                dgvDetail.Columns["qty"].HeaderText = "Qty";
                dgvDetail.Columns["qty"].Width = 90;
                dgvDetail.Columns["qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvDetail.Columns.Contains("buy_price"))
            {
                dgvDetail.Columns["buy_price"].HeaderText = "Harga Beli";
                dgvDetail.Columns["buy_price"].DefaultCellStyle.Format = "N0";
                dgvDetail.Columns["buy_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetail.Columns["buy_price"].Width = 120;
            }

            if (dgvDetail.Columns.Contains("note"))
            {
                dgvDetail.Columns["note"].HeaderText = "Catatan";
                dgvDetail.Columns["note"].Width = 220;
            }
        }

        private void SelectHeaderById(int id)
        {
            if (id <= 0) return;
            foreach (DataGridViewRow row in dgvHeader.Rows)
            {
                if (row.Cells["id"]?.Value == null) continue;
                if (Convert.ToInt32(row.Cells["id"].Value) == id)
                {
                    row.Selected = true;
                    dgvHeader.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index);
                    break;
                }
            }
        }
    }
}
