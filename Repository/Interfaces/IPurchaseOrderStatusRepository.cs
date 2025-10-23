using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IPurchaseOrderStatusRepository
    {
        Task<IEnumerable<PurchaseOrderStatus>> GetAllPurchaseOrderStatusesAsync();
    }
}
