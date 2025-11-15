using POS_qu.Controllers;
using POS_qu.Core;
using POS_qu.Helpers;
using POS_qu.Models;

public class CartService
{
    private ItemController _itemController;
    private readonly IActivityService _activityService;

    public CartService(ItemController itemController, IActivityService activityService)
    {
        _itemController = itemController;
        _activityService = activityService;
    }

    // =================== DELETE ITEM ===================
    public bool DeleteCartItem(int itemId, string barcode, int qty, int conversionRate, string reason = "DELETE_ITEM_FROM_CART", string cart_session_code = null)
    {
        try
        {
            int stockNeeded = qty * conversionRate;

            // Delete dari pending_transactions
            bool deleteSuccess = _itemController.DeletePendingTransaction(
                SessionUser.GetCurrentUser().TerminalId,
                SessionUser.GetCurrentUser().UserId,
                itemId,
                cart_session_code
            );

            if (!deleteSuccess)
            {
                MessageBox.Show("Failed to delete item from pending transactions.");
                return false;
            }

            // Update reserved stock
            int reservedStock = _itemController.GetItemReservedStock(barcode);
            int newReservedStock = reservedStock - stockNeeded;
            if (newReservedStock < 0) newReservedStock = 0;

            _itemController.UpdateReservedStock(barcode, newReservedStock);

            // Log aktivitas
            var session = SessionUser.GetCurrentUser();
            _activityService.LogAction(
                userId: session.UserId.ToString(),
                actionType: ActivityType.Cart.ToString(),
                referenceId: null,
                desc: $"Deleted item from cart: {itemId}",
                details: new
                {
                    itemId = itemId,
                    adjustmentType = reason,
                    referenceTable = "items",
                    terminal = session.TerminalId,
                    shiftId = session.ShiftId,
                    IpAddress = NetworkHelper.GetLocalIPAddress(),
                    UserAgent = GlobalContext.getAppVersion(),
                    loginId = session.LoginId,
                    cart_session_code = cart_session_code
                }
            );

            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error deleting item: " + ex.Message);
            return false;
        }
    }

    // =================== UPDATE DISCOUNT ===================
    public bool UpdateCartItemDiscount(int itemId, decimal discountPercentage, string cart_session_code)
    {
        bool success = _itemController.UpdatePendingTransactionDiscount(
            SessionUser.GetCurrentUser().TerminalId,
            SessionUser.GetCurrentUser().UserId,
            itemId,
            cart_session_code,
            discountPercentage
        );

        if (!success) MessageBox.Show("Failed to update discount in database.");
        return success;
    }

    // =================== UPDATE NOTE ===================
    public bool UpdateCartItemNote(int itemId, string note, string cart_session_code)
    {
        bool success = _itemController.UpdatePendingTransactionNote(
            SessionUser.GetCurrentUser().TerminalId,
            SessionUser.GetCurrentUser().UserId,
            itemId,
            cart_session_code,
            note
        );

        if (!success) MessageBox.Show("Failed to update note in database.");
        return success;
    }

    // =================== UPDATE QUANTITY / STOCK ===================
    public bool UpdateCartItemQuantity(int itemId, int newQty, int previousQty, string barcode, string cart_session_code)
    {
        if (newQty == 0)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove this item?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                return DeleteCartItem(itemId, barcode, previousQty, 1, "REMOVE_ITEM_FROM_CART", cart_session_code);
            }
            else
            {
                return false; // cancel, restore value di form
            }
        }
        else
        {
            // Bisa update pending stock jika ada method khusus di controller
            // Misal: _itemController.UpdatePendingTransactionQuantity(...)
            return true;
        }
    }






    public class UpdateStockResult
    {
        public bool Success { get; set; }
        public int EnteredQuantity { get; set; }
        public decimal Total { get; set; }
        public string ErrorMessage { get; set; }
    }

    public UpdateStockResult UpdateCartItemStock(
        int itemId,
        string barcode,
        string unit,
        decimal pricePerUnit,
        int enteredQuantity,
        int previousQuantity,
        bool allowAppend,
        string discount,
        string note,
        int conversionRate = 1,
        string cart_session_code = null,
        int additionalQuantity = 0)
    {




        MessageBox.Show("CALLED ");
        var result = new UpdateStockResult();

        int stockNeededOld = previousQuantity * conversionRate;
        int stockNeededNew = enteredQuantity * conversionRate;

        int currentStock = _itemController.GetItemStock(barcode);
        int reservedStock = _itemController.GetItemReservedStock(barcode);

        int newReservedStock = allowAppend ? reservedStock : reservedStock - stockNeededOld + stockNeededNew;

        // jika allowAppend true.. (berarti tambahkan dari qty yg di ambil cari searchforitem)
        // jika false berarti user ganti dari dlm car
        //int enteredQuantity = 0;
        if (allowAppend)
        {
            enteredQuantity = enteredQuantity + additionalQuantity;
        }
        else
        {
            enteredQuantity = additionalQuantity;
        }
        if (newReservedStock > currentStock)
        {
            result.Success = false;
            result.ErrorMessage = "Stock tidak cukup. Jumlah ini melebihi sisa stock yang tersedia.";
            return result;
        }

        decimal total = enteredQuantity * pricePerUnit;
        bool updateSuccess = _itemController.UpdatePendingTransactionStock(
            SessionUser.GetCurrentUser().TerminalId,
            itemId,
            enteredQuantity,
            total,
            unit,
            cart_session_code
        );

        if (!updateSuccess)
        {
            result.Success = false;
            result.ErrorMessage = "Failed to update stock in pending transactions.";
            return result;
        }

        _itemController.UpdateReservedStock(barcode, newReservedStock);

         //Logging
        _activityService.LogAction(
            userId: SessionUser.GetCurrentUser().UserId.ToString(),
            actionType: ActivityType.Cart.ToString(),
            referenceId: null,
            desc: $"Updated Item in cart: {itemId}, new reserved stock: {newReservedStock}, prev reserved stock: {stockNeededOld}",
            details: new
            {
                loginId = SessionUser.GetCurrentUser().LoginId,
                itemId = itemId,
                adjustmentType = "UPDATE_ITEM_IN_CART",
                reason = "default reason",
                referenceTable = "items",
                terminal = SessionUser.GetCurrentUser().TerminalId,
                shiftId = SessionUser.GetCurrentUser().ShiftId,
                IpAddress = NetworkHelper.GetLocalIPAddress(),
                UserAgent = GlobalContext.getAppVersion(),
                Discount = discount,
                Note = note,
                cart_session_code = cart_session_code
            }
        );


        result.Success = true;
        result.EnteredQuantity = enteredQuantity;
        result.Total = total;
        return result;
    }

}
