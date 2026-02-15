using Npgsql;
using POS_qu.DTO;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace POS_qu.Repositories
{
    public class CartActivity
    {

        public Item GetItemById(int id)
        {
            string sql = @"
SELECT 
    i.id, 
    i.name, 
    i.barcode, 
    i.sell_price, 
    i.buy_price, 
    i.stock, 
    i.reserved_stock, 
    i.unit AS unit_id,
    u.name AS unit_name,
    i.category_id, 
    i.note, 
    i.picture, 
    i.is_inventory_p, 
    i.is_purchasable, 
    i.is_sellable,
    i.is_note_payment, 
    i.is_changeprice_p, 
    i.is_have_bahan, 
    i.is_box, 
    i.is_produksi,
    i.discount_formula, 
    i.created_at, 
    i.updated_at, 
    i.deleted_at, 
    i.supplier_id, 
    i.flag
FROM items i
LEFT JOIN units u ON u.id = i.unit
WHERE i.deleted_at IS NULL
AND i.id = @id
LIMIT 1";

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            return new Item
            {
                id = Convert.ToInt32(dr.GetInt64(dr.GetOrdinal("id"))),
                name = dr.GetString(dr.GetOrdinal("name")),
                barcode = dr.IsDBNull(dr.GetOrdinal("barcode")) ? "" : dr.GetString(dr.GetOrdinal("barcode")),
                sell_price = dr.GetDecimal(dr.GetOrdinal("sell_price")),
                buy_price = dr.GetDecimal(dr.GetOrdinal("buy_price")),
                stock = Convert.ToInt32(dr.GetDouble(dr.GetOrdinal("stock"))),
                reserved_stock = Convert.ToInt32(dr.GetDouble(dr.GetOrdinal("reserved_stock"))),

                unitid = dr.GetInt32(dr.GetOrdinal("unit_id")),
                unit = dr["unit_name"]?.ToString() ?? "",

                category_id = dr.IsDBNull(dr.GetOrdinal("category_id")) ? 0 : dr.GetInt32(dr.GetOrdinal("category_id")),
                note = dr["note"]?.ToString() ?? "",
                picture = dr["picture"]?.ToString() ?? "",
                is_inventory_p = dr.GetBoolean(dr.GetOrdinal("is_inventory_p")),
                IsPurchasable = dr.GetBoolean(dr.GetOrdinal("is_purchasable")),
                IsSellable = dr.GetBoolean(dr.GetOrdinal("is_sellable")),
                RequireNotePayment = dr.GetBoolean(dr.GetOrdinal("is_note_payment")),
                is_changeprice_p = dr.GetBoolean(dr.GetOrdinal("is_changeprice_p")),
                HasMaterials = dr.GetBoolean(dr.GetOrdinal("is_have_bahan")),
                IsPackage = dr.GetBoolean(dr.GetOrdinal("is_box")),
                IsProduced = dr.GetBoolean(dr.GetOrdinal("is_produksi")),
                discount_formula = dr["discount_formula"]?.ToString() ?? "",
                created_at = dr.GetDateTime(dr.GetOrdinal("created_at")),
                updated_at = dr.GetDateTime(dr.GetOrdinal("updated_at")),
                deleted_at = dr.IsDBNull(dr.GetOrdinal("deleted_at")) ? null : dr.GetDateTime(dr.GetOrdinal("deleted_at")),
                supplier_id = dr.IsDBNull(dr.GetOrdinal("supplier_id")) ? 0 : dr.GetInt32(dr.GetOrdinal("supplier_id")),
                flag = dr.IsDBNull(dr.GetOrdinal("flag")) ? 0 : dr.GetInt32(dr.GetOrdinal("flag")),

                conversion = 1,
                price_per_pcs = dr.GetDecimal(dr.GetOrdinal("sell_price")),
                price_per_pcs_asli = dr.GetDecimal(dr.GetOrdinal("buy_price"))
            };
        }


        public Item GetSingleItemByName(string search)
        {
            string sql = @"
SELECT 
    i.id, 
    i.name, 
    i.barcode, 
    i.sell_price, 
    i.buy_price, 
    i.stock, 
    i.reserved_stock, 
    i.unit AS unit_id,
    u.name AS unit_name,
    i.category_id, 
    i.note, 
    i.picture, 
    i.is_inventory_p, 
    i.is_purchasable, 
    i.is_sellable,
    i.is_note_payment, 
    i.is_changeprice_p, 
    i.is_have_bahan, 
    i.is_box, 
    i.is_produksi,
    i.discount_formula, 
    i.created_at, 
    i.updated_at, 
    i.deleted_at, 
    i.supplier_id, 
    i.flag
FROM items i
LEFT JOIN units u ON u.id = i.unit
WHERE i.deleted_at IS NULL
AND i.name ILIKE @search
ORDER BY i.stock > 0 DESC, i.name
LIMIT 1";

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@search", "%" + search + "%");

            using var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            return new Item
            {
                id = Convert.ToInt32(dr.GetInt64(dr.GetOrdinal("id"))),
                name = dr.GetString(dr.GetOrdinal("name")),
                barcode = dr.IsDBNull(dr.GetOrdinal("barcode")) ? "" : dr.GetString(dr.GetOrdinal("barcode")),
                sell_price = dr.GetDecimal(dr.GetOrdinal("sell_price")),
                buy_price = dr.GetDecimal(dr.GetOrdinal("buy_price")),
                stock = Convert.ToInt32(dr.GetDouble(dr.GetOrdinal("stock"))),
                reserved_stock = Convert.ToInt32(dr.GetDouble(dr.GetOrdinal("reserved_stock"))),

                // 🔥 FIX DI SINI
                unitid = dr.GetInt32(dr.GetOrdinal("unit_id")),     // INT
                unit = dr["unit_name"]?.ToString() ?? "",            // STRING (nama unit)

                category_id = dr.IsDBNull(dr.GetOrdinal("category_id")) ? 0 : dr.GetInt32(dr.GetOrdinal("category_id")),
                note = dr["note"]?.ToString() ?? "",
                picture = dr["picture"]?.ToString() ?? "",
                is_inventory_p = dr.GetBoolean(dr.GetOrdinal("is_inventory_p")),
                IsPurchasable = dr.GetBoolean(dr.GetOrdinal("is_purchasable")),
                IsSellable = dr.GetBoolean(dr.GetOrdinal("is_sellable")),
                RequireNotePayment = dr.GetBoolean(dr.GetOrdinal("is_note_payment")),
                is_changeprice_p = dr.GetBoolean(dr.GetOrdinal("is_changeprice_p")),
                HasMaterials = dr.GetBoolean(dr.GetOrdinal("is_have_bahan")),
                IsPackage = dr.GetBoolean(dr.GetOrdinal("is_box")),
                IsProduced = dr.GetBoolean(dr.GetOrdinal("is_produksi")),
                discount_formula = dr["discount_formula"]?.ToString() ?? "",
                created_at = dr.GetDateTime(dr.GetOrdinal("created_at")),
                updated_at = dr.GetDateTime(dr.GetOrdinal("updated_at")),
                deleted_at = dr.IsDBNull(dr.GetOrdinal("deleted_at")) ? null : dr.GetDateTime(dr.GetOrdinal("deleted_at")),
                supplier_id = dr.IsDBNull(dr.GetOrdinal("supplier_id")) ? 0 : dr.GetInt32(dr.GetOrdinal("supplier_id")),
                flag = dr.IsDBNull(dr.GetOrdinal("flag")) ? 0 : dr.GetInt32(dr.GetOrdinal("flag")),

                conversion = 1,
                price_per_pcs = dr.GetDecimal(dr.GetOrdinal("sell_price")),
                price_per_pcs_asli = dr.GetDecimal(dr.GetOrdinal("buy_price"))
            };
        }





        public int GetPendingItemQty(
      int terminalId,
      int itemId,
      int unitId,
      string cartSessionCode)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
SELECT quantity
FROM pending_transactions
WHERE terminal_id = @terminalId
  AND item_id = @itemId
  AND unitid = @unitId
  AND cart_session_code = @cartSessionCode
LIMIT 1";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@unitId", unitId);
            cmd.Parameters.AddWithValue("@cartSessionCode", cartSessionCode);

            var result = cmd.ExecuteScalar();

            if (result == null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }





        public DataTable GetPendingItems(string sessionCode)
        {
            var dt = new DataTable();

            // Tambahkan kolom sesuai InvoiceBuilder
            dt.Columns.Add("item_id", typeof(int));
            dt.Columns.Add("barcode", typeof(string));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("unit", typeof(string));
            dt.Columns.Add("unitid", typeof(string));
            dt.Columns.Add("unit_variant", typeof(string));
            dt.Columns.Add("conversion_rate", typeof(int));
            dt.Columns.Add("qty", typeof(decimal));
            dt.Columns.Add("sell_price", typeof(decimal));
            dt.Columns.Add("buy_price", typeof(decimal));
            dt.Columns.Add("disc_percent", typeof(decimal));
            dt.Columns.Add("disc_amount", typeof(decimal));
            dt.Columns.Add("tax", typeof(decimal));
            dt.Columns.Add("note", typeof(string));
            dt.Columns.Add("total", typeof(decimal));

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
SELECT 
    pt.item_id,
    pt.barcode,
    i.name,
    pt.quantity AS qty,
    pt.unit,
    pt.unitid,
    u.name AS unit_variant,
    COALESCE(pt.tsd_conversion_rate, 1) AS conversion_rate,
    pt.sell_price,
    pt.buy_price,
    pt.discount_percentage AS disc_percent,
    pt.discount_total AS disc_amount,
    pt.tax,
    pt.note,
    pt.total
FROM pending_transactions pt
LEFT JOIN items i ON i.id = pt.item_id
LEFT JOIN units u ON u.id = i.unit
WHERE pt.cart_session_code = @session
ORDER BY pt.created_at;";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@session", sessionCode);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dt.Rows.Add(
                    reader.GetInt32(reader.GetOrdinal("item_id")),
                    reader.GetString(reader.GetOrdinal("barcode")),
                    reader["name"]?.ToString() ?? "",
                    reader.GetString(reader.GetOrdinal("unit")),
                    reader.GetInt32(reader.GetOrdinal("unitid")),
                    reader["unit_variant"]?.ToString() ?? "",
                    reader["conversion_rate"] != DBNull.Value ? Convert.ToInt32(reader["conversion_rate"]) : 1,
                    reader["qty"] != DBNull.Value ? Convert.ToDecimal(reader["qty"]) : 0,
                    reader.GetDecimal(reader.GetOrdinal("sell_price")),
                    reader.GetDecimal(reader.GetOrdinal("buy_price")),
                    reader["disc_percent"] != DBNull.Value ? Convert.ToDecimal(reader["disc_percent"]) : 0,
                    reader["disc_amount"] != DBNull.Value ? Convert.ToDecimal(reader["disc_amount"]) : 0,
                    reader["tax"] != DBNull.Value ? Convert.ToDecimal(reader["tax"]) : 0,
                    reader["note"]?.ToString() ?? "",
                    reader["total"] != DBNull.Value ? Convert.ToDecimal(reader["total"]) : 0
                );
            }

            return dt;
        }

        public void DeletePendingItem(string sessionCode, int itemId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
            DELETE FROM pending_transactions
            WHERE cart_session_code = @sessionCode
              AND item_id = @itemId
        ";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@sessionCode", sessionCode);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.ExecuteNonQuery();
        }

        public void UpdatePendingItemQty(string sessionCode, int itemId, decimal newQty)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
