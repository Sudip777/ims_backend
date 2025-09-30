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
            // Check if the user creating the customer exists
            var userExists = await _context.Suppliers.AnyAsync(u => u.SupplierId == supplier.CreatedByUserId);
            if (!userExists)
                throw new InvalidOperationException($"Supplier with ID {supplier.CreatedByUserId} does not exist.");

            // Check Uniqueness
            var supplierExists = await _context.Customers
                .AnyAsync(c => c.Name == supplier.Name ||
                               c.Email == supplier.Email ||
                               c.Phone == supplier.Phone);

            if (supplierExists)
                throw new InvalidOperationException("A supplier with the same name, email or  phone already exists.");

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

      

        public async Task<IEnumerable<Supplier>> GetAllSupplierAsync(Supplier customer)
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

            // Map only the updatable fields from DTO → entity
            existingSuplier.Name = supplier.Name;
            existingSuplier.Email = supplier.Email;
            existingSuplier.Phone = supplier.Phone;
            existingSuplier.Address = supplier.Address;

            await _context.SaveChangesAsync();

            return existingSuplier;
        }
        public Task DeleteSupplierAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
