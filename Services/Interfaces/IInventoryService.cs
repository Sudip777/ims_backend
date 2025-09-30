using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface IInventoryService
    {

        Task<InventoryResponse> CreateInventoryAsync(InventoryDto inventory);
        Task<IEnumerable<InventoryResponse>> GetAllInventoryAsync();
        Task<InventoryResponse?> GetInventoryByIdAsync(int id);
        Task<InventoryResponse> UpdateInventoryAsync(InventoryDto inventory, int inventoryId);
        Task<IEnumerable<InventoryResponse>> GetLowStocks();
    }
}
