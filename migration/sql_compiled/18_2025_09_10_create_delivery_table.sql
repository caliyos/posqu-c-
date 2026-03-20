
        CREATE TABLE IF NOT EXISTS delivery (
            dlv_id SERIAL PRIMARY KEY,
            transaction_id INT NOT NULL,
            amount NUMERIC(10,2) NOT NULL,
            dlv_by INT NOT NULL,
            created_by INT,
            updated_by INT,
            deleted_by INT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            deleted_at TIMESTAMP
        );
    
