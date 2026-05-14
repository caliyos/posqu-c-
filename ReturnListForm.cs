using POS_qu.Services;
using POS_qu.Repositories;
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
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor = Color.FromArgb(245, 246, 250);

            _repo = new TransactionRepo();
            _service = new TransactionService(_repo, new ActivityService());

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            grid.ColumnHeadersHeight = 45;
            grid.RowsDefaultCellStyle.BackColor = Color.White;
            grid.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10F);
            grid.RowsDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            grid.RowsDefaultCellStyle.Padding = new Padding(5);
            grid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            grid.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            grid.RowTemplate.Height = 45;

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 78, BackColor = Color.White };
            var lblTitle = new Label
            {
                Text = "Daftar Retur Barang",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Left = 20,
                Top = 18
            };

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 560,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 18, 16, 0),
                BackColor = Color.White
            };

            btnRefresh = BuildHeaderButton("Refresh", 110);
            btnDetail = BuildHeaderButton("Lihat Detail", 130);
            btnExport = BuildHeaderButton("Export CSV", 120);
            btnExportDetail = BuildHeaderButton("Export + Detail", 150);
            btnRefresh.Click += (s, e) => LoadData();
            btnDetail.Click += BtnDetail_Click;
            btnExport.Click += BtnExport_Click;
            btnExportDetail.Click += BtnExportDetail_Click;
            flow.Controls.Add(btnRefresh);
            flow.Controls.Add(btnDetail);
            flow.Controls.Add(btnExport);
            flow.Controls.Add(btnExportDetail);

            panelHeader.Controls.Add(flow);
            panelHeader.Controls.Add(lblTitle);

            var panelBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };
            panelBody.Controls.Add(grid);

            Controls.Add(panelBody);
            Controls.Add(panelHeader);

            Load += (_, __) => LoadData();
        }

        private static Button BuildHeaderButton(string text, int width)
        {
            var b = new Button
            {
                Text = text,
                Width = width,
                Height = 42,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Margin = new Padding(8, 0, 0, 0)
            };
            b.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            return b;
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
            if (grid.Columns["created_at"] != null)
            {
                grid.Columns["created_at"].HeaderText = "Tanggal";
                grid.Columns["created_at"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            if (grid.Columns["user_id"] != null) grid.Columns["user_id"].HeaderText = "Kasir";
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
            modal.Size = new Size(1100, 650);
            modal.StartPosition = FormStartPosition.CenterParent;
            modal.BackColor = Color.FromArgb(245, 246, 250);
            var dg = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DataSource = friendly,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false
            };
            dg.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dg.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dg.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dg.ColumnHeadersHeight = 45;
            dg.RowsDefaultCellStyle.BackColor = Color.White;
            dg.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dg.RowsDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dg.RowsDefaultCellStyle.Padding = new Padding(5);
            dg.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dg.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dg.RowTemplate.Height = 45;

            var top = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.White };
            var lbl = new Label
            {
                Text = "Detail Retur",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Left = 16,
                Top = 18
            };
            var btnExportDetail = BuildHeaderButton("Export CSV", 130);
            btnExportDetail.Left = top.Width - btnExportDetail.Width - 16;
            btnExportDetail.Top = 14;
            btnExportDetail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportDetail.Click += (s, ev) => ExportDataTableToCsv(friendly);
            top.Controls.Add(btnExportDetail);
            top.Controls.Add(lbl);

            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };
            body.Controls.Add(dg);
            modal.Controls.Add(body);
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
