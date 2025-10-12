using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;
namespace inventory_management_system.DTOs.Requests
{
    public class RolePermissionDto
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int UrlEndpointId { get; set; }
        [Required]
        public int MethodId { get; set; }
        public string? MappedUrl { get; set; } // Optional alias/versioning



        public RolePermission MappedRolePermission()
        {
            return new RolePermission
            {
                RoleId = this.RoleId,
                UrlEndpointId = this.UrlEndpointId,
                MethodId = this.MethodId,
                MappedUrl = this.MappedUrl
            };
        }
    }
}
