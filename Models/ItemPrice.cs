using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{
    public class ItemPrice
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        
        [DisplayName("Level Harga ID")]
        public int PriceLevelId { get; set; }
        
        [DisplayName("Level Harga")]
        public string PriceLevelName { get; set; }

        [DisplayName("Unit ID")]
        public int UnitId { get; set; }

        [DisplayName("Satuan")]
        public string UnitName { get; set; }

        [DisplayName("Min Qty")]
        public int MinQty { get; set; }

        [DisplayName("Max Qty")]
        public int? MaxQty { get; set; }

        [DisplayName("Harga")]
        public decimal Price { get; set; }

        // Temporary helper for UI
        [Browsable(false)]
        public decimal buy_price_temp { get; set; }
    }
}
