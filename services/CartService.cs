using DocumentFormat.OpenXml.Wordprocessing;
using Npgsql;
using PdfSharp.Drawing.BarCodes;
using POS_qu.Controllers;
using POS_qu.Core;
using POS_qu.DTO;
using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.Repositories;
using static POS_qu.Repositories.CartActivity;


public class AddToCartRequest
{
    public int ItemVariantId { get; set; }
    public string PriceType { get; set; }
    public int Quantity { get; set; }
}

public class ItemWithVariants
{
    public Item Item { get; set; }
    public List<UnitVariant> Variants { get; set; } = new();
}



// DATAGRID SYNCHRONIZATION WITH PENDING TRANSACTIONS IN DB
public class CartService

{
    //private CartActivity _repo;
    private readonly IActivityService _activityService;

    public bool IsPaymentMode { get; set; } = false;

    private CartActivity _repo = new CartActivity();

    


    private InvoiceItem MapToInvoiceItem(Item product)
    {
        return new InvoiceItem
        {
            ItemId = product.id,
            Barcode = product.barcode,
            Name = product.name,
            Unit = product.unit,
            UnitId = product.unitid,
            UnitVariant = product.unit, // default sama
            ConversionRate = 1,
            Qty = 1,
            Price = product.sell_price,
            CostPrice = product.buy_price,
            DiscountPercent = 0,
            DiscountAmount = 0,
            Tax = 0,
            Total = product.sell_price,
            Note = "",
            IsEditMode = false,
            AdditionalQuantity = 0
        };
    }

    private InvoiceItem MapVariantToInvoiceItem(UnitVariant variant, Item item, int qty = 1)
    {
        var total = variant.SellPrice * qty;

        return new InvoiceItem
        {
            ItemId = item.id,
            Barcode = item.barcode,
            Name = item.name,

            Unit = variant.UnitName,                 // base unit (pcs)
            UnitId = variant.UnitId,
            UnitVariant = variant.UnitName,   // dus / box / dll
            ConversionRate = variant.Conversion,

            Qty = qty,
            Price = variant.SellPrice,
            CostPrice = variant.SellPrice - variant.Profit,

            DiscountPercent = 0,
            DiscountAmount = 0,
            Tax = 0,

            Total = total,

            IsEditMode = false,
            AdditionalQuantity = qty,
            PreviousQuantity = 0,
            EnteredQuantity = qty
        };
    }

    public ItemWithVariants cekUnitVariant(string keyword)
    {
        var item = _repo.GetSingleItemByName(keyword);

        if (item == null)
            return null;

        var variants = _repo.GetVariantsByItemId(item.id);

        return new ItemWithVariants
        {
            Item = item,
            Variants = variants
        };
    }


    public Item GetItemByName(string keyword)
    {
       return _repo.GetSingleItemByName(keyword); 
    }

    public InvoiceData AddItemByVariant(
    InvoiceData currentInvoice,
    int variantId,
    int qty)
    {
        var variant = _repo.GetVariantById(variantId);

        if (variant == null)
            throw new Exception("Variant tidak ditemukan");

        var item = _repo.GetItemById(variant.ItemId);

        if (item == null)
            throw new Exception("Produk tidak ditemukan");

        var invoiceItem = MapVariantToInvoiceItem(variant, item, qty);

        invoiceItem.IsEditMode = false;
        invoiceItem.AdditionalQuantity = qty;

        // CORE
        currentInvoice = UpdateCartItemStock(invoiceItem, currentInvoice);

        var rows = _repo.GetPendingItems(Session.CartSessionCode);

        var updatedInvoice = InvoiceBuilder.FromPending(rows);

        currentInvoice.Items = updatedInvoice.Items;
        currentInvoice.NumOfItems = updatedInvoice.NumOfItems;
        currentInvoice.Subtotal = updatedInvoice.Subtotal;
        currentInvoice.TotalDiscount = updatedInvoice.TotalDiscount;
        currentInvoice.GrandTotal = updatedInvoice.GrandTotal;

        return currentInvoice;
    }


