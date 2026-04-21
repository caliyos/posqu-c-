<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS customers (
            id SERIAL PRIMARY KEY,
            
            -- basic identity
            name VARCHAR(150) NOT NULL,
            phone VARCHAR(30) DEFAULT NULL UNIQUE,
            email VARCHAR(150) DEFAULT NULL,
            address TEXT DEFAULT NULL,

            -- membership system
            is_member BOOLEAN DEFAULT TRUE, -- member / non-member
            member_code VARCHAR(50) DEFAULT NULL,

            -- loyalty level (tier)
            level VARCHAR(20) DEFAULT 'bronze', 
            -- bronze | silver | gold | platinum

            -- point system
            points INT DEFAULT 0,

            -- financial (optional POS advanced)
            credit_limit NUMERIC(12,2) DEFAULT 0,
            balance_due NUMERIC(12,2) DEFAULT 0,

            -- system tracking
            note TEXT DEFAULT NULL,
            created_by INT DEFAULT NULL,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_at TIMESTAMP NULL
        );
    ");
};