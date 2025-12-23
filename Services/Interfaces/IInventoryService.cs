using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    /// <summary>
    /// Interface for inventory service operations including retrieving, creating, updating, and managing inventory items.
    /// </summary>
    public interface IInventoryService
    {

        /// <summary>
        /// Creates a new inventory item asynchronously and returns the created inventory response.
        /// </summary>
        /// <param name="inventory">The inventory data transfer object containing inventory information</param>
        /// <returns>The created inventory response</returns>
        Task<InventoryResponse> CreateInventoryAsync(InventoryDto inventory);
        /// <summary>
        /// Retrieves all inventory items based on the provided request parameters asynchronously and returns a paged response of inventory responses.
        /// </summary>
        /// <param name="request">Request object containing query parameters for filtering and pagination</param>
        /// <returns>A paged response containing inventory responses</returns>
        Task<PagedResponse<InventoryResponse>> GetAllInventoryAsync(GetAllInventoriesRequest request);
        /// <summary>
        /// Retrieves a specific inventory item by its ID asynchronously and returns the inventory response.
        /// </summary>
        /// <param name="id">The ID of the inventory item to retrieve</param>
        /// <returns>The requested inventory response if found, otherwise null</returns>
        Task<InventoryResponse?> GetInventoryByIdAsync(int id);
        /// <summary>
        /// Updates an existing inventory item asynchronously and returns the updated inventory response.
        /// </summary>
        /// <param name="inventory">The inventory data transfer object containing updated inventory information</param>
        /// <param name="inventoryId">The ID of the inventory item to update</param>
        /// <returns>The updated inventory response</returns>
        Task<InventoryResponse> UpdateInventoryAsync(InventoryDto inventory, int inventoryId);
        /// <summary>
        /// Retrieves inventory items that are low in stock asynchronously and returns a collection of inventory responses.
        /// </summary>
        /// <returns>A collection of low stock inventory responses</returns>
        Task<IEnumerable<InventoryResponse>> GetLowStocks();
    }
}
