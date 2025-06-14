using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace POS_qu
{
    public partial class Roles : Form
    {
        public Roles()
        {
            InitializeComponent();

        }

        private DataGridViewManager dgvManager;
        private RolesController rolesController = new RolesController();
        private int selectedRoleId = 0; // To keep track of selected role ID for editing

        private void LoadRolesData()
        {
            // DataGridView setup
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.ReadOnly = true;

            // DataGridView Manager setup
            var rolesDataTable = rolesController.GetAllRoles();
            dgvManager = new DataGridViewManager(dgvRoles, rolesDataTable, 10);
            dgvManager.PagingInfoLabel = lblPageNumber;

            // Bind buttons to paging methods
            // btnNext.Click += btnNext_Click; (if paging is used)
            dgvRoles.CellClick += dgvRoles_CellClick;  // Event Binding
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvManager.Filter("", "name");

            // Dropdown page size
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            cmbPageSize.SelectedIndex = 0;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            SetTabOrder();
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedSize = Convert.ToInt32(cmbPageSize.SelectedItem);
            dgvManager.SetPageSize(selectedSize);
        }


        private void Roles_Load(object sender, EventArgs e)
        {
            LoadRolesData();
        }

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            if (rowIndex >= 0) // Ensure that the clicked row is valid
            {
                DataGridViewRow selectedRow = dgvRoles.Rows[rowIndex];
                selectedRoleId = Convert.ToInt32(selectedRow.Cells["id"].Value);
                txtRoleName.Text = selectedRow.Cells["name"].Value.ToString();
                txtDescription.Text = selectedRow.Cells["description"].Value.ToString();

                // Disable Add button and enable Edit/Delete buttons
                btnAdd.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvManager.Filter(txtSearch.Text, "name");
        }

        private void btnFirst_Click(object sender, EventArgs e) => dgvManager.FirstPage();
        private void btnPrev_Click(object sender, EventArgs e) => dgvManager.PreviousPage();
        private void btnNext_Click(object sender, EventArgs e) => dgvManager.NextPage();
        private void btnLast_Click(object sender, EventArgs e) => dgvManager.LastPage();

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string name = txtRoleName.Text;
            string description = txtDescription.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please enter both Name and Description.");
                return;
            }

            if (rolesController.AddRole(name, description))
            {
                MessageBox.Show("Role added successfully.");
                LoadRolesData(); // Refresh the data grid
                ClearForm(); // Clear form fields
            }
            else
            {
                MessageBox.Show("Failed to add role.");
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (selectedRoleId == 0)
            {
                MessageBox.Show("No role selected for editing.");
                return;
            }

            string name = txtRoleName.Text;
            string description = txtDescription.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please enter both Name and Description.");
                return;
            }

            if (rolesController.UpdateRole(selectedRoleId, name, description))
            {
                MessageBox.Show("Role updated successfully.");
                LoadRolesData(); // Refresh the data grid
                ClearForm(); // Clear form fields
                btnAdd.Enabled = true; // Enable Add button
                btnEdit.Enabled = false; // Disable Edit button after operation
                btnDelete.Enabled = false; // Disable Delete button after operation
            }
            else
            {
                MessageBox.Show("Failed to update role.");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRoleId == 0)
            {
                MessageBox.Show("No role selected for deletion.");
                return;
            }

            if (rolesController.DeleteRole(selectedRoleId))
            {
                MessageBox.Show("Role deleted successfully.");
                LoadRolesData(); // Refresh the data grid
                ClearForm(); // Clear form fields
                btnAdd.Enabled = true; // Enable Add button
                btnEdit.Enabled = false; // Disable Edit button
                btnDelete.Enabled = false; // Disable Delete button
            }
            else
            {
                MessageBox.Show("Failed to delete role.");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadRolesData(); // Refresh the data grid
            ClearForm(); // Clear form fields
        }

        private void ClearForm()
        {
            txtRoleName.Clear();
            txtDescription.Clear();
            selectedRoleId = 0;
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void SetTabOrder()
        {
            txtRoleName.TabIndex = 0;
            txtDescription.TabIndex = 1;
            btnAdd.TabIndex = 2;
            btnEdit.TabIndex = 3;
            btnDelete.TabIndex = 4;
            txtSearch.TabIndex = 5;
            dgvRoles.TabIndex = 6;
        }
    }
}
