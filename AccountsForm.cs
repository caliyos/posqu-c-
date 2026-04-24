using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class AccountsForm : Form
    {
        private DataTable _dt;
        private int _selectedId;

        public AccountsForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            Load += AccountsForm_Load;
            btnClose.Click += (s, e) => Close();
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            dgvAccounts.CellClick += dgvAccounts_CellClick;
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        private void AccountsForm_Load(object sender, EventArgs e)
        {
            cmbType.Items.Clear();
            cmbType.Items.AddRange(new object[] { "asset", "revenue", "expense" });
            if (cmbType.Items.Count > 0) cmbType.SelectedIndex = 0;

            EnsureTables();
            SeedDefaultAccounts();
            LoadData();
            ClearForm();
        }

        private void EnsureTables()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS accounts (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    type VARCHAR(20) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
    CONSTRAINT ck_accounts_type CHECK (type IN ('asset','revenue','expense'))
);
", con);
            cmd.ExecuteNonQuery();
        }

        private void SeedDefaultAccounts()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
INSERT INTO accounts (name, type)
VALUES
  ('Kas', 'asset'),
  ('Persediaan', 'asset'),
  ('Penjualan', 'revenue'),
  ('HPP', 'expense'),
  ('Diskon', 'expense'),
  ('Kerugian Stok', 'expense')
ON CONFLICT (name)
DO UPDATE SET type = EXCLUDED.type, updated_at = NOW();
", con);
            cmd.ExecuteNonQuery();
        }

        private void LoadData()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var da = new NpgsqlDataAdapter(@"
SELECT id, name, type, created_at
FROM accounts
ORDER BY id ASC
", con);
            _dt = new DataTable();
            da.Fill(_dt);

            dgvAccounts.DataSource = _dt;
            if (dgvAccounts.Columns.Contains("id")) dgvAccounts.Columns["id"].Visible = false;

            if (dgvAccounts.Columns.Contains("name"))
            {
                dgvAccounts.Columns["name"].HeaderText = "Nama";
                dgvAccounts.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAccounts.Columns["name"].MinimumWidth = 180;
            }

            if (dgvAccounts.Columns.Contains("type"))
            {
                dgvAccounts.Columns["type"].HeaderText = "Type";
                dgvAccounts.Columns["type"].Width = 120;
            }

            if (dgvAccounts.Columns.Contains("created_at"))
            {
                dgvAccounts.Columns["created_at"].HeaderText = "Dibuat";
                dgvAccounts.Columns["created_at"].Width = 160;
                dgvAccounts.Columns["created_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            ApplyFilter(txtSearch.Text);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
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
                $" OR Convert(type, 'System.String') LIKE '%{k}%'";
        }

        private void dgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvAccounts.Rows[e.RowIndex];
            if (row == null || row.IsNewRow) return;

            _selectedId = Convert.ToInt32(row.Cells["id"].Value);
            txtName.Text = row.Cells["name"].Value?.ToString() ?? "";
            var t = row.Cells["type"].Value?.ToString() ?? "asset";
            var idx = cmbType.Items.Cast<object>().Select(x => x.ToString()).ToList().FindIndex(x => string.Equals(x, t, StringComparison.OrdinalIgnoreCase));
            cmbType.SelectedIndex = idx >= 0 ? idx : 0;

            btnAdd.Enabled = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var name = (txtName.Text ?? "").Trim();
            var type = (cmbType.SelectedItem?.ToString() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Nama akun wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!IsValidType(type))
            {
                MessageBox.Show("Type tidak valid.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
INSERT INTO accounts (name, type, created_at, updated_at)
VALUES (@name, @type, NOW(), NOW())
", con);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);

            try
            {
                cmd.ExecuteNonQuery();
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal tambah akun: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Pilih akun dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var name = (txtName.Text ?? "").Trim();
            var type = (cmbType.SelectedItem?.ToString() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Nama akun wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!IsValidType(type))
            {
                MessageBox.Show("Type tidak valid.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
UPDATE accounts
SET name = @name,
    type = @type,
    updated_at = NOW()
WHERE id = @id
", con);
            cmd.Parameters.AddWithValue("@id", _selectedId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);

            try
            {
                cmd.ExecuteNonQuery();
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal update akun: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Pilih akun dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Hapus akun ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM accounts WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@id", _selectedId);

            try
            {
                cmd.ExecuteNonQuery();
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal hapus akun: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedId = 0;
            txtName.Text = "";
            if (cmbType.Items.Count > 0) cmbType.SelectedIndex = 0;
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private bool IsValidType(string type)
        {
            return type == "asset" || type == "revenue" || type == "expense";
        }
    }
}

