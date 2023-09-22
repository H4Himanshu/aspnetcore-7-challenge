using Challenge.Contracts;
using Challenge.Data.Models;
using Challenge.Utilities;

namespace Challenge.Services
{
    /// <summary>Class CustomerService.
    /// Implements the <see cref="ICustomerService" /></summary>
    public class CustomerService: ICustomerService
    {
        /// <summary>The customer repository</summary>
        private readonly IRepository<Customer> _customerRepository;

        /// <summary>Initializes a new instance of the <see cref="T:Challenge.Services.CustomerService" /> class.</summary>
        /// <param name="customerRepository">The customer repository.</param>
        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>Gets all customers.</summary>
        /// <returns>IEnumerable&lt;Customer&gt;.</returns>
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetAllAsync();
        }

        /// <summary>Gets the customer details by identifier.</summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Customer.</returns>
        public async Task<Customer> GetCustomerDetailsById(Guid customerId)
        {
            return await _customerRepository.GetByIdAsync(customerId);
        }

        /// <summary>Adds the customer.</summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Customer.</returns>
        public async Task<Customer> AddCustomer(Customer customer)
        {
            return await _customerRepository.AddAsync(customer);
        }

        /// <summary>Updates the customer.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Customer.</returns>
        public async Task<Customer> UpdateCustomer(Guid id, Customer customer)
        {
            return await _customerRepository.UpdateAsync(id, customer);
        }

        /// <summary>Deletes the customer.</summary>
        /// <param name="id">The identifier.</param>
        public async Task DeleteCustomer(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
        }

        /// <summary>Generate profile image as an asynchronous operation.</summary>
        /// <param name="fullName">The full name.</param>
        /// <returns>A Task&lt;System.String&gt; representing the asynchronous operation.</returns>
        public async Task<string> GenerateProfileImageAsync(string fullName)
        {
            return await Helper.GenerateProfileImageAsync(fullName);
        }
    }
}
