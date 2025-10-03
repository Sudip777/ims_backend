using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductResponse> CreateProductAsync(ProductDto product);
        Task<ProductResponse> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();     
        Task<Product> UpdateProductAsync(ProductDto product, int id);
        Task<bool> DeleteProductAsync(int id);
       

    }
}
