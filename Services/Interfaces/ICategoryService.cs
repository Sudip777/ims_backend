using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    /// <summary>
    /// Interface for category service operations including retrieving and creating categories.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Creates a new category asynchronously and returns the created category response.
        /// </summary>
        /// <param name="category">The category data transfer object containing category information</param>
        /// <returns>The created category response</returns>
        Task<CategoryResponse> CreateCategoryAsync(CategoryDto category);
        /// <summary>
        /// Retrieves all categories asynchronously and returns a collection of category responses.
        /// </summary>
        /// <returns>A collection of all category responses</returns>
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
        /// <summary>
        /// Retrieves a specific category by its ID asynchronously and returns the category response.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve</param>
        /// <returns>The requested category response if found, otherwise null</returns>
        Task<CategoryResponse> GetCategoryByIdAsync(int categoryId);
    }
}
