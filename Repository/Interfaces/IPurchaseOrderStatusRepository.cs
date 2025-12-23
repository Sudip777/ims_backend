using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    /// <summary>
    /// Interface for purchase order status repository operations including retrieving purchase order statuses.
    /// </summary>
    public interface IPurchaseOrderStatusRepository
    {
        /// <summary>
        /// Retrieves all purchase order statuses from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all purchase order statuses</returns>
        Task<IEnumerable<PurchaseOrderStatus>> GetAllPurchaseOrderStatusesAsync();
    }
}
