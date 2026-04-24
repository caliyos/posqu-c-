using Npgsql;
using POS_qu.Helpers;
using System;

namespace POS_qu.Services
{
    public sealed class JournalService
    {
        public void CreateJournalFromSale(int transactionId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            if (transactionId <= 0) throw new ArgumentOutOfRangeException(nameof(transactionId));
            EnsureTables(con, tran);

            DeleteJournalByReference("sale", transactionId, con, tran);

            var header = GetSaleHeader(transactionId, con, tran);
            decimal cogs = GetSaleCogs(transactionId, con, tran);

            int kasId = GetAccountIdByName("Kas", con, tran);
            int penjualanId = GetAccountIdByName("Penjualan", con, tran);
            int diskonId = GetAccountIdByName("Diskon", con, tran);
            int hppId = GetAccountIdByName("HPP", con, tran);
            int persediaanId = GetAccountIdByName("Persediaan", con, tran);

            decimal grossSales = header.Subtotal + header.GlobalDiscount + header.Delivery;
            decimal cashIn = header.GrandTotal;

            long journalId = InsertJournalEntry(
                header.CreatedAt,
                "sale",
                transactionId,
                $"Penjualan #{header.Numbering}",
                con,
                tran
            );

            InsertJournalDetail(journalId, kasId, debit: cashIn, credit: 0m, con, tran);
            if (header.GlobalDiscount > 0)
                InsertJournalDetail(journalId, diskonId, debit: header.GlobalDiscount, credit: 0m, con, tran);
            InsertJournalDetail(journalId, penjualanId, debit: 0m, credit: grossSales, con, tran);

            if (cogs > 0)
            {
                InsertJournalDetail(journalId, hppId, debit: cogs, credit: 0m, con, tran);
                InsertJournalDetail(journalId, persediaanId, debit: 0m, credit: cogs, con, tran);
            }
        }

        public void CreateJournalFromPurchase(int purchaseOrderId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            if (purchaseOrderId <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseOrderId));
            EnsureTables(con, tran);

            DeleteJournalByReference("purchase", purchaseOrderId, con, tran);

            var header = GetPurchaseHeader(purchaseOrderId, con, tran);

            int kasId = GetAccountIdByName("Kas", con, tran);
            int persediaanId = GetAccountIdByName("Persediaan", con, tran);

            long journalId = InsertJournalEntry(
                header.CreatedAt,
                "purchase",
                purchaseOrderId,
                $"Pembelian #{purchaseOrderId}",
                con,
                tran
            );

            InsertJournalDetail(journalId, persediaanId, debit: header.TotalAmount, credit: 0m, con, tran);
            InsertJournalDetail(journalId, kasId, debit: 0m, credit: header.TotalAmount, con, tran);
        }

        public void DeleteJournalForSale(int transactionId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            EnsureTables(con, tran);
            DeleteJournalByReference("sale", transactionId, con, tran);
        }

