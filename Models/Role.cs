using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string? RoleName { get; set; }

        // Navigation property
        public virtual ICollection<User>? Users { get; set; }
    }

}
