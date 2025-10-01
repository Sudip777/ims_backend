using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using System.ComponentModel.DataAnnotations;

namespace inventory_management_system.DTOs.Responses
{
    public class CategoryResponse
    {
        public required string CategoryName { get; set; }

        public int? ParentCategoryId { get; set; }


        public static CategoryResponse MappedCategoryResponse(Category dto)
        {


            return new CategoryResponse
            {
                CategoryName = dto.CategoryName!,
                ParentCategoryId = dto.ParentCategoryId
            };

        }
    }
}
