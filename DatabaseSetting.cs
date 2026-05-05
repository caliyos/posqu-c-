using Npgsql;
using POS_qu.Helpers;
using System.Text.Json;

namespace POS_qu
{
    public partial class DatabaseSetting : Form
    {
        private readonly string configPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Posqu", "config.json");

        public DatabaseSetting()
        {
            InitializeComponent();

            btnTest.Click += BtnTest_Click;
            btnResetSchema.Click += BtnResetSchema_Click;
            btnCreateDb.Click += BtnCreateDb_Click;
            btnSaveConfig.Click += BtnSaveConfig_Click;
            btnRunPhpMigrations.Click += BtnRunPhpMigrations_Click;
            btnRunPhpSeeders.Click += BtnRunPhpSeeders_Click;
            btnSetupAll.Click += BtnSetupAll_Click;

            TryLoadConfig();
        }

        private void BtnTest_Click(object? sender, EventArgs e)
        {
            try
            {
                string connString = $"Host={txtHost.Text.Trim()};Port={txtPort.Text.Trim()};Username={txtUser.Text.Trim()};Password={txtPass.Text.Trim()};Database=postgres";
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                MessageBox.Show("✅ Connected!", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Connection failed: {ex.Message}", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetSchema_Click(object? sender, EventArgs e)
        {
            try
            {
                string db = txtDb.Text.Trim();
                if (string.IsNullOrWhiteSpace(db)) { MessageBox.Show("Isi nama database dulu.", "Warning"); return; }
                using var conn = new NpgsqlConnection($"Host={txtHost.Text.Trim()};Port={txtPort.Text.Trim()};Username={txtUser.Text.Trim()};Password={txtPass.Text.Trim()};Database={db}");
                conn.Open();
                using var cmd = new NpgsqlCommand("DROP SCHEMA IF EXISTS public CASCADE; CREATE SCHEMA public;", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ Schema public direset.", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Reset gagal: {ex.Message}", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCreateDb_Click(object? sender, EventArgs e)
        {
            try
            {
                string db = txtDb.Text.Trim();
                if (string.IsNullOrWhiteSpace(db)) { MessageBox.Show("Isi nama database dulu.", "Warning"); return; }
                using var conn = new NpgsqlConnection($"Host={txtHost.Text.Trim()};Port={txtPort.Text.Trim()};Username={txtUser.Text.Trim()};Password={txtPass.Text.Trim()};Database=postgres");
                conn.Open();
                using var existsCmd = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname = @db", conn);
                existsCmd.Parameters.AddWithValue("db", db);
                var exists = existsCmd.ExecuteScalar() != null;
                if (!exists)
                {
                    using var createCmd = new NpgsqlCommand($"CREATE DATABASE \"{db}\"", conn);
                    createCmd.ExecuteNonQuery();
                    MessageBox.Show($"✅ Database '{db}' dibuat.", "Create DB", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"ℹ️ Database '{db}' sudah ada.", "Create DB", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Create DB gagal: {ex.Message}", "Create DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveConfig_Click(object? sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(configPath)!);
                var cfg = new PosquConfig
                {
                    Host = txtHost.Text.Trim(),
                    Port = int.TryParse(txtPort.Text.Trim(), out var p) ? p : 5432,
                    Username = txtUser.Text.Trim(),
                    Password = txtPass.Text.Trim(),
                    Database = txtDb.Text.Trim()
                };
                var json = JsonSerializer.Serialize(cfg, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configPath, json);
                MessageBox.Show("✅ Config tersimpan.", "Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Save config gagal: {ex.Message}", "Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TryLoadConfig()
        {
            try
            {
                if (File.Exists(configPath))
                {
                    var json = File.ReadAllText(configPath);
                    var cfg = JsonSerializer.Deserialize<PosquConfig>(json);
                    if (cfg != null)
                    {
                        txtHost.Text = cfg.Host ?? txtHost.Text;
                        txtPort.Text = (cfg.Port > 0 ? cfg.Port : int.Parse(txtPort.Text)).ToString();
                        txtUser.Text = cfg.Username ?? txtUser.Text;
                        txtPass.Text = cfg.Password ?? txtPass.Text;
                        txtDb.Text = cfg.Database ?? txtDb.Text;
                        return;
                    }
                }
                //var legacy = @"D:\dbconfigposqu.config";
                //if (File.Exists(legacy))
                //{
                //    var conn = File.ReadAllText(legacy).Trim();
                //    var map = ParseConnString(conn);
                //    if (map.TryGetValue("HOST", out var host)) txtHost.Text = host;
                //    if (map.TryGetValue("PORT", out var port)) txtPort.Text = port;
                //    if (map.TryGetValue("USERNAME", out var user)) txtUser.Text = user;
                //    if (map.TryGetValue("USER", out var user2)) txtUser.Text = user2;
                //    if (map.TryGetValue("PASSWORD", out var pass)) txtPass.Text = pass;
                //    if (map.TryGetValue("DATABASE", out var db)) txtDb.Text = db;
                //}
            }
            catch
            {
            }
        }

        private static Dictionary<string, string> ParseConnString(string conn)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var parts = conn.Split(';');
            foreach (var p in parts)
            {
                if (string.IsNullOrWhiteSpace(p)) continue;
                var idx = p.IndexOf('=');
                if (idx <= 0) continue;
                var k = p.Substring(0, idx).Trim();
                var v = p.Substring(idx + 1).Trim();
                dict[k.ToUpperInvariant()] = v;
            }
            return dict;
        }

        private void BtnRunPhpMigrations_Click(object? sender, EventArgs e)
        {
            try
            {
                string db = txtDb.Text.Trim();
                if (string.IsNullOrWhiteSpace(db)) { MessageBox.Show("Isi nama database dulu.", "Warning"); return; }
                string root = FindMigrationRootOrThrow();
                var env = new Dictionary<string, string>
                {
                    { "PGHOST", txtHost.Text.Trim() },
                    { "PGPORT", txtPort.Text.Trim() },
                    { "PGUSER", txtUser.Text.Trim() },
                    { "PGPASSWORD", txtPass.Text.Trim() },
                    { "PGDATABASE", db }
                };
                var (code, stdout, stderr) = PhpProcessRunner.RunScript(root, "posqumigration.php", env);
                txtLog.Text = stdout + (string.IsNullOrWhiteSpace(stderr) ? "" : "\r\n[stderr]\r\n" + stderr);
                if (code == 0)
                    MessageBox.Show("✅ Migrations selesai (PHP controller).", "Migrations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("❌ Migration (PHP) exit code: " + code, "Migrations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Migration gagal: {ex.Message}", "Migrations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRunPhpSeeders_Click(object? sender, EventArgs e)
        {
            try
            {
                string db = txtDb.Text.Trim();
                if (string.IsNullOrWhiteSpace(db)) { MessageBox.Show("Isi nama database dulu.", "Warning"); return; }
                string root = FindMigrationRootOrThrow();
                var env = new Dictionary<string, string>
                {
                    { "PGHOST", txtHost.Text.Trim() },
                    { "PGPORT", txtPort.Text.Trim() },
                    { "PGUSER", txtUser.Text.Trim() },
                    { "PGPASSWORD", txtPass.Text.Trim() },
                    { "PGDATABASE", db }
                };
                var (code, stdout, stderr) = PhpProcessRunner.RunScript(root, "posquseeder.php", env);
                txtLog.Text = stdout + (string.IsNullOrWhiteSpace(stderr) ? "" : "\r\n[stderr]\r\n" + stderr);
                if (code == 0)
                    MessageBox.Show("✅ Seeders selesai (PHP controller).", "Seeders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("❌ Seeder (PHP) exit code: " + code, "Seeders", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Seeder gagal: {ex.Message}", "Seeders", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnSetupAll_Click(object? sender, EventArgs e)
        {
            try
            {
                progress.Value = 0;
                BtnCreateDb_Click(sender, e);
                progress.Value = 25;

                string db = txtDb.Text.Trim();
                if (string.IsNullOrWhiteSpace(db)) { MessageBox.Show("Isi nama database dulu.", "Warning"); return; }
                string root = FindMigrationRootOrThrow();
                var env = new Dictionary<string, string>
                {
                    { "PGHOST", txtHost.Text.Trim() },
                    { "PGPORT", txtPort.Text.Trim() },
                    { "PGUSER", txtUser.Text.Trim() },
                    { "PGPASSWORD", txtPass.Text.Trim() },
                    { "PGDATABASE", db }
                };
                var mig = PhpProcessRunner.RunScript(root, "posqumigration.php", env);
                txtLog.Text = mig.stdout + (string.IsNullOrWhiteSpace(mig.stderr) ? "" : "\r\n[stderr]\r\n" + mig.stderr);
                if (mig.exitCode != 0) { MessageBox.Show("❌ Migration (PHP) exit code: " + mig.exitCode, "Migrations", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                progress.Value = 60;

                var seed = PhpProcessRunner.RunScript(root, "posquseeder.php", env);
                txtLog.Text += "\r\n\r\n" + seed.stdout + (string.IsNullOrWhiteSpace(seed.stderr) ? "" : "\r\n[stderr]\r\n" + seed.stderr);
                if (seed.exitCode != 0) { MessageBox.Show("❌ Seeder (PHP) exit code: " + seed.exitCode, "Seeders", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                progress.Value = 85;

                BtnSaveConfig_Click(sender, e);
                progress.Value = 100;
                MessageBox.Show("🎉 Setup selesai. Anda bisa mulai login.", "Initial Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Setup gagal: {ex.Message}", "Initial Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FindMigrationRootOrThrow()
        {
            string[] cand =
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "migration"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..","..","..","migration"),
                Path.Combine(Application.StartupPath, "migration"),
                Path.Combine(Application.StartupPath, "..","..","..","migration")
            };
            var root = Array.Find(cand, Directory.Exists);
            if (root == null) throw new DirectoryNotFoundException("Folder 'migration' tidak ditemukan.");
            return root;
        }

        private void DatabaseSetting_Load(object sender, EventArgs e)
        {

        }
    }
}
