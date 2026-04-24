<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS journal_entries (
            id BIGSERIAL PRIMARY KEY,
            date TIMESTAMP NOT NULL,
            reference_type VARCHAR(30) NOT NULL,
            reference_id BIGINT NOT NULL,
            description TEXT NULL,
            created_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        CREATE UNIQUE INDEX IF NOT EXISTS uq_journal_entries_reference ON journal_entries(reference_type, reference_id);

        CREATE TABLE IF NOT EXISTS journal_details (
            id BIGSERIAL PRIMARY KEY,
            journal_entry_id BIGINT NOT NULL REFERENCES journal_entries(id) ON DELETE CASCADE,
            account_id BIGINT NOT NULL REFERENCES accounts(id) ON DELETE RESTRICT,
            debit NUMERIC(18,2) NOT NULL DEFAULT 0,
            credit NUMERIC(18,2) NOT NULL DEFAULT 0,
            CONSTRAINT ck_journal_amount CHECK (
                debit >= 0 AND credit >= 0
                AND NOT (debit > 0 AND credit > 0)
                AND NOT (debit = 0 AND credit = 0)
            )
        );

        CREATE INDEX IF NOT EXISTS idx_journal_details_entry ON journal_details(journal_entry_id);
        CREATE INDEX IF NOT EXISTS idx_journal_details_account ON journal_details(account_id);
    ");
};
