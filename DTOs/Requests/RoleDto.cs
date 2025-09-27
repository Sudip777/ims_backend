using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Requests
{
    public class RoleDto
    {
        public string RoleName { get; set; } = null!;
        public Role MappedRole()
        {
            return new Role
            {
                RoleName = this.RoleName
            };
        }
    }
}
