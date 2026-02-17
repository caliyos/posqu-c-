using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.DTO
{
    public class TransactionDto
    {
        public int Id { get; set; }                  // ts_id
        public string TransactionNumber { get; set; } // ts_numbering
        public decimal TotalAmount { get; set; }    // ts_total
        public decimal AmountPaid { get; set; }     // ts_payment_amount
        public decimal DueAmount { get; set; }      // ts_due_amount
        public int Status { get; set; }          // ts_status (paid / partial / unpaid)
        public string TransactionCode { get; set; } // ts_code, kalau perlu
        public decimal GrandTotal { get; set; }     // ts_grand_total
        public decimal DeliveryAmount { get; set; } // ts_delivery_amount
        public decimal GlobalDiscountAmount { get; set; } // ts_global_discount_amount
        public decimal GlobalDiscountPercent { get; set; } // ts_global_discount_percent
        public int CustomerId { get; set; }         // ts_customer
        public string FreeName { get; set; }        // ts_freename
        public int UserId { get; set; }             // created_by / user_id
        public DateTime CreatedAt { get; set; }     // created_at
        public DateTime UpdatedAt { get; set; }     // updated_at
    }
}
