using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IInventoryRepository
    {
        Task<Inventory> CreateInventoryAsync(Inventory entity);
        Task<(IEnumerable<Inventory> inventories, int totalCount)> GetAllInventoriesAsync(
             GetAllInventoriesRequest request);
        Task<Inventory> GetByProductAndWarehouseAsync(int pid, int wid);
        Task<Inventory?> GetInventoryByIdAsync(int id);
        Task<Inventory> UpdateInventoryAsync(InventoryDto inventory, int inventoryId);
        Task<Inventory> UpdateInventoryFromOrderAsync(int productId, int warehouseId, int quantityChange);


    }
}
