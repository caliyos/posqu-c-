using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }

        // identity
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // membership
        public bool IsMember { get; set; }
        public string MemberCode { get; set; }

        // price level (JOIN table price_levels)
        public int? PriceLevelId { get; set; }
        public string PriceLevelName { get; set; }

        // loyalty
        public int Points { get; set; }

        // note
        public string Note { get; set; }

        // system
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
