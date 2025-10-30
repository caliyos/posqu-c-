using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{
    public static class DbInitializer
    {
        public static void CreateDatabase(string host, string port, string user, string pass, string dbName)
        {
            string connStringMaster = $"Host={host};Port={port};Username={user};Password={pass};Database=postgres";

            using (var conn = new NpgsqlConnection(connStringMaster))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"CREATE DATABASE \"{dbName}\"", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void CreateTables(string host, string port, string user, string pass, string dbName)
        {
            string connString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbName}";
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                CreateUsersTable(conn);
                CreateTransactionsTable(conn);
                // Tambah tabel lain di sini...
            }
        }

        private static void CreateUsersTable(NpgsqlConnection conn)
        {
            string sql = @"
                DROP TABLE IF EXISTS users CASCADE;
                CREATE TABLE users (
                    id SERIAL PRIMARY KEY,
                    username VARCHAR(50) UNIQUE NOT NULL,
                    password_hash TEXT NOT NULL,
                    role VARCHAR(50) NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreateTransactionsTable(NpgsqlConnection conn)
        {
            string sql = @"
                DROP TABLE IF EXISTS transactions CASCADE;
                CREATE TABLE transactions (
                    ts_id SERIAL PRIMARY KEY,
                    ts_numbering VARCHAR(50) NOT NULL,
                    ts_code VARCHAR(50) NOT NULL,
                    ts_total NUMERIC(18,2) NOT NULL,
                    ts_payment_amount NUMERIC(18,2) NOT NULL,
                    ts_cashback NUMERIC(15,2) DEFAULT 0,
                    ts_method VARCHAR(50) NOT NULL,
                    ts_status SMALLINT NOT NULL CHECK (ts_status IN (1, 2, 3)),
                    ts_change NUMERIC(15,2) DEFAULT 0,
                    ts_internal_note TEXT,
                    ts_note TEXT,
                    ts_customer INT,
                    ts_freename VARCHAR(255),
                    terminal_id INT,
                    shift_id INT,
                    user_id INT,
                    created_by INT,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    deleted_at TIMESTAMP NULL,
                    CONSTRAINT fk_user_id FOREIGN KEY (user_id) REFERENCES users(id),
                    CONSTRAINT fk_created_by FOREIGN KEY (created_by) REFERENCES users(id)
                );";

            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
    }
