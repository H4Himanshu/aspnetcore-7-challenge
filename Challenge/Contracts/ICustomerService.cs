using Challenge.Data.Models;

namespace Challenge.Contracts
{
    public interface ICustomerService
    {
        /// <summary>Gets all customers.</summary>
        /// <returns>Task&lt;IEnumerable&lt;Customer&gt;&gt;.</returns>
        Task<IEnumerable<Customer>> GetAllCustomers();

        /// <summary>Gets the customer details by identifier.</summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        Task<Customer> GetCustomerDetailsById(Guid customerId);

        /// <summary>Adds the customer.</summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        Task<Customer> AddCustomer(Customer customer);

        /// <summary>Updates the customer.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        Task<Customer> UpdateCustomer(Guid id, Customer customer);

        /// <summary>Deletes the customer.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteCustomer(Guid id);

        /// <summary>Generates the profile image asynchronous.</summary>
        /// <param name="fullName">The full name.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GenerateProfileImageAsync(string fullName);
    }
}
