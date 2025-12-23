using inventory_management_system.Data;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    /// <summary>
    /// Implementation of the category repository interface providing methods for category data access operations.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to use for data access operations</param>
        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Creates a new category in the database asynchronously.
        /// </summary>
        /// <param name="category">The category to create</param>
        /// <returns>The created category with updated information</returns>
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        /// <summary>
        /// Retrieves all categories from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all categories</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
           return await _context.Categories.ToListAsync();
        }

       

        /// <summary>
        /// Retrieves a specific category by its ID asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve</param>
        /// <returns>The requested category if found, otherwise null</returns>
        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }
    }
}