        private void EnsureTables(NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS journal_entries (
    id BIGSERIAL PRIMARY KEY,
    date TIMESTAMP NOT NULL,
    reference_type VARCHAR(30) NOT NULL,
    reference_id BIGINT NOT NULL,
    description TEXT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS uq_journal_entries_reference
ON journal_entries(reference_type, reference_id);

CREATE TABLE IF NOT EXISTS journal_details (
    id BIGSERIAL PRIMARY KEY,
    journal_entry_id BIGINT NOT NULL REFERENCES journal_entries(id) ON DELETE CASCADE,
    account_id BIGINT NOT NULL REFERENCES accounts(id) ON DELETE RESTRICT,
    debit NUMERIC(18,2) NOT NULL DEFAULT 0,
    credit NUMERIC(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT ck_journal_amount CHECK (
        debit >= 0 AND credit >= 0
        AND NOT (debit > 0 AND credit > 0)
        AND NOT (debit = 0 AND credit = 0)
    )
);

CREATE INDEX IF NOT EXISTS idx_journal_details_entry ON journal_details(journal_entry_id);
CREATE INDEX IF NOT EXISTS idx_journal_details_account ON journal_details(account_id);
", con, tran);
            cmd.ExecuteNonQuery();
        }

        private int GetAccountIdByName(string name, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand("SELECT id FROM accounts WHERE name = @n LIMIT 1", con, tran);
            cmd.Parameters.AddWithValue("@n", name);
            var obj = cmd.ExecuteScalar();
            if (obj == null || obj == DBNull.Value)
                throw new InvalidOperationException("Akun belum ada: " + name);
            return Convert.ToInt32(obj);
        }

        private void DeleteJournalByReference(string refType, int refId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand(@"
DELETE FROM journal_entries
WHERE reference_type = @t AND reference_id = @id
", con, tran);
            cmd.Parameters.AddWithValue("@t", refType);
            cmd.Parameters.AddWithValue("@id", refId);
            cmd.ExecuteNonQuery();
        }

        private long InsertJournalEntry(DateTime date, string refType, int refId, string description, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand(@"
INSERT INTO journal_entries (date, reference_type, reference_id, description, created_at)
VALUES (@d, @t, @rid, @desc, NOW())
RETURNING id
", con, tran);
            cmd.Parameters.AddWithValue("@d", date);
            cmd.Parameters.AddWithValue("@t", refType);
            cmd.Parameters.AddWithValue("@rid", refId);
            cmd.Parameters.AddWithValue("@desc", (object)description ?? DBNull.Value);
            var obj = cmd.ExecuteScalar();
            return obj != null ? Convert.ToInt64(obj) : 0;
        }

        private void InsertJournalDetail(long journalId, int accountId, decimal debit, decimal credit, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            if ((debit > 0m && credit > 0m) || (debit == 0m && credit == 0m))
                throw new InvalidOperationException("Debit/Credit invalid");

            using var cmd = new NpgsqlCommand(@"
INSERT INTO journal_details (journal_entry_id, account_id, debit, credit)
VALUES (@jid, @aid, @debit, @credit)
", con, tran);
            cmd.Parameters.AddWithValue("@jid", journalId);
            cmd.Parameters.AddWithValue("@aid", accountId);
            cmd.Parameters.AddWithValue("@debit", debit);
            cmd.Parameters.AddWithValue("@credit", credit);
            cmd.ExecuteNonQuery();
        }

        private (string Numbering, DateTime CreatedAt, decimal Subtotal, decimal GlobalDiscount, decimal Delivery, decimal GrandTotal) GetSaleHeader(int transactionId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT ts_numbering, created_at,
       COALESCE(ts_total,0) AS subtotal,
       COALESCE(ts_global_discount_amount,0) AS disc,
       COALESCE(ts_delivery_amount,0) AS delivery,
       COALESCE(ts_grand_total,0) AS grand
FROM transactions
WHERE ts_id = @id AND deleted_at IS NULL
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@id", transactionId);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) throw new InvalidOperationException("Transaksi tidak ditemukan");

            var numbering = r["ts_numbering"]?.ToString() ?? transactionId.ToString();
            var createdAt = r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now;
            var subtotal = r["subtotal"] != DBNull.Value ? Convert.ToDecimal(r["subtotal"]) : 0m;
            var disc = r["disc"] != DBNull.Value ? Convert.ToDecimal(r["disc"]) : 0m;
            var delivery = r["delivery"] != DBNull.Value ? Convert.ToDecimal(r["delivery"]) : 0m;
            var grand = r["grand"] != DBNull.Value ? Convert.ToDecimal(r["grand"]) : 0m;
            return (numbering, createdAt, subtotal, disc, delivery, grand);
        }

        private decimal GetSaleCogs(int transactionId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT COALESCE(SUM(
    COALESCE(tsd_buy_price,0) * COALESCE(tsd_quantity,0) * COALESCE(tsd_conversion_rate,1)
),0)
FROM transaction_details
WHERE ts_id = @id
", con, tran);
            cmd.Parameters.AddWithValue("@id", transactionId);
            var obj = cmd.ExecuteScalar();
            return obj == null || obj == DBNull.Value ? 0m : Convert.ToDecimal(obj);
        }

        private (DateTime CreatedAt, decimal TotalAmount) GetPurchaseHeader(int purchaseOrderId, NpgsqlConnection con, NpgsqlTransaction tran)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT COALESCE(created_at, NOW()) AS created_at, COALESCE(total_amount,0) AS total_amount
FROM purchase_orders
WHERE id = @id
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@id", purchaseOrderId);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) throw new InvalidOperationException("Pembelian tidak ditemukan");
            var createdAt = r["created_at"] != DBNull.Value ? Convert.ToDateTime(r["created_at"]) : DateTime.Now;
            var total = r["total_amount"] != DBNull.Value ? Convert.ToDecimal(r["total_amount"]) : 0m;
            return (createdAt, total);
        }
    }
}

