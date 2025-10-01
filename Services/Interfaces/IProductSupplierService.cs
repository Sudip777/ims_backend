using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface IProductSupplierService
    {
        Task<ProductSupplierResponse> CreateProductSupplierAsync(ProductSupplierDto dto);
        Task<ProductSupplierResponse> GetProductSupplierByIdAsync(int id);
        Task<IEnumerable<ProductSupplierResponse>> GetAllProductSuppliersAsync();

    }
}
