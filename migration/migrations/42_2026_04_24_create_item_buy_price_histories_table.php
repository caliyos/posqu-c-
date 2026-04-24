<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS item_buy_price_histories (
            id BIGSERIAL PRIMARY KEY,
            item_id BIGINT NOT NULL REFERENCES items(id) ON DELETE RESTRICT,
            old_price NUMERIC(18,2) NOT NULL DEFAULT 0,
            new_price NUMERIC(18,2) NOT NULL DEFAULT 0,
            source_type VARCHAR(30) NULL,
            source_id BIGINT NULL,
            changed_by INT NULL REFERENCES users(id) ON DELETE SET NULL,
            changed_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        CREATE INDEX IF NOT EXISTS idx_buy_price_hist_item ON item_buy_price_histories(item_id);
        CREATE INDEX IF NOT EXISTS idx_buy_price_hist_source ON item_buy_price_histories(source_type, source_id);
    ");
};

