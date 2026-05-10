<?php
return function($db) {
    $db->exec("
        ALTER TABLE stock_layers
        ADD COLUMN IF NOT EXISTS qty_initial NUMERIC(18,4) NOT NULL DEFAULT 0;

        UPDATE stock_layers sl
        SET qty_initial =
            CASE
                WHEN sl.qty_initial IS NULL OR sl.qty_initial = 0 THEN
                    sl.qty_remaining +
                    COALESCE(
                        (
                            SELECT SUM(l.qty_keluar)
                            FROM stock_log l
                            WHERE l.stock_layer_id = sl.id
                              AND COALESCE(l.is_allocation, FALSE) = TRUE
                        ),
                        0
                    )
                ELSE sl.qty_initial
            END;
    ");
};

