using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using static inventory_management_system.Constants.BusinessConstants;

namespace inventory_management_system.Repository.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _context;
        public CustomerRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            // Check if the user creating the customer exists
            var userExists = await _context.Users.AnyAsync(u => u.UserId == customer.CreatedByUserId);
            if (!userExists)
                throw new InvalidOperationException($"Customer with ID {customer.CreatedByUserId} does not exist.");

            // Check Uniqueness
            var customerExists = await _context.Customers
                .AnyAsync(c => c.Name == customer.Name ||
                               c.Email == customer.Email ||
                               c.Phone == customer.Phone);

            if (customerExists)
                throw new InvalidOperationException("A customer with the same Name, Email Or Phone Already Exists.");

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }


        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> UpdateCustomerAsync(CustomerDto customer, int customerId)
        {
            var existingCustomer = await _context.Customers
                .Include(p => p.Orders)
                .FirstOrDefaultAsync(p => p.CustomerId == customerId);

            if (existingCustomer == null)
                throw new Exception("Customer not found");

            //DTO → entity
            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;
            existingCustomer.IsActive = customer.IsActive;

            await _context.SaveChangesAsync();

            return existingCustomer;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

    }
}
