using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(150)]
        public required string  Name { get; set; }

        [Required]
        [StringLength(150)]
        public required string  Email { get; set; }

        [StringLength(50)]
        public required string Phone { get; set; }

        [StringLength(255)]
        public required string Address { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public required virtual User CreatedByUser { get; set; }

        // Navigation properties
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<ProductSupplier>? ProductSuppliers { get; set; }
        public virtual ICollection<PurchaseOrder>? PurchaseOrders { get; set; }
    }
}
