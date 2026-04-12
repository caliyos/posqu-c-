using Npgsql;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Controllers
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }

        public int Ord { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

        class UnitController
        {
            // =========================
            // GET ALL UNITS
            // =========================
            public List<Unit> GetUnits()
            {
                var list = new List<Unit>();

                string sql = "SELECT * FROM units ORDER BY id DESC";

                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var ordIndex = reader.GetOrdinal("ord");

                list.Add(new Unit
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Abbr = reader.GetString(reader.GetOrdinal("abbr")),
                    Ord = reader.IsDBNull(ordIndex) ? 0 : reader.GetInt32(ordIndex),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                });
            }

            return list;
            }

            // =========================
            // ADD UNIT
            // =========================
            public bool AddUnit(Unit unit)
            {
                string sql = @"
                INSERT INTO units (name, abbr,ord)
                VALUES (@name, @abbr, @ord)";

                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", unit.Name);
                cmd.Parameters.AddWithValue("@abbr", unit.Abbr);
                cmd.Parameters.AddWithValue("@ord", unit.Ord);

            return cmd.ExecuteNonQuery() > 0;
            }

            // =========================
            // UPDATE UNIT
            // =========================
            public bool UpdateUnit(Unit unit)
            {
                string sql = @"
                UPDATE units
                SET name = @name,
                    abbr = @abbr,
                    ord = @ord
                WHERE id = @id";

                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", unit.Id);
                cmd.Parameters.AddWithValue("@name", unit.Name);
                cmd.Parameters.AddWithValue("@abbr", unit.Abbr);
                cmd.Parameters.AddWithValue("@ord", unit.Ord);

            return cmd.ExecuteNonQuery() > 0;
            }

            // =========================
            // DELETE UNIT
            // =========================
            public bool DeleteUnit(int id)
            {
                string sql = "DELETE FROM units WHERE id = @id";

                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }

            // =========================
            // SEARCH (optional tapi kepake)
            // =========================
            public List<Unit> SearchUnits(string keyword)
            {
                var list = new List<Unit>();

                string sql = @"
                SELECT * FROM units
                WHERE LOWER(name) LIKE LOWER(@key)
                   OR LOWER(abbr) LIKE LOWER(@key)
                ORDER BY name";

                using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
                conn.Open();

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@key", "%" + keyword + "%");

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Unit
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Abbr = reader.GetString(reader.GetOrdinal("abbr")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                    });
                }

                return list;
            }
        }
    }
