using inventory_management_system.Data;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class PurchaseOrderStatusRepository : IPurchaseOrderStatusRepository
    {
        private readonly ApplicationDBContext _context;
        public PurchaseOrderStatusRepository(ApplicationDBContext context)
        {
                       _context = context;
        }
        public async Task<IEnumerable<PurchaseOrderStatus>> GetAllPurchaseOrderStatusesAsync()
        {
            return await _context.PurchaseOrderStatuses.ToListAsync();
        }
    }
}
