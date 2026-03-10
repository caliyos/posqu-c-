<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS terminals (
            id SERIAL PRIMARY KEY,
            terminal_name VARCHAR(50) NOT NULL UNIQUE,
            pc_id VARCHAR(50) NOT NULL,
            description TEXT
        );
    ");

    $db->exec("
       CREATE TABLE user_terminal (
            user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
            terminal_id INT NOT NULL REFERENCES terminals(id) ON DELETE CASCADE,
            PRIMARY KEY (user_id, terminal_id)
        );
    ");
    $db->exec("
      CREATE TABLE shifts (
            id SERIAL PRIMARY KEY,
            name VARCHAR(50) NOT NULL,
            start_time TIME NOT NULL,
            end_time TIME NOT NULL
        );
    ");

    
};
