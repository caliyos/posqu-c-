<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS groups (
            id BIGSERIAL PRIMARY KEY,
            groupname VARCHAR(255) NOT NULL DEFAULT '',
            groupshortname VARCHAR(10) NOT NULL DEFAULT '',
            created_at TIMESTAMP,
            updated_at TIMESTAMP
        );
    ");
};
