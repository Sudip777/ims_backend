using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        private readonly ApplicationDBContext _context;
        public ProductValidator(ApplicationDBContext Context)
        {
            _context = Context;

            RuleFor(x => x)
                .Must(x => x.UnitPrice > x.CostPrice)
                .WithMessage("UnitPrice must be greater than CostPrice.").WithErrorCode("ERR_UNITPRICE_INVALID");

            // MinStock <= ReorderLevel <= MaxStock
            RuleFor(x => x)
                .Must(x => x.ReorderLevel >= x.MinStock && (!x.MaxStock.HasValue || x.ReorderLevel <= x.MaxStock.Value))
                .WithMessage("ReorderLevel must be between MinStock and MaxStock.").WithErrorCode("ERR_ORDERLEVEL_INVALID");

            // Unique
            RuleFor(x => x.SKU)
                .NotEmpty().When(x => !string.IsNullOrEmpty(x.SKU))
                .MustAsync(async (dto, sku, ct) =>
                {
                    if (string.IsNullOrEmpty(sku)) return true; // allow empty

                    return !await _context.Products
                        .AnyAsync(p => p.SKU == sku, ct);
                })

                .WithMessage(dto => $"A product with SKU already exists.").WithErrorCode("ERR_SKU_INVALID");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.").WithErrorCode("ERR_NAME_INVALID");


            RuleFor(p => p.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.").WithErrorCode("ERR_UNITPRICE_INVALID");

            RuleFor(p => p.CostPrice)
               .GreaterThan(0).WithMessage("Cost price must be greater than zero.").WithErrorCode("ERR_COSTPRICE_INVALID");


            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, ct) => await _context.Categories.AnyAsync(p => p.CategoryId == id, ct))
                .WithMessage(id => $"Category does not exist.").WithErrorCode("ERR_CATEGORY_INVALID");


            RuleFor(x => x.SupplierId)
               .MustAsync(async (id, ct) => await _context.Suppliers.AnyAsync(p => p.SupplierId == id, ct))
               .WithMessage(id => $"Supplier does not exist.").WithErrorCode("ERR_SUPPLIER_INVALID");


        }
    }
}
