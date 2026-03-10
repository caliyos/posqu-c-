<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS customers (
            id SERIAL PRIMARY KEY,
            name VARCHAR(150) NOT NULL,
            phone VARCHAR(30) DEFAULT NULL,
            note TEXT DEFAULT NULL,
            created_by INT DEFAULT NULL, -- 🔹 user/kasir yang menambahkan customer
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_at TIMESTAMP NULL
        );
    ");
};
