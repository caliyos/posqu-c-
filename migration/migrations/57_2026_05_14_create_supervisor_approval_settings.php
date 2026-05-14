<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS supervisor_approval_settings (
            action_code VARCHAR(64) PRIMARY KEY,
            action_name VARCHAR(120) NOT NULL,
            is_enabled BOOLEAN NOT NULL DEFAULT TRUE,
            require_for_cashier BOOLEAN NOT NULL DEFAULT TRUE,
            min_amount NUMERIC(18,2) NULL,
            require_reason BOOLEAN NOT NULL DEFAULT FALSE,
            sort_order INT NOT NULL DEFAULT 0,
            updated_at TIMESTAMP NOT NULL DEFAULT now()
        );

        INSERT INTO supervisor_approval_settings(action_code, action_name, is_enabled, require_for_cashier, min_amount, require_reason, sort_order)
        VALUES
            ('close_shift',      'Tutup Shift',                    TRUE, TRUE, NULL,     FALSE, 10),
            ('reprint_receipt',  'Reprint Struk / Invoice',        TRUE, TRUE, NULL,     FALSE, 20),
            ('delete_item',      'Hapus Item (Void Item)',         TRUE, TRUE, 100000,   TRUE,  30),
            ('void_transaction', 'Batal Seluruh Transaksi (Void)', TRUE, TRUE, NULL,     TRUE,  40),
            ('manual_discount',  'Diskon Manual',                  FALSE, TRUE, 0,       TRUE,  50),
            ('manual_price',     'Ubah Harga Manual',              FALSE, TRUE, 0,       TRUE,  60),
            ('refund',           'Refund / Retur',                 FALSE, TRUE, 0,       TRUE,  70)
        ON CONFLICT (action_code) DO NOTHING;
    ");
};

