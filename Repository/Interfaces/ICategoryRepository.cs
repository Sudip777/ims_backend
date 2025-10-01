using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>>  GetAllCategoriesAsync();
        Task<Category> CreateCategoryAsync(Category category);
    }
}