    public InvoiceData AddItemByName(
     InvoiceData currentInvoice,
     string name,
     int qty)
    {
        Item product = _repo.GetSingleItemByName(name);

        if (product == null)
            throw new Exception("Produk tidak ditemukan");

        var invoiceItem = MapToInvoiceItem(product);

        invoiceItem.IsEditMode = false;
        invoiceItem.AdditionalQuantity = qty;

        // CORE
        currentInvoice = UpdateCartItemStock(invoiceItem, currentInvoice);

        var rows = _repo.GetPendingItems(Session.CartSessionCode);

        // 🔥 Build invoice baru dari DB
        // 
        var updatedInvoice = InvoiceBuilder.FromPending(rows);


        currentInvoice.Items = updatedInvoice.Items;
        currentInvoice.NumOfItems = updatedInvoice.NumOfItems;
        currentInvoice.Subtotal = updatedInvoice.Subtotal;
        currentInvoice.TotalDiscount = updatedInvoice.TotalDiscount;
        currentInvoice.GrandTotal = updatedInvoice.GrandTotal;

        return currentInvoice;
    }



    public InvoiceData UpdateItemQty(string itemName, int newQty, InvoiceData invoice)
    {
        var product = _repo.GetSingleItemByName(itemName);
        if (product == null)
            throw new Exception("Produk tidak ditemukan");


        var invoiceItem = MapToInvoiceItem(product);
        invoiceItem.IsEditMode = true;
        invoiceItem.EnteredQuantity = newQty;
        invoiceItem.PreviousQuantity = 0;
        invoiceItem.AdditionalQuantity = newQty;
        var _invoice = UpdateCartItemStock(invoiceItem, invoice);

        var rows = _repo.GetPendingItems(Session.CartSessionCode);
        return InvoiceBuilder.FromPending(rows);


    }





    public InvoiceData UpdateCartItemStock(InvoiceItem item,InvoiceData invoice)
    {
        //var result = new UpdateStockResult();
        var session = SessionUser.GetCurrentUser();

        //  TRANSACTION
        using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
        conn.Open();
        using var tran = conn.BeginTransaction(); // START TRANSACTION

        try
        {
            // 🔥 1. Ambil qty lama
            int existingQty = _repo.GetPendingItemQty(
                session.TerminalId,
                item.ItemId,
                item.UnitId,
                invoice.CartSessionCode
            );

            int newQty;

            if (item.IsEditMode)
                newQty = item.AdditionalQuantity;              // replace
            else
                newQty = existingQty + item.AdditionalQuantity; // tambah

            int qtyDiff = newQty - existingQty;

            int stockAdjustment = qtyDiff * item.ConversionRate;
            // ===============================
            // 🔥 2. Update reserved dulu (ATOMIC)
            // ===============================
            if (qtyDiff != 0)
            {
                bool reservedUpdated = _repo.TryUpdateReservedStock(
                    conn, tran,
                    item.ItemId,
                    stockAdjustment
                );

                if (!reservedUpdated)
                {
                    throw new InvalidOperationException("Stock tidak cukup.");
                }
            }

            // ===============================
            // 🔥 3. Hitung nulti harga
            // ===============================
            var priceResult = DecideMultiPrice(
                item.ItemId,
                item.UnitId,
                newQty);
            decimal price;

            if (priceResult.HasMultiPrice)
            {
                // Multi-price → total harga langsung pakai price dari tabel
                price = priceResult.Price;
                item.IsMultiPrice = 1;
            }
            else
            {
                // Harga biasa → total = qty * harga per unit
                price = priceResult.Price;
                item.IsMultiPrice = 0;
            }

            decimal total = priceResult.HasMultiPrice ? price : price * newQty;

            bool updateSuccess;

            if (existingQty == 0)
            {
                updateSuccess = _repo.UpsertPendingItem(
                    conn, tran,
                    invoice.CartSessionCode,
                    item.ItemId,
                    newQty,
                    item.Unit,
                    item.UnitId,
                    price,
                    total,
                    item.ConversionRate
                );
            }
            else
            {
                updateSuccess = _repo.UpdatePendingTransactionStock(
                    conn, 
                    tran,
                    session.TerminalId,
                    item.ItemId,
                    newQty,
                    total,
                    item.Unit,
                    price,
                    invoice.CartSessionCode
                );
            }



            if (!updateSuccess)
                throw new Exception("Gagal update pending_transactions.");

            tran.Commit();
            return invoice;
        }
        catch (Exception ex)
        {
            tran.Rollback();
            throw new Exception("ERROR." + ex.Message + " : " + ex.ToString());
        }
    }



    public InvoiceData RemoveItem(int itemId, int unitid, InvoiceData invoice, bool isPaymentMode = false)
    {
        // Ambil item dari pending
        var request = _repo.GetPendingItemById(Session.CartSessionCode, itemId, unitid);

        if (request == null)
            throw new Exception("Item tidak ditemukan di cart.");

        // Set mode payment kalau perlu
        request.IsPaymentMode = isPaymentMode;

        // Jalankan delete (anggap method ini throw kalau gagal)
        DeleteCartItem(request);

        // Reload cart
        var rows = _repo.GetPendingItems(Session.CartSessionCode);

        return InvoiceBuilder.FromPending(rows);
    }


 




