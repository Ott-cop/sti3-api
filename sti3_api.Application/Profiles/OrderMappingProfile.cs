using AutoMapper;
using sti3_api.Application.DTOs.Billing;
using sti3_api.Application.DTOs.Client;
using sti3_api.Application.DTOs.Order;
using sti3_api.Application.DTOs.Product;
using sti3_api.Application.Services;
using sti3_api.Domain.Entities;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        var paymentService = new PaymentService();
        CreateMap<OrderDTO, Order>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Client!.ClientId))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => new Client
            {
                ClientId = src.Client!.ClientId,
                Name = src.Client.Name,
                Cpf = src.Client.Cpf
            }))
            .ForMember(dest => dest.ClientCategory, opt => opt.Ignore())
            .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.Items!.Select(itemDTO => new OrderProduct
            {
                ProductId = itemDTO.ProductId, 
                Quantity = itemDTO.Quantity,
                UnitPrice = itemDTO.UnitPrice
            }).ToList()));

        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId)) 
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate)) 
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => new ClientDTO
            {
                ClientId = src.Client!.ClientId,
                Name = src.Client.Name,
                Cpf = src.Client.Cpf,        
                Category = src.ClientCategory!.Name  
            }))            
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderProducts!
                .Select(op => new ProductDTO
                {
                    ProductId = op.Product!.ProductId, 
                    Description = op.Product.Description, 
                    Quantity = op.Quantity,
                    UnitPrice = op.UnitPrice 
                }).ToList()));

        CreateMap<Order, BillingDTO>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.TotalCost))
            .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => paymentService.DiscountValue(src.ClientCategory!, src.TotalCost)))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => paymentService.CalculateDiscount(src.ClientCategory!, src.TotalCost)))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderProducts!.Select(itemDTO => new ProductBillingDTO
            {
                Quantity = itemDTO.Quantity,
                UnitPrice = itemDTO.UnitPrice
            }).ToList()));
    }




}
