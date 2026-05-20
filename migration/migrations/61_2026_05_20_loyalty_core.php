<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS membership_levels (
            id SERIAL PRIMARY KEY,
            code VARCHAR(20) NOT NULL UNIQUE,
            name VARCHAR(50) NOT NULL,
            rank INT NOT NULL DEFAULT 0,
            earn_multiplier NUMERIC(10,4) NOT NULL DEFAULT 1,
            discount_percent NUMERIC(10,4) NOT NULL DEFAULT 0,
            price_level_id INT NULL,
            min_total_spend NUMERIC(18,2) NOT NULL DEFAULT 0,
            min_total_txn INT NOT NULL DEFAULT 0,
            is_active BOOLEAN NOT NULL DEFAULT TRUE,
            created_at TIMESTAMP NOT NULL DEFAULT now(),
            updated_at TIMESTAMP NOT NULL DEFAULT now()
        );

        INSERT INTO membership_levels(code, name, rank, earn_multiplier, discount_percent, price_level_id, min_total_spend, min_total_txn, is_active)
        VALUES
            ('GUEST','Guest',0,0,0,NULL,0,0,TRUE),
            ('SILVER','Silver',10,1,0,NULL,0,0,TRUE),
            ('GOLD','Gold',20,2,0,NULL,0,0,TRUE),
            ('PLATINUM','Platinum',30,3,0,NULL,0,0,TRUE)
        ON CONFLICT (code) DO NOTHING;

        ALTER TABLE customers
            ADD COLUMN IF NOT EXISTS membership_level_id INT NULL REFERENCES membership_levels(id);

        UPDATE customers c
        SET membership_level_id = (
            SELECT id FROM membership_levels ml
            WHERE ml.code = CASE
                WHEN COALESCE(c.is_member, TRUE) = FALSE THEN 'GUEST'
                WHEN LOWER(COALESCE(c.level,'')) = 'platinum' THEN 'PLATINUM'
                WHEN LOWER(COALESCE(c.level,'')) = 'gold' THEN 'GOLD'
                WHEN LOWER(COALESCE(c.level,'')) = 'silver' THEN 'SILVER'
                WHEN LOWER(COALESCE(c.level,'')) = 'bronze' THEN 'SILVER'
                ELSE 'SILVER'
            END
            LIMIT 1
        )
        WHERE c.membership_level_id IS NULL;

        CREATE TABLE IF NOT EXISTS point_rules (
            id SERIAL PRIMARY KEY,
            membership_level_id INT NOT NULL REFERENCES membership_levels(id) ON DELETE CASCADE,
            spend_amount NUMERIC(18,2) NOT NULL DEFAULT 10000,
            earn_points INT NOT NULL DEFAULT 1,
            expiry_months INT NOT NULL DEFAULT 12,
            min_spend_per_txn NUMERIC(18,2) NOT NULL DEFAULT 0,
            max_points_per_txn INT NULL,
            is_active BOOLEAN NOT NULL DEFAULT TRUE,
            created_at TIMESTAMP NOT NULL DEFAULT now()
        );

        INSERT INTO point_rules(membership_level_id, spend_amount, earn_points, expiry_months, min_spend_per_txn, max_points_per_txn, is_active)
        SELECT ml.id, 10000, 1, 12, 0, NULL, TRUE
        FROM membership_levels ml
        WHERE ml.code IN ('SILVER','GOLD','PLATINUM')
        ON CONFLICT DO NOTHING;

        CREATE TABLE IF NOT EXISTS loyalty_accounts (
            id BIGSERIAL PRIMARY KEY,
            customer_id INT NOT NULL UNIQUE REFERENCES customers(id) ON DELETE CASCADE,
            membership_level_id INT NULL REFERENCES membership_levels(id),
            point_balance INT NOT NULL DEFAULT 0,
            lifetime_points_earned BIGINT NOT NULL DEFAULT 0,
            lifetime_points_redeemed BIGINT NOT NULL DEFAULT 0,
            lifetime_spend NUMERIC(18,2) NOT NULL DEFAULT 0,
            lifetime_txn INT NOT NULL DEFAULT 0,
            last_tx_at TIMESTAMP NULL,
            version INT NOT NULL DEFAULT 0,
            updated_at TIMESTAMP NOT NULL DEFAULT now()
        );

        INSERT INTO loyalty_accounts(customer_id, membership_level_id, point_balance, lifetime_points_earned, lifetime_spend, lifetime_txn, updated_at)
        SELECT c.id, c.membership_level_id, COALESCE(c.points,0), COALESCE(c.points,0), 0, 0, now()
        FROM customers c
        WHERE COALESCE(c.is_member, TRUE) = TRUE
        ON CONFLICT (customer_id) DO NOTHING;

        CREATE TABLE IF NOT EXISTS loyalty_transactions (
            id BIGSERIAL PRIMARY KEY,
            account_id BIGINT NOT NULL REFERENCES loyalty_accounts(id) ON DELETE CASCADE,
            tx_type VARCHAR(20) NOT NULL,
            idempotency_key VARCHAR(120) NOT NULL UNIQUE,
            ref_type VARCHAR(30) NULL,
            ref_id BIGINT NULL,
            invoice_number VARCHAR(80) NULL,
            warehouse_id INT NULL,
            terminal_id INT NULL,
            cashier_user_id INT NULL,
            login_id INT NULL,
            points_before INT NOT NULL DEFAULT 0,
            points_change INT NOT NULL DEFAULT 0,
            points_after INT NOT NULL DEFAULT 0,
            money_amount NUMERIC(18,2) NOT NULL DEFAULT 0,
            reason TEXT NULL,
            created_at TIMESTAMP NOT NULL DEFAULT now()
        );

        CREATE INDEX IF NOT EXISTS idx_loyalty_tx_account_created
            ON loyalty_transactions(account_id, created_at DESC, id DESC);

        CREATE INDEX IF NOT EXISTS idx_loyalty_tx_ref
            ON loyalty_transactions(ref_type, ref_id);

        CREATE TABLE IF NOT EXISTS loyalty_point_buckets (
            id BIGSERIAL PRIMARY KEY,
            account_id BIGINT NOT NULL REFERENCES loyalty_accounts(id) ON DELETE CASCADE,
            earned_tx_id BIGINT NULL REFERENCES loyalty_transactions(id) ON DELETE SET NULL,
            points_earned INT NOT NULL,
            points_used INT NOT NULL DEFAULT 0,
            expires_at DATE NULL,
            created_at TIMESTAMP NOT NULL DEFAULT now()
        );

        CREATE INDEX IF NOT EXISTS idx_loyalty_bucket_fifo
            ON loyalty_point_buckets(account_id, expires_at, created_at, id);

        CREATE TABLE IF NOT EXISTS reward_rules (
            id SERIAL PRIMARY KEY,
            code VARCHAR(50) NOT NULL UNIQUE,
            reward_type VARCHAR(20) NOT NULL,
            points_cost INT NOT NULL,
            value_amount NUMERIC(18,2) NOT NULL DEFAULT 0,
            free_item_id BIGINT NULL REFERENCES items(id),
            min_points INT NOT NULL DEFAULT 0,
            max_per_txn INT NULL,
            is_active BOOLEAN NOT NULL DEFAULT TRUE,
            created_at TIMESTAMP NOT NULL DEFAULT now()
        );

        CREATE TABLE IF NOT EXISTS vouchers (
            id BIGSERIAL PRIMARY KEY,
            code VARCHAR(50) NOT NULL UNIQUE,
            reward_rule_id INT NULL REFERENCES reward_rules(id),
            customer_id INT NULL REFERENCES customers(id),
            issued_at TIMESTAMP NOT NULL DEFAULT now(),
            expires_at DATE NULL,
            used_at TIMESTAMP NULL,
            used_ref_type VARCHAR(30) NULL,
            used_ref_id BIGINT NULL,
            status VARCHAR(20) NOT NULL DEFAULT 'ACTIVE'
        );

        CREATE INDEX IF NOT EXISTS idx_vouchers_customer_status
            ON vouchers(customer_id, status);

        CREATE TABLE IF NOT EXISTS loyalty_audit_logs (
            id BIGSERIAL PRIMARY KEY,
            action_type VARCHAR(50) NOT NULL,
            account_id BIGINT NULL REFERENCES loyalty_accounts(id) ON DELETE SET NULL,
            customer_id INT NULL REFERENCES customers(id) ON DELETE SET NULL,
            ref_type VARCHAR(30) NULL,
            ref_id BIGINT NULL,
            detail TEXT NULL,
            user_id INT NULL,
            login_id INT NULL,
            created_at TIMESTAMP NOT NULL DEFAULT now()
        );
    ");
};
