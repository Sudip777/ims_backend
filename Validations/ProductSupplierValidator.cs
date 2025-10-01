using System.Threading;
using System.Threading.Tasks;
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

            // CostPrice must be >= 0
            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("CostPrice cannot be negative.");

            // ProductId must exist in DB
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than zero.")
                .MustAsync(ProductExists)
                .WithMessage(x => $"Product with ID {x.ProductId} does not exist.");

            // SupplierId must exist in DB
            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .WithMessage("SupplierId must be greater than zero.")
                .MustAsync(SupplierExists)
                .WithMessage(x => $"Supplier with ID {x.SupplierId} does not exist.");
        }

        private async Task<bool> ProductExists(int productId, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == productId, cancellationToken);
        }

        private async Task<bool> SupplierExists(int supplierId, CancellationToken cancellationToken)
        {
            return await _context.Suppliers.AnyAsync(s => s.SupplierId == supplierId, cancellationToken);
        }
    }
}
