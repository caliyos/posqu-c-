<?php
return function($db) {
    $db->exec("
        DROP TABLE IF EXISTS audit_logs;

        CREATE TABLE audit_logs (
            id BIGSERIAL PRIMARY KEY,
            user_id BIGINT,
            action VARCHAR(100) NOT NULL,
            reference_id BIGINT,
            reference_table VARCHAR(50),
            description TEXT,
            ip_address VARCHAR(45),
            user_agent TEXT,
            details TEXT,
            created_at TIMESTAMP(6) NOT NULL DEFAULT CURRENT_TIMESTAMP
        );

        ALTER TABLE audit_logs
            ADD CONSTRAINT audit_logs_user_id_fkey
            FOREIGN KEY (user_id)
            REFERENCES users (id)
            ON DELETE SET NULL
            ON UPDATE NO ACTION;
    ");
};
