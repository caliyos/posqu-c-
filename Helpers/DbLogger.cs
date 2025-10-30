using POS_qu.Core;
using Npgsql; // kalau pakai PostgreSQL
using System;
using POS_qu.Helpers;

namespace POS_qu.Helpers
{
    public class DbLogger : ILogger
    {

        //public void Log(int userid,string message)
        //{
        //    using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
        //    conn.Open();

        //    string query = @"
        //        INSERT INTO audit_logs (user_id, action, description, created_at) 
        //        VALUES (@user_id, @action, @description, NOW());
        //    ";

        //    using var cmd = new NpgsqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("user_id", DBNull.Value); // nanti bisa inject user id aktif
        //    cmd.Parameters.AddWithValue("action", "APP_LOG");
        //    cmd.Parameters.AddWithValue("description", message);

        //    cmd.ExecuteNonQuery();
        //}

        // Versi baru untuk log terstruktur
        public void Log(string userId, string actionType, int? referenceId,string? desc, string? details = null)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string query = @"
                INSERT INTO audit_logs (user_id, action, reference_id, description, details, created_at) 
                VALUES (@user_id, @action, @reference_id, @description ,@details, NOW());
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            if (long.TryParse(userId, out long parsedUserId))
                cmd.Parameters.AddWithValue("user_id", parsedUserId);
            else
                cmd.Parameters.AddWithValue("user_id", DBNull.Value);
            cmd.Parameters.AddWithValue("action", actionType ?? "LOGIN");
            cmd.Parameters.AddWithValue("reference_id", Convert.ToInt32(referenceId));
            cmd.Parameters.AddWithValue("description", string.IsNullOrEmpty(desc) ? "{}" : desc);
            cmd.Parameters.AddWithValue("details", string.IsNullOrEmpty(details) ? "{}" : details);

            cmd.ExecuteNonQuery();
        }
    }
}
