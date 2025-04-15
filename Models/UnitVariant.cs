using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{
    public class UnitVariant
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; } = "";
        public int Conversion { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Profit { get; set; }

        //public override string ToString()
        //{
        //    return $"{UnitName} (x{Conversion}) - Rp {SellPrice:N0}";

        //}
        public string DisplayText => $"{UnitName} (x{Conversion}) - Rp {SellPrice:N0}";
    }

}
