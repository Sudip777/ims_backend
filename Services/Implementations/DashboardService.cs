using inventory_management_system.DTOs.Requests;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverview()
        {
           var response = await _dashboardRepository.GetInventoryOverview();
            if (response == null)  throw new KeyNotFoundException("Inventory Overview Data Not Found.");
            
            return response;
        }

        public async Task<IEnumerable<PurchaseOrderStatusOverviewDto>> GetPurchaseOrderStatuses()
        {
           var res =  await _dashboardRepository.GetPurchaseOrderStatuses();
            if(res == null) throw new KeyNotFoundException("Purchase Order Status Details Not Found.");
            return res;
        }

        public async Task<IEnumerable<SalesPerformanceDto>> GetSalesPerformance()
        {
           var data = await _dashboardRepository.GetSalesPerformance();
            if (data == null) throw new KeyNotFoundException("Sales Performance Data Not Found.");
            return data;
        }

        public async Task<IEnumerable<WarehouseOverviewDto>> GetWarehouseOverview()
        {
           var response = await _dashboardRepository.GetWarehouseOverview();
            if (response == null) throw new KeyNotFoundException("Warehouse Overview Data Not Found.");

            return response;
        }
        public async Task<IEnumerable<InventoryTransactionOverviewDto>> GetInventoryTransactionSummary()
        {
           var response = await _dashboardRepository.GetInventoryTransactionOverview();
            if (response == null) throw new KeyNotFoundException("Inventory Transaction Summary Data Not Found.");
            return response;
        }

      
    }
}
