<?php

require_once __DIR__ . '/Database.php';
use App\Config\Database;

try {
    $db = Database::connect();

    echo "🔄 Seeding database...\n";

    // =====================================================
    // 1️⃣ ROLES
    // =====================================================
    $roles = [
        ['id' => 1, 'name' => 'admin', 'description' => 'Full access'],
        ['id' => 2, 'name' => 'cashier', 'description' => 'Can handle transactions only'],
        ['id' => 3, 'name' => 'supervisor', 'description' => 'Monitor and limited settings'],
    ];

    $stmt = $db->prepare("
        INSERT INTO roles (id, name, description)
        VALUES (:id, :name, :description)
        ON CONFLICT (id) DO NOTHING
    ");
    foreach ($roles as $r) {
        $stmt->execute($r);
        echo "✅ Inserted role: {$r['name']}\n";
    }

    // =====================================================
    // 2️⃣ UNITS
    // =====================================================
    $units = [
        ['id' => 1, 'name' => 'Pieces', 'abbr' => 'pcs'],
        ['id' => 2, 'name' => 'Dus', 'abbr' => 'dus'],
        ['id' => 3, 'name' => 'Kilogram', 'abbr' => 'kg'],
        ['id' => 4, 'name' => 'Renteng', 'abbr' => 'rtg'],
        ['id' => 5, 'name' => 'Liter', 'abbr' => 'ltr'],
    ];

    $stmt = $db->prepare("
        INSERT INTO units (id,name, abbr)
        VALUES (:id, :name, :abbr)
        ON CONFLICT (id) DO NOTHING
    ");
    foreach ($units as $u) {
        $stmt->execute($u);
        echo "✅ Inserted unit: {$u['name']}\n";
    }

            // =========================
    // AUTO SYNC SEQUENCE (🔥 INI INTINYA)
    // =========================
    $db->exec("
        SELECT setval(
            'units_id_seq',
            (SELECT COALESCE(MAX(id), 0) FROM units) + 1,
            false
        );
    ");


      // =====================================================
    // 1️⃣ Customers
    // =====================================================
    $customers = [
        ['id' => 1, 'name' => 'John Doe', 'phone' => '081234567890', 'note' => 'Regular customer', 'created_by' => 1],
        ['id' => 2, 'name' => 'Jane Smith', 'phone' => '082345678901', 'note' => 'VIP customer', 'created_by' => 1],
        ['id' => 4, 'name' => 'Ahmad Santoso', 'phone' => '081987654321', 'note' => 'New customer', 'created_by' => 1],
    ];

    $stmt = $db->prepare("
        INSERT INTO customers (id, name, phone, note, created_by)
        VALUES (:id, :name, :phone, :note, :created_by)
        ON CONFLICT (id) DO NOTHING
    ");

    foreach ($customers as $c) {
        $stmt->execute($c);
    }

        $db->exec("
        SELECT setval(
            'customers_id_seq',
            (SELECT COALESCE(MAX(id), 0) FROM customers) + 1,
            false
        );
    ");


    // =====================================================
    // 3️⃣ GROUPS
    // =====================================================
    $groups = [
        ['id' => 1, 'groupname' => 'group1', 'groupshortname' => 'grp1'],
        ['id' => 2, 'groupname' => 'group2', 'groupshortname' => 'grp2'],
    ];

    $stmt = $db->prepare("
        INSERT INTO groups (id, groupname, groupshortname, created_at, updated_at)
        VALUES (:id, :groupname, :groupshortname, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
        ON CONFLICT (id) DO NOTHING
    ");
    foreach ($groups as $g) {
        $stmt->execute($g);
        echo "✅ Inserted group: {$g['name']}\n";
    }

    // =====================================================
    // 4️⃣ SETTING TOKO
    // =====================================================
    $settingToko = [
        'id' => 1,
        'nama' => 'Toko Yoskal',
        'alamat' => 'Jl. Sario Kecamatan Beo .Kelurahan Beo Barat Kab Talaud.',
        'npwp' => '01.234.567.8-901.000',
    ];

    $stmt = $db->prepare("
        INSERT INTO settingtoko (id, nama, alamat, npwp)
        VALUES (:id, :nama, :alamat, :npwp)
        ON CONFLICT (id) DO NOTHING
    ");
    $stmt->execute($settingToko);
    echo "✅ Inserted settingtoko: {$settingToko['nama']}\n";

    // =====================================================
// 5️⃣ STRUK SETTING
// =====================================================
$struk = [
    'id' => 1,
    'judul' => 'Toko Yoskal',
    'alamat' => 'Jl. Sario Kecamatan Beo .Kelurahan Beo Barat Kab Talaud.',
    'telepon' => '082292613349',
    'footer' => 'Terima kasih atas kunjungan Anda!',
    'logo' => null,
    'is_visible_nama_toko' => true,
    'is_visible_alamat' => true,
    'is_visible_telepon' => true,
    'is_visible_footer' => true,
    'is_visible_logo' => true,
];

$stmt = $db->prepare("
    INSERT INTO struk_setting
    (id, judul, alamat, telepon, footer, logo,
     is_visible_nama_toko, is_visible_alamat, is_visible_telepon,
     is_visible_footer, is_visible_logo, updated_at)
    VALUES
    (:id, :judul, :alamat, :telepon, :footer, :logo,
     :is_visible_nama_toko, :is_visible_alamat, :is_visible_telepon,
     :is_visible_footer, :is_visible_logo, NOW())
    ON CONFLICT (id) DO NOTHING
");
$stmt->execute($struk);

echo "✅ Inserted struk_setting: {$struk['judul']}\n";



 // Data kategori default
    $categories = [
        [
            'id' => 1,
            'name' => 'Bahan Pokok',
            'kode' => 'BP001',
            'description' => 'Kategori untuk semua bahan pokok',
            'parent_id' => null,
        ],
        [
            'id' => 2,
            'name' => 'Minuman',
            'kode' => 'MN001',
            'description' => 'Kategori untuk semua jenis minuman',
            'parent_id' => null,
        ],
        [
            'id' => 3,
            'name' => 'Snack',
            'kode' => 'SN001',
            'description' => 'Kategori untuk cemilan dan snack',
            'parent_id' => null,
        ],
        [
            'id' => 4,
            'name' => 'Makanan Ringan',
            'kode' => 'MR001',
            'description' => 'Kategori untuk makanan ringan lainnya',
            'parent_id' => 3, // Subkategori dari Snack
        ],
    ];

    // Prepare query insert
    $stmt = $db->prepare("
        INSERT INTO categories (id, name, kode, description, parent_id)
        VALUES (:id, :name, :kode, :description, :parent_id)
        ON CONFLICT (id) DO NOTHING
    ");

    // Loop insert
    foreach ($categories as $category) {
        $stmt->execute($category);
        echo "✅ Inserted category: {$category['name']}\n";
    }

// =====================================================
// 6️⃣ SUPPLIERS
// =====================================================
$suppliers = [
    [
        'id' => 1,
        'name' => 'PT Sumber Makmur',
        'contact_name' => 'Budi Santoso',
        'phone' => '081234567890',
        'email' => 'budi@sumbermakmur.com',
        'address' => 'Jl. Merdeka No.10, Jakarta',
        'note' => 'Supplier bahan pokok',
    ],
    [
        'id' => 2,
        'name' => 'CV Anugerah Jaya',
        'contact_name' => 'Siti Aminah',
        'phone' => '081298765432',
        'email' => 'siti@anugerahjaya.com',
        'address' => 'Jl. Diponegoro No.25, Bandung',
        'note' => 'Supplier minuman',
    ],
    [
        'id' => 3,
        'name' => 'UD Sejahtera',
        'contact_name' => 'Agus Salim',
        'phone' => '085678912345',
        'email' => 'agus@sejahtera.com',
        'address' => 'Jl. Sudirman No.5, Surabaya',
        'note' => 'Supplier snack',
    ],
];

$stmt = $db->prepare("
    INSERT INTO suppliers (id, name, contact_name, phone, email, address, note)
    VALUES (:id, :name, :contact_name, :phone, :email, :address, :note)
    ON CONFLICT (id) DO NOTHING
");

foreach ($suppliers as $supplier) {
    $stmt->execute($supplier);
    echo "✅ Inserted supplier: {$supplier['name']}\n";
}










///////////////////////////////////ITEMS////////////////////////////////////////////////

// =====================================================
// 7️⃣ ITEMS & UNIT VARIANTS
// =====================================================
try {
    echo "🧹 Clearing related tables...\n";

    $db->exec("
        TRUNCATE TABLE unit_variants RESTART IDENTITY CASCADE;
        TRUNCATE TABLE items RESTART IDENTITY CASCADE;
        TRUNCATE TABLE pending_transactions RESTART IDENTITY CASCADE;
        TRUNCATE TABLE transactions RESTART IDENTITY CASCADE;
        TRUNCATE TABLE transaction_details RESTART IDENTITY CASCADE;
        TRUNCATE TABLE orders RESTART IDENTITY CASCADE;
        TRUNCATE TABLE order_details RESTART IDENTITY CASCADE;
        ALTER SEQUENCE items_id_seq RESTART WITH 1;
    ");

    echo "✅ Tables truncated & sequences reset.\n";

    // ================================
    // Insert ITEMS
    // ================================
 // ================================
    // Insert ITEMS - SEMBAKO
    // ================================
$items = [
    ['Beras Pandan Wangi 5kg', 65000, 72000, '899100000001', 50, 0, 1, 1, true, true, 'Beras', 'Beras premium wangi lembut', 1, 1],
    // ['Beras Ramos 5kg', 60000, 68000, '899100000002', 40, 0, 2, 1, true, true, 'Beras', 'Beras pulen kualitas medium', 1, 1],
    // ['Minyak Goreng Bimoli 1L', 18000, 21000, '899100000003', 100, 0, 1, 1, true, true, 'Minyak Sawit', 'Minyak goreng kemasan botol', 1, 1],
    // ['Minyak Goreng Tropical 2L', 33000, 38000, '899100000004', 80, 0, 1, 1, true, true, 'Minyak Sawit', 'Minyak goreng isi 2 liter', 1, 1],
    // ['Gula Pasir Gulaku 1kg', 15500, 18000, '899100000005', 120, 0, 2, 1, true, true, 'Gula Tebu', 'Gula pasir kemasan 1kg', 1, 1],
    // ['Gula Pasir Rose Brand 1kg', 15000, 17500, '899100000006', 100, 0, 2, 1, true, true, 'Gula Tebu', 'Gula pasir halus', 1, 1],
    // ['Garam Dapur Cap Kapal 500g', 2500, 4000, '899100000007', 200, 0, 1, 1, true, true, 'Garam', 'Garam beryodium halus', 1, 1],
    // ['Tepung Terigu Segitiga Biru 1kg', 11000, 13500, '899100000008', 150, 0, 1, 1, true, true, 'Tepung Gandum', 'Tepung serbaguna berkualitas', 1, 1]
    // ['Tepung Beras Rose Brand 1kg', 10000, 13000, '899100000009', 120, 0, 5, 1, true, true, 'Tepung Beras', 'Tepung beras halus', 1, 1],
    // ['Telur Ayam Negeri 1kg', 26000, 30000, '899100000010', 50, 0, 5, 1, true, true, 'Telur Ayam', 'Telur ayam segar curah', 1, 1],
    // ['Telur Kampung 1kg', 34000, 38000, '899100000011', 40, 0, 5, 1, true, true, 'Telur Ayam', 'Telur kampung alami', 1, 1],
    // ['Mie Instan Indomie Goreng', 2800, 3500, '899100000012', 500, 0, 5, 1, true, true, 'Mie Instan', 'Indomie goreng klasik', 1, 1],
    // ['Mie Instan Soto Ayam', 2800, 3500, '899100000013', 400, 0, 5, 1, true, true, 'Mie Instan', 'Rasa soto ayam gurih', 1, 1],
    // ['Mie Instan Kari Ayam', 2800, 3500, '899100000014', 400, 0, 5, 1, true, true, 'Mie Instan', 'Rasa kari pedas', 1, 1],
    // ['Kecap Manis ABC 520ml', 16500, 19000, '899100000015', 60, 0, 5, 1, true, true, 'Kedelai', 'Kecap manis kental', 1, 1],
    // ['Kecap Manis Bango 220ml', 12000, 14500, '899100000016', 80, 0, 5, 1, true, true, 'Kedelai', 'Kecap manis asli', 1, 1],
    // ['Saus Sambal ABC 335ml', 9500, 12000, '899100000017', 100, 0, 5, 1, true, true, 'Cabai', 'Saus sambal pedas mantap', 1, 1],
    // ['Saus Tomat ABC 335ml', 9000, 11500, '899100000018', 100, 0, 5, 1, true, true, 'Tomat', 'Saus tomat kental', 1, 1],
    // ['Kopi Kapal Api 65g', 7000, 9000, '899100000019', 150, 0, 5, 1, true, true, 'Kopi Robusta', 'Kopi hitam legendaris', 1, 1],
    // ['Kopi Good Day Cappuccino', 1500, 2500, '899100000020', 300, 0, 5, 1, true, true, 'Kopi Instan', 'Minuman kopi instan', 1, 1],
    // ['Teh Celup Sosro 25pcs', 9500, 12000, '899100000021', 80, 0, 5, 1, true, true, 'Teh Hitam', 'Teh celup berkualitas', 1, 1],
    // ['Teh Sariwangi 25pcs', 9500, 12000, '899100000022', 70, 0, 5, 1, true, true, 'Teh Hitam', 'Teh celup harum', 1, 1],
    // ['Air Mineral Aqua 600ml', 2500, 3500, '899100000023', 300, 0, 5, 1, true, true, 'Air Murni', 'Air minum dalam botol', 1, 1],
    // ['Air Mineral Le Minerale 600ml', 2500, 3500, '899100000024', 250, 0, 5, 1, true, true, 'Air Murni', 'Air mineral sehat', 1, 1],
    // ['Sabun Mandi Lifebuoy 110g', 4000, 5500, '899100000025', 200, 0, 5, 1, true, true, 'Sabun Padat', 'Sabun antiseptik', 1, 1],
    // ['Shampoo Sunsilk 170ml', 12000, 15000, '899100000026', 80, 0, 5, 1, true, true, 'Shampoo', 'Untuk rambut lembut berkilau', 1, 1],
    // ['Pasta Gigi Pepsodent 190g', 11000, 13500, '899100000027', 100, 0, 5, 1, true, true, 'Pasta Gigi', 'Mencegah gigi berlubang', 1, 1],
    // ['Detergen Rinso 1kg', 15000, 18500, '899100000028', 90, 0, 5, 1, true, true, 'Detergen', 'Bersih dan wangi', 1, 1],
    // ['Sabun Cuci Sunlight 755ml', 13500, 16500, '899100000029', 100, 0, 5, 1, true, true, 'Cairan Pembersih', 'Untuk cuci piring efektif', 1, 1],
    // ['Tisu Paseo 250 Lembar', 9500, 12000, '899100000030', 120, 0, 5, 1, true, true, 'Tisu', 'Tisu lembut dan tebal', 1, 1],
    // ['Susu Kental Manis Frisian Flag 370g', 9500, 11500, '899100000031', 80, 0, 5, 1, true, true, 'Susu', 'SKM untuk kopi atau roti', 1, 1],
    // ['Susu Bubuk Dancow 400g', 45000, 52000, '899100000032', 50, 0, 5, 1, true, true, 'Susu Bubuk', 'Susu pertumbuhan anak', 1, 1],
    // ['Susu UHT Ultra Milk 250ml', 4000, 5500, '899100000033', 200, 0, 5, 1, true, true, 'Susu UHT', 'Susu cair siap minum', 1, 1],
    // ['Rokok Djarum Super 12', 25000, 30000, '899100000034', 100, 0, 5, 1, true, true, 'Tembakau', 'Rokok kretek lokal', 1, 1],
    // ['Rokok Sampoerna Mild 16', 33000, 37000, '899100000035', 80, 0, 5, 1, true, true, 'Tembakau', 'Rokok putih lembut', 1, 1],
    // ['Kopi Kapucino ABC Sachet', 1500, 2500, '899100000036', 300, 0, 5, 1, true, true, 'Kopi Instan', 'Kopi cappuccino sachet', 1, 1],
    // ['Bumbu Masako Ayam 250g', 8500, 11000, '899100000037', 100, 0, 5, 1, true, true, 'Penyedap', 'Penyedap rasa ayam', 1, 1],
    // ['Bumbu Royco Sapi 250g', 8500, 11000, '899100000038', 100, 0, 5, 1, true, true, 'Penyedap', 'Penyedap rasa sapi', 1, 1],
    // ['Cabe Kering 250g', 18000, 22000, '899100000039', 70, 0, 5, 1, true, true, 'Cabe Kering', 'Cabe kering merah', 1, 1],
    // ['Bawang Merah 1kg', 28000, 35000, '899100000040', 50, 0, 5, 1, true, true, 'Umbi', 'Bawang merah segar', 1, 1],
    // ['Bawang Putih 1kg', 25000, 32000, '899100000041', 50, 0, 5, 1, true, true, 'Umbi', 'Bawang putih impor', 1, 1],
    // ['Kentang 1kg', 15000, 20000, '899100000042', 60, 0, 5, 1, true, true, 'Umbi', 'Kentang segar lokal', 1, 1],
    // ['Wortel 1kg', 10000, 15000, '899100000043', 60, 0, 5, 1, true, true, 'Sayur', 'Wortel segar organik', 1, 1],
    // ['Kangkung Ikat', 3000, 5000, '899100000044', 100, 0, 5, 1, true, true, 'Sayur', 'Kangkung segar pasar', 1, 1],
    // ['Bayam Ikat', 3000, 5000, '899100000045', 100, 0, 5, 1, true, true, 'Sayur', 'Bayam hijau segar', 1, 1],
    // ['Tahu 10pcs', 5000, 7000, '899100000046', 80, 0, 5, 1, true, true, 'Kedelai', 'Tahu goreng siap masak', 1, 1],
    // ['Tempe 1 Papan', 4000, 6000, '899100000047', 80, 0, 5, 1, true, true, 'Kedelai', 'Tempe fermentasi segar', 1, 1],
    // ['Ikan Teri Kering 250g', 22000, 27000, '899100000048', 40, 0, 5, 1, true, true, 'Ikan', 'Teri asin kering', 1, 1],
    // ['Ikan Asin 250g', 18000, 23000, '899100000049', 40, 0, 5, 1, true, true, 'Ikan', 'Ikan asin laut', 1, 1],
    // ['Sarden ABC 155g', 9500, 12000, '899100000050', 100, 0, 5, 1, true, true, 'Ikan Kaleng', 'Sarden saus tomat', 1, 1],
    // Tambahkan hingga 100 (dengan pola serupa)
];


// $items = [
//   [
//     'name' => 'Aqua Botol',
//     'buy_price' => 1500,
//     'sell_price' => 3000,
//     'barcode' => '1234567890',
//     'stock' => 100,
//     'unit' => 1,

//     'is_inventory_p' => 1,
//     'is_purchasable' => 1,
//     'is_sellable' => 1,
//     'is_note_payment' => 0,
//     'is_changeprice_p' => 0,
//     'is_have_bahan' => 0,
//     'is_box' => 0,
//     'is_produksi' => 0,

//     'note' => null,
//     'picture' => null,
// ]

// ];


$stmt = $db->prepare("
    INSERT INTO items
    (
        name, buy_price, sell_price, barcode, stock, reserved_stock, unit, category_id,
        is_inventory_p, is_purchasable, is_sellable, is_note_payment,
        is_changeprice_p, is_have_bahan, is_box, is_produksi,
        note, picture, created_at, updated_at, supplier_id, flag
    )
    VALUES
    (
        :name, :buy_price, :sell_price, :barcode, :stock, :reserved_stock, :unit, :category_id,
        :is_inventory_p, :is_purchasable, :is_sellable, :is_note_payment,
        :is_changeprice_p, :is_have_bahan, :is_box, :is_produksi,
        :note, :picture, NOW(), NOW(), :supplier_id, :flag
    )
");


function b($v) {
    return $v ? 1 : 0;
}

foreach ($items as $item) {
$data = [
        'name' => $item[0],
        'buy_price' => $item[1],
        'sell_price' => $item[2],
        'barcode' => $item[3],
        'stock' => $item[4],
        'unit' => $item[6],
        'is_inventory_p' => $item[8],
        'is_purchasable' => $item[9],
        'is_sellable' => true,
        'is_note_payment' => false,
        'is_changeprice_p' => false,
        'is_have_bahan' => false,
        'is_box' => false,
        'is_produksi' => false,
    ];
    // echo '<pre>';
    // print_r($data);
    // echo '</pre>';
    // exit();
    
  $stmt->execute([
    ':name' => $data['name'],
    ':buy_price' => $data['buy_price'],
    ':sell_price' => $data['sell_price'],
    ':barcode' => $data['barcode'],
    ':stock' => $data['stock'],
    ':reserved_stock' => 0,
    ':unit' => $data['unit'],
    ':category_id' => 1,

    ':is_inventory_p' => b($data['is_inventory_p']),
    ':is_purchasable' => b($data['is_purchasable']),
    ':is_sellable' => b($data['is_sellable']),
    ':is_note_payment' => b($data['is_note_payment']),
    ':is_changeprice_p' => b($data['is_changeprice_p']),
    ':is_have_bahan' => b($data['is_have_bahan']),
    ':is_box' => b($data['is_box']),
    ':is_produksi' => b($data['is_produksi']),

    ':note' => "xx",
    ':picture' => "sad",
    ':supplier_id' => 1,
    ':flag' => 1,
]);


    echo "✅ Inserted item: {$data['name']}\n";
}


    // // ================================
    // // Insert UNIT VARIANTS
    // // ================================
    // $variants = [
    //     [1, 2, 10, 39000000,1,1, 5, '01'],
    //     // [2, 2, 5, 85000000,1,1, 0, '02'],
    //     // [3, 4, 1, 5000000,1,1,1, '03'],
    //     // [4, 4, 1, 4500000,1,1,1, '04'],
    //     // [5, 2, 1, 6500000,1,1,1, '05'],
    //     // [6, 2, 5, 40000000,1,1, 0, '06'],
    //     // [7, 3, 500, 700000,1,1,1, '07'],
    //     // [8, 3, 300, 2000000,1,1,1, '08'],
    //     // [9, 2, 2, 1400000,1,1,1, '09'],
    //     // [10, 5, 1, 800000,1,1,1, '10'],
    //     // [11, 2, 1, 1200000,1,1,1, '11'],
    //     // [12, 3, 2, 1800000,1,1,1, '12'],
    //     // [13, 3, 3, 1500000,1,1,1, '13'],
    //     // [14, 3, 100, 200000,1,1,1, '14'],
    //     // [15, 4, 100, 250000,1,1,1, '15'],
    //     // [16, 3, 500, 15000,1,1,1, '16'],
    //     // [17, 5, 50, 35000,1,1,1, '17'],
    //     // [18, 4, 1, 150000,1,1,1, '18'],
    //     // [19, 5, 1, 70000,1,1,1, '19'],
    //     // [20, 5, 5, 70000,1,1,1, '20'],
    // ];

    // $stmt = $db->prepare("
    //     INSERT INTO unit_variants (item_id, unit_id, conversion, sell_price,profit,minqty ,is_base_unit, barcode_suffix)
    //     VALUES (:item_id, :unit_id, :conversion, :sell_price,:profit,:minqty, :is_base_unit, :barcode_suffix)
    // ");

    // foreach ($variants as $v) {
    //     $stmt->execute([
    //         ':item_id' => $v[0],
    //         ':unit_id' => $v[1],
    //         ':conversion' => $v[2],
    //         ':sell_price' => $v[3],
    //         ':profit' => $v[4],
    //         ':minqty' => $v[5],
    //         ':is_base_unit' => $v[6],
    //         ':barcode_suffix' => $v[7],
    //     ]);
    // }
// 
    // echo "✅ Inserted all unit_variants.\n";

} catch (PDOException $e) {
    echo "❌ Item/variant seeding failed: " . $e->getMessage() . "\n";
}


///////////////////////////////////END ITEMS////////////////////////////////////////////////








$db->exec("TRUNCATE TABLE terminals, users, permissions, roles RESTART IDENTITY CASCADE;");

    // ================================
    // Insert ROLES
    // ================================
    $roles = [
        [1, 'admin', 'Full access'],
        [2, 'cashier', 'Can handle transactions only'],
        [3, 'supervisor', 'Monitor and limited settings'],
    ];

    $stmt = $db->prepare("INSERT INTO roles (id, name, description) VALUES (?, ?, ?)");
    foreach ($roles as $r) {
        $stmt->execute($r);
        echo "✅ Inserted role: {$r[1]}\n";
    }

    // ================================
    // Insert PERMISSIONS
    // ================================
    $permissions = [
        [1, 'manage_users', 'Manage users and roles'],
        [2, 'manage_products', 'Manage products'],
        [3, 'manage_purchases', 'Manage product purchases'],
        [4, 'view_reports', 'View reports and analytics'],
        [5, 'access_cashier', 'Access cashier menu'],
        [6, 'access_backoffice', 'Access backoffice menu'],
        [7, 'monitor_system', 'Monitor system status'],
    ];

    $stmt = $db->prepare("INSERT INTO permissions (id, name, description) VALUES (?, ?, ?)");
    foreach ($permissions as $p) {
        $stmt->execute($p);
        echo "✅ Inserted permission: {$p[1]}\n";
    }

    // ================================
    // Insert USERS
    // ================================
    // Passwords are bcrypt hashes
    $users = [
        [1, 'yoz','admin', '$2a$10$QMO96WOaMPGZ075dFKDOIeTX8323MGhe9QfS2bduTMjFa15l7RRQW'],
        [2, 'kaleb','kasir', '$2a$10$iW5cGwZIKQICFZ101H.zzezBWomfymDc7wIE6ASGFNTMehYlkq.I.'],
        [3, 'indah', 'supervisor', '$2a$10$SJgIDsAM3.SCFVLN6btWNeecqfm6as/KiSHB.m.rDI.0KQgdM/h12'],
    ];

    $stmt = $db->prepare("INSERT INTO users (id, name, username, password_hash, created_at) VALUES (?,?, ?, ?, NOW())");
    foreach ($users as $u) {
        $stmt->execute($u);
        echo "✅ Inserted user: {$u[1]}\n";
    }

    // ================================
    // Insert TERMINALS
    // ================================
    $terminals = [
        ['T1', '0A0027000018', 'Terminal Utama Kasir 1'],
        ['T2', 'PC-F6G7H8I9J0', 'Terminal Kasir 2'],
        ['T3', 'PC-K1L2M3N4O5', 'Terminal Supervisor'],
    ];

    $stmt = $db->prepare("INSERT INTO terminals (terminal_name, pc_id, description) VALUES (?, ?, ?)");
    foreach ($terminals as $t) {
        $stmt->execute($t);
        echo "✅ Inserted terminal: {$t[0]}\n";
    }




    // ================================
// Insert USER_TERMINAL
// ================================
$user_terminals = [
    [1, 3], // yoz → Terminal Supervisor (T3)
    [2, 1], // kaleb → Terminal Utama Kasir 1 (T1)
    [3, 2], // indah → Terminal Kasir 2 (T2)
];

$stmt = $db->prepare("INSERT INTO user_terminal (user_id, terminal_id) VALUES (?, ?)");
foreach ($user_terminals as $ut) {
    $stmt->execute($ut);
    echo "✅ Linked user_id {$ut[0]} to terminal_id {$ut[1]}\n";
}


    // ================================
    // Insert role user
    // ================================
    $roleuser = [
        [1, 1],      
        [2, 2],      
        [3, 3],      
    ];

    $stmt = $db->prepare("INSERT INTO role_user (user_id,role_id) VALUES (?, ?)");
    foreach ($roleuser as $p) {
        $stmt->execute($p);
        echo "✅ Inserted role user: {$p[1]}\n";
    }


    // ================================
// Insert SHIFTS
// ================================
$shifts = [
    ['Shift 1', '07:00', '15:00'],
    ['Shift 2', '15:00', '23:00'],
    ['Shift 3', '23:00', '07:00'],
];

$stmt = $db->prepare("INSERT INTO shifts (name, start_time, end_time) VALUES (?, ?, ?)");

foreach ($shifts as $s) {
    $stmt->execute($s);
    echo "✅ Inserted shift: {$s[0]}\n";
}












    echo "🎉 All seeding completed successfully!\n";

} catch (PDOException $e) {
    echo "❌ Seeding failed: " . $e->getMessage() . "\n";
}
