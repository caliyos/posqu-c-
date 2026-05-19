<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS stock_avg_history (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            warehouse_id INT NOT NULL REFERENCES warehouses(id) ON DELETE CASCADE,
            qty_before NUMERIC(18,4) NOT NULL DEFAULT 0,
            qty_in NUMERIC(18,4) NOT NULL DEFAULT 0,
            qty_after NUMERIC(18,4) NOT NULL DEFAULT 0,
            avg_before NUMERIC(15,2) NOT NULL DEFAULT 0,
            unit_cost_in NUMERIC(15,2) NOT NULL DEFAULT 0,
            avg_after NUMERIC(15,2) NOT NULL DEFAULT 0,
            ref_type VARCHAR(30) NULL,
            ref_id BIGINT NULL,
            note TEXT NULL,
            user_id INT NULL,
            login_id INT NULL,
            created_at TIMESTAMP NOT NULL DEFAULT now()
        );

        CREATE INDEX IF NOT EXISTS idx_stock_avg_history_item_wh_created
            ON stock_avg_history(item_id, warehouse_id, created_at DESC, id DESC);

        CREATE INDEX IF NOT EXISTS idx_stock_avg_history_ref
            ON stock_avg_history(ref_type, ref_id);
    ");
};
