using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Kode { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Category> Children { get; set; } = new List<Category>();
    }
}
