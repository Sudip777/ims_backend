using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class ProductDropdownResponse
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public static ProductDropdownResponse ToDropdown(Product product)
        {
            return new ProductDropdownResponse
            {
                ProductId = product.ProductId,
                Name = product.Name
            };
        }

    }
}
