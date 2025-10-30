using FluentValidation;
using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Validations
{
    public class GetAllOrdesrRequestValidator:AbstractValidator<GetAllOrdersRequest>
    {
        public GetAllOrdesrRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("PageSize cannot exceed 20 .");



            RuleFor(x => x.SortDirection)
                .Must(x => string.IsNullOrEmpty(x) || x.ToLower() == "asc" || x.ToLower() == "desc")
                .WithMessage("SortOrder must be 'asc' or 'desc'.");
        }
    }
}
