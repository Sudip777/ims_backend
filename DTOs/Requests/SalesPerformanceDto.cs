namespace inventory_management_system.DTOs.Requests
{
    public class SalesPerformanceDto
    {
        int TotalSalesAmount { get; set; }
        int CompletedOrderCount { get; set; }
        int AverageOrderValue { get; set; }
    }
}
