using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;

namespace inventory_management_system.Services.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderResponse> CreatePurchaseOrderAsync(PurchaseOrderDto orderDto);
        Task<PurchaseOrderResponse> GetPurchaseOrderByIdAsync(int id);
        Task<IEnumerable<PurchaseOrderResponse>> GetAllPurchaseOrdersAsync();
        Task<PurchaseOrderResponse> UpdatePurchaseOrderAsync(PurchaseOrderDto orderDto, int id);
        Task<PurchaseOrderResponse> UpdateOrderStatusAsync(int id, int newStatusId);
    }
}
