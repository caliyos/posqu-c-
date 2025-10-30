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

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace POS_qu
{
    public partial class PurchaseOrderListForm : Form
    {

        private DataTable _dtPO;

        public PurchaseOrderListForm()
        {
            InitializeComponent();
            LoadPO();

            btnViewDetail.Click += btnViewDetail_Click;
            btnRefresh.Click += btnRefresh_Click;
            this.WindowState = FormWindowState.Maximized;
            txtSearch.TextChanged += txtSearch_TextChanged;

            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.Add("All");      // untuk semua
            cmbFilterStatus.Items.Add("DRAFT");
            cmbFilterStatus.Items.Add("PENDING");
            cmbFilterStatus.Items.Add("APPROVED");
            cmbFilterStatus.Items.Add("REJECTED");
            cmbFilterStatus.Items.Add("COMPLETED");

            cmbFilterStatus.SelectedIndex = 0; // default "All"

            cmbFilterStatus.SelectedIndexChanged += cmbFilterStatus_SelectedIndexChanged;
            btnChangeStatus.Click += btnChangeStatus_Click;
            btnCetakPO.Click += btnCetakPO_Click;
        }

        private void LoadPO()
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string query = @"
        SELECT po.id, s.name AS supplier, po.order_date, po.expected_date,
               po.status, po.total_amount, po.note, po.created_at
        FROM purchase_orders po
        LEFT JOIN suppliers s ON po.supplier_id = s.id
        WHERE (@status = 'All' OR po.status = @status)
        ORDER BY po.created_at DESC";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@status", cmbFilterStatus.SelectedItem?.ToString() ?? "All");

            using var da = new NpgsqlDataAdapter(cmd);
            _dtPO = new DataTable();
            da.Fill(_dtPO);

            dgvPO.DataSource = _dtPO;
            dgvPO.Columns["id"].HeaderText = "PO ID";
            dgvPO.Columns["supplier"].HeaderText = "Supplier";
            dgvPO.Columns["order_date"].HeaderText = "Order Date";
            dgvPO.Columns["expected_date"].HeaderText = "Expected Date";
            dgvPO.Columns["status"].HeaderText = "Status";
            dgvPO.Columns["total_amount"].HeaderText = "Total";
            dgvPO.Columns["note"].HeaderText = "Note";
            dgvPO.Columns["created_at"].HeaderText = "Created At";
        }

        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPO();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPO();
        }

        private void btnViewDetail_Click(object sender, EventArgs e)
        {
            if (dgvPO.CurrentRow == null) return;

            long poId = Convert.ToInt64(dgvPO.CurrentRow.Cells["id"].Value);
            PurchaseOrderDetailForm detailForm = new PurchaseOrderDetailForm(poId);
            detailForm.ShowDialog();
        }

        private void btnPO_Click(object sender, EventArgs e)
        {
            using (PurchaseOrderForm frm = new PurchaseOrderForm())
            {
                // Show sebagai dialog
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Kalau berhasil save, refresh PO list
                    LoadPO();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_dtPO == null || _dtPO.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk dicetak.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // contoh label periode bisa pakai filter status
            string periodeLabel = cmbFilterStatus.SelectedItem?.ToString() ?? "All";

            pdfExport(periodeLabel, _dtPO);
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_dtPO == null) return;

            string keyword = txtSearch.Text.Replace("'", "''"); // escape single quote

            if (string.IsNullOrWhiteSpace(keyword))
            {
                // Reset filter
                dgvPO.DataSource = _dtPO;
                return;
            }

            // Buat DataView
            DataView dv = new DataView(_dtPO);

            // Cari semua kolom text
            List<string> filters = new List<string>();
            foreach (DataColumn col in _dtPO.Columns)
            {
                if (col.DataType == typeof(string) || col.DataType == typeof(DateTime) || col.DataType == typeof(decimal) || col.DataType == typeof(int))
                {
                    filters.Add($"Convert([{col.ColumnName}], 'System.String') LIKE '%{keyword}%'");
                }
            }

            dv.RowFilter = string.Join(" OR ", filters);

            dgvPO.DataSource = dv;
        }


        private void pdfExport(string periodeLabel, DataTable dt)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF files (*.pdf)|*.pdf";
                sfd.FileName = $"PurchaseOrder_{periodeLabel}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var filePath = sfd.FileName;
                        QuestPDF.Settings.License = LicenseType.Community;

                        Document.Create(container =>
                        {
                            container.Page(page =>
                            {
                                page.Size(PageSizes.A4);
                                page.Margin(20);

                                page.Content().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        for (int i = 0; i < dt.Columns.Count; i++)
                                            columns.RelativeColumn();
                                    });

                                    // header
                                    table.Header(header =>
                                    {
                                        foreach (DataColumn col in dt.Columns)
                                            header.Cell().Text(col.ColumnName).Bold();
                                    });

                                    // rows
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        foreach (var cell in row.ItemArray)
                                            table.Cell().Text(cell?.ToString() ?? "");
                                    }
                                });
                            });
                        })
                        .GeneratePdf(filePath);

                        MessageBox.Show("PDF berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start("explorer.exe", filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menyimpan PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            if (dgvPO.CurrentRow == null)
            {
                MessageBox.Show("Pilih dulu PO yang ingin diubah statusnya.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            long poId = Convert.ToInt64(dgvPO.CurrentRow.Cells["id"].Value);
            string currentStatus = dgvPO.CurrentRow.Cells["status"].Value.ToString();

            // tampilkan pilihan status baru
            using (var form = new Form())
            {
                form.Text = "Ubah Status PO";
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new System.Drawing.Size(400, 250);

                var cmb = new ComboBox { Left = 20, Top = 20, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cmb.Items.AddRange(new string[] { "DRAFT", "PENDING", "APPROVED", "REJECTED", "COMPLETED" });
                cmb.SelectedItem = currentStatus;

                var btnOk = new Button { Text = "OK", Left = 20, Top = 60, Width = 80, Height = 40, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Batal", Left = 140, Top = 60, Width = 80, Height = 40, DialogResult = DialogResult.Cancel };

                form.Controls.Add(cmb);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);

                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    string newStatus = cmb.SelectedItem.ToString();

                    if (newStatus != currentStatus)
                    {
                        using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                        conn.Open();

                        string updateQuery = "UPDATE purchase_orders SET status=@status, updated_at=NOW() WHERE id=@id";
                        using var cmd = new NpgsqlCommand(updateQuery, conn);
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@id", poId);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Status berhasil diubah!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadPO(); // refresh datagrid
                        }
                        else
                        {
                            MessageBox.Show("Gagal mengubah status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }



        private string SafeToString(object value, string format = null)
        {
            if (value == DBNull.Value || value == null) return "-";

            if (value is DateTime dt)
                return format != null ? dt.ToString(format) : dt.ToString("dd/MM/yyyy");

            if (value is decimal dec)
                return format != null ? dec.ToString(format) : dec.ToString("N0");

            return value.ToString();
        }

        private decimal SafeToDecimal(object value)
        {
            if (value == DBNull.Value || value == null) return 0;
            if (decimal.TryParse(value.ToString(), out var result)) return result;
            return 0;
        }

        private void btnCetakPO_Click(object sender, EventArgs e)
        {
            if (dgvPO.CurrentRow == null)
            {
                MessageBox.Show("Pilih PO yang ingin dicetak.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            long poId = Convert.ToInt64(dgvPO.CurrentRow.Cells["id"].Value);

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            // ===== Ambil data parent =====
            string queryParent = @"
        SELECT po.id, po.order_date, po.expected_date, po.status, po.total_amount, po.note,
               s.name AS supplier_name, s.contact_name, s.phone, s.email, s.address
        FROM purchase_orders po
        JOIN suppliers s ON po.supplier_id = s.id
        WHERE po.id = @id";
            using var cmdParent = new NpgsqlCommand(queryParent, conn);
            cmdParent.Parameters.AddWithValue("@id", poId);

            DataTable dtParent = new DataTable();
            using (var da = new NpgsqlDataAdapter(cmdParent))
                da.Fill(dtParent);

            if (dtParent.Rows.Count == 0)
            {
                MessageBox.Show("Data PO tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow po = dtParent.Rows[0];

            // ===== Ambil data child =====
            string queryChild = @"
        SELECT i.name AS item_name, poi.unit, poi.quantity, poi.unit_price, 
               (poi.quantity * poi.unit_price) AS subtotal, poi.note
        FROM purchase_order_items poi
        JOIN items i ON poi.item_id = i.id
        WHERE poi.po_id = @id";
            using var cmdChild = new NpgsqlCommand(queryChild, conn);
            cmdChild.Parameters.AddWithValue("@id", poId);

            DataTable dtChild = new DataTable();
            using (var da = new NpgsqlDataAdapter(cmdChild))
                da.Fill(dtChild);

            // ===== Cetak ke PDF =====
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF files (*.pdf)|*.pdf";
                sfd.FileName = $"PO_{po["id"]}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = sfd.FileName;
                        QuestPDF.Settings.License = LicenseType.Community;

                        Document.Create(container =>
                        {
                            container.Page(page =>
                            {
                                page.Size(PageSizes.A4);
                                page.Margin(30);
                                page.DefaultTextStyle(x => x.FontSize(10));

                                // ===== Header =====
                                page.Header()
                                    .Text($"PURCHASE ORDER #{SafeToString(po["id"])}")
                                    .FontSize(16).Bold().AlignCenter();

                                page.Content().Column(col =>
                                {
                                    // Info supplier & PO
                                    col.Item().Row(row =>
                                    {
                                        row.RelativeItem().Column(c =>
                                        {
                                            c.Item().Text("Supplier").Bold();
                                            c.Item().Text(SafeToString(po["supplier_name"]));
                                            c.Item().Text(SafeToString(po["address"]));
                                            c.Item().Text($"Telp: {SafeToString(po["phone"])}");
                                            c.Item().Text($"Email: {SafeToString(po["email"])}");
                                        });

                                        row.ConstantItem(50);

                                        row.RelativeItem().Column(c =>
                                        {
                                            c.Item().Text("PO Info").Bold();
                                            c.Item().Text($"Tanggal: {SafeToString(po["order_date"], "dd/MM/yyyy")}");
                                            c.Item().Text($"Expected: {SafeToString(po["expected_date"], "dd/MM/yyyy")}");
                                            c.Item().Text($"Status: {SafeToString(po["status"])}");
                                        });
                                    });

                                    col.Item().PaddingVertical(10);

                                    // ===== Table Items =====
                                    col.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(4); // Item
                                            columns.RelativeColumn(2); // Unit
                                            columns.RelativeColumn(2); // Qty
                                            columns.RelativeColumn(3); // Price
                                            columns.RelativeColumn(3); // Subtotal
                                        });

                                        // Header
                                        table.Header(header =>
                                        {
                                            header.Cell().Text("Item").Bold();
                                            header.Cell().Text("Unit").Bold();
                                            header.Cell().Text("Qty").Bold().AlignRight();
                                            header.Cell().Text("Harga").Bold().AlignRight();
                                            header.Cell().Text("Subtotal").Bold().AlignRight();
                                        });

                                        // Rows
                                        foreach (DataRow item in dtChild.Rows)
                                        {
                                            table.Cell().Text(SafeToString(item["item_name"]));
                                            table.Cell().Text(SafeToString(item["unit"]));
                                            table.Cell().Text(SafeToString(item["quantity"])).AlignRight();
                                            table.Cell().Text(SafeToString(item["unit_price"], "N0")).AlignRight();
                                            table.Cell().Text(SafeToString(item["subtotal"], "N0")).AlignRight();
                                        }
                                    });

                                    col.Item().AlignRight()
                                        .Text($"Total: Rp {SafeToString(po["total_amount"], "N0")}")
                                        .FontSize(12).Bold();

                                    if (!string.IsNullOrEmpty(SafeToString(po["note"])) && SafeToString(po["note"]) != "-")
                                    {
                                        col.Item().PaddingTop(10).Text($"Note: {SafeToString(po["note"])}");
                                    }
                                });

                                page.Footer()
                                    .AlignCenter()
                                    .Text(txt =>
                                    {
                                        txt.Span("Halaman ");
                                        txt.CurrentPageNumber();
                                        txt.Span(" / ");
                                        txt.TotalPages();
                                    });
                            });
                        })
                        .GeneratePdf(filePath);

                        MessageBox.Show("PO berhasil dicetak!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start("explorer.exe", filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal cetak PO: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



    }
}
