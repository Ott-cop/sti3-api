using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace sti3_api.Application.DTOs.Order
{
    public class GetOrderDTO
    {
        [JsonPropertyName("order")]
        public OrderDTO Order { get; set; }

        [JsonPropertyName("valorTotal")]
        [Precision(18, 2)]
        public decimal TotalCost { get; set; }

        public GetOrderDTO(OrderDTO order, decimal totalCost)
        {
            this.Order = order;
            this.TotalCost = totalCost;
        }
    }
}