<?php

require_once __DIR__ . '/Database.php';

use App\Config\Database;

$db = Database::connect();

// Ambil semua file migrasi
$migrations = glob(__DIR__ . '/migrations/*.php');

// Urutkan berdasarkan nama file ASCENDING
natsort($migrations); // <-- perbaikan utama
$migrations = array_values($migrations);

foreach ($migrations as $file) {
    echo "Migrating: " . basename($file) . "\n";
    $migration = require $file;

    if (is_callable($migration)) {
        try {
            $migration($db);
            echo "✓ Done\n";
        } catch (PDOException $e) {
            echo "✗ Failed: " . $e->getMessage() . "\n";
        }
    } else {
        echo "✗ Failed (not callable)\n";
    }
}
