<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS struk_setting (
            id SERIAL PRIMARY KEY,
            judul TEXT,
            alamat TEXT,
            telepon TEXT,
            footer TEXT,
            logo BYTEA,
            is_visible_nama_toko BOOLEAN DEFAULT TRUE,
            is_visible_alamat BOOLEAN DEFAULT TRUE,
            is_visible_telepon BOOLEAN DEFAULT TRUE,
            is_visible_footer BOOLEAN DEFAULT TRUE,
            is_visible_logo BOOLEAN DEFAULT TRUE,
            updated_at TIMESTAMP DEFAULT NOW()
        );
    ");
};
