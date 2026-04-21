<?php
namespace App\Config;

use PDO;
use PDOException;

class Database {
    public static function connect() {
        try {
            // Deteksi environment
            $envHost = getenv('PGHOST');
            if ($envHost !== false && $envHost !== '') {
                $dbHost = $envHost;
            } else {
                if (self::isDocker()) {
                    $dbHost = 'host.docker.internal'; // container → host
                } else {
                    $dbHost = 'localhost'; // host langsung
                }
            }

            $dbPort = getenv('PGPORT') ?: 5432; // default PostgreSQL
            $dbName = getenv('PGDATABASE') ?: 'A-Posqu001';
            $dbUser = getenv('PGUSER') ?: 'postgres';
            $dbPass = getenv('PGPASSWORD') ?: 'postgres14';

            // Koneksi PostgreSQL via PDO
            $dsn = "pgsql:host={$dbHost};port={$dbPort};dbname={$dbName};";

            return new PDO(
                $dsn,
                $dbUser,
                $dbPass,
                [
                    PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
                    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
                    PDO::ATTR_EMULATE_PREPARES => false,
                ]
            );
        } catch (PDOException $e) {
            die("❌ Database connection failed: " . $e->getMessage());
        }
    }

    private static function isDocker(): bool {
        return file_exists('/.dockerenv') || getenv('IS_DOCKER') === '1';
    }
}
