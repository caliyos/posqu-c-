<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS cashier_shifts (
            id SERIAL PRIMARY KEY,
            user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
            terminal_id INT NOT NULL REFERENCES terminals(id) ON DELETE CASCADE,
            opened_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
            closed_at TIMESTAMP,
            opening_cash NUMERIC(18,2) NOT NULL DEFAULT 0,
            expected_cash NUMERIC(18,2) NOT NULL DEFAULT 0,
            closing_cash NUMERIC(18,2),
            difference_cash NUMERIC(18,2),
            status VARCHAR(20) NOT NULL DEFAULT 'open',
            note TEXT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
    ");
};
