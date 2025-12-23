using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for order repository operations including retrieving, creating, updating, and deleting orders.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Retrieves an order by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve</param>
        /// <returns>The requested order if found, otherwise null</returns>
        Task<Order> GetByIdAsync(int id);
        /// <summary>
        /// Retrieves all orders based on the provided request parameters asynchronously.
        /// </summary>
        /// <param name="req">Request object containing query parameters for filtering and pagination</param>
        /// <returns>A tuple containing a collection of orders and the total count</returns>
        Task<(IEnumerable<Order> orders, int totalCount)> GetAllAsync(GetAllOrdersRequest req);
        /// <summary>
        /// Creates a new order in the database asynchronously.
        /// </summary>
        /// <param name="order">The order to create</param>
        /// <returns>The created order with updated information</returns>
        Task<Order> AddAsync(Order order);
        /// <summary>
        /// Updates an existing order in the database asynchronously.
        /// </summary>
        /// <param name="order">The order data to update</param>
        /// <param name="id">The ID of the order to update</param>
        /// <returns>The updated order</returns>
        Task<Order> UpdateOrderAsync(OrderDto order, int id);
        /// <summary>
        /// Deletes an order from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteOrderAsync(int id);
        /// <summary>
        /// Updates the status of an existing order in the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order to update</param>
        /// <param name="newStatusId">The new status ID</param>
        /// <returns>The updated order</returns>
        Task<Order> UpdateStatusAsync(int id, int newStatusId);
    }
}
