using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{
    class GlobalContext
    {
        public static string getAppVersion()
        {

            return $"PosQu/{Application.ProductVersion} ({Environment.OSVersion})";
        }
    }
}
