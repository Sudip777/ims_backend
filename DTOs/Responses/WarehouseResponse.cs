using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class WarehouseResponse
    {
        public int WarehouseId { get; set; }
        public required string Name { get; set; }
        public int CreatedByUserId { get; set; }


        public static WarehouseResponse MappedWarehouseResponse(Warehouse warehouse)
        {
            return new WarehouseResponse
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                CreatedByUserId = warehouse.CreatedByUserId
            };
        }
    }
}
