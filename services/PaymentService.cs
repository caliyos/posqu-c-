using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.services
{
    public class PaymentService
    {
        public decimal GlobalDiscountPercent { get; set; } = 0;
        public decimal Cashback { get; set; } = 0;
        public decimal PaymentAmount { get; set; } = 0;
        public string GlobalNote { get; set; } = null;
        public decimal DeliveryAmount { get; set; } = 0;

        public decimal GrandTotal { get; set; } = 0;
        public string PaymentMethod { get; set; }

        public void Reset()
        {
            GlobalDiscountPercent = 0;
            Cashback = 0;
            PaymentAmount = 0;
            GlobalNote = null;
            DeliveryAmount = 0;
            GrandTotal = 0;
            PaymentMethod = null;
        }
    }

}
