<?php

return function($db) {

    $db->exec("
        -- 1. Stock fields (ERP SAFE TYPE)
        ALTER TABLE stocks
        ADD COLUMN IF NOT EXISTS reserved_qty NUMERIC(18,3) NOT NULL DEFAULT 0;

        ALTER TABLE stocks
        ADD COLUMN IF NOT EXISTS min_qty NUMERIC(18,3) NOT NULL DEFAULT 0;

        -- 2. Valuation method
        ALTER TABLE items
        ADD COLUMN IF NOT EXISTS valuation_method VARCHAR(20) DEFAULT 'FIFO';

        -- enforce valid values (opsional tapi bagus)
        -- CHECK constraint bisa ditambah manual kalau PostgreSQL mendukung version kamu

        -- 3. clean architecture
        ALTER TABLE items DROP COLUMN IF EXISTS stock;
        ALTER TABLE items DROP COLUMN IF EXISTS reserved_stock;
    ");
};