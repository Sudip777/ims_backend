using inventory_management_system.Data;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class UserMenuRepository:IUserMenuRepository
    {
        private readonly ApplicationDBContext _context;

        public UserMenuRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetMappedUrlsByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.MappedUrl != null)
                .Select(rp => rp.MappedUrl)
                .Distinct()
                .ToListAsync();
        }
    }
}
