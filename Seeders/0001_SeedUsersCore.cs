using Npgsql;

namespace POS_qu.Seeders
{
    public class _0001_SeedUsersCore : ISeeder
    {
        public string Id => "0001_SeedUsersCore";

        public void Run(NpgsqlConnection conn, NpgsqlTransaction tran)
        {
            // Seed minimal roles via users.role string for now, align with your roles table if needed
            // Insert default admin/cashier/supervisor users if not exists
            string sql = @"
                INSERT INTO users (id, username, password_hash, role, created_at)
                VALUES 
                    (1, 'admin', '$2a$10$3bUqRzUO4qEoOJu5p3T0vO7xwBz9sV0iH6u7FZ0x7xJ8c7J8D0m8K', 'admin', NOW()),
                    (2, 'kasir', '$2a$10$iW5cGwZIKQICFZ101H.zzezBWomfymDc7wIE6ASGFNTMehYlkq.I.', 'cashier', NOW()),
                    (3, 'supervisor', '$2a$10$SJgIDsAM3.SCFVLN6btWNeecqfm6as/KiSHB.m.rDI.0KQgdM/h12', 'supervisor', NOW())
                ON CONFLICT (id) DO NOTHING;";
            using var cmd = new NpgsqlCommand(sql, conn, tran);
            cmd.ExecuteNonQuery();
        }
    }
}
