
using System;
using Npgsql;
using POSqu_menu;
using QuestPDF.Infrastructure;
using QuestPDF;
using POS_qu.Helpers;
namespace POS_qu



{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            // Aktifkan Community License (gratis)
            QuestPDF.Settings.License = LicenseType.Community;



            //Console.WriteLine("Test message from the Main method.");

            //     // Wait for user input before closing the console
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadLine();  // Waits for the user to press Enter
            // Inisialisasi UserAgent global sekali



            ApplicationConfiguration.Initialize();
            //Application.Run(new Form3_crud());




            try
            {
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
                        using var conn = new Npgsql.NpgsqlConnection(POS_qu.Helpers.DbConfig.ConnectionString);
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
                Application.Run(new Login());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An unhandled exception occurred:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            // Connection string (replace with your actual connection details)
            //string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=pos-qu";

            //try
            //{
            //    // Create and open a connection
            //    using (var conn = new NpgsqlConnection(connectionString))
            //    {
            //        conn.Open();

            //        // Execute a simple query
            //        using (var cmd = new NpgsqlCommand("SELECT version()", conn))
            //        {
            //            var version = cmd.ExecuteScalar().ToString();
            //            Console.WriteLine("PostgreSQL version: " + version);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error: " + ex.Message);
            //}

        }


    }
}
