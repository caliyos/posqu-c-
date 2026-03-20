
        CREATE TABLE IF NOT EXISTS stock_adjustments_logs (
            id SERIAL PRIMARY KEY,
            product_id INT NOT NULL,
            stok_sistem INT NOT NULL,                -- Stok sebelum adjustment
            metode_adjustment VARCHAR(20) NOT NULL CHECK (metode_adjustment IN ('selisih_fisik','qty_manual')),
            stok_fisik INT DEFAULT NULL,             -- Jika metode selisih fisik
            selisih INT NOT NULL,                    -- Perubahan stok (+/-)
            alasan VARCHAR(100) NOT NULL,            -- Rusak / Hilang / Expired / Sample dll
            catatan TEXT DEFAULT NULL,               -- Optional note
            adjusted_by INT NOT NULL,                -- user / kasir / admin
            adjusted_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            login_id INT NOT NULL                 --  relasi ke login_logs.id
        );
    
