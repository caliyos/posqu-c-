using Npgsql;
using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Data;

namespace POS_qu.Services
{
    public sealed class DocumentNumberingService
    {
        public void EnsureSeedDefaults(NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand(@"
INSERT INTO document_numberings (doc_type, prefix, last_date, last_number, pad_length, format)
VALUES
 ('SALE', 'TRX', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
 ('ORDER', 'ORD', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
 ('PO', 'PO', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
 ('PURCHASE', 'PB', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
 ('STOCK_OPNAME', 'SO', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}'),
 ('PURCHASE_RETURN', 'RB', NULL, 0, 4, '{prefix}-{yyyyMMdd}-{seq}')
ON CONFLICT (doc_type) DO NOTHING;
", con, tran);
            cmd.ExecuteNonQuery();
        }

        public DataTable GetAll()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            EnsureSeedDefaults(con, tran);
            tran.Commit();

            using var da = new NpgsqlDataAdapter(@"
SELECT doc_type, prefix, pad_length, format, last_date, last_number, updated_at
FROM document_numberings
ORDER BY doc_type ASC
", con);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void UpdateConfig(string docType, string prefix, int padLength, string format)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            EnsureSeedDefaults(con, tran);

            using var cmd = new NpgsqlCommand(@"
UPDATE document_numberings
SET prefix = @p,
    pad_length = @pad,
    format = @fmt,
    updated_at = NOW()
WHERE doc_type = @t
", con, tran);
            cmd.Parameters.AddWithValue("@t", docType);
            cmd.Parameters.AddWithValue("@p", prefix);
            cmd.Parameters.AddWithValue("@pad", padLength);
            cmd.Parameters.AddWithValue("@fmt", format);
            cmd.ExecuteNonQuery();

            tran.Commit();
        }

        public string PeekNext(string docType)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var tran = con.BeginTransaction();
            EnsureSeedDefaults(con, tran);

            using var cmd = new NpgsqlCommand(@"
SELECT prefix, last_date, last_number, pad_length, format
FROM document_numberings
WHERE doc_type = @t
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@t", docType);
            using var r = cmd.ExecuteReader();
            if (!r.Read())
                return docType + "-" + DateTime.Today.ToString("yyyyMMdd") + "-0001";

            var prefix = r["prefix"]?.ToString() ?? docType;
            var lastDate = r["last_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["last_date"]);
            var lastNo = r["last_number"] == DBNull.Value ? 0 : Convert.ToInt32(r["last_number"]);
            var pad = r["pad_length"] == DBNull.Value ? 4 : Convert.ToInt32(r["pad_length"]);
            var fmt = r["format"]?.ToString() ?? "{prefix}-{yyyyMMdd}-{seq}";
            r.Close();
            tran.Commit();

            int nextNo = (lastDate.HasValue && lastDate.Value.Date == DateTime.Today) ? lastNo + 1 : 1;
            return Format(prefix, DateTime.Today, nextNo, pad, fmt);
        }

        public string Generate(string docType, DateTime date, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            EnsureSeedDefaults(con, tran);

            using var cmdSel = new NpgsqlCommand(@"
SELECT prefix, last_date, last_number, pad_length, format
FROM document_numberings
WHERE doc_type = @t
FOR UPDATE
", con, tran);
            cmdSel.Parameters.AddWithValue("@t", docType);
            using var r = cmdSel.ExecuteReader();
            if (!r.Read())
                throw new InvalidOperationException("Doc type tidak ditemukan: " + docType);

            var prefix = r["prefix"]?.ToString() ?? docType;
            var lastDate = r["last_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["last_date"]);
            var lastNo = r["last_number"] == DBNull.Value ? 0 : Convert.ToInt32(r["last_number"]);
            var pad = r["pad_length"] == DBNull.Value ? 4 : Convert.ToInt32(r["pad_length"]);
            var fmt = r["format"]?.ToString() ?? "{prefix}-{yyyyMMdd}-{seq}";
            r.Close();

            if (!lastDate.HasValue || lastDate.Value.Date != date.Date)
                lastNo = 0;
            int nextNo = lastNo + 1;

            using var cmdUpd = new NpgsqlCommand(@"
UPDATE document_numberings
SET last_date = @d,
    last_number = @n,
    updated_at = NOW()
WHERE doc_type = @t
", con, tran);
            cmdUpd.Parameters.AddWithValue("@t", docType);
            cmdUpd.Parameters.AddWithValue("@d", date.Date);
            cmdUpd.Parameters.AddWithValue("@n", nextNo);
            cmdUpd.ExecuteNonQuery();

            return Format(prefix, date.Date, nextNo, pad, fmt);
        }

        private string Format(string prefix, DateTime date, int seq, int padLength, string fmt)
        {
            var seqStr = seq.ToString("D" + Math.Max(1, padLength));
            return fmt
                .Replace("{prefix}", prefix ?? "")
                .Replace("{yyyyMMdd}", date.ToString("yyyyMMdd"))
                .Replace("{seq}", seqStr);
        }
    }
}
