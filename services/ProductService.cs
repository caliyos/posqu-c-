using System;
using System.Data;
using POS_qu.Models;
using POS_qu.Core.Interfaces;
using POS_qu.Repositories;

namespace POS_qu.Services
{
    public class ProductService : IProductService
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

        public Item GetProductDetail(int id)
        {
            if (id <= 0) return null;
            return _repository.GetItemById(id);
        }

        public bool SaveProduct(Item item, out string message)
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

        public bool DeleteProduct(int id, out string message)
        {
            try
            {
                _repository.DeleteItem(id);
                message = "Produk berhasil dihapus.";
                return true;
            }
            catch (Exception ex)
            {
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