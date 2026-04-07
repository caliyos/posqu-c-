using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace POS_qu.Controllers
{
    public class WarehouseController
    {
        public DataTable GetWarehouses()
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                string sql = "SELECT id, name, type, is_active FROM warehouses ORDER BY id ASC";
                using (var cmd = new NpgsqlCommand(sql, con))
                using (var da = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public void InsertWarehouse(Warehouse warehouse)
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                string sql = "INSERT INTO warehouses (name, type, is_active) VALUES (@name, @type, @is_active)";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@name", warehouse.Name);
                    cmd.Parameters.AddWithValue("@type", warehouse.Type ?? "store");
                    cmd.Parameters.AddWithValue("@is_active", warehouse.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                string sql = "UPDATE warehouses SET name = @name, type = @type, is_active = @is_active WHERE id = @id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", warehouse.Id);
                    cmd.Parameters.AddWithValue("@name", warehouse.Name);
                    cmd.Parameters.AddWithValue("@type", warehouse.Type ?? "store");
                    cmd.Parameters.AddWithValue("@is_active", warehouse.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteWarehouse(int id)
        {
            using (var con = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                con.Open();
                string sql = "DELETE FROM warehouses WHERE id = @id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}