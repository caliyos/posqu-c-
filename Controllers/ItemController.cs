using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using POS_qu.Models;
using System.Transactions;
using Microsoft.VisualBasic.Devices;

namespace POS_qu.Controllers
{
    class ItemController
    {
        private string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";

        public DataTable GetItems()
        {
            MessageBox.Show("GetItems no param called.");
            DataTable dt = new DataTable();
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = @"
            SELECT 
                items.name, 
                items.barcode, 
                items.buy_price, 
                items.sell_price, 
                items.stock, 
                units.name AS unit, 
                items.reserved_stock
            FROM 
                items
            LEFT JOIN 
                units ON items.unit = units.id
            WHERE 
                items.deleted_at IS NULL
        ";
                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    NpgsqlDataReader dr = vCmd.ExecuteReader();
                    dt.Load(dr);
                }
            }
            return dt;
        }

           public DataTable GetItems(string searchTerm = null)
        {
            DataTable dt = new DataTable();

            // Build the base SQL query
            string sql = @"SELECT
            items.id, 
            items.name, 
            items.barcode, 
            items.buy_price, 
            items.sell_price, 
            items.stock, 
            units.name AS unit, 
            items.reserved_stock  FROM 
            items
        LEFT JOIN 
            units ON items.unit = units.id
        WHERE 
            items.deleted_at IS NULL ";
            //    string sql = @"
            //    SELECT 
            //        items.name, 
            //        items.barcode, 
            //        items.buy_price, 
            //        items.sell_price, 
            //        items.stock, 
            //        units.name AS unit, 
            //        items.reserved_stock
            //    FROM 
            //        items
            //    LEFT JOIN 
            //        units ON items.unit = units.id
            //    WHERE 
            //        items.deleted_at IS NULL
            //";


            // If searchTerm is provided, add search condition
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sql += " AND items.name ILIKE @searchTerm"; // Use ILIKE for case-insensitive partial match
            }


            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();

                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    // If searchTerm is provided, add it as a parameter to prevent SQL injection
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        vCmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    }

                    using (NpgsqlDataReader dr = vCmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }

            return dt;
        }

        public DataTable GetUnits()
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = @"
            SELECT 
                id, 
                name || ' (' || abbr || ')' AS display 
            FROM units 
            ORDER BY name";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, vCon))
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                }
            }
            return dt;
        }

        public DataTable GetGroups()
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = @"
            SELECT 
                id, 
                groupname || ' (' || groupshortname || ')' AS display 
            FROM groups 
            ORDER BY groupname";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, vCon))
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                }
            }
            return dt;
        }



        public int GetItemStock(string barcode)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
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

        public decimal GetItemPrice(int id)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
                {
                    vCon.Open();
                    string sql = "SELECT sell_price FROM items WHERE id = @id";
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@id", id);

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
        public int GetItemReservedStock(string barcode)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
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


        public int GetItemStock(int id)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
                {
                    vCon.Open();
                    string sql = "SELECT stock FROM items WHERE id = @id";
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@id", id);

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
        public int GetItemReservedStock(int id)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
                {
                    vCon.Open();
                    string sql = "SELECT reserved_stock FROM items WHERE id = @id";
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@id", id);

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
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
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

        public bool UpdateItemStockAndReservedStock(int id, int newStock, int rStock)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
                {
                    vCon.Open();
                    string sql = "UPDATE items SET stock = @stock, reserved_stock = @rStock WHERE id = @id";
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@stock", newStock);
                        vCmd.Parameters.AddWithValue("@rStock", rStock);
                        vCmd.Parameters.AddWithValue("@id", id);
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

        public int GetItemIdById(int id)
        {
            using (var conn = new NpgsqlConnection(vStrConnection))
            {
                conn.Open();
                string query = "SELECT id FROM items WHERE id = @id ORDER BY id DESC LIMIT 1";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }


     


        public bool DeleteUnitVariantsByItemId(int itemId)
        {
            using (var conn = new NpgsqlConnection(vStrConnection))
            {
                conn.Open();
                string query = "DELETE FROM unit_variants WHERE item_id = @item_id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@item_id", itemId);
                    return cmd.ExecuteNonQuery() >= 0;
                }
            }
        }

        public List<UnitVariant> GetUnitVariant(int itemId)
        {
            List<UnitVariant> variants = new List<UnitVariant>();

            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();

                string query = @"SELECT uv.unit_id, u.name AS unit_name, uv.conversion, uv.sell_price
                         FROM unit_variants uv
                         JOIN units u ON uv.unit_id = u.id
                         WHERE uv.item_id = @item_id";

                using (NpgsqlCommand vCmd = new NpgsqlCommand(query, vCon))
                {
                    vCmd.Parameters.AddWithValue("@item_id", itemId);

                    using (NpgsqlDataReader reader = vCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            variants.Add(new UnitVariant
                            {
                                UnitId = Convert.ToInt32(reader["unit_id"]),
                                UnitName = reader["unit_name"].ToString(),
                                Conversion = Convert.ToInt32(reader["conversion"]),
                                SellPrice = Convert.ToDecimal(reader["sell_price"])
                            });
                        }
                    }
                }
            }

            return variants;
        }

        public bool InsertUnitVariant(int itemId, UnitVariant variant)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
                {
                    vCon.Open();

                    string query = @"
                INSERT INTO unit_variants (
                    item_id, unit_id, conversion, sell_price
                ) VALUES (
                    @item_id, @unit_id, @conversion, @sell_price
                );";

                    using (NpgsqlCommand vCmd = new NpgsqlCommand(query, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@item_id", itemId);
                        vCmd.Parameters.AddWithValue("@unit_id", variant.UnitId);
                        vCmd.Parameters.AddWithValue("@conversion", variant.Conversion);
                        vCmd.Parameters.AddWithValue("@sell_price", variant.SellPrice);
                        //vCmd.Parameters.AddWithValue("@is_base_unit", variant.IsBaseUnit);
                        //vCmd.Parameters.AddWithValue("@barcode_suffix", (object)variant.BarcodeSuffix ?? DBNull.Value);

                        return vCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting unit variant: " + ex.Message);
                return false;
            }
        }


        public int? InsertItem(Item item)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
                {
                    vCon.Open();
                    string sql = @"
                INSERT INTO items (
                    name, buy_price, sell_price, barcode, stock, reserved_stock, unit, ""group"", 
                    is_inventory_p, is_changeprice_p, materials, note, picture, created_at, 
                    updated_at, deleted_at, supplier_id, flag
                ) VALUES (
                    @name, @buy_price, @sell_price, @barcode, @stock, @reserved_stock, @unit, @group, 
                    @is_inventory_p, @is_changeprice_p, @materials, @note, @picture, @created_at, 
                    @updated_at, @deleted_at, @supplier_id, @flag
                )
                RETURNING id;"; // 👈 This returns the newly inserted item's ID

                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@name", item.name);
                        vCmd.Parameters.AddWithValue("@buy_price", item.buy_price);
                        vCmd.Parameters.AddWithValue("@sell_price", item.sell_price);
                        vCmd.Parameters.AddWithValue("@barcode", item.barcode ?? "");
                        vCmd.Parameters.AddWithValue("@stock", item.stock);
                        vCmd.Parameters.AddWithValue("@reserved_stock", item.reserved_stock);
                        vCmd.Parameters.AddWithValue("@unit", item.unit ?? "");
                        vCmd.Parameters.AddWithValue("@group", item.group);
                        vCmd.Parameters.AddWithValue("@is_inventory_p", item.is_inventory_p ?? "Y");
                        vCmd.Parameters.AddWithValue("@is_changeprice_p", item.is_changeprice_p ?? "N");
                        vCmd.Parameters.AddWithValue("@materials", item.materials ?? "");
                        vCmd.Parameters.AddWithValue("@note", item.note ?? "");
                        vCmd.Parameters.AddWithValue("@picture", item.picture ?? "");
                        vCmd.Parameters.AddWithValue("@created_at", item.created_at);
                        vCmd.Parameters.AddWithValue("@updated_at", item.updated_at);
                        vCmd.Parameters.AddWithValue("@deleted_at", (object)item.deleted_at ?? DBNull.Value);
                        vCmd.Parameters.AddWithValue("@supplier_id", item.supplier_id);
                        vCmd.Parameters.AddWithValue("@flag", item.flag);

                        // Execute and return the inserted ID
                        object result = vCmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting item: " + ex.Message);
                return null;
            }
        }



        public bool UpdateItem(Item item)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
                {
                    vCon.Open();
                    string sql = @"
                UPDATE items SET 
                    name = @name,
                    buy_price = @buy_price,
                    sell_price = @sell_price,
                    barcode = @barcode,
                    stock = @stock,
                    reserved_stock = @reserved_stock,
                    unit = @unit,
                    ""group"" = @group,
                    is_inventory_p = @is_inventory_p,
                    is_changeprice_p = @is_changeprice_p,
                    materials = @materials,
                    note = @note,
                    picture = @picture,
                    updated_at = @updated_at,
                    deleted_at = @deleted_at,
                    supplier_id = @supplier_id,
                    flag = @flag
                WHERE id = @id";

                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@id", item.id);
                        vCmd.Parameters.AddWithValue("@name", item.name);
                        vCmd.Parameters.AddWithValue("@buy_price", item.buy_price);
                        vCmd.Parameters.AddWithValue("@sell_price", item.sell_price);
                        vCmd.Parameters.AddWithValue("@barcode", item.barcode ?? "");
                        vCmd.Parameters.AddWithValue("@stock", item.stock);
                        vCmd.Parameters.AddWithValue("@reserved_stock", item.reserved_stock);
                        vCmd.Parameters.AddWithValue("@unit", item.unit ?? "");
                        vCmd.Parameters.AddWithValue("@group", item.group);
                        vCmd.Parameters.AddWithValue("@is_inventory_p", item.is_inventory_p ?? "Y");
                        vCmd.Parameters.AddWithValue("@is_changeprice_p", item.is_changeprice_p ?? "N");
                        vCmd.Parameters.AddWithValue("@materials", item.materials ?? "");
                        vCmd.Parameters.AddWithValue("@note", item.note ?? "");
                        vCmd.Parameters.AddWithValue("@picture", item.picture ?? "");
                        vCmd.Parameters.AddWithValue("@updated_at", item.updated_at);
                        vCmd.Parameters.AddWithValue("@deleted_at", (object)item.deleted_at ?? DBNull.Value);
                        vCmd.Parameters.AddWithValue("@supplier_id", item.supplier_id);
                        vCmd.Parameters.AddWithValue("@flag", item.flag);

                        vCmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating item: " + ex.Message);
                return false;
            }
        }



        //public void UpdateItem(Item item)
        //{
        //    using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
        //    {
        //        vCon.Open();
        //        string sql = "UPDATE items SET name = @name, buy_price = @buy_price WHERE id = @id";
        //        using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
        //        {
        //            vCmd.Parameters.AddWithValue("@name", item.Name);
        //            vCmd.Parameters.AddWithValue("@buy_price", item.Price);
        //            vCmd.Parameters.AddWithValue("@id", item.Id);
        //            vCmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        public bool DeleteItem(int id)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
                {
                    vCon.Open();
                    string sql = @"UPDATE items SET deleted_at = @deleted_at WHERE id = @id";

                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@deleted_at", DateTime.Now);
                        vCmd.Parameters.AddWithValue("@id", id);
                        vCmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting item: " + ex.Message);
                return false;
            }
        }


        public DataTable SearchItems(string keyword)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = "SELECT * FROM items WHERE name ILIKE @keyword";
                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    vCmd.Parameters.AddWithValue("@keyword", $"%{keyword}%"); // Use ILIKE for case-insensitive search
                    NpgsqlDataReader dr = vCmd.ExecuteReader();
                    dt.Load(dr);
                }
            }
            return dt;
        }

        // Method to insert a payment record into the 'payment' table
        public int InsertTransaction(Transactions transaction)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = @"
        INSERT INTO transactions (
            ts_numbering, ts_code, ts_total, ts_payment_amount, ts_cashback, 
            ts_method, ts_status, ts_change, ts_internal_note, ts_note, 
            ts_customer, ts_freename, created_by, created_at) 
        VALUES (
            @ts_numbering, @ts_code, @ts_total, @ts_payment_amount, @ts_cashback, 
            @ts_method, @ts_status, @ts_change, @ts_internal_note, @ts_note, 
            @ts_customer, @ts_freename, @created_by, @created_at) 
        RETURNING ts_id";

                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    vCmd.Parameters.AddWithValue("@ts_numbering", transaction.TsNumbering);
                    vCmd.Parameters.AddWithValue("@ts_code", transaction.TsCode);
                    vCmd.Parameters.AddWithValue("@ts_total", transaction.TsTotal);
                    vCmd.Parameters.AddWithValue("@ts_payment_amount", transaction.TsPaymentAmount); // ✅ Add payment amount
                    vCmd.Parameters.AddWithValue("@ts_cashback", transaction.TsCashback);
                    vCmd.Parameters.AddWithValue("@ts_method", transaction.TsMethod);
                    vCmd.Parameters.AddWithValue("@ts_status", transaction.TsStatus);
                    vCmd.Parameters.AddWithValue("@ts_change", transaction.TsChange);
                    vCmd.Parameters.AddWithValue("@ts_internal_note", transaction.TsInternalNote);
                    vCmd.Parameters.AddWithValue("@ts_note", transaction.TsNote);
                    vCmd.Parameters.AddWithValue("@ts_customer", (object)transaction.TsCustomer ?? DBNull.Value);
                    vCmd.Parameters.AddWithValue("@ts_freename", transaction.TsFreename);
                    vCmd.Parameters.AddWithValue("@created_by", transaction.CreatedBy);
                    vCmd.Parameters.AddWithValue("@created_at", transaction.CreatedAt);

                    return Convert.ToInt32(vCmd.ExecuteScalar()); // Returns transaction ID
                }
            }
        }



        public void InsertTransactionDetails(List<TransactionDetail> details)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = @"
    INSERT INTO transaction_details (
        ts_id, item_id, tsd_barcode, tsd_sell_price, tsd_quantity, tsd_unit, tsd_note, 
        tsd_discount_per_item, tsd_discount_percentage, tsd_discount_total, 
        tsd_tax, tsd_total, created_by, created_at) 
    VALUES (
        @ts_id, @item_id, @tsd_barcode, @tsd_sell_price, @tsd_quantity, @tsd_unit, @tsd_note, 
        @tsd_discount_per_item, @tsd_discount_percentage, @tsd_discount_total, 
        @tsd_tax, @tsd_total, @created_by, @created_at)";

                foreach (var detail in details)
                {
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@ts_id", detail.TsId);
                        vCmd.Parameters.AddWithValue("@item_id", detail.ItemId);
                        vCmd.Parameters.AddWithValue("@tsd_barcode", detail.Barcode);
                        vCmd.Parameters.AddWithValue("@tsd_sell_price", detail.TsdSellPrice);
                        vCmd.Parameters.AddWithValue("@tsd_quantity", detail.TsdQuantity);
                        vCmd.Parameters.AddWithValue("@tsd_unit", detail.TsdUnit);
                        vCmd.Parameters.AddWithValue("@tsd_note", detail.TsdNote);
                        vCmd.Parameters.AddWithValue("@tsd_discount_per_item", detail.TsdDiscountPerItem);
                        vCmd.Parameters.AddWithValue("@tsd_discount_percentage", detail.TsdDiscountPercentage);
                        vCmd.Parameters.AddWithValue("@tsd_discount_total", detail.TsdDiscountTotal);
                        vCmd.Parameters.AddWithValue("@tsd_tax", detail.TsdTax);
                        vCmd.Parameters.AddWithValue("@tsd_total", detail.TsdTotal);
                        vCmd.Parameters.AddWithValue("@created_by", detail.CreatedBy);
                        vCmd.Parameters.AddWithValue("@created_at", detail.CreatedAt);

                        vCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////



        //update stock reserved stock 
        public void updateStockAndReservedStock (int id,decimal stock, decimal rstock)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = "UPDATE items SET reserved_stock = @reservedStock " +
                    ",stock = @stock" +
                    "WHERE id = @id";

                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    vCmd.Parameters.AddWithValue("@reservedStock", rstock);
                    vCmd.Parameters.AddWithValue("@stock", stock);
                    vCmd.Parameters.AddWithValue("@id", id);
                    vCmd.ExecuteNonQuery();
                }
            }
        }

        //  clear based on product and qty is the same
        // the rows in cart (product and qty must be the same with product and qty in pending_transactions table)
        public void ClearPendingTransaction(string barcode, decimal qty, string unit)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = "DELETE FROM pending_transactions WHERE barcode = @barcode AND quantity = @qty AND unit = @unit";

                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    vCmd.Parameters.AddWithValue("@barcode", NpgsqlTypes.NpgsqlDbType.Text, barcode);  // Ensure text type
                    vCmd.Parameters.AddWithValue("@qty", NpgsqlTypes.NpgsqlDbType.Numeric, qty);       // Ensure numeric type
                    vCmd.Parameters.AddWithValue("@unit", NpgsqlTypes.NpgsqlDbType.Text, unit);        // Ensure text type

                    int rowsAffected = vCmd.ExecuteNonQuery();
                    Console.WriteLine($"Deleted {rowsAffected} row(s)"); // Debugging output
                }
            }
        }

        public void UpdateReservedStock(string barcode, int newReservedStock)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
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
        public bool AddPendingTransaction(int terminalId, int cashierId, int itemId, string barcode, string unit, decimal quantity, decimal sellPrice, decimal discountPercentage, decimal discountTotal, decimal tax, decimal total, string note)
{
    using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
    {
        vCon.Open();
        string query = @"
            INSERT INTO pending_transactions 
            (terminal_id, cashier_id, item_id, barcode, unit, quantity, sell_price, discount_percentage, discount_total, tax, total, note) 
            VALUES (@terminalId, @cashierId, @itemId, @barcode, @unit, @quantity, @sellPrice, @discountPercentage, @discountTotal, @tax, @total, @note) 
            ON CONFLICT (terminal_id, item_id) 
            DO UPDATE SET 
                quantity = pending_transactions.quantity + EXCLUDED.quantity, 
                discount_percentage = EXCLUDED.discount_percentage,
                discount_total = EXCLUDED.discount_total,
                tax = EXCLUDED.tax,
                total = EXCLUDED.total,
                note = EXCLUDED.note,
                updated_at = CURRENT_TIMESTAMP 
            RETURNING pt_id";

        using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
        {
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@cashierId", cashierId);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@barcode", barcode);
            cmd.Parameters.AddWithValue("@unit", unit);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@sellPrice", sellPrice);
            cmd.Parameters.AddWithValue("@discountPercentage", discountPercentage);
            cmd.Parameters.AddWithValue("@discountTotal", discountTotal);
            cmd.Parameters.AddWithValue("@tax", tax);
            cmd.Parameters.AddWithValue("@total", total);
            cmd.Parameters.AddWithValue("@note", note ?? (object)DBNull.Value);

            return cmd.ExecuteScalar() != null;
        }
    }
}

