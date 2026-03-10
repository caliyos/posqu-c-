<?php

return function($db) {

    // Buat tabel categories untuk PostgreSQL
    $db->exec("
        CREATE TABLE IF NOT EXISTS categories (
            id SERIAL PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            kode VARCHAR(5) NOT NULL,
            description TEXT,
            parent_id INT REFERENCES categories(id) ON DELETE SET NULL,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
    ");
};
