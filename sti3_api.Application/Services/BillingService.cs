using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoMapper;
using dotenv.net;
using sti3_api.Application.DTOs.Billing;
using sti3_api.Domain.Entities;

namespace sti3_api.Application.Services
{
    public class BillingService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ILogger<BillingService> _logger;
        private readonly string BillingUrl = GlobalConfig.Dotenv["BILLING"];
        private readonly string Email = GlobalConfig.Dotenv["EMAIL"];

        public BillingService(HttpClient httpClient, ILogger<BillingService> logger, IMapper mapper)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> SendToBillingAsync(Order order, CancellationToken ct)
        {
            var billingDTO = _mapper.Map<BillingDTO>(order);

            var json = JsonSerializer.Serialize(billingDTO);

            var content = new StringContent(json, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json"));

            content.Headers.Add("email", Email);

            try
            {
                var response = await _httpClient.PostAsync(BillingUrl, content, ct);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Order {order.OrderId} billed with success. {response.StatusCode}");
                    
                    return true;
                }
                
                _logger.LogWarning($"Error billed order {order.OrderId}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to communicate with billing: {ex.Message}");
                return false;
            }
        }
    }

}