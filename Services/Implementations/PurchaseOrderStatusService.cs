using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using static inventory_management_system.Constants.ApiRoutes;

namespace inventory_management_system.Services.Implementations
{
    public class PurchaseOrderStatusService : IPurchaseOrderStatusService
    {
        private readonly IPurchaseOrderStatusRepository _purchaseOrderStatusRepository;
        public PurchaseOrderStatusService(IPurchaseOrderStatusRepository purchaseOrderStatusRepository)
        {
            _purchaseOrderStatusRepository = purchaseOrderStatusRepository;
        }
        public async Task<IEnumerable<PurchaseOrderStatusResponse>> GetAllPurchaseOrderStatusesAsync()
        {
           var response = await _purchaseOrderStatusRepository.GetAllPurchaseOrderStatusesAsync();
            if (response == null || !response.Any())
                throw new InvalidOperationException("No purchase order statuses found.");
            return response.Select(ps => new PurchaseOrderStatusResponse
            {
                StatusId = ps.StatusId,
                Name = ps.Name!,
            }).ToList();
        }
    }
}
