using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class OrderDetailResponse
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int WarehouseId { get; set; }
        public string ProductName { get; set; } = null!;
        // Maps the entity model to this DTO
        public static OrderDetailResponse MappedOrderDetailResponse(OrderDetail orderDetail)
        {
            return new OrderDetailResponse
            {
                OrderDetailId = orderDetail.OrderDetailId,
                ProductId = orderDetail.ProductId,
                WarehouseId = orderDetail.WarehouseId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
                ProductName = orderDetail.Product?.Name ?? String.Empty
            };
        }
    }
}
