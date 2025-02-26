
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using sti3_api.Domain.Entities;
using sti3_api.Domain.Entities.Enums;
using sti3_api.Infrastructure;

namespace sti3_api.Application.Services
{
    public class BackgroundBillingService(IServiceScopeFactory scopeFactory) : BackgroundService, IDisposable
    {
        private IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            Console.WriteLine("Send Billing Service is starting...");
            

            while (true)
            {
                await Task.Delay(10000, ct);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var billingService = scope.ServiceProvider.GetRequiredService<BillingService>();
                
                    var pendingOrders = await dbContext.Orders
                                                                    .Include(o => o.ClientCategory)
                                                                    .Include(o => o.OrderProducts!)
                                                                        .ThenInclude(op => op.Product)
                                                                    .Include(o => o.Client)
                                                                    .Where(o => o.Status == Status.Pending)
                                                                    .ToListAsync(ct);

                    foreach(var order in pendingOrders)
                    {
                        var isSended = await billingService.SendToBillingAsync(order, ct);

                        if (isSended) {
                            order.Status = Status.Completed;

                            using(var updateScope = _scopeFactory.CreateScope())
                            {
                                var updateDbContext = updateScope.ServiceProvider.GetRequiredService<AppDbContext>();
                                updateDbContext.Orders.Update(order);
                                await updateDbContext.SaveChangesAsync(ct);
                            }

                            Console.WriteLine($"Order {order.OrderId} sended and updated!");
                        }
                        else {
                            Console.WriteLine($"Fail to sendo Order {order.OrderId}");
                        }
                    }
                }
            }
        }
    }
}