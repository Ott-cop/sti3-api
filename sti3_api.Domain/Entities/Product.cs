namespace sti3_api.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public required string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
}