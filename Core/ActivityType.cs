using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Core
{
   public enum ActivityType
    {
        Error,
        Cart,
        Payment,
        Login,
        Transaction,
        System,
        Print,
        Other
    }
}
