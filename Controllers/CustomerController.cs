using Npgsql;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;

namespace POS_qu.Controllers
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string? Note { get; set; }
        public int? CreatedBy { get; set; }
        public int? PriceLevelId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    class CustomerController
    {
        // =========================
        // GET ALL CUSTOMERS
        // =========================
        public List<Customer> GetCustomers()
        {
            var list = new List<Customer>();
            string sql = "SELECT * FROM customers WHERE deleted_at IS NULL ORDER BY id DESC";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Customer
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                    Note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString(reader.GetOrdinal("note")),
                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("created_by")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("created_by")),
                    PriceLevelId = reader.IsDBNull(reader.GetOrdinal("price_level_id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("price_level_id")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("deleted_at"))
                });
            }

            return list;
        }

        // =========================
        // ADD CUSTOMER
        // =========================
        public bool AddCustomer(Customer customer)
        {
            string sql = @"
                INSERT INTO customers (name, phone, note, created_by, price_level_id)
                VALUES (@name, @phone, @note, @created_by, @price_level_id)";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", customer.Name);
            cmd.Parameters.AddWithValue("@phone", (object)customer.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@note", (object)customer.Note ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@created_by", (object)customer.CreatedBy ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@price_level_id", (object)customer.PriceLevelId ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        // =========================
        // UPDATE CUSTOMER
        // =========================
        public bool UpdateCustomer(Customer customer)
        {
            string sql = @"
                UPDATE customers
                SET name = @name,
                    phone = @phone,
                    note = @note,
                    price_level_id = @price_level_id
                WHERE id = @id AND deleted_at IS NULL";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", customer.Id);
            cmd.Parameters.AddWithValue("@name", customer.Name);
            cmd.Parameters.AddWithValue("@phone", (object)customer.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@note", (object)customer.Note ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@price_level_id", (object)customer.PriceLevelId ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        // =========================
        // DELETE CUSTOMER (soft delete)
        // =========================
        public bool DeleteCustomer(int id)
        {
            string sql = "UPDATE customers SET deleted_at = CURRENT_TIMESTAMP WHERE id = @id AND deleted_at IS NULL";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        // =========================
        // SEARCH CUSTOMER
        // =========================
        public List<Customer> SearchCustomers(string keyword)
        {
            var list = new List<Customer>();
            string sql = @"
                SELECT * FROM customers
                WHERE deleted_at IS NULL AND (LOWER(name) LIKE LOWER(@key) OR LOWER(phone) LIKE LOWER(@key))
                ORDER BY name";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@key", "%" + keyword + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Customer
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                    Note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString(reader.GetOrdinal("note")),
                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("created_by")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("created_by")),
                    PriceLevelId = reader.IsDBNull(reader.GetOrdinal("price_level_id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("price_level_id")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("deleted_at"))
                });
            }

            return list;
        }
    }
}
