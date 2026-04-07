using System;

namespace POS_qu.Models
{
    public class Stock
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public int WarehouseId { get; set; }
        public double Qty { get; set; }
        
        // Navigation properties (optional, untuk mempermudah binding)
        public string ItemName { get; set; }
        public string WarehouseName { get; set; }
    }
}