using Azure.Core;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class OrderRepository:IOrderRepository

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
           
            var query = _context.Orders
                .Include(i => i.Customer)
                .Include(i=>i.OrderDetails)
                  .ThenInclude(od => od.Product)
                 .Include(o => o.Status)
                .AsQueryable();

            // Filtering
            if (req.CustomerId.HasValue)
                query = query.Where(i => i.CustomerId == req.CustomerId.Value);

            if (req.StartDate.HasValue)
                query = query.Where(i => i.OrderDate >= req.StartDate.Value);

            if (req.EndDate.HasValue)
                query = query.Where(i => i.OrderDate <= req.EndDate.Value);

            // Total count
            var totalCount = await query.CountAsync();

        // sorting and pagination
        var orders = await query
            .OrderBy(i => i.CustomerId)
            .Skip((req.Page - 1) * req.PageSize)
            .Take(req.PageSize)
            .ToListAsync();
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
