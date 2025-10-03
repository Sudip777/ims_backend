using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Enums;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderStatus = inventory_management_system.Enums.OrderStatus;
using TransactionType = inventory_management_system.Enums.TransactionType;


namespace inventory_management_system.Services.Implementations
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService _userService;
        private readonly IInventoryRepository _inventoryRepository; // For stock updates
        private readonly IInventoryTransactionHistoryService _inventoryTransactionService;
        private readonly ApplicationDBContext _context;

        public OrderService(IOrderRepository orderRepository, IInventoryRepository inventoryRepository, ApplicationDBContext context, IUserService userService, IInventoryTransactionHistoryService inventoryTransactionService)
        {
            _orderRepository = orderRepository;
            _inventoryRepository = inventoryRepository;
            _userService = userService;
            _context = context;
            _inventoryTransactionService = inventoryTransactionService;
          
        }

        public async Task<OrderResponse> CreateOrderAsync(OrderDto orderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Map DTO to entity
                var order = orderDto.MappedOrder();

                if (order.OrderDetails == null || !order.OrderDetails.Any())
                    throw new InvalidOperationException("Order must contain at least one order detail.");

                // Ensure WarehouseId and ProductId are valid (not zero)
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.ProductId <= 0)
                        throw new InvalidOperationException($"Invalid ProductId {detail.ProductId}.");

                    if (detail.WarehouseId <= 0)
                        throw new InvalidOperationException($"Invalid WarehouseId {detail.WarehouseId}.");
                }

                // Calculate total
                order.TotalAmount = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
                order.CreatedByUserId = _userService.GetCurrentUserId();

                // Validate products and warehouses exist
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(detail.ProductId);
                    if (product == null)
                        throw new InvalidOperationException($"ProductId {detail.ProductId} not found.");

                    var warehouse = await _context.Warehouses.FindAsync(detail.WarehouseId);
                    if (warehouse == null)
                        throw new InvalidOperationException($"WarehouseId {detail.WarehouseId} not found.");
                }
                var createdOrder = await _orderRepository.AddAsync(order);

                // Update inventory and log transaction history
                foreach (var detail in createdOrder.OrderDetails)
                {
                    var inventory = await _inventoryRepository.GetByProductAndWarehouseAsync(detail.ProductId, detail.WarehouseId);
                    if (inventory == null)
                        throw new InvalidOperationException($"No inventory found for ProductId {detail.ProductId} in WarehouseId {detail.WarehouseId}");

                    if (inventory.Quantity < detail.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for ProductId {detail.ProductId} in WarehouseId {detail.WarehouseId}");

                    inventory.Quantity -= detail.Quantity;
                    await _inventoryRepository.UpdateInventoryFromOrderAsync(detail.ProductId, detail.WarehouseId, -detail.Quantity);

                    // Log transaction
                    var transactionDto = new InventoryTransactionHistoryDto
                    {
                        ProductId = detail.ProductId,
                        WarehouseId = detail.WarehouseId,
                        QuantityChange = -detail.Quantity,
                        TransactionTypeId = (int)TransactionType.Sale,
                        OrderId = createdOrder.OrderId,
                        Details = $"Order created: OrderId {createdOrder.OrderId}",
                        TransactionDate = DateTime.UtcNow,
                        UserId = _userService.GetCurrentUserId()
                    };
                    await _inventoryTransactionService.CreateInventoryTransactionHistoryAsync(transactionDto);
                }

                await transaction.CommitAsync();

                // Map to response
                var response = new OrderResponse
                {
                    OrderId = createdOrder.OrderId,
                    CustomerId = createdOrder.CustomerId,
                    CustomerName = createdOrder.Customer?.Name,
                    OrderDate = createdOrder.OrderDate,
                    StatusId = createdOrder.StatusId,
                    StatusName = createdOrder.Status?.Name,
                    TotalAmount = createdOrder.TotalAmount,
                    CreatedByUserId = createdOrder.CreatedByUserId,
                    OrderDetails = createdOrder.OrderDetails.Select(od => new OrderDetailResponse
                    {
                        ProductId = od.ProductId,
                        WarehouseId = od.WarehouseId,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    }).ToList()
                };

                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public Task<bool> DeleteOrderAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Order ID must be greater than zero.", nameof(id));

            return _orderRepository.DeleteOrderAsync(id);
        }

        public Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
           var orders =  _context.Orders
                .Include(o => o.OrderDetails)
                .Select(o => new OrderResponse
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,
                    OrderDate = o.OrderDate,
                    StatusId = o.StatusId,
                    StatusName = o.Status.Name,
                    TotalAmount = o.TotalAmount,
                    CreatedByUserId = o.CreatedByUserId,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailResponse
                    {
                        OrderDetailId = od.OrderDetailId,
                        ProductId = od.ProductId,
                        WarehouseId = od.WarehouseId,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    }).ToList()
                }).AsEnumerable();
            return Task.FromResult(orders);
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Order ID must be greater than zero.", nameof(id));

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderId == id)  
                .Select(o => new OrderResponse
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,
                    OrderDate = o.OrderDate,
                    StatusId = o.StatusId,
                    StatusName = o.Status.Name,
                    TotalAmount = o.TotalAmount,
                    CreatedByUserId = _userService.GetCurrentUserId(),
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailResponse
                    {
                        OrderDetailId = od.OrderDetailId,
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            return order;
        }

        public async Task<OrderResponse> UpdateOrderAsync( OrderDto orderDto, int id)
        {
            if (id <= 0)
                throw new ArgumentException("Order ID must be greater than zero.", nameof(id));

            var orderResponse = await _orderRepository.GetByIdAsync(id);
            if (orderResponse == null)
            {
                throw new KeyNotFoundException($"Inventory with ID {id} not found.");
            }
            var updatedOrder = await _orderRepository.UpdateOrderAsync(orderDto, id);

            return OrderResponse.MappedOrderResponse(updatedOrder);

        }


        public async Task<OrderResponse> UpdateOrderStatusAsync(int id, int newStatusId)
        {
            if (id <= 0)
                throw new ArgumentException("Order ID must be greater than zero.", nameof(id));

            if (newStatusId <= 0)
                throw new ArgumentException("Status ID must be greater than zero.", nameof(id));

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            if (newStatusId != order.StatusId + 1 && newStatusId != 5)
                throw new InvalidOperationException("Invalid status transition.");

            var updatedOrder = await _orderRepository.UpdateStatusAsync(id, newStatusId);

            return OrderResponse.MappedOrderResponse(updatedOrder); 
        }

       
    }
}
