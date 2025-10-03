using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static inventory_management_system.DTOs.Requests.OderDetailDto;

namespace inventory_management_system.DTOs.Requests
{
    public class OrderDto
    {
    
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int StatusId { get; set; }
     
       
       
        public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();



        public Order MappedOrder()
        {
            return new Order
            {
                CustomerId = this.CustomerId,
                StatusId = this.StatusId,
                OrderDetails = this.OrderDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    WarehouseId =od.WarehouseId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            };
        }
    }
}
