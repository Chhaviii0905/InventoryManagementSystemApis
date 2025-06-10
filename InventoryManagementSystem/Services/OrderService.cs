using InventoryManagementSystem.DTOs;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Services
{
    public class OrderService
    {
        private readonly InventoryDbContext _context;

        private readonly ActivityLogService _logService;

        public OrderService(InventoryDbContext context, ActivityLogService logService)
        {
            _context = context;
            _logService = logService;
        }


        public async Task<List<OrderDto>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.Customer) // 👈 Include customer
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = (DateTime)o.OrderDate,
                    UserId = o.UserId,
                    UserName = o.User != null ? o.User.Username : null,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer != null ? o.Customer.Name : null,
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        OrderItemId = oi.OrderItemId,
                        ProductId = (int)oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.Customer) // 👈 Include customer
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return null;

            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = (DateTime)order.OrderDate,
                UserId = order.UserId,
                UserName = order.User?.Username,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.Name,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = (int)oi.ProductId,
                    ProductName = oi.Product?.Name ?? string.Empty,
                    Quantity = oi.Quantity
                }).ToList()
            };
        }

        public async Task<OrderDto?> CreateAsync(CreateOrderDto dto)
        {
            var productIds = dto.Items.Select(i => i.ProductId).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId);

            foreach (var item in dto.Items)
            {
                if (!products.ContainsKey(item.ProductId))
                    throw new ArgumentException($"Product with ID {item.ProductId} does not exist.");

                if (products[item.ProductId].Quantity < item.Quantity)
                    throw new ArgumentException($"Insufficient stock for product ID {item.ProductId}.");
            }

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                throw new ArgumentException($"User with ID {dto.UserId} does not exist.");

            var customer = await _context.Customers.FindAsync(dto.CustomerId); // 👈 Validate customer
            if (customer == null)
                throw new ArgumentException($"Customer with ID {dto.CustomerId} does not exist.");

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                UserId = dto.UserId,
                CustomerId = dto.CustomerId, // 👈 Add CustomerId
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            foreach (var item in dto.Items)
            {
                products[item.ProductId].Quantity -= item.Quantity;
            }

            _context.Orders.Add(order);
            await _logService.LogAsync(
                action: "Order Created",
                performedBy: user.Username,
                entity: "Order",
                entityId: order.OrderId.ToString()
            );

            await _context.SaveChangesAsync();

            return await GetByIdAsync(order.OrderId);
        }
    }
}
