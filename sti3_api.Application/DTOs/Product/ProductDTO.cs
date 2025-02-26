using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace sti3_api.Application.DTOs.Product
{
    public class ProductDTO
    {
        [Required]
        [DisplayName("produtoId")]
        [JsonPropertyName("produtoId")]
        public int ProductId { get; set; }

        [Required, MinLength(5), MaxLength(100)]
        [DisplayName("descricao")]
        [JsonPropertyName("descricao")]
        public required string Description { get; set; }

        [Required]
        [DisplayName("quantidade")]
        [JsonPropertyName("quantidade")]
        [Precision(18, 2)]
        [Range(0, double.MaxValue, ErrorMessage = "The quantity cannot be negative.")]
        public decimal Quantity { get; set; }

        [Required]
        [DisplayName("precoUnitario")]
        [JsonPropertyName("precoUnitario")]
        [Precision(18, 2)]
        [Range(0, double.MaxValue, ErrorMessage = "The price cannot be negative.")]
        public decimal UnitPrice { get; set; }
    }
}