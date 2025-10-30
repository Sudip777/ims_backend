namespace inventory_management_system.DTOs.Requests
{
    public class GetAllInventoriesRequest
    {

        public int? ProductId { get; set; }
        public int? WarehouseId { get; set; }
        public string? Search { get; set; }
        public string? SortColumn { get; set; } = "InventoryId";
        public string? SortDirection { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

