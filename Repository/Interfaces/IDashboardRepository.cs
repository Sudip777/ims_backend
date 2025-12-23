using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for dashboard repository operations including retrieving inventory, sales, and purchase order data summaries.
    /// </summary>
    public interface IDashboardRepository
    {
        /// <summary>
        /// Retrieves inventory overview data for the dashboard asynchronously.
        /// </summary>
        /// <returns>A collection of inventory overview data</returns>
        Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverview();
        /// <summary>
        /// Retrieves sales performance data for the dashboard asynchronously.
        /// </summary>
        /// <returns>A collection of sales performance data</returns>
        Task<IEnumerable<SalesPerformanceDto>> GetSalesPerformance();

        /// <summary>
        /// Retrieves purchase order status data for the dashboard asynchronously.
        /// </summary>
        /// <returns>A collection of purchase order status data</returns>
        Task<IEnumerable<PurchaseOrderStatusOverviewDto>> GetPurchaseOrderStatuses();
        /// <summary>
        /// Retrieves warehouse overview data for the dashboard asynchronously.
        /// </summary>
        /// <returns>A collection of warehouse overview data</returns>
        Task<IEnumerable<WarehouseOverviewDto>> GetWarehouseOverview();
        /// <summary>
        /// Retrieves inventory transaction overview data for the dashboard asynchronously.
        /// </summary>
        /// <returns>A collection of inventory transaction overview data</returns>
        Task<IEnumerable<InventoryTransactionOverviewDto>> GetInventoryTransactionOverview();
    }
}
