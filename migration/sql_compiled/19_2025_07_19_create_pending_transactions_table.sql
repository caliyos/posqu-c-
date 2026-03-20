
            CREATE TABLE IF NOT EXISTS pending_orders (
                po_id SERIAL PRIMARY KEY,
                terminal_id INT NOT NULL,
                cashier_id INT NOT NULL,
                customer_name VARCHAR(100),
                note TEXT,
                total NUMERIC(18,2) DEFAULT 0,
                global_discount NUMERIC(5,2) DEFAULT 0,
                status VARCHAR(20) DEFAULT 'draft', -- draft, cancelled, paid
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                po_cart_session_code VARCHAR(50),
                expired_at TIMESTAMP DEFAULT (CURRENT_TIMESTAMP + INTERVAL '15 minutes')
            );
        
;

            CREATE TABLE IF NOT EXISTS pending_transactions (
                pt_id SERIAL PRIMARY KEY,
                terminal_id INT NOT NULL,
                cashier_id INT NOT NULL,
                ts_id INT,
                item_id INT NOT NULL,
                barcode VARCHAR(50) NOT NULL,
                quantity NUMERIC(15,4) NOT NULL CHECK (quantity > 0),
                unit VARCHAR(20) NULL,
                unitid INT NOT NULL,
                sell_price NUMERIC(15,2) NOT NULL,
                buy_price NUMERIC(15,2) NOT NULL,
                discount_percentage NUMERIC(5,2) DEFAULT 0,
                discount_total NUMERIC(15,2) DEFAULT 0,
                tax NUMERIC(15,2) DEFAULT 0,
                total NUMERIC(18,2) NOT NULL,
                note TEXT,
                tsd_conversion_rate NUMERIC(12,4) DEFAULT 1,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                expired_at TIMESTAMP DEFAULT (CURRENT_TIMESTAMP + INTERVAL '15 minutes'),
                po_id INT NULL,
                cart_session_code VARCHAR(50),
                is_multi_price INT NULL,

            --  CONSTRAINT unique_terminal_item UNIQUE (terminal_id, item_id, unit,cart_session_code),
                CONSTRAINT fk_pending_transactions_item FOREIGN KEY (item_id)
                    REFERENCES items(id) ON DELETE CASCADE
            );
        
