using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for product repository operations including retrieving, creating, updating, and deleting products.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Creates a new product in the database asynchronously.
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>The created product with updated information</returns>
        Task<Product> CreateProductAsync(Product product);
        /// <summary>
        /// Retrieves a product by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve</param>
        /// <returns>The requested product if found, otherwise null</returns>
        Task<Product> GetProductByIdAsync(int id);
        /// <summary>
        /// Retrieves all products based on the provided request parameters asynchronously.
        /// </summary>
        /// <param name="request">Request object containing query parameters for filtering and pagination</param>
        /// <returns>A tuple containing a collection of products and the total count</returns>
        Task<(IEnumerable<Product> products, int totalCount)> GetAllProductsAsync(GetAllProductsRequest request);
        /// <summary>
        /// Retrieves all product lists from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all products</returns>
        Task<IEnumerable<Product>> GetAllProductLists();
        /// <summary>
        /// Updates an existing product in the database asynchronously.
        /// </summary>
        /// <param name="product">The product data to update</param>
        /// <param name="id">The ID of the product to update</param>
        /// <returns>The updated product</returns>
        Task<Product> UpdateProductAsync(ProductDto product, int id);
        /// <summary>
        /// Deletes a product from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteProductAsync(int id);
       

    }
}
