using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } 

        public static UserResponse FromUser(User user)
        {
            return new UserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}

