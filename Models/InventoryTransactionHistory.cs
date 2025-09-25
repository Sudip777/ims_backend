using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace inventory_management_system.Models
{
    public class InventoryTransactionHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        [Required]
        public int QuantityChange { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

        [ForeignKey("TransactionTypeId")]
        public virtual TransactionType TransactionType { get; set; }

        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int? PurchaseOrderId { get; set; }

        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        [StringLength(255)]
        public string? Details { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        
    }

}
