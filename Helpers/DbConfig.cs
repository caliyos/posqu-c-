using Npgsql;
using System;
using System.IO;
using System.Windows.Forms;

namespace POS_qu.Helpers
{
    public static class DbConfig
    {
        private static readonly string configPath = @"D:\dbconfigposqu.config";
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
                if (!File.Exists(configPath))
                {
                    MessageBox.Show(
                        $"⚠️ Config file '{configPath}' tidak ditemukan.",
                        "Config Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    ConnectionString = string.Empty;
                    return;
                }

                string connString = File.ReadAllText(configPath).Trim();

                try
                {
                    var builder = new NpgsqlConnectionStringBuilder(connString);
                    ConnectionString = builder.ConnectionString;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"⚠️ Config file format invalid: {ex.Message}",
                        "Config Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    ConnectionString = string.Empty;
                }
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
}
