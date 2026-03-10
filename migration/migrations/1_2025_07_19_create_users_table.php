<?php

return function($db) {

    // Buat tabel users untuk PostgreSQL
    $db->exec("
        CREATE TABLE IF NOT EXISTS users (
            id SERIAL PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            username VARCHAR(100),
            email VARCHAR(150) UNIQUE,
            google_id VARCHAR(100) UNIQUE,
            picture VARCHAR(255),
            password_hash VARCHAR(255),
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
    ");
};
