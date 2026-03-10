<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS payment (
            payment_id SERIAL PRIMARY KEY,
            amount NUMERIC(10,2),
            payment_method VARCHAR(50),
            payment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
    ");
};
