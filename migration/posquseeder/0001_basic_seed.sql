-- Seed basic data safely
-- Units (only if empty)
INSERT INTO units (name, abbr)
SELECT v.name, v.abbr
FROM (VALUES
  ('Pieces','pcs'),
  ('Dus','dus'),
  ('Kilogram','kg')
) AS v(name, abbr)
WHERE NOT EXISTS (SELECT 1 FROM units);

-- Categories (only if empty)
INSERT INTO categories (name, kode)
SELECT v.name, v.kode
FROM (VALUES
  ('Umum','UM'),
  ('Makanan','MK'),
  ('Minuman','MN')
) AS v(name, kode)
WHERE NOT EXISTS (SELECT 1 FROM categories);

-- Suppliers (only if empty)
INSERT INTO suppliers (name, kode, phone)
SELECT v.name, v.kode, v.phone
FROM (VALUES
  ('Supplier Default','SUP','08123456789')
) AS v(name, kode, phone)
WHERE NOT EXISTS (SELECT 1 FROM suppliers);

-- Terminals (only if empty)
INSERT INTO terminals (terminal_name, pc_id, description)
SELECT v.terminal_name, v.pc_id, v.description
FROM (VALUES
  ('T1','PC-DEFAULT-1','Terminal Utama'),
  ('T2','PC-DEFAULT-2','Terminal Cadangan')
) AS v(terminal_name, pc_id, description)
WHERE NOT EXISTS (SELECT 1 FROM terminals);

-- Shifts (only if empty)
INSERT INTO shifts (name, start_time, end_time)
SELECT v.name, v.start_time, v.end_time
FROM (VALUES
  ('Shift 1','07:00','15:00'),
  ('Shift 2','15:00','23:00'),
  ('Shift 3','23:00','07:00')
) AS v(name, start_time, end_time)
WHERE NOT EXISTS (SELECT 1 FROM shifts);
