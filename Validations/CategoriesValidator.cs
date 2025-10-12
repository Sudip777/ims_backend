using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class CategoriesValidator : AbstractValidator<CategoryDto>
    {
        private readonly ApplicationDBContext _context;
        public CategoriesValidator(ApplicationDBContext context)
        {
            _context = context;


            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category name is required.").WithErrorCode("ERR_CATEGORYNAME_REQUIRED")
                .MaximumLength(60).WithErrorCode("ERR_NAME_REQUIRED");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ParentCategoryId must be 0 or greater.").WithErrorCode("ERR_PARENTCATEGORYID_REQUIRED");

            RuleFor(x => x.ParentCategoryId)
                .MustAsync(async (id, ct) =>
                {
                    // allow 0 or null (no parent category)
                    if (id == 0 || id == null)
                        return true;
                    return await _context.Categories.AnyAsync(c => c.CategoryId == id, ct);
                })
                .WithMessage(id => $"Parent Category does not exist.").WithErrorCode("ERR_PARENTCATEGORYID_REQUIRED");

        }
    }
}