<?php
return function($db) {
    // Tabel Stock Log (untuk transaksi normal: payment / purchase / return)
    $db->exec("
        CREATE TABLE IF NOT EXISTS stock_log (
            id SERIAL PRIMARY KEY,
            product_id INT NOT NULL,                           -- ID produk
            tipe_transaksi VARCHAR(20) NOT NULL CHECK (tipe_transaksi IN ('purchase','payment','installment')),
            qty_masuk INT DEFAULT 0,                           -- Jumlah bertambah (misal purchase)
            qty_keluar INT DEFAULT 0,                          -- Jumlah berkurang (misal payment)
            sisa_stock INT NOT NULL,                           -- Saldo stok setelah transaksi
            keterangan VARCHAR(100) DEFAULT NULL,             -- Catatan, misal 'Penjualan kasir #123'
            user_id INT NOT NULL,                              -- ID user / kasir / sistem
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- Waktu transaksi
            login_id INT DEFAULT NULL                          -- relasi ke login_logs.id (opsional)
        );
    ");
};
