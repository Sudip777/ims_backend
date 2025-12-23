using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    /// <summary>
    /// Interface for order service operations including retrieving, creating, updating, and deleting orders.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order asynchronously and returns the created order response.
        /// </summary>
        /// <param name="orderDto">The order data transfer object containing order information</param>
        /// <returns>The created order response</returns>
        Task<OrderResponse> CreateOrderAsync(OrderDto orderDto);
        /// <summary>
        /// Retrieves a specific order by its ID asynchronously and returns the order response.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve</param>
        /// <returns>The requested order response if found, otherwise null</returns>
        Task<OrderResponse> GetOrderByIdAsync(int id);
        /// <summary>
        /// Retrieves all orders based on the provided request parameters asynchronously and returns a paged response of order responses.
        /// </summary>
        /// <param name="request">Request object containing query parameters for filtering and pagination</param>
        /// <returns>A paged response containing order responses</returns>
        Task<PagedResponse<OrderResponse>> GetAllOrdersAsync(GetAllOrdersRequest request);
        /// <summary>
        /// Updates an existing order asynchronously and returns the updated order response.
        /// </summary>
        /// <param name="orderDto">The order data transfer object containing updated order information</param>
        /// <param name="id">The ID of the order to update</param>
        /// <returns>The updated order response</returns>
        Task<OrderResponse> UpdateOrderAsync( OrderDto orderDto, int id);
        /// <summary>
        /// Deletes an order asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteOrderAsync(int id);
        /// <summary>
        /// Updates the status of an existing order asynchronously and returns the updated order response.
        /// </summary>
        /// <param name="id">The ID of the order to update</param>
        /// <param name="newStatusId">The new status ID</param>
        /// <returns>The updated order response</returns>
        Task<OrderResponse> UpdateOrderStatusAsync(int id, int newStatusId);
    }
}
