using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class TaxTypeForm : Form
    {
        private DataTable _dt;

        public TaxTypeForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            Load += TaxTypeForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadData();
            dgvTaxTypes.CellEndEdit += dgvTaxTypes_CellEndEdit;
        }

        private void TaxTypeForm_Load(object sender, EventArgs e)
        {
            Seed();
            LoadData();
        }

        private void Seed()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
INSERT INTO tax_types (code, name, is_active)
VALUES
  ('NON', 'Non PPN', TRUE),
  ('INCLUDE', 'PPN Include', TRUE),
  ('EXCLUDE', 'PPN Exclude', TRUE)
ON CONFLICT (code)
DO UPDATE SET name = EXCLUDED.name, updated_at = NOW();
", con);
            cmd.ExecuteNonQuery();
        }

        private void LoadData()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var da = new NpgsqlDataAdapter(@"
SELECT id, code, name, is_active
FROM tax_types
ORDER BY id ASC
", con);
            _dt = new DataTable();
            da.Fill(_dt);
            dgvTaxTypes.DataSource = _dt;

            if (dgvTaxTypes.Columns.Contains("id")) dgvTaxTypes.Columns["id"].Visible = false;
            if (dgvTaxTypes.Columns.Contains("code"))
            {
                dgvTaxTypes.Columns["code"].HeaderText = "Kode";
                dgvTaxTypes.Columns["code"].ReadOnly = true;
                dgvTaxTypes.Columns["code"].Width = 120;
            }
            if (dgvTaxTypes.Columns.Contains("name"))
            {
                dgvTaxTypes.Columns["name"].HeaderText = "Nama";
                dgvTaxTypes.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvTaxTypes.Columns.Contains("is_active"))
            {
                dgvTaxTypes.Columns["is_active"].HeaderText = "Aktif";
                dgvTaxTypes.Columns["is_active"].Width = 80;
            }
        }

        private void dgvTaxTypes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvTaxTypes.Rows[e.RowIndex];
            if (row == null || row.IsNewRow) return;

            if (row.Cells["id"]?.Value == null || row.Cells["id"].Value == DBNull.Value) return;
            var id = Convert.ToInt64(row.Cells["id"].Value);
            var name = row.Cells["name"]?.Value?.ToString() ?? "";
            bool isActive = row.Cells["is_active"]?.Value != DBNull.Value && Convert.ToBoolean(row.Cells["is_active"].Value);

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
UPDATE tax_types
SET name = @n, is_active = @a, updated_at = NOW()
WHERE id = @id
", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@n", name);
            cmd.Parameters.AddWithValue("@a", isActive);
            cmd.ExecuteNonQuery();
        }
    }
}
