using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;

namespace inventory_management_system.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer>  CreateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(CustomerDto customer, int customerId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task DeleteCustomerAsync(int id);


    }
}
