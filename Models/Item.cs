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

        public int id { get; set; }
        public string name { get; set; }
        public decimal buy_price { get; set; }
        public decimal sell_price { get; set; }
        public string barcode { get; set; }
        public int stock { get; set; }
        public int reserved_stock { get; set; }
        public string unit { get; set; }

        public int unitid { get; set; }
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

        //////////////////////////////////////////////////////
        public int conversion { get; set; }  // Berapa pcs dalam 1 unit (default 1 kalau pcs biasa)
        public decimal price_per_pcs { get; set; }  // Harga per pcs untuk unit variant

        public decimal price_per_pcs_asli { get; set; }  // Harga per pcs untuk unit variant

    }
}
