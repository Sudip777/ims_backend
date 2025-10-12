using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public RolePermissionService(IRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }
        public async Task<IEnumerable<RolePermissionResponse>> GetAllRolePermissionsAsync()
        {
            var data = await _rolePermissionRepository.GetAllRolePermissionsAsync();
            if(data== null) throw new KeyNotFoundException($"Role Permissions Not Found.");

            return data
                .Select(res => new RolePermissionResponse
              {
                RolePermissionId = res.RolePermissionId,
                RoleId = res.RoleId,
                UrlEndpointId = res.UrlEndpointId,
                MethodId = res.MethodId,
                MappedUrl = res.MappedUrl,
                RoleName = res?.Role?.RoleName ?? string.Empty,
                Url = res?.UrlEndpoint?.Url ?? string.Empty,
                MethodName = res?.Method?.MethodName ?? string.Empty
                })
              .ToList();
        }

        public async Task<RolePermissionResponse> GetRolePermissionByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid Role Permission ID");

            var res = await _rolePermissionRepository.GetRolePermissionByIdAsync(id);

            if(res==null) throw new KeyNotFoundException($"Role Permission with ID {id} Not Found.");

            return new RolePermissionResponse
            {
                RolePermissionId = res.RolePermissionId,
                RoleId = res.RoleId,
                UrlEndpointId = res.UrlEndpointId,
                MethodId = res.MethodId,
                MappedUrl = res.MappedUrl,
                RoleName = res?.Role?.RoleName ?? string.Empty,
                Url = res?.UrlEndpoint?.Url ?? string.Empty,
                MethodName = res?.Method?.MethodName ?? string.Empty

            };
        }
        public async Task<RolePermissionResponse> CreateRolePermissionAsync(RolePermissionDto dto)
        {
            var response = dto.MappedRolePermission();
            var createdRole = await _rolePermissionRepository.CreateRolePermissionAsync(response);

              return new RolePermissionResponse
            {
                RolePermissionId = createdRole.RolePermissionId,
                RoleId = createdRole.RoleId,
                UrlEndpointId = createdRole.UrlEndpointId,
                MethodId = createdRole.MethodId,
                MappedUrl = createdRole.MappedUrl,
                RoleName = createdRole?.Role?.RoleName ?? string.Empty,
                Url = createdRole?.UrlEndpoint?.Url ?? string.Empty,
                MethodName = createdRole?.Method?.MethodName ?? string.Empty

            };

        }

        public async Task<RolePermissionResponse> UpdateRolePermissionAsync(RolePermissionDto dto, int id)
        {
            var data = await _rolePermissionRepository.GetRolePermissionByIdAsync(id);
            if (data == null) throw new KeyNotFoundException($"Role Permission with ID {id} Not Found.");
            
            var tempData = await _rolePermissionRepository.UpdateRolePermissionAsync(dto, id);

            return new RolePermissionResponse
            {
                RolePermissionId = tempData.RolePermissionId,
                RoleId = tempData.RoleId,
                UrlEndpointId = tempData.UrlEndpointId,
                MethodId = tempData.MethodId,
                MappedUrl = tempData.MappedUrl,
                RoleName = tempData?.Role?.RoleName ?? string.Empty,
                Url = tempData?.UrlEndpoint?.Url ?? string.Empty,
                MethodName = tempData?.Method?.MethodName ?? string.Empty

            };
        }

        public async Task<bool> DeleteRolePermissionAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid Role Permission ID");
            var data = GetRolePermissionByIdAsync(id);

            if (data == null) throw new KeyNotFoundException($"Role Permission with ID: {id} Not Found.");

            return await _rolePermissionRepository.DeleteRolePermissionAsync(id);
        }

      
    }
}
