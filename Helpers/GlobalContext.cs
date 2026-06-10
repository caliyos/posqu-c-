using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{
    class GlobalContext
    {
        public static string DeveloperName => "caliyos";
        public static string DeveloperWebsite => "https://caliyos.my.id";
        public static string DeveloperEmail => "support@caliyos.my.id";
        public static string DeveloperWhatsapp => "+6821-7516-048";

        public static string AppName => Application.ProductName;

        public static string LicenseInfo
        {
            get
            {
                try
                {
                    var license = LicenseManager.LoadAsync().GetAwaiter().GetResult();
                    //var license = LicenseManager.LoadAsync().GetAwaiter().GetResult();

                    //if (license != null)
                    //{
                    //    MessageBox.Show(
                    //        $"StoreId      : {license.StoreId}\n" +
                    //        $"LicenseType  : {license.LicenseType}\n" +
                    //        $"Key          : {license.Key}\n" +
                    //        $"MachineId    : {license.MachineId}\n" +
                    //        $"DeviceId     : {license.DeviceId}\n" +
                    //        $"ActivatedAt  : {license.ActivatedAt}\n" +
                    //        $"ExpiresAt    : {license.ExpiresAt}\n" +
                    //        $"ActivationId : {license.ActivationId}"
                    //    );
                    //}
                    if (license != null)
                    {
                        return $"{license.StoreId} ({license.LicenseType})";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return "Community Edition";
            }
        }

        public static string AppVersion =>
            $"v{Application.ProductVersion}";

        public static string getAppVersion()
        {
            return $"{Application.ProductName}/{Application.ProductVersion} ({Environment.OSVersion})";
        }

        public static string ConnectionHost { get; private set; } = "";
        public static string ConnectionDatabase { get; private set; } = "";
        public static int ConnectionPort { get; private set; } = 0;
        public static string ConnectionSummary { get; private set; } = "";
      
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
