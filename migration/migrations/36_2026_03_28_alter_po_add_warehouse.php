<?php
return function($db) {
    $db->exec("
        ALTER TABLE purchase_orders 
        ADD COLUMN IF NOT EXISTS warehouse_id INT NULL REFERENCES warehouses(id) ON DELETE SET NULL;
    ");
};