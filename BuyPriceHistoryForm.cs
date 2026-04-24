using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class BuyPriceHistoryForm : Form
    {
        private DataTable _dt;

        public BuyPriceHistoryForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            Load += BuyPriceHistoryForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadData();
            txtSearch.TextChanged += (s, e) => ApplyFilter(txtSearch.Text);
        }

        private void BuyPriceHistoryForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var da = new NpgsqlDataAdapter(@"
SELECT
    h.id,
    h.changed_at,
    i.barcode,
    i.name,
    h.old_price,
    h.new_price,
    h.source_type,
    h.source_id,
    COALESCE(u.username, '') AS changed_by
FROM item_buy_price_histories h
JOIN items i ON i.id = h.item_id
LEFT JOIN users u ON u.id = h.changed_by
ORDER BY h.id DESC
LIMIT 5000
", con);
            _dt = new DataTable();
            da.Fill(_dt);
            dgvList.DataSource = _dt;

            if (dgvList.Columns.Contains("id")) dgvList.Columns["id"].Visible = false;
            if (dgvList.Columns.Contains("changed_at"))
            {
                dgvList.Columns["changed_at"].HeaderText = "Tanggal";
                dgvList.Columns["changed_at"].Width = 160;
                dgvList.Columns["changed_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            if (dgvList.Columns.Contains("barcode"))
            {
                dgvList.Columns["barcode"].HeaderText = "Barcode";
                dgvList.Columns["barcode"].Width = 140;
            }
            if (dgvList.Columns.Contains("name"))
            {
                dgvList.Columns["name"].HeaderText = "Nama";
                dgvList.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvList.Columns["name"].MinimumWidth = 220;
            }
            if (dgvList.Columns.Contains("old_price"))
            {
                dgvList.Columns["old_price"].HeaderText = "Harga Lama";
                dgvList.Columns["old_price"].Width = 120;
                dgvList.Columns["old_price"].DefaultCellStyle.Format = "N0";
            }
            if (dgvList.Columns.Contains("new_price"))
            {
                dgvList.Columns["new_price"].HeaderText = "Harga Baru";
                dgvList.Columns["new_price"].Width = 120;
                dgvList.Columns["new_price"].DefaultCellStyle.Format = "N0";
            }
            if (dgvList.Columns.Contains("source_type"))
            {
                dgvList.Columns["source_type"].HeaderText = "Sumber";
                dgvList.Columns["source_type"].Width = 100;
            }
            if (dgvList.Columns.Contains("source_id"))
            {
                dgvList.Columns["source_id"].HeaderText = "Ref";
                dgvList.Columns["source_id"].Width = 80;
            }
            if (dgvList.Columns.Contains("changed_by"))
            {
                dgvList.Columns["changed_by"].HeaderText = "User";
                dgvList.Columns["changed_by"].Width = 120;
            }

            ApplyFilter(txtSearch.Text);
        }

        private void ApplyFilter(string keyword)
        {
            if (_dt == null) return;
            var dv = _dt.DefaultView;

            keyword = (keyword ?? "").Trim();
            if (keyword.Length == 0)
            {
                dv.RowFilter = "";
                return;
            }

            var k = keyword.Replace("'", "''");
            dv.RowFilter =
                $"Convert(barcode, 'System.String') LIKE '%{k}%'" +
                $" OR Convert(name, 'System.String') LIKE '%{k}%'" +
                $" OR Convert(source_type, 'System.String') LIKE '%{k}%'" +
                $" OR Convert(changed_by, 'System.String') LIKE '%{k}%'";
        }
    }
}
