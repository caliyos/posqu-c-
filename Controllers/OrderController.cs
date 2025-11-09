using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace POS_qu.Controllers
{
    public class OrderController
    {

        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            var details = new List<OrderDetail>();

            using (var vCon = new Npgsql.NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = @"
            SELECT *
            FROM order_details
            WHERE order_id = @order_id
        ";

                using (var cmd = new Npgsql.NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@order_id", orderId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var detail = new OrderDetail
                            {
                                ItemId = Convert.ToInt32(reader["item_id"]),
                                Barcode = reader["od_barcode"].ToString(),  // <-- ganti name jadi od_barcode
                                Quantity = Convert.ToDecimal(reader["od_quantity"]),
                                Unit = reader["od_unit"].ToString(),
                                PricePerUnit = Convert.ToDecimal(reader["od_price_per_unit"]),
                                DiscountPerItem = Convert.ToDecimal(reader["od_discount_total"]),
                                Total = Convert.ToDecimal(reader["od_total"])
                            };
                            details.Add(detail);
                        }
                    }
                }
            }

            return details;
        }


        public Orders GetOrderById(int orderId)
        {
            Orders order = null;

            using (var vCon = new Npgsql.NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = @"
            SELECT *
            FROM orders
            WHERE order_id = @order_id
              AND deleted_at IS NULL
        ";

                using (var cmd = new Npgsql.NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@order_id", orderId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new Orders
                            {
                                OrderId = reader.GetInt32(reader.GetOrdinal("order_id")),
                                OrderNumber = reader["order_number"].ToString(),
                                CustomerName = reader["customer_name"].ToString(),
                                CustomerPhone = reader["customer_phone"].ToString(),
                                OrderStatus = Convert.ToInt32(reader["order_status"]),
                                PaymentMethod = reader["payment_method"]?.ToString(),
                                DeliveryMethod = reader["delivery_method"]?.ToString(),
                                DeliveryTime = reader["delivery_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["delivery_time"]),
                                OrderNote = reader["order_note"]?.ToString(),
                                OrderTotal = Convert.ToDecimal(reader["order_total"])
                            };
                        }
                    }
                }
            }

            return order;
        }


        public DataTable GetPendingOrders()
        {
            DataTable dt = new DataTable();

            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = @"
SELECT 
    order_id,
    order_number,
    order_code,
    customer_name,
    customer_phone,
    order_total,
    order_status,
    payment_method,
    delivery_method,
    delivery_time,
    order_note,
    terminal_id,
    shift_id,
    user_id,
    created_by,
    created_at,
    updated_at,
    deleted_at
FROM orders
WHERE deleted_at IS NULL
ORDER BY created_at DESC
";

                using (var cmd = new NpgsqlCommand(query, vCon))
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }


        public int GetPendingOrdersCount()
        {
            try
            {
                using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    string query = @"
                SELECT COUNT(*) 
                FROM orders 
                WHERE order_status = 0
                  AND deleted_at IS NULL;
            ";

                    using (var cmd = new NpgsqlCommand(query, vCon))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch
            {
                return 0;
            }
        }


        public bool DeleteOrderDetail(int orderId)
        {
            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                using (var trans = vCon.BeginTransaction())
                {
                    try
                    {
                        // 1. Hapus detail order berdasarkan order_id
                        string deleteDetails = @"
                    DELETE FROM order_details
                    WHERE order_id = @order_id;
                ";

                        using (var cmd = new NpgsqlCommand(deleteDetails, vCon, trans))
                        {
                            cmd.Parameters.AddWithValue("order_id", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        // 2. (Opsional) Hapus juga record utama dari tabel orders
                        //    jika kamu memang ingin menghapus order-nya juga.
                        string deleteOrder = @"
                    DELETE FROM orders
                    WHERE order_id = @order_id;
                ";

                        using (var cmd = new NpgsqlCommand(deleteOrder, vCon, trans))
                        {
                            cmd.Parameters.AddWithValue("order_id", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Commit transaksi
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Console.WriteLine("Error deleting order detail: " + ex.Message);
                        return false;
                    }
                }
            }
        }



        public bool SaveOrderWithDetails(Orders order, int terminalId, int cashierId, out string errorMessage)
        {
            errorMessage = string.Empty;

            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                using (var trans = vCon.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert ke tabel orders
                        string insertOrder = @"
                            INSERT INTO orders (
                                order_number, order_code, order_total, order_status, payment_method,
                                delivery_method, delivery_time, order_note, customer_name, customer_phone,
                                terminal_id, shift_id, user_id, created_by, created_at, updated_at
                            ) VALUES (
                                @order_number, @order_code, @order_total, @order_status, @payment_method,
                                @delivery_method, @delivery_time, @order_note, @customer_name, @customer_phone,
                                @terminal_id, @shift_id, @user_id, @created_by, NOW(), NOW()
                            )
                            RETURNING order_id;
                        ";

                        int newOrderId;
                        using (var cmd = new NpgsqlCommand(insertOrder, vCon, trans))
                        {
                            cmd.Parameters.AddWithValue("order_number", order.OrderNumber);
                            cmd.Parameters.AddWithValue("order_code", order.OrderCode);
                            cmd.Parameters.AddWithValue("order_total", order.OrderTotal);
                            cmd.Parameters.AddWithValue("order_status", order.OrderStatus);
                            cmd.Parameters.AddWithValue("payment_method", order.PaymentMethod ?? "");
                            cmd.Parameters.AddWithValue("delivery_method", order.DeliveryMethod ?? "");
                            cmd.Parameters.AddWithValue("delivery_time", (object?)order.DeliveryTime ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("order_note", order.OrderNote ?? "");
                            cmd.Parameters.AddWithValue("customer_name", order.CustomerName ?? "");
                            cmd.Parameters.AddWithValue("customer_phone", order.CustomerPhone ?? "");
                            cmd.Parameters.AddWithValue("terminal_id", order.TerminalId);
                            cmd.Parameters.AddWithValue("shift_id", order.ShiftId);
                            cmd.Parameters.AddWithValue("user_id", order.UserId);
                            cmd.Parameters.AddWithValue("created_by", order.CreatedBy);

                            newOrderId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 2. Ambil data pending_transactions
                        string getPendingItems = @"
                            SELECT item_id, barcode, quantity, unit, sell_price,
                                   discount_percentage, discount_total, tax, total, note
                            FROM pending_transactions
                            WHERE terminal_id = @terminal_id AND cashier_id = @cashier_id;
                        ";

                        var pendingItems = new List<PendingTransaction>();
                        using (var cmd = new NpgsqlCommand(getPendingItems, vCon, trans))
                        {
                            cmd.Parameters.AddWithValue("terminal_id", terminalId);
                            cmd.Parameters.AddWithValue("cashier_id", cashierId);

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    pendingItems.Add(new PendingTransaction
                                    {
                                        ItemId = reader.GetInt32(0),
                                        Barcode = reader.GetString(1),
                                        Quantity = reader.GetDecimal(2),
                                        Unit = reader.GetString(3),
                                        SellPrice = reader.GetDecimal(4),
                                        DiscountPercentage = reader.GetDecimal(5),
                                        DiscountTotal = reader.GetDecimal(6),
                                        Tax = reader.GetDecimal(7),
                                        Total = reader.GetDecimal(8),
                                        Note = reader.IsDBNull(9) ? "" : reader.GetString(9)
                                    });
                                }
                            }
                        }

                        // 3. Insert ke order_details
                        string insertDetails = @"
                            INSERT INTO order_details (
                                order_id, item_id, od_barcode, od_quantity, od_unit,
                                od_price_per_unit, od_sell_price, od_total,
                                od_discount_percentage, od_discount_total, od_tax, od_note
                            ) VALUES (
                                @order_id, @item_id, @od_barcode, @od_quantity, @od_unit,
                                @od_price_per_unit, @od_sell_price, @od_total,
                                @od_discount_percentage, @od_discount_total, @od_tax, @od_note
                            );
                        ";

                        foreach (var item in pendingItems)
                        {
                            using (var cmd = new NpgsqlCommand(insertDetails, vCon, trans))
                            {
                                cmd.Parameters.AddWithValue("order_id", newOrderId);
                                cmd.Parameters.AddWithValue("item_id", item.ItemId);
                                cmd.Parameters.AddWithValue("od_barcode", item.Barcode);
                                cmd.Parameters.AddWithValue("od_quantity", item.Quantity);
                                cmd.Parameters.AddWithValue("od_unit", item.Unit);
                                cmd.Parameters.AddWithValue("od_price_per_unit", item.SellPrice);
                                cmd.Parameters.AddWithValue("od_sell_price", item.SellPrice * item.Quantity);
                                cmd.Parameters.AddWithValue("od_total", item.Total);
                                cmd.Parameters.AddWithValue("od_discount_percentage", item.DiscountPercentage);
                                cmd.Parameters.AddWithValue("od_discount_total", item.DiscountTotal);
                                cmd.Parameters.AddWithValue("od_tax", item.Tax);
                                cmd.Parameters.AddWithValue("od_note", item.Note ?? "");
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // 4. Hapus data pending_transactions
                        string deletePending = @"
                            DELETE FROM pending_transactions
                            WHERE terminal_id = @terminal_id AND cashier_id = @cashier_id;
                        ";
                        using (var cmd = new NpgsqlCommand(deletePending, vCon, trans))
                        {
                            cmd.Parameters.AddWithValue("terminal_id", terminalId);
                            cmd.Parameters.AddWithValue("cashier_id", cashierId);
                            cmd.ExecuteNonQuery();
                        }

                        // 5. Commit transaksi
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        errorMessage = ex.Message;
                        return false;
                    }
                }
            }
        }
    }
}
