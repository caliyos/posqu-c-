using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{
   public class Orders
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string OrderCode { get; set; }
        public decimal OrderTotal { get; set; }
        public int OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryMethod { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string OrderNote { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int TerminalId { get; set; }
        public int ShiftId { get; set; }
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
    }
}
