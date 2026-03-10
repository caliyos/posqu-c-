<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS transaction_details (
            tsd_id SERIAL PRIMARY KEY,
            ts_id INT NOT NULL,
            item_id INT NOT NULL,
            tsd_name TEXT,
            tsd_barcode TEXT,
            tsd_quantity NUMERIC(18,2) NOT NULL,
            tsd_unit TEXT NOT NULL,
            tsd_price_per_unit NUMERIC(18,2) DEFAULT 0,
            tsd_buy_price NUMERIC(18,2) DEFAULT 0,
            tsd_sell_price NUMERIC(18,2) DEFAULT 0,
            tsd_unit_variant TEXT,
            tsd_conversion_rate NUMERIC(12,4) DEFAULT 1,
            tsd_total NUMERIC(20,2) DEFAULT 0,
            tsd_note TEXT,
            tsd_discount_per_item NUMERIC(15,2) DEFAULT 0,
            tsd_discount_percentage NUMERIC(5,2) DEFAULT 0,
            tsd_discount_total NUMERIC(18,2) DEFAULT 0,
            tsd_tax NUMERIC(18,2) DEFAULT 0,
            created_by INT,
            updated_by INT,
            deleted_by INT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_at TIMESTAMP NULL,
            cart_session_code VARCHAR(50),


            CONSTRAINT fk_transaction_details_transaction 
                FOREIGN KEY (ts_id) REFERENCES transactions(ts_id) ON DELETE CASCADE
        );
    ");
};
