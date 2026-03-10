<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS orders (
           order_id SERIAL PRIMARY KEY,
    order_number VARCHAR(50) NOT NULL,          -- Nomor order, misal ORDR-20250810-0001
    order_code VARCHAR(50) NOT NULL,            -- Kode unik tambahan untuk order
    order_total NUMERIC(18,2) NOT NULL DEFAULT 0,         -- Total harga order (belum bayar)
    
    order_status SMALLINT NOT NULL CHECK (order_status IN (0, 1, 2, 3)),
    -- 0: pending, 1: completed (paid), 2: cancelled, 3: processing
    
    payment_method VARCHAR(50),        -- Metode pembayaran (Cash, Card, QRIS, Bank Transfer, COD)
    delivery_method VARCHAR(50) NOT NULL,       -- Metode pengantaran (Di Antar, Di Ambil Sendiri)
    delivery_time TIMESTAMP NULL,                -- Waktu pengantaran (jika delivery_method = 'Di Antar')
    
    order_note TEXT,                            -- Catatan order / keterangan
    
    customer_id INT NULL,                       -- Refer ke tabel customers, bisa null
    customer_name VARCHAR(255),                 -- Nama customer jika tanpa ID
    customer_phone VARCHAR(50),                 -- No HP customer
    
    terminal_id INT,
    shift_id INT,
    
    user_id INT,       -- Kasir / pelayan yang buat order
    created_by INT,    -- User yang input order
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP NULL,
    
    CONSTRAINT fk_order_user FOREIGN KEY (user_id) REFERENCES users(id),
    CONSTRAINT fk_order_created_by FOREIGN KEY (created_by) REFERENCES users(id)
        );
    ");
};
