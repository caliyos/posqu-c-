
DROP TABLE IF EXISTS purchase_orders CASCADE;

CREATE TABLE purchase_orders (
    id BIGSERIAL PRIMARY KEY,
    supplier_id BIGINT NULL,
    order_date DATE NOT NULL DEFAULT CURRENT_DATE,
    expected_date DATE NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'PENDING',  
        -- contoh: PENDING, APPROVED, RECEIVED, CANCELED
    total_amount NUMERIC(18,2) NOT NULL DEFAULT 0,
    note TEXT NULL,
    created_by BIGINT NOT NULL REFERENCES users(id),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);
    
