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
    using POS_qu.Helpers;

    namespace POS_qu.Controllers
    {
        public class ItemController
        {
            //private string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";




            ///////////////////////////////////////////////// NEW /////////////////////////////////////////////////////
            ///
            /// 


            public List<Item> GetItemsNew(string searchTerm = null)
            {
                List<Item> items = new List<Item>();

                string sql = @"
                SELECT
                    items.id,
                    items.name,
                    items.barcode,
                    items.buy_price,
                    items.sell_price,
                    items.stock,
                    items.reserved_stock,
                    units.name AS unit
                FROM items
                LEFT JOIN units ON items.unit = units.id
                WHERE items.deleted_at IS NULL";

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sql += " AND items.name ILIKE @searchTerm";
                }

                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            vCmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        }

                        using (NpgsqlDataReader dr = vCmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var item = new Item
                                {
                                    id = dr.GetInt32(dr.GetOrdinal("id")),
                                    name = dr.GetString(dr.GetOrdinal("name")),
                                    barcode = dr.GetString(dr.GetOrdinal("barcode")),
                                    buy_price = dr.GetDecimal(dr.GetOrdinal("buy_price")),
                                    sell_price = dr.GetDecimal(dr.GetOrdinal("sell_price")),
                                    stock = dr.GetInt32(dr.GetOrdinal("stock")),
                                    reserved_stock = dr.GetInt32(dr.GetOrdinal("reserved_stock")),
                                    unit = dr.IsDBNull(dr.GetOrdinal("unit")) ? null : dr.GetString(dr.GetOrdinal("unit"))
                                };

                                items.Add(item);
                            }
                        }
                    }
                }

                return items;
            }


        /// <summary>
        /// ////////////////////////////////////////////// END NEW /////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        //public static DataTable GetSuppliers()
        //{
        //    using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
        //    conn.Open();

        //    string sql = "SELECT id, name FROM suppliers ORDER BY name";
        //    using var cmd = new NpgsqlCommand(sql, conn);
        //    using var reader = cmd.ExecuteReader();

        //    DataTable dt = new DataTable();
        //    dt.Load(reader); // <- ini penting, biar hasil query masuk ke DataTable

        //    return dt;
        //}

        public List<UnitVariant> GetUnitVariants(int itemId)
        {
            List<UnitVariant> variants = new List<UnitVariant>();
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT uv.id, uv.unit_id, u.name AS unit_name, uv.conversion, uv.sell_price, uv.profit, uv.minqty
                       FROM unit_variants uv
                       JOIN units u ON uv.unit_id = u.id
                       WHERE uv.item_id = @item_id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@item_id", itemId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            variants.Add(new UnitVariant
                            {
                                UnitId = reader.GetInt32(0),
                                ItemId = reader.GetInt32(1),
                                UnitName = reader.GetString(2),
                                Conversion = reader.GetInt32(3),
                                SellPrice = reader.GetDecimal(4),
                                Profit = reader.GetDecimal(5),
                                MinQty = reader.GetDecimal(6)
                            });
                        }
                    }
                }
            }
            return variants;
        }

        public Item GetItemById(int id)
        {
            using (var conn = new Npgsql.NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM items WHERE id = @id LIMIT 1";
                using (var cmd = new Npgsql.NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Item
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                buy_price = reader.GetDecimal(reader.GetOrdinal("buy_price")),
                                sell_price = reader.GetDecimal(reader.GetOrdinal("sell_price")),
                                barcode = reader.GetString(reader.GetOrdinal("barcode")),

                                // stock dan reserved_stock numeric → convert ke int
                                stock = Convert.ToInt32(reader.GetDouble(reader.GetOrdinal("stock"))),
                                reserved_stock = Convert.ToInt32(reader.GetDouble(reader.GetOrdinal("reserved_stock"))),

                                // unit numeric di DB tapi di class string → convert
                                unit = reader.GetDecimal(reader.GetOrdinal("unit")).ToString(),
                                //unitid = reader.GetInt32(reader.GetOrdinal("unit_id")),
                                category_id = reader.IsDBNull(reader.GetOrdinal("category_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("category_id")),
                                supplier_id = reader.IsDBNull(reader.GetOrdinal("supplier_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("supplier_id")),
                                note = reader.IsDBNull(reader.GetOrdinal("note")) ? "" : reader.GetString(reader.GetOrdinal("note")),
                                picture = reader.IsDBNull(reader.GetOrdinal("picture")) ? "" : reader.GetString(reader.GetOrdinal("picture")),
                                created_at = reader.GetDateTime(reader.GetOrdinal("created_at")),
                                updated_at = reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                deleted_at = reader.IsDBNull(reader.GetOrdinal("deleted_at")) ? null : reader.GetDateTime(reader.GetOrdinal("deleted_at")),
                                flag = reader.IsDBNull(reader.GetOrdinal("flag")) ? 0 : reader.GetInt32(reader.GetOrdinal("flag")),

                                // SETTINGS
                                is_inventory_p = reader.GetBoolean(reader.GetOrdinal("is_inventory_p")),
                                IsPurchasable = reader.GetBoolean(reader.GetOrdinal("is_purchasable")),
                                IsSellable = reader.GetBoolean(reader.GetOrdinal("is_sellable")),
                                RequireNotePayment = reader.GetBoolean(reader.GetOrdinal("is_note_payment")),
                                is_changeprice_p = reader.GetBoolean(reader.GetOrdinal("is_changeprice_p")),
                                HasMaterials = reader.GetBoolean(reader.GetOrdinal("is_have_bahan")),
                                IsPackage = reader.GetBoolean(reader.GetOrdinal("is_box")),
                                IsProduced = reader.GetBoolean(reader.GetOrdinal("is_produksi")),
                                discount_formula = reader.IsDBNull(reader.GetOrdinal("discount_formula")) ? "" : reader.GetString(reader.GetOrdinal("discount_formula"))
                            };
                        }
                    }
                }
            }
            return null;
        }




        public int? InsertItem(Item item)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    using (var tran = vCon.BeginTransaction())
                    {
                        // ----------------------
                        // Insert item utama
                        // ----------------------
                        string sql = @"
                    INSERT INTO items (
                        name,
                        buy_price,
                        sell_price,
                        barcode,
                        stock,
                        unit,
                        category_id,
                        supplier_id,
                        is_inventory_p,
                        is_purchasable,
                        is_sellable,
                        discount_formula,
                        note,
                        created_at,
                        updated_at
                    )
                    VALUES (
                        @name,
                        @buy_price,
                        @sell_price,
                        @barcode,
                        @stock,
                        @unit,
                        @category_id,
                        @supplier_id,
                        @is_inventory_p,
                        @is_purchasable,
                        @is_sellable,
                        @discount_formula,
                        @note,
                        NOW(),
                        NOW()
                    )
                    RETURNING id;
                ";

                        int newItemId;
                        using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon, tran))
                        {
                            vCmd.Parameters.AddWithValue("@name", item.name);
                            vCmd.Parameters.AddWithValue("@buy_price", item.buy_price);
                            vCmd.Parameters.AddWithValue("@sell_price", item.sell_price);
                            vCmd.Parameters.AddWithValue("@barcode", item.barcode ?? "");
                            vCmd.Parameters.AddWithValue("@stock", item.stock);
                            vCmd.Parameters.AddWithValue("@unit", item.unitid);
                            vCmd.Parameters.AddWithValue("@category_id", item.category_id);
                            vCmd.Parameters.AddWithValue("@supplier_id", item.supplier_id);
                            vCmd.Parameters.AddWithValue("@is_inventory_p", item.is_inventory_p);
                            vCmd.Parameters.AddWithValue("@is_purchasable", item.IsPurchasable);
                            vCmd.Parameters.AddWithValue("@is_sellable", item.IsSellable);
                            vCmd.Parameters.AddWithValue("@discount_formula", item.discount_formula);
                            vCmd.Parameters.AddWithValue("@note", item.note ?? "");

                            object result = vCmd.ExecuteScalar();
                            if (result == null) throw new Exception("Gagal insert item");
                            newItemId = Convert.ToInt32(result);
                        }

                        // ----------------------
                        // Insert item_prices
                        // ----------------------
                        if (item.Prices != null && item.Prices.Count > 0)
                        {
                            string insertPriceSql = @"
                        INSERT INTO item_prices (item_id, min_qty, price, created_at)
                        VALUES (@item_id, @min_qty, @price, NOW());
                    ";

                            foreach (var price in item.Prices)
                            {
                                using (var cmd = new NpgsqlCommand(insertPriceSql, vCon, tran))
                                {
                                    cmd.Parameters.AddWithValue("@item_id", newItemId);
                                    cmd.Parameters.AddWithValue("@min_qty", price.MinQty);
                                    cmd.Parameters.AddWithValue("@price", price.Price);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }


                        ///////////////////////// UNIT VARIANT //////////////////
                        ///if (item.UnitVariants != null && item.UnitVariants.Count > 0)
                        {
                            // Hapus dulu variant lama untuk item ini
                            string deleteVariantSql = "DELETE FROM unit_variants WHERE item_id = @item_id;";
                            using (var deleteCmd = new NpgsqlCommand(deleteVariantSql, vCon, tran))
                            {
                                deleteCmd.Parameters.AddWithValue("@item_id", newItemId);
                                deleteCmd.ExecuteNonQuery();
                            }

                            // Insert variant baru
                            string insertVariantSql = @"
                                INSERT INTO unit_variants (
                                    item_id,
                                    unit_id,
                                    conversion,
                                    sell_price,
                                    profit,
                                    minqty,
                                    is_base_unit
                                )
                                VALUES (
                                    @item_id, @unit_id, @conversion, @sell_price, @profit, @minqty, @is_base_unit
                                );
                            ";

                            foreach (var variant in item.UnitVariants)
                            {
                                using (var cmd = new NpgsqlCommand(insertVariantSql, vCon, tran))
                                {
                                    cmd.Parameters.AddWithValue("@item_id", newItemId);
                                    cmd.Parameters.AddWithValue("@unit_id", variant.UnitId);
                                    cmd.Parameters.AddWithValue("@conversion", variant.Conversion);
                                    cmd.Parameters.AddWithValue("@sell_price", variant.SellPrice);
                                    cmd.Parameters.AddWithValue("@profit", variant.Profit);
                                    cmd.Parameters.AddWithValue("@minqty", variant.MinQty);
                                    cmd.Parameters.AddWithValue("@is_base_unit", false);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }


                        tran.Commit();
                        return newItemId;
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
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    using (var tran = vCon.BeginTransaction())
                    {
                        // ----------------------
                        // Update item utama
                        // ----------------------
                        string sql = @"
                UPDATE items SET
                    name = @name,
                    buy_price = @buy_price,
                    sell_price = @sell_price,
                    barcode = @barcode,
                    stock = @stock,
                    unit = @unit,
                    category_id = @category_id,
                    supplier_id = @supplier_id,
                    is_inventory_p = @is_inventory_p,
                    is_purchasable = @is_purchasable,
                    is_sellable = @is_sellable,
                    discount_formula = @discount_formula,
                    note = @note,
                    updated_at = NOW()
                WHERE id = @id
                ";

                        using (var vCmd = new NpgsqlCommand(sql, vCon, tran))
                        {
                            vCmd.Parameters.AddWithValue("@id", item.id);
                            vCmd.Parameters.AddWithValue("@name", item.name);
                            vCmd.Parameters.AddWithValue("@buy_price", item.buy_price);
                            vCmd.Parameters.AddWithValue("@sell_price", item.sell_price);
                            vCmd.Parameters.AddWithValue("@barcode", item.barcode ?? "");
                            vCmd.Parameters.AddWithValue("@stock", item.stock);
                            vCmd.Parameters.AddWithValue("@unit", item.unitid);
                            vCmd.Parameters.AddWithValue("@category_id", item.category_id);
                            vCmd.Parameters.AddWithValue("@supplier_id", item.supplier_id);
                            vCmd.Parameters.AddWithValue("@is_inventory_p", item.is_inventory_p);
                            vCmd.Parameters.AddWithValue("@is_purchasable", item.IsPurchasable);
                            vCmd.Parameters.AddWithValue("@is_sellable", item.IsSellable);
                            vCmd.Parameters.AddWithValue("@discount_formula", item.discount_formula);
                            vCmd.Parameters.AddWithValue("@note", item.note ?? "");

                            vCmd.ExecuteNonQuery();
                        }

                        // ----------------------
                        // Hapus item_prices lama
                        // ----------------------
                        string deletePriceSql = "DELETE FROM item_prices WHERE item_id = @item_id";
                        using (var delCmd = new NpgsqlCommand(deletePriceSql, vCon, tran))
                        {
                            delCmd.Parameters.AddWithValue("@item_id", item.id);
                            delCmd.ExecuteNonQuery();
                        }

                        // ----------------------
                        // Insert item_prices baru
                        // ----------------------
                        if (item.Prices != null && item.Prices.Count > 0)
                        {
                            string insertPriceSql = @"
                        INSERT INTO item_prices (item_id, min_qty, price, created_at)
                        VALUES (@item_id, @min_qty, @price, NOW());
                    ";

                            foreach (var price in item.Prices)
                            {
                                using (var cmd = new NpgsqlCommand(insertPriceSql, vCon, tran))
                                {
                                    cmd.Parameters.AddWithValue("@item_id", item.id);
                                    cmd.Parameters.AddWithValue("@min_qty", price.MinQty);
                                    cmd.Parameters.AddWithValue("@price", price.Price);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // ----------------------
                        // Update UnitVariants
                        // ----------------------
                        if (item.UnitVariants != null && item.UnitVariants.Count > 0)
                        {
                            // Hapus variant lama
                            string deleteVariantSql = "DELETE FROM unit_variants WHERE item_id = @item_id;";
                            using (var deleteCmd = new NpgsqlCommand(deleteVariantSql, vCon, tran))
                            {
                                deleteCmd.Parameters.AddWithValue("@item_id", item.id);
                                deleteCmd.ExecuteNonQuery();
                            }

                            // Insert variant baru
                            string insertVariantSql = @"
                        INSERT INTO unit_variants (
                            item_id,
                            unit_id,
                            conversion,
                            sell_price,
                            profit,
                            minqty,
                            is_base_unit
                        )
                        VALUES (
                            @item_id, @unit_id, @conversion, @sell_price, @profit, @minqty, @is_base_unit
                        );
                    ";

                            foreach (var variant in item.UnitVariants)
                            {
                                using (var cmd = new NpgsqlCommand(insertVariantSql, vCon, tran))
                                {
                                    cmd.Parameters.AddWithValue("@item_id", item.id);
                                    cmd.Parameters.AddWithValue("@unit_id", variant.UnitId);
                                    cmd.Parameters.AddWithValue("@conversion", variant.Conversion);
                                    cmd.Parameters.AddWithValue("@sell_price", variant.SellPrice);
                                    cmd.Parameters.AddWithValue("@profit", variant.Profit);
                                    cmd.Parameters.AddWithValue("@minqty", variant.MinQty);
                                    cmd.Parameters.AddWithValue("@is_base_unit", false);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating item: " + ex.Message);
                return false;
            }
        }



        public DataTable GetItems()
        {
            DataTable dt = new DataTable();

            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();

                string sql = @"
            SELECT 
                items.id,
                items.name,
                items.barcode,
                items.buy_price,
                items.sell_price,
                items.stock,
                items.reserved_stock,

                items.unit AS unit_id,
                units.name AS unit_name,

                items.category_id,
                categories.name AS category_name,

                items.note,
                items.picture,

                -- BOOLEAN FLAGS
                items.is_inventory_p,
                items.is_purchasable,
                items.is_sellable,
                items.is_note_payment,
                items.is_changeprice_p,
                items.is_have_bahan,
                items.is_box,
                items.is_produksi,

                items.discount_formula,

                items.supplier_id,
                suppliers.name AS supplier_name,

                items.flag,
                items.created_at,
                items.updated_at
            FROM items
            LEFT JOIN units       ON items.unit = units.id
            LEFT JOIN categories  ON items.category_id = categories.id
            LEFT JOIN suppliers   ON items.supplier_id = suppliers.id
            WHERE items.deleted_at IS NULL
            ORDER BY items.id DESC
        ";

                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    using (NpgsqlDataReader dr = vCmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }

            return dt;
        }

      

        public DataTable GetItemPrices(int itemId)
        {
            DataTable dt = new DataTable();

            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();

                string sql = @"
            SELECT 
                id,
                item_id,
                min_qty,
                price
            FROM item_prices
            WHERE item_id = @item_id
            ORDER BY min_qty ASC
        ";

                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    vCmd.Parameters.AddWithValue("@item_id", itemId);

                    using (NpgsqlDataReader dr = vCmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }

            return dt;
        }




        public DataTable GetAvailableItems()
        {
            MessageBox.Show("GetItems no param called.");
            DataTable dt = new DataTable();
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string sql = @"
            SELECT 
                items.id, 
                items.name, 
                items.barcode, 
                items.buy_price, 
                items.sell_price, 
                items.stock, 
                units.name AS unit, 
                items.unit AS unit_id, 
                items.group, 
                items.note,   
                items.picture, 
                items.reserved_stock
            FROM 
                items
            LEFT JOIN 
                units ON items.unit = units.id
            WHERE 
                items.deleted_at IS NULL
                AND items.stock > 0
        ";
                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    NpgsqlDataReader dr = vCmd.ExecuteReader();
                    dt.Load(dr);
                }
            }
            return dt;
        }

        public DataTable GetNonAvailableItems()
        {
            MessageBox.Show("GetItems no param called.");
            DataTable dt = new DataTable();
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string sql = @"
            SELECT 
                items.id, 
                items.name, 
                items.barcode, 
                items.buy_price, 
                items.sell_price, 
                items.stock, 
                units.name AS unit, 
                items.unit AS unit_id, 
                items.group, 
                items.note,   
                items.picture, 
                items.reserved_stock
            FROM 
                items
            LEFT JOIN 
                units ON items.unit = units.id
            WHERE 
                items.deleted_at IS NULL
                AND items.stock < 1
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


            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string sql = @"
            SELECT 
                id, 
                name || ' (' || abbr || ')' AS display 
            FROM units 
            ORDER BY id ASC";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, vCon))
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                }
            }
            return dt;
        }

        public DataTable GetCategories()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("display", typeof(string));

            List<Category> categories = new List<Category>();

            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string sql = "SELECT id, name, kode, parent_id FROM categories ORDER BY name";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, vCon))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = dr.GetInt32(dr.GetOrdinal("id")),
                            Name = dr.GetString(dr.GetOrdinal("name")),
                            Kode = dr.GetString(dr.GetOrdinal("kode")),
                            ParentId = dr.IsDBNull(dr.GetOrdinal("parent_id")) ? (int?)null : dr.GetInt32(dr.GetOrdinal("parent_id"))
                        });
                    }
                }
            }

            // Build hierarchical display (parent → child)
            foreach (var cat in categories)
            {
                string display = cat.Name;
                if (cat.ParentId.HasValue)
                {
                    var parent = categories.FirstOrDefault(x => x.Id == cat.ParentId.Value);
                    if (parent != null)
                    {
                        display = parent.Name + " → " + display;
                    }
                }
                display += " (" + cat.Kode + ")";
                dt.Rows.Add(cat.Id, display);
            }

            return dt;
        }

        public DataTable GetSuppliers()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(long));
            dt.Columns.Add("display", typeof(string));

            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();

                string sql = @"
            SELECT id, name, kode, phone
            FROM suppliers
            ORDER BY name
        ";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, vCon))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        long id = dr.GetInt64(dr.GetOrdinal("id"));
                        string name = dr["name"].ToString();
                        string kode = dr["kode"] == DBNull.Value ? "" : dr["kode"].ToString();
                        string phone = dr["phone"] == DBNull.Value ? "" : dr["phone"].ToString();

                        // contoh display: "Indofood (IF01) - 08123456789"
                        string display = name;

                        if (!string.IsNullOrEmpty(kode))
                            display += $" ({kode})";

                        if (!string.IsNullOrEmpty(phone))
                            display += $" - {phone}";

                        dt.Rows.Add(id, display);
                    }
                }
            }

            return dt;
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

        public decimal GetItemPrice(int id)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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

        public string GetItemUnit(int id)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    string sql = @"SELECT units.name FROM items
