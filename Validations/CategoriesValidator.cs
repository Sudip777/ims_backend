using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class CategoriesValidator : AbstractValidator<CategoryDto>
    {
        public CategoriesValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(100)
                .WithMessage("Category name must be at most 100 characters.");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ParentCategoryId must be 0 or greater.");
        }
    }
}