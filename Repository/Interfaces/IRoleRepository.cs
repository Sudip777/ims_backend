using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role);
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync();

    }
}
