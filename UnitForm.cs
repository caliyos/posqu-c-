using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class UnitForm : Form
    {
        public UnitForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; 
            this.Load += UnitForm_Load; // ⬅️ WAJIB
        }

        private DataGridViewManager dgvManager;
        private UnitController unitController = new UnitController();
        private int selectedUnitId = 0;

        private void UnitForm_Load(object sender, EventArgs e)
        {
            LoadUnitsData();
        }

        private void LoadUnitsData()
        {
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.ReadOnly = true;

            var units = unitController.GetUnits();
            DataTable unitsTable = ToDataTable(units);

            dgvManager = new DataGridViewManager(dgvUnits, unitsTable, 10);

            dgvManager.PagingInfoLabel = lblPageNumber;

            dgvUnits.CellClick += dgvUnits_CellClick;
            txtSearch.TextChanged += txtSearch_TextChanged;

            dgvManager.Filter("", "name");

            cmbPageSize.Items.Clear();
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            cmbPageSize.SelectedIndex = 0;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;

            SetTabOrder();
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
            selectedUnitId = Convert.ToInt32(row.Cells["id"].Value);
            txtUnitName.Text = row.Cells["name"].Value.ToString();
            txtAbbr.Text = row.Cells["abbr"].Value.ToString();

            btnAdd.Enabled = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        private DataTable ToDataTable(List<Unit> units)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("abbr", typeof(string));
            dt.Columns.Add("created_at", typeof(DateTime));
            dt.Columns.Add("updated_at", typeof(DateTime));

            foreach (var u in units)
            {
                dt.Rows.Add(u.Id, u.Name, u.Abbr, u.CreatedAt, u.UpdatedAt);
            }

            return dt;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUnitName.Text) ||
                string.IsNullOrWhiteSpace(txtAbbr.Text))
            {
                MessageBox.Show("Nama Unit dan Deskripsi wajib diisi");
                return;
            }
            var unit = new Unit
            {
                Name = txtUnitName.Text,
                Abbr = txtAbbr.Text
            };

            if (unitController.AddUnit(unit))
            {
                MessageBox.Show("Unit berhasil ditambahkan");
                LoadUnitsData();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Gagal menambahkan unit");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedUnitId == 0)
            {
                MessageBox.Show("Pilih unit terlebih dahulu");
                return;
            }

            var unit = new Unit
            {
                Id = selectedUnitId,
                Name = txtUnitName.Text,
                Abbr = txtAbbr.Text
            };

            if (unitController.UpdateUnit(unit))
            {
                MessageBox.Show("Unit berhasil diupdate");
                LoadUnitsData();
                ClearForm();
            }

            else
            {
                MessageBox.Show("Gagal update unit");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUnitId == 0)
            {
                MessageBox.Show("Pilih unit terlebih dahulu");
                return;
            }

            if (MessageBox.Show(
                "Yakin ingin menghapus unit ini?",
                "Konfirmasi",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (unitController.DeleteUnit(selectedUnitId))
                {
                    MessageBox.Show("Unit berhasil dihapus");
                    LoadUnitsData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Gagal menghapus unit");
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUnitsData();
            ClearForm();
        }

        private void btnFirst_Click(object sender, EventArgs e) => dgvManager.FirstPage();
        private void btnPrev_Click(object sender, EventArgs e) => dgvManager.PreviousPage();
        private void btnNext_Click(object sender, EventArgs e) => dgvManager.NextPage();
        private void btnLast_Click(object sender, EventArgs e) => dgvManager.LastPage();

        private void ClearForm()
        {
            txtUnitName.Clear();
            txtAbbr.Clear();
            selectedUnitId = 0;
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void SetTabOrder()
        {
            txtUnitName.TabIndex = 0;
            txtAbbr.TabIndex = 1;
            btnAdd.TabIndex = 2;
            btnEdit.TabIndex = 3;
            btnDelete.TabIndex = 4;
            txtSearch.TabIndex = 5;
            dgvUnits.TabIndex = 6;
        }

 
    }
}
