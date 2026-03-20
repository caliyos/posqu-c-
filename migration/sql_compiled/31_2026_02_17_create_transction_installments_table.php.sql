
       
   -- =========================
        -- TABLE CICILAN PEMBAYARAN
        -- =========================
        CREATE TABLE IF NOT EXISTS transaction_installments (
            id SERIAL PRIMARY KEY,
            transaction_id INT NOT NULL,
            amount NUMERIC(18,2) NOT NULL,
            note TEXT,
            created_by INT,
            updated_by INT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP NULL,

            CONSTRAINT fk_installment_transaction
                FOREIGN KEY (transaction_id)
                REFERENCES transactions(ts_id)
                ON DELETE CASCADE
        );

    
