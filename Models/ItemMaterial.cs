using System;

namespace POS_qu.Models
{
    public class ItemMaterial
    {
        public int Id { get; set; }
        public int ParentItemId { get; set; }
        public int ComponentItemId { get; set; }
        public string ComponentName { get; set; } = "";
        public decimal Qty { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; } = "";
        public decimal UnitCost { get; set; }
        public decimal Subtotal => Qty * UnitCost;
    }
}
