using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Extensions;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    /// <summary>
    /// Implementation of the inventory repository interface providing methods for inventory data access operations.
    /// </summary>
    public class InventoryRepository: IInventoryRepository
    {
        private readonly ApplicationDBContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to use for data access operations</param>
        public InventoryRepository(ApplicationDBContext context) {
            _context = context;
        }
        /// <summary>
        /// Retrieves all inventory items based on the provided request parameters asynchronously.
        /// </summary>
        /// <param name="request">Request object containing query parameters for filtering and pagination</param>
        /// <returns>A tuple containing a collection of inventories and the total count</returns>
        public async Task<(IEnumerable<Inventory> inventories, int totalCount)> GetAllInventoriesAsync(
            GetAllInventoriesRequest request)
        {
            var inventories = await _context.Set<Inventory>()
                .FromSqlInterpolated($@"
                EXEC dbo.GetAllInventoriesData
                @ProductId = {request.ProductId},
                @WarehouseId = {request.WarehouseId},
                @Search = {request.Search},
                @SortColumn = {request.SortColumn},
                @SortDirection = {request.SortDirection},
                @Page = {request.Page},
                @PageSize = {request.PageSize}")
                .AsNoTracking()
                .ToListAsync();

            var totalCount = await _context.Inventories
                .AsQueryable()
                .Where(i => (!request.ProductId.HasValue || i.ProductId == request.ProductId.Value)
                            && (!request.WarehouseId.HasValue || i.WarehouseId == request.WarehouseId.Value)
                            && (string.IsNullOrEmpty(request.Search)
                                || i.Product.Name.Contains(request.Search)
                                || i.Warehouse.Name.Contains(request.Search)))
                .CountAsync();

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
