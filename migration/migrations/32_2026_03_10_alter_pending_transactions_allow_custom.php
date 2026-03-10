<?php
return function($db) {
    // Drop FK constraint on pending_transactions.item_id to allow custom items (item_id = 0)
    $db->exec("
        DO $$
        DECLARE
            fk_name text;
        BEGIN
            SELECT conname INTO fk_name
            FROM pg_constraint
            WHERE conrelid = 'pending_transactions'::regclass
              AND contype = 'f'
              AND pg_get_constraintdef(oid) ILIKE '%FOREIGN KEY (item_id)%';
            IF fk_name IS NOT NULL THEN
                EXECUTE format('ALTER TABLE pending_transactions DROP CONSTRAINT %I', fk_name);
            END IF;
        END$$;
    ");
};
