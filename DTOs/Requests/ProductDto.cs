using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_management_system.DTOs.Requests
{
    public class ProductDto
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        public required string SKU { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal CostPrice { get; set; }

        public required int SupplierId { get; set; }

        public  int? CategoryId { get; set; }

        public int ReorderLevel { get; set; } = 0;

        public int MinStock { get; set; } = 0;

        public int? MaxStock { get; set; }

        public bool IsActive { get; set; } = true;

        // Map Dto to Product entity
        public Product MappedProduct()
            {
                return new Product
                {
                    Name = this.Name,
                    SKU = this.SKU,
                    SupplierId = this.SupplierId,
                    CategoryId= this.CategoryId,
                    UnitPrice = this.UnitPrice,
                    CostPrice = this.CostPrice,
                    ReorderLevel = this.ReorderLevel,
                    MinStock = this.MinStock,
                    MaxStock = this.MaxStock,
                    IsActive = this.IsActive,
                    CreatedAt = DateTime.UtcNow
                };
            }
        }
        
    }


