using sti3_api.Domain.Entities.Enums;

namespace sti3_api.Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public Status Status { get; set; }

        public Guid ClientId { get; set; }

        public Client? Client { get; set; }

        public string? ClientCategoryId { get; set; }

        public required Category ClientCategory { get; set; }

        public List<OrderProduct>? OrderProducts { get; set; } 

        public decimal TotalCost { get; set; } = 0;
    }
}