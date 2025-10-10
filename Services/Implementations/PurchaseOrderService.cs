using Azure.Core;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Implementations;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Services.Implementations
{
    public class PurchaseOrderService: IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IUserService _userService;
        private readonly ApplicationDBContext _context;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository, ApplicationDBContext context, IUserService userService)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _context = context;
            _userService = userService;
        }

        public async Task<PurchaseOrderResponse> CreatePurchaseOrderAsync(PurchaseOrderDto orderDto)
        {
            var order = orderDto.MappedPurchaseOrder();
            order.CreatedByUserId = _userService.GetCurrentUserId();

            if (order.PurchaseOrderDetails == null)
                order.PurchaseOrderDetails = new List<PurchaseOrderDetail>();

            order.TotalAmount = order.PurchaseOrderDetails.Any()
                ? order.PurchaseOrderDetails.Sum(d => d.Quantity * d.UnitPrice)
                : 0m;

            var createdPurchaseOrder = await _purchaseOrderRepository.AddPurchaseOrderAsync(order);
            return PurchaseOrderResponse.MappedPurchaseOrderResponse(createdPurchaseOrder);
        }



        public async Task<PagedResponse<PurchaseOrderResponse>> GetAllPurchaseOrdersAsync(GetAllPurchaseOrdersRequest request)
        {

            // Validate pagination parameters
            if (request.Page < 1) request.Page = 1;
            if (request.PageSize < 1 || request.PageSize > 100) request.PageSize = 10;
            var (orders, totalCount) = await _purchaseOrderRepository.GetAllPurchaseOrderAsync(request);

            var responses = orders.Select(PurchaseOrderResponse.MappedPurchaseOrderResponse).ToList();
            return new PagedResponse<PurchaseOrderResponse>
            {
                Data = responses,
                Meta = new PagedResponse<PurchaseOrderResponse>.MetaData
                {
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize
                }
            };
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
                    SupplierId = o.SupplierId,
                    SupplierName = o.Supplier.Name,
                    StatusId = o.StatusId,
                    StatusName = o.Status.Name,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    CreatedByUserId = o.CreatedByUserId,
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

            if (newStatusId <= 0)
                throw new ArgumentException(" Status ID must be greater than zero.", nameof(id));

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
