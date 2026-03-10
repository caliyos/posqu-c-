<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS unit_variants (
            id SERIAL PRIMARY KEY,
            item_id INT NOT NULL,
            unit_id INT NOT NULL,
            conversion INT NOT NULL CHECK (conversion > 0),
            sell_price NUMERIC(12,2) NOT NULL,
            profit NUMERIC(12,2) NOT NULL,
            minqty NUMERIC(12,2) NULL,
            is_base_unit BOOLEAN DEFAULT FALSE,
            barcode_suffix TEXT,

            created_at TIMESTAMP DEFAULT NOW(),
            updated_at TIMESTAMP DEFAULT NOW(),

            is_active BOOLEAN DEFAULT TRUE,
            
            CONSTRAINT fk_unit_variants_unit FOREIGN KEY (unit_id) REFERENCES units(id) ON DELETE NO ACTION ON UPDATE NO ACTION
        );
    ");
};
