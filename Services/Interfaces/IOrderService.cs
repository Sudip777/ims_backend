using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(OrderDto orderDto);
        Task<OrderResponse> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse> UpdateOrderAsync( OrderDto orderDto, int id);
        Task<bool> DeleteOrderAsync(int id);
        Task<OrderResponse> UpdateOrderStatusAsync(int id, int newStatusId);
    }
}
