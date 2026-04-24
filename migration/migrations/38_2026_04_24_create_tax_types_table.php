<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS tax_types (
            id BIGSERIAL PRIMARY KEY,
            code VARCHAR(20) NOT NULL UNIQUE,
            name VARCHAR(100) NOT NULL,
            is_active BOOLEAN NOT NULL DEFAULT TRUE,
            created_at TIMESTAMP NOT NULL DEFAULT NOW(),
            updated_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        INSERT INTO tax_types (code, name, is_active)
        VALUES
          ('NON', 'Non PPN', TRUE),
          ('INCLUDE', 'PPN Include', TRUE),
          ('EXCLUDE', 'PPN Exclude', TRUE)
        ON CONFLICT (code)
        DO UPDATE SET name = EXCLUDED.name, updated_at = NOW();
    ");
};

