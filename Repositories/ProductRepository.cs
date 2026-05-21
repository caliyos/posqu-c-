
using DocumentFormat.OpenXml.Office.Word;
using Npgsql;
using POS_qu.Core.Interfaces;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace POS_qu.Repositories
{
    public class ProductRepository 
    {
        public DataTable GetAllItems()
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                string sql = @"
            SELECT 
                items.id,
                items.name,
                items.barcode,
                items.buy_price,
                items.sell_price,
CAST(COALESCE((SELECT SUM(s.qty)
               FROM stocks s
               WHERE s.item_id = items.id), 0) AS NUMERIC(18,4)) AS stock,

CAST(COALESCE((SELECT SUM(s.min_qty)
               FROM stocks s
               WHERE s.item_id = items.id), 0) AS NUMERIC(18,4)) AS min_qty,

CAST(COALESCE((SELECT SUM(s.reserved_qty)
               FROM stocks s
               WHERE s.item_id = items.id), 0) AS NUMERIC(18,4)) AS reserved_qty,

(
    SELECT COALESCE(MAX(s.hpp_avg), 0)
    FROM stocks s
    WHERE s.item_id = items.id
) AS hpp_avg,
              

                items.valuation_method,

                items.unit AS unit_id,
                units.name AS unit_name,

CAST(COALESCE(uvbase.minqty, 0) AS NUMERIC(18,4)) AS min_stock,

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
            LEFT JOIN LATERAL (
                SELECT iv.minqty
                FROM unit_variants iv
                WHERE iv.item_id = items.id
                  AND iv.is_active = TRUE
                  AND iv.is_base_unit = TRUE
                LIMIT 1
            ) uvbase ON TRUE
            LEFT JOIN units       ON items.unit = units.id
            LEFT JOIN categories  ON items.category_id = categories.id
            LEFT JOIN suppliers   ON items.supplier_id = suppliers.id
            WHERE items.deleted_at IS NULL
            ORDER BY items.id ASC
        ";
                using (var cmd = new NpgsqlCommand(sql, con))
                using (var da = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public Item GetItemById(int id)
        {
            Item item = null;
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                string sql = @"
                    SELECT items.*, 
                           COALESCE((SELECT SUM(qty) FROM stocks WHERE item_id = items.id), 0) AS stock_qty,
                           COALESCE((SELECT SUM(min_qty) FROM stocks WHERE item_id = items.id), 0) AS min_qty,
                           CASE
                               WHEN (SELECT COUNT(*) FROM stocks WHERE item_id = items.id) = 1
                                   THEN COALESCE((SELECT warehouse_id FROM stocks WHERE item_id = items.id LIMIT 1), 0)
                               ELSE 0
                           END AS warehouse_id,
                           COALESCE((SELECT SUM(reserved_qty) FROM stocks WHERE item_id = items.id), 0) AS reserved_qty
                    FROM items 
                    WHERE id = @id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = new Item
                            {
                                id = Convert.ToInt32(reader["id"]),
                                barcode = reader["barcode"]?.ToString(),
                                name = reader["name"].ToString(),
                                category_id = reader["category_id"] != DBNull.Value ? Convert.ToInt32(reader["category_id"]) : 0,
                                unitid = reader["unit"] != DBNull.Value ? Convert.ToInt32(reader["unit"]) : 0,
                                supplier_id = reader["supplier_id"] != DBNull.Value ? Convert.ToInt32(reader["supplier_id"]) : 0,
                                brand_id = reader["brand_id"] != DBNull.Value ? Convert.ToInt32(reader["brand_id"]) : null,
                                rack_id = reader["rack_id"] != DBNull.Value ? Convert.ToInt32(reader["rack_id"]) : null,
                                initial_warehouse_id = reader["warehouse_id"] != DBNull.Value ? Convert.ToInt32(reader["warehouse_id"]) : null,
                                buy_price = reader["buy_price"] != DBNull.Value ? Convert.ToDecimal(reader["buy_price"]) : 0,
                                sell_price = reader["sell_price"] != DBNull.Value ? Convert.ToDecimal(reader["sell_price"]) : 0,
                                stock = Convert.ToInt32(reader["stock_qty"]),
                                min_qty = Convert.ToInt32(reader["min_qty"]),
                                reserved_stock = Convert.ToInt32(reader["reserved_qty"]),
                                is_inventory_p = reader["is_inventory_p"] != DBNull.Value && Convert.ToBoolean(reader["is_inventory_p"]),
                                IsPurchasable = reader["is_purchasable"] != DBNull.Value && Convert.ToBoolean(reader["is_purchasable"]),
                                IsSellable = reader["is_sellable"] != DBNull.Value && Convert.ToBoolean(reader["is_sellable"]),
                                RequireNotePayment = reader["is_note_payment"] != DBNull.Value && Convert.ToBoolean(reader["is_note_payment"]),
                                is_changeprice_p = reader["is_changeprice_p"] != DBNull.Value && Convert.ToBoolean(reader["is_changeprice_p"]),
                                HasMaterials = reader["is_have_bahan"] != DBNull.Value && Convert.ToBoolean(reader["is_have_bahan"]),
                                IsPackage = reader["is_box"] != DBNull.Value && Convert.ToBoolean(reader["is_box"]),
                                IsProduced = reader["is_produksi"] != DBNull.Value && Convert.ToBoolean(reader["is_produksi"]),
                                product_type_code = HasField(reader, "product_type_code") ? (reader["product_type_code"]?.ToString() ?? "") : "",
                                discount_formula = reader["discount_formula"]?.ToString() ?? "",
                                ExpiredAt = reader["expired_at"] != DBNull.Value ? Convert.ToDateTime(reader["expired_at"]) : null,
                                note = reader["note"]?.ToString(),
                                valuation_method = reader["valuation_method"]?.ToString() ?? "AVG",
                                
                            };
                        }
                    }
                }

                if (item != null)
                {
                    item.MaterialsList = GetItemMaterials(item.id, con);
                }
            }
            return item;
        }

        public int InsertItem(Item item)
        {
            int newItemId = 0;
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string sql = @"INSERT INTO items (
                                            barcode, name, category_id, unit, supplier_id, brand_id, rack_id,
                                            buy_price, sell_price, valuation_method, is_active, note,
                                            is_inventory_p, is_purchasable, is_sellable, is_note_payment, is_changeprice_p,
                                            is_have_bahan, is_box, is_produksi, discount_formula, expired_at
                                       ) VALUES (
                                            @barcode, @name, @category_id, @unit_id, @supplier_id, @brand_id, @rack_id,
                                            @buy_price, @sell_price, @valuation_method, @is_active, @note,
                                            @is_inventory_p, @is_purchasable, @is_sellable, @is_note_payment, @is_changeprice_p,
                                            @is_have_bahan, @is_box, @is_produksi, @discount_formula, @expired_at
                                       ) RETURNING id";
                        
                        using (var cmd = new NpgsqlCommand(sql, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@barcode", (object)item.barcode ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@name", item.name);
                            cmd.Parameters.AddWithValue("@category_id", item.category_id > 0 ? (object)item.category_id : DBNull.Value);
                            cmd.Parameters.AddWithValue("@unit_id", item.unitid > 0 ? (object)item.unitid : DBNull.Value);
                            cmd.Parameters.AddWithValue("@supplier_id", item.supplier_id > 0 ? (object)item.supplier_id : DBNull.Value);
                            cmd.Parameters.AddWithValue("@brand_id", item.brand_id.HasValue && item.brand_id > 0 ? (object)item.brand_id.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@rack_id", item.rack_id.HasValue && item.rack_id > 0 ? (object)item.rack_id.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@buy_price", item.buy_price);
                            cmd.Parameters.AddWithValue("@sell_price", item.sell_price);
                            cmd.Parameters.AddWithValue("@valuation_method", item.valuation_method ?? "AVG");
                            cmd.Parameters.AddWithValue("@is_active", item.IsSellable);
                            cmd.Parameters.AddWithValue("@note", (object)item.note ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@is_inventory_p", item.is_inventory_p);
                            cmd.Parameters.AddWithValue("@is_purchasable", item.IsPurchasable);
                            cmd.Parameters.AddWithValue("@is_sellable", item.IsSellable);
                            cmd.Parameters.AddWithValue("@is_note_payment", item.RequireNotePayment);
                            cmd.Parameters.AddWithValue("@is_changeprice_p", item.is_changeprice_p);
                            bool hasBahan = item.HasMaterials || (item.MaterialsList != null && item.MaterialsList.Count > 0);
                            cmd.Parameters.AddWithValue("@is_have_bahan", hasBahan);
                            cmd.Parameters.AddWithValue("@is_box", item.IsPackage);
                            cmd.Parameters.AddWithValue("@is_produksi", item.IsProduced);
                            cmd.Parameters.AddWithValue("@discount_formula", (object)(item.discount_formula ?? "") ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@expired_at", item.ExpiredAt.HasValue ? (object)item.ExpiredAt.Value.Date : DBNull.Value);

                            newItemId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        if (HasColumn(con, tran, "items", "product_type_code"))
                        {
                            using var cmdPt = new NpgsqlCommand("UPDATE items SET product_type_code = @pt WHERE id = @id", con, tran);
                            cmdPt.Parameters.AddWithValue("@pt", string.IsNullOrWhiteSpace(item.product_type_code) ? "stockable" : item.product_type_code);
                            cmdPt.Parameters.AddWithValue("@id", newItemId);
                            cmdPt.ExecuteNonQuery();
                        }

                        // Insert initial stock to default warehouse (or specified warehouse)
                        if (item.stock > 0)
                        {
                            int warehouseId = item.initial_warehouse_id ?? 1;

                            bool hasStockHppAvg = HasColumn(con, tran, "stocks", "hpp_avg");
                            string sSql = hasStockHppAvg
                                ? "INSERT INTO stocks (item_id, warehouse_id, qty, min_qty, hpp_avg) VALUES (@item_id, @w_id, @qty, @min_qty, @hpp_avg)"
                                : "INSERT INTO stocks (item_id, warehouse_id, qty, min_qty) VALUES (@item_id, @w_id, @qty, @min_qty)";
                            using (var sCmd = new NpgsqlCommand(sSql, con, tran))
                            {
                                sCmd.Parameters.AddWithValue("@item_id", newItemId);
                                sCmd.Parameters.AddWithValue("@w_id", warehouseId);
                                sCmd.Parameters.AddWithValue("@qty", item.stock);
                                sCmd.Parameters.AddWithValue("@min_qty", item.min_qty);
                                if (hasStockHppAvg)
                                    sCmd.Parameters.AddWithValue("@hpp_avg", item.buy_price);
                                sCmd.ExecuteNonQuery();
                            }

                            string slSql = "INSERT INTO stock_layers (item_id, warehouse_id, qty_initial, qty_remaining, buy_price, expired_at) VALUES (@item_id, @w_id, @qty, @qty, @buy_price, @exp)";
                            using (var slCmd = new NpgsqlCommand(slSql, con, tran))
                            {
                                slCmd.Parameters.AddWithValue("@item_id", newItemId);
                                slCmd.Parameters.AddWithValue("@w_id", warehouseId);
                                slCmd.Parameters.AddWithValue("@qty", item.stock);
                                slCmd.Parameters.AddWithValue("@buy_price", item.buy_price);
                                slCmd.Parameters.AddWithValue("@exp", item.ExpiredAt.HasValue ? (object)item.ExpiredAt.Value.Date : DBNull.Value);
                                slCmd.ExecuteNonQuery();
                            }
                        }

                        // Insert Unit Variants
                        if (item.UnitVariants != null)
                        {
                            foreach (var variant in item.UnitVariants)
                            {
                                string vSql = @"INSERT INTO unit_variants (
                                                    item_id,
                                                    unit_id,
                                                    conversion,
                                                    sell_price,
                                                    profit,
                                                    minqty,
                                                    is_base_unit,
                                                    barcode_suffix
                                                )
                                                VALUES (
                                                    @item_id,
                                                    @unit_id,
                                                    @conversion,
                                                    @sell_price,
                                                    @profit,
                                                    @minqty,
                                                    @is_base_unit,
                                                    @barcode_suffix
                                                )";
                                using (var vCmd = new NpgsqlCommand(vSql, con, tran))
                                {
                                    vCmd.Parameters.AddWithValue("@item_id", newItemId);
                                    vCmd.Parameters.AddWithValue("@unit_id", variant.UnitId);
                                    vCmd.Parameters.AddWithValue("@conversion", variant.Conversion);
                                    vCmd.Parameters.AddWithValue("@sell_price", variant.SellPrice);
                                    vCmd.Parameters.AddWithValue("@profit", variant.Profit);
                                    vCmd.Parameters.AddWithValue("@minqty", variant.MinQty > 0 ? (object)variant.MinQty : DBNull.Value);
                                    vCmd.Parameters.AddWithValue("@is_base_unit", variant.IsBaseUnit);
                                    vCmd.Parameters.AddWithValue("@barcode_suffix", (object)variant.BarcodeSuffix ?? DBNull.Value);
                                    vCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // Insert Item Prices (Tier Pricing)
                        if (item.Prices != null)
                        {
                            foreach (var price in item.Prices)
                            {
                                string pSql = @"INSERT INTO item_prices (item_id, unit_id, price_level_id, min_qty, max_qty, price, is_active) 
                                                VALUES (@item_id, @unit_id, @price_level_id, @min_qty, @max_qty, @price, true)";
                                using (var pCmd = new NpgsqlCommand(pSql, con, tran))
                                {
                                    pCmd.Parameters.AddWithValue("@item_id", newItemId);
                                    pCmd.Parameters.AddWithValue("@unit_id", price.UnitId > 0 ? price.UnitId : (item.unitid > 0 ? item.unitid : 1));
                                    pCmd.Parameters.AddWithValue("@price_level_id", price.PriceLevelId > 0 ? price.PriceLevelId : 1);
                                    pCmd.Parameters.AddWithValue("@min_qty", price.MinQty);
                                    pCmd.Parameters.AddWithValue("@max_qty", price.MaxQty > 0 ? (object)price.MaxQty : DBNull.Value);
                                    pCmd.Parameters.AddWithValue("@price", price.Price);
                                    pCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        SaveItemMaterials(newItemId, item.MaterialsList ?? new List<ItemMaterial>(), con, tran);

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            return newItemId;
        }

        public void UpdateItem(Item item)
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string sql = @"UPDATE items SET 
                                       barcode = @barcode, name = @name, category_id = @category_id, 
                                       unit = @unit_id, supplier_id = @supplier_id, brand_id = @brand_id, rack_id = @rack_id,
                                       buy_price = @buy_price, sell_price = @sell_price, valuation_method = @valuation_method,
                                       is_active = @is_active, note = @note,
                                       is_inventory_p = @is_inventory_p, is_purchasable = @is_purchasable, is_sellable = @is_sellable,
                                       is_note_payment = @is_note_payment, is_changeprice_p = @is_changeprice_p,
                                       is_have_bahan = @is_have_bahan, is_box = @is_box, is_produksi = @is_produksi,
                                       discount_formula = @discount_formula, expired_at = @expired_at,
                                       updated_at = NOW()
                                       WHERE id = @id";
                        
                        using (var cmd = new NpgsqlCommand(sql, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@id", item.id);
                            cmd.Parameters.AddWithValue("@barcode", (object)item.barcode ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@name", item.name);
                            cmd.Parameters.AddWithValue("@category_id", item.category_id > 0 ? (object)item.category_id : DBNull.Value);
                            cmd.Parameters.AddWithValue("@unit_id", item.unitid > 0 ? (object)item.unitid : DBNull.Value);
                            cmd.Parameters.AddWithValue("@supplier_id", item.supplier_id > 0 ? (object)item.supplier_id : DBNull.Value);
                            cmd.Parameters.AddWithValue("@brand_id", item.brand_id.HasValue && item.brand_id > 0 ? (object)item.brand_id.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@rack_id", item.rack_id.HasValue && item.rack_id > 0 ? (object)item.rack_id.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@buy_price", item.buy_price);
                            cmd.Parameters.AddWithValue("@sell_price", item.sell_price);
                            cmd.Parameters.AddWithValue("@valuation_method", item.valuation_method ?? "AVG");
                            cmd.Parameters.AddWithValue("@is_active", item.IsSellable);
                            cmd.Parameters.AddWithValue("@note", (object)item.note ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@is_inventory_p", item.is_inventory_p);
                            cmd.Parameters.AddWithValue("@is_purchasable", item.IsPurchasable);
                            cmd.Parameters.AddWithValue("@is_sellable", item.IsSellable);
                            cmd.Parameters.AddWithValue("@is_note_payment", item.RequireNotePayment);
                            cmd.Parameters.AddWithValue("@is_changeprice_p", item.is_changeprice_p);
                            bool hasBahan = item.HasMaterials || (item.MaterialsList != null && item.MaterialsList.Count > 0);
                            cmd.Parameters.AddWithValue("@is_have_bahan", hasBahan);
                            cmd.Parameters.AddWithValue("@is_box", item.IsPackage);
                            cmd.Parameters.AddWithValue("@is_produksi", item.IsProduced);
                            cmd.Parameters.AddWithValue("@discount_formula", (object)(item.discount_formula ?? "") ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@expired_at", item.ExpiredAt.HasValue ? (object)item.ExpiredAt.Value.Date : DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }

                        if (HasColumn(con, tran, "items", "product_type_code"))
                        {
                            using var cmdPt = new NpgsqlCommand("UPDATE items SET product_type_code = @pt WHERE id = @id", con, tran);
                            cmdPt.Parameters.AddWithValue("@pt", string.IsNullOrWhiteSpace(item.product_type_code) ? "stockable" : item.product_type_code);
                            cmdPt.Parameters.AddWithValue("@id", item.id);
                            cmdPt.ExecuteNonQuery();
                        }

                        int warehouseId = item.initial_warehouse_id ?? 1;

                        string stockSql = @"UPDATE stocks 
                    SET 
                        min_qty = @min_qty
                    WHERE item_id = @item_id 
                    AND warehouse_id = @w_id";

                        using (var sCmd = new NpgsqlCommand(stockSql, con, tran))
                        {
                            sCmd.Parameters.AddWithValue("@item_id", item.id);
                            sCmd.Parameters.AddWithValue("@w_id", warehouseId);
                            sCmd.Parameters.AddWithValue("@min_qty", item.min_qty);

                            sCmd.ExecuteNonQuery();
                        }

                        // Re-insert Unit Variants
                        using (var delVCmd = new NpgsqlCommand("DELETE FROM unit_variants WHERE item_id = @id", con, tran))
                        {
                            delVCmd.Parameters.AddWithValue("@id", item.id);
                            delVCmd.ExecuteNonQuery();
                        }

                        if (item.UnitVariants != null)
                        {
                            foreach (var variant in item.UnitVariants)
                            {
                                string vSql = @"INSERT INTO unit_variants (
                                                    item_id,
                                                    unit_id,
                                                    conversion,
                                                    sell_price,
                                                    profit,
                                                    minqty,
                                                    is_base_unit,
                                                    barcode_suffix
                                                )
                                                VALUES (
                                                    @item_id,
                                                    @unit_id,
                                                    @conversion,
                                                    @sell_price,
                                                    @profit,
                                                    @minqty,
                                                    @is_base_unit,
                                                    @barcode_suffix
                                                )";
                                using (var vCmd = new NpgsqlCommand(vSql, con, tran))
                                {
                                    vCmd.Parameters.AddWithValue("@item_id", item.id);
                                    vCmd.Parameters.AddWithValue("@unit_id", variant.UnitId);
                                    vCmd.Parameters.AddWithValue("@conversion", variant.Conversion);
                                    vCmd.Parameters.AddWithValue("@sell_price", variant.SellPrice);
                                    vCmd.Parameters.AddWithValue("@profit", variant.Profit);
                                    vCmd.Parameters.AddWithValue("@minqty", variant.MinQty > 0 ? (object)variant.MinQty : DBNull.Value);
                                    vCmd.Parameters.AddWithValue("@is_base_unit", variant.IsBaseUnit);
                                    vCmd.Parameters.AddWithValue("@barcode_suffix", (object)variant.BarcodeSuffix ?? DBNull.Value);
                                    vCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // Re-insert Item Prices (Tier Pricing)
                        using (var delPCmd = new NpgsqlCommand("DELETE FROM item_prices WHERE item_id = @id", con, tran))
                        {
                            delPCmd.Parameters.AddWithValue("@id", item.id);
                            delPCmd.ExecuteNonQuery();
                        }

                        if (item.Prices != null)
                        {
                            foreach (var price in item.Prices)
                            {
                                string pSql = @"INSERT INTO item_prices (item_id, unit_id, price_level_id, min_qty, max_qty, price, is_active) 
                                                VALUES (@item_id, @unit_id, @price_level_id, @min_qty, @max_qty, @price, true)";
                                using (var pCmd = new NpgsqlCommand(pSql, con, tran))
                                {
                                    pCmd.Parameters.AddWithValue("@item_id", item.id);
                                    pCmd.Parameters.AddWithValue("@unit_id", price.UnitId > 0 ? price.UnitId : (item.unitid > 0 ? item.unitid : 1));
                                    pCmd.Parameters.AddWithValue("@price_level_id", price.PriceLevelId > 0 ? price.PriceLevelId : 1);
                                    pCmd.Parameters.AddWithValue("@min_qty", price.MinQty);
                                    pCmd.Parameters.AddWithValue("@max_qty", price.MaxQty > 0 ? (object)price.MaxQty : DBNull.Value);
                                    pCmd.Parameters.AddWithValue("@price", price.Price);
                                    pCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        SaveItemMaterials(item.id, item.MaterialsList ?? new List<ItemMaterial>(), con, tran);

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        private List<ItemMaterial> GetItemMaterials(int parentItemId, NpgsqlConnection con)
        {
            var list = new List<ItemMaterial>();
            string sql = @"
                SELECT im.id, im.parent_item_id, im.component_item_id, im.qty, im.unit_id, im.unit_cost,
                       i.name AS component_name, u.name AS unit_name
                FROM item_materials im
                JOIN items i ON i.id = im.component_item_id
                LEFT JOIN units u ON u.id = im.unit_id
                WHERE im.parent_item_id = @id
                ORDER BY im.id ASC";
            using (var cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", parentItemId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ItemMaterial
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            ParentItemId = Convert.ToInt32(reader["parent_item_id"]),
                            ComponentItemId = Convert.ToInt32(reader["component_item_id"]),
                            ComponentName = reader["component_name"]?.ToString() ?? "",
                            Qty = reader["qty"] != DBNull.Value ? Convert.ToDecimal(reader["qty"]) : 0m,
                            UnitId = reader["unit_id"] != DBNull.Value ? Convert.ToInt32(reader["unit_id"]) : 0,
                            UnitName = reader["unit_name"]?.ToString() ?? "",
                            UnitCost = reader["unit_cost"] != DBNull.Value ? Convert.ToDecimal(reader["unit_cost"]) : 0m
                        });
                    }
                }
            }
            return list;
        }

        private void SaveItemMaterials(int parentItemId, List<ItemMaterial> materials, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using (var delCmd = new NpgsqlCommand("DELETE FROM item_materials WHERE parent_item_id = @id", con, tran))
            {
                delCmd.Parameters.AddWithValue("@id", parentItemId);
                delCmd.ExecuteNonQuery();
            }

            if (materials == null || materials.Count == 0) return;

            foreach (var m in materials)
            {
                string sql = @"INSERT INTO item_materials (parent_item_id, component_item_id, qty, unit_id, unit_cost)
                               VALUES (@parent_item_id, @component_item_id, @qty, @unit_id, @unit_cost)";
                using (var cmd = new NpgsqlCommand(sql, con, tran))
                {
                    cmd.Parameters.AddWithValue("@parent_item_id", parentItemId);
                    cmd.Parameters.AddWithValue("@component_item_id", m.ComponentItemId);
                    cmd.Parameters.AddWithValue("@qty", m.Qty);
                    cmd.Parameters.AddWithValue("@unit_id", m.UnitId > 0 ? m.UnitId : 1);
                    cmd.Parameters.AddWithValue("@unit_cost", m.UnitCost);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool DeleteItem(int id, out bool archived)
        {
            archived = true;

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            using (var existsCmd = new NpgsqlCommand("SELECT deleted_at FROM items WHERE id = @id", con))
            {
                existsCmd.Parameters.AddWithValue("@id", id);
                using var reader = existsCmd.ExecuteReader();
                if (!reader.Read())
                    return false;

                if (!reader.IsDBNull(0))
                    return true;
            }

            using (var cmd = new NpgsqlCommand("UPDATE items SET deleted_at = NOW() WHERE id = @id AND deleted_at IS NULL", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public DataTable GetCategories() => GetLookupTable("SELECT id, name AS display FROM categories ORDER BY name ASC");
        public DataTable GetUnits()
          => GetLookupTable("SELECT id, name AS display, ord FROM units");
        public DataTable GetSuppliers() => GetLookupTable("SELECT id, name AS display FROM suppliers ORDER BY name ASC");
        public DataTable GetPriceLevels() => GetLookupTable("SELECT id, name FROM price_levels ORDER BY id ASC");
        public DataTable GetBrands() => GetLookupTable("SELECT id, name AS display FROM brands ORDER BY name ASC");
        public DataTable GetRacks() => GetLookupTable("SELECT id, name AS display FROM racks ORDER BY name ASC");

        public List<ItemPrice> GetItemPrices(int itemId)
        {
            var list = new List<ItemPrice>();
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                string sql = @"
                    SELECT ip.id, ip.item_id, ip.unit_id, ip.price_level_id, ip.min_qty, ip.max_qty, ip.price, 
                           u.name as unit_name, pl.name as price_level_name 
                    FROM item_prices ip
                    LEFT JOIN units u ON ip.unit_id = u.id
                    LEFT JOIN price_levels pl ON ip.price_level_id = pl.id
                    WHERE ip.item_id = @id AND ip.is_active = true";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", itemId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ItemPrice
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                ItemId = Convert.ToInt32(reader["item_id"]),
                                UnitId = Convert.ToInt32(reader["unit_id"]),
                                PriceLevelId = Convert.ToInt32(reader["price_level_id"]),
                                MinQty = Convert.ToInt32(reader["min_qty"]),
                                MaxQty = reader["max_qty"] != DBNull.Value ? Convert.ToInt32(reader["max_qty"]) : 0,
                                Price = Convert.ToDecimal(reader["price"]),
                                UnitName = reader["unit_name"]?.ToString(),
                                PriceLevelName = reader["price_level_name"]?.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<UnitVariant> GetItemUnitVariants(int itemId)
        {
            var variants = new List<UnitVariant>();
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                string sql = @"
                    SELECT iv.id, iv.unit_id, iv.conversion, iv.sell_price,iv.profit, u.name AS unit_name,
                           iv.minqty, iv.is_base_unit, iv.barcode_suffix
                    FROM unit_variants iv
                    LEFT JOIN units u ON iv.unit_id = u.id
                    WHERE iv.item_id = @id
                    AND iv.is_active = TRUE
                    ORDER BY iv.is_base_unit DESC, u.name";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", itemId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            variants.Add(new UnitVariant
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                UnitId = Convert.ToInt32(reader["unit_id"]),
                                Conversion = Convert.ToInt32(reader["conversion"]),
                                SellPrice = Convert.ToDecimal(reader["sell_price"]),
                                UnitName = reader["unit_name"]?.ToString() ?? "",
                                MinQty = reader["minqty"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["minqty"]),
                                Profit = reader["profit"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["profit"]),
                                IsBaseUnit = reader["is_base_unit"] != DBNull.Value && Convert.ToBoolean(reader["is_base_unit"]),
                                BarcodeSuffix = reader["barcode_suffix"] == DBNull.Value ? null : reader["barcode_suffix"]?.ToString()
                            });
                        }
                    }
                }
            }
            return variants;
        }

        private static bool HasColumn(NpgsqlConnection con, NpgsqlTransaction tran, string tableName, string columnName)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.columns
WHERE table_schema='public'
  AND table_name = @t
  AND column_name = @c
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@t", tableName);
            cmd.Parameters.AddWithValue("@c", columnName);
            var obj = cmd.ExecuteScalar();
            return obj != null && obj != DBNull.Value;
        }

        private static bool HasField(IDataRecord record, string fieldName)
        {
            for (int i = 0; i < record.FieldCount; i++)
            {
                if (string.Equals(record.GetName(i), fieldName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private DataTable GetLookupTable(string query)
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                using (var cmd = new NpgsqlCommand(query, con))
                using (var da = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
