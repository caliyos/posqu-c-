using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using POS_qu.Models;
using System.Transactions;
using Microsoft.VisualBasic.Devices;

namespace POS_qu.Controllers
{
    class TransactionController
    {
        private string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";

        public DataTable GetSalesReport(DateTime start, DateTime end)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = "SELECT * FROM get_sales_report(@start, @end);";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, vCon))
                {
                    cmd.Parameters.AddWithValue("start", NpgsqlTypes.NpgsqlDbType.Date, start.Date);
                    cmd.Parameters.AddWithValue("end", NpgsqlTypes.NpgsqlDbType.Date, end.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public DataTable GetTopProducts(DateTime start, DateTime end)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection vCon = new NpgsqlConnection(vStrConnection))
            {
                vCon.Open();
                string sql = "SELECT * FROM get_top_products(@start, @end);";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, vCon))
                {
                    cmd.Parameters.AddWithValue("start", NpgsqlTypes.NpgsqlDbType.Date, start.Date);
                    cmd.Parameters.AddWithValue("end", NpgsqlTypes.NpgsqlDbType.Date, end.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }


    }

}


