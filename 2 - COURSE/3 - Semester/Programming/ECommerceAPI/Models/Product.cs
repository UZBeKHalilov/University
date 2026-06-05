using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }

        // Foreign key for Category
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } // Virtual for lazy loading
    }
}
