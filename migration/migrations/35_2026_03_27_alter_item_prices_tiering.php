<?php
return function($db) {
    $db->exec("
        -- 1. price_levels (misal: retail, grosir, member)
        CREATE TABLE IF NOT EXISTS price_levels (
            id SERIAL PRIMARY KEY,
            name VARCHAR(50) NOT NULL UNIQUE,
            description VARCHAR(255) NULL,
            is_active BOOLEAN DEFAULT TRUE,
            created_at TIMESTAMP DEFAULT now()
        );

        -- Tambahkan relasi price_level_id ke customers
        ALTER TABLE customers ADD COLUMN IF NOT EXISTS price_level_id INT REFERENCES price_levels(id) ON DELETE SET NULL;

        -- 2. Modifikasi item_prices untuk mendukung tier pricing multi dimensi
        -- Hapus constraint UNIQUE lama jika ada
        ALTER TABLE item_prices DROP CONSTRAINT IF EXISTS item_prices_item_id_unit_id_min_qty_key;

        -- Tambahkan kolom price_level_id
        ALTER TABLE item_prices ADD COLUMN IF NOT EXISTS price_level_id INT REFERENCES price_levels(id) ON DELETE CASCADE;
        
        -- Berikan nilai default untuk data lama (jika ada, anggap ID 1 adalah Retail)
        -- Ini butuh memastikan price_level dgn ID 1 ada, nanti diurus di seeder.
        UPDATE item_prices SET price_level_id = 1 WHERE price_level_id IS NULL;
        
        -- Jadikan NOT NULL setelah diisi default
        ALTER TABLE item_prices ALTER COLUMN price_level_id SET NOT NULL;

        -- Tambahkan kolom max_qty
        ALTER TABLE item_prices ADD COLUMN IF NOT EXISTS max_qty INT NULL;

        -- Buat constraint UNIQUE baru
        ALTER TABLE item_prices ADD CONSTRAINT item_prices_item_unit_level_min_key UNIQUE (item_id, unit_id, price_level_id, min_qty);
    ");
};