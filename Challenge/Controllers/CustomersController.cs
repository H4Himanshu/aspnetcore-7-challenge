using Challenge.Contracts;
using Challenge.Data.DTO;
using Challenge.Data.Models;
using Challenge.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(
            ICustomerService customerService)
        {
            _customerService = customerService;
        }


        // POST /api/customers (create a customer)
        /// <summary>Creates the customer.</summary>
        /// <param name="customerDto">The customer dto.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost(Name = "CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Invalid customer data.");
            }

            // Call external API to generate profile image
            var svgData = await _customerService.GenerateProfileImageAsync(customerDto.FullName);

            if (svgData == null)
            {
                return Problem("Serivce Down. Please contact Administrator.");
            }

            // Create a new Customer entity
            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = customerDto.FullName,
                DateOfBirth = customerDto.DateOfBirth,
                ProfileImageSvg = svgData
            };

            var createdCustomer = await _customerService.AddCustomer(customer);

            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        // GET /api/customers/{id}
        /// <summary>Gets the customer by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet("{id}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customerService.GetCustomerDetailsById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET /api/customers/age/{age}
        /// <summary>Gets the customers by age.</summary>
        /// <param name="age">The age.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet("age/{age}", Name = "GetCustomersByAge")]
        public async Task<IActionResult> GetCustomersByAge(int age)
        {
            // Assuming DateOfBirth is the field to calculate age
            if (age < 0)
            {
                return BadRequest();
            }
            var today = DateTime.Today;
            var customers = await _customerService.GetAllCustomers();
            var result = customers.Where(c => today.Year - c.DateOfBirth.Year == age).ToList();

            return Ok(result);
        }

        // PATCH /api/customers/{id}
        /// <summary>Updates the customer.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="patchData">The patch data.</param>
        /// <returns>IActionResult.</returns>
        [HttpPatch("{id}", Name = "UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDto patchData)
        {
            if (patchData == null)
            {
                return BadRequest("Invalid patch data.");
            }

            var existingCustomer = await _customerService.GetCustomerDetailsById(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            var customerDto = new CustomerDto
            {
                FullName = patchData.FullName,
                DateOfBirth = patchData.DateOfBirth
            };

            // Update the customer entity
            existingCustomer.FullName = customerDto.FullName;
            existingCustomer.DateOfBirth = customerDto.DateOfBirth;

            await _customerService.UpdateCustomer(id, existingCustomer);

            return NoContent();
        }

        // GET /api/customers
        /// <summary>Gets the customers.</summary>
        /// <returns>IActionResult.</returns>
        [HttpGet(Name = "GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerService.GetAllCustomers();
            return Ok(customers);
        }
    }
}
