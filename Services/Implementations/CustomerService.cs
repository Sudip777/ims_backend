using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;

namespace inventory_management_system.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserService _userService;
        private readonly ApplicationDBContext _context;

        public CustomerService(IUserService userService, ICustomerRepository customerRepository, ApplicationDBContext context)
        {
            _customerRepository = customerRepository;
            _userService = userService;
            _context = context;
        }
        public async Task<CustomerResponse> CreateCustomerAsync(CustomerDto customer)
        {
            var response = customer.MappedCustomer();
            response.CreatedByUserId = _userService.GetCurrentUserId();
            var createdResponse = await _customerRepository.CreateCustomerAsync(response);
            return new CustomerResponse
            {
                CustomerId = createdResponse.CustomerId,
                Name = createdResponse.Name,
                Email = createdResponse.Email,
                Phone = createdResponse.Phone,
                Address = createdResponse.Address,
                IsActive = createdResponse.IsActive,
                CreatedAt = createdResponse.CreatedAt,
                CreatedByUserId = createdResponse.CreatedByUserId,
            };
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            if (customers == null) throw new KeyNotFoundException("No Categories Found.");
            return  customers
                .Select(i => new CustomerResponse
                {
                    CustomerId = i.CustomerId,
                    Name = i.Name,
                    Email = i.Email,
                    Phone = i.Phone,
                    Address = i.Address,
                    IsActive = i.IsActive,
                    CreatedAt = i.CreatedAt,
                    CreatedByUserId = i.CreatedByUserId
                })
                .ToList();
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Customer ID");
            }

            var data = await _customerRepository.GetCustomerByIdAsync(id);

            if (data == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            return new CustomerResponse
            {
                CustomerId =data.CustomerId,
                Name =data.Name,
                Email =data.Email,
                Phone =data.Phone,
                Address =data.Address,
                IsActive =data.IsActive,
                CreatedAt =data.CreatedAt,
                CreatedByUserId = data.CreatedByUserId
            };
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(int id, CustomerDto customer)
        {
            if (id <= 0)
                throw new ArgumentException("Customer ID must be greater than zero.", nameof(id));

            var data = await _customerRepository.GetCustomerByIdAsync(id);
            if (data == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }
            var tempData = await _customerRepository.UpdateCustomerAsync(customer, id);

            return CustomerResponse.MappedCustomerResponse(tempData);
        }
        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var user = await _customerRepository.GetCustomerByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"Customer with {id} Not Found");
           
            await _customerRepository.DeleteCustomerAsync(id);
            return true;
        }
        
    }
}
