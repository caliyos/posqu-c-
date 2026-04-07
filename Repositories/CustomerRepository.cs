using Npgsql;
using POS_qu.DTO;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Repositories
{
    public class CustomerRepository
    {
        // =========================
        // GET ALL CUSTOMER
        // =========================
        public List<CustomerDto> GetAll()
        {
            var result = new List<CustomerDto>();

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                SELECT c.id, c.name, c.price_level_id, pl.name as price_level_name
                FROM customers c
                LEFT JOIN price_levels pl ON c.price_level_id = pl.id
                WHERE c.deleted_at IS NULL
                ORDER BY c.name ASC
            ", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new CustomerDto
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString(),
                    PriceLevelId = reader["price_level_id"] != DBNull.Value ? Convert.ToInt32(reader["price_level_id"]) : (int?)null,
                    PriceLevelName = reader["price_level_name"]?.ToString()
                });
            }

            return result;
        }

        // =========================
        // INSERT CUSTOMER
        // =========================
        public int Insert(string name, int? createdBy, int? priceLevelId = 1)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var tran = conn.BeginTransaction();

            try
            {
                using var cmd = new NpgsqlCommand(@"
                    INSERT INTO customers (name, created_by, price_level_id, created_at)
                    VALUES (@name, @createdBy, @priceLevelId, NOW())
                    RETURNING id
                ", conn, tran);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@createdBy", createdBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@priceLevelId", priceLevelId ?? (object)DBNull.Value);

                int newId = Convert.ToInt32(cmd.ExecuteScalar());

                tran.Commit();
                return newId;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // =========================
        // SOFT DELETE
        // =========================
        public void SoftDelete(int customerId)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var tran = conn.BeginTransaction();

            try
            {
                using var cmd = new NpgsqlCommand(@"
                    UPDATE customers
                    SET deleted_at = NOW()
                    WHERE id = @id
                ", conn, tran);

                cmd.Parameters.AddWithValue("@id", customerId);
                cmd.ExecuteNonQuery();

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }



    }
    }
