using Npgsql;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace POS_qu.Controllers
{
    public class Customer
    {
        public int Id { get; set; }

        // identity
        public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        // membership system
        public bool IsMember { get; set; } = true;
        public string? MemberCode { get; set; }

        // pricing level (FK price_levels)
        public int? PriceLevelId { get; set; }

        // loyalty system
        public int Points { get; set; } = 0;

        // note
        public string? Note { get; set; }

        // system tracking
        public int? CreatedBy { get; set; }
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

        // =========================
        // ADD CUSTOMER (FULL FIELD)
        // =========================
        public bool AddCustomer(Customer customer)
        {
            string sql = @"
            INSERT INTO customers 
            (name, phone, email, address, is_member, member_code, note, created_by, price_level_id)
            VALUES 
            (@name, @phone, @email, @address, @is_member, @member_code, @note, @created_by, @price_level_id)";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@name", customer.Name);
            cmd.Parameters.AddWithValue("@phone", (object)customer.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)customer.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object)customer.Address ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@is_member", customer.IsMember);

            cmd.Parameters.AddWithValue("@member_code", (object)customer.MemberCode ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@note", (object)customer.Note ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@created_by", (object)customer.CreatedBy ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@price_level_id",
                (object)customer.PriceLevelId ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        // =========================
        // GET PRICE LEVELS (NO MODEL DTO)
        // =========================
        public List<KeyValuePair<int, string>> GetPriceLevels()
        {
            var list = new List<KeyValuePair<int, string>>();

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT id, name FROM price_levels ORDER BY id", conn);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new KeyValuePair<int, string>(
                    reader.GetInt32(0),
                    reader.GetString(1)
                ));
            }

            return list;
        }

        // =========================
        // UPDATE CUSTOMER (FULL FIELD)
        // =========================
        public bool UpdateCustomer(Customer customer)
        {
            string sql = @"
            UPDATE customers
            SET name = @name,
                phone = @phone,
                email = @email,
                address = @address,
                is_member = @is_member,
                member_code = @member_code,
                note = @note,
                price_level_id = @price_level_id,
                updated_at = NOW()
            WHERE id = @id AND deleted_at IS NULL";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", customer.Id);
            cmd.Parameters.AddWithValue("@name", customer.Name);
            cmd.Parameters.AddWithValue("@phone", (object)customer.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)customer.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object)customer.Address ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@is_member", customer.IsMember);

            cmd.Parameters.AddWithValue("@member_code", (object)customer.MemberCode ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@note", (object)customer.Note ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@price_level_id",
                (object)customer.PriceLevelId ?? DBNull.Value);

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
