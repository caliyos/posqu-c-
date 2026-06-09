
using System;
using System.Threading;
using Npgsql;
using POSqu_menu;
using QuestPDF.Infrastructure;
using QuestPDF;
using POS_qu.Helpers;
namespace POS_qu



{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var splash = new Splash();
            splash.Show();

            Application.DoEvents();

            // =========================
            // SETUP + CHECK DB (OUTSIDE UI LOGIC)
            // =========================
            bool needSetup = false;

            try
            {
                POS_qu.Helpers.DbConfig.LoadConfig();

                if (string.IsNullOrWhiteSpace(POS_qu.Helpers.DbConfig.ConnectionString))
                {
                    needSetup = true;
                }
                else
                {
                    using var conn = new Npgsql.NpgsqlConnection(
                        POS_qu.Helpers.DbConfig.ConnectionString
                    );

                    conn.Open();
                }
            }
            catch
            {
                needSetup = true;
            }

            if (needSetup)
            {
                

                using (var setup = new POS_qu.DatabaseSetting())
                {
                    setup.ShowDialog();
                }

                POS_qu.Helpers.DbConfig.LoadConfig();
            }

            POS_qu.Helpers.GlobalContext.RefreshConnectionInfo();

            // =========================
            // SHOW SPLASH 3 DETIK (SMOOTH)
            // =========================
            var sw = System.Diagnostics.Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < 3000)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(50);
            }

            splash.Close();

            Application.Run(new Login());
        }

    }
}
