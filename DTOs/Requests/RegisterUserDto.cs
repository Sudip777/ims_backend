using inventory_management_system.Models;
using Konscious.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace inventory_management_system.DTOs.Requests
{
    public class RegisterUserDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public int RoleId { get; set; } // Use only RoleId, not Role string

        // Map DTO to User entity
        public User MappedUser()
        {
            return new User
            {
                Username = this.Username!,
                PasswordHash = this.Password,
                FullName = this.FullName,
                Email = this.Email,
                RoleId = this.RoleId, // Assign only the FK
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

     
    }
}
