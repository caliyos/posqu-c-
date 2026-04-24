<?php
return function($db) {
    $db->exec("
        ALTER TABLE stock_layers ADD COLUMN IF NOT EXISTS expired_at DATE NULL;
        CREATE INDEX IF NOT EXISTS idx_stock_layers_item_wh_exp ON stock_layers(item_id, warehouse_id, expired_at);
    ");
};

