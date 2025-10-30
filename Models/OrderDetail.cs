using System;

namespace POS_qu.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }       // order_detail_id
        public int OrderId { get; set; }             // order_id
        public int ItemId { get; set; }              // item_id
        public string Barcode { get; set; }          // od_barcode
        public decimal Quantity { get; set; }        // od_quantity
        public string Unit { get; set; }             // od_unit
        public decimal PricePerUnit { get; set; }    // od_price_per_unit

        public string UnitVariant { get; set; }      // od_unit_variant
        public decimal ConversionRate { get; set; }  // od_conversion_rate

        public decimal SellPrice { get; set; }       // od_sell_price
        public decimal Total { get; set; }           // od_total

        public string Note { get; set; }             // od_note

        public decimal DiscountPerItem { get; set; }     // od_discount_per_item
        public decimal DiscountPercentage { get; set; }  // od_discount_percentage
        public decimal DiscountTotal { get; set; }       // od_discount_total
        public decimal Tax { get; set; }                 // od_tax

        public int CreatedBy { get; set; }           // created_by
        public int? UpdatedBy { get; set; }          // updated_by
        public int? DeletedBy { get; set; }          // deleted_by

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
