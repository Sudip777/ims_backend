using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Services.Implementations
{
    public class InventoryService : IInventoryService
    {

        private readonly IInventoryRepository _inventoryRepository;
        private readonly ApplicationDBContext _context;

        public InventoryService(IInventoryRepository inventoryRepository, ApplicationDBContext context)
        {
            _inventoryRepository = inventoryRepository;
            _context = context;
        }


        public async Task<InventoryResponse> CreateInventoryAsync(InventoryDto dto)
        {
            var entity = dto.MappedInventory();
            var createdEntity = await _inventoryRepository.CreateInventoryAsync(entity);
            return new InventoryResponse
            {
                InventoryId = createdEntity.InventoryId,
                ProductId = createdEntity.ProductId,
                ProductName = createdEntity.Product?.Name,
                WarehouseId = createdEntity.WarehouseId,
                WarehouseName = createdEntity.Warehouse?.Name,
                Quantity = createdEntity.Quantity,
                ReorderLevel = createdEntity.Product.ReorderLevel
            };
        }


        public async Task<IEnumerable<InventoryResponse>> GetAllInventoryAsync()
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Select(i => new InventoryResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product != null ? i.Product.Name : string.Empty,
                    WarehouseId = i.WarehouseId,
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : string.Empty,
                    Quantity = i.Quantity,
                    InventoryId = i.InventoryId,
                    ReorderLevel = i.Product != null ? i.Product.ReorderLevel : 0
                })
                .ToListAsync();
        }

        public async Task<InventoryResponse?> GetInventoryByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid inventory ID");
            }

            var inventoryData = await _inventoryRepository.GetInventoryByIdAsync(id);

            if (inventoryData == null)
            {
                throw new KeyNotFoundException($"Inventory with ID {id} not found.");
            }

            return new InventoryResponse
            {
                InventoryId = inventoryData.InventoryId,
                ProductId = inventoryData.ProductId,
                ProductName = inventoryData.Product != null ? inventoryData.Product.Name : string.Empty,
                WarehouseId = inventoryData.WarehouseId,
                WarehouseName = inventoryData.Warehouse != null ? inventoryData.Warehouse.Name : string.Empty,
                Quantity = inventoryData.Quantity
            };
        }


        public async Task<IEnumerable <InventoryResponse>> GetLowStocks()
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Select(i => new InventoryResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product != null ? i.Product.Name : string.Empty,
                    WarehouseId = i.WarehouseId,
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : string.Empty,
                    Quantity = i.Quantity,
                    InventoryId = i.InventoryId,
                    ReorderLevel = i.Product != null ? i.Product.ReorderLevel : 0
                }).Where(i => i.Quantity <= i.ReorderLevel)
                .ToListAsync();
        }

        public async Task<InventoryResponse> UpdateInventoryAsync(InventoryDto inventory, int inventoryId)
        {
            if (inventoryId <= 0)
            {
                throw new ArgumentException("Invalid inventory ID");
            }
            var inventoryResponse = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
            if (inventoryResponse == null)
            {
                throw new KeyNotFoundException($"Inventory with ID {inventoryId} not Found.");
            }
            var updatedInventory = await _inventoryRepository.UpdateInventoryAsync(inventory, inventoryId);

            return InventoryResponse.MappedInventoryResponse(updatedInventory);
        }

       
    }
}
