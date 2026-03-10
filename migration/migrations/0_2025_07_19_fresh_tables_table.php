<?php

$dropAllTables = "
DO \$\$
DECLARE
    r RECORD;
BEGIN
    -- Loop semua tabel di schema 'public'
    FOR r IN (
        SELECT tablename 
        FROM pg_tables 
        WHERE schemaname = 'public'
    )
    LOOP
        -- Drop tabel dengan CASCADE agar FK ikut terhapus
        EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(r.tablename) || ' CASCADE';
    END LOOP;
END \$\$;
";

$db->exec($dropAllTables);
