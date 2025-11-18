using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Controllers
{
    public class SupplierController
    {
        public List<Supplier> GetSuppliers()
        {
            var list = new List<Supplier>();
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string sql = "SELECT * FROM suppliers ORDER BY name";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Supplier
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Kode = reader.IsDBNull(reader.GetOrdinal("kode")) ? "" : reader.GetString(reader.GetOrdinal("kode")),
                    ContactName = reader.IsDBNull(reader.GetOrdinal("contact_name")) ? "" : reader.GetString(reader.GetOrdinal("contact_name")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString(reader.GetOrdinal("phone")),
                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader.GetString(reader.GetOrdinal("email")),
                    Address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString(reader.GetOrdinal("address")),
                    Note = reader.IsDBNull(reader.GetOrdinal("note")) ? "" : reader.GetString(reader.GetOrdinal("note"))
                });
            }

            return list;
        }

        public bool AddSupplier(Supplier supplier)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string sql = @"
                INSERT INTO suppliers (name, kode, contact_name, phone, email, address, note)
                VALUES (@name, @kode, @contact_name, @phone, @email, @address, @note)
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", supplier.Name);
            cmd.Parameters.AddWithValue("@kode", supplier.Kode);
            cmd.Parameters.AddWithValue("@contact_name", (object)supplier.ContactName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object)supplier.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)supplier.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object)supplier.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@note", (object)supplier.Note ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string sql = @"
                UPDATE suppliers
                SET name=@name, kode=@kode, contact_name=@contact_name, phone=@phone, email=@email, address=@address, note=@note, updated_at=NOW()
                WHERE id=@id
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", supplier.Id);
            cmd.Parameters.AddWithValue("@name", supplier.Name);
            cmd.Parameters.AddWithValue("@kode", supplier.Kode);
            cmd.Parameters.AddWithValue("@contact_name", (object)supplier.ContactName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object)supplier.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)supplier.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object)supplier.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@note", (object)supplier.Note ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteSupplier(int id)
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();

            string sql = "DELETE FROM suppliers WHERE id=@id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
