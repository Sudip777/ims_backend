using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class PurchaseOrderStatusResponse
    {
        public required int StatusId { get; set; }
        public required string Name { get; set; } 

        public static PurchaseOrderStatusResponse MappedPurchaseOrderStatusResponse(PurchaseOrderStatus status)
        {

            return new PurchaseOrderStatusResponse
            {

                StatusId = status.StatusId,
                Name = status.Name!
            };
        }


    }
}
