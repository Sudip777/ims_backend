using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Services.Implementations
{
    public class PurchaseOrderService: IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ApplicationDBContext _context;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository, ApplicationDBContext context)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _context = context;
        }

        public async Task<PurchaseOrderResponse> CreatePurchaseOrderAsync(PurchaseOrderDto orderDto)
        {
            // 2. Map DTO
            var order = orderDto.MappedPurchaseOrder();
            order.OrderDate = DateTime.UtcNow;

            if (order.PurchaseOrderDetails == null)
                order.PurchaseOrderDetails = new List<PurchaseOrderDetail>();

            // total
            order.TotalAmount = order.PurchaseOrderDetails.Any()
                ? order.PurchaseOrderDetails.Sum(d => d.Quantity * d.UnitPrice)
                : 0m;   // default to 0 if no details

            var createdPurchaseOrder = await _purchaseOrderRepository.AddPurchaseOrderAsync(order);

            //Map to response DTO
            var response = new PurchaseOrderResponse
            {
                PurchaseOrderId = createdPurchaseOrder.PurchaseOrderId,
                SuplierId = createdPurchaseOrder.SupplierId,
                SupplierName = createdPurchaseOrder.Supplier.Name ,
                StatusId = createdPurchaseOrder.StatusId,
                StatusName = createdPurchaseOrder.Status.Name,
                TotalAmout = createdPurchaseOrder.TotalAmount,
                PurchaseOrderDetails = createdPurchaseOrder.PurchaseOrderDetails.Select(od => new PurchaseOrderDetailResponse
                {
                    PurchaseOrderDetailId = od.PurchaseOrderDetailId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            };

            return response;
        }

        public Task<PurchaseOrderResponse> CreatePurchaseOrderAsync(OrderDto orderDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PurchaseOrderResponse>> GetAllPurchaseOrdersAsync()
        {
            var orders = _context.PurchaseOrders
                .Include(o => o.PurchaseOrderDetails)
                .Select(o => new PurchaseOrderResponse
                {
                    PurchaseOrderId = o.PurchaseOrderId,
                    SuplierId = o.SupplierId,
                    SupplierName = o.Supplier.Name,
                    StatusId = o.StatusId,
                    StatusName = o.Status.Name,
                    TotalAmout = o.TotalAmount,
                    PurchaseOrderDetails = o.PurchaseOrderDetails.Select(od => new PurchaseOrderDetailResponse
                    {
                        PurchaseOrderDetailId = od.PurchaseOrderDetailId,
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    }).ToList()
                }).AsEnumerable();
            return Task.FromResult(orders);
        }

        public Task<PurchaseOrderResponse> GetPurchaseOrderByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(" Purchase Order ID must be greater than zero.", nameof(id));

            var orders = _context.PurchaseOrders
                .Include(o => o.PurchaseOrderDetails)
                .Select(o => new PurchaseOrderResponse
                {
                    PurchaseOrderId = o.PurchaseOrderId,
                    SuplierId = o.SupplierId,
                    SupplierName = o.Supplier.Name,
                    StatusId = o.StatusId,
                    StatusName = o.Status.Name,
                    TotalAmout = o.TotalAmount,
                    PurchaseOrderDetails = o.PurchaseOrderDetails.Select(od => new PurchaseOrderDetailResponse
                    {
                        PurchaseOrderDetailId = od.PurchaseOrderDetailId,
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    }).ToList()
                }).FirstOrDefaultAsync();
          

            if (orders == null)
                throw new KeyNotFoundException($" Purchase Order with ID {id} not found.");

            return orders;
        }

        public async Task<PurchaseOrderResponse> UpdateOrderStatusAsync(int id, int newStatusId)
        {
            if (id <= 0)
                throw new ArgumentException(" Purchase Order ID must be greater than zero.", nameof(id));

            var order = await _purchaseOrderRepository.GetPurchaseOrderByIdAsync(id);

            if (order == null)
                throw new KeyNotFoundException("Purchase Order not found.");

            if (newStatusId != order.StatusId + 1 && newStatusId != 5)
                throw new InvalidOperationException("Invalid status transition.");

            var updatedOrder = await _purchaseOrderRepository.UpdatePurchaseOrderStatusAsync(id, newStatusId);

            return PurchaseOrderResponse.MappedPurchaseOrderResponse(updatedOrder);
        }

        public async Task<PurchaseOrderResponse> UpdatePurchaseOrderAsync(PurchaseOrderDto orderDto, int id)
        {
            if (id <= 0)
                throw new ArgumentException(" Purchase Order ID must be greater than zero.", nameof(id));

            var orderResponse = await _purchaseOrderRepository.GetPurchaseOrderByIdAsync(id);
            if (orderResponse == null)
            {
                throw new KeyNotFoundException($"Purchase Order with ID {id} not found.");
            }
            var updatedOrder = await _purchaseOrderRepository.UpdatePurchaseOrderAsync(orderDto, id);

            return PurchaseOrderResponse.MappedPurchaseOrderResponse(updatedOrder);
        }

       
    }
}
