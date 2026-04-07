using System;
using System.Data;
using Npgsql;
using POS_qu.Core.Interfaces;

namespace POS_qu.Services.StockValuation
{
    public class AverageStrategy : IStockValuationStrategy
    {
        public void AddStockIn(int itemId, int warehouseId, int qtyAdded, decimal unitCost, object dbConnection, object dbTransaction)
        {
            var con = (NpgsqlConnection)dbConnection;
            var tran = (NpgsqlTransaction)dbTransaction;

            // Logika Average:
            // (Total Value Lama + (qtyAdded * unitCost)) / (Total Qty Lama + qtyAdded)
            
            decimal currentQty = 0;
            decimal currentAvgCost = 0;

            // Ambil stok dan cost rata-rata saat ini dari tabel stocks atau items
            string getStockSql = "SELECT quantity FROM stocks WHERE item_id = @item_id AND warehouse_id = @warehouse_id";
            using (var cmd = new NpgsqlCommand(getStockSql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value) currentQty = Convert.ToDecimal(result);
            }

            // Ambil harga rata-rata terakhir dari item
            string getAvgCostSql = "SELECT average_cost FROM items WHERE id = @item_id";
            using (var cmd = new NpgsqlCommand(getAvgCostSql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value) currentAvgCost = Convert.ToDecimal(result);
            }
            
            // Perhitungan HPP Rata-rata
            if (currentQty < 0) currentQty = 0; // Abaikan stok minus untuk perhitungan avg
            decimal newAvgCost = unitCost;
            if (currentQty + qtyAdded > 0)
            {
                newAvgCost = ((currentQty * currentAvgCost) + (qtyAdded * unitCost)) / (currentQty + qtyAdded);
            }

            // Update harga rata-rata di item
            string updateItemSql = "UPDATE items SET average_cost = @new_cost WHERE id = @item_id";
            using (var cmd = new NpgsqlCommand(updateItemSql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@new_cost", newAvgCost);
                cmd.ExecuteNonQuery();
            }

            // Update stok
            UpdateMainStock(itemId, warehouseId, qtyAdded, con, tran);
        }

        public decimal CalculateCOGSAndDeductStock(int itemId, int warehouseId, int qtyToDeduct, object dbConnection, object dbTransaction)
        {
            var con = (NpgsqlConnection)dbConnection;
            var tran = (NpgsqlTransaction)dbTransaction;
            
            decimal currentAvgCost = 0;

            // Ambil harga rata-rata
            string getAvgCostSql = "SELECT COALESCE(average_cost, buy_price) FROM items WHERE id = @item_id";
            using (var cmd = new NpgsqlCommand(getAvgCostSql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value) currentAvgCost = Convert.ToDecimal(result);
            }

            // HPP adalah Qty * Harga Rata-rata
            decimal totalCogs = qtyToDeduct * currentAvgCost;

            // Kurangi stok
            UpdateMainStock(itemId, warehouseId, -qtyToDeduct, con, tran);

            return totalCogs;
        }

        private void UpdateMainStock(int itemId, int warehouseId, int qtyChange, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            string sql = @"
                INSERT INTO stocks (item_id, warehouse_id, quantity)
                VALUES (@item_id, @warehouse_id, @qty)
                ON CONFLICT (item_id, warehouse_id) 
                DO UPDATE SET quantity = stocks.quantity + @qty, updated_at = NOW()";
            using (var cmd = new NpgsqlCommand(sql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                cmd.Parameters.AddWithValue("@qty", qtyChange);
                cmd.ExecuteNonQuery();
            }
        }
    }
}