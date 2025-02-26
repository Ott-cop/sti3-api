using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using sti3_api.Application.DTOs.Product;
using sti3_api.Domain.Entities.Enums;

namespace sti3_api.Application.DTOs.Billing
{
    public class BillingDTO
    {
        [DisplayName("identificador")]
        [JsonPropertyName("identificador")]
        public Guid OrderId { get; set; }

        [DisplayName("subtotal")]
        [JsonPropertyName("subtotal")]
        [Precision(18, 2)]
        public decimal SubTotal { get; set; }

        [DisplayName("descontos")]
        [JsonPropertyName("descontos")]
        [Precision(18, 2)]
        public decimal Discounts { get; set; }

        [DisplayName("valorTotal")]
        [JsonPropertyName("valorTotal")]
        [Precision(18, 2)]
        public decimal TotalCost { get; set; }

        [DisplayName("itens")]
        [JsonPropertyName("itens")]
        public List<ProductBillingDTO>? Items { get; set; }
    }
}