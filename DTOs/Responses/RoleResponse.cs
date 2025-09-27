using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class RoleResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public static RoleResponse FromRole(Role role)
        {
            return new RoleResponse
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }
    }
}
