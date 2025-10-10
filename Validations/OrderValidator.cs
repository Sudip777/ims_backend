using FluentValidation;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Data;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        private readonly ApplicationDBContext _context;

        public OrderValidator(ApplicationDBContext context)
        {
            _context = context;

            
            // Validate Order-level fields

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("CustomerId is required.")
                .WithErrorCode("ERR_CUSTOMERID_REQUIRED")
                .MustAsync(async (id, ct) =>
                    await _context.Customers.AnyAsync(c => c.CustomerId == id, ct))
                .WithMessage("Customer does not exist.")
                .WithErrorCode("ERR_CUSTOMERID_INVALID");

            RuleFor(x => x.StatusId)
                .GreaterThan(0)
                .WithMessage("StatusId is required.")
                .WithErrorCode("ERR_STATUSID_REQUIRED")
                .MustAsync(async (id, ct) =>
                    await _context.OrderStatuses.AnyAsync(s => s.StatusId == id, ct))
                .WithMessage("Invalid order status.")
                .WithErrorCode("ERR_STATUSID_INVALID");

            RuleFor(x => x.OrderDetails)
                .NotEmpty()
                .WithMessage("At least one order detail is required.")
                .WithErrorCode("ERR_ORDERDETAILS_REQUIRED");

            // Validate nested OrderDetails
         
            RuleForEach(x => x.OrderDetails).ChildRules(details =>
            {
                details.RuleFor(d => d.ProductId)
                    .GreaterThan(0)
                    .WithMessage("ProductId is required.")
                    .WithErrorCode("ERR_PRODUCTID_REQUIRED")
                    .MustAsync(async (id, ct) =>
                        await _context.Products.AnyAsync(p => p.ProductId == id && p.IsActive, ct))
                    .WithMessage("Product does not exist or is inactive.")
                    .WithErrorCode("ERR_PRODUCTID_INVALID");

                details.RuleFor(d => d.WarehouseId)
                    .GreaterThan(0)
                    .WithMessage("WarehouseId is required.")
                    .WithErrorCode("ERR_WAREHOUSEID_REQUIRED")
                    .MustAsync(async (id, ct) =>
                        await _context.Warehouses.AnyAsync(w => w.WarehouseId == id, ct))
                    .WithMessage("Warehouse does not exist.")
                    .WithErrorCode("ERR_WAREHOUSEID_INVALID");

                details.RuleFor(d => d.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.")
                    .WithErrorCode("ERR_QUANTITY_INVALID");

                details.RuleFor(d => d.UnitPrice)
                    .GreaterThan(0)
                    .WithMessage("Unit price must be greater than zero.")
                    .WithErrorCode("ERR_UNITPRICE_INVALID");
            });
        }
    }
}
