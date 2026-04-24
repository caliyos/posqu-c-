<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS promotions (
            id BIGSERIAL PRIMARY KEY,
            name VARCHAR(150) NOT NULL,
            promo_type VARCHAR(20) NOT NULL,
            status VARCHAR(20) NOT NULL DEFAULT 'aktif',
            start_date DATE NULL,
            end_date DATE NULL,
            priority INT NOT NULL DEFAULT 0,
            config_json TEXT NOT NULL DEFAULT '{}',
            created_by INT NULL REFERENCES users(id) ON DELETE SET NULL,
            created_at TIMESTAMP NOT NULL DEFAULT NOW(),
            updated_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        ALTER TABLE promotions ADD COLUMN IF NOT EXISTS promo_type VARCHAR(20) NULL;
        ALTER TABLE promotions ADD COLUMN IF NOT EXISTS status VARCHAR(20) NOT NULL DEFAULT 'aktif';
        ALTER TABLE promotions ADD COLUMN IF NOT EXISTS priority INT NOT NULL DEFAULT 0;
        ALTER TABLE promotions ADD COLUMN IF NOT EXISTS config_json TEXT NOT NULL DEFAULT '{}';
        ALTER TABLE promotions ADD COLUMN IF NOT EXISTS created_by INT NULL;

        DO $$
        BEGIN
            IF EXISTS (
                SELECT 1 FROM information_schema.columns
                WHERE table_schema = current_schema()
                  AND table_name = 'promotions'
                  AND column_name = 'type'
            ) THEN
                UPDATE promotions
                SET promo_type = COALESCE(NULLIF(promo_type, ''), type)
                WHERE (promo_type IS NULL OR promo_type = '')
                  AND type IS NOT NULL
                  AND type <> '';
            END IF;
        END $$;

        DO $$
        BEGIN
            IF EXISTS (
                SELECT 1 FROM information_schema.columns
                WHERE table_schema = current_schema()
                  AND table_name = 'promotions'
                  AND column_name = 'is_active'
            ) THEN
                UPDATE promotions
                SET status = CASE WHEN COALESCE(is_active, TRUE) THEN 'aktif' ELSE 'nonaktif' END
                WHERE status IS NULL OR status = '';
            END IF;
        END $$;

        UPDATE promotions
        SET promo_type = COALESCE(promo_type, 'DISKON')
        WHERE promo_type IS NULL OR promo_type = '';

        CREATE INDEX IF NOT EXISTS idx_promotions_type_status ON promotions(promo_type, status);
        CREATE INDEX IF NOT EXISTS idx_promotions_period ON promotions(start_date, end_date);
        CREATE INDEX IF NOT EXISTS idx_promotions_priority ON promotions(priority);
    ");
};
