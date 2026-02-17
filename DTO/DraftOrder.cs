using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.DTO
{
    public class DraftOrder
    {
        public int PoId { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }

        // opsional tapi biasanya ada
        public string CartSessionCode { get; set; }
    }

}
