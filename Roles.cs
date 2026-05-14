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
        private DataTable _dtApprovalSettings;

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
            btnSetUserPin.Click += BtnSetUserPin_Click;

            dgvApproval.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvApproval.IsCurrentCellDirty)
                    dgvApproval.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            txtApprovalSearch.TextChanged += (s, e) => ApplyApprovalFilter();
            btnApprovalRefresh.Click += (s, e) => ReloadApprovalSettings();
            btnSaveApprovalSettings.Click += BtnSaveApprovalSettings_Click;
        }

        private void BtnSetUserPin_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.CurrentRow == null)
                {
                    MessageBox.Show("Pilih user dulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int userId = 0;
                string username = "";
                string name = "";
                bool assigned = false;

                if (dgvUsers.CurrentRow.DataBoundItem is DataRowView rv)
                {
                    if (rv.Row.Table.Columns.Contains("id") && rv["id"] != DBNull.Value) userId = Convert.ToInt32(rv["id"]);
                    if (rv.Row.Table.Columns.Contains("username")) username = rv["username"]?.ToString() ?? "";
                    if (rv.Row.Table.Columns.Contains("name")) name = rv["name"]?.ToString() ?? "";
                    if (rv.Row.Table.Columns.Contains("assigned") && rv["assigned"] != DBNull.Value) assigned = Convert.ToBoolean(rv["assigned"]);
                }
                else
                {
                    if (dgvUsers.Columns.Contains("id") && dgvUsers.CurrentRow.Cells["id"].Value != null && dgvUsers.CurrentRow.Cells["id"].Value != DBNull.Value)
                        userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["id"].Value);
                    if (dgvUsers.Columns.Contains("username")) username = dgvUsers.CurrentRow.Cells["username"].Value?.ToString() ?? "";
                    if (dgvUsers.Columns.Contains("name")) name = dgvUsers.CurrentRow.Cells["name"].Value?.ToString() ?? "";
                    if (dgvUsers.Columns.Contains("assigned") && dgvUsers.CurrentRow.Cells["assigned"].Value != null && dgvUsers.CurrentRow.Cells["assigned"].Value != DBNull.Value)
                        assigned = Convert.ToBoolean(dgvUsers.CurrentRow.Cells["assigned"].Value);
                }

                if (userId <= 0)
                {
                    MessageBox.Show("UserId tidak valid.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_selectedRoleId == 3 && !assigned)
                {
                    MessageBox.Show("User ini belum di-assign ke role Supervisor.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using var modal = new Form
                {
                    Text = "Set PIN Supervisor",
                    StartPosition = FormStartPosition.CenterParent,
                    Size = new Size(460, 300),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    Padding = new Padding(16),
                    KeyPreview = true
                };

                var lbl = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 42,
                    Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                    Text = $"User: {(!string.IsNullOrWhiteSpace(username) ? username : name)}"
                };

                var pnl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Padding = new Padding(0, 8, 0, 0) };
                pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
                pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                pnl.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
                pnl.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
                pnl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                var lblPin1 = new Label { Text = "PIN", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                var txtPin1 = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 16F, FontStyle.Bold), UseSystemPasswordChar = true, MaxLength = 8 };
                var lblPin2 = new Label { Text = "Ulangi", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
                var txtPin2 = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 16F, FontStyle.Bold), UseSystemPasswordChar = true, MaxLength = 8 };
                var lblHint = new Label { Text = "PIN angka 4-6 digit", Dock = DockStyle.Fill, ForeColor = Color.DimGray };

                void digitsOnly(object? s, KeyPressEventArgs e)
                {
                    if (char.IsControl(e.KeyChar)) return;
                    if (!char.IsDigit(e.KeyChar)) e.Handled = true;
                }
                txtPin1.KeyPress += digitsOnly;
                txtPin2.KeyPress += digitsOnly;

                pnl.Controls.Add(lblPin1, 0, 0);
                pnl.Controls.Add(txtPin1, 1, 0);
                pnl.Controls.Add(lblPin2, 0, 1);
                pnl.Controls.Add(txtPin2, 1, 1);
                pnl.Controls.Add(lblHint, 1, 2);

                var footer = new Panel { Dock = DockStyle.Bottom, Height = 56 };
                var btnCancel = new Button { Text = "Batal (Esc)", Dock = DockStyle.Right, Width = 120 };
                var btnSave = new Button { Text = "Simpan", Dock = DockStyle.Right, Width = 120, BackColor = Color.FromArgb(22, 163, 74), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
                btnSave.FlatAppearance.BorderSize = 0;
                footer.Controls.Add(btnCancel);
                footer.Controls.Add(btnSave);

                btnCancel.Click += (_, __) => modal.Close();
                btnSave.Click += (_, __) =>
                {
                    string p1 = (txtPin1.Text ?? "").Trim();
                    string p2 = (txtPin2.Text ?? "").Trim();
                    if (p1.Length < 4 || p1.Length > 6)
                    {
                        MessageBox.Show(modal, "PIN harus 4-6 digit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (p1 != p2)
                    {
                        MessageBox.Show(modal, "PIN tidak sama.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    bool ok = _controller.SetUserPin(userId, p1);
                    if (!ok)
                    {
                        MessageBox.Show(modal, "Gagal menyimpan PIN.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show(modal, "PIN tersimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    modal.Close();
                };

                modal.AcceptButton = btnSave;
                modal.CancelButton = btnCancel;
                modal.Shown += (_, __) => txtPin1.Focus();

                modal.Controls.Add(pnl);
                modal.Controls.Add(footer);
                modal.Controls.Add(lbl);
                modal.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Roles_Load(object sender, EventArgs e)
        {
            ApplyGridStyle(dgvRoles);
            ApplyGridStyle(dgvPermissions);
            ApplyGridStyle(dgvUsers);
            ApplyGridStyle(dgvApproval);

            cmbPageSize.Items.Clear();
            cmbPageSize.Items.AddRange(new object[] { 10, 50, 100, 500 });
            if (cmbPageSize.Items.Count > 0) cmbPageSize.SelectedIndex = 0;

            ReloadRoles();
            ReloadPermissionsSection();
            ReloadUsersSection();
            ReloadApprovalSettings();

            ClearRoleForm();
        }

        private void ReloadApprovalSettings()
        {
            _dtApprovalSettings = _controller.GetSupervisorApprovalSettings();
            dgvApproval.DataSource = _dtApprovalSettings;

            DataGridViewColumn FindCol(string name)
            {
                if (dgvApproval == null || dgvApproval.Columns == null) return null;
                foreach (DataGridViewColumn c in dgvApproval.Columns)
                {
                    if (c == null) continue;
                    if (string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)) return c;
                    if (string.Equals(c.DataPropertyName, name, StringComparison.OrdinalIgnoreCase)) return c;
                }
                return null;
            }

            void ApplyApprovalGridLayout()
            {
                var colActionCode = FindCol("action_code");
                if (colActionCode != null) colActionCode.Visible = false;

                var colSortOrder = FindCol("sort_order");
                if (colSortOrder != null)
                {
                    colSortOrder.HeaderText = "Urut";
                    colSortOrder.Width = 70;
                }

                var colActionName = FindCol("action_name");
                if (colActionName != null && colActionName.DataGridView != null)
                {
                    colActionName.HeaderText = "Aksi";
                    colActionName.ReadOnly = true;
                    colActionName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    colActionName.FillWeight = 350;
                }

                var colEnabled = FindCol("is_enabled");
                if (colEnabled != null)
                {
                    colEnabled.HeaderText = "Aktif";
                    colEnabled.Width = 80;
                }

                var colRequireCashier = FindCol("require_for_cashier");
                if (colRequireCashier != null)
                {
                    colRequireCashier.HeaderText = "Minta Supervisor (Kasir)";
                    colRequireCashier.Width = 190;
                }

                var colMinAmount = FindCol("min_amount");
                if (colMinAmount != null)
                {
                    colMinAmount.HeaderText = "Min Nominal";
                    colMinAmount.Width = 140;
                    colMinAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    colMinAmount.DefaultCellStyle.FormatProvider = UiNumberFormat.DotCulture;
                    colMinAmount.DefaultCellStyle.Format = "N0";
                }

                var colRequireReason = FindCol("require_reason");
                if (colRequireReason != null)
                {
                    colRequireReason.HeaderText = "Wajib Alasan";
                    colRequireReason.Width = 130;
                }
            }

            ApplyApprovalFilter();
            if (IsHandleCreated)
                BeginInvoke((Action)ApplyApprovalGridLayout);
            else
                ApplyApprovalGridLayout();
        }

        private void ApplyApprovalFilter()
        {
            if (_dtApprovalSettings == null) return;
            string q = (txtApprovalSearch.Text ?? "").Trim().Replace("'", "''");
            if (string.IsNullOrWhiteSpace(q))
            {
                _dtApprovalSettings.DefaultView.RowFilter = "";
            }
            else
            {
                _dtApprovalSettings.DefaultView.RowFilter = $"action_name LIKE '%{q}%' OR action_code LIKE '%{q}%'";
            }
        }

        private void BtnSaveApprovalSettings_Click(object sender, EventArgs e)
        {
            try
            {
                dgvApproval.EndEdit();
                _dtApprovalSettings?.AcceptChanges();
                bool ok = _controller.SaveSupervisorApprovalSettings(_dtApprovalSettings);
                if (!ok)
                {
                    MessageBox.Show("Gagal menyimpan pengaturan supervisor approval.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Pengaturan supervisor approval tersimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadApprovalSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
