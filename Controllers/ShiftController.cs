using System;
using System.Collections.Generic;
using Npgsql;
using POS_qu.Helpers;

namespace POS_qu.Controllers
{
    public class ShiftController
    {
        public Dictionary<string, object>? GetOpenShift(int userId, int terminalId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT id, user_id, terminal_id, opened_at, opening_cash, status
FROM cashier_shifts
WHERE user_id = @userId AND terminal_id = @terminalId AND status = 'open'
ORDER BY opened_at DESC
LIMIT 1", con);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;
            var dict = new Dictionary<string, object>();
            dict["id"] = reader.GetInt32(0);
            dict["user_id"] = reader.GetInt32(1);
            dict["terminal_id"] = reader.GetInt32(2);
            dict["opened_at"] = reader.GetDateTime(3);
            dict["opening_cash"] = reader.GetDecimal(4);
            dict["status"] = reader.GetString(5);
            return dict;
        }

        public int OpenShift(int userId, int terminalId, decimal openingCash)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
INSERT INTO cashier_shifts (user_id, terminal_id, opened_at, opening_cash, status, created_at, updated_at)
VALUES (@userId, @terminalId, NOW(), @openingCash, 'open', NOW(), NOW())
RETURNING id;", con);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@openingCash", openingCash);
            var id = cmd.ExecuteScalar();
            return Convert.ToInt32(id);
        }

        public void CloseShift(int shiftId, decimal expectedCash, decimal closingCash)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
UPDATE cashier_shifts
SET expected_cash = @expected,
    closing_cash = @closing,
    difference_cash = COALESCE(@closing,0) - COALESCE(@expected,0),
    closed_at = NOW(),
    status = 'closed',
    updated_at = NOW()
WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@expected", expectedCash);
            cmd.Parameters.AddWithValue("@closing", closingCash);
            cmd.Parameters.AddWithValue("@id", shiftId);
            cmd.ExecuteNonQuery();
        }
    }
}
