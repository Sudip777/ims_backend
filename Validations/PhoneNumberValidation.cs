using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace inventory_management_system.Validations
{
    public class PhoneNumberValidation:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Phone number is required");

            string phone = value.ToString() ?? "";

            //Exactly 10 digits
            if (Regex.IsMatch(phone, @"^\d{10}$"))
                return ValidationResult.Success;

            // Rule 2: +<countrycode>-XXXXXXXXXX (country code = 1–3 digits, phone = 10 digits)
            if (Regex.IsMatch(phone, @"^\+\d{1,3}-\d{10}$"))
                return ValidationResult.Success;

            return new ValidationResult("Phone must be either 10 digits, or +<countrycode>- followed by 10 digits");
        }
    }
}
