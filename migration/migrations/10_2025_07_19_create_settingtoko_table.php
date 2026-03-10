<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS settingtoko (
            id SERIAL PRIMARY KEY,
            nama VARCHAR(255) NOT NULL,
            alamat TEXT NOT NULL,
            npwp VARCHAR(50),
            logo BYTEA
        );
    ");
};
