using System;
using System.Data;
using Npgsql;
using POS_qu.Core.Interfaces;

namespace POS_qu.Services.StockValuation
{
    public class AverageStrategy : IStockValuationStrategy
    {
        public void AddStockIn(int itemId, int warehouseId, decimal qtyAdded, decimal unitCost, object dbConnection, object dbTransaction)
        {
            var con = (NpgsqlConnection)dbConnection;
            var tran = (NpgsqlTransaction)dbTransaction;

            DateTime? expiredAt = null;
            using (var expCmd = new NpgsqlCommand("SELECT expired_at FROM items WHERE id=@id", con, tran))
            {
                expCmd.Parameters.AddWithValue("@id", itemId);
                var res = expCmd.ExecuteScalar();
                if (res != null && res != DBNull.Value) expiredAt = Convert.ToDateTime(res).Date;
            }

            using (var cmd = new NpgsqlCommand(@"
INSERT INTO stock_layers (item_id, warehouse_id, qty_remaining, buy_price, expired_at, created_at)
VALUES (@item_id, @warehouse_id, @qty, @cost, @exp, NOW())
", con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                cmd.Parameters.AddWithValue("@qty", (double)qtyAdded);
                cmd.Parameters.AddWithValue("@cost", unitCost);
                cmd.Parameters.AddWithValue("@exp", (object?)expiredAt ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }

            UpdateMainStock(itemId, warehouseId, qtyAdded, con, tran);
        }

        public StockDeductionResult CalculateCOGSAndDeductStock(int itemId, int warehouseId, decimal qtyToDeduct, object dbConnection, object dbTransaction)
        {
            var con = (NpgsqlConnection)dbConnection;
            var tran = (NpgsqlTransaction)dbTransaction;
            
            decimal avgCost = GetAverageCost(itemId, warehouseId, con, tran);
            var result = new StockDeductionResult { QtyDeducted = qtyToDeduct, TotalCogs = qtyToDeduct * avgCost };

            decimal remainingToDeduct = qtyToDeduct;
            using (var cmd = new NpgsqlCommand(@"
SELECT id, qty_remaining
FROM stock_layers
WHERE item_id=@item_id AND warehouse_id=@warehouse_id AND qty_remaining > 0
ORDER BY created_at ASC, id ASC
FOR UPDATE
", con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                using var r = cmd.ExecuteReader();
                var rows = new System.Collections.Generic.List<(long Id, decimal Qty)>();
                while (r.Read())
                {
                    long id = Convert.ToInt64(r["id"]);
                    decimal qty = r["qty_remaining"] != DBNull.Value ? Convert.ToDecimal(r["qty_remaining"]) : 0m;
                    rows.Add((id, qty));
                }
                r.Close();

                foreach (var row in rows)
                {
                    if (remainingToDeduct <= 0) break;
                    decimal take = Math.Min(row.Qty, remainingToDeduct);
                    remainingToDeduct -= take;
                    result.Lines.Add(new StockDeductionLine
                    {
                        StockLayerId = row.Id,
                        Qty = take,
                        UnitCost = avgCost
                    });
                    using var upd = new NpgsqlCommand("UPDATE stock_layers SET qty_remaining = qty_remaining - @q WHERE id = @id", con, tran);
                    upd.Parameters.AddWithValue("@q", (double)take);
                    upd.Parameters.AddWithValue("@id", row.Id);
                    upd.ExecuteNonQuery();
                }
            }

            if (remainingToDeduct > 0)
            {
                decimal fallbackCost = GetFallbackBuyPrice(itemId, con, tran);
                result.Lines.Add(new StockDeductionLine
                {
                    StockLayerId = null,
                    Qty = remainingToDeduct,
                    UnitCost = fallbackCost
                });
                using var ins = new NpgsqlCommand(@"
INSERT INTO stock_layers (item_id, warehouse_id, qty_remaining, buy_price, created_at)
VALUES (@item_id, @warehouse_id, @qty, @cost, NOW())
", con, tran);
                ins.Parameters.AddWithValue("@item_id", itemId);
                ins.Parameters.AddWithValue("@warehouse_id", warehouseId);
                ins.Parameters.AddWithValue("@qty", (double)(-remainingToDeduct));
                ins.Parameters.AddWithValue("@cost", fallbackCost);
                ins.ExecuteNonQuery();
            }

            UpdateMainStock(itemId, warehouseId, -qtyToDeduct, con, tran);
            return result;
        }

        private void UpdateMainStock(int itemId, int warehouseId, decimal qtyChange, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            string sql = @"
                INSERT INTO stocks (item_id, warehouse_id, qty)
                VALUES (@item_id, @warehouse_id, @qty)
                ON CONFLICT (item_id, warehouse_id) 
                DO UPDATE SET qty = stocks.qty + @qty";
            using (var cmd = new NpgsqlCommand(sql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                cmd.Parameters.AddWithValue("@qty", (double)qtyChange);
                cmd.ExecuteNonQuery();
            }
        }

        private static decimal GetAverageCost(int itemId, int warehouseId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using (var cmd = new NpgsqlCommand(@"
SELECT
    CASE
        WHEN SUM(qty_remaining) > 0 THEN SUM(qty_remaining * buy_price) / SUM(qty_remaining)
        ELSE NULL
    END AS avg_cost
FROM stock_layers
WHERE item_id=@item_id AND warehouse_id=@warehouse_id AND qty_remaining > 0
", con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                var res = cmd.ExecuteScalar();
                if (res != null && res != DBNull.Value) return Convert.ToDecimal(res);
            }
            return GetFallbackBuyPrice(itemId, con, tran);
        }

        private static decimal GetFallbackBuyPrice(int itemId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand("SELECT COALESCE(buy_price,0) FROM items WHERE id=@item_id", con, tran);
            cmd.Parameters.AddWithValue("@item_id", itemId);
            var res = cmd.ExecuteScalar();
            return res != null && res != DBNull.Value ? Convert.ToDecimal(res) : 0m;
        }
    }
}
