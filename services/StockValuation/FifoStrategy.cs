using System;
using Npgsql;
using POS_qu.Core.Interfaces;

namespace POS_qu.Services.StockValuation
{
    public class FifoStrategy : IStockValuationStrategy
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

            string sql = @"
                INSERT INTO stock_layers (item_id, warehouse_id, qty_remaining, buy_price, expired_at, created_at)
                VALUES (@item_id, @warehouse_id, @qty, @cost, @exp, NOW())";
                
            using (var cmd = new NpgsqlCommand(sql, con, tran))
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
            
            var result = new StockDeductionResult { QtyDeducted = qtyToDeduct };
            decimal remainingToDeduct = qtyToDeduct;

            string getLayersSql = @"
                SELECT id, qty_remaining, buy_price 
                FROM stock_layers 
                WHERE item_id = @item_id AND warehouse_id = @warehouse_id AND qty_remaining > 0
                ORDER BY created_at ASC, id ASC
                FOR UPDATE";

            using (var cmd = new NpgsqlCommand(getLayersSql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                
                using (var reader = cmd.ExecuteReader())
                {
                    var layers = new System.Collections.Generic.List<dynamic>();
                    while (reader.Read())
                    {
                        layers.Add(new
                        {
                            Id = Convert.ToInt64(reader["id"]),
                            QtyRemaining = reader["qty_remaining"] != DBNull.Value ? Convert.ToDecimal(reader["qty_remaining"]) : 0m,
                            UnitCost = reader["buy_price"] != DBNull.Value ? Convert.ToDecimal(reader["buy_price"]) : 0m
                        });
                    }
                    reader.Close();

                    foreach (var layer in layers)
                    {
                        if (remainingToDeduct <= 0) break;

                        decimal qtyFromThisLayer = Math.Min(layer.QtyRemaining, remainingToDeduct);
                        result.TotalCogs += qtyFromThisLayer * layer.UnitCost;
                        remainingToDeduct -= qtyFromThisLayer;
                        result.Lines.Add(new StockDeductionLine
                        {
                            StockLayerId = layer.Id,
                            Qty = qtyFromThisLayer,
                            UnitCost = layer.UnitCost
                        });

                        string updateLayerSql = "UPDATE stock_layers SET qty_remaining = qty_remaining - @qty WHERE id = @id";
                        using (var updateCmd = new NpgsqlCommand(updateLayerSql, con, tran))
                        {
                            updateCmd.Parameters.AddWithValue("@qty", (double)qtyFromThisLayer);
                            updateCmd.Parameters.AddWithValue("@id", layer.Id);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            if (remainingToDeduct > 0)
            {
                string fallbackCostSql = "SELECT buy_price FROM items WHERE id = @item_id";
                decimal fallbackCost = 0;
                using (var cmd = new NpgsqlCommand(fallbackCostSql, con, tran))
                {
                    cmd.Parameters.AddWithValue("@item_id", itemId);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value)
                    {
                        fallbackCost = Convert.ToDecimal(obj);
                    }
                }
                
                result.TotalCogs += remainingToDeduct * fallbackCost;
                result.Lines.Add(new StockDeductionLine
                {
                    StockLayerId = null,
                    Qty = remainingToDeduct,
                    UnitCost = fallbackCost
                });
                
                string insertMinusSql = @"
                    INSERT INTO stock_layers (item_id, warehouse_id, qty_remaining, buy_price, created_at)
                    VALUES (@item_id, @warehouse_id, @qty, @cost, NOW())";
                using (var cmd = new NpgsqlCommand(insertMinusSql, con, tran))
                {
                    cmd.Parameters.AddWithValue("@item_id", itemId);
                    cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    cmd.Parameters.AddWithValue("@qty", (double)(-remainingToDeduct));
                    cmd.Parameters.AddWithValue("@cost", fallbackCost);
                    cmd.ExecuteNonQuery();
                }
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
    }
}
