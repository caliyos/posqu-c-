using POS_qu.Services;
using POS_qu.Repositories;
using POS_qu.Core;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public class TransactionsListForm : Form
    {
        private DataGridView grid;
        private Button btnRefresh;
        private Button btnDetail;
        private Button btnExport;
        private Button btnExportDetail;
        private Button btnCancel;
        private Button btnReturn;
        private ComboBox cbStatus;
        private Label lblTitle;

        private TransactionRepo _repo;
        private TransactionService _service;

        public TransactionsListForm()
        {
            Text = "Daftar Transaksi (Sukses)";
            Size = new Size(1100, 650);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Padding = new Padding(10);

            _repo = new TransactionRepo();
            _service = new TransactionService(_repo, new ActivityService());

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 11);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
            grid.RowTemplate.Height = 36;
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 90 };
            lblTitle = new Label { Text = "Daftar Transaksi", Left = 0, Top = 6, AutoSize = true, Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold) };
            cbStatus = new ComboBox { Left = 0, Top = 44, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cbStatus.Items.AddRange(new object[] { "Semua", "Sukses", "Retur", "Dibatalkan" });
            cbStatus.SelectedIndex = 1;
            btnRefresh = new Button { Text = "Refresh", Width = 100, Left = 180, Top = 42 };
            btnDetail = new Button { Text = "Lihat Detail", Width = 120, Left = 290, Top = 42 };
            btnExport = new Button { Text = "Export Excel", Width = 120, Left = 420, Top = 42 };
            btnExportDetail = new Button { Text = "Export + Detail", Width = 140, Left = 550, Top = 42 };
            btnCancel = new Button { Text = "Batal Transaksi", Width = 150, Left = 700, Top = 40, BackColor = Color.IndianRed, ForeColor = Color.White };
            btnReturn = new Button { Text = "Retur Barang", Width = 120, Left = 860, Top = 40, BackColor = Color.DarkOrange, ForeColor = Color.White };
            btnRefresh.Click += (s, e) => LoadData();
            btnDetail.Click += BtnDetail_Click;
            btnExport.Click += BtnExport_Click;
            btnExportDetail.Click += BtnExportDetail_Click;
            btnCancel.Click += BtnCancel_Click;
            cbStatus.SelectedIndexChanged += (s, e) => LoadData();
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(cbStatus);
            panelTop.Controls.Add(btnRefresh);
            panelTop.Controls.Add(btnDetail);
            panelTop.Controls.Add(btnExport);
            panelTop.Controls.Add(btnExportDetail);
            panelTop.Controls.Add(btnCancel);
            panelTop.Controls.Add(btnReturn);
            btnReturn.Click += BtnReturn_Click;

            Controls.Add(grid);
            Controls.Add(panelTop);

            Load += (_, __) => LoadData();
        }

        private void LoadData()
        {
            string filter = "Sukses";
            if (cbStatus != null && cbStatus.SelectedItem != null)
                filter = cbStatus.SelectedItem.ToString();
            DataTable dt = _repo.GetTransactionsByFilter(filter);
            grid.DataSource = dt;
            if (grid.Columns["ts_id"] != null) grid.Columns["ts_id"].HeaderText = "ID";
            if (grid.Columns["ts_numbering"] != null) grid.Columns["ts_numbering"].HeaderText = "No Transaksi";
            if (grid.Columns["ts_grand_total"] != null)
            {
                grid.Columns["ts_grand_total"].HeaderText = "Grand Total";
                grid.Columns["ts_grand_total"].DefaultCellStyle.Format = "N0";
                grid.Columns["ts_grand_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (grid.Columns["ts_method"] != null) grid.Columns["ts_method"].HeaderText = "Metode";
            if (grid.Columns["ts_status"] != null) grid.Columns["ts_status"].HeaderText = "Status";
            if (grid.Columns["created_at"] != null) grid.Columns["created_at"].HeaderText = "Tanggal";
            if (grid.Columns["user_id"] != null) grid.Columns["user_id"].HeaderText = "User";
            if (grid.Columns.Contains("deleted_at")) grid.Columns["deleted_at"].HeaderText = "Dibatalkan";
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih transaksi dulu.");
                return;
            }
            int tsId = Convert.ToInt32(grid.SelectedRows[0].Cells["ts_id"].Value);
            var confirm = MessageBox.Show("Batalkan transaksi ini? Stok akan dikembalikan.", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var result = _service.CancelTransaction(tsId);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message);
                return;
            }
            MessageBox.Show("Transaksi dibatalkan dan stok dikembalikan.");
            LoadData();
        }

        private void BtnDetail_Click(object? sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih transaksi dulu.");
                return;
            }
            int tsId = Convert.ToInt32(grid.SelectedRows[0].Cells["ts_id"].Value);
            var dt = _repo.GetTransactionDetailsById(tsId);
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Detail tidak ditemukan.");
                return;
            }
            var friendly = new DataTable();
            friendly.Columns.Add("Nama Barang");
            friendly.Columns.Add("Barcode");
            friendly.Columns.Add("Jumlah");
            friendly.Columns.Add("Satuan");
            friendly.Columns.Add("Harga Satuan");
            friendly.Columns.Add("Subtotal");
            friendly.Columns.Add("Diskon");
            friendly.Columns.Add("Pajak");
            foreach (DataRow r in dt.Rows)
            {
                var name = r["name"]?.ToString() ?? "";
                var barcode = r["barcode"]?.ToString() ?? "";
                var qty = Convert.ToDecimal(r["tsd_quantity"]);
                var unit = r["tsd_unit"]?.ToString() ?? "";
                var price = Convert.ToDecimal(r["tsd_sell_price"]);
                var total = r.Table.Columns.Contains("tsd_total") ? Convert.ToDecimal(r["tsd_total"]) : (price * qty);
                var disc = r.Table.Columns.Contains("tsd_discount_total") ? Convert.ToDecimal(r["tsd_discount_total"]) : 0m;
                var tax = r.Table.Columns.Contains("tsd_tax") ? Convert.ToDecimal(r["tsd_tax"]) : 0m;
                friendly.Rows.Add(name, barcode, qty.ToString("N0"), unit, price.ToString("N0"), total.ToString("N0"), disc.ToString("N0"), tax.ToString("N0"));
            }

            using var modal = new Form();
            modal.Text = "Detail Transaksi";
            modal.Size = new Size(900, 500);
            modal.StartPosition = FormStartPosition.CenterParent;
            var dg = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DataSource = friendly
            };
            var top = new Panel { Dock = DockStyle.Top, Height = 46 };
            var btnExportDetail = new Button { Text = "Export Excel", Width = 120, Left = 10, Top = 6 };
            btnExportDetail.Click += (s, ev) => ExportDataTableToCsv(friendly);
            top.Controls.Add(btnExportDetail);
            modal.Controls.Add(dg);
            modal.Controls.Add(top);
            modal.ShowDialog(this);
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            if (grid.DataSource is DataTable dt)
            {
                ExportDataTableToCsv(dt);
            }
            else
            {
                MessageBox.Show("Tidak ada data untuk diexport.");
            }
        }

        private void BtnExportDetail_Click(object? sender, EventArgs e)
        {
            if (!(grid.DataSource is DataTable dt) || dt.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk diexport.");
                return;
            }
            using var sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            sfd.FileName = "transaksi_dengan_detail.csv";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;
            using var w = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
            // header friendly
            w.WriteLine("No Transaksi,Tanggal,User,Metode,Status,Nama Barang,Barcode,Jumlah,Satuan,Harga Satuan,Subtotal,Diskon,Pajak");
            foreach (DataRow tr in dt.Rows)
            {
                int tsId = Convert.ToInt32(tr["ts_id"]);
                string tsNum = tr["ts_numbering"]?.ToString() ?? "";
                string createdAt = tr["created_at"]?.ToString() ?? "";
                string userId = tr["user_id"]?.ToString() ?? "";
                string method = tr.Table.Columns.Contains("ts_method") ? (tr["ts_method"]?.ToString() ?? "") : "";
                string status = tr.Table.Columns.Contains("ts_status") ? (tr["ts_status"]?.ToString() ?? "") : "1";
                var details = _repo.GetTransactionDetailsById(tsId);
                if (details.Rows.Count == 0)
                {
                    // tetap tulis header transaksi, detail kosong
                    w.WriteLine($"{Csv(tsNum)},{Csv(createdAt)},{Csv(userId)},{Csv(method)},{Csv(status)},,,,,,,");
                    continue;
                }
                foreach (DataRow d in details.Rows)
                {
                    string name = d["name"]?.ToString() ?? "";
                    string barcode = d["barcode"]?.ToString() ?? "";
                    string qty = ToRaw(d, "tsd_quantity");
                    string unit = d["tsd_unit"]?.ToString() ?? "";
                    string price = ToRaw(d, "tsd_sell_price");
                    string total = details.Columns.Contains("tsd_total") ? ToRaw(d, "tsd_total") : "";
                    string disc = details.Columns.Contains("tsd_discount_total") ? ToRaw(d, "tsd_discount_total") : "0";
                    string tax = details.Columns.Contains("tsd_tax") ? ToRaw(d, "tsd_tax") : "0";
                    w.WriteLine($"{Csv(tsNum)},{Csv(createdAt)},{Csv(userId)},{Csv(method)},{Csv(status)},{Csv(name)},{Csv(barcode)},{qty},{Csv(unit)},{price},{total},{disc},{tax}");
                }
            }
            w.Flush();
            MessageBox.Show("Export dengan detail selesai.");
        }

        private static string Csv(string s)
        {
            s ??= "";
            if (s.Contains(",") || s.Contains("\""))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
        private static string ToRaw(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return "0";
            // tulis raw tanpa format ribuan agar Excel mengenali numerik
            return Convert.ToDecimal(r[col]).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        private void ExportDataTableToCsv(DataTable dt)
        {
            using var sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            sfd.FileName = "export.csv";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;
            using var w = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                w.Write(dt.Columns[i].ColumnName);
                if (i < dt.Columns.Count - 1) w.Write(",");
            }
            w.WriteLine();
            foreach (DataRow r in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var val = r[i]?.ToString() ?? "";
                    if (val.Contains(",") || val.Contains("\""))
                        val = "\"" + val.Replace("\"", "\"\"") + "\"";
                    w.Write(val);
                    if (i < dt.Columns.Count - 1) w.Write(",");
                }
                w.WriteLine();
            }
            w.Flush();
            MessageBox.Show("Export selesai.");
        }

        private void BtnReturn_Click(object? sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih transaksi dulu.");
                return;
            }
            if (!grid.Columns.Contains("ts_numbering"))
            {
                MessageBox.Show("Nomor transaksi tidak tersedia.");
                return;
            }
            string tsNumber = grid.SelectedRows[0].Cells["ts_numbering"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(tsNumber))
            {
                MessageBox.Show("Nomor transaksi tidak valid.");
                return;
            }
            using var frm = new ReturnForm();
            frm.PreloadTransaction(tsNumber, autoLoad: true);
            frm.ShowDialog(this);
        }
    }
}
