using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDBContext _context;

        public WarehouseRepository(ApplicationDBContext context)
        {
          _context = context;
            
        }
        public async Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }

        public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync()
        {
            return await _context.Warehouses.ToListAsync();
        }

        public async Task<Warehouse> GetWarehouseByIdAsync(int id)
        {
            return await _context.Warehouses.FindAsync(id)!;
        }

        public async Task<Warehouse> UpdateWarehouseAsync(WarehouseDto warehouse, int id)
        {
            var existingWarehouse = await _context.Warehouses
               
               .FirstOrDefaultAsync(p => p.WarehouseId == id);

            if (existingWarehouse == null)
                throw new Exception("Warehouse is not Found");

            //Map DTO → entity
            existingWarehouse.Name = warehouse.Name;
            await _context.SaveChangesAsync();

            return existingWarehouse;


        }
        public async Task<Warehouse?> GetByNameAsync(string name)
        {
            return await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Name.ToLower() == name.ToLower());
        }

    }
}
