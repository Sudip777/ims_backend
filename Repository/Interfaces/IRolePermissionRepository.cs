using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<IEnumerable<RolePermission>> GetAllRolePermissionsAsync();
        Task<RolePermission> GetRolePermissionByIdAsync(int id);
        Task<RolePermission> CreateRolePermissionAsync(RolePermission permission);
        Task<RolePermission> UpdateRolePermissionAsync(RolePermissionDto permission, int id);
        Task<bool> DeleteRolePermissionAsync(int id);


    }
}
