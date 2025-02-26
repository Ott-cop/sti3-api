namespace sti3_api.Domain.Entities
{
    public class Category
    {
        public required string Name { get; set; }
        public decimal PercentDiscount { get; set; }
        public decimal ConditionalDiscount { get; set; }
    }
}