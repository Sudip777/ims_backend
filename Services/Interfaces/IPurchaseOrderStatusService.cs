using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface IPurchaseOrderStatusService
    {
        Task<IEnumerable<PurchaseOrderStatusResponse>> GetAllPurchaseOrderStatusesAsync();
    }
}
