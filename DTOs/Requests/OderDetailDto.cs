using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.DTOs.Requests
{
    public class OderDetailDto
    {
        public class OrderDetailDto
        {
            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive integer.")]
            public int ProductId { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]

            public int Quantity { get; set; }

            [Required]
            [Column(TypeName = "decimal(18,2)")]

            public decimal UnitPrice { get; set; }

            // Maps this DTO to the entity model
            public OrderDetail MappedOrderDetail()
            {
                return new OrderDetail
                {
                    ProductId = this.ProductId,
                    Quantity = this.Quantity,
                    UnitPrice = this.UnitPrice
                };
            }
        }
    }
}
