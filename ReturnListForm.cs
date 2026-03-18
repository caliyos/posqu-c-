using POS_qu.services;
using POS_qu.Core;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public class ReturnListForm : Form
    {
        private DataGridView grid;
        private Button btnRefresh;
        private Button btnDetail;
        private Button btnExport;
        private Button btnExportDetail;

        private TransactionRepo _repo;
        private TransactionService _service;

        public ReturnListForm()
        {
            Text = "Daftar Retur Barang";
            Size = new Size(920, 560);
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
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 46 };
            btnRefresh = new Button { Text = "Refresh", Width = 100, Left = 0, Top = 6 };
            btnDetail = new Button { Text = "Lihat Detail", Width = 120, Left = 110, Top = 6 };
            btnExport = new Button { Text = "Export Excel", Width = 120, Left = 240, Top = 6 };
            btnExportDetail = new Button { Text = "Export + Detail", Width = 140, Left = 370, Top = 6 };
            btnRefresh.Click += (s, e) => LoadData();
            btnDetail.Click += BtnDetail_Click;
            btnExport.Click += BtnExport_Click;
            btnExportDetail.Click += BtnExportDetail_Click;
            panelTop.Controls.Add(btnRefresh);
            panelTop.Controls.Add(btnDetail);
            panelTop.Controls.Add(btnExport);
            panelTop.Controls.Add(btnExportDetail);

            Controls.Add(grid);
            Controls.Add(panelTop);

            Load += (_, __) => LoadData();
        }

        private void LoadData()
        {
            DataTable dt = _repo.GetReturnTransactions();
            grid.DataSource = dt;
            if (grid.Columns["ts_id"] != null) grid.Columns["ts_id"].HeaderText = "ID";
            if (grid.Columns["ts_numbering"] != null) grid.Columns["ts_numbering"].HeaderText = "No Retur";
            if (grid.Columns["ts_grand_total"] != null)
            {
                grid.Columns["ts_grand_total"].HeaderText = "Total Retur";
                grid.Columns["ts_grand_total"].DefaultCellStyle.Format = "N0";
                grid.Columns["ts_grand_total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (grid.Columns["ts_method"] != null) grid.Columns["ts_method"].HeaderText = "Metode";
            if (grid.Columns["created_at"] != null) grid.Columns["created_at"].HeaderText = "Tanggal";
            if (grid.Columns["user_id"] != null) grid.Columns["user_id"].HeaderText = "User";
        }

        private void BtnDetail_Click(object? sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih transaksi retur dulu.");
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
            modal.Text = "Detail Retur";
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

        private void ExportDataTableToCsv(DataTable dt)
        {
            using var sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            sfd.FileName = "retur.csv";
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

        private void BtnExportDetail_Click(object? sender, EventArgs e)
        {
            if (!(grid.DataSource is DataTable dt) || dt.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk diexport.");
                return;
            }
            using var sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            sfd.FileName = "retur_dengan_detail.csv";
            if (sfd.ShowDialog(this) != DialogResult.OK) return;
            using var w = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
            w.WriteLine("No Retur,Tanggal,User,Metode,Status,Nama Barang,Barcode,Jumlah,Satuan,Harga Satuan,Subtotal,Diskon,Pajak");
            foreach (DataRow tr in dt.Rows)
            {
                int tsId = Convert.ToInt32(tr["ts_id"]);
                string tsNum = tr["ts_numbering"]?.ToString() ?? "";
                string createdAt = tr["created_at"]?.ToString() ?? "";
                string userId = tr["user_id"]?.ToString() ?? "";
                string method = tr.Table.Columns.Contains("ts_method") ? (tr["ts_method"]?.ToString() ?? "") : "RETURN";
                string status = tr.Table.Columns.Contains("ts_status") ? (tr["ts_status"]?.ToString() ?? "") : "1";
                var details = _repo.GetTransactionDetailsById(tsId);
                if (details.Rows.Count == 0)
                {
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
            return Convert.ToDecimal(r[col]).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
