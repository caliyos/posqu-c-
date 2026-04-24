using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class Roles : Form
    {
        private DataGridViewManager _rolesGridManager;
        private readonly RolesController _controller = new RolesController();

        private int _selectedRoleId;
        private int _selectedPermissionId;

        private DataTable _dtRoles;
        private DataTable _dtRolePermissions;
        private DataTable _dtRoleUsers;

        public Roles()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;

            btnFirst.Click += (s, e) => _rolesGridManager?.FirstPage();
            btnPrev.Click += (s, e) => _rolesGridManager?.PreviousPage();
            btnNext.Click += (s, e) => _rolesGridManager?.NextPage();
            btnLast.Click += (s, e) => _rolesGridManager?.LastPage();
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;

            dgvRoles.CellClick += dgvRoles_CellClick;

            dgvPermissions.CellClick += dgvPermissions_CellClick;
            dgvPermissions.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvPermissions.IsCurrentCellDirty)
                    dgvPermissions.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            dgvUsers.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvUsers.IsCurrentCellDirty)
                    dgvUsers.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            txtPermSearch.TextChanged += (s, e) => ApplyPermissionsFilter();
            txtUserSearch.TextChanged += (s, e) => ApplyUsersFilter();

            btnPermAdd.Click += BtnPermAdd_Click;
            btnPermEdit.Click += BtnPermEdit_Click;
            btnPermDelete.Click += BtnPermDelete_Click;
            btnPermRefresh.Click += (s, e) => ReloadPermissionsSection();
            btnSaveRolePermissions.Click += BtnSaveRolePermissions_Click;

            btnUsersRefresh.Click += (s, e) => ReloadUsersSection();
            btnSaveRoleUsers.Click += BtnSaveRoleUsers_Click;
        }

        private void Roles_Load(object sender, EventArgs e)
        {
            ApplyGridStyle(dgvRoles);
            ApplyGridStyle(dgvPermissions);
            ApplyGridStyle(dgvUsers);

            cmbPageSize.Items.Clear();
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            if (cmbPageSize.Items.Count > 0) cmbPageSize.SelectedIndex = 0;

            ReloadRoles();
            ReloadPermissionsSection();
            ReloadUsersSection();

            ClearRoleForm();
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_rolesGridManager == null) return;
            if (cmbPageSize.SelectedItem == null) return;
            int selectedSize = Convert.ToInt32(cmbPageSize.SelectedItem);
            _rolesGridManager.SetPageSize(selectedSize);
        }

        private void ReloadRoles()
        {
            _dtRoles = _controller.GetAllRoles();
            if (_rolesGridManager == null)
            {
                _rolesGridManager = new DataGridViewManager(dgvRoles, _dtRoles, 10);
                _rolesGridManager.PagingInfoLabel = lblPageNumber;
            }
            else
            {
                _rolesGridManager.reloadData(_dtRoles);
            }

            _rolesGridManager.Filter(txtSearch.Text, "name");

            if (dgvRoles.Columns.Contains("id")) dgvRoles.Columns["id"].Visible = false;
            if (dgvRoles.Columns.Contains("name")) dgvRoles.Columns["name"].HeaderText = "Role";
            if (dgvRoles.Columns.Contains("description")) dgvRoles.Columns["description"].HeaderText = "Deskripsi";

            if (dgvRoles.Rows.Count > 0)
            {
                dgvRoles.Rows[0].Selected = true;
                SelectRoleFromRow(dgvRoles.Rows[0]);
            }
        }

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            SelectRoleFromRow(dgvRoles.Rows[e.RowIndex]);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _rolesGridManager?.Filter(txtSearch.Text, "name");
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string name = (txtRoleName.Text ?? "").Trim();
            string description = (txtDescription.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Role name wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_controller.AddRole(name, description))
            {
                MessageBox.Show("Role berhasil ditambah.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadRoles();
                ClearRoleForm();
            }
            else
            {
                MessageBox.Show("Gagal menambah role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedRoleId == 0)
            {
                MessageBox.Show("Pilih role dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = (txtRoleName.Text ?? "").Trim();
            string description = (txtDescription.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Role name wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_controller.UpdateRole(_selectedRoleId, name, description))
            {
                MessageBox.Show("Role berhasil diubah.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadRoles();
            }
            else
            {
                MessageBox.Show("Gagal mengubah role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedRoleId == 0)
            {
                MessageBox.Show("Pilih role dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("Hapus role ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            if (_controller.DeleteRole(_selectedRoleId))
            {
                MessageBox.Show("Role berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadRoles();
                ClearRoleForm();
            }
            else
            {
                MessageBox.Show("Gagal menghapus role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ReloadRoles();
            ReloadPermissionsSection();
            ReloadUsersSection();
            ClearRoleForm();
        }

        private void SelectRoleFromRow(DataGridViewRow row)
        {
            if (row == null) return;
            if (!dgvRoles.Columns.Contains("id")) return;

            int roleId = 0;
            if (row.Cells["id"].Value != null && row.Cells["id"].Value != DBNull.Value)
                roleId = Convert.ToInt32(row.Cells["id"].Value);

            _selectedRoleId = roleId;
            txtRoleName.Text = row.Cells["name"].Value?.ToString() ?? "";
            txtDescription.Text = row.Cells["description"].Value?.ToString() ?? "";

            btnAdd.Enabled = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;

            ReloadPermissionsSection();
            ReloadUsersSection();
        }

        private void ClearRoleForm()
        {
            txtRoleName.Text = "";
            txtDescription.Text = "";
            _selectedRoleId = 0;
            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void ReloadPermissionsSection()
        {
            if (_selectedRoleId > 0)
                _dtRolePermissions = _controller.GetPermissionsForRole(_selectedRoleId);
            else
            {
                _dtRolePermissions = _controller.GetAllPermissions();
                if (!_dtRolePermissions.Columns.Contains("assigned"))
                    _dtRolePermissions.Columns.Add("assigned", typeof(bool));
            }

            dgvPermissions.DataSource = _dtRolePermissions;

            if (dgvPermissions.Columns.Contains("id")) dgvPermissions.Columns["id"].Visible = false;
            if (dgvPermissions.Columns.Contains("assigned"))
            {
                dgvPermissions.Columns["assigned"].HeaderText = "Assign";
                dgvPermissions.Columns["assigned"].Width = 80;
            }
            if (dgvPermissions.Columns.Contains("name"))
            {
                dgvPermissions.Columns["name"].HeaderText = "Nama";
                dgvPermissions.Columns["name"].Width = 220;
            }
            if (dgvPermissions.Columns.Contains("description"))
            {
                dgvPermissions.Columns["description"].HeaderText = "Deskripsi";
                dgvPermissions.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            ApplyPermissionsFilter();
        }

        private void ApplyPermissionsFilter()
        {
            if (_dtRolePermissions == null) return;
            string q = (txtPermSearch.Text ?? "").Trim().Replace("'", "''");
            if (string.IsNullOrWhiteSpace(q))
            {
                _dtRolePermissions.DefaultView.RowFilter = "";
            }
            else
            {
                _dtRolePermissions.DefaultView.RowFilter = $"name LIKE '%{q}%' OR description LIKE '%{q}%'";
            }
        }

        private void dgvPermissions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPermissions.Rows[e.RowIndex];
            if (row.Cells["id"]?.Value == null || row.Cells["id"].Value == DBNull.Value) return;
            _selectedPermissionId = Convert.ToInt32(row.Cells["id"].Value);
            txtPermName.Text = row.Cells["name"]?.Value?.ToString() ?? "";
            txtPermDesc.Text = row.Cells["description"]?.Value?.ToString() ?? "";
        }

        private void BtnPermAdd_Click(object sender, EventArgs e)
        {
            string name = (txtPermName.Text ?? "").Trim();
            string desc = (txtPermDesc.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Nama permission wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_controller.AddPermission(name, desc))
            {
                ReloadPermissionsSection();
                txtPermName.Text = "";
                txtPermDesc.Text = "";
                _selectedPermissionId = 0;
            }
            else
            {
                MessageBox.Show("Gagal menambah permission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPermEdit_Click(object sender, EventArgs e)
        {
            if (_selectedPermissionId <= 0)
            {
                MessageBox.Show("Pilih permission dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = (txtPermName.Text ?? "").Trim();
            string desc = (txtPermDesc.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Nama permission wajib diisi.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_controller.UpdatePermission(_selectedPermissionId, name, desc))
            {
                ReloadPermissionsSection();
            }
            else
            {
                MessageBox.Show("Gagal mengubah permission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPermDelete_Click(object sender, EventArgs e)
        {
            if (_selectedPermissionId <= 0)
            {
                MessageBox.Show("Pilih permission dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("Hapus permission ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            if (_controller.DeletePermission(_selectedPermissionId))
            {
                _selectedPermissionId = 0;
                txtPermName.Text = "";
                txtPermDesc.Text = "";
                ReloadPermissionsSection();
            }
            else
            {
                MessageBox.Show("Gagal menghapus permission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveRolePermissions_Click(object sender, EventArgs e)
        {
            if (_selectedRoleId <= 0)
            {
                MessageBox.Show("Pilih role dulu untuk assign permission.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_dtRolePermissions == null || !_dtRolePermissions.Columns.Contains("assigned"))
            {
                MessageBox.Show("Data permission belum siap.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ids = _dtRolePermissions.AsEnumerable()
                .Where(r => r["assigned"] != DBNull.Value && Convert.ToBoolean(r["assigned"]))
                .Select(r => Convert.ToInt32(r["id"]))
                .Distinct()
                .ToArray();

            if (_controller.SaveRolePermissions(_selectedRoleId, ids))
            {
                MessageBox.Show("Assign permission berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Gagal simpan assign permission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadUsersSection()
        {
            if (_selectedRoleId > 0)
                _dtRoleUsers = _controller.GetUsersForRole(_selectedRoleId);
            else
            {
                _dtRoleUsers = _controller.GetAllUsers();
                if (!_dtRoleUsers.Columns.Contains("assigned"))
                    _dtRoleUsers.Columns.Add("assigned", typeof(bool));
            }

            dgvUsers.DataSource = _dtRoleUsers;

            if (dgvUsers.Columns.Contains("id")) dgvUsers.Columns["id"].Visible = false;
            if (dgvUsers.Columns.Contains("assigned"))
            {
                dgvUsers.Columns["assigned"].HeaderText = "Assign";
                dgvUsers.Columns["assigned"].Width = 80;
            }
            if (dgvUsers.Columns.Contains("name"))
            {
                dgvUsers.Columns["name"].HeaderText = "Nama";
                dgvUsers.Columns["name"].Width = 220;
            }
            if (dgvUsers.Columns.Contains("username"))
            {
                dgvUsers.Columns["username"].HeaderText = "Username";
                dgvUsers.Columns["username"].Width = 170;
            }
            if (dgvUsers.Columns.Contains("email"))
            {
                dgvUsers.Columns["email"].HeaderText = "Email";
                dgvUsers.Columns["email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            ApplyUsersFilter();
        }

        private void ApplyUsersFilter()
        {
            if (_dtRoleUsers == null) return;
            string q = (txtUserSearch.Text ?? "").Trim().Replace("'", "''");
            if (string.IsNullOrWhiteSpace(q))
            {
                _dtRoleUsers.DefaultView.RowFilter = "";
            }
            else
            {
                _dtRoleUsers.DefaultView.RowFilter = $"name LIKE '%{q}%' OR username LIKE '%{q}%' OR email LIKE '%{q}%'";
            }
        }

        private void BtnSaveRoleUsers_Click(object sender, EventArgs e)
        {
            if (_selectedRoleId <= 0)
            {
                MessageBox.Show("Pilih role dulu untuk assign user.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_dtRoleUsers == null || !_dtRoleUsers.Columns.Contains("assigned"))
            {
                MessageBox.Show("Data user belum siap.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ids = _dtRoleUsers.AsEnumerable()
                .Where(r => r["assigned"] != DBNull.Value && Convert.ToBoolean(r["assigned"]))
                .Select(r => Convert.ToInt32(r["id"]))
                .Distinct()
                .ToArray();

            if (_controller.SaveRoleUsers(_selectedRoleId, ids))
            {
                MessageBox.Show("Assign role ke user berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Gagal simpan assign role ke user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ApplyGridStyle(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.RowsDefaultCellStyle.Padding = new Padding(5);
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgv.RowTemplate.Height = 40;
        }
    }
}
