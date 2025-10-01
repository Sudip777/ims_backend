using inventory_management_system.Data;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class ProductSupplierRepository : IProductSupplierRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductSupplierRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<ProductSupplier> CreateProductSupplierAsync(ProductSupplier productSupplier)
        {
            _context.ProductSuppliers.Add(productSupplier);
            await _context.SaveChangesAsync();
            return productSupplier;

        }

        public async Task<IEnumerable<ProductSupplier>> GetAllProductSuppliersAsync()
        {
            return await _context.ProductSuppliers.ToListAsync();
        }

        public async Task<ProductSupplier> GetProductSupplierByIdAsync(int id)
        {
            return await _context.ProductSuppliers.FirstOrDefaultAsync((o) => o.ProductSupplierId == id);
        }
    }
}
