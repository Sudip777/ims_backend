using FluentValidation;
using inventory_management_system.Validations;

namespace inventory_management_system.Extensions
{
    public static class ValidatorsExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
          
            services.AddValidatorsFromAssemblyContaining<ProductSupplierValidator>();
            return services;
           
        }
    }
}
