using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace POS_qu
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += CustomerForm_Load;
            btnDelete.Click += btnDelete_Click;
            btnEdit.Click += btnEdit_Click;
            btnAdd.Click += btnAdd_Click;
            btnRefresh.Click += btnRefresh_Click;
        }

        private DataGridViewManager dgvManager;
        private CustomerController customerController = new CustomerController();
        private int selectedCustomerId = 0;

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadCustomersData();
        }

        private void LoadCustomersData()
        {
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.ReadOnly = true;

            var customers = customerController.GetCustomers();
            DataTable customersTable = ToDataTable(customers);

            dgvManager = new DataGridViewManager(dgvUnits, customersTable, 10);
            dgvManager.PagingInfoLabel = lblPageNumber;

            dgvUnits.CellClick += dgvUnits_CellClick;
            txtSearch.TextChanged += txtSearch_TextChanged;

            dgvManager.Filter("", "name");

            cmbPageSize.Items.Clear();
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            cmbPageSize.SelectedIndex = 0;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvManager.SetPageSize(Convert.ToInt32(cmbPageSize.SelectedItem));
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvManager.Filter(txtSearch.Text, "name");
        }

        private void dgvUnits_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvUnits.Rows[e.RowIndex];
            selectedCustomerId = Convert.ToInt32(row.Cells["id"].Value);
            txtName.Text = row.Cells["name"].Value.ToString();
            txtPhone.Text = row.Cells["phone"].Value.ToString();
            textNote.Text = row.Cells["note"].Value.ToString();

            btnAdd.Enabled = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        private DataTable ToDataTable(List<Customer> customers)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("phone", typeof(string));
            dt.Columns.Add("note", typeof(string));
            dt.Columns.Add("created_by", typeof(int));
            dt.Columns.Add("created_at", typeof(DateTime));
            dt.Columns.Add("deleted_at", typeof(DateTime));

            foreach (var c in customers)
            {
                dt.Rows.Add(c.Id, c.Name, c.Phone, c.Note, c.CreatedBy, c.CreatedAt, c.DeletedAt);
            }

            return dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Nama Customer wajib diisi");
                return;
            }

            var customer = new Customer
            {
                Name = txtName.Text,
                Phone = txtPhone.Text,
                Note = textNote.Text
            };

            if (customerController.AddCustomer(customer))
            {
                MessageBox.Show("Customer berhasil ditambahkan");
                LoadCustomersData();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Gagal menambahkan customer");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0)
            {
                MessageBox.Show("Pilih customer terlebih dahulu");
                return;
            }

            var customer = new Customer
            {
                Id = selectedCustomerId,
                Name = txtName.Text,
                Phone = txtPhone.Text,
                Note = textNote.Text
            };

            if (customerController.UpdateCustomer(customer))
            {
                MessageBox.Show("Customer berhasil diupdate");
                LoadCustomersData();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Gagal update customer");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0)
            {
                MessageBox.Show("Pilih customer terlebih dahulu");
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus customer ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (customerController.DeleteCustomer(selectedCustomerId))
                {
                    MessageBox.Show("Customer berhasil dihapus");
                    LoadCustomersData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Gagal menghapus customer");
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCustomersData();
            ClearForm();
        }

        private void btnFirst_Click(object sender, EventArgs e) => dgvManager.FirstPage();
        private void btnPrev_Click(object sender, EventArgs e) => dgvManager.PreviousPage();
        private void btnNext_Click(object sender, EventArgs e) => dgvManager.NextPage();
        private void btnLast_Click(object sender, EventArgs e) => dgvManager.LastPage();

        private void ClearForm()
        {
            txtName.Clear();
            txtPhone.Clear();
            //textNote.Clear();
            selectedCustomerId = 0;
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
    }
}
