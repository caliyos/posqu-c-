<?php
return function($db) {
    $db->exec("
       CREATE TABLE order_details (
    order_detail_id SERIAL PRIMARY KEY,
    order_id INT NOT NULL REFERENCES orders(order_id) ON DELETE CASCADE,
    item_id INT NOT NULL,
    
    od_barcode TEXT,                   -- Barcode barang atau variant (optional)
    od_quantity NUMERIC(18,2) NOT NULL,   -- Qty sesuai unit yang dipilih user (misal 1 dus)
    od_unit TEXT NOT NULL,             -- Unit yang dipilih (dus, pack, pcs)
    
    od_price_per_unit NUMERIC(18,2) DEFAULT 0,   -- Harga per unit sesuai unit yang dipilih
    
    od_unit_variant TEXT,             -- Nama unit variant (misal dus)
    od_conversion_rate NUMERIC(12,4) DEFAULT 1, -- Konversi ke unit dasar (misal 1 dus = 10 pcs)
    
    od_sell_price NUMERIC(18,2) DEFAULT 0,  -- Harga jual total sebelum diskon (od_price_per_unit * od_quantity)
    od_total NUMERIC(20,2) DEFAULT 0,       -- Total akhir setelah diskon & pajak (bisa kosong jika belum dihitung)
    
    od_note TEXT,                      -- Catatan khusus per item
    
    od_discount_per_item NUMERIC(15,2) DEFAULT 0,
    od_discount_percentage NUMERIC(5,2) DEFAULT 0,
    od_discount_total NUMERIC(18,2) DEFAULT 0,
    od_tax NUMERIC(18,2) DEFAULT 0,
    
    created_by INT,
    updated_by INT,
    deleted_by INT,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP NULL
);
    ");
};
