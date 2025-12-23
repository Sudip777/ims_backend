using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for inventory transaction history repository operations including creating and retrieving transaction history records.
    /// </summary>
    public interface IInventoryTransactionHistoryRepository
    {
        /// <summary>
        /// Creates a new inventory transaction history record in the database asynchronously.
        /// </summary>
        /// <param name="transaction">The inventory transaction history to create</param>
        /// <returns>The created transaction history record with updated information</returns>
        Task<InventoryTransactionHistory> CreateInventoryTransactionHistoryAsync(InventoryTransactionHistory transaction);
        /// <summary>
        /// Retrieves all inventory transaction history records from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all inventory transaction history records</returns>
        Task<IEnumerable<InventoryTransactionHistory>> GetAllInventoryTransactionHistoriesAsync();
    }
}
