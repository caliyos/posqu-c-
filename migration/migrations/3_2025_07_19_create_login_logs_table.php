<?php
return function($db) {

    $db->exec("
        CREATE TABLE IF NOT EXISTS login_logs (
            id SERIAL PRIMARY KEY,
            user_id INT NULL,
            email VARCHAR(150),
            ip_address VARCHAR(45) NOT NULL,
            user_agent TEXT,
            terminal TEXT,
            shift TEXT,
            status VARCHAR(10) NOT NULL CHECK (status IN ('sukses', 'gagal')),
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            CONSTRAINT fk_login_logs_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL
        );
    ");

    // Index opsional untuk optimasi pencarian berdasarkan email & status
    // $db->exec(\"CREATE INDEX IF NOT EXISTS idx_login_logs_email ON login_logs(email)\");
    // $db->exec(\"CREATE INDEX IF NOT EXISTS idx_login_logs_status ON login_logs(status)\");
};
