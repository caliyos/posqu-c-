using Npgsql;
using System;
using POS_qu.Models;
using BCrypt.Net;
using POS_qu.Helpers;
using POS_qu.Core;

namespace POS_qu.Controllers
{

        class LoginController
        {

        private IActivityService activityService;
        private ILogger filelogger = new FileLogger();
        private ILogger dbLogger = new DbLogger();
        public LoginController()
        {
            activityService = new ActivityService(filelogger, dbLogger);
        }

        public UserModel AuthenticateUser(string username, string password)
        {
            UserModel user = null;

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // 🔹 Ambil data user + role + terminal
                    string query = @"
                SELECT 
                    u.id AS user_id,
                    u.username,
                    u.password_hash,
                    r.id AS role_id,
                    r.name AS role_name,
                    t.id AS terminal_id,
                    t.terminal_name
                FROM users u
                LEFT JOIN role_user ru ON ru.user_id = u.id
                LEFT JOIN roles r ON r.id = ru.role_id
                LEFT JOIN user_terminal ut ON ut.user_id = u.id
                LEFT JOIN terminals t ON t.id = ut.terminal_id
                WHERE u.username = @username
                LIMIT 1;
            ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string dbPasswordHash = reader.IsDBNull(reader.GetOrdinal("password_hash"))
                                    ? ""
                                    : reader.GetString(reader.GetOrdinal("password_hash"));

                                if (VerifyPassword(password.Trim(), dbPasswordHash))
                                {
                                    user = new UserModel
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("user_id")),
                                        Username = reader.GetString(reader.GetOrdinal("username")),
                                        RoleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("role_id")),
                                        RoleName = reader.IsDBNull(reader.GetOrdinal("role_name")) ? "" : reader.GetString(reader.GetOrdinal("role_name")),
                                        TerminalId = reader.IsDBNull(reader.GetOrdinal("terminal_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("terminal_id")),
                                        TerminalName = reader.IsDBNull(reader.GetOrdinal("terminal_name")) ? "" : reader.GetString(reader.GetOrdinal("terminal_name")),
                                        ShiftId = 0 // sementara, nanti diupdate
                                    };
                                }
                            }
                        }
                    }

                    // ✅ Tentukan shift berdasarkan jam saat ini
                    if (user != null)
                    {
                        string shiftQuery = @"
                    SELECT id
                    FROM shifts
                    WHERE 
                        (start_time <= end_time AND @now >= start_time AND @now < end_time)
                        OR
                        (start_time > end_time AND (@now >= start_time OR @now < end_time))
                    LIMIT 1;
                ";

                        using (var shiftCmd = new NpgsqlCommand(shiftQuery, conn))
                        {
                            shiftCmd.Parameters.AddWithValue("@now", DateTime.Now.TimeOfDay);
                            object shiftIdObj = shiftCmd.ExecuteScalar();
                            user.ShiftId = shiftIdObj != null ? Convert.ToInt32(shiftIdObj) : 1;
                        }

                        // ✅ Simpan log login dan ambil ID-nya
                        int loginId;
                        using (var logCmd = new NpgsqlCommand(@"
                    INSERT INTO login_logs (user_id, email, ip_address, user_agent, terminal, shift, status)
                    VALUES (@user_id, @email, @ip, @agent, @terminal, @shift, 'sukses')
                    RETURNING id;
                ", conn))
                        {
                            logCmd.Parameters.AddWithValue("@user_id", user.Id);
                            logCmd.Parameters.AddWithValue("@email", user.Username);
                            logCmd.Parameters.AddWithValue("@ip", NetworkHelper.GetLocalIPAddress() ?? "0.0.0.0");
                            logCmd.Parameters.AddWithValue("@agent", GlobalContext.getAppVersion() ?? "PosQu/unknown");
                            logCmd.Parameters.AddWithValue("@terminal", user.TerminalName ?? "PosQu/unknown");
                            logCmd.Parameters.AddWithValue("@shift", user.ShiftId.ToString() ?? "PosQu/unknown");

                            loginId = (int)logCmd.ExecuteScalar(); // ambil id login
                        }

                        // ✅ Tambahkan loginId ke Session
                        SessionUser.CreateSession(
                            user.Id,
                            loginId,  // tambahan baru
                            user.Username,
                            user.RoleId,
                            user.RoleName,
                            user.ShiftId,
                            user.TerminalId,
                            user.TerminalName
                        );

                        // ✅ Log ke activityService
                        activityService.LogAction(
                            userId: user.Id.ToString(),
                            actionType: "Login",
                            referenceId: (int?)loginId, // bisa null tapi diisi dengan login id
                            details: new
                            {
                                username = user.Username,
                                role = user.RoleName,
                                terminal = user.TerminalName,
                                shift = user.ShiftId,
                                status = "Sukses",
                                login_id = loginId
                            }
                        );
                    }
                    else
                    {
                        // ❌ Login gagal
                        using (var logCmd = new NpgsqlCommand(@"
                    INSERT INTO login_logs (email, ip_address, user_agent, status)
                    VALUES (@email, @ip, @agent, 'gagal');
                ", conn))
                        {
                            logCmd.Parameters.AddWithValue("@email", username);
                            logCmd.Parameters.AddWithValue("@ip", NetworkHelper.GetLocalIPAddress() ?? "0.0.0.0");
                            logCmd.Parameters.AddWithValue("@agent", GlobalContext.getAppVersion() ?? "PosQu/unknown");
                            logCmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error ke login_logs juga
                    using (var conn2 = new NpgsqlConnection(DbConfig.ConnectionString))
                    {
                        conn2.Open();
                        using (var logCmd = new NpgsqlCommand(@"
                    INSERT INTO login_logs (email, ip_address, user_agent, status)
                    VALUES (@email, @ip, @agent, 'error');
                ", conn2))
                        {
                            logCmd.Parameters.AddWithValue("@email", username);
                            logCmd.Parameters.AddWithValue("@ip", NetworkHelper.GetLocalIPAddress() ?? "0.0.0.0");
                            logCmd.Parameters.AddWithValue("@agent", GlobalContext.getAppVersion() ?? "PosQu/unknown");
                            logCmd.ExecuteNonQuery();
                        }
                    }

                    Console.WriteLine("❌ Error authenticating user: " + ex.Message);
                }
            }

            return user;
        }


        private bool VerifyPassword(string inputPassword, string dbPasswordHash)
            {
                string passwordTrimmed = inputPassword.Trim();
                string hashTrimmed = dbPasswordHash?.Trim() ?? "";

                if (string.IsNullOrEmpty(hashTrimmed)) return false;

                return BCrypt.Net.BCrypt.Verify(passwordTrimmed, hashTrimmed);
            }

            public string HashPassword(string plainPassword)
            {
                return BCrypt.Net.BCrypt.HashPassword(plainPassword);
            }
        }
    
}
