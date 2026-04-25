<?php
return function($db) {
    $db->exec("
        ALTER TABLE stock_log DROP CONSTRAINT IF EXISTS stock_log_tipe_transaksi_check;

        ALTER TABLE stock_log
            ALTER COLUMN qty_masuk TYPE NUMERIC(18,4) USING qty_masuk::numeric,
            ALTER COLUMN qty_keluar TYPE NUMERIC(18,4) USING qty_keluar::numeric,
            ALTER COLUMN sisa_stock TYPE NUMERIC(18,4) USING sisa_stock::numeric;

        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS warehouse_id INT NOT NULL DEFAULT 1;
        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS ref_type VARCHAR(30) NULL;
        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS ref_id INT NULL;
        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS supplier_id INT NULL;
        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS unit_cost NUMERIC(18,2) NULL;
        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS method VARCHAR(20) NULL;
        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS stock_layer_id BIGINT NULL;
        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS is_allocation BOOLEAN NOT NULL DEFAULT FALSE;
        ALTER TABLE stock_log ADD COLUMN IF NOT EXISTS parent_id INT NULL;

        CREATE INDEX IF NOT EXISTS idx_stock_log_product_wh_created ON stock_log(product_id, warehouse_id, created_at, id);
        CREATE INDEX IF NOT EXISTS idx_stock_log_parent ON stock_log(parent_id);
    ");
};

