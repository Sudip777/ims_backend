using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;


namespace inventory_management_system.DTOs.Requests
{
    public class PurchaseOrderDto
    {

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int StatusId { get; set; } = 1; // Default: Pending
        public ICollection<PurchaseOrderDetailDto> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetailDto>();
        public PurchaseOrder MappedPurchaseOrder()
        {
            return new PurchaseOrder
            {
                SupplierId = this.SupplierId,
                StatusId = this.StatusId,
                PurchaseOrderDetails = this.PurchaseOrderDetails.Select(od => new PurchaseOrderDetail
                {
                    PurchaseOrderId = od.PurchaseOrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            };
        }
    }
}
