using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;


namespace inventory_management_system.Services.Interfaces
{
    /// <summary>
    /// Interface for customer service operations including retrieving, creating, updating, and deleting customers.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Creates a new customer asynchronously and returns the created customer response.
        /// </summary>
        /// <param name="customer">The customer data transfer object containing customer information</param>
        /// <returns>The created customer response</returns>
        Task<CustomerResponse> CreateCustomerAsync(CustomerDto customer);
        /// <summary>
        /// Retrieves a specific customer by their ID asynchronously and returns the customer response.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve</param>
        /// <returns>The requested customer response if found, otherwise null</returns>
        Task<CustomerResponse> GetCustomerByIdAsync(int id);
        /// <summary>
        /// Retrieves all customers asynchronously and returns a collection of customer responses.
        /// </summary>
        /// <returns>A collection of all customer responses</returns>
        Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync();
        /// <summary>
        /// Updates an existing customer asynchronously and returns the updated customer response.
        /// </summary>
        /// <param name="id">The ID of the customer to update</param>
        /// <param name="customer">The customer data transfer object containing updated customer information</param>
        /// <returns>The updated customer response</returns>
        Task<CustomerResponse> UpdateCustomerAsync(int id, CustomerDto customer);
        /// <summary>
        /// Deletes a customer asynchronously by their ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteCustomerAsync(int id);
    }
}
