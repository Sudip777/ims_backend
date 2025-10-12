using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class RolePermissionValidator: AbstractValidator<RolePermissionDto>
    {
        private readonly ApplicationDBContext _context;
        public RolePermissionValidator(ApplicationDBContext context) {

            _context = context;

            RuleFor(x => x.RoleId)
                 .NotEmpty()
                 .WithMessage("Role Id is required.").WithErrorCode("ERR_ROLEID_REQUIRED")
                 .MustAsync(async (id, ct) => await _context.Roles.AnyAsync(p => p.RoleId == id, ct))
                 .WithMessage(id => $"Role ID  does not exist.").WithErrorCode("ERR_ROLEID_INVALID");

            RuleFor(x => x.UrlEndpointId)
                .NotEmpty()
                .WithMessage("Url Endpoint Id is required.").WithErrorCode("ERR_URLENDPOINTID_REQUIRED")
                .MustAsync(async (id, ct) => await _context.UrlEndpoints.AnyAsync(p => p.UrlEndpointId == id, ct))
                .WithMessage(id => $"Url Endpoint does not exist.").WithErrorCode("ERR_URLENDPOINTID_INVALID");

            RuleFor(x => x.MethodId)
               .NotEmpty()
               .WithMessage("Method Id is required.").WithErrorCode("ERR_METHODID_REQUIRED")
               .MustAsync(async (id, ct) => await _context.Methods.AnyAsync(p => p.MethodId == id, ct))
               .WithMessage(id => $"Role ID  does not exist.").WithErrorCode("ERR_ROLEID_INVALID");


           

        }
    }
}