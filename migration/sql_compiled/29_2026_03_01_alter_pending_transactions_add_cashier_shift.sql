
        ALTER TABLE pending_transactions
        ADD COLUMN IF NOT EXISTS cashier_shift_id INT;
    
;

        DO $$
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM information_schema.table_constraints
                WHERE constraint_name = 'fk_pending_cashier_shift'
            ) THEN
                ALTER TABLE pending_transactions
                ADD CONSTRAINT fk_pending_cashier_shift
                FOREIGN KEY (cashier_shift_id) REFERENCES cashier_shifts(id) ON DELETE SET NULL;
            END IF;
        END $$;
    
