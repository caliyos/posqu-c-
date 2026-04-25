<?php
return function($db) {
    $db->exec("
       ALTER TABLE license_activations 
MODIFY expires_at DATETIME;
    ");
};

