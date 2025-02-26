using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace sti3_api.Application.DTOs.Product
{
    public class EditProductDTO
    {
        [DisplayName("quantidade")]
        [JsonPropertyName("quantidade")]
        [Precision(18, 2)]
        [Range(0, double.MaxValue, ErrorMessage = "The quantity cannot be negative.")]
        public decimal Quantity { get; set; }

        [DisplayName("precoUnitario")]
        [JsonPropertyName("precoUnitario")]
        [Precision(18, 2)]
        [Range(0, double.MaxValue, ErrorMessage = "The price cannot be negative.")]
        public decimal UnitPrice { get; set; }
    }
}