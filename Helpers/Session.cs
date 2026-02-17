using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{
    public static class Session
    {
        public static string CartSessionCode { get;  set; }

        public static void StartNewCart()
        {
            CartSessionCode = "CART-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
        }

        public static void ClearCart()
        {
            CartSessionCode = null;
        }
    }

}
