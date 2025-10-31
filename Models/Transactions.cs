using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POS_qu.Models
{
    public class Transactions
    {
        public int TsId { get; set; }  // ts_id
        public string TsNumbering { get; set; } // ts_numbering
        public string TsCode { get; set; } // ts_code
        public decimal TsTotal { get; set; } // ts_total

        public decimal TsDiscountTotal { get; set; } // 

        public decimal TsGrandTotal { get; set; } // 

        public decimal TsPaymentAmount { get; set; } // ✅ Add this property
        public decimal TsCashback { get; set; } = 0; // ts_cashback
        public string TsMethod { get; set; } // ts_method
        public short TsStatus { get; set; } // ts_status (1: paid, 2: unpaid/utang, 3: cancelled)
        public decimal TsChange { get; set; } = 0; // ts_change
        public string TsInternalNote { get; set; } // ts_internal_note
        public string TsNote { get; set; } // ts_note
        public int? TsCustomer { get; set; } // ts_customer (nullable for optional reference)
        public string TsFreename { get; set; } // ts_freename (free text customer name)

        // Menambahkan properti untuk ShiftId dan TerminalId
        public int? ShiftId { get; set; }  // shift_id (nullable)
        public int? TerminalId { get; set; }  // terminal_id (nullable)


        public int UserId { get; set; } // yang melakukan transaksi


        public int? CreatedBy { get; set; } // created_by
        public int? UpdatedBy { get; set; } // updated_by
        public int? DeletedBy { get; set; } // deleted_by

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // created_at
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // updated_at
        public DateTime? DeletedAt { get; set; } // deleted_at (nullable)

        public int? OrderId { get; set; } // yang melakukan transaksi
    }
}
