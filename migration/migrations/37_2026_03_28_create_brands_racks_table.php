<?php
return function($db) {
    // 1. Create brands table
    $db->exec("
        CREATE TABLE IF NOT EXISTS brands (
            id SERIAL PRIMARY KEY,
            name VARCHAR(255) NOT NULL UNIQUE,
            created_at TIMESTAMP DEFAULT NOW(),
            updated_at TIMESTAMP DEFAULT NOW()
        );
    ");

    // 2. Create racks table
    $db->exec("
        CREATE TABLE IF NOT EXISTS racks (
            id SERIAL PRIMARY KEY,
            name VARCHAR(255) NOT NULL UNIQUE,
            created_at TIMESTAMP DEFAULT NOW(),
            updated_at TIMESTAMP DEFAULT NOW()
        );
    ");

    // 3. Alter items table to add brand_id and rack_id
    $db->exec("
        ALTER TABLE items 
        ADD COLUMN IF NOT EXISTS brand_id INT NULL REFERENCES brands(id) ON DELETE SET NULL,
        ADD COLUMN IF NOT EXISTS rack_id INT NULL REFERENCES racks(id) ON DELETE SET NULL;
    ");
};