using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface IInventoryTransactionHistoryService
    
        {
        Task<InventoryTransactionHistoryResponse> CreateInventoryTransactionHistoryAsync(InventoryTransactionHistoryDto transaction);
        Task<IEnumerable<InventoryTransactionHistoryResponse>> GetAllInventoryTransactionHistoriesAsync();
    }
}
