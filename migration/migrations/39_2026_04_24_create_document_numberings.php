<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS document_numberings (
            doc_type VARCHAR(30) PRIMARY KEY,
            prefix VARCHAR(20) NOT NULL,
            last_date DATE NULL,
            last_number INT NOT NULL DEFAULT 0,
            pad_length INT NOT NULL DEFAULT 4,
            format VARCHAR(50) NOT NULL DEFAULT '{prefix}-{yyyyMMdd}-{seq}',
            updated_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        INSERT INTO document_numberings (doc_type, prefix, last_date, last_number, pad_length, format)
        VALUES
         ('SALE', 'TRX', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
         ('ORDER', 'ORD', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
         ('PO', 'PO', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
         ('PURCHASE', 'PB', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
         ('STOCK_OPNAME', 'SO', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
         ('PURCHASE_RETURN', 'RB', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}')
        ON CONFLICT (doc_type) DO NOTHING;
    ");
};

