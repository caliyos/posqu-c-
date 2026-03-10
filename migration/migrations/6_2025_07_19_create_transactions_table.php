<?php
return function($db) {
    $db->exec("

        CREATE TABLE IF NOT EXISTS transactions (
            ts_id SERIAL PRIMARY KEY,
            ts_numbering VARCHAR(50) NOT NULL,
            ts_code VARCHAR(50) NOT NULL,
            ts_total NUMERIC(18,2) NOT NULL,
            ts_payment_amount NUMERIC(18,2) NOT NULL,
            ts_cashback NUMERIC(15,2) DEFAULT 0,
            ts_method VARCHAR(50) NOT NULL,

            -- status transaksi
            -- 1 = paid
            -- 2 = unpaid
            -- 3 = partial
            ts_status SMALLINT NOT NULL,

            ts_change NUMERIC(15,2) DEFAULT 0,
            ts_internal_note TEXT,
            ts_note TEXT,
            ts_customer INT,
            ts_freename VARCHAR(255),
            order_id INT NULL,
            terminal_id INT,
            shift_id INT,
            user_id INT,
            created_by INT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_at TIMESTAMP NULL,

            ts_global_discount_percent NUMERIC(5,2) DEFAULT 0,
            ts_global_discount_amount NUMERIC(18,2) DEFAULT 0,
            ts_delivery_amount NUMERIC(15,2) DEFAULT 0,
            ts_grand_total NUMERIC(18,2) DEFAULT 0,

            -- 🔥 untuk sistem cicilan
            ts_due_amount NUMERIC(18,2) DEFAULT 0,

            cart_session_code VARCHAR(50)
        );

      
    ");
};
