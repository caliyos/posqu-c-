using POS_qu.Models;
using POS_qu.DTO;
using System.Collections.Generic;

namespace POS_qu.Core.Interfaces
{
    public interface ICartService
    {
        bool IsPaymentMode { get; set; }
        
        ItemWithVariants cekUnitVariant(string keyword);
        Item GetItemByName(string keyword);
        InvoiceData AddItemByVariant(InvoiceData currentInvoice, int variantId, int qty);
        InvoiceData updateItemByVariant(InvoiceData currentInvoice, int variantId, int qty);
        InvoiceData AddItemByName(InvoiceData currentInvoice, string name, int qty);
        InvoiceData UpdateItemQty(int pt_id, int newQty, InvoiceData invoice);
        InvoiceData UpdateCartItemStock(InvoiceItem item, InvoiceData invoice);
        InvoiceData RemoveItem(int itemId, int unitid, InvoiceData invoice, bool isPaymentMode = false);
        
        bool SaveCartAsDraft(string? customerName = null, string? note = null);
        List<InvoiceItem> GetDraftItems(int poId);
        InvoiceData LoadDraftToInvoice(int poId);
        InvoiceData LoadInvoiceFromCartSession(string cartCode);
        List<PendingOrderDto> GetDraftOrders();
        bool PayInstallment(string cartSessionCode, decimal amount, string customerName, string note);
        bool DeleteCartItem(DeleteCartItemRequest request);
        InvoiceData RecalculateCartPrices(InvoiceData invoice);
    }
}