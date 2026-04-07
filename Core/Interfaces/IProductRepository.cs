using System.Collections.Generic;
using System.Data;
using POS_qu.Models;

namespace POS_qu.Core.Interfaces
{
    public interface IProductRepository
    {
        DataTable GetAllItems();
        Item GetItemById(int id);
        int InsertItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(int id);
        
        DataTable GetCategories();
        DataTable GetUnits();
        DataTable GetSuppliers();
        DataTable GetPriceLevels();
        List<ItemPrice> GetItemPrices(int itemId);
        List<UnitVariant> GetItemUnitVariants(int itemId);
    }
}