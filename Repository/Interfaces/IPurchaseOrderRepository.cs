using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for purchase order repository operations including retrieving, creating, updating, and managing purchase orders.
    /// </summary>
    public interface IPurchaseOrderRepository
    {
        /// <summary>
        /// Retrieves a purchase order by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the purchase order to retrieve</param>
        /// <returns>The requested purchase order if found, otherwise null</returns>
        Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id);
        /// <summary>
        /// Retrieves all purchase orders based on the provided request parameters asynchronously.
        /// </summary>
        /// <param name="req">Request object containing query parameters for filtering and pagination</param>
        /// <returns>A tuple containing a collection of purchase orders and the total count</returns>
        Task<(IEnumerable<PurchaseOrder> purchaseOrders, int totalCount)> GetAllPurchaseOrderAsync(GetAllPurchaseOrdersRequest req);
        /// <summary>
        /// Creates a new purchase order in the database asynchronously.
        /// </summary>
        /// <param name="order">The purchase order to create</param>
        /// <returns>The created purchase order with updated information</returns>
        Task<PurchaseOrder> AddPurchaseOrderAsync(PurchaseOrder order);
        /// <summary>
        /// Updates an existing purchase order in the database asynchronously.
        /// </summary>
        /// <param name="order">The purchase order data to update</param>
        /// <param name="id">The ID of the purchase order to update</param>
        /// <returns>The updated purchase order</returns>
        Task<PurchaseOrder> UpdatePurchaseOrderAsync(PurchaseOrderDto order, int id);
        /// <summary>
        /// Updates the status of an existing purchase order in the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the purchase order to update</param>
        /// <param name="newStatusId">The new status ID</param>
        /// <returns>The updated purchase order</returns>
        Task<PurchaseOrder> UpdatePurchaseOrderStatusAsync(int id, int newStatusId);
    }
}
