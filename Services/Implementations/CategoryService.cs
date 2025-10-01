using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            // Validate ParentCategoryId
            if (category.ParentCategoryId != 0)
            {
                var exists = await _context.Categories
                    .AnyAsync(c => c.CategoryId == category.ParentCategoryId);

                if (!exists)
                    throw new KeyNotFoundException($"Parent Category {category.ParentCategoryId} does not exist.");
            }

            var entity = category.MappedCategory();
            var createdCategory = await _categoryRepository.CreateCategoryAsync(entity);
            if (createdCategory == null) throw new KeyNotFoundException("No Categories Found.");


            return new CategoryResponse
            {
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
                CategoryName = ps.CategoryName!,
                ParentCategoryId = ps.ParentCategoryId
            }).ToList();
        }
    }
}
