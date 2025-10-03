using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class PurchaseOrderValidator: AbstractValidator<PurchaseOrderDto>
    {
        private readonly ApplicationDBContext _context;

        public PurchaseOrderValidator(ApplicationDBContext context)
        {
            _context = context;

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).WithMessage("SupplierId must be greater than zero.").WithErrorCode("ERR_SUPPLIERID_INVALID")
                .MustAsync(async (id, ct) => await _context.Suppliers.AnyAsync(s => s.SupplierId == id && s.IsActive, ct))
                .WithMessage(id => $"Supplier does not exist or is inactive.").WithErrorCode("ERR_SUPPLIERID_INVALID");

            RuleFor(x => x.StatusId)
                .GreaterThan(0).WithMessage("StatusId must be greater than zero.").WithErrorCode("ERR_STATUSID_INVALID");
              

            RuleForEach(x => x.PurchaseOrderDetails).ChildRules(detail =>
            {
                detail.RuleFor(d => d.ProductId)
                    .GreaterThan(0).WithMessage("ProductId must be greater than zero.").WithErrorCode("ERR_PRODUCTID_INVALID")
                    .MustAsync(async (productId, ct) => await _context.Products.AnyAsync(p => p.ProductId == productId && p.IsActive, ct))
                    .WithMessage(productId => $"Product  ID  does not exist or is inactive.").WithErrorCode("ERR_PRODUCTID_INVALID");

                detail.RuleFor(d => d.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero.").WithErrorCode("ERR_QUANTITY_INVALID")
                    .LessThanOrEqualTo(10000).WithMessage("Quantity cannot exceed 10,000.").WithErrorCode("ERR_QUANTITY_INVALID");

                // UnitPrice > 0 and matches Product.UnitPrice
                detail.RuleFor(d => d)
                    .MustAsync(async (d, ct) =>
                    {
                        if (d.UnitPrice <= 0) return false;

                        var product = await _context.Products
                            .Where(p => p.ProductId == d.ProductId)
                            .Select(p => new { p.UnitPrice })
                            .FirstOrDefaultAsync(ct);

                        if (product == null) return false;
                        return d.UnitPrice == product.UnitPrice;
                    })
                    .WithMessage(d => $"UnitPrice for ProductId  must match the product's UnitPrice in DB and cannot be zero.").WithErrorCode("ERR_UNITPRICE_INVALID");
            });
        }
    }
}
