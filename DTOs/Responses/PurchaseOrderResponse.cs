using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.DTOs.Responses
{
    public class PurchaseOrderResponse
    {
        public int PurchaseOrderId { get; set; }
        public int SuplierId { get; set; }
        public int StatusId { get; set; }
        public string SupplierName { get; set; } = String.Empty;
        public string StatusName { get; set; } = String.Empty;

        public decimal TotalAmout { get; set; }
        public List<PurchaseOrderDetailResponse>? PurchaseOrderDetails { get; set; } // Nested details


        public static PurchaseOrderResponse MappedPurchaseOrderResponse(PurchaseOrder od)
        {
            return new PurchaseOrderResponse
            {
                PurchaseOrderId = od.PurchaseOrderId,
                SuplierId = od.SupplierId,
                SupplierName = od.Supplier.Name,
                StatusId = od.StatusId,
                StatusName = od.Status.Name,
                TotalAmout = od.TotalAmount,
                PurchaseOrderDetails = od.PurchaseOrderDetails
                                     .Select(od => PurchaseOrderDetailResponse.MappedPurchaseOrderDetailResponse(od))
                                     .ToList()
            };
        }

    }
}
