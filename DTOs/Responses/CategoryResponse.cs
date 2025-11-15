using inventory_management_system.Models;

namespace inventory_management_system.DTOs.Responses
{
    public class CategoryResponse
    {
        public required int CategoryId { get; set; }
        public required string CategoryName { get; set; }

        public int? ParentCategoryId { get; set; }
        public string? ParentCategory { get; set; }



        public static CategoryResponse MappedCategoryResponse(Category dto)
        {
            return new CategoryResponse
            {
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName!,
                ParentCategoryId = dto.ParentCategoryId,
                ParentCategory = dto.ParentCategory != null ? dto.ParentCategory.CategoryName: "N/A"

            };

        }
    }
}
