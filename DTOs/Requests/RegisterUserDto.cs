using inventory_management_system.Models;
using Konscious.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace inventory_management_system.DTOs.Requests
{
    public class RegisterUserDto
    {
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;


        public required bool IsActive { get; set; } = true;

        [Required]
        public int RoleId { get; set; }

        // Map DTO to User entity
        public User MappedUser()
        {
            return new User
            {
                Username = this.Username!,
                PasswordHash = this.Password,
                FullName = this.FullName,
                Email = this.Email,
                RoleId = this.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

     
    }
}
