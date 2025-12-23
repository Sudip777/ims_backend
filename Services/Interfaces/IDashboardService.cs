using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Services.Interfaces
{
    /// <summary>
    /// Interface for dashboard service operations including retrieving inventory, sales, and purchase order data summaries.
    /// </summary>
    public interface IDashboardService
    {
          /// <summary>
          /// Retrieves inventory overview data for the dashboard asynchronously and returns a collection of inventory overview data transfer objects.
          /// </summary>
          /// <returns>A collection of inventory overview data transfer objects</returns>
          Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverview();
          /// <summary>
          /// Retrieves sales performance data for the dashboard asynchronously and returns a collection of sales performance data transfer objects.
          /// </summary>
          /// <returns>A collection of sales performance data transfer objects</returns>
          Task<IEnumerable<SalesPerformanceDto>> GetSalesPerformance();
          /// <summary>
          /// Retrieves purchase order status data for the dashboard asynchronously and returns a collection of purchase order status overview data transfer objects.
          /// </summary>
          /// <returns>A collection of purchase order status overview data transfer objects</returns>
          Task<IEnumerable<PurchaseOrderStatusOverviewDto>> GetPurchaseOrderStatuses();
          /// <summary>
          /// Retrieves warehouse overview data for the dashboard asynchronously and returns a collection of warehouse overview data transfer objects.
          /// </summary>
          /// <returns>A collection of warehouse overview data transfer objects</returns>
          Task<IEnumerable<WarehouseOverviewDto>> GetWarehouseOverview();
          /// <summary>
          /// Retrieves inventory transaction summary data for the dashboard asynchronously and returns a collection of inventory transaction overview data transfer objects.
          /// </summary>
          /// <returns>A collection of inventory transaction overview data transfer objects</returns>
          Task<IEnumerable<InventoryTransactionOverviewDto>> GetInventoryTransactionSummary();
    }
}
