using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using static inventory_management_system.Constants.ApiRoutes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace inventory_management_system.Services.Implementations
{
    public class CategoryService : ICategoryService

    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDBContext _context;
        public CategoryService(ICategoryRepository categoryRepository, ApplicationDBContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }
        public async Task<CategoryResponse> CreateCategoryAsync(CategoryDto category)
        {
          
            var entity = category.MappedCategory();
            var createdCategory = await _categoryRepository.CreateCategoryAsync(entity);
            if (createdCategory == null) throw new KeyNotFoundException("No Categories Found.");


            return new CategoryResponse
            {
                CategoryId = createdCategory.CategoryId,
                CategoryName = createdCategory.CategoryName!,
                ParentCategoryId = createdCategory.ParentCategoryId
            };
        }

 

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            if(categories == null) throw new KeyNotFoundException("No Categories Found.");
            
            return categories.Select(ps => new CategoryResponse
            {
                CategoryId = ps.CategoryId,
                CategoryName = ps.CategoryName!,
                ParentCategoryId = ps.ParentCategoryId
            }).ToList();
        }

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
