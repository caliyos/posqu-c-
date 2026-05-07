using Npgsql;
using POS_qu.Helpers;
using System.Diagnostics;
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
            btnRefreshDbList.Click += BtnRefreshDbList_Click;
            cmbDbList.SelectedIndexChanged += CmbDbList_SelectedIndexChanged;
            btnBackupDb.Click += BtnBackupDb_Click;
            btnRestoreDb.Click += BtnRestoreDb_Click;

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
                DbConfig.LoadConfig();
                UpdateCurrentDbLabel();
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
            UpdateCurrentDbLabel();
            LoadDatabaseListSafe();
        }

        private void UpdateCurrentDbLabel()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DbConfig.ConnectionString))
                {
                    lblCurrentDb.Text = "Sekarang memakai database: -";
                    return;
                }
                var b = new NpgsqlConnectionStringBuilder(DbConfig.ConnectionString);
                lblCurrentDb.Text = $"Sekarang memakai database: {b.Database}";
            }
            catch
            {
                lblCurrentDb.Text = "Sekarang memakai database: -";
            }
        }

        private NpgsqlConnectionStringBuilder BuildConnBuilderFromInputs(string database)
        {
            return new NpgsqlConnectionStringBuilder
            {
                Host = txtHost.Text.Trim(),
                Port = int.TryParse(txtPort.Text.Trim(), out var p) && p > 0 ? p : 5432,
                Username = txtUser.Text.Trim(),
                Password = txtPass.Text.Trim(),
                Database = database
            };
        }

        private void LoadDatabaseListSafe()
        {
            try
            {
                var list = GetDatabaseNames();
                cmbDbList.BeginUpdate();
                cmbDbList.Items.Clear();
                foreach (var db in list)
                    cmbDbList.Items.Add(db);
                cmbDbList.EndUpdate();

                var current = txtDb.Text.Trim();
                if (!string.IsNullOrWhiteSpace(current))
                {
                    var idx = cmbDbList.FindStringExact(current);
                    if (idx >= 0) cmbDbList.SelectedIndex = idx;
                }
            }
            catch (Exception ex)
            {
                AppendLog("Gagal load list database: " + ex.Message);
            }
        }

        private List<string> GetDatabaseNames()
        {
            var builder = BuildConnBuilderFromInputs("postgres");
            using var conn = new NpgsqlConnection(builder.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT datname
FROM pg_database
WHERE datistemplate = FALSE
ORDER BY datname ASC
", conn);
            var list = new List<string>();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var name = rdr.IsDBNull(0) ? "" : rdr.GetString(0);
                if (!string.IsNullOrWhiteSpace(name))
                    list.Add(name);
            }
            return list;
        }

        private void BtnRefreshDbList_Click(object? sender, EventArgs e)
        {
            LoadDatabaseListSafe();
        }

        private void CmbDbList_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                if (cmbDbList.SelectedItem is string db && !string.IsNullOrWhiteSpace(db))
                    txtDb.Text = db;
            }
            catch
            {
            }
        }

        private async void BtnBackupDb_Click(object? sender, EventArgs e)
        {
            string db = txtDb.Text.Trim();
            if (string.IsNullOrWhiteSpace(db))
            {
                MessageBox.Show("Isi nama database dulu.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Postgres Backup (*.backup)|*.backup|All Files (*.*)|*.*",
                FileName = $"{db}_{DateTime.Now:yyyyMMdd_HHmmss}.backup",
                Title = "Simpan Backup Database"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                txtLog.Clear();
                AppendLog("Backup database: " + db);
                AppendLog("File: " + sfd.FileName);

                progress.Style = ProgressBarStyle.Marquee;
                progress.MarqueeAnimationSpeed = 30;

                var dumpExe = FindPgToolExecutable("pg_dump.exe");
                await RunPgDumpAsync(dumpExe, db, sfd.FileName);

                AppendLog("✅ Backup selesai.");
                MessageBox.Show("✅ Backup selesai.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog("❌ Backup gagal: " + ex.Message);
                MessageBox.Show("❌ Backup gagal: " + ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progress.Style = ProgressBarStyle.Blocks;
                progress.MarqueeAnimationSpeed = 0;
                progress.Value = 0;
            }
        }

        private async void BtnRestoreDb_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Postgres Backup (*.backup;*.dump;*.sql)|*.backup;*.dump;*.sql|All Files (*.*)|*.*",
                Title = "Pilih File Backup"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            var target = PromptTargetDatabase(txtDb.Text.Trim());
            if (target == null) return;

            var targetDb = target.Value.DatabaseName;
            var createIfMissing = target.Value.CreateIfMissing;

            try
            {
                txtLog.Clear();
                AppendLog("Restore database");
                AppendLog("File: " + ofd.FileName);
                AppendLog("Target DB: " + targetDb);

                progress.Style = ProgressBarStyle.Marquee;
                progress.MarqueeAnimationSpeed = 30;

                if (createIfMissing)
                    EnsureDatabaseExists(targetDb);

                TerminateConnections(targetDb);

                if (Path.GetExtension(ofd.FileName).Equals(".sql", StringComparison.OrdinalIgnoreCase))
                {
                    var psqlExe = FindPgToolExecutable("psql.exe");
                    await RunPsqlRestoreAsync(psqlExe, targetDb, ofd.FileName);
                }
                else
                {
                    var restoreExe = FindPgToolExecutable("pg_restore.exe");
                    await RunPgRestoreAsync(restoreExe, targetDb, ofd.FileName);
                }

                txtDb.Text = targetDb;
                LoadDatabaseListSafe();
                AppendLog("✅ Restore selesai.");
                MessageBox.Show("✅ Restore selesai.\nJika ingin memakai database ini, klik Save Config.", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog("❌ Restore gagal: " + ex.Message);
                MessageBox.Show("❌ Restore gagal: " + ex.Message, "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progress.Style = ProgressBarStyle.Blocks;
                progress.MarqueeAnimationSpeed = 0;
                progress.Value = 0;
            }
        }

        private void AppendLog(string text)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke((Action)(() =>
                    {
                        txtLog.AppendText(text + Environment.NewLine);
                    }));
                }
                else
                {
                    txtLog.AppendText(text + Environment.NewLine);
                }
            }
            catch
            {
            }
        }

        private void EnsureDatabaseExists(string db)
        {
            var admin = BuildConnBuilderFromInputs("postgres");
            using var conn = new NpgsqlConnection(admin.ConnectionString);
            conn.Open();
            using var existsCmd = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname = @db", conn);
            existsCmd.Parameters.AddWithValue("@db", db);
            var exists = existsCmd.ExecuteScalar() != null;
            if (exists) return;
            using var createCmd = new NpgsqlCommand($"CREATE DATABASE \"{db.Replace("\"", "\"\"")}\"", conn);
            createCmd.ExecuteNonQuery();
        }

        private void TerminateConnections(string db)
        {
            var admin = BuildConnBuilderFromInputs("postgres");
            using var conn = new NpgsqlConnection(admin.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT pg_terminate_backend(pid)
FROM pg_stat_activity
WHERE datname = @db AND pid <> pg_backend_pid()
", conn);
            cmd.Parameters.AddWithValue("@db", db);
            cmd.ExecuteNonQuery();
        }

        private async Task RunPgDumpAsync(string exePath, string db, string outputFile)
        {
            var b = BuildConnBuilderFromInputs(db);
            var host = b.Host;
            var port = b.Port.ToString();
            var user = b.Username;
            var pass = b.Password;

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            psi.Environment["PGPASSWORD"] = pass ?? "";
            psi.ArgumentList.Add("-h");
            psi.ArgumentList.Add(host);
            psi.ArgumentList.Add("-p");
            psi.ArgumentList.Add(port);
            psi.ArgumentList.Add("-U");
            psi.ArgumentList.Add(user);
            psi.ArgumentList.Add("-F");
            psi.ArgumentList.Add("c");
            psi.ArgumentList.Add("-b");
            psi.ArgumentList.Add("-v");
            psi.ArgumentList.Add("-f");
            psi.ArgumentList.Add(outputFile);
            psi.ArgumentList.Add(db);

            await RunProcessAsync(psi);
        }

        private async Task RunPgRestoreAsync(string exePath, string db, string inputFile)
        {
            var b = BuildConnBuilderFromInputs(db);
            var host = b.Host;
            var port = b.Port.ToString();
            var user = b.Username;
            var pass = b.Password;

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            psi.Environment["PGPASSWORD"] = pass ?? "";
            psi.ArgumentList.Add("-h");
            psi.ArgumentList.Add(host);
            psi.ArgumentList.Add("-p");
            psi.ArgumentList.Add(port);
            psi.ArgumentList.Add("-U");
            psi.ArgumentList.Add(user);
            psi.ArgumentList.Add("-d");
            psi.ArgumentList.Add(db);
            psi.ArgumentList.Add("--clean");
            psi.ArgumentList.Add("--if-exists");
            psi.ArgumentList.Add("--no-owner");
            psi.ArgumentList.Add("--no-privileges");
            psi.ArgumentList.Add("-v");
            psi.ArgumentList.Add(inputFile);

            await RunProcessAsync(psi);
        }

        private async Task RunPsqlRestoreAsync(string exePath, string db, string inputFile)
        {
            var b = BuildConnBuilderFromInputs(db);
            var host = b.Host;
            var port = b.Port.ToString();
            var user = b.Username;
            var pass = b.Password;

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            psi.Environment["PGPASSWORD"] = pass ?? "";
            psi.ArgumentList.Add("-h");
            psi.ArgumentList.Add(host);
            psi.ArgumentList.Add("-p");
            psi.ArgumentList.Add(port);
            psi.ArgumentList.Add("-U");
            psi.ArgumentList.Add(user);
            psi.ArgumentList.Add("-d");
            psi.ArgumentList.Add(db);
            psi.ArgumentList.Add("-f");
            psi.ArgumentList.Add(inputFile);

            await RunProcessAsync(psi);
        }

        private async Task RunProcessAsync(ProcessStartInfo psi)
        {
            using var p = new Process { StartInfo = psi, EnableRaisingEvents = true };
            p.OutputDataReceived += (_, e) => { if (e.Data != null) AppendLog(e.Data); };
            p.ErrorDataReceived += (_, e) => { if (e.Data != null) AppendLog(e.Data); };

            if (!p.Start())
                throw new InvalidOperationException("Gagal menjalankan proses.");

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            await p.WaitForExitAsync();
            if (p.ExitCode != 0)
                throw new InvalidOperationException("Exit code: " + p.ExitCode);
        }

        private string FindPgToolExecutable(string exeName)
        {
            var fromPath = TryFindOnPath(exeName);
            if (!string.IsNullOrWhiteSpace(fromPath))
                return fromPath;

            var programFiles = new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
            };

            foreach (var root in programFiles.Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                try
                {
                    var baseDir = Path.Combine(root, "PostgreSQL");
                    if (!Directory.Exists(baseDir)) continue;

                    var versionDirs = Directory.GetDirectories(baseDir)
                        .OrderByDescending(d => d, StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    foreach (var vdir in versionDirs)
                    {
                        var cand = Path.Combine(vdir, "bin", exeName);
                        if (File.Exists(cand)) return cand;
                    }
                }
                catch
                {
                }
            }

            var fallback = Path.Combine("C:\\", "PostgreSQL", "bin", exeName);
            if (File.Exists(fallback)) return fallback;

            throw new FileNotFoundException($"Tool Postgres '{exeName}' tidak ditemukan. Install PostgreSQL client tools atau masukkan pg_dump/pg_restore ke PATH.");
        }

        private static string? TryFindOnPath(string exeName)
        {
            try
            {
                var path = Environment.GetEnvironmentVariable("PATH") ?? "";
                foreach (var dir in path.Split(';'))
                {
                    if (string.IsNullOrWhiteSpace(dir)) continue;
                    var p = dir.Trim();
                    var cand = Path.Combine(p, exeName);
                    if (File.Exists(cand)) return cand;
                }
            }
            catch
            {
            }
            return null;
        }

        private readonly struct RestoreTarget
        {
            public RestoreTarget(string databaseName, bool createIfMissing)
            {
                DatabaseName = databaseName;
                CreateIfMissing = createIfMissing;
            }

            public string DatabaseName { get; }
            public bool CreateIfMissing { get; }
        }

        private RestoreTarget? PromptTargetDatabase(string defaultDb)
        {
            var list = new List<string>();
            try
            {
                list = GetDatabaseNames();
            }
            catch
            {
            }

            using var f = new Form
            {
                Text = "Target Restore",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Width = 460,
                Height = 240
            };

            var lbl = new Label
            {
                Text = "Restore ke database:",
                AutoSize = true,
                Location = new Point(16, 18)
            };

            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                Location = new Point(16, 48),
                Width = 410
            };
            foreach (var d in list)
                cmb.Items.Add(d);
            if (!string.IsNullOrWhiteSpace(defaultDb))
                cmb.Text = defaultDb;

            var chk = new CheckBox
            {
                Text = "Buat database jika belum ada",
                AutoSize = true,
                Location = new Point(16, 88),
                Checked = true
            };

            var btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Width = 100,
                Height = 32,
                Location = new Point(216, 128)
            };
            var btnCancel = new Button
            {
                Text = "Batal",
                DialogResult = DialogResult.Cancel,
                Width = 100,
                Height = 32,
                Location = new Point(326, 128)
            };

            f.Controls.Add(lbl);
            f.Controls.Add(cmb);
            f.Controls.Add(chk);
            f.Controls.Add(btnOk);
            f.Controls.Add(btnCancel);
            f.AcceptButton = btnOk;
            f.CancelButton = btnCancel;

            if (f.ShowDialog(this) != DialogResult.OK) return null;
            var name = (cmb.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Nama database wajib diisi.", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            return new RestoreTarget(name, chk.Checked);
        }
    }
}
