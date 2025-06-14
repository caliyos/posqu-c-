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
    public partial class Terminal : Form
    {
        public Terminal()
        {
            InitializeComponent();
        }

        private DataGridViewManager dgvManager;
        private TerminalController terminalController = new TerminalController();
        private int selectedId = 0; // To keep track of selected  ID for editing

        private void LoadData()
        {
            // DataGridView setup
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;

            // DataGridView Manager setup
            var terminalDataTable = terminalController.GetAllTerminals();
            dgvManager = new DataGridViewManager(dgv, terminalDataTable, 10);
            dgvManager.PagingInfoLabel = lblPageNumber;

            // Bind buttons to paging methods
             //btnNext.Click += btnNext_Click; (if paging is used)
                dgv.CellClick += dgvRoles_CellClick;  // Event Binding
                txtSearch.TextChanged += txtSearch_TextChanged;
            dgvManager.Filter("", "terminal_name");

            // Dropdown page size
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            cmbPageSize.SelectedIndex = 0;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            SetTabOrder();
        }

        private void Terminal_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            if (rowIndex >= 0) // Ensure that the clicked row is valid
            {
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                selectedId = Convert.ToInt32(selectedRow.Cells["id"].Value);
                txtTerminalName.Text = selectedRow.Cells["terminal_name"].Value.ToString();
                txtDesc.Text = selectedRow.Cells["description"].Value.ToString();

                // Disable Add button and enable Edit/Delete buttons
                btnAdd.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvManager.Filter(txtSearch.Text, "terminal_name");
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData(); // Refresh the data grid
            ClearForm(); // Clear form fields
        }

        private void ClearForm()
        {
            txtTerminalName.Clear();
            txtDesc.Clear();
            selectedId = 0;
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedSize = Convert.ToInt32(cmbPageSize.SelectedItem);
            dgvManager.SetPageSize(selectedSize);
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            if (rowIndex >= 0) // Ensure that the clicked row is valid
            {
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                selectedId = Convert.ToInt32(selectedRow.Cells["id"].Value);
                txtTerminalName.Text = selectedRow.Cells["name"].Value.ToString();
                txtDesc.Text = selectedRow.Cells["description"].Value.ToString();

                // Disable Add button and enable Edit/Delete buttons
                btnAdd.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void SetTabOrder()
        {
            txtTerminalName.TabIndex = 0;
            txtDesc.TabIndex = 1;
            btnAdd.TabIndex = 2;
            btnEdit.TabIndex = 3;
            btnDelete.TabIndex = 4;
            txtSearch.TabIndex = 5;
            dgv.TabIndex = 6;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblPageNumber_Click(object sender, EventArgs e)
        {

        }
    }
}
