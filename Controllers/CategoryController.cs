using Npgsql;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_qu.Models;

namespace POS_qu.Controllers
{
    class CategoryController
    {

        public List<Category> GetCategoryHierarchy()
        {
            var allCategories = GetCategories(); // ambil semua kategori flat
            var lookup = allCategories.ToDictionary(c => c.Id, c => c);

            List<Category> roots = new List<Category>();

            foreach (var cat in allCategories)
            {
                if (cat.ParentId.HasValue)
                {
                    if (lookup.ContainsKey(cat.ParentId.Value))
                        lookup[cat.ParentId.Value].Children.Add(cat);
                }
                else
                {
                    roots.Add(cat);
                }
            }

            return roots; // hanya root kategori, sudah ada Children di dalamnya
        }


        // Ambil semua kategori
        public List<Category> GetCategories()
        {
            var list = new List<Category>();

            string sql = "SELECT * FROM categories ORDER BY id";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Category
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Kode = reader.GetString(reader.GetOrdinal("kode")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                    ParentId = reader.IsDBNull(reader.GetOrdinal("parent_id")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("parent_id")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                });
            }

            return list;
        }

        // Tambah kategori
        public bool AddCategory(Category cat)
        {
            string sql = @"
                INSERT INTO categories(name, kode, description, parent_id) 
                VALUES(@name, @kode, @desc, @parentId)";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", cat.Name);
            cmd.Parameters.AddWithValue("@kode", cat.Kode);
            cmd.Parameters.AddWithValue("@desc", string.IsNullOrEmpty(cat.Description) ? (object)DBNull.Value : cat.Description);
            cmd.Parameters.AddWithValue("@parentId", cat.ParentId.HasValue ? (object)cat.ParentId.Value : DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        // Update kategori
        public bool UpdateCategory(Category cat)
        {
            string sql = @"
                UPDATE categories 
                SET name=@name, kode=@kode, description=@desc, parent_id=@parentId, updated_at=NOW()
                WHERE id=@id";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", cat.Id);
            cmd.Parameters.AddWithValue("@name", cat.Name);
            cmd.Parameters.AddWithValue("@kode", cat.Kode);
            cmd.Parameters.AddWithValue("@desc", string.IsNullOrEmpty(cat.Description) ? (object)DBNull.Value : cat.Description);
            cmd.Parameters.AddWithValue("@parentId", cat.ParentId.HasValue ? (object)cat.ParentId.Value : DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        // Hapus kategori
        public bool DeleteCategory(int id)
        {
            string sql = "DELETE FROM categories WHERE id=@id";

            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    
}
}
