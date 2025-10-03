using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IInventoryTransactionHistoryRepository
    {
        Task<InventoryTransactionHistory> CreateInventoryTransactionHistoryAsync(InventoryTransactionHistory transaction);
        Task<IEnumerable<InventoryTransactionHistory>> GetAllInventoryTransactionHistoriesAsync();
    }
}
