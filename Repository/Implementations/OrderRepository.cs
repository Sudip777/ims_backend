using Azure.Core;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Extensions;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class OrderRepository : IOrderRepository

    {
        private readonly ApplicationDBContext _context;

        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
              return  await _context.Orders
             .AsNoTracking() 
             .Include(o => o.OrderDetails)
             .Include(o => o.Customer)
             .Include(o => o.Status)
             .FirstOrDefaultAsync(o => o.OrderId == id);

        }

        public async Task<(IEnumerable<Order> orders, int totalCount)> GetAllAsync(GetAllOrdersRequest req)
        {
            // call stored procedure with dynamic sort and pagination 
            var orders = await _context.Set<Order>()
                .FromSqlInterpolated($@"
            EXEC dbo.GetAllOrdersData
                @CustomerId = {req.CustomerId},
                @Search = {req.Search},
                @SortColumn = {req.SortColumn},
                @SortDirection = {req.SortDirection},
                @Page = {req.Page},
                @PageSize = {req.PageSize}")
                .AsNoTracking() // result will not be tracked by context
                .ToListAsync();

            var totalCount = await _context.Orders
                .AsQueryable()
                .Where(o => (!req.CustomerId.HasValue || o.CustomerId == req.CustomerId.Value)
                            && (string.IsNullOrEmpty(req.Search)
                                || o.Customer.Name.Contains(req.Search)
                                || o.OrderId.ToString().Contains(req.Search)))
                .CountAsync();

            return (orders, totalCount);
        }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                throw new InvalidOperationException($"Database error while creating order: {inner}");
            }

            // Load navigation properties
            var createdOrder = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            return createdOrder!;
        }

        public async Task<Order> UpdateOrderAsync(OrderDto orderDto, int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            // Update
            order.StatusId = orderDto.StatusId;
            order.CustomerId = orderDto.CustomerId;
            order.OrderDate = DateTime.UtcNow;

            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Add
            order.OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                 WarehouseId = od.WarehouseId,
            }).ToList();

            // Recalculate total
            order.TotalAmount = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
                return true;
        }

        public async Task<Order> UpdateStatusAsync(int id, int newStatusId)
        {
            var order = await GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            order.StatusId = newStatusId;
            await _context.SaveChangesAsync();

            return order;
        }

    }
}
