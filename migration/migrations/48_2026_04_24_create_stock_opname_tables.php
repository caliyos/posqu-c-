<?php
return function($db) {
    $db->exec("
        CREATE TABLE IF NOT EXISTS stock_opnames (
          id BIGSERIAL PRIMARY KEY,
          opname_no VARCHAR(50) NOT NULL,
          opname_date DATE NOT NULL,
          warehouse_id INT NOT NULL REFERENCES warehouses(id),
          mode VARCHAR(20) NOT NULL,
          created_by INT NULL,
          created_at TIMESTAMP NOT NULL DEFAULT NOW()
        );

        CREATE TABLE IF NOT EXISTS stock_opname_items (
          id BIGSERIAL PRIMARY KEY,
          opname_id BIGINT NOT NULL REFERENCES stock_opnames(id) ON DELETE CASCADE,
          item_id BIGINT NOT NULL REFERENCES items(id),
          unit_id INT NOT NULL,
          conversion DOUBLE PRECISION NOT NULL DEFAULT 1,
          system_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
          physical_qty_input DOUBLE PRECISION NOT NULL DEFAULT 0,
          physical_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
          diff_qty_base DOUBLE PRECISION NOT NULL DEFAULT 0,
          note TEXT
        );

        CREATE INDEX IF NOT EXISTS idx_stock_opnames_date ON stock_opnames(opname_date);
        CREATE INDEX IF NOT EXISTS idx_stock_opname_items_opname ON stock_opname_items(opname_id);
    ");
};

