using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POS_qu.Models
{
    public class PendingTransaction
    {
        public int PtId { get; set; }
        public int TerminalId { get; set; }
        public int CashierId { get; set; }
        public int ItemId { get; set; }
        public string Barcode { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal SellPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }

}
