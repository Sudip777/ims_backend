using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class ProductSupplierResponse
    {
        public int ProductSupplierId { get; set; }
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public static ProductSupplier MappedProductSupplierResponse(ProductSupplier ps)
        {
            return new ProductSupplier
            {
                ProductSupplierId = ps.ProductSupplierId,
                ProductId = ps.ProductId,
                SupplierId = ps.SupplierId,
                CostPrice = ps.CostPrice,
                CreatedAt = ps.CreatedAt
            };

        }
    }
}
