<?php
return function($db) {
    $db->exec("
        -- 1. Pindahkan reserved_stock ke tabel stocks
        ALTER TABLE stocks ADD COLUMN IF NOT EXISTS reserved_qty DOUBLE PRECISION NOT NULL DEFAULT 0;

        -- 2. Tambahkan valuation_method ke tabel items
        ALTER TABLE items ADD COLUMN IF NOT EXISTS valuation_method VARCHAR(10) DEFAULT 'FIFO';

        -- 3. Hapus stock dan reserved_stock dari items
        ALTER TABLE items DROP COLUMN IF EXISTS stock;
        ALTER TABLE items DROP COLUMN IF EXISTS reserved_stock;
    ");
};