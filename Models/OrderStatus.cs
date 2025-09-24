using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class OrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        [Required]
        [StringLength(50)]
        [Index("IX_OrderStatuses_Name", IsUnique = true)]
        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; }
    }

}
