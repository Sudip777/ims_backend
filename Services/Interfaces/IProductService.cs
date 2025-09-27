using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Services.Interfaces
{
    public interface  IProductService
    {
        Task<ProductResponse> RegisterProductAsync(ProductDto dto);
        Task<ProductResponse?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();

        Task<ProductResponse> UpdateProductAsync(int productId, ProductDto dto);
        Task<bool> DeleteProductAsync(int productId);
        Task ValidateProduct(ProductDto dto, int productId);
        Task<bool> CheckProductAndCategoryId(int supplierId, int categoryId);


    }
}
