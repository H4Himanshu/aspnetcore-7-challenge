using Challenge.Contracts;
using Challenge.Data;
using Challenge.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Implementations
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly AppDbContext _context;

        /// <summary>Initializes a new instance of the <see cref="T:Challenge.Implementations.CustomerRepository" /> class.</summary>
        /// <param name="context">The context.</param>
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>Get all as an asynchronous operation.</summary>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        /// <summary>Get by identifier as an asynchronous operation.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A Task&lt;Customer&gt; representing the asynchronous operation.</returns>
        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        /// <summary>Add as an asynchronous operation.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task&lt;Customer&gt; representing the asynchronous operation.</returns>
        public async Task<Customer> AddAsync(Customer entity)
        {
            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>Update as an asynchronous operation.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task&lt;Customer&gt; representing the asynchronous operation.</returns>
        public async Task<Customer> UpdateAsync(Guid id, Customer entity)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

            if (existingCustomer == null)
            {
                return null; // Customer not found
            }

            existingCustomer.FullName = entity.FullName;
            existingCustomer.DateOfBirth = entity.DateOfBirth;

            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        /// <summary>Delete as an asynchronous operation.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
