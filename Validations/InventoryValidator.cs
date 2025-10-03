using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class InventoryValidator: AbstractValidator<InventoryDto>


    {
        private readonly ApplicationDBContext _context;
        public InventoryValidator(ApplicationDBContext context)
            
        {
            _context = context;

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.").WithErrorCode("ERR_PRODUCTID_INVALID")
                .NotEmpty().WithMessage("ProductId is required.").WithErrorCode("ERR_PRODUCTID_REQUIRED");

            RuleFor(x => x.ProductId)
                .MustAsync(async (id, ct) => await _context.Products.AnyAsync(p => p.ProductId == id && p.IsActive, ct))
                .WithMessage(id => $"Product ID  does not exist.").WithErrorCode("ERR_PRODUCTID_INVALID");

            RuleFor(x => x.WarehouseId)
                .GreaterThan(0).WithMessage("WarehouseId must be greater than 0.").WithErrorCode("ERR_WAREHOUSEID_INVALID")
                .NotEmpty().WithMessage("Warehouse ID is required.").WithErrorCode("ERR_WAREHOUSEID_REQUIRED"); ;

            RuleFor(x => x.WarehouseId)
              .MustAsync(async (id, ct) => await _context.Warehouses.AnyAsync(p => p.WarehouseId == id, ct))
              .WithMessage(id => $"Warehouse ID does not exist.").WithErrorCode("ERR_WAREHOUSEID_INVALID");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be non-negative.").WithErrorCode("ERR_NAME_INVALID");
               

            
        }
    }
}
