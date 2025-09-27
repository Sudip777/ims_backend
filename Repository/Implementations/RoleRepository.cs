using inventory_management_system.Data;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDBContext _context;

        public RoleRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Role> CreateAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RoleResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                })
                .ToListAsync();
        }

    }
}
