using FluentValidation;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Validations
{
    public class UserValidator:AbstractValidator<RegisterUserDto>
    {
        private readonly ApplicationDBContext _context;
        public UserValidator(ApplicationDBContext context)
        {
            _context = context;

            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Invalid user credentials.").WithErrorCode("ERR_USERNAME_REQUIRED")
                .MinimumLength(5).WithMessage("Invalid user credentials.").WithErrorCode("ERR_USERNAME_INVALID")
                .MaximumLength(50).WithMessage("Invalid user credentials.").WithErrorCode("ERR_USERNAME_INVALID");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Invalid user credentials.").WithErrorCode("ERR_EMAIL_REQUIRED")
                .EmailAddress().WithMessage("Invalid user credentials.").WithErrorCode("ERR_EMAIL_INVALID");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Invalid user credentials.").WithErrorCode("ERR_PASSWORD_REQUIRED")
                .MinimumLength(6).WithMessage("Invalid user credentials.").WithErrorCode("ERR_PASSWORD_INVALID")
                .Matches("[A-Z]").WithMessage("Invalid user credentials.").WithErrorCode("ERR_PASSWORD_INVALID")
                .Matches("[a-z]").WithMessage("Invalid user credentials.").WithErrorCode("ERR_PASSWORD_INVALID")
                .Matches("[0-9]").WithMessage("Invalid user credentials.").WithErrorCode("ERR_PASSWORD_INVALID")
                .Matches("[^a-zA-Z0-9]").WithMessage("Invalid user credentials.").WithErrorCode("ERR_PASSWORD_INVALID");

            RuleFor(user => user.FullName)
                .NotEmpty().WithMessage("Invalid user credentials.").WithErrorCode("ERR_FULLNAME_REQUIRED")
                .MaximumLength(100).WithMessage("Invalid user credentials.").WithErrorCode("ERR_FULLNAME_INVALID");

            RuleFor(d => d.RoleId)
                .GreaterThan(0).WithMessage("Invalid user credentials.").WithErrorCode("ERR_ROLEID_INVALID")
                .MustAsync(async (roleId, ct) => await _context.Roles.AnyAsync(p => p.RoleId == roleId, ct))
                .WithMessage("Invalid user credentials.").WithErrorCode("ERR_ROLEID_INVALID");

            RuleFor(d => d.IsActive)
                .Equal(true).WithMessage("User must be active when creating.").WithErrorCode("ERR_ISACTIVE_INVALID");
        }
    }
}