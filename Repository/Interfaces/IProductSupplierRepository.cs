using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for product supplier repository operations including retrieving, creating, and managing product suppliers.
    /// </summary>
    public interface IProductSupplierRepository
    {
        /// <summary>
        /// Creates a new product supplier in the database asynchronously.
        /// </summary>
        /// <param name="productSupplier">The product supplier to create</param>
        /// <returns>The created product supplier with updated information</returns>
        Task<ProductSupplier> CreateProductSupplierAsync(ProductSupplier productSupplier);
        /// <summary>
        /// Retrieves a product supplier by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product supplier to retrieve</param>
        /// <returns>The requested product supplier if found, otherwise null</returns>
        Task<ProductSupplier> GetProductSupplierByIdAsync(int id);
        /// <summary>
        /// Retrieves all product suppliers from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all product suppliers</returns>
        Task<IEnumerable<ProductSupplier>> GetAllProductSuppliersAsync();
    }
}
