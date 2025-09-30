using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Implementations;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using inventory_management_system.Validations;
using Microsoft.EntityFrameworkCore;


namespace inventory_management_system.Services.Implementations
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService _userService;
        private readonly IInventoryRepository _inventoryRepository; // For stock updates
        private readonly ApplicationDBContext _context;

        public OrderService(IOrderRepository orderRepository, IInventoryRepository inventoryRepository, ApplicationDBContext context, IUserService userService)
        {
            _orderRepository = orderRepository;
            _inventoryRepository = inventoryRepository;
            _userService = userService;
            _context = context;
          
        }
       
        

        public async Task<OrderResponse> CreateOrderAsync(OrderDto orderDto)
        {
            // Validate
            var validator = new BusinessValidations();
            validator.ValidateOrder(orderDto);   // now throws if invalid

            // 2. Map DTO
            var order = orderDto.MappedOrder();
            order.OrderDate = DateTime.UtcNow;

            if (order.OrderDetails == null)
                order.OrderDetails = new List<OrderDetail>();

            // total
            order.TotalAmount = order.OrderDetails.Any()
                ? order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)
                : 0m;   // default to 0 if no details

            order.CreatedByUserId = _userService.GetCurrentUserId();
            var createdOrder = await _orderRepository.AddAsync(order);

            //Map to response DTO
            var response = new OrderResponse
            {
                OrderId = createdOrder.OrderId,
                CustomerId = createdOrder.CustomerId,
                CustomerName = createdOrder.Customer.Name,

                OrderDate = createdOrder.OrderDate,
                StatusId = createdOrder.StatusId,
                StatusName = createdOrder.Status.Name,
                TotalAmount = createdOrder.TotalAmount,
                CreatedByUserId = createdOrder.CreatedByUserId,
                OrderDetails = createdOrder.OrderDetails.Select(od => new OrderDetailResponse
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            };

            return response;
        }


        public Task<bool> DeleteOrderAsync(int id)
        {
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
                .Where(o => o.OrderId == id)  // filter by ID
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
