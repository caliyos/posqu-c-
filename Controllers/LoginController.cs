using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_qu.Models;

namespace POS_qu.Controllers
{
    class LoginController
    {
        private string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";

        public UserModel AuthenticateUser(string username, string password)
        {
            UserModel user = null;

            using (var conn = new NpgsqlConnection(vStrConnection))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT id, username, password_hash, role_id, terminal_id FROM users WHERE username = @username LIMIT 1";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbPasswordHash = reader.GetString(reader.GetOrdinal("password_hash"));

                                if (VerifyPassword(password, dbPasswordHash))
                                {
                                    user = new UserModel
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                                        Username = reader.GetString(reader.GetOrdinal("username")),
                                        RoleId = reader.GetInt32(reader.GetOrdinal("role_id")),
                                        TerminalId = reader.IsDBNull(reader.GetOrdinal("terminal_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("terminal_id")),
                                       // update ke 3 shift berdasarkan waktu
                                       // nanti tnya ke chtgpt
                                        ShiftId = 1
                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return user;
        }

        private bool VerifyPassword(string inputPassword, string dbPasswordHash)
        {
            // Sementara plain-text, sebaiknya pakai SHA256 atau bcrypt di produksi
            return inputPassword == dbPasswordHash;
        }
    }
}
