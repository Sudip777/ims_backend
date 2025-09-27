using System.ComponentModel.DataAnnotations;

namespace inventory_management_system.DTOs.Requests
{
    public class UpdateUserDto
    {
       
            [Required]
            public string Username { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            public string? Password { get; set; }   // <-- Optional for update
            public int RoleId { get; set; }
            public bool IsActive { get; set; }
        
    }
}
