using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class InventoryResponse
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = null!;
        public string WarehouseName { get; set; } = null!;
        public int ReorderLevel { get; set; }



        public static InventoryResponse MappedInventoryResponse(Inventory inventory)
        {

            return new InventoryResponse
            {
                InventoryId = inventory.InventoryId,
                ProductId = inventory.ProductId,
                WarehouseId = inventory.WarehouseId,
                Quantity = inventory.Quantity,
                ProductName=inventory.Product?.Name,
                WarehouseName=inventory.Warehouse?.Name,
                ReorderLevel = inventory.Product?.ReorderLevel ?? 0
            };
        }
    }
}
