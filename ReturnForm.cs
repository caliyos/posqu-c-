using POS_qu.Helpers;
using POS_qu.Core;
using POS_qu.Models;
using POS_qu.Services;
using POS_qu.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_qu
{
    public class ReturnForm : Form
    {
        private DataGridView grid;
        private TextBox txtName;
        private TextBox txtQty;
        private TextBox txtPrice;
        private TextBox txtUnit;
        private TextBox txtBarcode;
        private TextBox txtTxnNumber;
        private Button btnLoadTxn;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnSave;
        private Label lblTotal;
        private Panel pnlTop;
        private FlowLayoutPanel pnlManual;
        private Panel pnlBottom;
        private Label lblHint;
        private bool _isTransactionMode = false;

        private TransactionService _txService;
        private TransactionRepo _txRepo;

        public ReturnForm()
        {
            Text = "Retur Barang";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor = Color.FromArgb(245, 246, 250);

            _txRepo = new TransactionRepo();
            _txService = new TransactionService(_txRepo, new ActivityService());

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 78, BackColor = Color.White };
            var lblTitle = new Label
            {
                Text = "Retur Barang",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Left = 20,
                Top = 18
            };
            panelHeader.Controls.Add(lblTitle);

            pnlTop = new Panel { Dock = DockStyle.Top, Height = 124, BackColor = Color.White, Padding = new Padding(20, 14, 20, 14) };
            var lblTxn = new Label { Text = "No. Transaksi:", AutoSize = true, Location = new Point(0, 6) };
            txtTxnNumber = new TextBox { PlaceholderText = "Contoh: TRX-20260311120000", Width = 320, Location = new Point(120, 2) };
            btnLoadTxn = BuildHeaderButton("Load", 90);
            btnLoadTxn.Location = new Point(txtTxnNumber.Right + 10, 0);
            btnLoadTxn.Click += BtnLoadTxn_Click;

            lblHint = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(90, 90, 90),
                Location = new Point(0, 44),
                Text = "Masukkan No. Transaksi untuk retur dari transaksi, atau isi manual di bawah."
            };

            pnlManual = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                AutoSize = false,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };

            txtName = new TextBox { PlaceholderText = "Nama Item / Custom", Width = 280, Margin = new Padding(0, 0, 10, 0) };
            txtQty = new TextBox { PlaceholderText = "Qty", Width = 90, Text = "1", Margin = new Padding(0, 0, 10, 0) };
            txtUnit = new TextBox { PlaceholderText = "Unit", Width = 110, Text = "pcs", Margin = new Padding(0, 0, 10, 0) };
            txtPrice = new TextBox { PlaceholderText = "Harga", Width = 130, Text = "0", Margin = new Padding(0, 0, 10, 0) };
            txtBarcode = new TextBox { PlaceholderText = "Barcode (opsional)", Width = 200, Margin = new Padding(0, 0, 10, 0) };
            btnAdd = BuildPrimaryButton("Tambah", 120);
            btnAdd.Click += BtnAdd_Click;
            pnlManual.Controls.AddRange(new Control[] { txtName, txtQty, txtUnit, txtPrice, txtBarcode, btnAdd });

            pnlTop.Controls.Add(lblTxn);
            pnlTop.Controls.Add(txtTxnNumber);
            pnlTop.Controls.Add(btnLoadTxn);
            pnlTop.Controls.Add(lblHint);
            pnlTop.Controls.Add(pnlManual);

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false
            };
            grid.Columns.Add("ItemId", "ItemId");
            grid.Columns.Add("Barcode", "Barcode");
            grid.Columns.Add("Name", "Nama");
            grid.Columns.Add("QtyAsal", "Qty Asal");
            var colQtyRet = new DataGridViewTextBoxColumn { Name = "QtyReturn", HeaderText = "Qty Retur" };
            grid.Columns.Add(colQtyRet);
            grid.Columns.Add("Unit", "Unit");
            grid.Columns.Add("Price", "Harga");
            grid.Columns.Add("Total", "Total");
            grid.Columns["ItemId"].Visible = false;
            grid.Columns["QtyAsal"].ReadOnly = true;
            grid.Columns["QtyAsal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["QtyReturn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Price"].DefaultCellStyle.Format = "N0";
            grid.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Total"].DefaultCellStyle.Format = "N0";
            grid.Columns["Total"].ReadOnly = true;
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

            grid.Columns["QtyAsal"].Visible = false;
            grid.Columns["QtyReturn"].HeaderText = "Qty";

            pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Color.White, Padding = new Padding(20, 14, 20, 14) };
            btnRemove = BuildHeaderButton("Hapus", 110);
            btnRemove.Location = new Point(0, 8);
            btnRemove.Click += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0) grid.Rows.RemoveAt(grid.SelectedRows[0].Index);
                RecalcTotal();
            };
            btnSave = new Button { Text = "Simpan Retur", Width = 160, Height = 42, BackColor = Color.IndianRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold) };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(pnlBottom.Width - btnSave.Width, 8);
            btnSave.Click += BtnSave_Click;
            lblTotal = new Label { Text = "Total: 0", AutoSize = true, Location = new Point(btnRemove.Right + 16, 14), Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(51, 51, 51) };

            pnlBottom.Controls.AddRange(new Control[] { btnRemove, lblTotal, btnSave });
            pnlBottom.SizeChanged += (_, __) =>
            {
                btnSave.Location = new Point(pnlBottom.Width - btnSave.Width, 8);
            };

            var panelBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };
            panelBody.Controls.Add(grid);

            Controls.Add(panelBody);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Controls.Add(panelHeader);
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
                Font = new Font("Segoe UI", 10F)
            };
            b.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            return b;
        }

        private static Button BuildPrimaryButton(string text, int width)
        {
            var b = new Button
            {
                Text = text,
                Width = width,
                Height = 42,
                BackColor = Color.FromArgb(0, 120, 215),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (_isTransactionMode)
            {
                MessageBox.Show("Tambah manual dinonaktifkan saat retur dari transaksi.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Nama item wajib.");
                return;
            }
            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Qty tidak valid.");
                return;
            }
            if (!decimal.TryParse(CleanMoney(txtPrice.Text), out decimal price) || price < 0)
            {
                MessageBox.Show("Harga tidak valid.");
                return;
            }
            string unit = string.IsNullOrWhiteSpace(txtUnit.Text) ? "pcs" : txtUnit.Text.Trim();
            decimal total = price * qty;
            grid.Rows.Add(
                0,                           // ItemId = 0 untuk custom
                txtBarcode.Text?.Trim() ?? "",
                txtName.Text.Trim(),
                qty,
                qty,
                unit,
                price,
                total
            );
            txtName.Clear();
            txtQty.Text = "1";
            txtUnit.Text = "pcs";
            txtPrice.Text = "0";
            txtBarcode.Clear();
            RecalcTotal();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (grid.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada item retur.");
                return;
            }

            var invoice = new InvoiceData
            {
                Items = new List<InvoiceItem>()
            };
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                int itemId = 0;
                int.TryParse(row.Cells["ItemId"].Value?.ToString() ?? "0", out itemId);
                int qty = 0;
                int.TryParse(row.Cells["QtyReturn"].Value?.ToString() ?? "0", out qty);
                if (qty <= 0) continue;
                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                string unit = row.Cells["Unit"].Value?.ToString() ?? "pcs";
                string name = row.Cells["Name"].Value?.ToString() ?? "-";
                string barcode = row.Cells["Barcode"].Value?.ToString() ?? "";
                invoice.Items.Add(new InvoiceItem
                {
                    ItemId = itemId,
                    Name = name,
                    Barcode = barcode,
                    Unit = unit,
                    UnitVariant = unit,
                    ConversionRate = 1,
                    Qty = qty,
                    Price = price,
                    CostPrice = 0,
                    DiscountPercent = 0,
                    DiscountAmount = 0,
                    Tax = 0,
                    Note = "RETURN",
                    Total = qty * price
                });
            }
            var res = _txService.ProcessReturnAndSave(invoice, "RETUR");
            if (!res.IsSuccess)
            {
                MessageBox.Show(res.Message);
                return;
            }
            MessageBox.Show("Retur tersimpan.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void PreloadTransaction(string txnNumber, bool autoLoad = false)
        {
            txtTxnNumber.Text = txnNumber;
            EnterTransactionMode();
            if (autoLoad)
            {
                LoadTxnByNumber(txnNumber);
            }
        }

        private void BtnLoadTxn_Click(object? sender, EventArgs e)
        {
            string num = txtTxnNumber.Text?.Trim();
            if (string.IsNullOrWhiteSpace(num))
            {
                MessageBox.Show("Masukkan nomor transaksi.");
                return;
            }
            LoadTxnByNumber(num);
        }

        private void LoadTxnByNumber(string num)
        {
            try
            {
                var dt = _txRepo.GetTransactionDetailsByNumber(num);
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Transaksi tidak ditemukan.");
                    return;
                }
                grid.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    int itemId = r["item_id"] == DBNull.Value ? 0 : Convert.ToInt32(r["item_id"]);
                    string name = r["name"] == DBNull.Value ? "(tanpa nama)" : r["name"].ToString();
                    string barcode = r["barcode"] == DBNull.Value ? "" : r["barcode"].ToString();
                    int qtyAsal = Convert.ToInt32((decimal)r["tsd_quantity"]);
                    string unit = r["tsd_unit"].ToString();
                    decimal price = (decimal)r["tsd_sell_price"];
                    grid.Rows.Add(itemId, barcode, name, qtyAsal, 0, unit, price, 0);
                }
                // pastikan hanya kolom QtyReturn yang bisa diedit
                if (grid.Columns.Contains("Name")) grid.Columns["Name"].ReadOnly = true;
                if (grid.Columns.Contains("Barcode")) grid.Columns["Barcode"].ReadOnly = true;
                if (grid.Columns.Contains("QtyAsal")) grid.Columns["QtyAsal"].ReadOnly = true;
                if (grid.Columns.Contains("Unit")) grid.Columns["Unit"].ReadOnly = true;
                if (grid.Columns.Contains("Price")) grid.Columns["Price"].ReadOnly = true;
                if (grid.Columns.Contains("Total")) grid.Columns["Total"].ReadOnly = true;
                RecalcTotal();
                grid.CellEndEdit -= Grid_CellEndEdit;
                grid.CellEndEdit += Grid_CellEndEdit;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load transaksi: " + ex.Message);
            }
        }

        private void Grid_CellEndEdit(object? sender, DataGridViewCellEventArgs ev)
        {
            if (grid.Columns.Contains("QtyReturn") && ev.ColumnIndex == grid.Columns["QtyReturn"].Index)
                RecalcTotal();
        }

        private void EnterTransactionMode()
        {
            _isTransactionMode = true;
            // Sembunyikan input manual yang tidak dipakai
            pnlManual.Visible = false;
            // Tombol hapus baris manual juga disembunyikan
            if (btnRemove != null) btnRemove.Visible = false;
            // Perkecil header
            pnlTop.Height = 78;
            if (lblHint != null) lblHint.Text = "Mode: Retur dari transaksi (isi Qty Retur pada tabel).";
            if (grid.Columns.Contains("QtyAsal")) grid.Columns["QtyAsal"].Visible = true;
            if (grid.Columns.Contains("QtyReturn")) grid.Columns["QtyReturn"].HeaderText = "Qty Retur";
        }

        private void RecalcTotal()
        {
            decimal sum = 0;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                int qty = 0;
                int.TryParse(row.Cells["QtyReturn"].Value?.ToString() ?? "0", out qty);
                decimal price = 0;
                decimal.TryParse(row.Cells["Price"].Value?.ToString() ?? "0", out price);
                decimal total = qty * price;
                row.Cells["Total"].Value = total;
                sum += total;
            }
            lblTotal.Text = $"Total: {sum:N0}";
        }

        private static string CleanMoney(string t)
        {
            return (t ?? "").Replace("Rp", "").Replace(".", "").Replace(",", "").Trim();
        }
    }
}
