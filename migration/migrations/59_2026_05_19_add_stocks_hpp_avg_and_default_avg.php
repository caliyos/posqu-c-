<?php

return function($db) {

    // 1. tambah kolom HPP AVG (aman untuk ERP)
    $db->exec("
        ALTER TABLE stocks
        ADD COLUMN IF NOT EXISTS hpp_avg NUMERIC(18,6) NOT NULL DEFAULT 0;
    ");

    // 2. set default metode valuation
    $db->exec("
        ALTER TABLE items
        ALTER COLUMN valuation_method SET DEFAULT 'AVG';
    ");

    // 3. normalisasi data lama items
    $db->exec("
        UPDATE items
        SET valuation_method = 'AVG'
        WHERE valuation_method IS NULL
           OR TRIM(valuation_method) = ''
           OR UPPER(valuation_method) = 'FIFO';
    ");
};