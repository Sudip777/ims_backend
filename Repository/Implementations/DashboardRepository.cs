using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class DashboardRepository : IDashboardRepository

    {
        private readonly ApplicationDBContext _context;
        public DashboardRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryOverviewDto>> GetInventoryOverview()
        {
            var result = await _context.Database
                  .SqlQuery<InventoryOverviewDto>($"EXEC dbo.InventoryOverview")
                  .AsNoTracking()
                  .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<PurchaseOrderStatusOverviewDto>> GetPurchaseOrderStatuses()
        {
            var result = await _context.Database
            .SqlQuery<PurchaseOrderStatusOverviewDto>($"EXEC dbo.PurchaseOrderStatusOverview")
            .AsNoTracking()
            .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SalesPerformanceDto>> GetSalesPerformance()
        {
            var result = await _context.Database
                 .SqlQuery<SalesPerformanceDto>($"EXEC dbo.SalesPerformance")
                 .AsNoTracking()
                 .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<WarehouseOverviewDto>> GetWarehouseOverview()
        {
            var result = await _context.Database
                .SqlQuery<WarehouseOverviewDto>($"EXEC dbo.WarehouseInventoryOverview")
                .AsNoTracking()
                .ToListAsync();

            return result;
        }


        public async Task<IEnumerable<InventoryTransactionOverviewDto>> GetInventoryTransactionOverview()
        {
            var result = await _context.Database
                .SqlQuery<InventoryTransactionOverviewDto>($"EXEC dbo.InventoryTransactionSummary")
                .AsNoTracking()
                .ToListAsync();

            return result;
        }


    }
}
