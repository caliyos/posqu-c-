<?php
return function($db) {
    // Buat fungsi laporan penjualan per tanggal & produk
    // $db->exec("
    // CREATE OR REPLACE FUNCTION public.get_sales_report(start_date date, end_date date)
    // RETURNS TABLE(
    //     ProductName varchar,
    //     Date date,
    //     Sales numeric,
    //     Transaction bigint,
    //     Items numeric,
    //     Delivery bigint,
    //     Profit numeric,
    //     Discount numeric
    // )
    // AS \$BODY\$
    // BEGIN
    //     RETURN QUERY
    //     SELECT
    //         i.name AS ProductName,
    //         DATE(t.created_at) AS Date,
    //         SUM(td.tsd_total) AS Sales,
    //         COUNT(DISTINCT t.ts_id) AS Transaction,
    //         SUM(td.tsd_quantity) AS Items,
    //         SUM(CASE WHEN t.ts_method ILIKE '%Delivery%' THEN 1 ELSE 0 END) AS Delivery,
    //         SUM(td.tsd_total - td.tsd_discount_per_item) AS Profit,
    //         SUM(
    //             CASE 
    //                 WHEN td.tsd_sell_price < td.tsd_price_per_unit THEN 
    //                     (td.tsd_price_per_unit - td.tsd_sell_price) * td.tsd_quantity
    //                 ELSE 0 
    //             END
    //         ) AS Discount
    //     FROM transactions t
    //     JOIN transaction_details td ON t.ts_id = td.ts_id
    //     JOIN items i ON td.item_id = i.id
    //     WHERE DATE(t.created_at) BETWEEN start_date AND end_date
    //     GROUP BY i.name, DATE(t.created_at)
    //     ORDER BY DATE(t.created_at), i.name;
    // END;
    // \$BODY\$
    // LANGUAGE plpgsql VOLATILE
    // COST 100
    // ROWS 1000;
    // ");

    // // Buat fungsi produk terlaris (top products)
    // $db->exec("
    // CREATE OR REPLACE FUNCTION public.get_top_products(start_date date, end_date date)
    // RETURNS TABLE(
    //     ProductName varchar,
    //     Sales numeric,
    //     Transaction bigint,
    //     Items numeric,
    //     Profit numeric,
    //     Discount numeric
    // )
    // AS \$BODY\$
    // BEGIN
    //     RETURN QUERY
    //     SELECT
    //         i.name AS ProductName,
    //         SUM(td.tsd_total) AS Sales,
    //         COUNT(DISTINCT t.ts_id) AS Transaction,
    //         SUM(td.tsd_quantity) AS Items,
    //         SUM(td.tsd_total - td.tsd_discount_per_item) AS Profit,
    //         SUM(
    //             CASE 
    //                 WHEN td.tsd_sell_price < td.tsd_price_per_unit THEN 
    //                     (td.tsd_price_per_unit - td.tsd_sell_price) * td.tsd_quantity
    //                 ELSE 0 
    //             END
    //         ) AS Discount
    //     FROM transactions t
    //     JOIN transaction_details td ON t.ts_id = td.ts_id
    //     JOIN items i ON td.item_id = i.id
    //     WHERE DATE(t.created_at) BETWEEN start_date AND end_date
    //     GROUP BY i.name
    //     ORDER BY SUM(td.tsd_total) DESC, i.name;
    // END;
    // \$BODY\$
    // LANGUAGE plpgsql VOLATILE
    // COST 100
    // ROWS 1000;
    // ");
};
