using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id);
        Task<(IEnumerable<PurchaseOrder> purchaseOrders, int totalCount)> GetAllPurchaseOrderAsync(GetAllPurchaseOrdersRequest req);
        Task<PurchaseOrder> AddPurchaseOrderAsync(PurchaseOrder order);
        Task<PurchaseOrder> UpdatePurchaseOrderAsync(PurchaseOrderDto order, int id);
        //Task<bool> DeletePurchaseOrderAsync(int id);
        Task<PurchaseOrder> UpdatePurchaseOrderStatusAsync(int id, int newStatusId);
    }
}
