<?php
return function($db) {
    $db->exec("
         ALTER DATABASE \"A-Posqu001\" SET TIME ZONE 'Asia/Makassar';
    ");
};
