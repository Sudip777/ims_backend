using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;

namespace inventory_management_system.DTOs.Requests
{
    public class InventoryDto
    {


        [Required]
        public int ProductId { get; set; }


        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }

        public Inventory MappedInventory()
        {
            return new Inventory
            {
                ProductId = this.ProductId,
                WarehouseId = this.WarehouseId,
                Quantity = this.Quantity,
            };
        }
    }
}
