<?php
return function($db) {
    $db->exec("
       DROP TABLE IF EXISTS suppliers CASCADE;

CREATE TABLE suppliers (
    id INT PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    kode VARCHAR(5) NULL,
    contact_name VARCHAR(100) NULL,
    phone VARCHAR(50) NULL,
    email VARCHAR(100) NULL,
    address TEXT NULL,
    note TEXT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);
    ");
};
