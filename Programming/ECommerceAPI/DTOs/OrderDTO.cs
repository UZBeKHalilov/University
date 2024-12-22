namespace ECommerceAPI.DTOs
{
    public class OrderCreateDTO
    {
        public int CustomerId { get; set; }
        public ICollection<OrderItemCreateDTO> OrderItems { get; set; }
    }

    public class OrderItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
