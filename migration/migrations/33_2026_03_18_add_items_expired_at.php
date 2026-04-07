<?php
return function($db) {
    $db->exec("
        ALTER TABLE items
        ADD COLUMN IF NOT EXISTS expired_at DATE;
    ");

        $db->exec("
        ALTER TABLE items
        ADD COLUMN IF NOT EXISTS is_active BOOLEAN DEFAULT TRUE;
    ");
};
