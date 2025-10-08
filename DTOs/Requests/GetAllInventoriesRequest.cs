namespace inventory_management_system.DTOs.Requests
{
    public class GetAllInventoriesRequest
    {

        public int? InventoryId { get; set; }
        public int? ProductId { get; set; }
        public int? WarehouseId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

