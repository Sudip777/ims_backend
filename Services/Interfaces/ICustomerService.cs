using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;


namespace inventory_management_system.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponse> CreateCustomerAsync(CustomerDto customer);
        Task<CustomerResponse> GetCustomerByIdAsync(int id);
        Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync();
        Task<CustomerResponse> UpdateCustomerAsync(int id, CustomerDto customer);
        Task<bool> DeleteCustomerAsync(int id);
    }
}
