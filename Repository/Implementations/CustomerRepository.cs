using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using static inventory_management_system.Constants.BusinessConstants;

namespace inventory_management_system.Repository.Implementations
{
    /// <summary>
    /// Implementation of the customer repository interface providing methods for customer data access operations.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _context;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to use for data access operations</param>
        public CustomerRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Creates a new customer in the database asynchronously.
        /// </summary>
        /// <param name="customer">The customer to create</param>
        /// <returns>The created customer with updated information</returns>
        /// <exception cref="InvalidOperationException">Thrown when the user creating the customer doesn't exist or when a customer with the same name, email, or phone already exists</exception>
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


        /// <summary>
        /// Retrieves all customers from the database asynchronously.
        /// </summary>
        /// <returns>A collection of all customers</returns>
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific customer by their ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve</param>
        /// <returns>The requested customer if found, otherwise null</returns>
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        /// <summary>
        /// Updates an existing customer in the database asynchronously.
        /// </summary>
        /// <param name="customer">The customer data to update</param>
        /// <param name="customerId">The ID of the customer to update</param>
        /// <returns>The updated customer</returns>
        /// <exception cref="Exception">Thrown when the customer is not found</exception>
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

        /// <summary>
        /// Deletes a customer from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the customer to delete</param>
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
