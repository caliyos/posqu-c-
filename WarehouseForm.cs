using POS_qu.Controllers;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Data;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class WarehouseForm : Form
    {
        public WarehouseForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += WarehouseForm_Load;
            
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            
            btnFirst.Click += btnFirst_Click;
            btnPrev.Click += btnPrev_Click;
            btnNext.Click += btnNext_Click;
            btnLast.Click += btnLast_Click;
        }

        private DataGridViewManager dgvManager;
        private WarehouseController warehouseController = new WarehouseController();
        private int selectedId = 0;

        private void WarehouseForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvWarehouses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWarehouses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWarehouses.AllowUserToAddRows = false;
            dgvWarehouses.ReadOnly = true;

            DataTable dt = warehouseController.GetWarehouses();

            dgvManager = new DataGridViewManager(dgvWarehouses, dt, 10);
            dgvManager.PagingInfoLabel = lblPageNumber;

            dgvWarehouses.CellClick -= dgvWarehouses_CellClick;
            dgvWarehouses.CellClick += dgvWarehouses_CellClick;
            
            txtSearch.TextChanged -= txtSearch_TextChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;

            dgvManager.Filter("", "name");

            cmbPageSize.Items.Clear();
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            cmbPageSize.SelectedIndex = 0;
            cmbPageSize.SelectedIndexChanged -= cmbPageSize_SelectedIndexChanged;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

            ClearForm();
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvManager.SetPageSize(Convert.ToInt32(cmbPageSize.SelectedItem));
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvManager.Filter(txtSearch.Text, "name");
        }

        private void dgvWarehouses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvWarehouses.Rows[e.RowIndex];
            selectedId = Convert.ToInt32(row.Cells["id"].Value);
            txtName.Text = row.Cells["name"].Value.ToString();
            cmbType.Text = row.Cells["type"].Value.ToString();
            chkIsActive.Checked = Convert.ToBoolean(row.Cells["is_active"].Value);

            btnAdd.Enabled = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Nama Gudang wajib diisi");
                return;
            }
            
            var warehouse = new Warehouse
            {
                Name = txtName.Text,
                Type = cmbType.Text,
                IsActive = chkIsActive.Checked
            };

            warehouseController.InsertWarehouse(warehouse);
            MessageBox.Show("Gudang berhasil ditambahkan");
            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih gudang terlebih dahulu");
                return;
            }

            var warehouse = new Warehouse
            {
                Id = selectedId,
                Name = txtName.Text,
                Type = cmbType.Text,
                IsActive = chkIsActive.Checked
            };

            warehouseController.UpdateWarehouse(warehouse);
            MessageBox.Show("Gudang berhasil diupdate");
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0) return;

            if (MessageBox.Show("Yakin ingin menghapus gudang ini?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                warehouseController.DeleteWarehouse(selectedId);
                MessageBox.Show("Gudang berhasil dihapus");
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnFirst_Click(object sender, EventArgs e) => dgvManager.FirstPage();
        private void btnPrev_Click(object sender, EventArgs e) => dgvManager.PreviousPage();
        private void btnNext_Click(object sender, EventArgs e) => dgvManager.NextPage();
        private void btnLast_Click(object sender, EventArgs e) => dgvManager.LastPage();

        private void ClearForm()
        {
            txtName.Clear();
            cmbType.SelectedIndex = 0;
            chkIsActive.Checked = true;
            selectedId = 0;
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
    }
}