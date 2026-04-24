<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS item_materials (
            id BIGSERIAL PRIMARY KEY,
            parent_item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE CASCADE,
            component_item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE RESTRICT,
            qty NUMERIC(15,4) NOT NULL DEFAULT 0,
            unit_id INT NOT NULL REFERENCES units(id) ON DELETE RESTRICT,
            unit_cost NUMERIC(15,2) NOT NULL DEFAULT 0,
            created_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        CREATE INDEX IF NOT EXISTS idx_item_materials_parent ON item_materials(parent_item_id);
    ");
};
