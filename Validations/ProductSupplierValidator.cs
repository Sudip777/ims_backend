using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class ProductSupplierValidator : AbstractValidator<ProductSupplierDto>
    {
        private readonly ApplicationDBContext _context;
        public ProductSupplierValidator(ApplicationDBContext context)
        {
            _context = context;


            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("CostPrice cannot be negative.").WithErrorCode("ERR_COSTPRICE_INVALID");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than zero.").WithErrorCode("ERR_PRODUCTID_INVALID")
                .MustAsync(async (productId, ct) =>
                    await _context.Products.AnyAsync(p => p.ProductId == productId, ct))
                .WithMessage(productId => $"Product with ID {productId} does not exist.").WithErrorCode("ERR_PRODUCTID_INVALID");

            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .WithMessage("SupplierId must be greater than zero.").WithErrorCode("ERR_SUPPLIERID_INVALID")
                .MustAsync(async (supplierId, ct) =>
                    await _context.Suppliers.AnyAsync(s => s.SupplierId == supplierId, ct))
                .WithMessage(supplierId => $"Supplier with ID {supplierId} does not exist.").WithErrorCode("ERR_SUPPLIERID_INVALID");

            RuleFor(x => x)
                .MustAsync(async (dto, ct) =>
                {
                    var product = await _context.Products
                        .Where(p => p.ProductId == dto.ProductId)
                        .Select(p => new { p.CostPrice })
                        .FirstOrDefaultAsync(ct);

                    if (product == null) return true; 
                    return product.CostPrice == dto.CostPrice;
                })
                .WithMessage(dto => $"CostPrice does not match the Product's cost price in Database.").WithErrorCode("ERR_COSTPRICE_INVALID");
        }
    }

       
    }
