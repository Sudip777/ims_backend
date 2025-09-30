using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class CustomerResponse
    {
         
        public int CustomerId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public static CustomerResponse MappedCustomerResponse(Customer customer)
        {
            return new CustomerResponse
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                CreatedByUserId = customer.CreatedByUserId
            };
        }
    }
}
