<?php
return function($db) {
    $db->exec("
        ALTER TABLE transactions
        ADD COLUMN IF NOT EXISTS warehouse_id INT NOT NULL DEFAULT 1;
    ");

    $db->exec("
        ALTER TABLE transaction_details
        ADD COLUMN IF NOT EXISTS warehouse_id INT NOT NULL DEFAULT 1;
    ");

    $db->exec("
        ALTER TABLE pending_transactions
        ADD COLUMN IF NOT EXISTS warehouse_id INT NOT NULL DEFAULT 1;
    ");

    $db->exec("
        DO $$
        BEGIN
            IF to_regclass('public.warehouses') IS NOT NULL THEN
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.table_constraints
                    WHERE constraint_name = 'fk_transactions_warehouse'
                ) THEN
                    ALTER TABLE transactions
                    ADD CONSTRAINT fk_transactions_warehouse
                    FOREIGN KEY (warehouse_id) REFERENCES warehouses(id) ON DELETE RESTRICT;
                END IF;

                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.table_constraints
                    WHERE constraint_name = 'fk_transaction_details_warehouse'
                ) THEN
                    ALTER TABLE transaction_details
                    ADD CONSTRAINT fk_transaction_details_warehouse
                    FOREIGN KEY (warehouse_id) REFERENCES warehouses(id) ON DELETE RESTRICT;
                END IF;

                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.table_constraints
                    WHERE constraint_name = 'fk_pending_transactions_warehouse'
                ) THEN
                    ALTER TABLE pending_transactions
                    ADD CONSTRAINT fk_pending_transactions_warehouse
                    FOREIGN KEY (warehouse_id) REFERENCES warehouses(id) ON DELETE RESTRICT;
                END IF;
            END IF;
        END $$;
    ");

    $db->exec("
        CREATE INDEX IF NOT EXISTS idx_transactions_wh_created_at
        ON transactions(warehouse_id, created_at);
    ");

    $db->exec("
        CREATE INDEX IF NOT EXISTS idx_transaction_details_wh_ts
        ON transaction_details(warehouse_id, ts_id);
    ");

    $db->exec("
        CREATE INDEX IF NOT EXISTS idx_pending_transactions_wh_session
        ON pending_transactions(warehouse_id, cart_session_code);
    ");
};

