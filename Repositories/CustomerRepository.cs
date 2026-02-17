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
                SELECT id, name
                FROM customers
                WHERE deleted_at IS NULL
                ORDER BY name ASC
            ", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new CustomerDto
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString()
                });
            }

            return result;
        }

        // =========================
        // INSERT CUSTOMER
        // =========================
        public int Insert(string name, int? createdBy)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var tran = conn.BeginTransaction();

            try
            {
                using var cmd = new NpgsqlCommand(@"
                    INSERT INTO customers (name, created_by, created_at)
                    VALUES (@name, @createdBy, NOW())
                    RETURNING id
                ", conn, tran);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@createdBy",
                    createdBy ?? (object)DBNull.Value);

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
