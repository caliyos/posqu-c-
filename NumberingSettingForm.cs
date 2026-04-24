using POS_qu.Services;
using System;
using System.Data;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class NumberingSettingForm : Form
    {
        private DataTable _dt;
        private readonly DocumentNumberingService _svc = new DocumentNumberingService();

        public NumberingSettingForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            Load += NumberingSettingForm_Load;
            btnClose.Click += (s, e) => Close();
            btnRefresh.Click += (s, e) => LoadData();
            dgvNumbering.CellEndEdit += dgvNumbering_CellEndEdit;
        }

        private void NumberingSettingForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _dt = _svc.GetAll();
            dgvNumbering.DataSource = _dt;

            if (dgvNumbering.Columns.Contains("doc_type"))
            {
                dgvNumbering.Columns["doc_type"].HeaderText = "DocType";
                dgvNumbering.Columns["doc_type"].ReadOnly = true;
                dgvNumbering.Columns["doc_type"].Width = 150;
            }
            if (dgvNumbering.Columns.Contains("prefix"))
            {
                dgvNumbering.Columns["prefix"].HeaderText = "Prefix";
                dgvNumbering.Columns["prefix"].Width = 120;
            }
            if (dgvNumbering.Columns.Contains("pad_length"))
            {
                dgvNumbering.Columns["pad_length"].HeaderText = "Pad";
                dgvNumbering.Columns["pad_length"].Width = 80;
            }
            if (dgvNumbering.Columns.Contains("format"))
            {
                dgvNumbering.Columns["format"].HeaderText = "Format";
                dgvNumbering.Columns["format"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvNumbering.Columns["format"].MinimumWidth = 280;
            }
            if (dgvNumbering.Columns.Contains("last_date"))
            {
                dgvNumbering.Columns["last_date"].HeaderText = "Last Date";
                dgvNumbering.Columns["last_date"].Width = 120;
            }
            if (dgvNumbering.Columns.Contains("last_number"))
            {
                dgvNumbering.Columns["last_number"].HeaderText = "Last No";
                dgvNumbering.Columns["last_number"].Width = 100;
            }
            if (dgvNumbering.Columns.Contains("updated_at"))
            {
                dgvNumbering.Columns["updated_at"].HeaderText = "Updated";
                dgvNumbering.Columns["updated_at"].Width = 160;
                dgvNumbering.Columns["updated_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvNumbering.Columns["updated_at"].ReadOnly = true;
            }
        }

        private void dgvNumbering_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvNumbering.Rows[e.RowIndex];
            if (row == null || row.IsNewRow) return;

            var docType = row.Cells["doc_type"]?.Value?.ToString() ?? "";
            var prefix = row.Cells["prefix"]?.Value?.ToString() ?? "";
            var fmt = row.Cells["format"]?.Value?.ToString() ?? "{prefix}-{yyyyMMdd}-{seq}";
            int pad = 4;
            int.TryParse(row.Cells["pad_length"]?.Value?.ToString(), out pad);
            if (pad <= 0) pad = 4;

            if (string.IsNullOrWhiteSpace(docType)) return;
            if (string.IsNullOrWhiteSpace(prefix)) prefix = docType;

            _svc.UpdateConfig(docType, prefix, pad, fmt);
            LoadData();
        }
    }
}

