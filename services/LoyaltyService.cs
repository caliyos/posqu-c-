using System;
using Npgsql;
using POS_qu.Models;
using POS_qu.Repositories;

namespace POS_qu.Services
{
    public class LoyaltyService
    {
        private readonly LoyaltyRepository _repo = new LoyaltyRepository();

        public LoyaltyEarnResult ApplyEarnForSale(
            NpgsqlConnection con,
            NpgsqlTransaction tran,
            int? customerId,
            int transactionId,
            string invoiceNumber,
            decimal eligibleSpend,
            int warehouseId,
            int? terminalId,
            int? cashierUserId,
            int? loginId)
        {
            var result = new LoyaltyEarnResult
            {
                Applied = false,
                CustomerId = customerId ?? 0,
                EarnedPoints = 0,
                BalanceBefore = 0,
                BalanceAfter = 0,
                EligibleSpend = eligibleSpend
            };

            if (customerId == null || customerId.Value <= 0)
            {
                result.MembershipCode = "GUEST";
                return result;
            }

            if (!_repo.IsLoyaltyInstalled(con, tran))
                return result;

            if (eligibleSpend <= 0m)
                eligibleSpend = 0m;

            string idem = $"EARN:SALE:{transactionId}";

            var account = _repo.GetOrCreateAccountForCustomer(con, tran, customerId.Value);
            result.MembershipCode = string.IsNullOrWhiteSpace(account.MembershipCode) ? "SILVER" : account.MembershipCode.Trim().ToUpperInvariant();

            if (result.MembershipCode == "GUEST")
                return result;

            var rule = _repo.GetPointRule(con, tran, account.MembershipLevelId);
            if (eligibleSpend < rule.MinSpendPerTxn)
                return result;

            decimal mult = account.EarnMultiplier;
            if (mult <= 0m) mult = 1m;

            int basePoints = 0;
            if (rule.SpendAmount > 0m && rule.EarnPoints > 0 && eligibleSpend > 0m)
            {
                var blocks = Math.Floor(eligibleSpend / rule.SpendAmount);
                if (blocks > 0)
                {
                    var raw = blocks * rule.EarnPoints * (decimal)mult;
                    basePoints = (int)Math.Floor(raw);
                }
            }

            if (rule.MaxPointsPerTxn.HasValue && rule.MaxPointsPerTxn.Value >= 0)
                basePoints = Math.Min(basePoints, rule.MaxPointsPerTxn.Value);

            if (basePoints <= 0)
                return result;

            int balanceBefore = _repo.GetAccountBalanceForUpdate(con, tran, account.AccountId);
            int balanceAfter = balanceBefore + basePoints;

            long txId = _repo.InsertLoyaltyTransaction(
                con,
                tran,
                accountId: account.AccountId,
                txType: "EARN",
                idempotencyKey: idem,
                refType: "SALE",
                refId: transactionId,
                invoiceNumber: invoiceNumber,
                warehouseId: warehouseId,
                terminalId: terminalId,
                cashierUserId: cashierUserId,
                loginId: loginId,
                pointsBefore: balanceBefore,
                pointsChange: basePoints,
                pointsAfter: balanceAfter,
                moneyAmount: eligibleSpend,
                reason: null
            );

            DateTime? exp = DateTime.Today.AddMonths(rule.ExpiryMonths <= 0 ? 12 : rule.ExpiryMonths);
            _repo.InsertPointBucket(con, tran, account.AccountId, txId, basePoints, exp);
            _repo.UpdateAccountAfterEarn(con, tran, account.AccountId, basePoints, eligibleSpend);
            _repo.UpdateCustomerLoyaltyCache(con, tran, customerId.Value, balanceAfter, account.MembershipLevelId);

            result.Applied = true;
            result.EarnedPoints = basePoints;
            result.BalanceBefore = balanceBefore;
            result.BalanceAfter = balanceAfter;
            result.CreatedAt = DateTime.Now;
            return result;
        }
    }
}
