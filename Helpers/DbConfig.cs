using Npgsql;
using System;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;

namespace POS_qu.Helpers
{
    public static class DbConfig
    {
        private static readonly string programDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Posqu");
        private static readonly string configJsonPath = Path.Combine(programDataDir, "config.json");
        private static readonly string legacyPath = @"D:\dbconfigposqu.config";
        private static readonly string defaultTimeZone = "Asia/Makassar"; // ✅ ganti sesuai kebutuhan kamu

        public static string ConnectionString { get; private set; }

        static DbConfig()
        {
            LoadConfig();
            //SetDatabaseTimeZone(); // ✅ panggil otomatis setelah load config
        }

        public static void LoadConfig()
        {
            try
            {
                if (File.Exists(configJsonPath))
                {
                    string json = File.ReadAllText(configJsonPath);
                    var cfg = JsonSerializer.Deserialize<PosquConfig>(json);
                    if (cfg == null)
                    {
                        ConnectionString = string.Empty;
                        return;
                    }
                    var builder = new NpgsqlConnectionStringBuilder
                    {
                        Host = cfg.Host ?? "localhost",
                        Port = cfg.Port > 0 ? cfg.Port : 5432,
                        Username = cfg.Username ?? "postgres",
                        Password = cfg.Password ?? "",
                        Database = cfg.Database ?? "postgres"
                    };
                    ConnectionString = builder.ConnectionString;
                    return;
                }

                if (File.Exists(legacyPath))
                {
                    string connString = File.ReadAllText(legacyPath).Trim();
                    var builder = new NpgsqlConnectionStringBuilder(connString);
                    var cfg = new PosquConfig
                    {   
                        Host = builder.Host,
                        Port = builder.Port,
                        Username = builder.Username,
                        Password = builder.Password,
                        Database = builder.Database
                    };
                    Directory.CreateDirectory(programDataDir);
                    File.WriteAllText(configJsonPath, JsonSerializer.Serialize(cfg, new JsonSerializerOptions { WriteIndented = true }));
                    ConnectionString = builder.ConnectionString;
                    return;
                }

                ConnectionString = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Error saat membaca config: {ex.Message}",
                    "Config Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                ConnectionString = string.Empty;
            }
        }

        /// <summary>
        /// ✅ Set timezone database (hanya sekali saat aplikasi start)
        /// </summary>
       
    }

    public class PosquConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
    }
}
