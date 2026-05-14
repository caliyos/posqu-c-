using DocumentFormat.OpenXml.Wordprocessing;
using Npgsql;
using PdfSharp.Drawing.BarCodes;
using POS_qu.Controllers;
using POS_qu.Core;
using POS_qu.DTO;
using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.Repositories;
using POS_qu.Core.Interfaces;
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
public class CartService : ICartService
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
            ConversionRate = product.conversion,
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


    public InvoiceData updateItemByVariant(
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



    public InvoiceData UpdateItemQty(int pt_id, int newQty, InvoiceData invoice)
    {
        return UpdateItemQtyWithMeta(pt_id, newQty, 0m, 0m, "", invoice);
    }

    public InvoiceData UpdateItemQtyWithMeta(int pt_id, int newQty, decimal discountPercent, decimal discountAmount, string note, InvoiceData invoice)
    {
        var product = _repo.GetSinglePendingItemById(pt_id);
        if (product == null)
            throw new Exception("Produk tidak ditemukan");

        var invoiceItem = MapToInvoiceItem(product);
        invoiceItem.pt_id = pt_id;
        invoiceItem.IsEditMode = true;
        invoiceItem.EnteredQuantity = newQty;
        invoiceItem.PreviousQuantity = 0;
        invoiceItem.AdditionalQuantity = newQty;
        invoiceItem.DiscountPercent = discountPercent < 0 ? 0 : discountPercent;
        invoiceItem.DiscountAmount = discountAmount < 0 ? 0 : discountAmount;
        invoiceItem.Note = note ?? "";

        var _invoice = UpdateCartItemStock(invoiceItem, invoice);

        var rows = _repo.GetPendingItems(Session.CartSessionCode);
        var updated = InvoiceBuilder.FromPending(rows);
        CopyInvoiceMeta(invoice, updated);
        return updated;
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
                int activeWarehouseId = SessionUser.GetCurrentUser()?.WarehouseId ?? 1;
                bool reservedUpdated = _repo.TryUpdateReservedStock(
                    conn, tran,
                    item.ItemId,
                    activeWarehouseId,
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
                newQty,
                invoice.PriceLevelId);
            decimal price;

            if (priceResult.HasMultiPrice)
            {
                // Multi-price → total harga = qty * harga dari tabel tier
                price = priceResult.Price;
                item.IsMultiPrice = 1;
            }
            else
            {
                // Harga biasa → total = qty * harga per unit
                price = priceResult.Price;
                item.IsMultiPrice = 0;
            }

            decimal discountPercent = item.DiscountPercent;
            decimal discountAmountFixed = item.DiscountAmount;
            string note = item.Note ?? "";
            if (existingQty > 0 && item.ItemId > 0)
            {
                var meta = _repo.GetPendingTransactionMeta(conn, tran, session.TerminalId, item.ItemId, item.UnitId, invoice.CartSessionCode);
                if (discountPercent <= 0m && discountAmountFixed <= 0m)
                {
                    discountPercent = meta.DiscountPercent;
                    discountAmountFixed = meta.DiscountAmount;
                }
                if (string.IsNullOrWhiteSpace(note)) note = meta.Note;
            }

            decimal subTotal = price * newQty;
            decimal discountTotal;
            decimal discountPercentToStore;
            if (discountAmountFixed > 0m && discountPercent <= 0m)
            {
                discountPercentToStore = 0m;
                discountTotal = discountAmountFixed;
            }
            else
            {
                if (discountPercent < 0m) discountPercent = 0m;
                if (discountPercent > 100m) discountPercent = 100m;
                discountPercentToStore = discountPercent;
                discountTotal = Math.Round((subTotal * discountPercent) / 100m, 2, MidpointRounding.AwayFromZero);
            }
            if (discountTotal < 0m) discountTotal = 0m;
            if (discountTotal > subTotal) discountTotal = subTotal;
            decimal total = subTotal - discountTotal;

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
                    item.ConversionRate,
                    discountPercentToStore,
                    discountTotal,
                    note
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
                    invoice.CartSessionCode,
                    discountPercentToStore,
                    discountTotal,
                    note
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

        var updated = InvoiceBuilder.FromPending(rows);
        CopyInvoiceMeta(invoice, updated);
        return updated;
    }

    public void VoidCurrentTransaction(string cartSessionCode, int terminalId, int cashierId, int defaultWarehouseId)
    {
        cartSessionCode ??= "";
        if (string.IsNullOrWhiteSpace(cartSessionCode)) return;
        if (terminalId <= 0 || cashierId <= 0) return;

        using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
        conn.Open();
        using var tran = conn.BeginTransaction();

        try
        {
            var lines = new List<(int ItemId, int WarehouseId, int BaseQty)>();

            bool hasWarehouse = false;
            using (var chk = new NpgsqlCommand(@"
SELECT 1
FROM information_schema.columns
WHERE table_schema='public' AND table_name='pending_transactions' AND column_name='warehouse_id'
LIMIT 1
", conn, tran))
            {
                hasWarehouse = chk.ExecuteScalar() != null;
            }

            string selectSql = hasWarehouse
                ? @"
SELECT
    item_id,
    COALESCE(warehouse_id, @wid) AS warehouse_id,
    quantity,
    COALESCE(tsd_conversion_rate, 1) AS conversion_rate
FROM pending_transactions
WHERE cart_session_code = @code
  AND terminal_id = @terminal
  AND cashier_id = @cashier
"
                : @"
SELECT
    item_id,
    @wid AS warehouse_id,
    quantity,
    COALESCE(tsd_conversion_rate, 1) AS conversion_rate
FROM pending_transactions
WHERE cart_session_code = @code
  AND terminal_id = @terminal
  AND cashier_id = @cashier
";

            using (var cmd = new NpgsqlCommand(selectSql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@code", cartSessionCode);
                cmd.Parameters.AddWithValue("@terminal", terminalId);
                cmd.Parameters.AddWithValue("@cashier", cashierId);
                cmd.Parameters.AddWithValue("@wid", defaultWarehouseId <= 0 ? 1 : defaultWarehouseId);
                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    int itemId = r["item_id"] != DBNull.Value ? Convert.ToInt32(r["item_id"]) : 0;
                    int wid = r["warehouse_id"] != DBNull.Value ? Convert.ToInt32(r["warehouse_id"]) : (defaultWarehouseId <= 0 ? 1 : defaultWarehouseId);
                    decimal qty = r["quantity"] != DBNull.Value ? Convert.ToDecimal(r["quantity"]) : 0m;
                    int conv = r["conversion_rate"] != DBNull.Value ? Convert.ToInt32(r["conversion_rate"]) : 1;
                    if (conv <= 0) conv = 1;

                    if (itemId <= 0) continue;
                    int baseQty = (int)Math.Round(qty * conv, 0, MidpointRounding.AwayFromZero);
                    if (baseQty == 0) continue;
                    lines.Add((itemId, wid, baseQty));
                }
            }

            foreach (var l in lines)
            {
                _repo.TryUpdateReservedStock(conn, tran, l.ItemId, l.WarehouseId, -l.BaseQty);
            }

            using (var del = new NpgsqlCommand(@"
DELETE FROM pending_transactions
WHERE cart_session_code = @code
  AND terminal_id = @terminal
  AND cashier_id = @cashier
", conn, tran))
            {
                del.Parameters.AddWithValue("@code", cartSessionCode);
                del.Parameters.AddWithValue("@terminal", terminalId);
                del.Parameters.AddWithValue("@cashier", cashierId);
                del.ExecuteNonQuery();
            }

            tran.Commit();
        }
        catch
        {
            try { tran.Rollback(); } catch { }
            throw;
        }
    }

    private static void CopyInvoiceMeta(InvoiceData from, InvoiceData to)
    {
        if (from == null || to == null) return;
        to.CartSessionCode = from.CartSessionCode;
        to.CustomerId = from.CustomerId;
        to.CustomerName = from.CustomerName;
        to.PriceLevelId = from.PriceLevelId;
        to.GlobalDiscountPercent = from.GlobalDiscountPercent;
        to.GlobalDiscountIsAmount = from.GlobalDiscountIsAmount;
        to.GlobalDiscountValue = from.GlobalDiscountValue;
        to.DeliveryAmount = from.DeliveryAmount;
        to.GlobalNote = from.GlobalNote;
    }


 




    public CartService(CartActivity cartrepo, IActivityService activityService)
    {
        _repo = cartrepo;
        _activityService = activityService;
    }

    public bool SaveCartAsDraft(string? customerName = null, string? note = null)
    {
        var session = SessionUser.GetCurrentUser();

        using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
        conn.Open();
        using var tran = conn.BeginTransaction();

        try
        {
            // 1️⃣ Hitung total & diskon
            var totals = _repo.GetPendingTotals(
                Session.CartSessionCode,
                session.TerminalId,
                session.UserId
            );

            if (totals.TotalItems == 0)
                throw new InvalidOperationException("Cart kosong, tidak bisa disimpan sebagai draft.");

            // 2️⃣ Insert pending_orders
            int poId = _repo.InsertPendingOrder(
                terminalId: session.TerminalId,
                cashierId: session.UserId,
                customerName: customerName,   // 👈 kirim nullable
                note: note,                   // 👈 kirim nullable
                total: totals.GrandTotal,
                discount: totals.TotalDiscount,
                cartSessionCode: Session.CartSessionCode,
                conn: conn,
                tran: tran
            );

            // 3️⃣ Link item ke order
            _repo.LinkPendingTransactionsToOrder(
                poId,
                Session.CartSessionCode,
                session.TerminalId,
                session.UserId,
                conn,
                tran
            );

            tran.Commit();

            _activityService.LogAction(
                userId: session.UserId.ToString(),
                actionType: ActivityType.Cart.ToString(),
                referenceId: poId,
                desc: "Cart disimpan sebagai draft",
                details: new { Session.CartSessionCode, totals.GrandTotal, totals.TotalDiscount, customerName, note }
            );

            return true;
        }
        catch (Exception ex)
        {
            tran.Rollback();
            throw new InvalidOperationException("Gagal menyimpan cart sebagai draft: " + ex.Message, ex);
        }
    }




    public List<InvoiceItem> GetDraftItems(int poId)
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();

            string sql = @"
SELECT 
    item_id,
    barcode,
    quantity,
    sell_price,
    total,
    unitid
FROM pending_transactions
WHERE po_id = @poId";

            using var cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@poId", poId);

            var items = new List<InvoiceItem>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new InvoiceItem
                {
                    ItemId = reader.GetInt32(0),
                    Barcode = reader.GetString(1),
                    Qty = reader.GetInt32(2),
                    Price = reader.GetDecimal(3),
                    Total = reader.GetDecimal(4),
                    UnitId = reader.GetInt32(5)
                });
            }

            return items;
        }

    public InvoiceData LoadDraftToInvoice(int poId)
    {
        var session = SessionUser.GetCurrentUser();

        // 1️⃣ Ambil cart_session_code dari pending_orders
        string cartCode = _repo.GetCartSessionCodeByPoId(poId);

        if (string.IsNullOrEmpty(cartCode))
            throw new Exception("Draft tidak ditemukan.");

        // 2️⃣ Set session cart ke draft itu
        Session.CartSessionCode = cartCode;

        // 3️⃣ Ambil item pending dari DB
        var rows = _repo.GetPendingItems(cartCode);

       
            if (rows == null || rows.Rows.Count == 0)
                throw new Exception("Draft kosong atau sudah tidak valid.");

        // 4️⃣ Build invoice dari pending_transactions
        var invoice = InvoiceBuilder.FromPending(rows);
        invoice.CartSessionCode = cartCode;
        invoice.IsFromDraft = 1;
        invoice.Status = "draft";

        try
        {
            using var conn = new NpgsqlConnection(DbConfig.ConnectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT COALESCE(customer_name,''), COALESCE(note,'')
FROM pending_orders
WHERE po_id = @id
LIMIT 1
", conn);
            cmd.Parameters.AddWithValue("@id", poId);
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                var customer = r.GetString(0);
                var note = r.GetString(1);
                if (!string.IsNullOrWhiteSpace(customer))
                    invoice.CustomerName = customer;
                if (!string.IsNullOrWhiteSpace(note))
                    invoice.GlobalNote = note;
            }
        }
        catch
        {
        }

        return invoice;
    }

    public InvoiceData LoadInvoiceFromCartSession(string cartCode)
    {
        Session.CartSessionCode = cartCode;
        var rows = _repo.GetPendingItems(cartCode);
        var invoice = InvoiceBuilder.FromPending(rows);
        invoice.CartSessionCode = cartCode;
        return invoice;
    }

    public List<PendingOrderDto> GetDraftOrders()
    {
        var session = SessionUser.GetCurrentUser();
        return _repo.GetDraftOrders(session.TerminalId, session.UserId);
    }






    ///////////////////////INSTALLMETNS////////////////////////////

    public bool PayInstallment(
     string cartSessionCode,
     decimal amount,
     string customerName,
     string note
 )
    {
        if (amount <= 0)
            throw new InvalidOperationException("Nominal harus lebih besar dari 0.");

        if (string.IsNullOrWhiteSpace(cartSessionCode))
            throw new InvalidOperationException("Cart tidak valid.");

        var session = SessionUser.GetCurrentUser();

        return _repo.PayInstallmentDb(
            cartSessionCode,
            amount,
            customerName,
            note,
            session.UserId
        );
    }




    ///////////////////////INSTALLMENTS//////////////////////////



    // =================== DELETE ITEM ===================
    public bool DeleteCartItem(DeleteCartItemRequest request)
    {
        try
        {
            var session = SessionUser.GetCurrentUser();
            int activeWarehouseId = session?.WarehouseId ?? 1;

            int stockNeeded = request.Quantity * request.ConversionRate;

            using (var conn = new NpgsqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                using var tran = conn.BeginTransaction();

                bool deleteSuccess = _repo.DeletePendingTransaction(
                    conn, tran,
                    session.TerminalId,
                    session.UserId,
                    request.ItemId,
                    request.UnitId,
                    request.CartSessionCode
                );

                if (!deleteSuccess)
                    throw new InvalidOperationException("Failed to delete item from pending transactions.");

                if (stockNeeded != 0)
                {
                    bool ok = _repo.TryUpdateReservedStock(conn, tran, request.ItemId, activeWarehouseId, -stockNeeded);
                    if (!ok)
                        throw new InvalidOperationException("Gagal melepas reserved stock.");
                }

                tran.Commit();
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

    public InvoiceData RecalculateCartPrices(InvoiceData invoice)
    {
        var itemsCopy = invoice.Items.ToList();
        foreach (var item in itemsCopy)
        {
            item.IsEditMode = true;
            item.AdditionalQuantity = item.Qty; // Keep same quantity, just recalculate
            invoice = UpdateCartItemStock(item, invoice);
        }
        return invoice;
    }


    private PriceDecisionResult DecideMultiPrice(int itemId, int unitId, int qty, int priceLevelId = 1)
    {
        return _repo.DecideMultiPriceFromDb(itemId, unitId, qty, priceLevelId);
    }




}
