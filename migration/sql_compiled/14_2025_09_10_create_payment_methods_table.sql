
        CREATE TABLE IF NOT EXISTS payment_methods (
            pm_id SERIAL PRIMARY KEY,
            pm_name VARCHAR(100) NOT NULL,
            pm_number VARCHAR(50),
            pm_note TEXT
        );
    
