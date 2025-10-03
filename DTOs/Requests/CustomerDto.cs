using inventory_management_system.Models;
using inventory_management_system.Validations;
using System.ComponentModel.DataAnnotations;

namespace inventory_management_system.DTOs.Requests
{
    public class CustomerDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(4, ErrorMessage = "Name must be at least 4 characters long")]
        [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters long")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]

        public required string Email { get; set; }
        [Required]
        public required string Phone { get; set; }
        [MinLength(4, ErrorMessage = "Address must be at least 4 characters long")]
        [MaxLength(50, ErrorMessage = "Address cannot exceed 25 characters long")]
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public Customer MappedCustomer()
        {
            return new Customer
            {
                Name = this.Name,
                Email = this.Email,
                Phone = this.Phone,
                Address = this.Address,
                IsActive = this.IsActive
            };
        }
    }

}