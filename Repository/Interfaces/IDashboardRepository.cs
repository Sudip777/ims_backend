using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverview();
        Task<IEnumerable<SalesPerformanceDto>> GetSalesPerformance();

        Task<IEnumerable<PurchaseOrderStatusOverviewDto>> GetPurchaseOrderStatuses();
        Task<IEnumerable<WarehouseOverviewDto>> GetWarehouseOverview();
        Task<IEnumerable<InventoryTransactionOverviewDto>>  GetInventoryTransactionOverview();
    }
}
