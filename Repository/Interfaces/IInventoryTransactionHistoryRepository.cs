using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IInventoryTransactionRepository
    {
        Task<InventoryTransactionHistory> CreateTransactionHistoryAsync(InventoryTransactionHistory transaction);
        Task<IEnumerable<InventoryTransactionHistory>> GetInventoryTransactionHistoriesAsync();
    }
}
