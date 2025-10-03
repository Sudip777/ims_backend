using FluentValidation;
using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Validations
{
    public class RoleValidator:AbstractValidator<RoleDto>
    {

        public RoleValidator()
        {
            RuleFor(r=>r.RoleName)
                .NotEmpty().WithMessage("Role name is required.").WithErrorCode("ERR_ROLENAME_REQUIRED")
                .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.").WithErrorCode("ERR_ROLENAME_INVALID");
        }       
    }
}
