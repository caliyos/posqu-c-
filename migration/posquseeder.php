<?php

require_once __DIR__ . '/Database.php';
use App\Config\Database;

try {
    $db = Database::connect();

    echo "🔄 Seeding database...\n";

    $db->exec("
        CREATE TABLE IF NOT EXISTS transaction_payments (
            id SERIAL PRIMARY KEY,
            ts_id INT NOT NULL REFERENCES transactions(ts_id) ON DELETE CASCADE,
            method VARCHAR(50) NOT NULL,
            amount NUMERIC(18,2) NOT NULL DEFAULT 0,
            note TEXT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
    ");
    echo "✅ Ensured table: transaction_payments\n";

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
        echo "✅ Inserted group: {$g['groupname']}\n";
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



 
// =====================================================
// 1️⃣ UNITS
// =====================================================
$units = [
    ['id' => 1, 'name' => 'Pieces', 'abbr' => 'pcs'],
    ['id' => 2, 'name' => 'Dus', 'abbr' => 'dus'],
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

// =====================================================
// 2️⃣ ITEM: Aqua Gelas 1 pcs
// =====================================================
$item = [
    'name' => 'Aqua Gelas 220ml',
    'buy_price' => 1000,
    'sell_price' => 1500,
    'barcode' => '899100000002',
    'stock' => 100,
    'reserved_stock' => 0,
    'unit' => 1,
    'category_id' => 1,
    'is_inventory_p' => 1,
    'is_purchasable' => 1,
    'is_sellable' => 1,
    'is_note_payment' => 0,
    'is_changeprice_p' => 0,
    'is_have_bahan' => 0,
    'is_box' => 0,
    'is_produksi' => 0,
    'note' => 'Air mineral gelas',
    'picture' => null,
    'supplier_id' => 1,
    'flag' => 1,
];


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
    RETURNING id
");

$item_id = $stmt->execute($item) ? $stmt->fetchColumn() : null;

if ($item_id) {
    echo "✅ Inserted item: {$item['name']} (ID: $item_id)\n";
} else {
    die("❌ Failed inserting item.\n");
}

// =====================================================
// 3️⃣ UNIT VARIANTS
// =====================================================
// PCS = base unit, DUS = 1 dus = 48 pcs
$variants = [
    // item_id, unit_id, conversion, sell_price, profit, minqty, is_base_unit, barcode_suffix
    // [$item_id, 1, 1, 1500, 500, 1, 1, '01'],   // PCS
    [$item_id, 2, 48, 70000, 22000, 1, 0, '02'], // DUS 48 pcs
];

$stmt = $db->prepare("
    INSERT INTO unit_variants (item_id, unit_id, conversion, sell_price, profit, minqty, is_base_unit, barcode_suffix)
    VALUES (:item_id, :unit_id, :conversion, :sell_price, :profit, :minqty, :is_base_unit, :barcode_suffix)
");

foreach ($variants as $v) {
    $stmt->execute([
        ':item_id' => $v[0],
        ':unit_id' => $v[1],
        ':conversion' => $v[2],
        ':sell_price' => $v[3],
        ':profit' => $v[4],
        ':minqty' => $v[5],
        ':is_base_unit' => $v[6],
        ':barcode_suffix' => $v[7],
    ]);
}

echo "✅ Inserted all unit_variants for Aqua Gelas.\n";


 
// =====================================================
// 3️⃣ MULTI-HARGA (tiered pricing)
// =====================================================
$item_prices = [
    // ['min_qty' => 1,  'price' => 1500],   // 1 PCS
    ['min_qty' => 10, 'price' => 1400],   // beli 10 PCS → diskon
    ['min_qty' => 48, 'price' => 70000],  // 1 DUS
    ['min_qty' => 96, 'price' => 138000], // 2 DUS
];

$stmt = $db->prepare("
    INSERT INTO item_prices (item_id, unit_id,min_qty, price, created_at)
    VALUES (:item_id,:unit_id, :min_qty, :price, NOW())
");

foreach ($item_prices as $p) {
    $stmt->execute([
        ':item_id' => $item_id,
        ':unit_id' => 1, 
        ':min_qty' => $p['min_qty'],
        ':price' => $p['price'],
    ]);
}
echo "✅ Inserted multi-prices for Aqua Gelas.\n";


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

    // Jalankan semua seeder di folder 'seeders' SETELAH core data (roles/users/etc)
    $seedersDir = __DIR__ . '/seeders';
    if (is_dir($seedersDir)) {
        $files = glob($seedersDir . '/*.php');
        sort($files);
        foreach ($files as $file) {
            echo "▶ Menjalankan seeder: " . basename($file) . "\n";
            require $file;
        }
    } else {
        echo "ℹ️ Folder seeders tidak ditemukan: $seedersDir\n";
    }











    echo "🎉 All seeding completed successfully!\n";

} catch (PDOException $e) {
    echo "❌ Seeding failed: " . $e->getMessage() . "\n";
}
