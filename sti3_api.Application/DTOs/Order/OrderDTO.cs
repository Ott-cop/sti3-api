using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using sti3_api.Application.DTOs.Client;
using sti3_api.Application.DTOs.Product;

namespace sti3_api.Application.DTOs.Order
{
    public class OrderDTO
    {
        [Required]
        [DisplayName("identificador")]
        [JsonPropertyName("identificador")]
        public Guid OrderId { get; set; }

        [Required]
        [DisplayName("dataVenda")]
        [JsonPropertyName("dataVenda")]
        public DateTime OrderDate { get; set; }
        
        [Required]
        [DisplayName("cliente")]
        [JsonPropertyName("cliente")]
        public required ClientDTO Client { get; set; }

        [Required]
        [DisplayName("itens")]
        [JsonPropertyName("itens")]
        public List<ProductDTO> Items { get; set; } = [];

        [JsonIgnore]
        [Precision(18, 2)]
        public decimal TotalCost { get; set; }
    }
}