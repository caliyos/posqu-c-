using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data;
using Npgsql;
using POS_qu.Models;
using POS_qu.Helpers;

namespace POS_qu.Controllers
{
    class SettingController
    {
       

        public DataRow GetSettingToko()
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string sql = "SELECT * FROM settingtoko LIMIT 1"; // ambil 1 data saja

            using var cmd = new NpgsqlCommand(sql, conn);
            using var adapter = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

        public void UpdateSettingToko(string nama, string alamat, string npwp, byte[] logoBytes)
        {
            string query = "UPDATE settingtoko SET nama = @nama, alamat = @alamat, npwp = @npwp, logo = @logo WHERE id = 1";

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@alamat", alamat);
                    cmd.Parameters.AddWithValue("@npwp", npwp);
                    cmd.Parameters.AddWithValue("@logo", logoBytes ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataRow GetStrukSetting()
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM struk_setting LIMIT 1", conn))
                using (var da = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0];
                    return null;
                }
            }
        }

        public void SaveStrukSetting(StrukSettingModel model)
        {
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();

                // Cek apakah ada data
                using (var checkCmd = new NpgsqlCommand("SELECT COUNT(*) FROM struk_setting", conn))
                {
                    long count = (long)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Update
                        using (var cmd = new NpgsqlCommand(@"
                        UPDATE struk_setting 
                        SET judul = @judul, alamat = @alamat, telepon = @telepon, footer = @footer, logo = @logo, updated_at = NOW()", conn))
                        {
                            cmd.Parameters.AddWithValue("@judul", model.Judul ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@alamat", model.Alamat ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@telepon", model.NomorTelepon ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@footer", model.Footer ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@logo", model.LogoBytes ?? (object)DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert
                        using (var cmd = new NpgsqlCommand(@"
                        INSERT INTO struk_setting (judul, alamat, telepon, footer, logo, updated_at) 
                        VALUES (@judul, @alamat, @telepon, @footer, @logo, NOW())", conn))
                        {
                            cmd.Parameters.AddWithValue("@judul", model.Judul ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@alamat", model.Alamat ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@telepon", model.NomorTelepon ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@footer", model.Footer ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@logo", model.LogoBytes ?? (object)DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public void UpdateVisibilitySetting(string settingKey, bool isVisible)
        {
            string query = "UPDATE struk_setting SET " + settingKey + " = @isVisible, updated_at = NOW() WHERE id = 1";

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@isVisible", isVisible);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public string GetNamaToko()
        {
            string sql = "SELECT nama FROM settingtoko LIMIT 1";
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : "";
                }
            }
        }
        public string GetNamaTokoDariStrukSetting()
        {
            string sql = "SELECT judul FROM struk_setting LIMIT 1";
            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : "";
                }
            }
        }

        
        public void UpdateNamaTokoManual(string namaManual)
        {
            string sql = @"
        UPDATE struk_setting 
        SET judul = @namaManual, updated_at = NOW()
        WHERE id = 1; -- asumsikan data hanya 1 baris setting
    ";

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("namaManual", namaManual);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        // Jika belum ada row, insert
                        string insertSql = @"
                    INSERT INTO struk_setting (id, judul, updated_at)
                    VALUES (1, @namaManual, NOW())
                ";
                        using (var insertCmd = new NpgsqlCommand(insertSql, conn))
                        {
                            insertCmd.Parameters.AddWithValue("namaManual", namaManual);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public void UpdateJudul(string judul)
        {
            string sql = @"
        UPDATE struk_setting
        SET judul = @judul, updated_at = NOW()
        WHERE id = 1;
    ";

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("judul", judul);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        string insertSql = @"
                    INSERT INTO struk_setting (id, judul, updated_at)
                    VALUES (1, @judul, NOW())
                ";
                        using (var insertCmd = new NpgsqlCommand(insertSql, conn))
                        {
                            insertCmd.Parameters.AddWithValue("judul", judul);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }






    }
}               
