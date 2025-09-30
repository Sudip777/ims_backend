using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class SupplierResponse
    {
        public int SupplierId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public static SupplierResponse MappedSupplierResponse(Supplier suppplier)
        {
            return new SupplierResponse
            {
                SupplierId = suppplier.SupplierId,
                Name = suppplier.Name,
                Email = suppplier.Email,
                Phone = suppplier.Phone,
                Address = suppplier.Address,
                IsActive = suppplier.IsActive,
                CreatedAt = suppplier.CreatedAt,
                CreatedByUserId = suppplier.CreatedByUserId
            };
        }
    }
}
