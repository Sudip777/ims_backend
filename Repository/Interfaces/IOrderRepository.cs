using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> AddAsync(Order order);
        Task<Order> UpdateOrderAsync(OrderDto order, int id);
        Task<bool> DeleteOrderAsync(int id);
        Task<Order> UpdateStatusAsync(int id, int newStatusId);
    }
}
