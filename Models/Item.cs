using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POS_qu.Models
{
    public class Item
    {


        //////////////////////////////
        //public string id { get; set; }
        //public string barcode { get; set; }
        //public string name { get; set; }
        //public decimal stock { get; set; }
        //public string unit { get; set; }
        //public decimal sell_price { get; set; }
        //public decimal buy_price { get; set; }

        public long id { get; set; }
        public string name { get; set; }
        public decimal buy_price { get; set; }
        public decimal sell_price { get; set; }
        public string barcode { get; set; }
        public double stock { get; set; }
        public double reserved_stock { get; set; }
        public string unit { get; set; }
        public int group { get; set; }
        public string is_inventory_p { get; set; }
        public string is_changeprice_p { get; set; }
        public string materials { get; set; }
        public string note { get; set; }
        public string picture { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public int supplier_id { get; set; }
        public int flag { get; set; }

    }
}
