using System.Collections.Generic;
using System.Data;
using POS_qu.Models;

namespace POS_qu.Core.Interfaces
{
    public interface IProductService
    {
        DataTable GetAllProducts();
        Item GetProductDetail(int id);
        bool SaveProduct(Item item, out string message);
        bool DeleteProduct(int id, out string message);
        
        DataTable GetCategories();
        DataTable GetUnits();
        DataTable GetSuppliers();
        DataTable GetPriceLevels();
        DataTable GetBrands();
        DataTable GetRacks();
        List<ItemPrice> GetItemPrices(int itemId);
        List<UnitVariant> GetItemUnitVariants(int itemId);
    }
}