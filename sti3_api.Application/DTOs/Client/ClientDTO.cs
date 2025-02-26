using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace sti3_api.Application.DTOs.Client
{
    public class ClientDTO
    {
        [Required]
        [DisplayName("clienteId")]
        [JsonPropertyName("clienteId")]
        public Guid ClientId { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        [DisplayName("nome")]
        [JsonPropertyName("nome")]
        public string? Name { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF must be in the format 000.000.000-00")]
        [DisplayName("cpf")]
        [JsonPropertyName("cpf")]
        public string? Cpf { get; set; }

        [Required]
        [DisplayName("categoria")]
        [JsonPropertyName("categoria")]
        public required string Category { get; set; }
    }
}