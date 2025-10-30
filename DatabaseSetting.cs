using Npgsql;
using System;
using System.IO;
using System.Windows.Forms;

using POS_qu.Helpers;

namespace POS_qu
{
    public partial class DatabaseSetting : Form
    {
        private string configPath = @"D:\dbconfigposqu.config";

        public DatabaseSetting()
        {
            InitializeComponent();

            button1.Click += BtnTestConnection_Click;
            button2.Click += BtnCreateDatabase_Click;

            LoadConfigToFields();
        }

        private void LoadConfigToFields()
        {
            if (!File.Exists(configPath))
            {
                MessageBox.Show($"Config file '{configPath}' tidak ditemukan. Silakan isi manual dan simpan.",
                                "Config Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }
            else
            {
                string connString = File.ReadAllText(configPath).Trim();

                try
                {
                    var builder = new NpgsqlConnectionStringBuilder(connString);
                    textBox1.Text = builder.Host;        // IP / Host
                    textBox2.Text = builder.Port.ToString(); // Port
                    textBox3.Text = builder.Username;    // Username
                    textBox4.Text = builder.Password;    // Password
                    textBox5.Text = builder.Database;    // Database name
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Config file format invalid: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                }
            }

        }

        private void SaveConfigFromFields()
        {
            string connString = $"Host={textBox1.Text.Trim()};" +
                                $"Port={textBox2.Text.Trim()};" +
                                $"Username={textBox3.Text.Trim()};" +
                                $"Password={textBox4.Text.Trim()};" +
                                $"Database={textBox5.Text.Trim()}";
            File.WriteAllText(configPath, connString);
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            string connString = $"Host={textBox1.Text.Trim()};" +
                                $"Port={textBox2.Text.Trim()};" +
                                $"Username={textBox3.Text.Trim()};" +
                                $"Password={textBox4.Text.Trim()};" +
                                $"Database=postgres";

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    MessageBox.Show("✅ Connected successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SaveConfigFromFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCreateDatabase_Click(object sender, EventArgs e)
        {
            string host = textBox1.Text.Trim();
            string port = textBox2.Text.Trim();
            string user = textBox3.Text.Trim();
            string pass = textBox4.Text.Trim();
            string newDbName = textBox5.Text.Trim();

            if (string.IsNullOrWhiteSpace(newDbName))
            {
                MessageBox.Show("Database name cannot be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DbInitializer.CreateDatabase(host, port, user, pass, newDbName);
                DbInitializer.CreateTables(host, port, user, pass, newDbName);
                MessageBox.Show("✅ Database & tables created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
