<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS purchase_returns (
            id BIGSERIAL PRIMARY KEY,
            return_number VARCHAR(50) NOT NULL UNIQUE,
            return_date DATE NOT NULL,
            supplier_id BIGINT NOT NULL REFERENCES suppliers(id) ON DELETE RESTRICT,
            warehouse_id INT NOT NULL REFERENCES warehouses(id) ON DELETE RESTRICT,
            note TEXT NULL,
            created_by INT NULL REFERENCES users(id) ON DELETE SET NULL,
            created_at TIMESTAMP NOT NULL DEFAULT NOW(),
            updated_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        CREATE TABLE IF NOT EXISTS purchase_return_items (
            id BIGSERIAL PRIMARY KEY,
            purchase_return_id BIGINT NOT NULL REFERENCES purchase_returns(id) ON DELETE CASCADE,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE RESTRICT,
            qty DOUBLE PRECISION NOT NULL DEFAULT 0,
            buy_price NUMERIC(18,2) NOT NULL DEFAULT 0,
            note TEXT NULL,
            created_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        CREATE INDEX IF NOT EXISTS idx_purchase_return_items_return_id ON purchase_return_items(purchase_return_id);
    ");
};

