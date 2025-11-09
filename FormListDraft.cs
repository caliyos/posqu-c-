using Npgsql;
using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class FormListDraft : Form
    {
        private readonly ItemController itemController = new ItemController();
        private DataTable _dtDrafts;
        private HashSet<int> checkedIds = new HashSet<int>();

        public string ActionType { get; private set; } // "load" atau "delete"

        public FormListDraft()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            btnLoadDraft.Click += btnLoadDraft_Click;
            btnDeleteDraft.Click += btnDeleteDraft_Click;
            btnRefresh.Click += (s, e) => LoadDraftList();
            btnClose.Click += (s, e) => Close();

            LoadDraftList();
        }

        private void LoadDraftList()
        {
            try
            {
                // Simpan ID yang sedang dicentang sebelum reload
                checkedIds = dataGridViewDrafts?.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells["chkSelect"] != null &&
                                Convert.ToBoolean(r.Cells["chkSelect"].Value ?? false))
                    .Select(r => Convert.ToInt32(r.Cells["po_id"].Value))
                    .ToHashSet() ?? new HashSet<int>();

                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                string query = @"
                    SELECT po_id, customer_name, total, global_discount, status, created_at, expired_at
                    FROM pending_orders
                    WHERE status = 'draft'
                    ORDER BY created_at DESC;";

                using var da = new NpgsqlDataAdapter(query, conn);
                _dtDrafts = new DataTable();
                da.Fill(_dtDrafts);

                dataGridViewDrafts.DataSource = _dtDrafts;
                dataGridViewDrafts.AllowUserToAddRows = false;

                // Tambahkan kolom checkbox jika belum ada
                if (!dataGridViewDrafts.Columns.Contains("chkSelect"))
                {
                    var chk = new DataGridViewCheckBoxColumn
                    {
                        HeaderText = "",
                        Name = "chkSelect",
                        Width = 40,
                        ReadOnly = false
                    };
                    dataGridViewDrafts.Columns.Insert(0, chk);
                }

                // Header kolom
                dataGridViewDrafts.Columns["po_id"].HeaderText = "ID";
                dataGridViewDrafts.Columns["customer_name"].HeaderText = "Customer";
                dataGridViewDrafts.Columns["total"].HeaderText = "Total";
                dataGridViewDrafts.Columns["global_discount"].HeaderText = "Diskon";
                dataGridViewDrafts.Columns["status"].HeaderText = "Status";
                dataGridViewDrafts.Columns["created_at"].HeaderText = "Tanggal";
                dataGridViewDrafts.Columns["expired_at"].HeaderText = "Kadaluarsa";

                // Readonly setting
                dataGridViewDrafts.ReadOnly = false;
                dataGridViewDrafts.Columns["chkSelect"].ReadOnly = false;
                foreach (DataGridViewColumn col in dataGridViewDrafts.Columns)
                {
                    if (col.Name != "chkSelect")
                        col.ReadOnly = true;
                }

                dataGridViewDrafts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewDrafts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewDrafts.MultiSelect = false;
                dataGridViewDrafts.RowHeadersVisible = false;

                // Restore checkbox yang sebelumnya
                foreach (DataGridViewRow row in dataGridViewDrafts.Rows)
                {
                    int id = Convert.ToInt32(row.Cells["po_id"].Value);
                    if (checkedIds.Contains(id))
                        row.Cells["chkSelect"].Value = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat daftar draft: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadDraft_Click(object sender, EventArgs e)
        {
            if (dataGridViewDrafts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih salah satu draft terlebih dahulu.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int poId = Convert.ToInt32(dataGridViewDrafts.SelectedRows[0].Cells["po_id"].Value);
            this.Tag = poId;
            ActionType = "load";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnDeleteDraft_Click(object sender, EventArgs e)
        {
            var selectedDrafts = dataGridViewDrafts.Rows
                .Cast<DataGridViewRow>()
                .Where(r => Convert.ToBoolean(r.Cells["chkSelect"].Value ?? false))
                .Select(r => Convert.ToInt32(r.Cells["po_id"].Value))
                .ToList();

            if (selectedDrafts.Count == 0)
            {
                MessageBox.Show("Pilih draft yang ingin dibatalkan (centang checkbox).", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Yakin ingin membatalkan {selectedDrafts.Count} draft terpilih?",
                "Konfirmasi Pembatalan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            // Simpan data ke properti agar bisa diproses di form utama
            int poId = Convert.ToInt32(dataGridViewDrafts.SelectedRows[0].Cells["po_id"].Value);
            this.Tag = poId;
            //this.Tag = selectedDrafts;
            ActionType = "delete";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
