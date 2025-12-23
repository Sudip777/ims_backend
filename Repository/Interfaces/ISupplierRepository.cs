using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for supplier repository operations including retrieving, creating, updating, and deleting suppliers.
    /// </summary>
    public interface ISupplierRepository
    {
        /// <summary>
        /// Creates a new supplier in the database asynchronously.
        /// </summary>
        /// <param name="supplier">The supplier to create</param>
        /// <returns>The created supplier with updated information</returns>
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        /// <summary>
        /// Updates an existing supplier in the database asynchronously.
        /// </summary>
        /// <param name="supplier">The supplier data to update</param>
        /// <param name="id">The ID of the supplier to update</param>
        /// <returns>The updated supplier</returns>
        Task<Supplier> UpdateSupplierAsync(SupplierDto supplier, int id);
        /// <summary>
        /// Retrieves all suppliers from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all suppliers</returns>
        Task<IEnumerable<Supplier>> GetAllSupplierAsync();
        /// <summary>
        /// Retrieves a supplier by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the supplier to retrieve</param>
        /// <returns>The requested supplier if found, otherwise null</returns>
        Task<Supplier> GetSupplierByIdAsync(int id);
        /// <summary>
        /// Deletes a supplier from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the supplier to delete</param>
        Task DeleteSupplierAsync(int id);
    }
}
