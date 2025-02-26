using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sti3_api.Application.DTOs.Billing;
using sti3_api.Application.DTOs.Client;
using sti3_api.Application.DTOs.Order;
using sti3_api.Application.DTOs.Product;
using sti3_api.Domain.Entities;
using sti3_api.Domain.Entities.Enums;
using sti3_api.Infrastructure;

namespace sti3_api.Application.Services
{
    public class OrderService(AppDbContext dbContext, IMapper mapper, IPaymentService paymentService, BillingService billingService)
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly IPaymentService _paymentService = paymentService;
        private readonly BillingService _billingService = billingService;


        public async Task<Order> CreateOrderAsync(OrderDTO req, CancellationToken ct)
        {   
            var client = await GetOrCreateClientAsync(req.Client, ct);

            var category = await GetCategoryAsync(req.Client.Category, ct);

            await UpdateOrAddProductsAsync(req.Items, ct);

            var order = await CreateOrderWithCalculationsAsync(req, category, client, ct);

            try
            {
                await _billingService.SendToBillingAsync(order, ct);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            

            return order;
        } 

        private async Task<Client> GetOrCreateClientAsync(ClientDTO clientDTO, CancellationToken ct)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == clientDTO.ClientId, ct);
            
            if (client == null)
            {
                client = new Client
                {
                    ClientId = clientDTO.ClientId,
                    Name = clientDTO.Name,
                    Cpf = clientDTO.Cpf
                };
                await _dbContext.Clients.AddAsync(client, ct);
            }
            else
            {
                if (client.Name != clientDTO.Name)
                    client.Name = clientDTO.Name;
                if (client.Cpf != clientDTO.Cpf)
                    client.Cpf = clientDTO.Cpf;
                
                _dbContext.Clients.Update(client);
            }
            await _dbContext.SaveChangesAsync(ct);

            return client;
        }

        private async Task<Category> GetCategoryAsync(string categoryName, CancellationToken ct)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName, ct);

            if (category == null)
                throw new Exception("Category not found!");

            return category;
        }

        private async Task UpdateOrAddProductsAsync(List<ProductDTO> items, CancellationToken ct)
        {
            foreach (var item in items)
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId, ct);

                if (product == null)
                {
                    product = new Product
                    {
                        ProductId = item.ProductId,
                        Description = item.Description,
                        UnitPrice = Math.Round(item.UnitPrice, 2)
                    };
                    await _dbContext.Products.AddAsync(product, ct);
                }
                else
                {
                    if (product.Description != item.Description)
                        product.Description = item.Description;
                    if (product.UnitPrice != item.UnitPrice)
                        product.UnitPrice = item.UnitPrice;
                    
                    _dbContext.Products.Update(product);
                }
            }
            await _dbContext.SaveChangesAsync(ct);
        }

        private async Task<Order> CreateOrderWithCalculationsAsync(OrderDTO req, Category category, Client client, CancellationToken ct)
        {
            var order = _mapper.Map<Order>(req);

            var price = _paymentService.CalculateCost(order.OrderProducts);

            order.TotalCost = price;
            order.Client = client;
            order.ClientId = client.ClientId;
            order.ClientCategory = category;
            order.ClientCategoryId = category.Name;
            order.Status = Status.Pending;

            await _dbContext.Orders.AddAsync(order, ct);
            await _dbContext.SaveChangesAsync(ct);

            return order;
        }
    }
}