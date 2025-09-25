using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class PurchaseOrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderDetailId { get; set; }

        [Required]
        public int PurchaseOrderId { get; set; }

        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }

}
