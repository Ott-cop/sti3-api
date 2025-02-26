namespace sti3_api.Domain.Entities
{
    public class Client
    {
        public Guid ClientId { get; set; }

        public string? Name { get; set; }

        public string? Cpf { get; set; }

        public List<Order> Orders { get; set; } = [];

    }
}