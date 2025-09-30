using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; } = 0;
        public string? CustomerName { get; set; }
        public int StatusId { get; set; } = 0;
        public string? StatusName { get; set; }
        public int CreatedByUserId { get; set; }

        public List<OrderDetailResponse>? OrderDetails { get; set; } // Nested details
        
        public static OrderResponse MappedOrderResponse(Order orderEntity)
        {
            return new OrderResponse
            {
                OrderId = orderEntity.OrderId,
                OrderDate = orderEntity.OrderDate,
                CustomerName = orderEntity.Customer?.Name,
                CustomerId = orderEntity.CustomerId,
                StatusId = orderEntity.StatusId,
                StatusName = orderEntity.Status?.Name,
                TotalAmount = orderEntity.TotalAmount,
                CreatedByUserId = orderEntity.CreatedByUserId,
                OrderDetails = orderEntity.OrderDetails
                                     .Select(od => OrderDetailResponse.MappedOrderDetailResponse(od))
                                     .ToList()
            };
        }

    }
}
