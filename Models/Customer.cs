using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(150)]
        public required string Name { get; set; }

        [Required]
        [StringLength(150)]
        public required string Email { get; set; }

        [StringLength(50)]
        public required string Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User? CreatedByUser { get; set; }

        // Navigation property
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
