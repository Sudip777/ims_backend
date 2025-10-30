using inventory_management_system.Constants;
using inventory_management_system.Data;
using inventory_management_system.Enums;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class UserMenuRepository : IUserMenuRepository
    {
        private readonly ApplicationDBContext _context;

        public UserMenuRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetMappedUrlsByRoleIdAsync(int roleId)
        {
            var query = _context.RolePermissions.AsQueryable();

            if (roleId != (int)UserRole.SUPER_ADMIN && roleId != (int)UserRole.ADMIN)
                query = query.Where(rp => rp.RoleId == roleId);

            return await query
                .Where(rp => rp.MappedUrl != null)
                .Select(rp => rp.MappedUrl)
                .Distinct()
                .ToListAsync();
        }
    }
}
