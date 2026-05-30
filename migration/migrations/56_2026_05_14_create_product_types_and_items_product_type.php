<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS product_types (
            code VARCHAR(24) PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            created_at TIMESTAMP DEFAULT now()
        );

        INSERT INTO product_types (code, name)
        VALUES
            ('stockable', 'Stockable (Inventory Item)'),
            ('consumable', 'Consumable (Non-Stock Item)'),
            ('service', 'Service'),
            ('manufactured', 'Paket / Bundle)')
        ON CONFLICT (code) DO NOTHING;

        ALTER TABLE items
            ADD COLUMN IF NOT EXISTS product_type_code VARCHAR(24);

        UPDATE items
        SET product_type_code =
            CASE
                WHEN COALESCE(is_produksi, FALSE) = TRUE OR COALESCE(is_have_bahan, FALSE) = TRUE THEN 'manufactured'
                WHEN COALESCE(is_inventory_p, FALSE) = TRUE THEN 'stockable'
                ELSE 'consumable'
            END
        WHERE product_type_code IS NULL OR product_type_code = '';

        ALTER TABLE items
            ALTER COLUMN product_type_code SET DEFAULT 'stockable';

        ALTER TABLE items
            ALTER COLUMN product_type_code SET NOT NULL;

        DO $$
        BEGIN
            IF NOT EXISTS (
                SELECT 1
                FROM information_schema.table_constraints tc
                WHERE tc.table_schema='public'
                  AND tc.table_name='items'
                  AND tc.constraint_type='FOREIGN KEY'
                  AND tc.constraint_name='items_product_type_code_fkey'
            ) THEN
                ALTER TABLE items
                    ADD CONSTRAINT items_product_type_code_fkey
                    FOREIGN KEY (product_type_code) REFERENCES product_types(code);
            END IF;
        END $$;
    ");
};

