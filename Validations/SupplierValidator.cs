using FluentValidation;
using inventory_management_system.DTOs.Requests;
using System.Text.RegularExpressions;

namespace inventory_management_system.Validations
{
    public class SupplierValidator:AbstractValidator<SupplierDto>
    {
        public SupplierValidator()
        {
            RuleFor(c => c.Name)
               .NotEmpty().WithMessage("Customer name is required.").WithErrorCode("ERR_SUPPLIERNAME_REQUIRED")
               .MaximumLength(60).WithMessage("Customer name cannot exceed 100 characters.").WithErrorCode("ERR_SUPPLIERNAME_INVALID");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required.").WithErrorCode("ERR_EMAIL_REQUIRED")
                .EmailAddress().WithMessage("A valid email is required.").WithErrorCode("ERR_EMAIL_INVALID")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.").WithErrorCode("ERR_EMAIL_INVALID");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("Phone number is required.").WithErrorCode("ERR_PHONE_REQUIRED")
                .Must(phone =>
                {
                    if (string.IsNullOrWhiteSpace(phone))
                        return false;

                 //10 digits
                    if (Regex.IsMatch(phone, @"^\d{10}$"))
                        return true;

                    // Rule 2: +<countrycode>-XXXXXXXXXX (country code = 1–3 digits, phone = 10 digits)
                    if (Regex.IsMatch(phone, @"^\+\d{1,3}-\d{10}$"))
                        return true;

                    return false;
                })
                .WithMessage("Phone must be either 10 digits, or +<countrycode>- followed by 10 digits").WithErrorCode("ERR_PHONE_INVALID")
                .MaximumLength(15).WithMessage("Phone number cannot exceed 15 characters.").WithErrorCode("ERR_PHONE_INVALID");


            RuleFor(c => c.Address)
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters.").WithErrorCode("ERR_ADDRESS_INVALID");
        }
    }
}
