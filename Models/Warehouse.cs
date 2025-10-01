using inventory_management_system.DTOs.Responses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarehouseId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public  virtual User CreatedByUser { get; set; }

        // Navigation properties
        public  virtual ICollection<Inventory> Inventories { get; set; }
        public  virtual ICollection<InventoryTransactionHistory> Transactions { get; set; }

        internal object Select(Func<object, WarehouseResponse> value)
        {
            throw new NotImplementedException();
        }
    }
}
