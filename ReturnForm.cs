using POS_qu.Helpers;
using POS_qu.Core;
using POS_qu.Models;
using POS_qu.services;
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

        private TransactionService _txService;
        private TransactionRepo _txRepo;

        public ReturnForm()
        {
            Text = "Retur Barang";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Padding = new Padding(10);

            _txRepo = new TransactionRepo();
            _txService = new TransactionService(_txRepo, new ActivityService());

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 100 };
            var lblTxn = new Label { Text = "No. Transaksi:", Left = 0, Top = 12, AutoSize = true };
            txtTxnNumber = new TextBox { PlaceholderText = "Contoh: TRX-20260311120000", Width = 260, Left = 110, Top = 8 };
            btnLoadTxn = new Button { Text = "Load", Left = 380, Top = 6, Width = 60 };
            btnLoadTxn.Click += BtnLoadTxn_Click;
            txtName = new TextBox { PlaceholderText = "Nama Item / Custom", Width = 220, Left = 0, Top = 16 };
            txtQty = new TextBox { PlaceholderText = "Qty", Width = 60, Left = 230, Top = 16, Text = "1" };
            txtUnit = new TextBox { PlaceholderText = "Unit", Width = 80, Left = 300, Top = 16, Text = "pcs" };
            txtPrice = new TextBox { PlaceholderText = "Harga", Width = 100, Left = 390, Top = 16, Text = "0" };
            txtBarcode = new TextBox { PlaceholderText = "Barcode (opsional)", Width = 160, Left = 500, Top = 16 };
            btnAdd = new Button { Text = "Tambah", Left = 670, Top = 14, Width = 90 };
            btnAdd.Click += BtnAdd_Click;
            pnlTop.Controls.AddRange(new Control[] { lblTxn, txtTxnNumber, btnLoadTxn, txtName, txtQty, txtUnit, txtPrice, txtBarcode, btnAdd });

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false
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

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 60 };
            btnRemove = new Button { Text = "Hapus", Left = 10, Top = 12, Width = 80 };
            btnRemove.Click += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0) grid.Rows.RemoveAt(grid.SelectedRows[0].Index);
                RecalcTotal();
            };
            btnSave = new Button { Text = "Simpan Retur", Left = 680, Top = 12, Width = 120, BackColor = Color.IndianRed, ForeColor = Color.White };
            btnSave.Click += BtnSave_Click;
            lblTotal = new Label { Text = "Total: 0", AutoSize = true, Left = 110, Top = 18, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            pnlBottom.Controls.AddRange(new Control[] { btnRemove, lblTotal, btnSave });

            Controls.Add(grid);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
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
                qty.ToString(),
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

        private void BtnLoadTxn_Click(object? sender, EventArgs e)
        {
            string num = txtTxnNumber.Text?.Trim();
            if (string.IsNullOrWhiteSpace(num))
            {
                MessageBox.Show("Masukkan nomor transaksi.");
                return;
            }
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
                RecalcTotal();
                grid.CellEndEdit += (s, ev) =>
                {
                    if (ev.ColumnIndex == grid.Columns["QtyReturn"].Index ||
                        ev.ColumnIndex == grid.Columns["Price"].Index)
                    {
                        RecalcTotal();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load transaksi: " + ex.Message);
            }
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
