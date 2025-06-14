using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POS_qu.Models
{
    public class TransactionDetail
    {
        public int TsdId { get; set; } // Primary Key
        public int TsId { get; set; } // Foreign Key to Transactions
        public int ItemId { get; set; } // Foreign Key to Items
        public string Barcode { get; set; } 
        public decimal TsdSellPrice { get; set; } // Price of the item at purchase time
        public decimal TsdQuantity { get; set; } // Quantity of the item
        public string TsdUnit { get; set; } // Unit of measurement (e.g., kg, pcs)
        public string TsdNote { get; set; } // Additional notes
        public decimal TsdDiscountPerItem { get; set; } // Discount per item
        public decimal TsdDiscountPercentage { get; set; } // Discount percentage
        public decimal TsdDiscountTotal { get; set; } // Total discount amount
        public decimal TsdTax { get; set; } // Tax applied to the item
        public decimal TsdTotal { get; set; } // Final total per item (after discount & tax)
        
        public decimal TsdConversionRate { get; set; }
        public decimal TsdPricePerUnit { get; set; } // Final total per item (after discount & tax)
        public string TsdUnitVariant { get; set; } // Unit of measurement (e.g., kg, pcs)

        public int CreatedBy { get; set; } // User who created the record
        public DateTime CreatedAt { get; set; } // Timestamp when created
        public int? UpdatedBy { get; set; } // Nullable, in case of updates
        public DateTime? UpdatedAt { get; set; } // Timestamp for updates
        public int? DeletedBy { get; set; } // Nullable, in case of deletion
        public DateTime? DeletedAt { get; set; } // Timestamp for deletion
    }
}
