using POS_qu.Core;
using Npgsql;
using POS_qu.Helpers;

namespace POS_qu.Services
{
    public class StockAdjustmentService : IStockAdjustmentService
    {
 
        public void LogAdjustment(long itemId, string adjustmentType, decimal oldStock, decimal newStock, string reason, long? referenceId = null, string? referenceTable = null, long? userId = null)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string query = @"
                INSERT INTO stock_adjustment_logs 
                (item_id, adjustment_type, old_stock, new_stock, difference, reason, reference_id, reference_table, user_id, created_at)
                VALUES (@item_id, @adjustment_type, @old_stock, @new_stock, @difference, @reason, @reference_id, @reference_table, @user_id, NOW());
            ";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("item_id", itemId);
            cmd.Parameters.AddWithValue("adjustment_type", adjustmentType);
            cmd.Parameters.AddWithValue("old_stock", oldStock);
            cmd.Parameters.AddWithValue("new_stock", newStock);
            cmd.Parameters.AddWithValue("difference", newStock - oldStock);
            cmd.Parameters.AddWithValue("reason", reason);
            cmd.Parameters.AddWithValue("reference_id", (object?)referenceId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("reference_table", (object?)referenceTable ?? DBNull.Value);
            cmd.Parameters.AddWithValue("user_id", (object?)userId ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }
    }
}
