<?php
return function($db) {
    $db->exec("
        -- 1. warehouses
        CREATE TABLE IF NOT EXISTS warehouses (
            id SERIAL PRIMARY KEY,
            name VARCHAR(255) NOT NULL,
            type VARCHAR(50) DEFAULT 'store', -- store / warehouse / kitchen
            is_active BOOLEAN DEFAULT TRUE,
            created_at TIMESTAMP DEFAULT now()
        );

        -- 2. stocks (ringkasan per gudang)
        CREATE TABLE IF NOT EXISTS stocks (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            warehouse_id INT NOT NULL REFERENCES warehouses(id) ON DELETE CASCADE,
            qty DOUBLE PRECISION NOT NULL DEFAULT 0,
            UNIQUE(item_id, warehouse_id)
        );

        -- 3. stock_layers (FIFO)
        CREATE TABLE IF NOT EXISTS stock_layers (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            warehouse_id INT NOT NULL REFERENCES warehouses(id) ON DELETE CASCADE,
            qty_remaining DOUBLE PRECISION NOT NULL DEFAULT 0,
            buy_price NUMERIC(15,2) NOT NULL,
            created_at TIMESTAMP DEFAULT now()
        );

        -- 4. item_units (Multi Satuan)
        CREATE TABLE IF NOT EXISTS item_units (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            unit_id INT NOT NULL REFERENCES units(id) ON DELETE CASCADE,
            conversion_qty DOUBLE PRECISION NOT NULL DEFAULT 1,
            is_default BOOLEAN DEFAULT FALSE,
            UNIQUE(item_id, unit_id)
        );

        -- Note: Kolom stock dan reserved_stock pada tabel items akan di-drop
        -- setelah semua kode C# disesuaikan dengan multi-warehouse.
    ");
};