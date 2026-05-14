using Npgsql;
using System;
using System.Data;
using POS_qu.Models;
using POS_qu.Helpers;

namespace POS_qu.Controllers
{
    class RolesController
    {
        public DataTable GetAllRoles()
        {
            DataTable dt = new DataTable();

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT id, name, description FROM roles ORDER BY id ASC";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable GetAllPermissions()
        {
            var dt = new DataTable();
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT id, name, description FROM permissions ORDER BY id ASC";
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public bool AddPermission(string name, string description)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "INSERT INTO permissions (name, description) VALUES (@name, @description)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description ?? "");
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdatePermission(int id, string name, string description)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "UPDATE permissions SET name = @name, description = @description WHERE id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description ?? "");
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeletePermission(int id)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "DELETE FROM permissions WHERE id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public DataTable GetPermissionsForRole(int roleId)
        {
            var dt = new DataTable();
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = @"
SELECT
    p.id,
    p.name,
    p.description,
    CASE WHEN pr.role_id IS NULL THEN FALSE ELSE TRUE END AS assigned
FROM permissions p
LEFT JOIN permission_role pr
    ON pr.permission_id = p.id AND pr.role_id = @rid
ORDER BY p.name ASC";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@rid", roleId);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public bool SaveRolePermissions(int roleId, int[] permissionIds)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var del = new NpgsqlCommand("DELETE FROM permission_role WHERE role_id = @rid", conn, tran))
                        {
                            del.Parameters.AddWithValue("@rid", roleId);
                            del.ExecuteNonQuery();
                        }

                        if (permissionIds != null)
                        {
                            foreach (var pid in permissionIds)
                            {
                                using (var ins = new NpgsqlCommand("INSERT INTO permission_role (permission_id, role_id) VALUES (@pid, @rid)", conn, tran))
                                {
                                    ins.Parameters.AddWithValue("@pid", pid);
                                    ins.Parameters.AddWithValue("@rid", roleId);
                                    ins.ExecuteNonQuery();
                                }
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        public DataTable GetAllUsers()
        {
            var dt = new DataTable();
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT id, name, username, email FROM users ORDER BY id ASC";
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable GetUsersForRole(int roleId)
        {
            var dt = new DataTable();
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = @"
SELECT
    u.id,
    u.name,
    u.username,
    u.email,
    CASE WHEN ru.role_id IS NULL THEN FALSE ELSE TRUE END AS assigned
FROM users u
LEFT JOIN role_user ru
    ON ru.user_id = u.id AND ru.role_id = @rid
ORDER BY u.id ASC";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@rid", roleId);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public bool SaveRoleUsers(int roleId, int[] userIds)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var del = new NpgsqlCommand("DELETE FROM role_user WHERE role_id = @rid", conn, tran))
                        {
                            del.Parameters.AddWithValue("@rid", roleId);
                            del.ExecuteNonQuery();
                        }

                        if (userIds != null)
                        {
                            foreach (var uid in userIds)
                            {
                                using (var ins = new NpgsqlCommand("INSERT INTO role_user (user_id, role_id) VALUES (@uid, @rid)", conn, tran))
                                {
                                    ins.Parameters.AddWithValue("@uid", uid);
                                    ins.Parameters.AddWithValue("@rid", roleId);
                                    ins.ExecuteNonQuery();
                                }
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool AddRole(string name, string description)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "INSERT INTO roles (name, description) VALUES (@name, @description)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateRole(int id, string name, string description)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "UPDATE roles SET name = @name, description = @description WHERE id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteRole(int id)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "DELETE FROM roles WHERE id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public DataTable GetSupervisorApprovalSettings()
        {
            var dt = new DataTable();
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS supervisor_approval_settings (
    action_code VARCHAR(64) PRIMARY KEY,
    action_name VARCHAR(120) NOT NULL,
    is_enabled BOOLEAN NOT NULL DEFAULT TRUE,
    require_for_cashier BOOLEAN NOT NULL DEFAULT TRUE,
    min_amount NUMERIC(18,2) NULL,
    require_reason BOOLEAN NOT NULL DEFAULT FALSE,
    sort_order INT NOT NULL DEFAULT 0,
    updated_at TIMESTAMP NOT NULL DEFAULT now()
);", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var seed = new NpgsqlCommand(@"
INSERT INTO supervisor_approval_settings(action_code, action_name, is_enabled, require_for_cashier, min_amount, require_reason, sort_order)
VALUES
    ('close_shift',      'Tutup Shift',                    TRUE, TRUE, NULL,     FALSE, 10),
    ('reprint_receipt',  'Reprint Struk / Invoice',        TRUE, TRUE, NULL,     FALSE, 20),
    ('delete_item',      'Hapus Item (Void Item)',         TRUE, TRUE, 100000,   TRUE,  30),
    ('void_transaction', 'Batal Seluruh Transaksi (Void)', TRUE, TRUE, NULL,     TRUE,  40),
    ('manual_discount',  'Diskon Manual',                  FALSE, TRUE, 0,       TRUE,  50),
    ('manual_price',     'Ubah Harga Manual',              FALSE, TRUE, 0,       TRUE,  60),
    ('refund',           'Refund / Retur',                 FALSE, TRUE, 0,       TRUE,  70)
ON CONFLICT (action_code) DO NOTHING;
", conn))
                {
                    seed.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand(@"
SELECT
    action_code,
    action_name,
    is_enabled,
    require_for_cashier,
    min_amount,
    require_reason,
    sort_order
FROM supervisor_approval_settings
ORDER BY sort_order ASC, action_name ASC
", conn))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public bool SaveSupervisorApprovalSettings(DataTable settings)
        {
            if (settings == null) return false;

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow r in settings.Rows)
                        {
                            if (r.RowState == DataRowState.Deleted) continue;

                            string code = r["action_code"]?.ToString() ?? "";
                            string name = r["action_name"]?.ToString() ?? "";
                            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name)) continue;

                            bool enabled = r["is_enabled"] != DBNull.Value && Convert.ToBoolean(r["is_enabled"]);
                            bool requireCashier = r["require_for_cashier"] != DBNull.Value && Convert.ToBoolean(r["require_for_cashier"]);
                            bool requireReason = r.Table.Columns.Contains("require_reason") && r["require_reason"] != DBNull.Value && Convert.ToBoolean(r["require_reason"]);
                            int sortOrder = r.Table.Columns.Contains("sort_order") && r["sort_order"] != DBNull.Value ? Convert.ToInt32(r["sort_order"]) : 0;

                            object minAmountObj = DBNull.Value;
                            if (r.Table.Columns.Contains("min_amount") && r["min_amount"] != DBNull.Value)
                            {
                                if (decimal.TryParse(r["min_amount"].ToString(), out var min))
                                    minAmountObj = min;
                            }

                            using (var cmd = new NpgsqlCommand(@"
INSERT INTO supervisor_approval_settings(action_code, action_name, is_enabled, require_for_cashier, min_amount, require_reason, sort_order, updated_at)
VALUES(@code, @name, @en, @rc, @min, @rr, @so, NOW())
ON CONFLICT (action_code)
DO UPDATE SET
    action_name = EXCLUDED.action_name,
    is_enabled = EXCLUDED.is_enabled,
    require_for_cashier = EXCLUDED.require_for_cashier,
    min_amount = EXCLUDED.min_amount,
    require_reason = EXCLUDED.require_reason,
    sort_order = EXCLUDED.sort_order,
    updated_at = NOW();
", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@code", code);
                                cmd.Parameters.AddWithValue("@name", name);
                                cmd.Parameters.AddWithValue("@en", enabled);
                                cmd.Parameters.AddWithValue("@rc", requireCashier);
                                cmd.Parameters.AddWithValue("@min", minAmountObj);
                                cmd.Parameters.AddWithValue("@rr", requireReason);
                                cmd.Parameters.AddWithValue("@so", sortOrder);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}
