using POS_qu.Core.Interfaces;
using POS_qu.Models;
using POS_qu.Repositories;
using System;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace POS_qu.Services
{
    public class ProductService 
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }

        public DataTable GetAllProducts()
        {
            return _repository.GetAllItems();
        }

        public POS_qu.Models.Item GetProductDetail(int id)
        {
            if (id <= 0) return null;
            return _repository.GetProductDetail(id);
        }

        public ItemMaterial AddMaterial(POS_qu.Models.Item detail)
        {
            var row = new ItemMaterial
            {
                ComponentItemId = detail.id,
                ComponentName = detail.name,
                Qty = 1m,
                UnitId = detail.unitid > 0 ? detail.unitid : 1,
                Hpp = detail.hpp,
                UnitCost = detail.sell_price
            };
            return row;
        }
        public decimal GetKitBundleStock(IEnumerable<ItemMaterial> materials)
        {
            if (materials == null || !materials.Any())
                return 0m;

            decimal minStock = decimal.MaxValue;

            foreach (var material in materials)
            {
                var item = _repository.GetProductDetail(material.ComponentItemId);

                if (item == null)
                    return 0m;

                if (material.Qty <= 0)
                    continue;

                decimal possibleQty = item.stock / material.Qty;

                if (possibleQty < minStock)
                    minStock = possibleQty;
            }

            return minStock == decimal.MaxValue
                ? 0m
                : Math.Floor(minStock);
        }
        public DataTable GetProducts()
        {
            return _repository.GetAllItems();
        }

        public bool SaveProduct(POS_qu.Models.Item item, out string message)
        {
            message = "";

            // Validasi Bisnis Layer
            if (string.IsNullOrWhiteSpace(item.name))
            {
                message = "Nama produk tidak boleh kosong.";
                return false;
            }

            if (item.buy_price < 0 || item.sell_price < 0)
            {
                message = "Harga beli dan harga jual tidak boleh negatif.";
                return false;
            }

            try
            {
                if (item.id == 0)
                {
                    int newId = _repository.InsertItem(item);
                    item.id = newId;
                    message = "Produk berhasil ditambahkan.";
                }
                else
                {
                    _repository.UpdateItem(item);
                    message = "Produk berhasil diperbarui.";
                }
                return true;
            }
            catch (Exception ex)
            {
                message = $"Terjadi kesalahan saat menyimpan produk: {ex.Message}";
                return false;
            }
        }

        public bool DeleteProduct(int id, out bool archived, out string message)
        {
            try
            {
                bool ok = _repository.DeleteItem(id, out archived);
                if (!ok)
                {
                    message = "Produk tidak ditemukan.";
                    return false;
                }

                message = "Produk dinonaktifkan (arsip).";
                return true;
            }
            catch (Exception ex)
            {
                archived = false;
                message = $"Gagal menghapus produk: {ex.Message}";
                return false;
            }
        }

        public DataTable GetCategories() => _repository.GetCategories();
        public DataTable GetUnits() => _repository.GetUnits();
        public DataTable GetSuppliers() => _repository.GetSuppliers();
        public DataTable GetPriceLevels() => _repository.GetPriceLevels();
        public DataTable GetBrands() => _repository.GetBrands();
        public DataTable GetRacks() => _repository.GetRacks();
        public List<ItemPrice> GetItemPrices(int itemId) => _repository.GetItemPrices(itemId);
        public List<UnitVariant> GetItemUnitVariants(int itemId) => _repository.GetItemUnitVariants(itemId);
    }
}
