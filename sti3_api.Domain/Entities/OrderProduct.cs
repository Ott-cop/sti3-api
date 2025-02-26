namespace sti3_api.Domain.Entities
{
    public class OrderProduct
    {
        public Guid OrderProductId { get; set; }
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}