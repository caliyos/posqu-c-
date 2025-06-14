using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Core
{
    public interface IActivityService
    {

        /* 
            ✅ actionType → e.g., "ERROR", "CART", "PAYMENT", "LOGIN".
            ✅ message → short description, e.g., "Added item to cart", "Payment succeeded".
            ✅ details → optional: could pass an object with details (itemCode, amount, username, etc.).
         */
        void LogAction(string actionType, string message, object? details = null);

    }
}
