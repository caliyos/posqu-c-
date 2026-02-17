using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.DTO;
using System;
using System.Collections.Generic;

namespace POS_qu.services
{
    public class TransactionRepo
    {
        public int InsertTransaction(
     NpgsqlConnection con,
     NpgsqlTransaction tran,
     Transactions transaction)
        {
            string sql = @"
INSERT INTO transactions (
    ts_numbering, ts_code, ts_total, ts_payment_amount, ts_cashback, 
    ts_method, ts_status, ts_change, ts_internal_note, ts_note, 
    ts_global_discount_amount, ts_grand_total, 
    ts_customer, ts_freename, terminal_id, shift_id, user_id, 
    created_by, created_at, order_id, ts_delivery_amount, cart_session_code,
    ts_due_amount
) 
VALUES (
    @ts_numbering, @ts_code, @ts_total, @ts_payment_amount, @ts_cashback, 
    @ts_method, @ts_status, @ts_change, @ts_internal_note, @ts_note, 
    @ts_global_discount_amount, @ts_grand_total, 
    @ts_customer, @ts_freename, @terminal_id, @shift_id, 
    @user_id, @created_by, @created_at, @order_id, @ts_delivery_amount, @cart_session_code,
    @ts_due_amount
) 
RETURNING ts_id";

            using (var cmd = new NpgsqlCommand(sql, con, tran))
            {
                cmd.Parameters.AddWithValue("@ts_numbering", transaction.TsNumbering);
                cmd.Parameters.AddWithValue("@ts_code", transaction.TsCode);
                cmd.Parameters.AddWithValue("@ts_total", transaction.TsTotal);
                cmd.Parameters.AddWithValue("@ts_payment_amount", transaction.TsPaymentAmount);
                cmd.Parameters.AddWithValue("@ts_cashback", transaction.TsCashback);
                cmd.Parameters.AddWithValue("@ts_method", transaction.TsMethod);
                cmd.Parameters.AddWithValue("@ts_status", (int)transaction.TsStatus);
                cmd.Parameters.AddWithValue("@ts_change", transaction.TsChange);
                cmd.Parameters.AddWithValue("@ts_internal_note", transaction.TsInternalNote ?? "");
                cmd.Parameters.AddWithValue("@ts_note", transaction.TsNote ?? "");

                cmd.Parameters.AddWithValue("@ts_global_discount_amount", transaction.TsDiscountTotal);
                cmd.Parameters.AddWithValue("@ts_grand_total", transaction.TsGrandTotal);

                cmd.Parameters.AddWithValue("@ts_customer",
                    (object)transaction.TsCustomer ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ts_freename", transaction.TsFreename ?? "");
                cmd.Parameters.AddWithValue("@terminal_id", transaction.TerminalId);
                cmd.Parameters.AddWithValue("@shift_id", transaction.ShiftId);
                cmd.Parameters.AddWithValue("@user_id", transaction.UserId);
                cmd.Parameters.AddWithValue("@created_by", transaction.CreatedBy);
                cmd.Parameters.AddWithValue("@created_at", transaction.CreatedAt);

                cmd.Parameters.AddWithValue("@order_id",
                    (object)transaction.OrderId ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ts_delivery_amount", transaction.TsDelivery);
                cmd.Parameters.AddWithValue("@cart_session_code", transaction.CartSessionCode ?? "");
                cmd.Parameters.AddWithValue("@ts_delivery_amount", transaction.TsDelivery);
                cmd.Parameters.AddWithValue("@ts_due_amount", transaction.TsDueAmount);
                

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        public void InsertTransactionDetails(
    NpgsqlConnection con,
    NpgsqlTransaction tran,
    List<TransactionDetail> details)
        {
            string sql = @"
INSERT INTO transaction_details (
    ts_id, item_id, tsd_name, tsd_barcode, tsd_buy_price, tsd_sell_price, 
    tsd_quantity, tsd_unit, tsd_note, 
    tsd_discount_per_item, tsd_discount_percentage, tsd_discount_total, 
    tsd_tax, tsd_total, tsd_conversion_rate, tsd_price_per_unit, 
    tsd_unit_variant, created_by, created_at, cart_session_code
) 
VALUES (
    @ts_id, @item_id, @tsd_name, @tsd_barcode, @tsd_buy_price, @tsd_sell_price, 
    @tsd_quantity, @tsd_unit, @tsd_note, 
    @tsd_discount_per_item, @tsd_discount_percentage, @tsd_discount_total, 
    @tsd_tax, @tsd_total, @tsd_conversion_rate, @tsd_price_per_unit, 
    @tsd_unit_variant, @created_by, @created_at, @cart_session_code
)";

            using (var cmd = new NpgsqlCommand(sql, con, tran))
            {
                foreach (var detail in details)
                {
                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@ts_id", detail.TsId);
                    cmd.Parameters.AddWithValue("@item_id", detail.ItemId);
                    cmd.Parameters.AddWithValue("@tsd_name", detail.Name ?? "");
                    cmd.Parameters.AddWithValue("@tsd_barcode", detail.Barcode ?? "");
                    cmd.Parameters.AddWithValue("@tsd_buy_price", detail.TsdBuyPrice);
                    cmd.Parameters.AddWithValue("@tsd_sell_price", detail.TsdSellPrice);
                    cmd.Parameters.AddWithValue("@tsd_quantity", detail.TsdQuantity);
                    cmd.Parameters.AddWithValue("@tsd_unit", detail.TsdUnit ?? "");
                    cmd.Parameters.AddWithValue("@tsd_note", detail.TsdNote ?? "");
                    cmd.Parameters.AddWithValue("@tsd_discount_per_item", detail.TsdDiscountPerItem);
                    cmd.Parameters.AddWithValue("@tsd_discount_percentage", detail.TsdDiscountPercentage);
                    cmd.Parameters.AddWithValue("@tsd_discount_total", detail.TsdDiscountTotal);
                    cmd.Parameters.AddWithValue("@tsd_tax", detail.TsdTax);
                    cmd.Parameters.AddWithValue("@tsd_total", detail.TsdTotal);
                    cmd.Parameters.AddWithValue("@tsd_conversion_rate", detail.TsdConversionRate);
                    cmd.Parameters.AddWithValue("@tsd_price_per_unit", detail.TsdPricePerUnit);
                    cmd.Parameters.AddWithValue("@tsd_unit_variant", detail.TsdUnitVariant ?? "");
                    cmd.Parameters.AddWithValue("@created_by", detail.CreatedBy);
                    cmd.Parameters.AddWithValue("@created_at", detail.CreatedAt);
                    cmd.Parameters.AddWithValue("@cart_session_code", detail.CartSessionCode ?? "");

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int ReduceStock(
      NpgsqlConnection con,
      NpgsqlTransaction tran,
      int itemId,
      decimal qty)
        {
            string sql = @"
        UPDATE items
        SET 
            stock = stock - @qty,
            reserved_stock = reserved_stock - @qty
        WHERE id = @id 
        AND reserved_stock >= @qty";

            using (var cmd = new NpgsqlCommand(sql, con, tran))
            {
                cmd.Parameters.AddWithValue("@id", itemId);
                cmd.Parameters.AddWithValue("@qty", qty);

                return cmd.ExecuteNonQuery();
            }
        }


        public decimal GetCurrentStock(
     NpgsqlConnection con,
     NpgsqlTransaction tran,
     int itemId)
        {
            string sql = "SELECT stock FROM items WHERE id = @id FOR UPDATE";

            using (var cmd = new NpgsqlCommand(sql, con, tran))
            {
                cmd.Parameters.AddWithValue("@id", itemId);

                object result = cmd.ExecuteScalar();

                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }

        public void DeletePendingTransactions(NpgsqlConnection con, NpgsqlTransaction tran, string code)
        {
            string sql = "DELETE FROM pending_transactions WHERE cart_session_code = @code";

            using (var cmd = new NpgsqlCommand(sql, con, tran))
            {
                cmd.Parameters.AddWithValue("@code", code);
                cmd.ExecuteNonQuery();
            }
        }


        public void InsertStockLog(
      NpgsqlConnection con,
      NpgsqlTransaction tran,
      StockLog stockLog)
        {
            string query = @"
        INSERT INTO stock_log
        (product_id, tipe_transaksi, qty_masuk, qty_keluar, 
         sisa_stock, keterangan, user_id, created_at, login_id)
        VALUES
        (@product_id, @tipe_transaksi, @qty_masuk, @qty_keluar, 
         @sisa_stock, @keterangan, @user_id, @created_at, @login_id)
    ";

            using (var cmd = new NpgsqlCommand(query, con, tran))
            {
                cmd.Parameters.AddWithValue("@product_id", stockLog.ProductId);
                cmd.Parameters.AddWithValue("@tipe_transaksi", stockLog.TipeTransaksi ?? "");
                cmd.Parameters.AddWithValue("@qty_masuk", stockLog.QtyMasuk);
                cmd.Parameters.AddWithValue("@qty_keluar", stockLog.QtyKeluar);
                cmd.Parameters.AddWithValue("@sisa_stock", stockLog.SisaStock);
                cmd.Parameters.AddWithValue("@keterangan", stockLog.Keterangan ?? "");
                cmd.Parameters.AddWithValue("@user_id", stockLog.UserId);
                cmd.Parameters.AddWithValue("@created_at", stockLog.CreatedAt);
                cmd.Parameters.AddWithValue("@login_id",
                    stockLog.LoginId.HasValue
                        ? (object)stockLog.LoginId.Value
                        : DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }



        ////////////////////// CICILAN ///////////////////////////
        public void InsertInstallment(
    NpgsqlConnection con,
    NpgsqlTransaction tran,
    int transactionId,
    decimal amount,
    string note,
    int userId)
        {
            using var cmd = new NpgsqlCommand(@"
        INSERT INTO transaction_installments
        (transaction_id, amount, note, created_by, created_at)
        VALUES (@tsId, @amount, @note, @userId, NOW())
    ", con, tran);

            cmd.Parameters.AddWithValue("@tsId", transactionId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@note",
                string.IsNullOrWhiteSpace(note) ? (object)DBNull.Value : note);
            cmd.Parameters.AddWithValue("@userId", userId);

            cmd.ExecuteNonQuery();
        }




        public TransactionDto GetTransactionById(int transactionId)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT ts_id, ts_total, ts_payment_amount, ts_due_amount, ts_status, ts_numbering FROM transactions WHERE ts_id=@id", conn);
            cmd.Parameters.AddWithValue("@id", transactionId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TransactionDto
                {
                    Id = reader.GetInt32(0),
                    TotalAmount = reader.GetDecimal(1),
                    AmountPaid = reader.GetDecimal(2),
                    DueAmount = reader.GetDecimal(3),
                    Status = reader.GetInt16(4),
                    TransactionNumber = reader.GetString(5)
                };
            }
            return null;
        }

        public void InsertInstallment(int transactionId, decimal amount, string note, int userId)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "INSERT INTO transaction_installments (transaction_id, amount, note, created_by) VALUES (@txId, @amt, @note, @userId)", conn);
            cmd.Parameters.AddWithValue("@txId", transactionId);
            cmd.Parameters.AddWithValue("@amt", amount);
            cmd.Parameters.AddWithValue("@note", note ?? "");
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.ExecuteNonQuery();
        }

        public void UpdateTransactionPayment(int transactionId, decimal amountPaid, decimal dueAmount, TransactionStatus status)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "UPDATE transactions SET ts_payment_amount=@paid, ts_due_amount=@due, ts_status=@status, updated_at=NOW() WHERE ts_id=@txId", conn);
            cmd.Parameters.AddWithValue("@paid", amountPaid);
            cmd.Parameters.AddWithValue("@due", dueAmount);
            cmd.Parameters.AddWithValue("@status", (int)status);
            cmd.Parameters.AddWithValue("@txId", transactionId);
            cmd.ExecuteNonQuery();
        }

        ///////////////// END CICILAN /////////////////////////////















    }
}


