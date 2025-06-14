
using System;
using Npgsql;
using POSqu_menu;
using QuestPDF.Infrastructure;
using QuestPDF;
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


            ApplicationConfiguration.Initialize();
            //Application.Run(new Form3_crud());


            try
            {

                //Application.Run(new Form2_crud());
                //Application.Run(new Roles());
                //Application.Run(new Terminal());
                //Application.Run(new MenuNative());
                Application.Run(new Login());
                //Application.Run(new Casher_POS());
                //Application.Run(new SalesReports());
                //Application.Run(new StockReports());
                //Application.Run(new ProductPage());
                //Application.Run(new TokoSetting());
                //Application.Run(new StrukSetting());

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