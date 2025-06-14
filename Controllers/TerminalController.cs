using Npgsql;
using System;
using System.Data;
using POS_qu.Models;
using System.Xml.Linq;

namespace POS_qu.Controllers
{
    class TerminalController
    {
        private string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";

        public DataTable GetAllTerminals()
        {
            DataTable dt = new DataTable();

            using (var conn = new NpgsqlConnection(vStrConnection))
            {
                conn.Open();
                string query = "SELECT id, terminal_name, pc_id,description FROM terminals ORDER BY id ASC";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public bool AddTerminal(string terminalname, string pc_id ,string description)
        {
            using (var conn = new NpgsqlConnection(vStrConnection))
            {
                conn.Open();
                string query = "INSERT INTO terminals (terminal_name,pc_id, description) VALUES (@terminal_name,@pc_id, @description)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@terminal_name", terminalname);
                    cmd.Parameters.AddWithValue("@pc_id", pc_id);
                    cmd.Parameters.AddWithValue("@description", description);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateRole(int id, string terminal_name, string pc_id, string description)
        {
            using (var conn = new NpgsqlConnection(vStrConnection))
            {
                conn.Open();
                string query = "UPDATE terminals SET terminal_name = @terminal_name, pc_id = @pc_id, description = @description WHERE id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@terminal_name", terminal_name);
                    cmd.Parameters.AddWithValue("@pc_id", pc_id);
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
                string query = "DELETE FROM terminals WHERE id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
