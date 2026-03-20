-- Units
DROP TABLE IF EXISTS units CASCADE;
CREATE TABLE units (
  id SERIAL PRIMARY KEY,
  name VARCHAR(50) NOT NULL,
  abbr VARCHAR(10) NOT NULL
);

-- Categories
DROP TABLE IF EXISTS categories CASCADE;
CREATE TABLE categories (
  id SERIAL PRIMARY KEY,
  name VARCHAR(100) NOT NULL,
  kode VARCHAR(20) NOT NULL,
  parent_id INT NULL
);

-- Suppliers
DROP TABLE IF EXISTS suppliers CASCADE;
CREATE TABLE suppliers (
  id BIGSERIAL PRIMARY KEY,
  name VARCHAR(120) NOT NULL,
  kode VARCHAR(20),
  phone VARCHAR(30)
);

-- Items
DROP TABLE IF EXISTS items CASCADE;
CREATE TABLE items (
  id SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  buy_price NUMERIC(18,2) NOT NULL DEFAULT 0,
  sell_price NUMERIC(18,2) NOT NULL DEFAULT 0,
  barcode VARCHAR(120) DEFAULT '',
  stock INT NOT NULL DEFAULT 0,
  reserved_stock INT NOT NULL DEFAULT 0,
  unit INT NOT NULL,
  category_id INT NULL,
  supplier_id BIGINT NULL,
  note TEXT,
  picture TEXT,
  is_inventory_p BOOLEAN NOT NULL DEFAULT TRUE,
  is_purchasable BOOLEAN NOT NULL DEFAULT TRUE,
  is_sellable BOOLEAN NOT NULL DEFAULT TRUE,
  is_note_payment BOOLEAN NOT NULL DEFAULT FALSE,
  is_changeprice_p BOOLEAN NOT NULL DEFAULT FALSE,
  is_have_bahan BOOLEAN NOT NULL DEFAULT FALSE,
  is_box BOOLEAN NOT NULL DEFAULT FALSE,
  is_produksi BOOLEAN NOT NULL DEFAULT FALSE,
  discount_formula VARCHAR(50) DEFAULT '',
  flag INT NOT NULL DEFAULT 0,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  deleted_at TIMESTAMP NULL,
  CONSTRAINT fk_items_unit FOREIGN KEY (unit) REFERENCES units(id),
  CONSTRAINT fk_items_category FOREIGN KEY (category_id) REFERENCES categories(id),
  CONSTRAINT fk_items_supplier FOREIGN KEY (supplier_id) REFERENCES suppliers(id)
);

-- Unit variants
DROP TABLE IF EXISTS unit_variants CASCADE;
CREATE TABLE unit_variants (
  id SERIAL PRIMARY KEY,
  item_id INT NOT NULL,
  unit_id INT NOT NULL,
  conversion INT NOT NULL DEFAULT 1,
  sell_price NUMERIC(18,2) NOT NULL DEFAULT 0,
  profit NUMERIC(18,2) NOT NULL DEFAULT 0,
  minqty NUMERIC(18,2) NOT NULL DEFAULT 1,
  is_base_unit BOOLEAN NOT NULL DEFAULT FALSE,
  CONSTRAINT fk_uv_item FOREIGN KEY (item_id) REFERENCES items(id),
  CONSTRAINT fk_uv_unit FOREIGN KEY (unit_id) REFERENCES units(id)
);

-- Item tiered prices
DROP TABLE IF EXISTS item_prices CASCADE;
CREATE TABLE item_prices (
  id SERIAL PRIMARY KEY,
  item_id INT NOT NULL,
  min_qty INT NOT NULL,
  price NUMERIC(18,2) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_ip_item FOREIGN KEY (item_id) REFERENCES items(id)
);

-- Users (minimal)
DROP TABLE IF EXISTS users CASCADE;
CREATE TABLE users (
  id SERIAL PRIMARY KEY,
  name VARCHAR(100),
  username VARCHAR(50) UNIQUE NOT NULL,
  password_hash TEXT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Terminals (minimal)
DROP TABLE IF EXISTS terminals CASCADE;
CREATE TABLE terminals (
  id SERIAL PRIMARY KEY,
  terminal_name VARCHAR(50) NOT NULL,
  pc_id VARCHAR(120) NOT NULL,
  description VARCHAR(255)
);

-- Shifts (minimal)
DROP TABLE IF EXISTS shifts CASCADE;
CREATE TABLE shifts (
  id SERIAL PRIMARY KEY,
  name VARCHAR(50) NOT NULL,
  start_time TIME NOT NULL,
  end_time TIME NOT NULL
);

-- Seed minimal master data
INSERT INTO units (name, abbr) VALUES
('Pieces', 'pcs'),
('Dus', 'dus'),
('Kilogram', 'kg');

INSERT INTO categories (name, kode) VALUES
('Umum', 'UM'),
('Makanan', 'MK'),
('Minuman', 'MN');

INSERT INTO suppliers (name, kode, phone) VALUES
('Supplier Default', 'SUP', '08123456789');
