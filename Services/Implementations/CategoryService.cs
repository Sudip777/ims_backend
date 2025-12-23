using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using static inventory_management_system.Constants.ApiRoutes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace inventory_management_system.Services.Implementations
{
    /// <summary>
    /// Implementation of the category service interface providing business logic for category operations.
    /// </summary>
    public class CategoryService : ICategoryService

    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDBContext _context;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository for data access operations</param>
        /// <param name="context">The database context for data access operations</param>
        public CategoryService(ICategoryRepository categoryRepository, ApplicationDBContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }
        /// <summary>
        /// Creates a new category asynchronously and returns the created category response.
        /// </summary>
        /// <param name="category">The category data transfer object containing category information</param>
        /// <returns>The created category response</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no categories are found</exception>
        public async Task<CategoryResponse> CreateCategoryAsync(CategoryDto category)
        {
           
            var entity = category.MappedCategory();
            var createdCategory = await _categoryRepository.CreateCategoryAsync(entity);
            if (createdCategory == null) throw new KeyNotFoundException("No Categories Found.");


            return new CategoryResponse
            {
                CategoryId = createdCategory.CategoryId,
                CategoryName = createdCategory.CategoryName!,
                ParentCategoryId = createdCategory.ParentCategoryId,

            };
        }

 

        /// <summary>
        /// Retrieves all categories asynchronously and returns a collection of category responses.
        /// </summary>
        /// <returns>A collection of all category responses</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no categories are found</exception>
        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            if(categories == null) throw new KeyNotFoundException("No Categories Found.");
            
            return categories.Select(ps => new CategoryResponse
            {
                CategoryId = ps.CategoryId,
                CategoryName = ps.CategoryName!,
                ParentCategoryId = ps.ParentCategoryId,
                ParentCategory = ps.ParentCategory != null ? ps.ParentCategory.CategoryName : "N/A",
            }).ToList();
        }

        /// <summary>
        /// Retrieves a specific category by its ID asynchronously and returns the category response.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve</param>
        /// <returns>The requested category response if found, otherwise null</returns>
        /// <exception cref="ArgumentException">Thrown when the category ID is not valid</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the category is not found</exception>
        public async Task<CategoryResponse> GetCategoryByIdAsync(int categoryId)
        {
            if (categoryId <= 0) throw new ArgumentException($"Category with ID{categoryId} is Not Valid.");

            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} Not Found.");
            }
            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName!,
                ParentCategoryId = category.ParentCategoryId
            };
        }
    }
}
