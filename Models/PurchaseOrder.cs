using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class PurchaseOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int StatusId { get; set; } = 1; // Default: Pending

        [ForeignKey("StatusId")]
        public virtual PurchaseOrderStatus? Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal TotalAmount { get; set; } // Computed via trigger

        [Required]
        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User? CreatedByUser { get; set; }

        // Navigation properties
        public virtual ICollection<PurchaseOrderDetail>? PurchaseOrderDetails { get; set; }
        public virtual ICollection<InventoryTransactionHistory>? Transactions { get; set; }
    }

}
