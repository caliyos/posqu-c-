<?php
require_once __DIR__ . '/../Database.php';
use App\Config\Database;

try {

    $db = Database::connect();

    echo "▶ Seeding warehouses...\n";

    $warehouses = [
        [1, 'Main Store', 'store'],
        [2, 'Gudang 1', 'warehouse'],
        [3, 'Gudang 2', 'warehouse'],
    ];

    $stmt = $db->prepare("
        INSERT INTO warehouses (id, name, type, is_active)
        VALUES (?, ?, ?, TRUE)
        ON CONFLICT (id) DO NOTHING
    ");

    foreach ($warehouses as $w) {
        $stmt->execute($w);
        echo "✅ {$w[1]}\n";
    }

    // fix sequence
    $db->exec("
        SELECT setval(
            'warehouses_id_seq',
            (SELECT COALESCE(MAX(id),0) FROM warehouses)
        )
    ");

    echo "✅ Warehouses seeded\n";

} catch (Exception $e) {
    echo "❌ Error seeding warehouses: " . $e->getMessage() . "\n";
}