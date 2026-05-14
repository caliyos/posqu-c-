<?php
return function($db) {
    $db->exec("
        ALTER TABLE users
        ADD COLUMN IF NOT EXISTS pin_hash TEXT;
    ");
};

