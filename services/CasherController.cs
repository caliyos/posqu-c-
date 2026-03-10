using POS_qu.Helpers;
using POS_qu.Models;
using POS_qu.services;

namespace POS_qu.services
{
    public class CasherController
    {
        private readonly CartService _cartService;

        public CasherController(CartService cartService)
        {
            _cartService = cartService;
        }

        public InvoiceData LoadCartForResume(string? currentCartCode, string newCartCode)
        {
            if (!string.IsNullOrEmpty(currentCartCode) && currentCartCode != newCartCode)
            {
                Session.ClearCart();
            }
            var invoice = _cartService.LoadInvoiceFromCartSession(newCartCode);
            return invoice;
        }
    }
}
