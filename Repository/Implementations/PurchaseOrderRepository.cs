using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDBContext _context;

        public PurchaseOrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id)
        {
            return await _context.PurchaseOrders.Include(p => p.Supplier)
            .Include(p => p.Status)
            .Include(p => p.PurchaseOrderDetails).FirstOrDefaultAsync(o => o.PurchaseOrderId == id);

        }

        public async Task<(IEnumerable<PurchaseOrder> purchaseOrders, int totalCount)> GetAllPurchaseOrderAsync(GetAllPurchaseOrdersRequest req)
        {

            var query = _context.PurchaseOrders
                .Include(i => i.Supplier)
                .AsQueryable();

            // Filtering
            if (req.SupplierId.HasValue)
                query = query.Where(i => i.SupplierId == req.SupplierId.Value);

            if (req.StartDate.HasValue)
                query = query.Where(i => i.OrderDate >= req.StartDate.Value);

            if (req.EndDate.HasValue)
                query = query.Where(i => i.OrderDate <= req.EndDate.Value);

            // Total count
            var totalCount = await query.CountAsync();

            // sorting and pagination
            var orders = await query
                .OrderBy(i => i.SupplierId)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();
            return (orders, totalCount);
        }

        public async Task<PurchaseOrder> AddPurchaseOrderAsync(PurchaseOrder order)
        {
            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();

            // Reload with navigation properties
            var createdOrder = await _context.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.Status)
                .Include(o => o.PurchaseOrderDetails)
                .FirstOrDefaultAsync(o => o.PurchaseOrderId == order.PurchaseOrderId);

            return createdOrder!;
        }



        public async Task<PurchaseOrder> UpdatePurchaseOrderAsync(PurchaseOrderDto orderDto, int id)
        {
            var order = await _context.PurchaseOrders.FirstOrDefaultAsync(o => o.PurchaseOrderId == id);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            // Update
            order.StatusId = orderDto.StatusId;
            order.OrderDate = DateTime.UtcNow;

            // Remove existing details
            _context.PurchaseOrderDetails.RemoveRange(order.PurchaseOrderDetails);

            // Add new details
            order.PurchaseOrderDetails = orderDto.PurchaseOrderDetails.Select(od => new PurchaseOrderDetail
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            }).ToList();

            // Recalculate total
            order.TotalAmount = order.PurchaseOrderDetails.Sum(d => d.Quantity * d.UnitPrice);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<PurchaseOrder> UpdatePurchaseOrderStatusAsync(int id, int newStatusId)
        {
            var order = await GetPurchaseOrderByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            order.StatusId = newStatusId;
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
