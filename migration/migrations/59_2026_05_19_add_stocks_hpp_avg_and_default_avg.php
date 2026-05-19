<?php
return function($db) {
    $db->exec("
        ALTER TABLE stocks
            ADD COLUMN IF NOT EXISTS hpp_avg NUMERIC(15,2) NOT NULL DEFAULT 0;

        ALTER TABLE items
            ALTER COLUMN valuation_method SET DEFAULT 'AVG';

        UPDATE items
        SET valuation_method = 'AVG'
        WHERE valuation_method IS NULL
           OR TRIM(valuation_method) = ''
           OR UPPER(TRIM(valuation_method)) = 'FIFO';

        UPDATE stocks s
        SET hpp_avg = COALESCE(
            (
                SELECT ROUND(
                    SUM((sl.qty_remaining::numeric) * sl.buy_price) / NULLIF(SUM(sl.qty_remaining::numeric), 0),
                    2
                )
                FROM stock_layers sl
                WHERE sl.item_id = s.item_id
                  AND sl.warehouse_id = s.warehouse_id
                  AND sl.qty_remaining > 0
            ),
            (
                SELECT COALESCE(i.buy_price, 0)
                FROM items i
                WHERE i.id = s.item_id
            ),
            0
        )
        WHERE COALESCE(s.hpp_avg, 0) = 0;
    ");
};
