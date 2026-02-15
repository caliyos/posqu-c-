using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.DTO
{
    public class UpdateCartItemRequest
    {
        public int ItemId { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }
        public int UnitId { get; set; }

        public decimal PricePerUnit { get; set; }

        public int EnteredQuantity { get; set; }
        public int PreviousQuantity { get; set; }

        public bool AllowAppend { get; set; }

        public string Discount { get; set; }
        public string Note { get; set; }

        public int ConversionRate { get; set; } = 1;
        public string CartSessionCode { get; set; }
        public int AdditionalQuantity { get; set; }
        public bool IsEditMode { get; set; }
    }

    public class DeleteCartItemRequest
    {
        public int ItemId { get; set; }
        public int UnitId { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public int ConversionRate { get; set; } = 1;

        public string Reason { get; set; } = "DELETE_ITEM_FROM_CART";
        public string CartSessionCode { get; set; }
        public bool IsPaymentMode { get; set; } = false;
    }

}
