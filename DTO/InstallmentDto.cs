using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.DTO
{
    public class InstallmentDto
    {
        public int Id { get; set; }                     // ID record cicilan (transaction_installments)
        public int TransactionId { get; set; }         // ID transaksi
        public string TransactionNumber { get; set; }  // ts_numbering
        public string TransactionCode { get; set; }    // ts_code
        public decimal TotalAmount { get; set; }       // ts_total
        public decimal DueAmount { get; set; }         // ts_due_amount
        public decimal Amount { get; set; }            // jumlah cicilan yang dibayarkan (ti.amount)
        public string Note { get; set; }               // catatan cicilan
        public string CreatedByName { get; set; }      // nama user yang buat cicilan
        public DateTime CreatedAt { get; set; }        // tanggal cicilan dibuat,

    }
}
