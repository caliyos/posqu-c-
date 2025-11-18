using Npgsql;
using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class PurchaseOrderForm : Form
    {

        private readonly ItemController _itemController;
        private DataGridViewManager _dgvItemManager;

        // Tombol navigasi
        //private void btnNext_Click(object sender, EventArgs e) => _dgvItemManager.NextPage();
        //private void btnPrev_Click(object sender, EventArgs e) => _dgvItemManager.PreviousPage();
        //private void btnFirst_Click(object sender, EventArgs e) => _dgvItemManager.FirstPage();
        //private void btnLast_Click(object sender, EventArgs e) => _dgvItemManager.LastPage();


        private void btnFirst_Click(object sender, EventArgs e) => _dgvItemManager.FirstPage();
        private void btnPrev_Click(object sender, EventArgs e) => _dgvItemManager.PreviousPage();
        private void btnNext_Click(object sender, EventArgs e) => _dgvItemManager.NextPage();
        private void btnLast_Click(object sender, EventArgs e) => _dgvItemManager.LastPage();

        // Pencarian
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _dgvItemManager.Filter(txtSearch.Text, "name"); // search by nama barang
        }


        public PurchaseOrderForm()
        {
            _itemController = new ItemController(); // inisialisasi sekali
            InitializeComponent();
            LoadSuppliers();
            LoadStatuses();
            LoadItems();
            InitOrderDetailsGrid();


            //_dgvItemManager.ReadOnly = true;
            _dgvItemManager.PagingInfoLabel = lblPageNumber;

            txtSearch.TextChanged += txtSearch_TextChanged;
            _dgvItemManager.Filter("", "name");

            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            cmbPageSize.SelectedIndex = 0;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

            btnFirst.Click += btnFirst_Click;
            btnPrev.Click += btnPrev_Click;
            btnNext.Click += btnNext_Click;
            btnLast.Click += btnLast_Click;

            btnAddItem.Click += btnAddItem_Click;
            btnSave.Click += btnSave_Click;

            this.WindowState = FormWindowState.Maximized;
            dgvOrderDetails.CellEndEdit += dgvOrderDetails_CellEndEdit;
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedSize = Convert.ToInt32(cmbPageSize.SelectedItem);
            _dgvItemManager.SetPageSize(selectedSize);
        }

        private void LoadSuppliers()
        {
            //DataTable dt = ItemController.GetSuppliers();
            //cmbSupplier.DataSource = dt;
            //cmbSupplier.DisplayMember = "name";
            //cmbSupplier.ValueMember = "id";
            //cmbSupplier.SelectedIndex = -1; // default kosong


        }

        private void LoadStatuses()
        {
            cmbStatus.Items.AddRange(new string[] { "PENDING", "APPROVED", "RECEIVED", "CANCELLED" });
            cmbStatus.SelectedIndex = 0;
        }

        private void LoadItems()
        {
            DataTable dt = _itemController.GetItems();

            // Init manager
            _dgvItemManager = new DataGridViewManager(dgvItems, dt, 10);
            _dgvItemManager.PagingInfoLabel = lblPagingInfo; // Label untuk info "Showing x–y of z"
            _dgvItemManager.OnAfterLoadPage += () =>
            {
                // bisa sembunyikan kolom ID otomatis
                _dgvItemManager.ToggleColumnVisibility("id", false);
                _dgvItemManager.ToggleColumnVisibility("barcode", false);

                // bikin semua kolom readonly
                foreach (DataGridViewColumn col in dgvItems.Columns)
                {
                    col.ReadOnly = true;
                }
            };
        }

        private void InitOrderDetailsGrid()
        {
            dgvOrderDetails.Columns.Clear();

            // ID barang (hidden)
            dgvOrderDetails.Columns.Add("item_id", "Item ID");
            dgvOrderDetails.Columns["item_id"].Visible = false;

            // Nama barang
            dgvOrderDetails.Columns.Add("name", "Name");

            // Quantity
            var qtyCol = new DataGridViewTextBoxColumn();
            qtyCol.Name = "quantity";
            qtyCol.HeaderText = "Qty";
            dgvOrderDetails.Columns.Add(qtyCol);

            var unit = new DataGridViewTextBoxColumn();
            unit.Name = "unit";
            unit.HeaderText = "Unit";
            dgvOrderDetails.Columns.Add(unit);


            // Harga per unit
            var priceCol = new DataGridViewTextBoxColumn();
            priceCol.Name = "unit_price";
            priceCol.HeaderText = "Unit Price";
            dgvOrderDetails.Columns.Add(priceCol);

            // Subtotal (Qty * Price, readonly)
            var subtotalCol = new DataGridViewTextBoxColumn();
            subtotalCol.Name = "subtotal";
            subtotalCol.HeaderText = "Subtotal";
            subtotalCol.ReadOnly = true;
            dgvOrderDetails.Columns.Add(subtotalCol);

            // Note
            var noteCol = new DataGridViewTextBoxColumn();
            noteCol.Name = "note";
            noteCol.HeaderText = "Note";
            dgvOrderDetails.Columns.Add(noteCol);
        }

        private void dgvOrderDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvOrderDetails.Columns["quantity"].Index ||
                e.ColumnIndex == dgvOrderDetails.Columns["unit_price"].Index)
            {
                var row = dgvOrderDetails.Rows[e.RowIndex];

                decimal qty = 0;
                decimal price = 0;

                decimal.TryParse(row.Cells["quantity"].Value?.ToString(), out qty);
                decimal.TryParse(row.Cells["unit_price"].Value?.ToString(), out price);

                row.Cells["subtotal"].Value = qty * price;
            }
        }


        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow != null)
            {
                string itemId = dgvItems.CurrentRow.Cells["id"].Value.ToString();
                string name = dgvItems.CurrentRow.Cells["name"].Value.ToString();
                string unit = dgvItems.CurrentRow.Cells["unit"].Value.ToString();
                decimal unitPrice = 0;
                decimal.TryParse(dgvItems.CurrentRow.Cells["buy_price"].Value?.ToString(), out unitPrice);

                decimal quantity = 1;

                // Hitung subtotal
                decimal subtotal = quantity * unitPrice;

                dgvOrderDetails.Rows.Add(itemId, name, quantity, unit, unitPrice, subtotal, ""); // tambahkan subtotal
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                // Insert header (purchase_orders)
                string insertPO = @"
            INSERT INTO purchase_orders (supplier_id, order_date, status, total_amount, note, created_by)
            VALUES (@supplier_id, CURRENT_DATE, @status, @total_amount, @note, @created_by)
            RETURNING id;";

                decimal totalAmount = dgvOrderDetails.Rows.Cast<DataGridViewRow>()
                    .Sum(r => Convert.ToDecimal(r.Cells["subtotal"].Value ?? 0));

                using var cmdPO = new NpgsqlCommand(insertPO, conn, tran);
                cmdPO.Parameters.AddWithValue("@supplier_id", Convert.ToInt64(cmbSupplier.SelectedValue));

                cmdPO.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());
                cmdPO.Parameters.AddWithValue("@total_amount", totalAmount);
                cmdPO.Parameters.AddWithValue("@note", txtNote.Text ?? "");
                cmdPO.Parameters.AddWithValue("@created_by", 1); // TODO: ganti user login id
                long poId = (long)cmdPO.ExecuteScalar();

                // Insert details (purchase_order_items)
                foreach (DataGridViewRow row in dgvOrderDetails.Rows)
                {
                    if (row.IsNewRow) continue;

                    string insertItem = @"
                INSERT INTO purchase_order_items (po_id, item_id, quantity, unit, unit_price, note)
                VALUES (@po_id, @item_id, @qty, @unit, @unit_price, @note);";

                    using var cmdItem = new NpgsqlCommand(insertItem, conn, tran);
                    cmdItem.Parameters.AddWithValue("@po_id", poId);
                    cmdItem.Parameters.AddWithValue("@item_id", Convert.ToInt64(row.Cells["item_id"].Value));
                    cmdItem.Parameters.AddWithValue("@qty", Convert.ToDecimal(row.Cells["quantity"].Value));
                    cmdItem.Parameters.AddWithValue("@unit", row.Cells["unit"].Value?.ToString() ?? "");
                    cmdItem.Parameters.AddWithValue("@unit_price", Convert.ToDecimal(row.Cells["unit_price"].Value));
                    cmdItem.Parameters.AddWithValue("@note", row.Cells["note"].Value?.ToString() ?? "");
                    cmdItem.ExecuteNonQuery();
                }

                tran.Commit();
                MessageBox.Show("Purchase Order saved successfully!");

                // kasih sinyal ke pemanggil
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show("Error saving PO: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PurchaseOrderListForm POForm = new PurchaseOrderListForm();
            this.Hide();
            POForm.FormClosed += (s, args) => this.Show();
            POForm.Show();
        }
    }
}
