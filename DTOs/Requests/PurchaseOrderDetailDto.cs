using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.DTOs.Requests
{
    public class PurchaseOrderDetailDto
    {

        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }



        public PurchaseOrderDetail MappedPurchaseOrderDetail()
        {

            return new PurchaseOrderDetail
            {
                PurchaseOrderId = this.PurchaseOrderId,
                ProductId = this.ProductId,
                Quantity = this.Quantity,
                UnitPrice = this.UnitPrice

            };
        }

    }

}
    

