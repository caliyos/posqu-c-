<?php
require_once __DIR__ . '/../Database.php';
use App\Config\Database;

try {

    $db = Database::connect();

    echo "\n▶ Seeding: default_price_levels.php\n";

    $levels = [
        [1, 'retail', 'Harga eceran standar'],
        [2, 'grosir', 'Harga grosir untuk pedagang'],
        [3, 'member', 'Harga khusus member terdaftar'],
    ];

    $stmt = $db->prepare("
        INSERT INTO price_levels (id, name, description)
        VALUES (?, ?, ?)
        ON CONFLICT (id) DO NOTHING
    ");

    foreach ($levels as $l) {
        $stmt->execute($l);
        echo "✅ Inserted price level: {$l[1]}\n";
    }

    // fix sequence
    $db->exec("
        SELECT setval(
            'price_levels_id_seq',
            (SELECT COALESCE(MAX(id),0) FROM price_levels)
        )
    ");

    // default customer level
    $db->exec("
        UPDATE customers 
        SET price_level_id = 1 
        WHERE price_level_id IS NULL
    ");

    echo "✅ Updated customers default level\n";

} catch (Exception $e) {
    echo "❌ Seeder price_levels error: " . $e->getMessage() . "\n";
}