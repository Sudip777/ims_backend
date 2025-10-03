using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        Task<Supplier> UpdateSupplierAsync(SupplierDto supplier, int id);
        Task<IEnumerable<Supplier>> GetAllSupplierAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task DeleteSupplierAsync(int id);
    }
}
