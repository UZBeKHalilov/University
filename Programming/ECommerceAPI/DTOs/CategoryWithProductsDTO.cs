namespace ECommerceAPI.DTOs
{
    public class CategoryWithProductsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductNoCategoryDTO> Products { get; set; } = new List<ProductNoCategoryDTO>();
    }
}
