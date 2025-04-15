using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{
    // Payment class to map payment details
    public class Payment
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }

        public Payment(decimal amount, string paymentMethod, DateTime paymentDate)
        {
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentDate = paymentDate;
        }
    }
}
