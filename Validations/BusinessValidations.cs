using inventory_management_system.DTOs.Requests;

namespace inventory_management_system.Validations
{
    public class BusinessValidations
    {
        public void  ValidateOrder(OrderDto orderDto)
        {
            if (orderDto == null)
                throw new ArgumentNullException(nameof(orderDto), "Order cannot be null.");

            if (orderDto.CustomerId <= 0)
                throw new ArgumentException("CustomerId must be a positive integer.");

            if (orderDto.OrderDetails == null || !orderDto.OrderDetails.Any())
                throw new ArgumentException("Order must have at least one order detail.");

            foreach (var detail in orderDto.OrderDetails)
            {
                if (detail.ProductId <= 0)
                    throw new ArgumentException("ProductId must be a positive integer.");

                if (detail.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.");

                if (detail.UnitPrice < 0)
                    throw new ArgumentException("UnitPrice cannot be negative.");
            }
        }
    }
}
