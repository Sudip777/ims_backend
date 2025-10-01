using FluentValidation;
using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Validations
{
  
        public class OrderValidator : AbstractValidator<OrderDto>
        {
            public OrderValidator()
            {
                RuleFor(o => o.CustomerId).GreaterThan(0);
                RuleFor(o => o.OrderDetails).NotEmpty();
                RuleForEach(o => o.OrderDetails).ChildRules(d =>
                {
                    d.RuleFor(x => x.ProductId).GreaterThan(0);
                    d.RuleFor(x => x.Quantity).GreaterThan(0);
                });
            }
        }

    }
