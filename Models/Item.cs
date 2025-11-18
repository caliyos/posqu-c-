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
        public int category_id { get; set; }

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

        ///////////////////////////////// SETTINGS ///////////////////////////
        public bool is_inventory_p { get; set; }
        public bool IsPurchasable { get; set; }      // is_dibeli
        public bool IsSellable { get; set; }         // is_dijual

        public bool RequireNotePayment { get; set; } // is_note_payment
        public bool is_changeprice_p { get; set; }

        public bool HasMaterials { get; set; }       // is_have_bahan
        public bool IsPackage { get; set; }          // is_box
        public bool IsProduced { get; set; }         // is_produksi

        // Helper untuk mapping dari DB Y/N ke bool

        // DISCOUNT FORMULA PER ITEM
        public string discount_formula { get; set; }


        // MULTI HARGA
        public List<ItemPrice> Prices { get; set; } = new();
        public List<UnitVariant> UnitVariants { get; set; } = new List<UnitVariant>();

    }
}
