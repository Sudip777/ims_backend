namespace inventory_management_system.DTOs.Requests
{
    public class WarehouseOverviewDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int TotalStockQuantity { get; set; }
        public decimal PercentageUtilization { get; set; }
        public string StockStatusDescription { get; set; }
    }
}
