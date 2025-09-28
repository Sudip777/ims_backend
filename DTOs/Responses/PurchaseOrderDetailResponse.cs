using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.DTOs.Responses
{
    public class PurchaseOrderDetailResponse
    {
        public int PurchaseOrderDetailId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public static PurchaseOrderDetailResponse MappedPurchaseOrderDetailResponse(PurchaseOrderDetail od)
        {

            return new PurchaseOrderDetailResponse
            {
                PurchaseOrderDetailId = od.PurchaseOrderDetailId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
            };
        }
    }
}