public bool UpdatePendingTransactionQuantity(int terminalId, int itemId, decimal newQuantity)
{
    using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
    {
        vCon.Open();
        string query = "UPDATE pending_transactions SET quantity = @newQuantity, updated_at = CURRENT_TIMESTAMP WHERE terminal_id = @terminalId AND item_id = @itemId";

        using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
        {
            cmd.Parameters.AddWithValue("@newQuantity", newQuantity);
            cmd.Parameters.AddWithValue("@terminalId", terminalId);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}

public bool UpdatePendingTransactionDiscount(int terminalId, int itemId, decimal discountPercentage, decimal discountTotal)
{
    using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
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



        
        public bool UpdatePendingTransactionStock(int terminalId, int itemId, decimal newQuantity,decimal newTotal)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string query = "UPDATE pending_transactions SET quantity = @newQuantity,total = @newTotal, updated_at = CURRENT_TIMESTAMP WHERE terminal_id = @terminalId AND item_id = @itemId";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@newQuantity", newQuantity);
                    cmd.Parameters.AddWithValue("@newTotal", newTotal);
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeletePendingTransaction(int terminalId, int itemId)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string query = "DELETE FROM pending_transactions WHERE terminal_id = @terminalId AND item_id = @itemId";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdatePendingTransactionNote(int terminalId, int itemId, string note)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string query = "UPDATE pending_transactions SET note = @note, updated_at = CURRENT_TIMESTAMP WHERE terminal_id = @terminalId AND item_id = @itemId";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@note", note ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdatePendingTransactionDiscount(int terminalId, int itemId, decimal discountPercentage)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string query = "UPDATE pending_transactions SET discount_percentage = @discountPercentage, updated_at = CURRENT_TIMESTAMP WHERE terminal_id = @terminalId AND item_id = @itemId";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@discountPercentage", discountPercentage);
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        //////////////////////////////////////////////////////////////////








    }



    }


