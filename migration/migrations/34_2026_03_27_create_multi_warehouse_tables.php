<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS warehouses (
            id SERIAL PRIMARY KEY,
            name VARCHAR(255) NOT NULL,
            type VARCHAR(50) DEFAULT 'store',
            is_active BOOLEAN DEFAULT TRUE,
            created_at TIMESTAMP DEFAULT now()
        );

        CREATE TABLE IF NOT EXISTS stocks (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            warehouse_id INT NOT NULL REFERENCES warehouses(id) ON DELETE CASCADE,
            qty NUMERIC(18,4) NOT NULL DEFAULT 0,
            UNIQUE(item_id, warehouse_id)
        );

        CREATE TABLE IF NOT EXISTS stock_layers (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            warehouse_id INT NOT NULL REFERENCES warehouses(id) ON DELETE CASCADE,
            qty_remaining NUMERIC(18,4) NOT NULL DEFAULT 0,
            buy_price NUMERIC(18,4) NOT NULL,
            created_at TIMESTAMP DEFAULT now()
        );

        CREATE TABLE IF NOT EXISTS item_units (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            unit_id INT NOT NULL REFERENCES units(id) ON DELETE CASCADE,
            conversion_qty NUMERIC(18,6) NOT NULL DEFAULT 1,
            is_default BOOLEAN DEFAULT FALSE,
            UNIQUE(item_id, unit_id)
        );
    ");
};