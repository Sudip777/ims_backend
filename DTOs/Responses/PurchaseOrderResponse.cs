using inventory_management_system.Models;


namespace inventory_management_system.DTOs.Responses
{
    public class PurchaseOrderResponse
    {
        public int PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public int StatusId { get; set; }
        public string SupplierName { get; set; } = String.Empty;
        public string StatusName { get; set; } = String.Empty;
        public required DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }
        public int CreatedByUserId { get; set; }
        public List<PurchaseOrderDetailResponse>? PurchaseOrderDetails { get; set; } // Nested details


        public static PurchaseOrderResponse MappedPurchaseOrderResponse(PurchaseOrder od)
        {
            return new PurchaseOrderResponse
            {
                PurchaseOrderId = od.PurchaseOrderId,
                SupplierId = od.SupplierId,
                SupplierName = od.Supplier?.Name ?? string.Empty,
                StatusId = od.StatusId,
                StatusName = od.Status?.Name ?? string.Empty,
                TotalAmount = od.TotalAmount,
                OrderDate = od.OrderDate,
                CreatedByUserId = od.CreatedByUserId,
                PurchaseOrderDetails = od.PurchaseOrderDetails?
                    .Select(temp => PurchaseOrderDetailResponse.MappedPurchaseOrderDetailResponse(temp))
                    .ToList() ?? new List<PurchaseOrderDetailResponse>()
            };
        }


    }
}
