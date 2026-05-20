using System;

namespace POS_qu.Models
{
    public class LoyaltyEarnResult
    {
        public bool Applied { get; set; }
        public int CustomerId { get; set; }
        public string MembershipCode { get; set; } = "GUEST";
        public int EarnedPoints { get; set; }
        public int BalanceBefore { get; set; }
        public int BalanceAfter { get; set; }
        public decimal EligibleSpend { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
