using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Extensions;
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
        public async Task<(IEnumerable<Inventory> inventories, int totalCount)> GetAllInventoriesAsync(
                GetAllInventoriesRequest request)
        {
            var query = _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .AsQueryable();

            // Filtering
            if (request.ProductId.HasValue)
                query = query.Where(i => i.ProductId == request.ProductId.Value);

            if (request.WarehouseId.HasValue)
                query = query.Where(i => i.WarehouseId == request.WarehouseId.Value);

            if (!string.IsNullOrEmpty(request.Search))
    {
        var searchTerm = request.Search.ToLower();
            query = query.Where(i =>
                EF.Functions.Like(i.Product.Name.ToLower(), $"%{searchTerm}%") ||
                EF.Functions.Like(i.Warehouse.Name.ToLower(), $"%{searchTerm}%") ||
                EF.Functions.Like(i.InventoryId.ToString(), $"%{searchTerm}%")
            );
    }

            query = request.SortDirection == "asc" ? query.OrderByDynamic(request.SortColumn) : query.OrderByDescendingDynamic(request.SortColumn);

            // Total count
            var totalCount = await query.CountAsync();

            // sorting and pagination
            var inventories = await query
    
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return (inventories, totalCount);
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


            entity.ProductId = inventory.ProductId;
            entity.WarehouseId = inventory.WarehouseId;
            entity.Quantity = inventory.Quantity;
            _context.Inventories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
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

            // Navigation properties
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
