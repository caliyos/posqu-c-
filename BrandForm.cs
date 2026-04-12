using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;
using System.Data;
using POS_qu.Helpers;

namespace POS_qu
{
    public partial class BrandForm : Form
    {
        public BrandForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                string sql = "SELECT id, name FROM brands ORDER BY id ASC";
                using var adapter = new NpgsqlDataAdapter(sql, conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                
                dgvBrands.DataSource = dt;
                
                if (dgvBrands.Columns.Contains("id")) 
                {
                    dgvBrands.Columns["id"].HeaderText = "ID";
                    dgvBrands.Columns["id"].ReadOnly = true;
                    dgvBrands.Columns["id"].Width = 80;
                }
                
                if (dgvBrands.Columns.Contains("name"))
                {
                    dgvBrands.Columns["name"].HeaderText = "Nama Merk";
                    dgvBrands.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void dgvBrands_RowValidated(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            
            var row = dgvBrands.Rows[e.RowIndex];
            if (row.IsNewRow) return; // Skip if it's the empty new row placeholder

            string name = row.Cells["name"].Value?.ToString()?.Trim() ?? "";
            string idStr = row.Cells["id"].Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(name)) return;

            try
            {
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                if (string.IsNullOrEmpty(idStr))
                {
                    // INSERT new
                    string sql = "INSERT INTO brands (name) VALUES (@name) ON CONFLICT (name) DO NOTHING RETURNING id";
                    using var cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    var newId = cmd.ExecuteScalar();
                    if (newId != null)
                    {
                        row.Cells["id"].Value = newId;
                    }
                }
                else
                {
                    // UPDATE existing
                    long id = Convert.ToInt64(idStr);
                    string sql = "UPDATE brands SET name = @name, updated_at = NOW() WHERE id = @id";
                    using var cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message);
            }
        }

        private void dgvBrands_UserDeletingRow(object? sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row == null || e.Row.IsNewRow) return;

            var idStr = e.Row.Cells["id"].Value?.ToString();
            if (string.IsNullOrEmpty(idStr)) return;

            var confirm = MessageBox.Show("Apakah Anda yakin ingin menghapus merk ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                long id = Convert.ToInt64(idStr);
                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();
                string sql = "DELETE FROM brands WHERE id = @id";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting data: " + ex.Message);
                e.Cancel = true;
            }
        }
    }
}