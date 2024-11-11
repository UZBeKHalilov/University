using Microsoft.EntityFrameworkCore;

namespace ShopAPI.Models;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) 
        : base(options) 
    { 
    }

    public DbSet<Product> Products { get; set; } = null!;


}
