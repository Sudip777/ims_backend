using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Services.Interfaces
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<RolePermissionResponse>> GetAllRolePermissionsAsync();
        Task<RolePermissionResponse> GetRolePermissionByIdAsync(int id);
        Task<RolePermissionResponse> CreateRolePermissionAsync(RolePermissionDto dto);
        Task<RolePermissionResponse> UpdateRolePermissionAsync(RolePermissionDto dto, int id);
        Task<bool> DeleteRolePermissionAsync(int id);
    }
}
