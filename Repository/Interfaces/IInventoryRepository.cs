using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for inventory repository operations including retrieving, creating, updating, and managing inventory items.
    /// </summary>
    public interface IInventoryRepository
    {
        /// <summary>
        /// Creates a new inventory item in the database asynchronously.
        /// </summary>
        /// <param name="entity">The inventory item to create</param>
        /// <returns>The created inventory item with updated information</returns>
        Task<Inventory> CreateInventoryAsync(Inventory entity);
        /// <summary>
        /// Retrieves all inventory items based on the provided request parameters asynchronously.
        /// </summary>
        /// <param name="request">Request object containing query parameters for filtering and pagination</param>
        /// <returns>A tuple containing a collection of inventories and the total count</returns>
        Task<(IEnumerable<Inventory> inventories, int totalCount)> GetAllInventoriesAsync(
             GetAllInventoriesRequest request);
        /// <summary>
        /// Retrieves an inventory item by product ID and warehouse ID asynchronously.
        /// </summary>
        /// <param name="pid">The product ID</param>
        /// <param name="wid">The warehouse ID</param>
        /// <returns>The requested inventory item if found, otherwise null</returns>
        Task<Inventory> GetByProductAndWarehouseAsync(int pid, int wid);
        /// <summary>
        /// Retrieves an inventory item by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the inventory item to retrieve</param>
        /// <returns>The requested inventory item if found, otherwise null</returns>
        Task<Inventory?> GetInventoryByIdAsync(int id);
        /// <summary>
        /// Updates an existing inventory item in the database asynchronously.
        /// </summary>
        /// <param name="inventory">The inventory data to update</param>
        /// <param name="inventoryId">The ID of the inventory item to update</param>
        /// <returns>The updated inventory item</returns>
        Task<Inventory> UpdateInventoryAsync(InventoryDto inventory, int inventoryId);
        /// <summary>
        /// Updates an inventory item's quantity based on order changes asynchronously.
        /// </summary>
        /// <param name="productId">The ID of the product</param>
        /// <param name="warehouseId">The ID of the warehouse</param>
        /// <param name="quantityChange">The change in quantity</param>
        /// <returns>The updated inventory item</returns>
        Task<Inventory> UpdateInventoryFromOrderAsync(int productId, int warehouseId, int quantityChange);


    }
}
