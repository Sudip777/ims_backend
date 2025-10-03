using FluentValidation;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Data;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class OrderValidator : AbstractValidator<ProductDto>
    {
        private readonly ApplicationDBContext _context;

        public OrderValidator(ApplicationDBContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.").WithErrorCode("ERR_PRODUCTNAME_REQUIRED")
                .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.").WithErrorCode("ERR_NAME_INVALID");

            RuleFor(x => x.SKU)
                .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters.").WithErrorCode("ERR_SKU_INVALID")
                .MustAsync(async (sku, ct) =>
                {
                    if (string.IsNullOrEmpty(sku)) return true; // allow empty
                    return !await _context.Products.AnyAsync(p => p.SKU == sku, ct);
                })
                .WithMessage(sku => $"A product with SKU already exists.").WithErrorCode("ERR_SKU_INVALID");


            RuleFor(x => x)
                .Must(x => x.UnitPrice > x.CostPrice)
                .WithMessage("UnitPrice must be greater than CostPrice.").WithErrorCode("ERR_SUPPLIERID_INVALID");

            RuleFor(x => x.ReorderLevel)
                .InclusiveBetween(1, 10)
                .WithMessage("ReorderLevel must be between 1 (severe requirement) and 10 (low requirement).").WithErrorCode("ERR_REORDERLEVEL_INVALID");

            RuleFor(x => x.MinStock)
                .InclusiveBetween(0, 10000)
                .WithMessage("MinStock must be between 0 and 10,000.").WithErrorCode("ERR_MINSTOCK_INVALID");

            RuleFor(x => x.MaxStock)
                .GreaterThanOrEqualTo(x => x.MinStock)
                .WithMessage("MaxStock must be greater than or equal to MinStock.").WithErrorCode("ERR_MAXSTOCK_INVALID");

            RuleFor(x => x.IsActive)
                .Equal(true)
                .WithMessage("Product must be active when creating.").WithErrorCode("ERR_STATUS_INVALID");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).WithMessage("SupplierId is required.").WithErrorCode("ERR_SUPPLIERID_REQUIRED")
                .MustAsync(async (id, ct) => await _context.Suppliers.AnyAsync(s => s.SupplierId == id && s.IsActive, ct))
                .WithMessage(id => $"Supplier  does not exist or is inactive.").WithErrorCode("ERR_SUPPLIERID_INVALID");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId is required.").WithErrorCode("ERR_CATEGORYID_REQUIRED")
                .MustAsync(async (id, ct) => await _context.Categories.AnyAsync(c => c.CategoryId == id, ct))
                .WithMessage(id => $"Category  does not exist.").WithErrorCode("ERR_CATEGORYID_INVALID");
        }
    }
}
