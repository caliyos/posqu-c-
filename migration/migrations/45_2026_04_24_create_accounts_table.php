<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS accounts (
            id BIGSERIAL PRIMARY KEY,
            name VARCHAR(100) NOT NULL UNIQUE,
            type VARCHAR(20) NOT NULL,
            created_at TIMESTAMP NOT NULL DEFAULT NOW(),
            updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
            CONSTRAINT ck_accounts_type CHECK (type IN ('asset','revenue','expense'))
        );
    ");
};