UPDATE pending_transactions
SET quantity = @qty,
    total = sell_price * @qty
WHERE cart_session_code = @sessionCode
  AND item_id = @itemId
";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@sessionCode", sessionCode);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@qty", newQty);

            cmd.ExecuteNonQuery();
        }

        public DeleteCartItemRequest? GetPendingItemById(string sessionCode, int itemId, int unitId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
SELECT 
    pt.item_id,
    pt.unitid,
    pt.barcode,
    pt.quantity,
    COALESCE(pt.tsd_conversion_rate, 1) AS conversion_rate
FROM pending_transactions pt
WHERE pt.cart_session_code = @session
  AND pt.item_id = @itemId
  AND pt.unitid = @unitId
LIMIT 1;";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@session", sessionCode);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@unitId", unitId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new DeleteCartItemRequest
            {
                ItemId = reader.GetInt32(reader.GetOrdinal("item_id")),
                UnitId = reader.GetInt32(reader.GetOrdinal("unitid")),
                Barcode = reader["barcode"]?.ToString() ?? "",
                Quantity = reader["quantity"] != DBNull.Value
                                ? Convert.ToInt32(reader["quantity"])
                                : 0,
                ConversionRate = reader["conversion_rate"] != DBNull.Value
                                ? Convert.ToInt32(reader["conversion_rate"])
                                : 1,
                CartSessionCode = sessionCode
            };
        }























        public bool UpdatePendingTransactionStock(
      NpgsqlConnection conn,
      NpgsqlTransaction tran,
      int terminalId,
      int itemId,
      decimal newQuantity,
      decimal newTotal,
      string unit,
      decimal sell_price,
      string cartSessionCode
  )
        {
            string query = @"
UPDATE pending_transactions
SET quantity = @newQuantity,
    sell_price = @sell_price,
    total = @newTotal,
    updated_at = CURRENT_TIMESTAMP
WHERE terminal_id = @terminalId
  AND item_id = @itemId
  AND unit = @unit
  AND cart_session_code = @cartSessionCode";

            using var cmd = new NpgsqlCommand(query, conn, tran); // pakai conn + tran dari luar
            cmd.Parameters.AddWithValue("@newQuantity", newQuantity);
            cmd.Parameters.AddWithValue("@newTotal", newTotal);
            cmd.Parameters.AddWithValue("@sell_price", sell_price);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@unit", unit);
            cmd.Parameters.AddWithValue("@cartSessionCode", cartSessionCode);

            int affected = cmd.ExecuteNonQuery();

            return affected > 0;
        }


        public class PriceDecisionResult
        {
            public decimal Price { get; set; }       // Harga final yang berlaku
            public bool HasMultiPrice { get; set; }  // Apakah item ini pakai multi-price
        }

        public PriceDecisionResult DecideMultiPriceFromDb(int itemId, int unitId, int qty)
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();

                string sql = @"
            SELECT price
            FROM item_prices
            WHERE item_id = @itemId
              AND unit_id = @unitId
              AND min_qty <= @qty
              AND is_active = TRUE
              AND (effective_to IS NULL OR effective_to >= NOW())
            ORDER BY min_qty DESC
            LIMIT 1
        ";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@unitId", unitId);
                    cmd.Parameters.AddWithValue("@qty", qty);

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return new PriceDecisionResult
                        {
                            Price = Convert.ToDecimal(result),
                            HasMultiPrice = true
                        };
                    }
                }
            }

            // fallback harga normal PER UNIT
            decimal normalPrice = GetItemPrice(itemId, unitId);

            return new PriceDecisionResult
            {
                Price = normalPrice,
                HasMultiPrice = false
            };
        }



        public bool UpsertPendingItem(
      NpgsqlConnection conn,
      NpgsqlTransaction tran,
      string sessionCode,
      int itemId,
      decimal qty,
      string unit,
      int unitid,
      decimal sell_price,
      decimal total,
      decimal conversionRate
  )
        {
            string sql = @"
INSERT INTO pending_transactions
(cart_session_code, terminal_id, cashier_id, item_id, barcode,
 quantity, unitid, unit, sell_price, buy_price, total, tsd_conversion_rate)
SELECT
    @session,
    @terminal,
    @cashier,
    i.id,
    i.barcode,
    @qty,
    @unitid,
    @unit,                         
    @sell_price,
    i.buy_price,
    @total,
    @conversionRate
FROM items i
WHERE i.id = @item
";

            using var cmd = new NpgsqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@session", sessionCode);
            cmd.Parameters.AddWithValue("@item", itemId);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@unit", unit);
            cmd.Parameters.AddWithValue("@unitid", unitid);
            cmd.Parameters.AddWithValue("@sell_price", sell_price);
            cmd.Parameters.AddWithValue("@total", total);
            cmd.Parameters.AddWithValue("@conversionRate", conversionRate);

            var session = SessionUser.GetCurrentUser();
            cmd.Parameters.AddWithValue("@terminal", session.TerminalId);
            cmd.Parameters.AddWithValue("@cashier", session.UserId);

            int affected = cmd.ExecuteNonQuery();
            return affected > 0;
        }


        public bool TryUpdateReservedStock(
      NpgsqlConnection conn,
      NpgsqlTransaction tran,
      int itemId,
      int baseQtyDiff)
        {
            string sql = @"
UPDATE items
SET reserved_stock = reserved_stock + @baseQtyDiff
WHERE id = @itemId
AND (reserved_stock + @baseQtyDiff) >= 0
AND (reserved_stock + @baseQtyDiff) <= stock;
";

            using var cmd = new NpgsqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@baseQtyDiff", baseQtyDiff);
            cmd.Parameters.AddWithValue("@itemId", itemId);

            return cmd.ExecuteNonQuery() > 0;
        }















































        public decimal GetItemPrice(int itemId, int unitId)
        {
            try
            {
                using var con = new NpgsqlConnection(DbConfig.ConnectionString);
                con.Open();

                // Ambil harga dari unit variant
                string sql = @"
            SELECT sell_price 
            FROM unit_variants
            WHERE item_id = @itemId AND unit_id = @unitId AND is_active = TRUE
            LIMIT 1";

                using var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@itemId", itemId);
                cmd.Parameters.AddWithValue("@unitId", unitId);

                var result = cmd.ExecuteScalar();

                if (result != null)
                    return Convert.ToDecimal(result);

                // fallback ke harga item biasa
                sql = "SELECT sell_price FROM items WHERE id = @itemId";
                using var cmd2 = new NpgsqlCommand(sql, con);
                cmd2.Parameters.AddWithValue("@itemId", itemId);
                result = cmd2.ExecuteScalar();

                return result != null ? Convert.ToDecimal(result) : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving price: {ex.Message}");
                return 0;
            }
        }



        public bool UpdatePendingTransactionDiscount(int terminalId, int itemId, decimal discountPercentage, decimal discountTotal)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = "UPDATE pending_transactions SET discount_percentage = @discountPercentage, discount_total = @discountTotal, updated_at = CURRENT_TIMESTAMP WHERE terminal_id = @terminalId AND item_id = @itemId";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@discountPercentage", discountPercentage);
                    cmd.Parameters.AddWithValue("@discountTotal", discountTotal);
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeletePendingTransaction(int terminalId, int userId, int itemId, int unitId, string cart_session_code)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();

                string query = @"