    public CartService(CartActivity cartrepo, IActivityService activityService)
    {
        _repo = cartrepo;
        _activityService = activityService;
    }

    // =================== DELETE ITEM ===================
    public bool DeleteCartItem(DeleteCartItemRequest request)
    {
        try
        {
            var session = SessionUser.GetCurrentUser();

            int stockNeeded = request.Quantity * request.ConversionRate;

            // Delete pending
            bool deleteSuccess = _repo.DeletePendingTransaction(
                session.TerminalId,
                session.UserId,
                request.ItemId,
                request.UnitId,
                request.CartSessionCode
            );

            if (!deleteSuccess)
                throw new InvalidOperationException("Failed to delete item from pending transactions.");

            // Update reserved stock
            int reservedStock = _repo.GetItemReservedStock(request.Barcode);
            int newReservedStock = Math.Max(0, reservedStock - stockNeeded);
            _repo.UpdateReservedStock(request.Barcode, newReservedStock);

            if (request.IsPaymentMode)
            {
                // Mode payment: kurangi stock permanen
                int currentStock = _repo.GetItemStock(request.Barcode);
                int newStock = Math.Max(0, currentStock - stockNeeded);
                _repo.UpdateItemStock(request.Barcode, newStock);
            }

            // Log aktivitas
            _activityService.LogAction(
                userId: session.UserId.ToString(),
                actionType: ActivityType.Cart.ToString(),
                referenceId: null,
                desc: $"Deleted item from cart: {request.ItemId}",
                details: new
                {
                    request.ItemId,
                    request.Reason,
                    terminal = session.TerminalId,
                    shiftId = session.ShiftId,
                    IpAddress = NetworkHelper.GetLocalIPAddress(),
                    UserAgent = GlobalContext.getAppVersion(),
                    loginId = session.LoginId,
                    request.CartSessionCode,
                    request.IsPaymentMode
                }
            );

            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to delete item from cart.", ex);
       
        }
    }



    //// =================== UPDATE DISCOUNT ===================
    //public bool UpdateCartItemDiscount(int itemId, decimal discountPercentage, string cart_session_code)
    //{
    //    bool success = _repo.UpdatePendingTransactionDiscount(
    //        SessionUser.GetCurrentUser().TerminalId,
    //        SessionUser.GetCurrentUser().UserId,
    //        itemId,
    //        cart_session_code,
    //        discountPercentage
    //    );

    //    if (!success) MessageBox.Show("Failed to update discount in database.");
    //    return success;
    //}

    //// =================== UPDATE NOTE ===================
    //public bool UpdateCartItemNote(int itemId, string note, string cart_session_code)
    //{
    //    bool success = _repo.UpdatePendingTransactionNote(
    //        SessionUser.GetCurrentUser().TerminalId,
    //        SessionUser.GetCurrentUser().UserId,
    //        itemId,
    //        cart_session_code,
    //        note
    //    );

    //    if (!success) MessageBox.Show("Failed to update note in database.");
    //    return success;
    //}

    // =================== UPDATE QUANTITY / STOCK ===================
    //public bool UpdateCartItemQuantity(int itemId, int newQty, int previousQty, string barcode, string cart_session_code)
    //{
    //    if (newQty == 0)
    //    {
    //        DialogResult result = MessageBox.Show("Are you sure you want to remove this item?", "Confirm", MessageBoxButtons.YesNo);
    //        if (result == DialogResult.Yes)
    //        {
    //            return DeleteCartItem(itemId, barcode, previousQty, 1, "REMOVE_ITEM_FROM_CART", cart_session_code);
    //        }
    //        else
    //        {
    //            return false; // cancel, restore value di form
    //        }
    //    }
    //    else
    //    {
    //        // Bisa update pending stock jika ada method khusus di controller
    //        // Misal: _itemController.UpdatePendingTransactionQuantity(...)
    //        return true;
    //    }
    //}






    public class UpdateStockResult
    {
        public bool Success { get; set; }
        public int EnteredQuantity { get; set; }
        public decimal Total { get; set; }
        public string ErrorMessage { get; set; }
        public decimal PricePerUnit { get; set; }
        public string ExceptionDetail { get; set; }

    }


    private PriceDecisionResult DecideMultiPrice(int itemId, int unitId, int qty)
    {
        return _repo.DecideMultiPriceFromDb(itemId, unitId, qty);
    }




}
