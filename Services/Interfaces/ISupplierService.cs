using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierResponse> CreateSupplierAsync(SupplierDto supplier);
        Task<SupplierResponse> GetSupplierByIdAsync(int id);
        Task<IEnumerable<SupplierResponse>> GetAllSupplierAsync();
        Task<SupplierResponse> UpdateSupplierAsync(int id, SupplierDto supplier);
        Task<bool> DeleteSupplierAsync(int id);
    }
}
