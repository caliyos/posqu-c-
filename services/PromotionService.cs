using Npgsql;
using POS_qu.Helpers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace POS_qu.Services
{
    public sealed class PromotionApplyResult
    {
        public int? PromotionId { get; set; }
        public string PromotionName { get; set; }
        public string PromotionType { get; set; }
        public string PromotionMode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CashbackAmount { get; set; }
        public List<InvoiceItem> BonusItems { get; set; } = new();
    }

    public sealed class PromotionService
    {
        public PromotionApplyResult ApplyBestPromotion(InvoiceData invoice)
        {
            if (invoice == null || invoice.Items == null || invoice.Items.Count == 0)
                return new PromotionApplyResult();

            invoice.Items.RemoveAll(i => string.Equals(i.Note, "Bonus promo", StringComparison.OrdinalIgnoreCase));

            EnsurePromotionTables();

            var promos = LoadActivePromotions(DateTime.Today);
            if (promos.Count == 0) return new PromotionApplyResult();

            var subtotal = invoice.Items.Sum(i => i.Total);
            var best = new PromotionApplyResult();

            foreach (var p in promos)
            {
                var r = Simulate(invoice, p, subtotal);
                if (r == null) continue;

                decimal score = r.DiscountAmount + r.CashbackAmount;
                decimal bestScore = best.DiscountAmount + best.CashbackAmount;
                if (score > bestScore)
                    best = r;
            }

            if ((best.DiscountAmount + best.CashbackAmount) <= 0 && (best.BonusItems == null || best.BonusItems.Count == 0))
                return new PromotionApplyResult();

            ApplyResultToInvoice(invoice, best);
            return best;
        }

        private void ApplyResultToInvoice(InvoiceData invoice, PromotionApplyResult result)
        {
            if (result == null) return;

            if (result.DiscountAmount > 0)
            {
                var subtotal = invoice.Items.Sum(i => i.Total);
                if (subtotal > 0)
                    invoice.GlobalDiscountPercent = (result.DiscountAmount / subtotal) * 100m;
                else
                    invoice.GlobalDiscountPercent = 0m;
            }

            if (result.BonusItems != null && result.BonusItems.Count > 0)
            {
                foreach (var b in result.BonusItems)
                    invoice.Items.Add(b);
            }
        }

        private PromotionApplyResult Simulate(InvoiceData invoice, PromotionRow p, decimal subtotal)
        {
            if (!string.Equals(p.Status, "aktif", StringComparison.OrdinalIgnoreCase))
                return null;

            var type = (p.PromoType ?? "").ToUpperInvariant();
            if (type == "DISKON")
            {
                var cfg = JsonDocument.Parse(p.ConfigJson ?? "{}").RootElement;
                var mode = cfg.TryGetProperty("mode", out var m) ? (m.GetString() ?? "percent") : "percent";
                var value = cfg.TryGetProperty("value", out var v) ? v.GetDecimal() : 0m;

                decimal discount = 0m;
                if (string.Equals(mode, "percent", StringComparison.OrdinalIgnoreCase))
                    discount = subtotal * value / 100m;
                else
                    discount = value;

                if (discount <= 0m) return null;
                if (discount > subtotal) discount = subtotal;

                return new PromotionApplyResult
                {
                    PromotionId = p.Id,
                    PromotionName = p.Name,
                    PromotionType = type,
                    PromotionMode = mode,
                    DiscountAmount = discount
                };
            }

            if (type == "PROMO")
            {
                var cfg = JsonDocument.Parse(p.ConfigJson ?? "{}").RootElement;
                var buyQty = cfg.TryGetProperty("buy_qty", out var bq) ? bq.GetInt32() : 0;
                var getQty = cfg.TryGetProperty("get_qty", out var gq) ? gq.GetInt32() : 0;
                var bonusId = cfg.TryGetProperty("item_bonus_id", out var bi) ? bi.GetInt32() : 0;
                var bonusName = cfg.TryGetProperty("item_bonus_name", out var bn) ? (bn.GetString() ?? "") : "";

                if (buyQty <= 0 || getQty <= 0 || bonusId <= 0) return null;

                int totalQty = invoice.Items.Where(i => i.ItemId > 0).Sum(i => i.Qty);
                int eligible = totalQty / buyQty;
                int bonusQty = eligible * getQty;
                if (bonusQty <= 0) return null;

                return new PromotionApplyResult
                {
                    PromotionId = p.Id,
                    PromotionName = p.Name,
                    PromotionType = type,
                    BonusItems = new List<InvoiceItem>
                    {
                        new InvoiceItem
                        {
                            ItemId = bonusId,
                            Barcode = "",
                            Name = bonusName,
                            Unit = "pcs",
                            UnitVariant = "pcs",
                            ConversionRate = 1,
                            Qty = bonusQty,
                            Price = 0m,
                            CostPrice = 0m,
                            DiscountPercent = 0m,
                            DiscountAmount = 0m,
                            Tax = 0m,
                            Total = 0m,
                            Note = "Bonus promo"
                        }
                    }
                };
            }

            if (type == "CASHBACK")
            {
                var cfg = JsonDocument.Parse(p.ConfigJson ?? "{}").RootElement;
                var mode = cfg.TryGetProperty("mode", out var m) ? (m.GetString() ?? "nominal") : "nominal";
                var method = cfg.TryGetProperty("method", out var mm) ? (mm.GetString() ?? "langsung") : "langsung";
                var value = cfg.TryGetProperty("value", out var v) ? v.GetDecimal() : 0m;

                decimal cashback = 0m;
                if (string.Equals(mode, "percent", StringComparison.OrdinalIgnoreCase))
                    cashback = subtotal * value / 100m;
                else
                    cashback = value;

                if (cashback <= 0m) return null;
                if (string.Equals(method, "langsung", StringComparison.OrdinalIgnoreCase))
                {
                    if (cashback > subtotal) cashback = subtotal;
                    return new PromotionApplyResult
                    {
                        PromotionId = p.Id,
                        PromotionName = p.Name,
                        PromotionType = type,
                        PromotionMode = method,
                        DiscountAmount = cashback
                    };
                }

                return new PromotionApplyResult
                {
                    PromotionId = p.Id,
                    PromotionName = p.Name,
                    PromotionType = type,
                    PromotionMode = method,
                    CashbackAmount = cashback
                };
            }

            return null;
        }

        private sealed class PromotionRow
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string PromoType { get; set; }
            public string Status { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int Priority { get; set; }
            public string ConfigJson { get; set; }
        }

        private List<PromotionRow> LoadActivePromotions(DateTime today)
        {
            var list = new List<PromotionRow>();
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
SELECT id, name, promo_type, status, start_date, end_date, priority, config_json
FROM promotions
WHERE status = 'aktif'
  AND (start_date IS NULL OR start_date <= @d)
  AND (end_date IS NULL OR end_date >= @d)
ORDER BY priority DESC, id DESC
", con);
            cmd.Parameters.AddWithValue("@d", today.Date);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new PromotionRow
                {
                    Id = Convert.ToInt32(r["id"]),
                    Name = r["name"]?.ToString() ?? "",
                    PromoType = r["promo_type"]?.ToString() ?? "",
                    Status = r["status"]?.ToString() ?? "",
                    StartDate = r["start_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(r["start_date"]),
                    EndDate = r["end_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(r["end_date"]),
                    Priority = r["priority"] == DBNull.Value ? 0 : Convert.ToInt32(r["priority"]),
                    ConfigJson = r["config_json"]?.ToString() ?? "{}"
                });
            }
            return list;
        }

        private void EnsurePromotionTables()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand(@"
CREATE TABLE IF NOT EXISTS promotions (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    promo_type VARCHAR(20) NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'aktif',
    start_date DATE NULL,
    end_date DATE NULL,
    priority INT NOT NULL DEFAULT 0,
    config_json TEXT NOT NULL DEFAULT '{}',
    created_by INT NULL REFERENCES users(id) ON DELETE SET NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);
", con);
            cmd.ExecuteNonQuery();
        }
    }
}
