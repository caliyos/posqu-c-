using System;

namespace POS_qu.Models
{
    public class StockLayer
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public int WarehouseId { get; set; }
        public double QtyRemaining { get; set; }
        public decimal BuyPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
    }
}
