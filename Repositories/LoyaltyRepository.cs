using System;
using Npgsql;

namespace POS_qu.Repositories
{
    public class LoyaltyRepository
    {
        private static bool HasTable(NpgsqlConnection con, NpgsqlTransaction tran, string tableName)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.tables
WHERE table_schema = current_schema()
  AND table_name = @t
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@t", tableName);
            var obj = cmd.ExecuteScalar();
            return obj != null && obj != DBNull.Value;
        }

        public bool IsLoyaltyInstalled(NpgsqlConnection con, NpgsqlTransaction tran)
        {
            return HasTable(con, tran, "loyalty_accounts") && HasTable(con, tran, "loyalty_transactions");
        }

        public (long AccountId, int MembershipLevelId, string MembershipCode, decimal EarnMultiplier) GetOrCreateAccountForCustomer(
            NpgsqlConnection con,
            NpgsqlTransaction tran,
            int customerId)
        {
            using (var ins = new NpgsqlCommand(@"
INSERT INTO loyalty_accounts(customer_id, membership_level_id, point_balance, updated_at)
SELECT c.id, c.membership_level_id, COALESCE(c.points,0), now()
FROM customers c
WHERE c.id = @cid
ON CONFLICT (customer_id) DO NOTHING
", con, tran))
            {
                ins.Parameters.AddWithValue("@cid", customerId);
                ins.ExecuteNonQuery();
            }

            using (var lockCmd = new NpgsqlCommand(@"
SELECT
    la.id AS account_id,
    COALESCE(la.membership_level_id, c.membership_level_id, 0) AS membership_level_id,
    COALESCE(ml.code, 'SILVER') AS membership_code,
    COALESCE(ml.earn_multiplier, 1) AS earn_multiplier
FROM loyalty_accounts la
JOIN customers c ON c.id = la.customer_id
LEFT JOIN membership_levels ml ON ml.id = COALESCE(la.membership_level_id, c.membership_level_id)
WHERE la.customer_id = @cid
FOR UPDATE
", con, tran))
            {
                lockCmd.Parameters.AddWithValue("@cid", customerId);
                using var r = lockCmd.ExecuteReader();
                if (!r.Read())
                    throw new InvalidOperationException("Loyalty account tidak ditemukan.");
                long accountId = Convert.ToInt64(r["account_id"]);
                int levelId = r["membership_level_id"] != DBNull.Value ? Convert.ToInt32(r["membership_level_id"]) : 0;
                string code = r["membership_code"]?.ToString() ?? "SILVER";
                decimal mult = r["earn_multiplier"] != DBNull.Value ? Convert.ToDecimal(r["earn_multiplier"]) : 1m;
                return (accountId, levelId, code, mult);
            }
        }

        public (decimal SpendAmount, int EarnPoints, int ExpiryMonths, decimal MinSpendPerTxn, int? MaxPointsPerTxn) GetPointRule(
            NpgsqlConnection con,
            NpgsqlTransaction tran,
            int membershipLevelId)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT spend_amount, earn_points, expiry_months, min_spend_per_txn, max_points_per_txn
FROM point_rules
WHERE membership_level_id = @mid
  AND COALESCE(is_active, TRUE) = TRUE
ORDER BY id DESC
LIMIT 1
", con, tran);
            cmd.Parameters.AddWithValue("@mid", membershipLevelId);
            using var r = cmd.ExecuteReader();
            if (!r.Read())
                return (10000m, 1, 12, 0m, null);
            decimal spend = r["spend_amount"] != DBNull.Value ? Convert.ToDecimal(r["spend_amount"]) : 10000m;
            int earn = r["earn_points"] != DBNull.Value ? Convert.ToInt32(r["earn_points"]) : 1;
            int expiry = r["expiry_months"] != DBNull.Value ? Convert.ToInt32(r["expiry_months"]) : 12;
            decimal minSpend = r["min_spend_per_txn"] != DBNull.Value ? Convert.ToDecimal(r["min_spend_per_txn"]) : 0m;
            int? maxPts = r["max_points_per_txn"] != DBNull.Value ? Convert.ToInt32(r["max_points_per_txn"]) : (int?)null;
            if (spend <= 0m) spend = 10000m;
            if (earn <= 0) earn = 1;
            if (expiry <= 0) expiry = 12;
            if (minSpend < 0m) minSpend = 0m;
            return (spend, earn, expiry, minSpend, maxPts);
        }

        public int GetAccountBalanceForUpdate(NpgsqlConnection con, NpgsqlTransaction tran, long accountId)
        {
            using var cmd = new NpgsqlCommand(@"
SELECT point_balance
FROM loyalty_accounts
WHERE id = @id
FOR UPDATE
", con, tran);
            cmd.Parameters.AddWithValue("@id", accountId);
            var obj = cmd.ExecuteScalar();
            return obj != null && obj != DBNull.Value ? Convert.ToInt32(obj) : 0;
        }

        public long InsertLoyaltyTransaction(
            NpgsqlConnection con,
            NpgsqlTransaction tran,
            long accountId,
            string txType,
            string idempotencyKey,
            string? refType,
            long? refId,
            string? invoiceNumber,
            int? warehouseId,
            int? terminalId,
            int? cashierUserId,
            int? loginId,
            int pointsBefore,
            int pointsChange,
            int pointsAfter,
            decimal moneyAmount,
            string? reason)
        {
            using var cmd = new NpgsqlCommand(@"
INSERT INTO loyalty_transactions
(account_id, tx_type, idempotency_key, ref_type, ref_id, invoice_number, warehouse_id, terminal_id, cashier_user_id, login_id,
 points_before, points_change, points_after, money_amount, reason, created_at)
VALUES
(@aid, @t, @k, @rt, @rid, @inv, @wh, @term, @cashier, @login,
 @pb, @pc, @pa, @amt, @reason, now())
RETURNING id
", con, tran);
            cmd.Parameters.AddWithValue("@aid", accountId);
            cmd.Parameters.AddWithValue("@t", (txType ?? "").Trim().ToUpperInvariant());
            cmd.Parameters.AddWithValue("@k", (idempotencyKey ?? "").Trim());
            cmd.Parameters.AddWithValue("@rt", string.IsNullOrWhiteSpace(refType) ? (object)DBNull.Value : refType.Trim().ToUpperInvariant());
            cmd.Parameters.AddWithValue("@rid", refId.HasValue ? (object)refId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@inv", string.IsNullOrWhiteSpace(invoiceNumber) ? (object)DBNull.Value : invoiceNumber.Trim());
            cmd.Parameters.AddWithValue("@wh", warehouseId.HasValue && warehouseId.Value > 0 ? (object)warehouseId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@term", terminalId.HasValue && terminalId.Value > 0 ? (object)terminalId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@cashier", cashierUserId.HasValue && cashierUserId.Value > 0 ? (object)cashierUserId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@login", loginId.HasValue && loginId.Value > 0 ? (object)loginId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@pb", pointsBefore);
            cmd.Parameters.AddWithValue("@pc", pointsChange);
            cmd.Parameters.AddWithValue("@pa", pointsAfter);
            cmd.Parameters.AddWithValue("@amt", moneyAmount);
            cmd.Parameters.AddWithValue("@reason", string.IsNullOrWhiteSpace(reason) ? (object)DBNull.Value : reason);
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        public void InsertPointBucket(
            NpgsqlConnection con,
            NpgsqlTransaction tran,
            long accountId,
            long earnedTxId,
            int pointsEarned,
            DateTime? expiresAt)
        {
            using var cmd = new NpgsqlCommand(@"
INSERT INTO loyalty_point_buckets(account_id, earned_tx_id, points_earned, points_used, expires_at, created_at)
VALUES(@aid, @tx, @pe, 0, @exp, now())
", con, tran);
            cmd.Parameters.AddWithValue("@aid", accountId);
            cmd.Parameters.AddWithValue("@tx", earnedTxId);
            cmd.Parameters.AddWithValue("@pe", pointsEarned);
            cmd.Parameters.AddWithValue("@exp", expiresAt.HasValue ? (object)expiresAt.Value.Date : DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public void UpdateAccountAfterEarn(
            NpgsqlConnection con,
            NpgsqlTransaction tran,
            long accountId,
            int pointsEarned,
            decimal spendAmount)
        {
            using var cmd = new NpgsqlCommand(@"
UPDATE loyalty_accounts
SET point_balance = point_balance + @p,
    lifetime_points_earned = lifetime_points_earned + @p,
    lifetime_spend = lifetime_spend + @amt,
    lifetime_txn = lifetime_txn + 1,
    last_tx_at = now(),
    version = version + 1,
    updated_at = now()
WHERE id = @id
", con, tran);
            cmd.Parameters.AddWithValue("@id", accountId);
            cmd.Parameters.AddWithValue("@p", pointsEarned);
            cmd.Parameters.AddWithValue("@amt", spendAmount);
            cmd.ExecuteNonQuery();
        }

        public void UpdateCustomerLoyaltyCache(NpgsqlConnection con, NpgsqlTransaction tran, int customerId, int pointBalance, int? membershipLevelId)
        {
            using var cmd = new NpgsqlCommand(@"
UPDATE customers
SET points = @p,
    membership_level_id = COALESCE(@mid, membership_level_id),
    updated_at = now()
WHERE id = @cid
", con, tran);
            cmd.Parameters.AddWithValue("@cid", customerId);
            cmd.Parameters.AddWithValue("@p", pointBalance);
            cmd.Parameters.AddWithValue("@mid", membershipLevelId.HasValue && membershipLevelId.Value > 0 ? (object)membershipLevelId.Value : DBNull.Value);
            cmd.ExecuteNonQuery();
        }
    }
}
