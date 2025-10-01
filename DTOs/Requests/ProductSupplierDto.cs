using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Requests
{
    public class ProductSupplierDto
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public decimal CostPrice { get; set; }


        public ProductSupplier MappedProductSupplier()
        {
            return new ProductSupplier
            {
                ProductId = this.ProductId,
                SupplierId = this.SupplierId,
                CostPrice = this.CostPrice
            };

        }
    }
}
