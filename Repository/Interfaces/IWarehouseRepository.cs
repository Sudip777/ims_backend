using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for warehouse repository operations including retrieving, creating, updating, and managing warehouses.
    /// </summary>
    public interface IWarehouseRepository
    {
        /// <summary>
        /// Creates a new warehouse in the database asynchronously.
        /// </summary>
        /// <param name="warehouse">The warehouse to create</param>
        /// <returns>The created warehouse with updated information</returns>
        Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse);
        /// <summary>
        /// Retrieves all warehouses from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all warehouses</returns>
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync();
        /// <summary>
        /// Retrieves a warehouse by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the warehouse to retrieve</param>
        /// <returns>The requested warehouse if found, otherwise null</returns>
        Task<Warehouse> GetWarehouseByIdAsync(int id);
        /// <summary>
        /// Updates an existing warehouse in the database asynchronously.
        /// </summary>
        /// <param name="warehouse">The warehouse data to update</param>
        /// <param name="id">The ID of the warehouse to update</param>
        /// <returns>The updated warehouse</returns>
        Task<Warehouse> UpdateWarehouseAsync(WarehouseDto warehouse, int id);
        /// <summary>
        /// Retrieves a warehouse by its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the warehouse to retrieve</param>
        /// <returns>The requested warehouse if found, otherwise null</returns>
        Task<Warehouse?> GetByNameAsync(string name);
    }
}
