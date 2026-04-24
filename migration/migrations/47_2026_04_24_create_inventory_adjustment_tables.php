<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS inventory_adjustments (
            id BIGSERIAL PRIMARY KEY,
            adjustment_no VARCHAR(30) NOT NULL UNIQUE,
            direction VARCHAR(10) NOT NULL,
            adjustment_date DATE NOT NULL,
            warehouse_id INT NULL REFERENCES warehouses(id) ON DELETE SET NULL,
            reason VARCHAR(100) NOT NULL,
            note TEXT NULL,
            created_by INT NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
            created_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        CREATE TABLE IF NOT EXISTS inventory_adjustment_items (
            id BIGSERIAL PRIMARY KEY,
            adjustment_id BIGINT NOT NULL REFERENCES inventory_adjustments(id) ON DELETE CASCADE,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE RESTRICT,
            qty DOUBLE PRECISION NOT NULL DEFAULT 0,
            buy_price NUMERIC(15,2) NULL,
            note TEXT NULL
        );

        CREATE INDEX IF NOT EXISTS idx_inventory_adjustments_dir_date ON inventory_adjustments(direction, adjustment_date);
        CREATE INDEX IF NOT EXISTS idx_inventory_adjustment_items_adj ON inventory_adjustment_items(adjustment_id);
    ");
};

