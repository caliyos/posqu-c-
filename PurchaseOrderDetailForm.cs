using Npgsql;
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
    public partial class PurchaseOrderDetailForm : Form
    {
        public PurchaseOrderDetailForm()
        {
            InitializeComponent();

            // Bikin full layar
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private long _poId;

        public PurchaseOrderDetailForm(long poId)
        {
            InitializeComponent();
            _poId = poId;
            LoadPODetail();
        }

        private void LoadPODetail()
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            // Ambil header
            string queryHeader = @"
                SELECT po.id, s.name AS supplier, po.order_date, po.status, po.total_amount, po.note
                FROM purchase_orders po
                JOIN suppliers s ON po.supplier_id = s.id
                WHERE po.id = @id";

            using var cmd = new NpgsqlCommand(queryHeader, conn);
            cmd.Parameters.AddWithValue("@id", _poId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                lblPOId.Text = $"PO ID: {reader["id"]}";
                lblSupplier.Text = $"Supplier: {reader["supplier"]}";
                lblDate.Text = $"Tanggal: {Convert.ToDateTime(reader["order_date"]).ToShortDateString()}";
                lblStatus.Text = $"Status: {reader["status"]}";
                lblTotal.Text = $"Total: {reader["total_amount"]}";
                lblNote.Text = $"Note: {reader["note"]}";
            }
            reader.Close();

            // Ambil detail
            string queryDetail = @"
    SELECT i.name AS item, poi.quantity, poi.unit_price,poi.unit,
           (poi.quantity * poi.unit_price) AS subtotal, poi.note
    FROM purchase_order_items poi
    JOIN items i ON poi.item_id = i.id
    WHERE poi.po_id = @po_id";

            using var cmdd = new NpgsqlCommand(queryDetail, conn);
            cmd.Parameters.AddWithValue("@po_id", _poId);

            using var da = new NpgsqlDataAdapter(cmdd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvItems.DataSource = dt;

            dgvItems.Columns["item"].HeaderText = "Item";
            dgvItems.Columns["quantity"].HeaderText = "Qty";
            dgvItems.Columns["unit"].HeaderText = "Unit";
            dgvItems.Columns["unit_price"].HeaderText = "Harga Beli";
            dgvItems.Columns["subtotal"].HeaderText = "Subtotal";
            dgvItems.Columns["note"].HeaderText = "Note";

            // Jadikan semua kolom readonly
            foreach (DataGridViewColumn col in dgvItems.Columns)
            {
                col.ReadOnly = true;
            }
        }

        private void btnPO_Click(object sender, EventArgs e)
        {
            //PurchaseOrderForm frm = new PurchaseOrderForm();
            //frm.Show();
        }
    }
}
