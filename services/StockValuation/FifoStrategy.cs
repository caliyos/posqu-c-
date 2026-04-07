using System;
using System.Data;
using Npgsql;
using POS_qu.Core.Interfaces;

namespace POS_qu.Services.StockValuation
{
    public class FifoStrategy : IStockValuationStrategy
    {
        public void AddStockIn(int itemId, int warehouseId, int qtyAdded, decimal unitCost, object dbConnection, object dbTransaction)
        {
            var con = (NpgsqlConnection)dbConnection;
            var tran = (NpgsqlTransaction)dbTransaction;

            string sql = @"
                INSERT INTO stock_layers (item_id, warehouse_id, qty_original, qty_remaining, unit_cost, received_at)
                VALUES (@item_id, @warehouse_id, @qty, @qty, @cost, NOW())";
                
            using (var cmd = new NpgsqlCommand(sql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                cmd.Parameters.AddWithValue("@qty", qtyAdded);
                cmd.Parameters.AddWithValue("@cost", unitCost);
                cmd.ExecuteNonQuery();
            }
            
            UpdateMainStock(itemId, warehouseId, qtyAdded, con, tran);
        }

        public decimal CalculateCOGSAndDeductStock(int itemId, int warehouseId, int qtyToDeduct, object dbConnection, object dbTransaction)
        {
            var con = (NpgsqlConnection)dbConnection;
            var tran = (NpgsqlTransaction)dbTransaction;
            
            decimal totalCogs = 0;
            int remainingToDeduct = qtyToDeduct;

            // Ambil layer stok yang masih ada isinya, urutkan dari yang tertua (FIFO)
            string getLayersSql = @"
                SELECT id, qty_remaining, unit_cost 
                FROM stock_layers 
                WHERE item_id = @item_id AND warehouse_id = @warehouse_id AND qty_remaining > 0
                ORDER BY received_at ASC, id ASC
                FOR UPDATE"; // Lock for update

            using (var cmd = new NpgsqlCommand(getLayersSql, con, tran))
            {
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                
                using (var reader = cmd.ExecuteReader())
                {
                    // Kita kumpulkan data dulu karena kita akan update layer ini setelahnya
                    var layers = new System.Collections.Generic.List<dynamic>();
                    while (reader.Read())
                    {
                        layers.Add(new { 
                            Id = reader.GetInt32(0), 
                            QtyRemaining = reader.GetInt32(1), 
                            UnitCost = reader.GetDecimal(2) 
                        });
                    }
                    reader.Close();

                    foreach (var layer in layers)
                    {
                        if (remainingToDeduct <= 0) break;

                        int qtyFromThisLayer = Math.Min(layer.QtyRemaining, remainingToDeduct);
                        totalCogs += qtyFromThisLayer * layer.UnitCost;
                        remainingToDeduct -= qtyFromThisLayer;

                        // Update layer di database
                        string updateLayerSql = "UPDATE stock_layers SET qty_remaining = qty_remaining - @qty WHERE id = @id";
                        using (var updateCmd = new NpgsqlCommand(updateLayerSql, con, tran))
                        {
                            updateCmd.Parameters.AddWithValue("@qty", qtyFromThisLayer);
                            updateCmd.Parameters.AddWithValue("@id", layer.Id);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Jika stok layer habis tapi masih ada permintaan (Stok minus/negatif)
            if (remainingToDeduct > 0)
            {
                // Ambil harga beli terakhir dari items sebagai fallback
                string fallbackCostSql = "SELECT buy_price FROM items WHERE id = @item_id";
                decimal fallbackCost = 0;
                using (var cmd = new NpgsqlCommand(fallbackCostSql, con, tran))
                {
                    cmd.Parameters.AddWithValue("@item_id", itemId);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        fallbackCost = Convert.ToDecimal(result);
                    }
                }
                
                totalCogs += remainingToDeduct * fallbackCost;
                
                // Tambahkan layer minus untuk dicatat
                string insertMinusSql = @"
                    INSERT INTO stock_layers (item_id, warehouse_id, qty_original, qty_remaining, unit_cost, received_at)
                    VALUES (@item_id, @warehouse_id, @qty, @qty, @cost, NOW())";
                using (var cmd = new NpgsqlCommand(insertMinusSql, con, tran))
                {
                    cmd.Parameters.AddWithValue("@item_id", itemId);
                    cmd.Parameters.AddWithValue("@warehouse_id", warehouseId);
                    cmd.Parameters.AddWithValue("@qty", -remainingToDeduct);
                    cmd.Parameters.AddWithValue("@cost", fallbackCost);
                    cmd.ExecuteNonQuery();
                }
            }
            
            // Kurangi stok utama
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