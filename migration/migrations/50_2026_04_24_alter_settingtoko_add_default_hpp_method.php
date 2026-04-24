<?php
return function($db) {
    $db->exec("
        ALTER TABLE settingtoko ADD COLUMN IF NOT EXISTS default_hpp_method VARCHAR(20) NOT NULL DEFAULT 'FIFO';
        UPDATE settingtoko SET default_hpp_method = COALESCE(NULLIF(default_hpp_method,''), 'FIFO') WHERE id = 1;
    ");
};

