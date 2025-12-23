using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for category repository operations including retrieving and creating categories.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieves all categories from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all categories</returns>
        Task<IEnumerable<Category>>  GetAllCategoriesAsync();
        /// <summary>
        /// Retrieves a specific category by its ID asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve</param>
        /// <returns>The requested category if found, otherwise null</returns>
        Task<Category> GetCategoryByIdAsync(int categoryId);
        /// <summary>
        /// Creates a new category in the database asynchronously.
        /// </summary>
        /// <param name="category">The category to create</param>
        /// <returns>The created category with updated information</returns>
        Task<Category> CreateCategoryAsync(Category category);
    }
}
