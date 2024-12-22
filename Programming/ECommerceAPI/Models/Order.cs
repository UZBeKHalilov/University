using System.Text.Json.Serialization;

namespace ECommerceAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
