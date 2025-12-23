using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for customer repository operations including retrieving, creating, updating, and deleting customers.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Creates a new customer in the database asynchronously.
        /// </summary>
        /// <param name="customer">The customer to create</param>
        /// <returns>The created customer with updated information</returns>
        Task<Customer> CreateCustomerAsync(Customer customer);
        /// <summary>
        /// Updates an existing customer in the database asynchronously.
        /// </summary>
        /// <param name="customer">The customer data to update</param>
        /// <param name="customerId">The ID of the customer to update</param>
        /// <returns>The updated customer</returns>
        Task<Customer> UpdateCustomerAsync(CustomerDto customer, int customerId);
        /// <summary>
        /// Retrieves all customers from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all customers</returns>
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        /// <summary>
        /// Retrieves a specific customer by their ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve</param>
        /// <returns>The requested customer if found, otherwise null</returns>
        Task<Customer> GetCustomerByIdAsync(int id);
        /// <summary>
        /// Deletes a customer from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the customer to delete</param>
        Task DeleteCustomerAsync(int id);


    }
}
