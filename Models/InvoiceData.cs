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
    }

}
