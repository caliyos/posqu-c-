using System;

namespace POS_qu.Core.Interfaces
{
    public interface IStockValuationStrategy
    {
        /// <summary>
        /// Menghitung Total Harga Pokok Penjualan (HPP) / COGS saat barang keluar (dijual/disesuaikan)
        /// dan memotong stok dari layer yang sesuai.
        /// </summary>
        decimal CalculateCOGSAndDeductStock(int itemId, int warehouseId, int qtyToDeduct, object dbConnection, object dbTransaction);

        /// <summary>
        /// Menambahkan stok masuk (pembelian/retur) ke dalam layer stok.
        /// </summary>
        void AddStockIn(int itemId, int warehouseId, int qtyAdded, decimal unitCost, object dbConnection, object dbTransaction);
    }
}