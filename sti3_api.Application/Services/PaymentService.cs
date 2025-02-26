using sti3_api.Domain.Entities;

namespace sti3_api.Application.Services
{
    public interface IPaymentService
    {
        decimal CalculateCost(List<OrderProduct>? orderProductList);
        decimal CalculateDiscount(Category category, decimal price);
    }

    public class PaymentService : IPaymentService
    {
        public decimal CalculateCost(List<OrderProduct>? orderProductList)
        {
            decimal price = 0;
            
            foreach (var item in orderProductList!)
            {
                price += item.UnitPrice * item.Quantity;
            }

            return Math.Round(price, 2);
        }

        public decimal CalculateDiscount(Category category, decimal price)
        {
            decimal discount = 0;

            if (price >= category.ConditionalDiscount) 
                discount = price * category.PercentDiscount; 

            return Math.Round(price - discount, 2);
        }

        public decimal DiscountValue(Category category, decimal price)
        {
            decimal discount = 0;

            if (price >= category.ConditionalDiscount) 
                discount = price * category.PercentDiscount; 

            return Math.Round(discount, 2);
        }
    }
}