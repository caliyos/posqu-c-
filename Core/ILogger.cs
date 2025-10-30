using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Core
{
    public interface ILogger
    {
        //void Log(string message);
        void Log(string userId, string actionType, int? referenceId, string? desc , string? details); // versi baru
    }

}
