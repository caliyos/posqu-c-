<?php
require_once __DIR__ . '/../Database.php';
use App\Config\Database;

try {
    $db = Database::connect();
    echo "Seeding suppliers, units, items, and unit variants...\n";

    $suppliers = [
        ['id' => 11, 'name' => 'PT Nusantara Abadi', 'contact_name' => 'Rina', 'phone' => '081200000001', 'email' => 'rina@nusantara.id', 'address' => 'Jakarta', 'note' => 'Grocery'],
        ['id' => 12, 'name' => 'CV Maju Jaya',        'contact_name' => 'Andi', 'phone' => '081200000002', 'email' => 'andi@majujaya.id',  'address' => 'Bandung', 'note' => 'Beverage'],
        ['id' => 13, 'name' => 'UD Segar Makmur',     'contact_name' => 'Sari', 'phone' => '081200000003', 'email' => 'sari@segarmakmur.id','address' => 'Surabaya', 'note' => 'Snack'],
        ['id' => 14, 'name' => 'PT Berkah Sejati',    'contact_name' => 'Budi', 'phone' => '081200000004', 'email' => 'budi@berkahsejati.id','address' => 'Semarang', 'note' => 'Staples'],
        ['id' => 15, 'name' => 'CV Tirta Anugerah',   'contact_name' => 'Tuti', 'phone' => '081200000005', 'email' => 'tuti@tirta.id',       'address' => 'Bogor', 'note' => 'Water'],
    ];
    $stmt = $db->prepare("INSERT INTO suppliers (id, name, contact_name, phone, email, address, note) VALUES (:id, :name, :contact_name, :phone, :email, :address, :note) ON CONFLICT (id) DO NOTHING");
    foreach ($suppliers as $s) { $stmt->execute($s); echo "Inserted supplier: {$s['name']}\n"; }

    $units = [
        ['id' => 1, 'name' => 'Pieces', 'abbr' => 'pcs'],
        ['id' => 2, 'name' => 'Dus',    'abbr' => 'dus'],
        ['id' => 3, 'name' => 'Pack',   'abbr' => 'pack'],
        ['id' => 4, 'name' => 'Box',    'abbr' => 'box'],
        ['id' => 5, 'name' => 'Kg',     'abbr' => 'kg'],
    ];
    $stmt = $db->prepare("INSERT INTO units (id, name, abbr) VALUES (:id, :name, :abbr) ON CONFLICT (id) DO NOTHING");
    foreach ($units as $u) { $stmt->execute($u); echo "Inserted unit: {$u['name']}\n"; }

    $db->exec("SELECT setval('units_id_seq',(SELECT COALESCE(MAX(id),0) FROM units)+1,false);");

    $items = [
        ['name'=>'Aqua Botol 600ml','buy_price'=>2500,'sell_price'=>3500,'barcode'=>'899100000101','stock'=>200,'reserved_stock'=>0,'unit'=>1,'category_id'=>2,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Air mineral botol','picture'=>null,'supplier_id'=>15,'flag'=>1],
        ['name'=>'Teh Kotak 250ml','buy_price'=>3500,'sell_price'=>5000,'barcode'=>'899200000201','stock'=>150,'reserved_stock'=>0,'unit'=>1,'category_id'=>2,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Minuman teh','picture'=>null,'supplier_id'=>12,'flag'=>1],
        ['name'=>'Indomie Goreng','buy_price'=>2800,'sell_price'=>3500,'barcode'=>'899300000301','stock'=>500,'reserved_stock'=>0,'unit'=>1,'category_id'=>3,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Mi instan goreng','picture'=>null,'supplier_id'=>11,'flag'=>1],
        ['name'=>'Kopi Sachet','buy_price'=>1500,'sell_price'=>2500,'barcode'=>'899400000401','stock'=>300,'reserved_stock'=>0,'unit'=>1,'category_id'=>3,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Kopi sachet','picture'=>null,'supplier_id'=>13,'flag'=>1],
        ['name'=>'Gula Pasir 1kg','buy_price'=>12000,'sell_price'=>15000,'barcode'=>'899500000501','stock'=>100,'reserved_stock'=>0,'unit'=>5,'category_id'=>1,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Gula pasir','picture'=>null,'supplier_id'=>14,'flag'=>1],
        ['name'=>'Beras Ramos 5kg','buy_price'=>65000,'sell_price'=>80000,'barcode'=>'899600000601','stock'=>80,'reserved_stock'=>0,'unit'=>5,'category_id'=>1,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Beras ramos','picture'=>null,'supplier_id'=>14,'flag'=>1],
        ['name'=>'Wafer Coklat','buy_price'=>4000,'sell_price'=>6000,'barcode'=>'899700000701','stock'=>250,'reserved_stock'=>0,'unit'=>1,'category_id'=>3,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Wafer coklat','picture'=>null,'supplier_id'=>13,'flag'=>1],
        ['name'=>'Susu UHT 1L','buy_price'=>18000,'sell_price'=>23000,'barcode'=>'899800000801','stock'=>120,'reserved_stock'=>0,'unit'=>1,'category_id'=>2,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Susu UHT','picture'=>null,'supplier_id'=>12,'flag'=>1],
        ['name'=>'Minyak Goreng 1L','buy_price'=>15000,'sell_price'=>20000,'barcode'=>'899900000901','stock'=>180,'reserved_stock'=>0,'unit'=>1,'category_id'=>1,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Minyak goreng','picture'=>null,'supplier_id'=>11,'flag'=>1],
        ['name'=>'Sarden Kaleng','buy_price'=>9000,'sell_price'=>12000,'barcode'=>'899010001001','stock'=>140,'reserved_stock'=>0,'unit'=>1,'category_id'=>1,'is_inventory_p'=>1,'is_purchasable'=>1,'is_sellable'=>1,'is_note_payment'=>0,'is_changeprice_p'=>0,'is_have_bahan'=>0,'is_box'=>0,'is_produksi'=>0,'note'=>'Ikan sarden','picture'=>null,'supplier_id'=>11,'flag'=>1],
    ];
    $stmtItem = $db->prepare("INSERT INTO items (name, buy_price, sell_price, barcode, stock, reserved_stock, unit, category_id, is_inventory_p, is_purchasable, is_sellable, is_note_payment, is_changeprice_p, is_have_bahan, is_box, is_produksi, note, picture, created_at, updated_at, supplier_id, flag) VALUES (:name, :buy_price, :sell_price, :barcode, :stock, :reserved_stock, :unit, :category_id, :is_inventory_p, :is_purchasable, :is_sellable, :is_note_payment, :is_changeprice_p, :is_have_bahan, :is_box, :is_produksi, :note, :picture, NOW(), NOW(), :supplier_id, :flag) RETURNING id");
    $stmtVar = $db->prepare("INSERT INTO unit_variants (item_id, unit_id, conversion, sell_price, profit, minqty, is_base_unit, barcode_suffix) VALUES (:item_id, :unit_id, :conversion, :sell_price, :profit, :minqty, :is_base_unit, :barcode_suffix)");

    foreach ($items as $idx => $it) {
        $stmtItem->execute($it);
        $itemId = $stmtItem->fetchColumn();
        echo "Inserted item: {$it['name']} (ID: {$itemId})\n";
        $baseSell = $it['sell_price'];
        $baseProfit = max(0, $baseSell - $it['buy_price']);
        $stmtVar->execute([':item_id'=>$itemId, ':unit_id'=>$it['unit'], ':conversion'=>1, ':sell_price'=>$baseSell, ':profit'=>$baseProfit, ':minqty'=>1, ':is_base_unit'=>1, ':barcode_suffix'=>'01']);
        $packConv = 12;
        $packPrice = $baseSell * $packConv * 0.92;
        $stmtVar->execute([':item_id'=>$itemId, ':unit_id'=>2, ':conversion'=>$packConv, ':sell_price'=>$packPrice, ':profit'=>($packPrice - $it['buy_price']*$packConv), ':minqty'=>1, ':is_base_unit'=>0, ':barcode_suffix'=>'02']);
    }

    $db->exec("SELECT setval('items_id_seq',(SELECT COALESCE(MAX(id),0) FROM items)+1,false);");
    $db->exec("SELECT setval('unit_variants_id_seq',(SELECT COALESCE(MAX(id),0) FROM unit_variants)+1,false);");

    echo "Bulk seeding completed.\n";
} catch (PDOException $e) {
    echo "Seeding failed: " . $e->getMessage() . "\n";
}
