using System;

namespace POS_qu.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // store, warehouse, kitchen
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}