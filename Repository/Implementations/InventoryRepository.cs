using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class InventoryRepository: IInventoryRepository
    {
        private readonly ApplicationDBContext _context;

        public InventoryRepository(ApplicationDBContext context) { 
            _context = context;
        }
        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync(Inventory inventory)
        {
            return (IEnumerable<Inventory>)await _context.Inventories.Include(p => p.Product).Include(p => p.Warehouse)
                .Select(inventory => new InventoryResponse
                {
                    InventoryId = inventory.InventoryId,
                    ProductId = inventory.ProductId,
                    WarehouseId = inventory.WarehouseId,
                    Quantity = inventory.Quantity


                })
                .ToListAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(int id)
        {
            return await _context.Inventories.FindAsync(id);
        }

        public async Task<Inventory> UpdateInventoryAsync(InventoryDto inventory, int inventoryId)
        {
            var entity = await _context.Inventories
               .Include(p => p.Product)
               .Include(p => p.Warehouse)
               .FirstOrDefaultAsync(p => p.InventoryId == inventoryId);

            if (entity == null)
                throw new KeyNotFoundException($"Inventory with ID {inventoryId} not found.");


            var updatedInventory = inventory.MappedInventory();
            _context.Inventories.Update(updatedInventory);
            await _context.SaveChangesAsync();
            return updatedInventory;
        }


        public async Task<Inventory> CreateInventoryAsync(Inventory entity)
        {
            // Validate Product
            if (!await _context.Products.AnyAsync(p => p.ProductId == entity.ProductId))
                throw new KeyNotFoundException($"Product {entity.ProductId} not found.");

            // Validate Warehouse
            if (!await _context.Warehouses.AnyAsync(w => w.WarehouseId == entity.WarehouseId))
                throw new KeyNotFoundException($"Warehouse {entity.WarehouseId} not found.");

            // Prevent duplicate (Product + Warehouse)
            var existing = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == entity.ProductId && i.WarehouseId == entity.WarehouseId);
            if (existing != null)
                throw new InvalidOperationException("Inventory already exists for this product in this warehouse.");

            _context.Inventories.Add(entity);
            await _context.SaveChangesAsync();

            // Reload with navigation properties
            return await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstAsync(i => i.InventoryId == entity.InventoryId);
        }

        public async Task<Inventory> GetByProductAndWarehouseAsync(int pid, int wid)
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(i => i.ProductId == pid && i.WarehouseId == wid);
        }

        public async Task<Inventory> UpdateInventoryFromOrderAsync(int productId, int warehouseId, int quantityChange)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);
            if (inventory == null)
                throw new InvalidOperationException($"No inventory found for ProductId {productId} in WarehouseId {warehouseId}");

            inventory.Quantity += quantityChange;
            if (inventory.Quantity < 0)
                throw new InvalidOperationException($"Inventory cannot be negative for ProductId {productId} in WarehouseId {warehouseId}");

            await _context.SaveChangesAsync();
            return inventory;
        }
    }
}
