using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IInventoryRepository
    {
        Task<Inventory> CreateInventoryAsync(Inventory entity);
        Task<IEnumerable<Inventory>> GetAllInventoryAsync(Inventory inventory);
        Task<Inventory?> GetInventoryByIdAsync(int id);
        Task<Inventory> UpdateInventoryAsync(InventoryDto inventory, int inventoryId);
    }
}
