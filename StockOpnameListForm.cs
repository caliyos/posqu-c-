using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using POS_qu.Helpers;

namespace POS_qu
{
    public partial class StockOpnameListForm : Form
    {
        private DataTable _dtOpnames;
        private DataTable _dtItems;

        public StockOpnameListForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            Load += StockOpnameListForm_Load;
            dgvOpnames.SelectionChanged += dgvOpnames_SelectionChanged;
        }

        private void StockOpnameListForm_Load(object sender, EventArgs e)
        {
            EnsureTables();
            ApplyGridStyle(dgvOpnames);
            ApplyGridStyle(dgvItems);
            LoadOpnames();
        }

        private void EnsureTables()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS stock_opnames (
  id BIGSERIAL PRIMARY KEY,
  opname_no VARCHAR(50) NOT NULL,
  opname_date DATE NOT NULL,
  warehouse_id INT NOT NULL REFERENCES warehouses(id),
  mode VARCHAR(20) NOT NULL,
  created_by INT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS stock_opname_items (
  id BIGSERIAL PRIMARY KEY,
  opname_id BIGINT NOT NULL REFERENCES stock_opnames(id) ON DELETE CASCADE,
  item_id BIGINT NOT NULL REFERENCES items(id),
  unit_id INT NOT NULL,
  conversion DOUBLE PRECISION NOT NULL DEFAULT 1,
  system_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
  physical_qty_input DOUBLE PRECISION NOT NULL DEFAULT 0,
  physical_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
  diff_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
  note TEXT
);
", con);
            cmd.ExecuteNonQuery();
        }

        private void LoadOpnames()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT so.id, so.opname_no, so.opname_date, w.name as warehouse, so.mode, so.created_at,
       (SELECT COUNT(*) FROM stock_opname_items soi WHERE soi.opname_id = so.id) as total_item
FROM stock_opnames so
LEFT JOIN warehouses w ON w.id = so.warehouse_id
ORDER BY so.created_at DESC
", con);
            using var dr = cmd.ExecuteReader();
            _dtOpnames = new DataTable();
            _dtOpnames.Load(dr);
            dgvOpnames.DataSource = _dtOpnames;

            if (dgvOpnames.Columns.Contains("id")) dgvOpnames.Columns["id"].Visible = false;
            if (dgvOpnames.Columns.Contains("opname_no")) dgvOpnames.Columns["opname_no"].HeaderText = "No";
            if (dgvOpnames.Columns.Contains("opname_date")) dgvOpnames.Columns["opname_date"].HeaderText = "Tanggal";
            if (dgvOpnames.Columns.Contains("warehouse")) dgvOpnames.Columns["warehouse"].HeaderText = "Gudang";
            if (dgvOpnames.Columns.Contains("mode")) dgvOpnames.Columns["mode"].HeaderText = "Mode";
            if (dgvOpnames.Columns.Contains("total_item")) dgvOpnames.Columns["total_item"].HeaderText = "Jumlah Item";
            if (dgvOpnames.Columns.Contains("created_at")) dgvOpnames.Columns["created_at"].HeaderText = "Dibuat";
        }

        private void dgvOpnames_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOpnames.CurrentRow == null) return;
            if (dgvOpnames.CurrentRow.Cells["id"]?.Value == null) return;
            long opnameId = Convert.ToInt64(dgvOpnames.CurrentRow.Cells["id"].Value);
            LoadItems(opnameId);
        }

        private void LoadItems(long opnameId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT soi.item_id, i.barcode, i.name, u.name as unit,
       soi.system_qty_base, soi.physical_qty_input, soi.physical_qty_base, soi.diff_qty_base, soi.note
FROM stock_opname_items soi
LEFT JOIN items i ON i.id = soi.item_id
LEFT JOIN units u ON u.id = soi.unit_id
WHERE soi.opname_id = @id
ORDER BY i.name
", con);
            cmd.Parameters.AddWithValue("@id", opnameId);
            using var dr = cmd.ExecuteReader();
            _dtItems = new DataTable();
            _dtItems.Load(dr);
            dgvItems.DataSource = _dtItems;

            if (dgvItems.Columns.Contains("item_id")) dgvItems.Columns["item_id"].Visible = false;
            if (dgvItems.Columns.Contains("barcode")) dgvItems.Columns["barcode"].HeaderText = "Barcode";
            if (dgvItems.Columns.Contains("name")) dgvItems.Columns["name"].HeaderText = "Nama";
            if (dgvItems.Columns.Contains("unit")) dgvItems.Columns["unit"].HeaderText = "Satuan Input";

            if (dgvItems.Columns.Contains("system_qty_base")) dgvItems.Columns["system_qty_base"].HeaderText = "Sistem (Base)";
            if (dgvItems.Columns.Contains("physical_qty_input")) dgvItems.Columns["physical_qty_input"].HeaderText = "Fisik (Input)";
            if (dgvItems.Columns.Contains("physical_qty_base")) dgvItems.Columns["physical_qty_base"].HeaderText = "Fisik (Base)";
            if (dgvItems.Columns.Contains("diff_qty_base")) dgvItems.Columns["diff_qty_base"].HeaderText = "Selisih (Base)";
            if (dgvItems.Columns.Contains("note")) dgvItems.Columns["note"].HeaderText = "Catatan";
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
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOpnames();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

