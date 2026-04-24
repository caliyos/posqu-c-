<?php
return function($db) {
    $db->exec("
        ALTER TABLE transactions ADD COLUMN IF NOT EXISTS ts_total_before_tax NUMERIC(18,2) NOT NULL DEFAULT 0;
        ALTER TABLE transactions ADD COLUMN IF NOT EXISTS ts_tax_mode VARCHAR(20) NOT NULL DEFAULT 'NON';
        ALTER TABLE transactions ADD COLUMN IF NOT EXISTS ts_tax_rate NUMERIC(6,2) NOT NULL DEFAULT 0;
        ALTER TABLE transactions ADD COLUMN IF NOT EXISTS ts_tax_amount NUMERIC(18,2) NOT NULL DEFAULT 0;
    ");
};

