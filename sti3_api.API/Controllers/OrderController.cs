using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sti3_api.Application.DTOs.Billing;
using sti3_api.Application.DTOs.Order;
using sti3_api.Application.DTOs.Product;
using sti3_api.Application.Services;
using sti3_api.Domain.Entities;
using sti3_api.Infrastructure;

namespace sti3_api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(AppDbContext dbContext, IMapper mapper, IPaymentService paymentService, OrderService orderService) : ControllerBase
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly IPaymentService _paymentService = paymentService;
        private readonly OrderService _orderService = orderService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDTO req, CancellationToken ct)
        {
            try
            {
                var order = _mapper.Map<OrderDTO>(await _orderService.CreateOrderAsync(req, ct));
                return Ok(new GetOrderDTO(order, order.TotalCost));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var orders = await _dbContext.Orders
                .Include(o => o.ClientCategory)
                .Include(o => o.OrderProducts!)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Client)
                .ToListAsync(ct);

            var getOrderListDTO = new List<GetOrderDTO>();

            foreach (var order in orders)
            {
                var orderDTO = new GetOrderDTO(_mapper.Map<OrderDTO>(order), order.TotalCost);

                getOrderListDTO.Add(orderDTO);
            }

            return Ok(getOrderListDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var order = await _dbContext.Orders
                .Include(o => o.ClientCategory)
                .Include(o => o.OrderProducts!)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Client)
                .FirstOrDefaultAsync(o => o.OrderId == id, ct);

            if (order == null) {
                return NotFound();
            }
           
            return Ok(new GetOrderDTO(_mapper.Map<OrderDTO>(order), order.TotalCost));
        }

        [HttpPut("{id}/{product_id}")]
        public async Task<IActionResult> Update(Guid id, int product_id, [FromBody] EditProductDTO req, CancellationToken ct)
        {
            var order = await _dbContext.Orders
                .Include(o => o.ClientCategory)
                .Include(o => o.OrderProducts!)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Client)
                .FirstOrDefaultAsync(o => o.OrderId == id, ct);

            if (order == null) {
                return NotFound();
            }

            var product = order.OrderProducts!.FirstOrDefault(i => i.ProductId == product_id);

            if (product == null) {
                return NotFound("Product not found");
            }

            if (product.Quantity != req.Quantity && req.Quantity != 0)
                product.Quantity = req.Quantity;
            if (product.UnitPrice != req.UnitPrice && req.UnitPrice != 0)
                product.UnitPrice = req.UnitPrice;

            order.TotalCost = _paymentService.
                                CalculateDiscount(order.ClientCategory, _paymentService.CalculateCost(order.OrderProducts));

            var orderDTO = _mapper.Map<OrderDTO>(order);

            order = _mapper.Map<Order>(orderDTO);

            try
            {
                await _dbContext.SaveChangesAsync(ct);

                return Ok(orderDTO);
            }
            catch (Exception ex)
            {   
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/{product_id}")]
        public async Task<IActionResult> InsertProduct(Guid id, int product_id, [FromBody] EditProductDTO req, CancellationToken ct)
        {
            var order = await _dbContext.Orders
                .Include(o => o.ClientCategory)
                .Include(o => o.Client)
                .Include(o => o.OrderProducts!)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id, ct);

            if (order == null) {
                return NotFound();
            }

            var find_product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == product_id, ct);

            if (find_product == null) {
               return NotFound("Product not found"); 
            }

            var product = order.OrderProducts!.FirstOrDefault(i => i.ProductId == product_id);

            if (product != null) {
                return Conflict("Product already in Order");
            }

            await _dbContext.OrderProducts.AddAsync(new OrderProduct 
            {
                OrderId = order.OrderId,
                ProductId = product_id,
                Quantity = req.Quantity,
                UnitPrice = req.UnitPrice,
            }, ct);
            
            order.TotalCost = _paymentService.
                                CalculateDiscount(order.ClientCategory, _paymentService.CalculateCost(order.OrderProducts));
            try
            {   
                await _dbContext.SaveChangesAsync(ct);

                var newProducts = await _dbContext.OrderProducts
                    .Include(op => op.Product)
                    .Where(op => op.OrderId == order.OrderId)
                    .ToListAsync(ct);

                order.OrderProducts = newProducts;

                var orderDTO = _mapper.Map<OrderDTO>(order);

                return Ok(orderDTO);
            }
            catch (Exception ex)
            {   
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}/{product_id}")]
        public async Task<IActionResult> DeleteProduct(Guid id, int product_id, CancellationToken ct)
        {
            var order = await _dbContext.Orders
                .Include(o => o.ClientCategory)
                .Include(o => o.OrderProducts!)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Client)
                .FirstOrDefaultAsync(o => o.OrderId == id, ct);

            if (order == null) {
                return NotFound();
            }

            var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(i => i.ProductId == product_id, ct);

            if (product == null) {
                return NotFound("Product not found");
            }

            var item = order.OrderProducts!.FirstOrDefault(i => i.ProductId == product_id);

            if (item == null) {
                return NotFound("Product are not in list");
            }

            order.OrderProducts!.Remove(item);
            order.TotalCost = _paymentService.CalculateDiscount(order.ClientCategory,_paymentService.CalculateCost(order.OrderProducts));
            try
            {
                await _dbContext.SaveChangesAsync(ct);

                var orderDTO = _mapper.Map<OrderDTO>(order);
                return Ok(orderDTO);
            }
            catch (Exception ex)
            {   
                return BadRequest(ex.Message);
            }

        }

        
    }
}