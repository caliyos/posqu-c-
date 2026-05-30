<?php
return function($db) {
    $db->exec("
        DROP VIEW IF EXISTS v_inventory_value_v2;

        CREATE VIEW v_inventory_value_v2 AS

        WITH fifo_summary AS (
            SELECT
                sl.item_id,
                sl.warehouse_id,
                SUM(sl.qty_remaining) AS stock_qty_layers,
                SUM(sl.qty_remaining * sl.buy_price) AS fifo_stock_value
            FROM stock_layers sl
            WHERE sl.qty_remaining > 0
            GROUP BY
                sl.item_id,
                sl.warehouse_id
        )

        SELECT
            i.id AS item_id,
            i.sell_price,
            s.warehouse_id,

            COALESCE(s.qty, 0) AS stock_qty_stocks,
            COALESCE(fs.stock_qty_layers, 0) AS stock_qty_layers,

            CASE
                WHEN COALESCE(s.qty, 0) = COALESCE(fs.stock_qty_layers, 0)
                THEN 'SAME'
                ELSE 'DIFF'
            END AS stock_integrity_status,

            CASE
                WHEN UPPER(COALESCE(i.valuation_method, 'AVG')) = 'AVG'
                    THEN COALESCE(s.qty, 0)

                WHEN UPPER(COALESCE(i.valuation_method, 'AVG')) = 'FIFO'
                    THEN COALESCE(fs.stock_qty_layers, 0)

                ELSE 0
            END AS stock_qty,

            CASE
                WHEN UPPER(COALESCE(i.valuation_method, 'AVG')) = 'AVG'
                    THEN COALESCE(s.hpp_avg, 0)

                WHEN UPPER(COALESCE(i.valuation_method, 'AVG')) = 'FIFO'
                    THEN (
                        SELECT sl.buy_price
                        FROM stock_layers sl
                        WHERE sl.item_id = i.id
                          AND sl.warehouse_id = s.warehouse_id
                          AND sl.qty_remaining > 0
                        ORDER BY sl.created_at ASC
                        LIMIT 1
                    )

                ELSE 0
            END AS current_hpp,

            CASE
                WHEN UPPER(COALESCE(i.valuation_method, 'AVG')) = 'AVG'
                    THEN COALESCE(s.qty, 0) * COALESCE(s.hpp_avg, 0)

                WHEN UPPER(COALESCE(i.valuation_method, 'FIFO')) = 'FIFO'
                    THEN COALESCE(fs.fifo_stock_value, 0)

                ELSE 0
            END AS stock_value,

            COALESCE(s.qty, 0) * COALESCE(i.sell_price, 0) AS stock_sell_value

        FROM items i

        LEFT JOIN stocks s
            ON s.item_id = i.id

        LEFT JOIN (
            SELECT
                sl.item_id,
                sl.warehouse_id,
                SUM(sl.qty_remaining) AS stock_qty_layers,
                SUM(sl.qty_remaining * sl.buy_price) AS fifo_stock_value
            FROM stock_layers sl
            WHERE sl.qty_remaining > 0
            GROUP BY sl.item_id, sl.warehouse_id
        ) fs
            ON fs.item_id = i.id
           AND fs.warehouse_id = s.warehouse_id;
    ");
};