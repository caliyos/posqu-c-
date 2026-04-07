using System;

namespace POS_qu.Models
{
    public class ItemUnit
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public int UnitId { get; set; }
        public double ConversionQty { get; set; }
        public bool IsDefault { get; set; }
        
        public string UnitName { get; set; }
        public string UnitAbbr { get; set; }
    }
}