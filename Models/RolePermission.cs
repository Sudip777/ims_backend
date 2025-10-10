using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.Models
{
    public class RolePermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolePermissionId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int UrlEndpointId { get; set; }

        [Required]
        public int MethodId { get; set; }

        public string? MappedUrl { get; set; } // Nullable for optional alias/versioning

        [ForeignKey("RoleId")]
        [Required]
        public required Role Role { get; set; }

        [ForeignKey("UrlEndpointId")]
        [Required]
        public required UrlEndpoint UrlEndpoint { get; set; }

        [ForeignKey("MethodId")]
        [Required]
        public required Method Method { get; set; }
    }
}
