<?php
return function($db) {

    $db->exec("
        CREATE TABLE IF NOT EXISTS units (
            id SERIAL PRIMARY KEY,
            name VARCHAR(50) NOT NULL,
            abbr VARCHAR(10) NOT NULL UNIQUE,
            ord INT NULL,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
    ");

    // Tambahkan trigger untuk auto-update kolom updated_at
    $db->exec("
        CREATE OR REPLACE FUNCTION update_units_timestamp()
        RETURNS TRIGGER AS \$\$
        BEGIN
            NEW.updated_at = CURRENT_TIMESTAMP;
            RETURN NEW;
        END;
        \$\$ LANGUAGE plpgsql;
    ");

    $db->exec("
        DO \$\$
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM pg_trigger WHERE tgname = 'set_timestamp_units'
            ) THEN
                CREATE TRIGGER set_timestamp_units
                BEFORE UPDATE ON units
                FOR EACH ROW
                EXECUTE FUNCTION update_units_timestamp();
            END IF;
        END;
        \$\$;
    ");



    // Tambahkan index opsional untuk optimasi pencarian berdasarkan abbr dan name
    $db->exec("CREATE INDEX IF NOT EXISTS idx_units_abbr ON units(abbr)");
    $db->exec("CREATE INDEX IF NOT EXISTS idx_units_name ON units(name)");

};
