using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;

namespace inventory_management_system.DTOs.Requests
{
    public class WarehouseDto
    {
        [Required]
        public required string Name { get; set; }


        //Map DTO to Model
        public Warehouse MappedWarehouse()
        {
           
                return new Warehouse
                {
                    Name = this.Name,
                };
        }
    }
}
