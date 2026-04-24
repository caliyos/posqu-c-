using System;
using System.Collections.Generic;

namespace POS_qu.Core.Interfaces
{
    public sealed class StockDeductionLine
    {
        public long? StockLayerId { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitCost { get; set; }
    }

    public sealed class StockDeductionResult
    {
        public decimal TotalCogs { get; set; }
        public decimal QtyDeducted { get; set; }
        public List<StockDeductionLine> Lines { get; } = new List<StockDeductionLine>();
    }

    public interface IStockValuationStrategy
    {
        /// <summary>
        /// Menghitung Total Harga Pokok Penjualan (HPP) / COGS saat barang keluar (dijual/disesuaikan)
        /// dan memotong stok dari layer yang sesuai.
        /// </summary>
        StockDeductionResult CalculateCOGSAndDeductStock(int itemId, int warehouseId, decimal qtyToDeduct, object dbConnection, object dbTransaction);

        /// <summary>
        /// Menambahkan stok masuk (pembelian/retur) ke dalam layer stok.
        /// </summary>
        void AddStockIn(int itemId, int warehouseId, decimal qtyAdded, decimal unitCost, object dbConnection, object dbTransaction);
    }
}
