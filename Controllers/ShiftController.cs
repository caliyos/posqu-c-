using System.Collections.Generic;

namespace POS_qu.Controllers
{
    public class ShiftController
    {
        public Dictionary<string, object>? GetOpenShift(int userId, int terminalId)
        {
            return null;
        }

        public int OpenShift(int userId, int terminalId, decimal openingCash)
        {
            return 1;
        }

        public void CloseShift(int shiftId, decimal expectedCash, decimal closingCash)
        {
        }
    }
}
