<?php
return function($db) {
    $db->exec("
        ALTER TABLE purchase_orders ADD COLUMN IF NOT EXISTS po_number VARCHAR(50) NULL;
        ALTER TABLE purchase_orders ADD COLUMN IF NOT EXISTS purchase_number VARCHAR(50) NULL;

        ALTER TABLE purchase_orders ADD COLUMN IF NOT EXISTS total_before_tax NUMERIC(18,2) NOT NULL DEFAULT 0;
        ALTER TABLE purchase_orders ADD COLUMN IF NOT EXISTS tax_mode VARCHAR(20) NOT NULL DEFAULT 'NON';
        ALTER TABLE purchase_orders ADD COLUMN IF NOT EXISTS tax_rate NUMERIC(6,2) NOT NULL DEFAULT 0;
        ALTER TABLE purchase_orders ADD COLUMN IF NOT EXISTS tax_amount NUMERIC(18,2) NOT NULL DEFAULT 0;

        CREATE UNIQUE INDEX IF NOT EXISTS ux_purchase_orders_po_number ON purchase_orders(po_number);
        CREATE UNIQUE INDEX IF NOT EXISTS ux_purchase_orders_purchase_number ON purchase_orders(purchase_number);
    ");
};

