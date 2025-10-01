using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<RoleResponse> RegisterRoleAsync(Role role)
        {
            var createdRole = await _roleRepository.CreateAsync(role);
            return RoleResponse.FromRole(createdRole);
        }

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return roles;
        }

       
    }
}
