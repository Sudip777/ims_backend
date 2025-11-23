using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<(IEnumerable<Product> products, int totalCount)> GetAllProductsAsync(GetAllProductsRequest request);
        Task<IEnumerable<Product>> GetAllProductLists();
        Task<Product> UpdateProductAsync(ProductDto product, int id);
        Task<bool> DeleteProductAsync(int id);
       

    }
}
