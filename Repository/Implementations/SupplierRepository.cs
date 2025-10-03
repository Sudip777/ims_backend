using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDBContext _context;

        public SupplierRepository (ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            // Check Uniqueness
            var supplierExists = await _context.Suppliers
                .AnyAsync(c => c.Name == supplier.Name ||
                               c.Email == supplier.Email ||
                               c.Phone == supplier.Phone);

            if (supplierExists)
                throw new InvalidOperationException("A supplier with the same Name, Email or  Phone number already exists.");

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

      

        public async Task<IEnumerable<Supplier>> GetAllSupplierAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Supplier> UpdateSupplierAsync(SupplierDto supplier, int id)
        {
            var existingSuplier = await _context.Suppliers
               .Include(p => p.PurchaseOrders)
               .FirstOrDefaultAsync(p => p.SupplierId == id);

            if (existingSuplier == null)
                throw new Exception("Customer not found");

            // Map DTO → entity
            existingSuplier.Name = supplier.Name;
            existingSuplier.Email = supplier.Email;
            existingSuplier.Phone = supplier.Phone;
            existingSuplier.Address = supplier.Address;
            existingSuplier.IsActive = supplier.IsActive;

            await _context.SaveChangesAsync();

            return existingSuplier;
        }
        public Task DeleteSupplierAsync(int id)
        {
           var res = _context.Suppliers.Find(id);
            if (res != null)
            {
                _context.Suppliers.Remove(res);
                _context.SaveChanges();
            }
            return Task.CompletedTask;
        }
    }
}
