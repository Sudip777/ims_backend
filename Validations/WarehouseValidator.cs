using FluentValidation;
using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Validations
{
    public class WarehouseValidator:AbstractValidator<WarehouseDto>
    {
        public WarehouseValidator()
        {
            RuleFor(w => w.Name)
                .NotEmpty().WithMessage("Warehouse name is required.").WithErrorCode("ERR_WAREHOUSENAME_REQUIRED")
                .MaximumLength(100).WithMessage("Warehouse name cannot exceed 100 characters.").WithErrorCode("ERR_WAREHOUSENAME_INVALID");
            
        }
    }
}
