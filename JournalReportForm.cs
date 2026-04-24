using Npgsql;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class JournalReportForm : Form
    {
        private DataTable _dtEntries;

        public JournalReportForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Maximized;

            Load += JournalReportForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadEntries();
            dtFrom.ValueChanged += (s, e) => LoadEntries();
            dtTo.ValueChanged += (s, e) => LoadEntries();
            dgvEntries.SelectionChanged += (s, e) => LoadSelectedEntryDetails();
        }

        private void JournalReportForm_Load(object sender, EventArgs e)
        {
            ApplyGridStyle(dgvEntries);
            ApplyGridStyle(dgvDetails);

            dtTo.Value = DateTime.Today;
            dtFrom.Value = DateTime.Today.AddDays(-30);

            LoadEntries();
        }

        private void LoadEntries()
        {
            DateTime from = dtFrom.Value.Date;
            DateTime to = dtTo.Value.Date.AddDays(1).AddTicks(-1);

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using var cmd = new NpgsqlCommand(@"
SELECT
    je.id,
    je.date,
    je.reference_type,
    je.reference_id,
    je.description,
    COALESCE(SUM(jd.debit),0) AS total_debit,
    COALESCE(SUM(jd.credit),0) AS total_credit
FROM journal_entries je
JOIN journal_details jd ON jd.journal_entry_id = je.id
WHERE je.date >= @from AND je.date <= @to
GROUP BY je.id, je.date, je.reference_type, je.reference_id, je.description
ORDER BY je.date DESC, je.id DESC
", con);
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            using var da = new NpgsqlDataAdapter(cmd);
            _dtEntries = new DataTable();
            da.Fill(_dtEntries);

            dgvEntries.DataSource = _dtEntries;
            ConfigureEntriesGrid();

            decimal totalDebit = 0m;
            decimal totalCredit = 0m;
            foreach (DataRow r in _dtEntries.Rows)
            {
                if (r["total_debit"] != DBNull.Value) totalDebit += Convert.ToDecimal(r["total_debit"]);
                if (r["total_credit"] != DBNull.Value) totalCredit += Convert.ToDecimal(r["total_credit"]);
            }
            lblSummary.Text = $"Total Debit: {totalDebit:N0} | Kredit: {totalCredit:N0}";

            if (dgvEntries.Rows.Count > 0)
            {
                dgvEntries.Rows[0].Selected = true;
                LoadSelectedEntryDetails();
            }
            else
            {
                dgvDetails.DataSource = null;
            }
        }

        private void LoadSelectedEntryDetails()
        {
            if (dgvEntries.CurrentRow == null) return;
            if (!dgvEntries.Columns.Contains("id")) return;

            var idObj = dgvEntries.CurrentRow.Cells["id"].Value;
            if (idObj == null || idObj == DBNull.Value) return;
            long entryId = Convert.ToInt64(idObj);

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT
    a.name AS account,
    jd.debit,
    jd.credit
FROM journal_details jd
JOIN accounts a ON a.id = jd.account_id
WHERE jd.journal_entry_id = @id
ORDER BY jd.id ASC
", con);
            cmd.Parameters.AddWithValue("@id", entryId);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            dgvDetails.DataSource = dt;
            ConfigureDetailsGrid();
        }

        private void ConfigureEntriesGrid()
        {
            if (dgvEntries.Columns.Contains("id")) dgvEntries.Columns["id"].Visible = false;

            if (dgvEntries.Columns.Contains("date"))
            {
                dgvEntries.Columns["date"].HeaderText = "Tanggal";
                dgvEntries.Columns["date"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvEntries.Columns["date"].Width = 160;
            }

            if (dgvEntries.Columns.Contains("reference_type"))
            {
                dgvEntries.Columns["reference_type"].HeaderText = "Ref";
                dgvEntries.Columns["reference_type"].Width = 90;
            }

            if (dgvEntries.Columns.Contains("reference_id"))
            {
                dgvEntries.Columns["reference_id"].HeaderText = "Ref ID";
                dgvEntries.Columns["reference_id"].Width = 90;
            }

            if (dgvEntries.Columns.Contains("description"))
            {
                dgvEntries.Columns["description"].HeaderText = "Deskripsi";
                dgvEntries.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvEntries.Columns.Contains("total_debit"))
            {
                dgvEntries.Columns["total_debit"].HeaderText = "Debit";
                dgvEntries.Columns["total_debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvEntries.Columns["total_debit"].DefaultCellStyle.Format = "N0";
                dgvEntries.Columns["total_debit"].Width = 120;
            }

            if (dgvEntries.Columns.Contains("total_credit"))
            {
                dgvEntries.Columns["total_credit"].HeaderText = "Kredit";
                dgvEntries.Columns["total_credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvEntries.Columns["total_credit"].DefaultCellStyle.Format = "N0";
                dgvEntries.Columns["total_credit"].Width = 120;
            }
        }

        private void ConfigureDetailsGrid()
        {
            if (dgvDetails.Columns.Contains("account"))
            {
                dgvDetails.Columns["account"].HeaderText = "Akun";
                dgvDetails.Columns["account"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvDetails.Columns.Contains("debit"))
            {
                dgvDetails.Columns["debit"].HeaderText = "Debit";
                dgvDetails.Columns["debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetails.Columns["debit"].DefaultCellStyle.Format = "N0";
                dgvDetails.Columns["debit"].Width = 140;
            }
            if (dgvDetails.Columns.Contains("credit"))
            {
                dgvDetails.Columns["credit"].HeaderText = "Kredit";
                dgvDetails.Columns["credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetails.Columns["credit"].DefaultCellStyle.Format = "N0";
                dgvDetails.Columns["credit"].Width = 140;
            }
        }

        private static void ApplyGridStyle(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.RowsDefaultCellStyle.Padding = new Padding(5);
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgv.RowTemplate.Height = 40;
        }
    }
}

