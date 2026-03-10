<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS items (
            id BIGSERIAL PRIMARY KEY,
            name VARCHAR(255) NOT NULL,
            buy_price NUMERIC(15,2) NOT NULL,
            sell_price NUMERIC(15,2) NOT NULL,
            barcode VARCHAR(64) NOT NULL UNIQUE,
            stock DOUBLE PRECISION NOT NULL DEFAULT 0,
            reserved_stock DOUBLE PRECISION NULL DEFAULT 0,
            unit NUMERIC(15,2) NOT NULL,
            category_id INT NULL,

            note VARCHAR(255) NULL,
            picture VARCHAR(255) NULL,

      -- BOOLEAN SEMUA
            is_inventory_p BOOLEAN DEFAULT TRUE,
            is_purchasable BOOLEAN DEFAULT TRUE,
            is_sellable BOOLEAN DEFAULT TRUE,
            is_note_payment BOOLEAN DEFAULT FALSE,
            is_changeprice_p BOOLEAN DEFAULT FALSE,
            is_have_bahan BOOLEAN DEFAULT FALSE,
            is_box BOOLEAN DEFAULT FALSE,
            is_produksi BOOLEAN DEFAULT FALSE,

            discount_formula VARCHAR(50) NULL,
            
            created_at TIMESTAMP DEFAULT now(),
            updated_at TIMESTAMP DEFAULT now(),
            deleted_at TIMESTAMP,
            supplier_id INT NULL,
            flag INT
        );

CREATE TABLE item_prices (
    id SERIAL PRIMARY KEY,
    
    item_id INT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
    unit_id INT NOT NULL,
    
    min_qty INT NOT NULL CHECK (min_qty > 0),
    price NUMERIC(15,2) NOT NULL CHECK (price >= 0),

    is_active BOOLEAN DEFAULT TRUE,
    
    effective_from TIMESTAMP DEFAULT NOW(),
    effective_to TIMESTAMP NULL,

    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW(),

    UNIQUE (item_id, unit_id, min_qty)
);


        CREATE TABLE item_discounts (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            discount_formula VARCHAR(50) NOT NULL,
            start_date TIMESTAMP NULL,
            end_date TIMESTAMP NULL,
            created_at TIMESTAMP DEFAULT NOW()
        );


CREATE TABLE promotions (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,             -- Nama promo, misal Diskon Akhir Pekan
    type VARCHAR(50) NOT NULL,              -- Jenis promo: discount, b2g1, combo
    discount_formula VARCHAR(50),           -- Bisa persentase 10% atau nominal 5000 (untuk type=discount)
    start_date TIMESTAMP NULL,              -- Waktu mulai aktif
    end_date TIMESTAMP NULL,                -- Waktu selesai aktif
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW(),
    is_active BOOLEAN DEFAULT TRUE
);

CREATE TABLE promotion_items (
    id BIGSERIAL PRIMARY KEY,
    promotion_id BIGINT NOT NULL REFERENCES promotions(id) ON DELETE CASCADE,
    item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
    min_qty INT DEFAULT 1,                    -- Minimal pembelian item agar promo berlaku
    special_price NUMERIC(15,2) DEFAULT NULL, -- Harga khusus item untuk promo, opsional
    created_at TIMESTAMP DEFAULT NOW()
);


    ");
};
