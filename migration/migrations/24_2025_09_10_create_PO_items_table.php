<?php
return function($db) {
    $db->exec("
        DROP TABLE IF EXISTS purchase_order_items CASCADE;
        CREATE TABLE purchase_order_items (
            id BIGSERIAL PRIMARY KEY,
            po_id BIGINT NOT NULL REFERENCES purchase_orders(id) ON DELETE CASCADE,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE RESTRICT,
            unit TEXT,
            quantity NUMERIC(18,2) NOT NULL,
            unit_price NUMERIC(18,2) NOT NULL,
            note TEXT NULL,
            created_at TIMESTAMP NOT NULL DEFAULT NOW(),
            updated_at TIMESTAMP NOT NULL DEFAULT NOW()
        );
    ");
};
