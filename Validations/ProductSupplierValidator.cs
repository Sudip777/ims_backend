using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Validations
{
    public class ProductSupplierValidator : AbstractValidator<ProductSupplierDto>
    {

        public ProductSupplierValidator()
        {

            // CostPrice must be >= 0
            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(decimal.MaxValue)
                .WithMessage("CostPrice cannot be negative.");

            // ProductId must exist in DB
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than zero.")
                .WithMessage(x => $"Product with ID {x.ProductId} does not exist.");

            // SupplierId must exist in DB
            RuleFor(x => x.SupplierId)
                .GreaterThan(0)
                .WithMessage("SupplierId must be greater than zero.")
                .WithMessage(x => $"Supplier with ID {x.SupplierId} does not exist.");
        }

       
    }
}
