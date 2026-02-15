using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{

    public class StockLog
    {
        public int ProductId { get; set; }
        public string TipeTransaksi { get; set; }   // "payment", "purchase", etc
        public decimal QtyMasuk { get; set; }
        public decimal QtyKeluar { get; set; }
        public decimal SisaStock { get; set; }
        public string Keterangan { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LoginId { get; set; }
    }


}
