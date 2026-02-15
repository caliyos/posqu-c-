using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.Data;

namespace POS_qu.Models
{
    public class InvoiceItem
    {
        public int ItemId { get; set; }             // ID produk
        public string Barcode { get; set; }         // barcode produk
        public string Name { get; set; }            // nama produk
        public string Unit { get; set; }            // satuan utama, misal pcs/dus/kg
        public int UnitId { get; set; }            // satuan utama, misal pcs/dus/kg
        public string UnitVariant { get; set; }     // unit varian (misal dus), bisa sama dengan Unit jika tidak ada
        public int ConversionRate { get; set; }     // konversi unit varian ke unit utama
        public int Qty { get; set; }                // jumlah dalam unit varian
        public decimal Price { get; set; }          // harga per unit
        public decimal CostPrice { get; set; }          // cost per unit
        public decimal DiscountPercent { get; set; } // persen diskon per item
        public decimal DiscountAmount { get; set; }  // total diskon per item
        public decimal Tax { get; set; }             // pajak per item
        public decimal Total { get; set; }           // total setelah diskon & pajak
        public string Note { get; set; }            // catatan per item, misal “tanpa sambal”

        public bool IsEditMode { get; set; }

        public int AdditionalQuantity { get; set; }
        public int PreviousQuantity { get; set; }
        public int EnteredQuantity { get; set; }

        public int IsMultiPrice { get; set; }


    }

    public class InvoiceData
    {
        public List<InvoiceItem> Items { get; set; } = new();

        // Nilai dasar perhitungan
        public decimal Subtotal { get; set; }               // Total sebelum diskon global
        public decimal TotalDiscount { get; set; }          // Total diskon dari tiap item
        public decimal GlobalDiscountPercent { get; set; }  // Diskon global dalam persen
        public decimal GlobalDiscountValue { get; set; }    // Nilai diskon global dalam rupiah
        public decimal DeliveryAmount { get; set; }         // Ongkir
        public decimal GrandTotal { get; set; }             // Total akhir (setelah semua diskon dan ongkir)
        public decimal PaymentAmount { get; set; }          // Jumlah uang yang dibayar
        public decimal Cashback { get; set; }               // Kembalian
        public string PaymentMethod { get; set; }               // Kembalian
        

        // Informasi tambahan transaksi
        public string GlobalNote { get; set; }              // Catatan global transaksi
        public string DeliveryName { get; set; }            // Nama penerima
        public string DeliveryAddress { get; set; }         // Alamat pengiriman
        public string DeliveryMethod { get; set; }          // Kurir / Ambil sendiri / dll
        public string DeliveryPhone { get; set; }           // Nomor HP penerima

        // Informasi meta transaksi
        public string CartSessionCode { get; set; }         // Kode sesi keranjang (tracking internal)

        public int NumOfItems { get; set; }
        public int IsFromDraft { get; set; }
        public int IsFromOrders { get; set; }

        public string Status { get; set; }

        public decimal ChangeAmount { get; set; }



    }


    public static class InvoiceBuilder
    {
        public static InvoiceData FromPending(DataTable rows)
        {
            //var invoice = new InvoiceData();
            var newinvoice = new InvoiceData();   // 🔥 selalu buat baru
            foreach (DataRow r in rows.Rows)
            {
                var item = new InvoiceItem
                {
                    ItemId = r["item_id"] != DBNull.Value ? Convert.ToInt32(r["item_id"]) : 0,
                    Barcode = r["barcode"]?.ToString() ?? "",
                    Name = r["name"]?.ToString() ?? "",
                    Unit = r["unit"]?.ToString() ?? "",
                    UnitId = r["unitid"] != DBNull.Value ? Convert.ToInt32(r["unitid"]) : 0,
                    //Barcode = r["barcode"]?.ToString() ?? "",
                    UnitVariant = r["unit_variant"]?.ToString() ?? "",
                    ConversionRate = r["conversion_rate"] != DBNull.Value ? Convert.ToInt32(r["conversion_rate"]) : 1,
                    Qty = r["qty"] != DBNull.Value ? Convert.ToInt32(r["qty"]) : 0,
                    Price = r["sell_price"] != DBNull.Value ? Convert.ToDecimal(r["sell_price"]) : 0,
                    CostPrice = r["buy_price"] != DBNull.Value ? Convert.ToDecimal(r["buy_price"]) : 0,
                    DiscountPercent = r["disc_percent"] != DBNull.Value ? Convert.ToDecimal(r["disc_percent"]) : 0,
                    DiscountAmount = r["disc_amount"] != DBNull.Value ? Convert.ToDecimal(r["disc_amount"]) : 0,
                    Tax = r["tax"] != DBNull.Value ? Convert.ToDecimal(r["tax"]) : 0,
                    Note = r["note"]?.ToString() ?? "",
                    Total = r["total"] != DBNull.Value ? Convert.ToDecimal(r["total"]) : 0,
                };

                newinvoice.Items.Add(item);
            }

            // ========================
            // 🧮 HITUNG TOTAL INVOICE
            // ========================
            newinvoice.NumOfItems = newinvoice.Items.Sum(i => i.Qty);
            newinvoice.Subtotal = newinvoice.Items.Sum(i => i.Total);
            newinvoice.TotalDiscount = newinvoice.Items.Sum(i => i.DiscountAmount);
            newinvoice.GrandTotal = newinvoice.Items.Sum(i => i.Total);
            newinvoice.CartSessionCode = Session.CartSessionCode;

            return newinvoice;
        }
    }


}
