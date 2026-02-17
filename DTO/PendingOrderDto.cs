using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.DTO
{
    public class PendingOrderDto
    {
        public int PoId { get; set; }
        public string? CustomerName { get; set; }
        public string? Note { get; set; }
        public string CartSessionCode { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
