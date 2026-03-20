
        CREATE TABLE IF NOT EXISTS shifts (
            shift_id SERIAL PRIMARY KEY,

            user_id INT NOT NULL,

            opened_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
            closed_at TIMESTAMP,

            opening_cash NUMERIC(12,2) NOT NULL DEFAULT 0,
            expected_cash NUMERIC(12,2) NOT NULL DEFAULT 0,
            closing_cash NUMERIC(12,2),
            difference_cash NUMERIC(12,2),

            status VARCHAR(20) NOT NULL DEFAULT 'open',
            note TEXT,  

            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
    
