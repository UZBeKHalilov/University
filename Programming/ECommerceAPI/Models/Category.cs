﻿using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property for related products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>(); // Virtual for lazy loading
    }
}
