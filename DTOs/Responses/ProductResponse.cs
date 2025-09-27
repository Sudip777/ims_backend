using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string SKU { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }

        public required int SupplierId { get; set; }
        public string? SupplierName { get; set; }   // only include needed info

        public required int CategoryId { get; set; }
        public string? CategoryName { get; set; }   // only include needed info

        public int ReorderLevel { get; set; } = 10;
        public int MinStock { get; set; } = 0;
        public int? MaxStock { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }


        public static ProductResponse MappeddProductResponse(Product product)
        {
            return new ProductResponse
            {

                ProductId = product.ProductId,
                Name = product.Name,
                SKU = product.SKU,
                UnitPrice = product.UnitPrice,
                CostPrice = product.CostPrice,
                SupplierId = product.SupplierId,
                SupplierName = product.Supplier?.Name,
                CategoryId = (int)product.CategoryId,
                CategoryName = product.Category?.CategoryName,
                ReorderLevel = product.ReorderLevel,
                MinStock = product.MinStock,
                MaxStock = product.MaxStock,
                IsActive = product.IsActive,
                CreatedAt = DateTime.UtcNow
            };
        }
    }

}
