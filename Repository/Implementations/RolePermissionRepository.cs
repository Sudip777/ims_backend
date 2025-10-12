using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDBContext _context;
        
        public RolePermissionRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RolePermission>> GetAllRolePermissionsAsync()
        {
           return await _context.RolePermissions.ToListAsync();
        }

        public async Task<RolePermission> GetRolePermissionByIdAsync(int id)
        {
            return await _context.RolePermissions.FindAsync(id);
        }
        public async Task<RolePermission> CreateRolePermissionAsync(RolePermission dto)
        {
            await _context.RolePermissions.AddAsync(dto);
            await _context.SaveChangesAsync();

            // Load navigation properties
            var res = await _context.RolePermissions
                .Include(o => o.Role)
                .Include(o => o.Method)
                .Include(o => o.UrlEndpoint)
                .FirstOrDefaultAsync(o => o.RolePermissionId == dto.RolePermissionId);

            return dto;
        }

        public async Task<bool> DeleteRolePermissionAsync(int id)
        {
            var res = await GetRolePermissionByIdAsync(id);  
            if (res == null) throw new KeyNotFoundException($"Role Permission with {id} Not Found");

            _context.RolePermissions.Remove(res);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<RolePermission> UpdateRolePermissionAsync(RolePermissionDto permission, int id)
        {
            var oldRolePermission = await _context.RolePermissions.Include((u)=>u.Role).Include((u=>u.UrlEndpoint)).Include((u)=>u.Method)
                .FirstOrDefaultAsync(p => p.RolePermissionId == id);

            if (oldRolePermission == null) throw new Exception("Role Permission Not Found");

            oldRolePermission.RoleId = permission.RoleId;
            oldRolePermission.MappedUrl = permission.MappedUrl;
            oldRolePermission.MethodId = permission.MethodId;
            oldRolePermission.UrlEndpointId = permission.UrlEndpointId;

            await _context.SaveChangesAsync();
            return oldRolePermission;
        }

       
    }
}