DELETE FROM pending_transactions 
WHERE terminal_id = @terminalId 
  AND item_id = @itemId
  AND unitid = @unitId
  AND cashier_id = @userId
  AND cart_session_code = @cart_session_code";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@unitId", unitId);
                    cmd.Parameters.AddWithValue("@cart_session_code", cart_session_code);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public int GetItemReservedStock(string barcode)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    string sql = "SELECT reserved_stock FROM items WHERE barcode = @barcode";
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@barcode", barcode);

                        var result = vCmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0; // Return the current stock, or 0 if not found
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving stock: {ex.Message}");
                return 0;
            }
        }

        public void UpdateReservedStock(string barcode, int newReservedStock)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string sql = "UPDATE items SET reserved_stock = @reservedStock WHERE barcode = @barcode";

                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    vCmd.Parameters.AddWithValue("@reservedStock", newReservedStock);
                    vCmd.Parameters.AddWithValue("@barcode", barcode);
                    vCmd.ExecuteNonQuery();
                }
            }
        }

        public int GetItemStock(string barcode)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    string sql = "SELECT stock FROM items WHERE barcode = @barcode";
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@barcode", barcode);

                        var result = vCmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0; // Return the current stock, or 0 if not found
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving stock: {ex.Message}");
                return 0;
            }
        }

        public bool UpdateItemStock(string barcode, int newStock)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    string sql = "UPDATE items SET stock = @stock WHERE barcode = @barcode";
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@stock", newStock);
                        vCmd.Parameters.AddWithValue("@barcode", barcode);
                        int rowsAffected = vCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }




        public List<UnitVariant> GetVariantsByItemId(int itemId)
        {
            const string sql = @"
SELECT 
    uv.id,
    uv.item_id,
    uv.unit_id,
    u.name AS unit_name,
    uv.conversion,
    uv.sell_price,
    uv.profit,
    uv.minqty,
    uv.is_base_unit,
    uv.barcode_suffix
FROM unit_variants uv
JOIN units u ON u.id = uv.unit_id
WHERE uv.item_id = @itemId
AND uv.is_active = TRUE
ORDER BY uv.is_base_unit DESC, u.name";

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@itemId", itemId);

            using var dr = cmd.ExecuteReader();

            var list = new List<UnitVariant>();

            while (dr.Read())
            {
                list.Add(new UnitVariant
                {
                    Id = dr.GetInt32(dr.GetOrdinal("id")),
                    ItemId = dr.GetInt32(dr.GetOrdinal("item_id")),
                    UnitId = dr.GetInt32(dr.GetOrdinal("unit_id")),
                    UnitName = dr.GetString(dr.GetOrdinal("unit_name")),
                    Conversion = dr.GetInt32(dr.GetOrdinal("conversion")),
                    SellPrice = dr.GetDecimal(dr.GetOrdinal("sell_price")),
                    Profit = dr.GetDecimal(dr.GetOrdinal("profit")),
                    MinQty = dr.IsDBNull(dr.GetOrdinal("minqty"))
                        ? 0
                        : dr.GetDecimal(dr.GetOrdinal("minqty")),
                    IsBaseUnit = dr.GetBoolean(dr.GetOrdinal("is_base_unit")),
                    BarcodeSuffix = dr.IsDBNull(dr.GetOrdinal("barcode_suffix"))
                        ? null
                        : dr.GetString(dr.GetOrdinal("barcode_suffix"))
                });
            }

            return list;
        }

        public UnitVariant GetVariantById(int variantId)
        {
            const string sql = @"
SELECT 
    uv.id,
    uv.item_id,
    uv.unit_id,
    u.name AS unit_name,
    uv.conversion,
    uv.sell_price,
    uv.profit,
    uv.minqty,
    uv.is_base_unit,
    uv.barcode_suffix
FROM unit_variants uv
JOIN units u ON u.id = uv.unit_id
WHERE uv.id = @id
AND uv.is_active = TRUE";

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", variantId);

            using var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            return new UnitVariant
            {
                Id = dr.GetInt32(dr.GetOrdinal("id")),
                ItemId = dr.GetInt32(dr.GetOrdinal("item_id")),
                UnitId = dr.GetInt32(dr.GetOrdinal("unit_id")),
                UnitName = dr.GetString(dr.GetOrdinal("unit_name")),
                Conversion = dr.GetInt32(dr.GetOrdinal("conversion")),
                SellPrice = dr.GetDecimal(dr.GetOrdinal("sell_price")),
                Profit = dr.GetDecimal(dr.GetOrdinal("profit")),
                MinQty = dr.IsDBNull(dr.GetOrdinal("minqty"))
                    ? 0
                    : dr.GetDecimal(dr.GetOrdinal("minqty")),
                IsBaseUnit = dr.GetBoolean(dr.GetOrdinal("is_base_unit")),
                BarcodeSuffix = dr.IsDBNull(dr.GetOrdinal("barcode_suffix"))
                    ? null
                    : dr.GetString(dr.GetOrdinal("barcode_suffix"))
            };
        }





































    }
}
