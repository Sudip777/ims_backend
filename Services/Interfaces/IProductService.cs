using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Services.Interfaces
{
    public interface  IProductService
    {
        Task<ProductResponse> RegisterProductAsync(ProductDto dto);
        Task<ProductResponse?> GetProductByIdAsync(int id);
        Task<PagedResponse<ProductResponse>> GetAllProductsAsync(GetAllProductsRequest req);

        Task<ProductResponse> UpdateProductAsync(int productId, ProductDto dto);
        Task<bool> DeleteProductAsync(int productId);
 


    }
}
