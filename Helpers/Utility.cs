using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace POS_qu.Helpers
{
    class Utility
    {
        //private static string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";

        public static string GetPcId()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .FirstOrDefault(nic => nic.OperationalStatus == OperationalStatus.Up
                                       && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)?
                .GetPhysicalAddress().ToString();
        }
        public static string GetCurrentShift()
        {
            var hour = DateTime.Now.Hour;
            if (hour >= 6 && hour < 14)
                return "Shift 1";
            else if (hour >= 14 && hour < 22)
                return "Shift 2";
            else
                return "Shift 3"; // Malam
        }

        public static string getTrxNumbering()
        {
            return "TXN-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        // Method untuk mengambil nama terminal berdasarkan PC ID
        public static string GetTerminalName(string pcId)
        {
            string terminalName = null;

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT terminal_name FROM terminals WHERE pc_id = @pc_id LIMIT 1";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@pc_id", pcId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                terminalName = reader.GetString(reader.GetOrdinal("terminal_name"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching terminal name: " + ex.Message);
                }
            }

            return terminalName;
        }

    }
}
