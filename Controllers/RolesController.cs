using Npgsql;
using System;
using System.Data;
using POS_qu.Models;

namespace POS_qu.Controllers
{
    class RolesController
    {
        private string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";

        public DataTable GetAllRoles()
        {
            DataTable dt = new DataTable();

            using (var conn = new NpgsqlConnection(vStrConnection))
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

        public bool AddRole(string name, string description)
        {
            using (var conn = new NpgsqlConnection(vStrConnection))
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
            using (var conn = new NpgsqlConnection(vStrConnection))
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
            using (var conn = new NpgsqlConnection(vStrConnection))
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
