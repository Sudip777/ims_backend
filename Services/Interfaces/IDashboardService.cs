using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Services.Interfaces
{
    public interface IDashboardService
    {
          Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverview();
          Task<IEnumerable<SalesPerformanceDto>> GetSalesPerformance();
          Task<IEnumerable<PurchaseOrderStatusOverviewDto>> GetPurchaseOrderStatuses();
          Task<IEnumerable<WarehouseOverviewDto>> GetWarehouseOverview();
          Task<IEnumerable<InventoryTransactionOverviewDto>> GetInventoryTransactionSummary();
    }
}
