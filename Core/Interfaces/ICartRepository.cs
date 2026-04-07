using System.Collections.Generic;
using POS_qu.Models;

namespace POS_qu.Core.Interfaces
{
    public interface ICartRepository
    {
        Item GetItemById(int id);
        Item GetSingleItemByName(string search);
        Item GetSinglePendingItemById(int pt_id);
        UnitVariant GetVariantById(int variantId);
    }
}