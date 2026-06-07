using System;
using System.Data;
using Npgsql;
using POS_qu.Core.Interfaces;
using POS_qu.Models;
using POS_qu.Repositories;

namespace POS_qu.Services.StockValuation
{
    //_cartrepo = new CartActivity;
    public class AverageStrategy : IStockValuationStrategy
    {
        private static bool HasColumn(NpgsqlConnection con, NpgsqlTransaction tran, string tableName, string columnName)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.columns
WHERE table_schema = current_schema()
  AND table_name = @t
  AND column_name = @c
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@t", tableName);
            cmd.Parameters.AddWithValue("@c", columnName);
            var obj = cmd.ExecuteScalar();
            return obj != null && obj != DBNull.Value;
        }

        private static bool HasTable(NpgsqlConnection con, NpgsqlTransaction tran, string tableName)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.tables
WHERE table_schema = current_schema()
  AND table_name = @t
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@t", tableName);
            var obj = cmd.ExecuteScalar();
            return obj != null && obj != DBNull.Value;
        }

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
INSERT INTO stock_layers (item_id, warehouse_id, qty_initial, qty_remaining, buy_price, expired_at, created_at)
VALUES (@item_id, @warehouse_id, @qty, @qty, @cost, @exp, NOW())
", con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                cmd.Parameters.AddWithValue("@qty", (double)qtyAdded);
                cmd.Parameters.AddWithValue("@cost", unitCost);
                cmd.Parameters.AddWithValue("@exp", (object?)expiredAt ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }

            UpdateMainStock(itemId, warehouseId, qtyAdded, unitCost, con, tran);
        }

        public StockDeductionResult CalculateCOGSAndDeductStock(int itemId, int warehouseId, decimal qtyToDeduct, object dbConnection, object dbTransaction)
        {
            var con = (NpgsqlConnection)dbConnection;
            var tran = (NpgsqlTransaction)dbTransaction;

            // KIT/ BUNDLE handling: if the item is a kit/bundle, we need to break it down to its components and deduct stock from them instead, but we won't create stock deduction lines for the kit/bundle itself since it doesn't have its own stock layers
            if (_cartrepo.IsKitBundle(con, tran, itemId))
            {
                return DeductKitOnly(itemId, qtyToDeduct,warehouseId, con, tran);
            }

            // For regular items, we calculate COGS based on average cost and deduct stock from layers accordingly

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
INSERT INTO stock_layers (item_id, warehouse_id, qty_initial, qty_remaining, buy_price, created_at)
VALUES (@item_id, @warehouse_id, @qty, @qty, @cost, NOW())
", con, tran);
                ins.Parameters.AddWithValue("@item_id", itemId);
                ins.Parameters.AddWithValue("@warehouse_id", warehouseId);
                ins.Parameters.AddWithValue("@qty", (double)(-remainingToDeduct));
                ins.Parameters.AddWithValue("@cost", fallbackCost);
                ins.ExecuteNonQuery();
            }

            UpdateMainStock(itemId, warehouseId, -qtyToDeduct, avgCost, con, tran);
            return result;
        }

        private StockDeductionResult DeductKitOnly(
    int kitItemId,
    decimal qtyToDeduct,
    int warehouseId,
    NpgsqlConnection con,
    NpgsqlTransaction tran)
        {
            var result = new StockDeductionResult();

            var boms = _cartrepo.GetKitBundleBom(con, tran, kitItemId);

            foreach (var bom in boms)
            {
                decimal totalQty = bom.Qty * qtyToDeduct;

                var r = CalculateCOGSAndDeductStock(
                    bom.ComponentItemId,
                    warehouseId,
                    totalQty,
                    con,
                    tran
                );

                result.TotalCogs += r.TotalCogs;
                result.Lines.AddRange(r.Lines);
            }

            return result;
        }

        private void UpdateMainStock(int itemId, int warehouseId, decimal qtyChange, decimal unitCost, NpgsqlConnection con, NpgsqlTransaction tran)
        {

            bool hasHppAvg = HasColumn(con, tran, "stocks", "hpp_avg");
            if (hasHppAvg)
            {
                using (var ensure = new NpgsqlCommand(@"
INSERT INTO stocks (item_id, warehouse_id, qty, hpp_avg)
SELECT @item_id, @warehouse_id, 0, 0
WHERE NOT EXISTS (
    SELECT 1 FROM stocks WHERE item_id = @item_id AND warehouse_id = @warehouse_id
)
", con, tran))
                {
                    ensure.Parameters.AddWithValue("@item_id", itemId);
                    ensure.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    ensure.ExecuteNonQuery();
                }

                decimal qtyBefore = 0m;
                decimal avgBefore = 0m;
                using (var sel = new NpgsqlCommand(@"
SELECT COALESCE(qty,0) AS qty, COALESCE(hpp_avg,0) AS hpp_avg
FROM stocks
WHERE item_id = @item_id AND warehouse_id = @warehouse_id
FOR UPDATE
", con, tran))
                {
                    sel.Parameters.AddWithValue("@item_id", itemId);
                    sel.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    using var r = sel.ExecuteReader();
                    if (r.Read())
                    {
                        qtyBefore = r["qty"] != DBNull.Value ? Convert.ToDecimal(r["qty"]) : 0m;
                        avgBefore = r["hpp_avg"] != DBNull.Value ? Convert.ToDecimal(r["hpp_avg"]) : 0m;
                    }
                }

                decimal qtyAfter = qtyBefore + qtyChange;
                decimal avgAfter = avgBefore;
                if (qtyChange > 0m && qtyAfter > 0m)
                    avgAfter = Math.Round(((qtyBefore * avgBefore) + (qtyChange * unitCost)) / qtyAfter, 2, MidpointRounding.AwayFromZero);

                using (var upd = new NpgsqlCommand(@"
UPDATE stocks
SET qty = @qty_after,
    hpp_avg = @avg_after
WHERE item_id = @item_id AND warehouse_id = @warehouse_id
", con, tran))
                {
                    upd.Parameters.AddWithValue("@item_id", itemId);
                    upd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    upd.Parameters.AddWithValue("@qty_after", (double)qtyAfter);
                    upd.Parameters.AddWithValue("@avg_after", avgAfter);
                    upd.ExecuteNonQuery();
                }

                if (qtyChange > 0m && HasTable(con, tran, "stock_avg_history"))
                {
                    var session = SessionUser.GetCurrentUser();
                    using var insHist = new NpgsqlCommand(@"
INSERT INTO stock_avg_history
(item_id, warehouse_id, qty_before, qty_in, qty_after, avg_before, unit_cost_in, avg_after, ref_type, ref_id, note, user_id, login_id, created_at)
VALUES
(@item_id, @warehouse_id, @qty_before, @qty_in, @qty_after, @avg_before, @unit_cost_in, @avg_after, @ref_type, @ref_id, @note, @user_id, @login_id, NOW())
", con, tran);
                    insHist.Parameters.AddWithValue("@item_id", itemId);
                    insHist.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    insHist.Parameters.AddWithValue("@qty_before", qtyBefore);
                    insHist.Parameters.AddWithValue("@qty_in", qtyChange);
                    insHist.Parameters.AddWithValue("@qty_after", qtyAfter);
                    insHist.Parameters.AddWithValue("@avg_before", avgBefore);
                    insHist.Parameters.AddWithValue("@unit_cost_in", unitCost);
                    insHist.Parameters.AddWithValue("@avg_after", avgAfter);
                    insHist.Parameters.AddWithValue("@ref_type", "STOCK_IN");
                    insHist.Parameters.AddWithValue("@ref_id", DBNull.Value);
                    insHist.Parameters.AddWithValue("@note", DBNull.Value);
                    insHist.Parameters.AddWithValue("@user_id", session != null && session.UserId > 0 ? session.UserId : (object)DBNull.Value);
                    insHist.Parameters.AddWithValue("@login_id", session != null && session.LoginId > 0 ? session.LoginId : (object)DBNull.Value);
                    insHist.ExecuteNonQuery();
                }
                return;
            }

            using (var cmd = new NpgsqlCommand(@"
INSERT INTO stocks (item_id, warehouse_id, qty)
VALUES (@item_id, @warehouse_id, @qty)
ON CONFLICT (item_id, warehouse_id)
DO UPDATE SET qty = stocks.qty + EXCLUDED.qty
", con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                cmd.Parameters.AddWithValue("@qty", (double)qtyChange);
                cmd.ExecuteNonQuery();
            }
        }

        private CartActivity _cartrepo = new CartActivity();
  
        private static decimal GetAverageCost(int itemId, int warehouseId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            if (HasColumn(con, tran, "stocks", "hpp_avg"))
            {
                using var cmdStock = new NpgsqlCommand(@"
SELECT COALESCE(hpp_avg, 0)
FROM stocks
WHERE item_id = @item_id AND warehouse_id = @warehouse_id
LIMIT 1
", con, tran);
                cmdStock.Parameters.AddWithValue("@item_id", itemId);
                cmdStock.Parameters.AddWithValue("@warehouse_id", warehouseId);
                var resStock = cmdStock.ExecuteScalar();
                var v = resStock != null && resStock != DBNull.Value ? Convert.ToDecimal(resStock) : 0m;
                if (v > 0m) return v;
            }

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
