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


        public Item GetSinglePendingItemById(int pt_id)
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
    pt.unitid AS unit_id,
    pt.unit AS unit_name,
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
    i.flag,
    pt.tsd_conversion_rate
FROM pending_transactions pt

LEFT JOIN items i ON pt.item_id = i.id 
LEFT JOIN units u ON u.id = i.unit
WHERE i.deleted_at IS NULL
AND pt.pt_id = @search
ORDER BY i.stock > 0 DESC, i.name
LIMIT 1";

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@search",pt_id);

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

                conversion = dr.GetInt32(dr.GetOrdinal("tsd_conversion_rate")) ,
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
            dt.Columns.Add("pt_id", typeof(int));
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
    pt.pt_id,
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
                    reader.GetInt32(reader.GetOrdinal("pt_id")),
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



        /////////////////////////////////////////////// ORDER ////////////////////////////////////
        ///// Ambil total dan diskon
        public (decimal GrandTotal, decimal TotalDiscount, int TotalItems) GetPendingTotals(string sessionCode, int terminalId, int cashierId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            string sql = @"
SELECT 
    COALESCE(SUM(total),0) AS grand_total,
    COALESCE(SUM(discount_total),0) AS total_discount,
    COUNT(*) AS total_items
FROM pending_transactions
WHERE cart_session_code = @sessionCode AND terminal_id = @terminalId AND cashier_id = @cashierId";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@sessionCode", sessionCode);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@cashierId", cashierId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (
                    GrandTotal: Convert.ToDecimal(reader["grand_total"]),
                    TotalDiscount: Convert.ToDecimal(reader["total_discount"]),
                    TotalItems: Convert.ToInt32(reader["total_items"])
                );
            }
            return (0, 0, 0);
        }

        // Insert pending_orders
        public int InsertPendingOrder(int terminalId, int cashierId, string customerName, string note, decimal total, decimal discount, string cartSessionCode, NpgsqlConnection conn, NpgsqlTransaction tran)
        {
            string sql = @"
INSERT INTO pending_orders(
    terminal_id, cashier_id, customer_name, note, total, global_discount, status, po_cart_session_code
) VALUES (
    @terminalId, @cashierId, @customerName, @note, @total, @discount, 'draft', @cartCode
) RETURNING po_id";

            using var cmd = new NpgsqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@cashierId", cashierId);
            cmd.Parameters.AddWithValue("@customerName", customerName);
            cmd.Parameters.AddWithValue("@note", note);
            cmd.Parameters.AddWithValue("@total", total);
            cmd.Parameters.AddWithValue("@discount", discount);
            cmd.Parameters.AddWithValue("@cartCode", cartSessionCode);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Link pending_transactions ke pending_orders
        public void LinkPendingTransactionsToOrder(int poId, string cartSessionCode, int terminalId, int cashierId, NpgsqlConnection conn, NpgsqlTransaction tran)
        {
            string sql = @"
UPDATE pending_transactions
SET po_id = @poId, updated_at = CURRENT_TIMESTAMP
WHERE cart_session_code = @cartCode AND terminal_id = @terminalId AND cashier_id = @cashierId";

            using var cmd = new NpgsqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@poId", poId);
            cmd.Parameters.AddWithValue("@cartCode", cartSessionCode);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@cashierId", cashierId);

            cmd.ExecuteNonQuery();
        }

        public List<PendingOrderDto> GetDraftOrders(int terminalId, int cashierId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
SELECT 
    po_id,
    po_cart_session_code,
    customer_name,   -- tambahin
    note,            -- tambahin
    total,
    global_discount,
    created_at
FROM pending_orders
WHERE terminal_id = @terminalId
  AND cashier_id = @cashierId
  AND status = 'draft'
ORDER BY created_at DESC";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@cashierId", cashierId);

            var list = new List<PendingOrderDto>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PendingOrderDto
                {
                    PoId = reader.GetInt32(0),
                    CartSessionCode = reader.GetString(1),

                    CustomerName = reader.IsDBNull(2)
                        ? null
                        : reader.GetString(2),

                    Note = reader.IsDBNull(3)
                        ? null
                        : reader.GetString(3),

                    Total = reader.GetDecimal(4),
                    Discount = reader.GetDecimal(5),
                    CreatedAt = reader.GetDateTime(6)
                });
            }

            return list;
        }

        public string GetCartSessionCodeByPoId(int poId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
SELECT po_cart_session_code
FROM pending_orders
WHERE po_id = @poId
LIMIT 1";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@poId", poId);

            var result = cmd.ExecuteScalar();
            return result?.ToString();
        }

        public DataTable GetPendingCartsByCashier(int cashierId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            string sql = @"
SELECT 
    cart_session_code,
    COUNT(*) AS total_items,
    COALESCE(SUM(total),0) AS grand_total,
    MAX(updated_at) AS last_update
FROM pending_transactions
WHERE cashier_id = @cashierId
GROUP BY cart_session_code
ORDER BY last_update DESC";
            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@cashierId", cashierId);
            using var da = new NpgsqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }






        ///////////////////////////////////////////////END ORDER //////////////////////////////////



        ////////////////////////////////// INSTALLMENTS////////////////////////////////

        public bool PayInstallmentDb(
            string cartSessionCode,
            decimal amount,
            string customerName,
            string note,
            int userId
        )
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var tran = conn.BeginTransaction();

            try
            {
                int tsId;
                decimal dueAmount;

                // 1️⃣ ambil transaksi berdasarkan cart_session_code + lock
                using (var cmd = new NpgsqlCommand(@"
            SELECT ts_id, ts_due_amount
            FROM transactions
            WHERE cart_session_code = @code
            FOR UPDATE
        ", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@code", cartSessionCode);

                    using var reader = cmd.ExecuteReader();

                    if (!reader.Read())
                        throw new Exception("Transaksi tidak ditemukan.");

                    tsId = reader.GetInt32(0);
                    dueAmount = reader.GetDecimal(1);
                }

                if (dueAmount <= 0)
                    throw new InvalidOperationException("Transaksi sudah lunas.");

                if (amount > dueAmount)
                    throw new InvalidOperationException("Pembayaran melebihi sisa tagihan.");

                decimal newDue = dueAmount - amount;

                // 2️⃣ insert cicilan
                using (var cmd = new NpgsqlCommand(@"
            INSERT INTO transaction_installments
            (transaction_id, amount, note, created_by, created_at)
            VALUES (@tsId, @amount, @note, @userId, NOW())
        ", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@tsId", tsId);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@note",
                        string.IsNullOrWhiteSpace(note) ? (object)DBNull.Value : note);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    cmd.ExecuteNonQuery();
                }

                short newStatus = newDue == 0
                    ? (short)TransactionStatus.Paid
                    : (short)TransactionStatus.Partial;

                // 3️⃣ update transaksi
                using (var cmd = new NpgsqlCommand(@"
            UPDATE transactions
            SET 
                ts_due_amount = @due,
                ts_status = @status,
                ts_freename = COALESCE(@customer, ts_freename),
                ts_note = COALESCE(@note, ts_note),
                updated_at = NOW()
            WHERE ts_id = @id
        ", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@due", newDue);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@customer",
                        string.IsNullOrWhiteSpace(customerName) ? (object)DBNull.Value : customerName);
                    cmd.Parameters.AddWithValue("@note",
                        string.IsNullOrWhiteSpace(note) ? (object)DBNull.Value : note);
                    cmd.Parameters.AddWithValue("@id", tsId);

                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }


        ////////////////////////////////// END INSTALLMENTS ///////////////////////////
































    }
}
