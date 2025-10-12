using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class RolePermissionResponse
    {
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        public int UrlEndpointId { get; set; }
        public int MethodId { get; set; }
        public string? MappedUrl { get; set; } // Nullable for optional alias/versioning
        public string RoleName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string MethodName { get; set; } = null!;


        public static RolePermissionResponse MappedRolePermissionResponse(RolePermission rolePermission)
        {
            return new RolePermissionResponse
            {
                RolePermissionId = rolePermission.RolePermissionId,
                RoleId = rolePermission.RoleId,
                UrlEndpointId = rolePermission.UrlEndpointId,
                MethodId = rolePermission.MethodId,
                MappedUrl = rolePermission.MappedUrl,
                RoleName = rolePermission.Role.RoleName,
                Url = rolePermission.UrlEndpoint.Url,
                MethodName = rolePermission.Method.MethodName
            };
        }
    }
}
