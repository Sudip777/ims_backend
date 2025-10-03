using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IProductSupplierRepository
    {
        Task<ProductSupplier> CreateProductSupplierAsync(ProductSupplier productSupplier);
        Task<ProductSupplier> GetProductSupplierByIdAsync(int id);
        Task<IEnumerable<ProductSupplier>> GetAllProductSuppliersAsync();
    }
}
