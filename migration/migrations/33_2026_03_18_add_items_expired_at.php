<?php
return function($db) {
    $db->exec("
        ALTER TABLE items
        ADD COLUMN IF NOT EXISTS expired_at DATE;
    ");
};
