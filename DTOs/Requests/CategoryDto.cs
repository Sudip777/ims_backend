using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;

namespace inventory_management_system.DTOs.Requests
{
    public class CategoryDto
    {
        [Required]
        public required string CategoryName { get; set; }

        public int? ParentCategoryId { get; set; }


        public Category MappedCategory()
        {


            return new Category
            {
                CategoryName = this.CategoryName,
                ParentCategoryId = this.ParentCategoryId
            };

        }
    }


}