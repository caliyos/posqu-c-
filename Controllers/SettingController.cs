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
       
        public void EnsureSettingTokoSchema()
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
ALTER TABLE settingtoko ADD COLUMN IF NOT EXISTS is_pkp BOOLEAN NOT NULL DEFAULT FALSE;
ALTER TABLE settingtoko ADD COLUMN IF NOT EXISTS ppn_rate NUMERIC(6,2) NOT NULL DEFAULT 11;
ALTER TABLE settingtoko ADD COLUMN IF NOT EXISTS purchase_prefix VARCHAR(20) NOT NULL DEFAULT 'PB';
ALTER TABLE settingtoko ADD COLUMN IF NOT EXISTS purchase_last_date DATE NULL;
ALTER TABLE settingtoko ADD COLUMN IF NOT EXISTS purchase_last_number INT NOT NULL DEFAULT 0;
ALTER TABLE settingtoko ADD COLUMN IF NOT EXISTS default_hpp_method VARCHAR(20) NOT NULL DEFAULT 'FIFO';
", conn);
            cmd.ExecuteNonQuery();
        }

        public DataRow GetSettingToko()
        {
            EnsureSettingTokoSchema();
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

        public void UpdateSettingToko(
            string nama,
            string alamat,
            string npwp,
            byte[] logoBytes,
            bool? isPkp = null,
            decimal? ppnRate = null,
            string purchasePrefix = null)
        {
            EnsureSettingTokoSchema();
            string query = @"
UPDATE settingtoko
SET nama = @nama,
    alamat = @alamat,
    npwp = @npwp,
    logo = @logo,
    is_pkp = COALESCE(@is_pkp, is_pkp),
    ppn_rate = COALESCE(@ppn_rate, ppn_rate),
    purchase_prefix = COALESCE(@purchase_prefix, purchase_prefix)
WHERE id = 1";

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@alamat", alamat);
                    cmd.Parameters.AddWithValue("@npwp", npwp);
                    cmd.Parameters.AddWithValue("@logo", logoBytes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_pkp", (object?)isPkp ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ppn_rate", (object?)ppnRate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@purchase_prefix", (object?)purchasePrefix ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public (bool IsPkp, decimal PpnRate, string PurchasePrefix) GetPurchaseAndTaxConfig()
        {
            EnsureSettingTokoSchema();
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT COALESCE(is_pkp, FALSE) AS is_pkp,
       COALESCE(ppn_rate, 11) AS ppn_rate,
       COALESCE(purchase_prefix, 'PB') AS purchase_prefix
FROM settingtoko
WHERE id = 1
LIMIT 1
", conn);
            using var r = cmd.ExecuteReader();
            if (!r.Read())
                return (false, 11m, "PB");

            var isPkp = r["is_pkp"] != DBNull.Value && Convert.ToBoolean(r["is_pkp"]);
            var rate = r["ppn_rate"] != DBNull.Value ? Convert.ToDecimal(r["ppn_rate"]) : 11m;
            var prefix = r["purchase_prefix"]?.ToString() ?? "PB";
            return (isPkp, rate, prefix);
        }

        public string GetDefaultHppMethod()
        {
            EnsureSettingTokoSchema();
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT COALESCE(NULLIF(default_hpp_method,''),'FIFO') AS m
FROM settingtoko
WHERE id = 1
LIMIT 1
", conn);
            var res = cmd.ExecuteScalar();
            return res != null && res != DBNull.Value ? res.ToString() : "FIFO";
        }

        public void SetDefaultHppMethod(string method)
        {
            EnsureSettingTokoSchema();
            method = (method ?? "FIFO").Trim().ToUpperInvariant();
            if (method != "FIFO" && method != "AVG" && method != "LIFO" && method != "FEFO")
                method = "FIFO";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE settingtoko SET default_hpp_method = @m WHERE id = 1", conn);
            cmd.Parameters.AddWithValue("@m", method);
            cmd.ExecuteNonQuery();
        }

        public string PeekNextPurchaseNumber()
        {
            EnsureSettingTokoSchema();
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT purchase_prefix, purchase_last_date, purchase_last_number
FROM settingtoko
WHERE id = 1
LIMIT 1
", conn);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return "PB-" + DateTime.Today.ToString("yyyyMMdd") + "-0001";

            var prefix = r["purchase_prefix"]?.ToString() ?? "PB";
            var lastDate = r["purchase_last_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["purchase_last_date"]);
            var lastNo = r["purchase_last_number"] == DBNull.Value ? 0 : Convert.ToInt32(r["purchase_last_number"]);

            int nextNo = (lastDate.HasValue && lastDate.Value.Date == DateTime.Today) ? lastNo + 1 : 1;
            return $"{prefix}-{DateTime.Today:yyyyMMdd}-{nextNo:D4}";
        }

        public string GeneratePurchaseNumber(NpgsqlConnection conn, NpgsqlTransaction tran)
        {
            EnsureSettingTokoSchema();
            using var cmdSel = new NpgsqlCommand(@"
SELECT purchase_prefix, purchase_last_date, purchase_last_number
FROM settingtoko
WHERE id = 1
FOR UPDATE
", conn, tran);
            using var r = cmdSel.ExecuteReader();
            if (!r.Read())
                throw new InvalidOperationException("Setting toko tidak ditemukan.");

            var prefix = r["purchase_prefix"]?.ToString() ?? "PB";
            var lastDate = r["purchase_last_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["purchase_last_date"]);
            var lastNo = r["purchase_last_number"] == DBNull.Value ? 0 : Convert.ToInt32(r["purchase_last_number"]);
            r.Close();

            int nextNo = (lastDate.HasValue && lastDate.Value.Date == DateTime.Today) ? lastNo + 1 : 1;

            using var cmdUpd = new NpgsqlCommand(@"
UPDATE settingtoko
SET purchase_last_date = @d,
    purchase_last_number = @n
WHERE id = 1
", conn, tran);
            cmdUpd.Parameters.AddWithValue("@d", DateTime.Today);
            cmdUpd.Parameters.AddWithValue("@n", nextNo);
            cmdUpd.ExecuteNonQuery();

            return $"{prefix}-{DateTime.Today:yyyyMMdd}-{nextNo:D4}";
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
