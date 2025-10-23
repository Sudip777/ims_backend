using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateCategoryAsync(CategoryDto category);
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
        Task<CategoryResponse> GetCategoryByIdAsync(int categoryId);
    }
}