LEFT JOIN units ON items.unit = units.id
WHERE items.id = @id";

                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@id", id);

                        var result = vCmd.ExecuteScalar();
                        return result != null ? result.ToString() : "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving unit: {ex.Message}");
                return "";
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


        public int GetItemStock(int id)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    vCon.Open();
                    string sql = "SELECT reserved_stock FROM items WHERE id = @id";
                    using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                    {
                        vCmd.Parameters.AddWithValue("@id", id);

                        var result = vCmd.ExecuteScalar();
                        MessageBox.Show(result != null ? $"Raw result: {result.ToString()}" : "Result is NULL");
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

        public bool UpdateItemStockAndReservedStock(int id, int newStock, int rStock)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
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
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
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

            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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




        public bool DeleteItem(int id)
        {
            try
            {
                using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string sql = @"
    INSERT INTO transactions (
        ts_numbering, ts_code, ts_total, ts_payment_amount, ts_cashback, 
        ts_method, ts_status, ts_change, ts_internal_note, ts_note, 
        ts_global_discount_amount, ts_grand_total, 
        ts_customer, ts_freename, terminal_id, shift_id, user_id, 
        created_by, created_at, order_id,ts_delivery_amount,cart_session_code
    ) 
    VALUES (
        @ts_numbering, @ts_code, @ts_total, @ts_payment_amount, @ts_cashback, 
        @ts_method, @ts_status, @ts_change, @ts_internal_note, @ts_note, 
        @ts_global_discount_amount, @ts_grand_total, 
        @ts_customer, @ts_freename, @terminal_id, @shift_id, 
        @user_id, @created_by, @created_at, @order_id, @ts_delivery_amount,@cart_session_code
    ) 
    RETURNING ts_id";

                using (NpgsqlCommand vCmd = new NpgsqlCommand(sql, vCon))
                {
                    vCmd.Parameters.AddWithValue("@ts_numbering", transaction.TsNumbering);
                    vCmd.Parameters.AddWithValue("@ts_code", transaction.TsCode);
                    vCmd.Parameters.AddWithValue("@ts_total", transaction.TsTotal);
                    vCmd.Parameters.AddWithValue("@ts_payment_amount", transaction.TsPaymentAmount);
                    vCmd.Parameters.AddWithValue("@ts_cashback", transaction.TsCashback);
                    vCmd.Parameters.AddWithValue("@ts_method", transaction.TsMethod);
                    vCmd.Parameters.AddWithValue("@ts_status", transaction.TsStatus);
                    vCmd.Parameters.AddWithValue("@ts_change", transaction.TsChange);
                    vCmd.Parameters.AddWithValue("@ts_internal_note", transaction.TsInternalNote);
                    vCmd.Parameters.AddWithValue("@ts_note", transaction.TsNote);

                    // ✅ Tambahan baru
                    vCmd.Parameters.AddWithValue("@ts_global_discount_amount", transaction.TsDiscountTotal);
                    vCmd.Parameters.AddWithValue("@ts_grand_total", transaction.TsGrandTotal);

                    vCmd.Parameters.AddWithValue("@ts_customer", (object)transaction.TsCustomer ?? DBNull.Value);
                    vCmd.Parameters.AddWithValue("@ts_freename", transaction.TsFreename);
                    vCmd.Parameters.AddWithValue("@terminal_id", transaction.TerminalId);
                    vCmd.Parameters.AddWithValue("@shift_id", transaction.ShiftId);
                    vCmd.Parameters.AddWithValue("@user_id", transaction.UserId);
                    vCmd.Parameters.AddWithValue("@created_by", transaction.CreatedBy);
                    vCmd.Parameters.AddWithValue("@created_at", transaction.CreatedAt);
                    vCmd.Parameters.AddWithValue("@order_id", (object)transaction.OrderId ?? DBNull.Value);
                    vCmd.Parameters.AddWithValue("@ts_delivery_amount", transaction.TsDelivery);
                    vCmd.Parameters.AddWithValue("@cart_session_code", transaction.CartSessionCode);

                    return Convert.ToInt32(vCmd.ExecuteScalar());
                }
            }

        }



        public void InsertTransactionDetails(List<TransactionDetail> details)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string sql = @"
INSERT INTO transaction_details (
    ts_id, item_id, tsd_barcode, tsd_sell_price, tsd_quantity, tsd_unit, tsd_note, 
    tsd_discount_per_item, tsd_discount_percentage, tsd_discount_total, 
    tsd_tax, tsd_total, tsd_conversion_rate, tsd_price_per_unit, tsd_unit_variant, created_by, created_at,cart_session_code) 
VALUES (
    @ts_id, @item_id, @tsd_barcode, @tsd_sell_price, @tsd_quantity, @tsd_unit, @tsd_note, 
    @tsd_discount_per_item, @tsd_discount_percentage, @tsd_discount_total, 
    @tsd_tax, @tsd_total, @tsd_conversion_rate, @tsd_price_per_unit,@tsd_unit_variant, @created_by, @created_at, @cart_session_code)";

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
                        vCmd.Parameters.AddWithValue("@tsd_conversion_rate", detail.TsdConversionRate); // Added conversion rate
                        vCmd.Parameters.AddWithValue("@tsd_price_per_unit", detail.TsdPricePerUnit); // Added price per unit
                        vCmd.Parameters.AddWithValue("@tsd_unit_variant", detail.TsdUnitVariant);
                        vCmd.Parameters.AddWithValue("@created_by", detail.CreatedBy);
                        vCmd.Parameters.AddWithValue("@created_at", detail.CreatedAt);
                        vCmd.Parameters.AddWithValue("@cart_session_code", detail.CartSessionCode);

                        vCmd.ExecuteNonQuery();
                    }
                }
            }
        }


        /////////////////////////////////////////////////////////////////



        //update stock reserved stock 
        public void updateStockAndReservedStock (int id,decimal stock, decimal rstock)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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
        public bool AddPendingTransaction(
      int terminalId,
      int cashierId,
      int itemId,
      string barcode,
      string unit,
      decimal quantity,
      decimal sellPrice,
      decimal discountPercentage,
      decimal discountTotal,
      decimal tax,
      decimal total,
      string note,
      string cartSessionCode,
      decimal conversionRate // ✅ kolom baru
  )
        {
            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = @"
INSERT INTO pending_transactions 
    (terminal_id, cashier_id, item_id, barcode, unit, quantity, sell_price, discount_percentage, 
     discount_total, tax, total, note, cart_session_code, tsd_conversion_rate)
VALUES 
    (@terminalId, @cashierId, @itemId, @barcode, @unit, @quantity, @sellPrice, @discountPercentage,
     @discountTotal, @tax, @total, @note, @cartSessionCode, @conversionRate)
RETURNING pt_id;
";

                using (var cmd = new NpgsqlCommand(query, vCon))
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
                    cmd.Parameters.AddWithValue("@cartSessionCode", cartSessionCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@conversionRate", conversionRate); // ✅ tambahan

                    return cmd.ExecuteScalar() != null;
                }
            }
        }



        public bool UpdatePendingTransactionQuantity(int terminalId, int itemId, decimal newQuantity)
{
    using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
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




        public bool UpdatePendingTransactionStock(
          int terminalId,
          int itemId,
          decimal newQuantity,
          decimal newTotal,
          string unit,
          string cartSessionCode
      )
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = @"
UPDATE pending_transactions
SET quantity = @newQuantity,
    total = @newTotal,
    updated_at = CURRENT_TIMESTAMP
WHERE terminal_id = @terminalId
  AND item_id = @itemId
  AND unit = @unit
  AND cart_session_code = @cartSessionCode"; // 🔹 filter by cart session

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@newQuantity", newQuantity);
                    cmd.Parameters.AddWithValue("@newTotal", newTotal);
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@unit", unit);
                    cmd.Parameters.AddWithValue("@cartSessionCode", cartSessionCode);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        //public bool DeletePendingTransaction(int terminalId, int itemId, string cart_session_code)
        //{
        //    using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
        //    {
        //        vCon.Open();

        //        string query = @"
        //    DELETE FROM pending_transactions 
        //    WHERE terminal_id = @terminalId 
        //      AND item_id = @itemId 
        //      AND cart_session_code = @cart_session_code";

        //        using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
        //        {
        //            cmd.Parameters.AddWithValue("@terminalId", terminalId);
        //            cmd.Parameters.AddWithValue("@itemId", itemId);
        //            cmd.Parameters.AddWithValue("@cart_session_code", cart_session_code);

        //            return cmd.ExecuteNonQuery() > 0;
        //        }
        //    }
        //}

        public bool DeletePendingTransaction(int terminalId, int userId, int itemId, string cart_session_code)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();

                string query = @"
            DELETE FROM pending_transactions 
            WHERE terminal_id = @terminalId 
              AND item_id = @itemId
              AND cashier_id = @userId
              AND cart_session_code = @cart_session_code";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@cart_session_code", cart_session_code);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public bool UpdatePendingTransactionNote(int terminalId, int userId,int itemId, string cart_session_code, string note)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = @"
            UPDATE pending_transactions 
            SET note = @note, 
                updated_at = CURRENT_TIMESTAMP 
            WHERE terminal_id = @terminalId 
              AND item_id = @itemId
               AND  cashier_id = @userId
              AND cart_session_code = @cart_session_code";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@note", string.IsNullOrEmpty(note) ? (object)DBNull.Value : note);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@cart_session_code", cart_session_code);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public bool UpdatePendingTransactionDiscount(int terminalId, int userId, int itemId, string cart_session_code, decimal discountPercentage)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = @"
            UPDATE pending_transactions 
            SET discount_percentage = @discountPercentage,
                updated_at = CURRENT_TIMESTAMP 
            WHERE terminal_id = @terminalId 
            AND cashier_id = @userId
              AND item_id = @itemId
              AND cart_session_code = @cart_session_code";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@discountPercentage", discountPercentage);
                    cmd.Parameters.AddWithValue("@terminalId", terminalId);
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@cart_session_code", cart_session_code);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        //////////////////////////////////////////////////////////////////


        //get item by barcode
        public Item GetItemByBarcode(string barcode)
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();

                string itemSql = @"
            SELECT items.id, items.name, items.barcode, items.sell_price, items.stock,
                   units.name as unitname
            FROM items
            LEFT JOIN units ON items.unit = units.id
            WHERE items.barcode = @barcode AND items.deleted_at IS NULL
            LIMIT 1";

                using (var cmd = new NpgsqlCommand(itemSql, con))
                {
                    cmd.Parameters.AddWithValue("@barcode", barcode);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int productId = reader.GetInt32(reader.GetOrdinal("id"));
                            string name = reader.GetString(reader.GetOrdinal("name"));
                            string barcodeVal = reader.GetString(reader.GetOrdinal("barcode"));
                            double stock = reader.GetDouble(reader.GetOrdinal("stock"));
                            string unitName = reader.GetString(reader.GetOrdinal("unitname"));
                            decimal sellPrice = reader.GetDecimal(reader.GetOrdinal("sell_price"));
                            int conversion = 1;

                            int reservedStock = GetItemReservedStock(barcodeVal);
                            int quantity = 1;
                            int stockNeeded = quantity * conversion;

                            if (stockNeeded > stock)
                                throw new InvalidOperationException("Stok tidak cukup.");

                            int newReservedStock = reservedStock + stockNeeded;
                            if (newReservedStock > stock)
                                throw new InvalidOperationException("Stok sudah penuh oleh reserved stock.");

                            // Update reserved stock
                            UpdateReservedStock(barcodeVal, newReservedStock);

                            // Hitung harga asli per pcs
                            decimal realPrice = GetItemPrice(productId);

                            return new Item
                            {
                                id = productId,
                                barcode = barcodeVal,
                                name = name,
                                stock = quantity,
                                unit = unitName,
                                conversion = conversion,
                                sell_price = sellPrice,
                                price_per_pcs = Math.Round(sellPrice / conversion, 2),
                                price_per_pcs_asli = realPrice
                            };
                        }
                    }
                }
            }

            return null;
        }



        //// RANDOM BARCODE
        public Item GetRandomItemByBarcode()
        {
            List<string> barcodes = new List<string>();

            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();

                // Step 1: Get all barcodes
                string barcodeSql = "SELECT barcode FROM items WHERE deleted_at IS NULL";
                using (var cmd = new NpgsqlCommand(barcodeSql, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        barcodes.Add(reader.GetString(0));
                    }
                }

                // Step 2: Check if we got any
                if (barcodes.Count == 0)
                    return null;

                // Step 3: Pick a random barcode
                var random = new Random();
                int index = random.Next(barcodes.Count);
                string randomBarcode = barcodes[index];

                // Step 4: Get item by barcode


                string itemSql = "SELECT items.id,items.name,items.barcode, items.sell_price,items.stock, units.name as unitname FROM items LEFT JOIN units units ON items.unit = units.id WHERE barcode = @barcode";
                using (var itemCmd = new NpgsqlCommand(itemSql, con))
                {
                    itemCmd.Parameters.AddWithValue("@barcode", randomBarcode);

                    using (var itemReader = itemCmd.ExecuteReader())
                    {
                        if (itemReader.Read())
                        {
                            var sellPrice = itemReader.GetDecimal(itemReader.GetOrdinal("sell_price"));
                            int productId = itemReader.GetInt32(itemReader.GetOrdinal("id"));
                            string name = itemReader.GetString(itemReader.GetOrdinal("name"));
                            string barcode = itemReader.GetString(itemReader.GetOrdinal("barcode"));
                            double stock = itemReader.GetDouble(itemReader.GetOrdinal("stock"));
                            string unitName = itemReader.GetString(itemReader.GetOrdinal("unitname"));
                            int conversion = 1;

                            int reservedStock = GetItemReservedStock(barcode);
                            int quantity = 1;  // assume 1 by default
                            int stockNeeded = quantity * conversion;


                            if (stockNeeded > stock)
                                throw new InvalidOperationException("Insufficient stock for random item.");

                            int newReservedStock = reservedStock + stockNeeded;
                            if (newReservedStock > stock)
                                throw new InvalidOperationException("Stock already reserved for random item.");

                            // Step 5: Update reserved stock
                            UpdateReservedStock(barcode, newReservedStock);

                            // Step 6: Get real price
                            decimal realprice = GetItemPrice(productId);

                            // Step 7: Build item object
                            return new Item
                            {
                                id = productId,
                                barcode = barcode,
                                name = name,
                                stock = quantity,
                                unit = unitName,
                                conversion = conversion,
                                sell_price = sellPrice,
                                price_per_pcs = Math.Round(sellPrice / conversion, 2),
                                price_per_pcs_asli = realprice
                            };
                        }
                    }
                }
            }

            return null; // fallback if not found
        }


        public bool UpdateOrderPayment(int orderId, int newStatus, string paymentMethod)
        {
            try
            {
                using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                UPDATE orders
                SET order_status = @status,
                    payment_method = @method,
                    updated_at = NOW()
                WHERE order_id = @orderId
            ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@method", paymentMethod ?? "");
                        cmd.Parameters.AddWithValue("@orderId", orderId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update order: " + ex.Message);
                return false;
            }
        }


        public bool DeleteDraftPayment(int poId)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlDeleteOrder = @"
DELETE FROM pending_orders
WHERE po_id = @po_id";

                        using (var cmd = new NpgsqlCommand(sqlDeleteOrder, conn))
                        {
                            cmd.Parameters.AddWithValue("@po_id", poId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Failed to delete draft payment: " + ex.Message);
                        return false;
                    }
                }
            }
        }





        public bool UpdateOrderStatus(int oId, int status)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlUpdateOrder = @"
