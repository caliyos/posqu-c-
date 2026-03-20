using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{
    class GlobalContext
    {
        public static string ConnectionHost { get; private set; } = "";
        public static string ConnectionDatabase { get; private set; } = "";
        public static int ConnectionPort { get; private set; } = 0;
        public static string ConnectionSummary { get; private set; } = "";
        public static string getAppVersion()
        {

            return $"PosQu/{Application.ProductVersion} ({Environment.OSVersion})";
        }
        public static void RefreshConnectionInfo()
        {
            var cs = DbConfig.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
            {
                ConnectionHost = "";
                ConnectionDatabase = "";
                ConnectionPort = 0;
                ConnectionSummary = "";
                return;
            }
            var b = new Npgsql.NpgsqlConnectionStringBuilder(cs);
            ConnectionHost = b.Host;
            ConnectionDatabase = b.Database;
            ConnectionPort = b.Port;
            ConnectionSummary = $"{ConnectionDatabase}@{ConnectionHost}:{ConnectionPort}";
        }
    }
}
