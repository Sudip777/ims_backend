using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual Warehouse? Warehouse { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }
    }
}
