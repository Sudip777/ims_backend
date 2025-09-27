using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Services.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponse> RegisterRoleAsync(Role role);
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync();
    }
}
