using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class PromotionListForm : Form
    {
        private DataTable _dt;

        public PromotionListForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            Load += PromotionListForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadData();
            btnNew.Click += (s, e) =>
            {
                using var f = new PromotionProgramForm();
                if (f.ShowDialog(this) == DialogResult.OK) LoadData();
            };
            txtSearch.TextChanged += (s, e) => ApplyFilter(txtSearch.Text);
            dgvList.CellDoubleClick += dgvList_CellDoubleClick;
        }

        private void PromotionListForm_Load(object sender, EventArgs e)
        {
            EnsurePromotionTables();
            LoadData();
        }

        private void EnsurePromotionTables()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS promotions (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    promo_type VARCHAR(20) NOT NULL, -- DISKON / PROMO / CASHBACK
    status VARCHAR(20) NOT NULL DEFAULT 'aktif', -- aktif / nonaktif
    start_date DATE NULL,
    end_date DATE NULL,
    priority INT NOT NULL DEFAULT 0,
    config_json TEXT NOT NULL DEFAULT '{}',
    created_by INT NULL REFERENCES users(id) ON DELETE SET NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_promotions_type_status ON promotions(promo_type, status);
CREATE INDEX IF NOT EXISTS idx_promotions_period ON promotions(start_date, end_date);
CREATE INDEX IF NOT EXISTS idx_promotions_priority ON promotions(priority);
", con);
            cmd.ExecuteNonQuery();
        }

        private void LoadData()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var da = new NpgsqlDataAdapter(@"
SELECT
    id,
    name,
    promo_type,
    status,
    start_date,
    end_date,
    priority,
    updated_at
FROM promotions
ORDER BY priority DESC, id DESC
", con);
            _dt = new DataTable();
            da.Fill(_dt);
            dgvList.DataSource = _dt;

            if (dgvList.Columns.Contains("id")) dgvList.Columns["id"].Visible = false;
            if (dgvList.Columns.Contains("name")) dgvList.Columns["name"].HeaderText = "Nama Program";
            if (dgvList.Columns.Contains("promo_type")) dgvList.Columns["promo_type"].HeaderText = "Tipe";
            if (dgvList.Columns.Contains("status")) dgvList.Columns["status"].HeaderText = "Status";
            if (dgvList.Columns.Contains("start_date")) dgvList.Columns["start_date"].HeaderText = "Mulai";
            if (dgvList.Columns.Contains("end_date")) dgvList.Columns["end_date"].HeaderText = "Selesai";
            if (dgvList.Columns.Contains("priority")) dgvList.Columns["priority"].HeaderText = "Priority";
            if (dgvList.Columns.Contains("updated_at"))
            {
                dgvList.Columns["updated_at"].HeaderText = "Update";
                dgvList.Columns["updated_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvList.Columns["updated_at"].Width = 160;
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
                $"Convert(name, 'System.String') LIKE '%{k}%'" +
                $" OR Convert(promo_type, 'System.String') LIKE '%{k}%'" +
                $" OR Convert(status, 'System.String') LIKE '%{k}%'";
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvList.Rows[e.RowIndex];
            if (row == null || row.IsNewRow) return;
            var idObj = row.Cells["id"]?.Value;
            if (idObj == null || idObj == DBNull.Value) return;

            int id = Convert.ToInt32(idObj);
            using var f = new PromotionProgramForm(id);
            if (f.ShowDialog(this) == DialogResult.OK) LoadData();
        }
    }
}

