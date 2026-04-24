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
    }
}
