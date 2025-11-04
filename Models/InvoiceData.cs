using System;
using System.Collections.Generic;

namespace POS_qu.Models
{
    public class InvoiceItem
    {
        public int ItemId { get; set; }             // ID produk
        public string Barcode { get; set; }         // barcode produk
        public string Name { get; set; }            // nama produk
        public string Unit { get; set; }            // satuan utama, misal pcs/dus/kg
        public string UnitVariant { get; set; }     // unit varian (misal dus), bisa sama dengan Unit jika tidak ada
        public int ConversionRate { get; set; }     // konversi unit varian ke unit utama
        public int Qty { get; set; }                // jumlah dalam unit varian
        public decimal Price { get; set; }          // harga per unit
        public decimal DiscountPercent { get; set; } // persen diskon per item
        public decimal DiscountAmount { get; set; }  // total diskon per item
        public decimal Tax { get; set; }             // pajak per item
        public decimal Total { get; set; }           // total setelah diskon & pajak
        public string Note { get; set; }            // catatan per item, misal “tanpa sambal”
    }

    public class InvoiceData
    {
        public List<InvoiceItem> Items { get; set; } = new();

        public decimal Subtotal { get; set; }           // total sebelum diskon global
        public decimal TotalDiscount { get; set; }      // total diskon item
        public decimal GlobalDiscountValue { get; set; }// diskon global
        public decimal GlobalDiscountPercent { get; set; }
        public decimal GrandTotal { get; set; }         // total akhir setelah diskon global
        public decimal Cashback { get; set; }           // kembalian
        public int NumOfItems { get; set; }            // jumlah total item

        // Tambahan info transaksi
        public string GlobalNote { get; set; }         // keterangan umum transaksi
        public decimal DeliveryAmount { get; set; }         // keterangan umum transaksi
        public string DeliveryName { get; set; }       // nama penerima
        public string DeliveryAddress { get; set; }    // alamat pengiriman
        public string DeliveryMethod { get; set; }     // kurir, ambil sendiri, dsb
        public string DeliveryPhone { get; set; }      // no HP penerima
    }
}
