using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    /// <summary>
    /// Interface for product service operations including retrieving, creating, updating, and deleting products.
    /// </summary>
    public interface  IProductService
    {
        /// <summary>
        /// Registers a new product asynchronously and returns the created product response.
        /// </summary>
        /// <param name="dto">The product data transfer object containing product information</param>
        /// <returns>The created product response</returns>
        Task<ProductResponse> RegisterProductAsync(ProductDto dto);
        /// <summary>
        /// Retrieves a specific product by its ID asynchronously and returns the product response.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve</param>
        /// <returns>The requested product response if found, otherwise null</returns>
        Task<ProductResponse?> GetProductByIdAsync(int id);
        /// <summary>
        /// Retrieves all products based on the provided request parameters asynchronously and returns a paged response of product responses.
        /// </summary>
        /// <param name="req">Request object containing query parameters for filtering and pagination</param>
        /// <returns>A paged response containing product responses</returns>
        Task<PagedResponse<ProductResponse>> GetAllProductsAsync(GetAllProductsRequest req);
        /// <summary>
        /// Retrieves all product lists asynchronously and returns a collection of product dropdown responses.
        /// </summary>
        /// <returns>A collection of product dropdown responses</returns>
        Task <IEnumerable<ProductDropdownResponse>> GetAllProductLists();

        /// <summary>
        /// Updates an existing product asynchronously and returns the updated product response.
        /// </summary>
        /// <param name="productId">The ID of the product to update</param>
        /// <param name="dto">The product data transfer object containing updated product information</param>
        /// <returns>The updated product response</returns>
        Task<ProductResponse> UpdateProductAsync(int productId, ProductDto dto);
        /// <summary>
        /// Deletes a product asynchronously by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteProductAsync(int productId);
 


    }
}
