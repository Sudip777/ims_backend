using FluentValidation;
using inventory_management_system.Constants;
using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace inventory_management_system.Validations
{
    public class UrlEndpointValidator : AbstractValidator<UrlEndpointDto>
    {
        private static readonly HashSet<string> ValidBaseRoutes = GetAllBaseRoutes();
        private readonly ApplicationDBContext _context;

        public UrlEndpointValidator(ApplicationDBContext context)
        {
            _context = context;

            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("URL is required.")
                .WithErrorCode("ERR_URL_REQUIRED")
                .MustAsync(async (url, ct) =>
                    !await _context.UrlEndpoints.AnyAsync(u => u.Url == url, ct))
                .WithMessage("URL must be unique. This URL already exists.")
                .WithErrorCode("ERR_URL_DUPLICATE")


                .Must(url => url.StartsWith("api/", StringComparison.OrdinalIgnoreCase))
                .WithMessage("URL must start with 'api/'.")
                .WithErrorCode("ERR_URL_INVALID_PREFIX")

                .Must(BeAValidApiRoute)
                .WithMessage(url => $"The URL '{url}' is not a valid API route. Must match one of: {string.Join(", ", ValidBaseRoutes)}")
                .WithErrorCode("ERR_URL_INVALID_ROUTE");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .WithErrorCode("ERR_DESCRIPTION_REQUIRED")
                .MaximumLength(200)
                .WithErrorCode("ERR_DESCRIPTION_TOO_LONG");
        }

        private static bool BeAValidApiRoute(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            // Only match base routes exactly
            return ValidBaseRoutes.Contains(url.Trim().ToLowerInvariant());
        }

        private static HashSet<string> GetAllBaseRoutes()
        {
            var baseRoutes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var nestedTypes = typeof(ApiRoutes).GetNestedTypes(BindingFlags.Public);
            foreach (var type in nestedTypes)
            {
                var baseField = type.GetField("Base", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (baseField?.GetValue(null) is string baseValue)
                    baseRoutes.Add(baseValue.Trim().ToLowerInvariant());
            }

            return baseRoutes;
        }
    }
}