UPDATE orders
SET 
    order_status = @status,
    deleted_at = NOW(),
    updated_at = NOW()
WHERE order_id = @oId";

                        using (var cmd = new NpgsqlCommand(sqlUpdateOrder, conn))
                        {
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@oId", oId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Failed to update order status: " + ex.Message);
                        return false;
                    }
                }
            }
        }





        public int SaveDraftOrder(int terminalId, int cashierId, string customerName, string note, decimal total, decimal globalDiscount = 0, string csc = null)
        {
            using (NpgsqlConnection vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                using (var transaction = vCon.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ Insert header draft
                        string sqlOrder = @"
INSERT INTO pending_orders 
    (terminal_id, cashier_id, customer_name, note, total, global_discount, po_cart_session_code, status)
VALUES 
    (@terminal_id, @cashier_id, @customer_name, @note, @total, @global_discount, @po_cart_session_code, 'draft')
RETURNING po_id";

                        int poId;
                        using (NpgsqlCommand cmd = new NpgsqlCommand(sqlOrder, vCon))
                        {
                            cmd.Parameters.AddWithValue("@terminal_id", terminalId);
                            cmd.Parameters.AddWithValue("@cashier_id", cashierId);
                            cmd.Parameters.AddWithValue("@customer_name", (object)customerName ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@note", (object)note ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@total", total);
                            cmd.Parameters.AddWithValue("@global_discount", globalDiscount);
                            cmd.Parameters.AddWithValue("@po_cart_session_code", (object)csc ?? DBNull.Value);

                            poId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 2️⃣ Update semua pending_transactions terkait terminal + cashier + cart_session_code
                        string sqlUpdateTransactions = @"
UPDATE pending_transactions
SET po_id = @po_id
WHERE terminal_id = @terminal_id AND cashier_id = @cashier_id AND cart_session_code = @po_cart_session_code";

                        using (NpgsqlCommand cmdUpdate = new NpgsqlCommand(sqlUpdateTransactions, vCon))
                        {
                            cmdUpdate.Parameters.AddWithValue("@po_id", poId);
                            cmdUpdate.Parameters.AddWithValue("@terminal_id", terminalId);
                            cmdUpdate.Parameters.AddWithValue("@cashier_id", cashierId);
                            cmdUpdate.Parameters.AddWithValue("@po_cart_session_code", (object)csc ?? DBNull.Value);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return poId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // 🔹 Ambil semua draft dari pending_orders
        public DataTable GetDraftOrders()
        {
            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = @"
                SELECT 
                    po_id,
                    terminal_id,
                    cashier_id,
                    customer_name,
                    note,
                    total,
                    global_discount,
                    status,
                    created_at,
                    expired_at,
                    po_cart_session_code
                FROM pending_orders
                WHERE status = 'draft'
                ORDER BY created_at DESC;
            ";

                using (var da = new NpgsqlDataAdapter(query, vCon))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // 🔹 Ambil 1 draft berdasarkan po_id
        public DataRow GetDraftOrderById(int poId)
        {
            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = "SELECT * FROM pending_orders WHERE po_id = @poId";

                using (var da = new NpgsqlDataAdapter(query, vCon))
                {
                    da.SelectCommand.Parameters.AddWithValue("@poId", poId);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0];
                    return null;
                }
            }
        }

        // 🔹 Hapus draft
        public bool DeleteDraftOrder(int poId)
        {
            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                string query = "DELETE FROM pending_orders WHERE po_id = @poId";

                using (var cmd = new NpgsqlCommand(query, vCon))
                {
                    cmd.Parameters.AddWithValue("@poId", poId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public DataTable GetPendingTransactionsBySession(string sessionCode)
        {
            MessageBox.Show("GetPendingTransactionsBySession " + sessionCode);
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                string query = @"
            SELECT 
                pt.item_id,
                i.name AS item_name,
                pt.barcode,
                pt.quantity,
                pt.unit,
                pt.sell_price,
                pt.discount_total,
                pt.tax,
                pt.total,
                pt.note,
                pt.tsd_conversion_rate
            FROM pending_transactions pt
            JOIN items i ON i.id = pt.item_id
            WHERE pt.cart_session_code = @sessionCode
            ORDER BY pt.created_at;";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sessionCode", sessionCode);
                    using (var da = new NpgsqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }



        public bool CancelDraftOrder(int poId)
        {
            using (var vCon = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                vCon.Open();
                using (var tx = vCon.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ Ambil semua item di draft
                        string getItemsQuery = "SELECT item_id, quantity FROM pending_transactions WHERE po_id = @poId";
                        var items = new List<(int itemId, decimal qty)>();
                        using (var cmd = new NpgsqlCommand(getItemsQuery, vCon, tx))
                        {
                            cmd.Parameters.AddWithValue("@poId", poId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                    items.Add((reader.GetInt32(0), reader.GetDecimal(1)));
                            }
                        }

                        // 2️⃣ Update stok (kembalikan stock dan reserved_stock)
                        foreach (var (itemId, qty) in items)
                        {
                            string stockUpdate = @"
                        UPDATE items
                        SET stock = stock + @qty,
                            reserved_stock = reserved_stock - @qty,
                            updated_at = CURRENT_TIMESTAMP
                        WHERE id = @itemId;
                    ";

                            using (var cmd = new NpgsqlCommand(stockUpdate, vCon, tx))
                            {
                                cmd.Parameters.AddWithValue("@qty", qty);
                                cmd.Parameters.AddWithValue("@itemId", itemId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // 3️⃣ Hapus detail pending_transactions
                        string delPendingTrans = "DELETE FROM pending_transactions WHERE po_id = @poId";
                        using (var cmd = new NpgsqlCommand(delPendingTrans, vCon, tx))
                        {
                            cmd.Parameters.AddWithValue("@poId", poId);
                            cmd.ExecuteNonQuery();
                        }

                        // 4️⃣ Update status pending_orders jadi cancelled
                        string updateOrder = @"
                    UPDATE pending_orders
                    SET status = 'cancelled', updated_at = CURRENT_TIMESTAMP
                    WHERE po_id = @poId;
                ";
                        using (var cmd = new NpgsqlCommand(updateOrder, vCon, tx))
                        {
                            cmd.Parameters.AddWithValue("@poId", poId);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
                        return false;
                    }
                }
            }
        }




















    }



}


